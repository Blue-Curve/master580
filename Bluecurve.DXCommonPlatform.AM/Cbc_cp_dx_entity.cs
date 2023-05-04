using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
   

    public class Cbc_cp_dx_entity
    {
        Ibc_cp_dx_entity _view;
        public static bc_om_schemas schemas = new bc_om_schemas();
        public Boolean load_data(Ibc_cp_dx_entity view,bool view_only)
        {
            try
            {
                
                _view = view;

                _view.Eloadentity += load_entity;
                _view.Eloadclass += load_class_attributes;
                _view.Eloadentitylinks += get_entity_class_links;
                _view.Eassign += load_assign_screen;
                _view.Eupdateentity += update_entity;
              
                
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    schemas.db_read();
                else
                {
                    schemas.tmode = bc_cs_soap_base_class.tREAD;
                    object oschemas;
                    oschemas = (object)schemas;

                    if (schemas.transmit_to_server_and_receive(ref oschemas, true) == false)
                        return false;
                    schemas = (bc_om_schemas)oschemas;
                }
               
                return _view.load_view(schemas,view_only);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }

        void load_assign_screen(object sender, EassignArgs e)
        {
            try
            { 

            bc_dx_cp_assign fa = new  bc_dx_cp_assign();
            Cbc_dx_cp_assign ca = new  Cbc_dx_cp_assign();
            
            if (ca.load_data(fa, e) == true)
            {
                fa.ShowDialog();
                _view.load_links();
            }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_assign_screen", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return;
            }
        }


       void load_class_attributes(object sender,  EloadclassArgs e)
       {
            try
            {
              


                bc_om_class_attributes ca = new bc_om_class_attributes();
                ca.class_id = e.sclass.class_id;
                if (bc_cs_central_settings.selected_conn_method== bc_cs_central_settings.ADO)
                    ca.db_read();
                else
                {
                 
                    ca.tmode = bc_cs_soap_base_class.tREAD;
                    object oca=(object)ca;
                    if (ca.transmit_to_server_and_receive(ref oca, true)== false)
                        return;
                    ca = (bc_om_class_attributes)oca;
                  
                }
                _view.load_class_attributes(ca.attributes, e.sclass.class_name);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_class_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
       }
       
        void load_entity(object sender, EloadentityArgs e)
        {
            try
            {
                e.sentity.attributes_only = true;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    e.sentity.db_read();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    e.sentity.tmode = bc_cs_soap_base_class.tREAD;
                    object oentity = (object)e.sentity;
                    if (e.sentity.transmit_to_server_and_receive(ref oentity, true) == false)
                        return;

                    e.sentity = (bc_om_entity)oentity;
                }
                _view.load_entity(e.sentity);

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_entity", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void get_entity_class_links(object sender, EloadentitylinksArgs e)
        {
            try
            {
                
                bc_om_cp_entity_links el = new bc_om_cp_entity_links();
                el.entity_id = e.entity_id;
                el.schema_id = e.schema_id;
                el.pref_type_id = e.pref_type_id;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    el.db_read();
                else
                {
                    el.tmode = bc_cs_soap_base_class.tREAD;
                    object oel;
                    oel = (object)el;

                    if (el.transmit_to_server_and_receive(ref oel, true) == false)
                        return;
                    el = (bc_om_cp_entity_links)oel;

                }

                _view.load_entity_links(el);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "get_entity_class_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void update_entity(object sender, EupdateentityArgs e)
        {
            try
            {
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                e.entity.db_write();
            else
            {
                e.entity.tmode = bc_cs_soap_base_class.tWRITE;
                object oentity;
                oentity = (object)e.entity;

                if (e.entity.transmit_to_server_and_receive(ref oentity, true) == false)
                    return;
                e.entity = (bc_om_entity)oentity;
            }

            if (e.entity.deactivate_fail_text != null)
            {
                if (e.entity.deactivate_fail_text != "")
                {
                    bc_cs_message omsg = new bc_cs_message("Blue Curve", e.entity.deactivate_fail_text, bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                }
            }

            if (e.reload == true)
            {
                _view.reload_entities(e.entity.name);
            }
             }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "update_entity", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
    }
    public interface Ibc_cp_dx_entity
    {
        Boolean load_view(bc_om_schemas schemas, bool view_only);
        void load_entity(bc_om_entity entity);
        void load_class_attributes(List<bc_om_attribute> attributes, string class_name);
        void load_entity_links(bc_om_cp_entity_links elinks);
        void load_links();
        void reload_entities(string sel_entity_name);
        event EventHandler<EloadentityArgs> Eloadentity;
        event EventHandler<EloadclassArgs> Eloadclass;
        event EventHandler<EloadentitylinksArgs> Eloadentitylinks;
        event EventHandler<EassignArgs> Eassign;
        event EventHandler<EupdateentityArgs> Eupdateentity;
    }
    public class EloadentitylinksArgs : EventArgs
    {
        public long entity_id { get; set; }
        public long schema_id { get; set; }
        public int pref_type_id { get; set; }
    }
    public class EupdateentityArgs : EventArgs
    {
        public bc_om_entity entity { get; set; }
        public bool reload { get; set; }
    }
}
