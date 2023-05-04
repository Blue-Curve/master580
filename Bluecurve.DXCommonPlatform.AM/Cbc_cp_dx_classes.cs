using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public class Cbc_cp_dx_classes
    {
        Ibc_cp_dx_classes _view;
        bc_om_class_attributes _pca;
        public bool load_data(Ibc_cp_dx_classes view, bool read_only)
        {
            try
            {
                _view = view;
                _view.Eload_schema_classes += Eload_schema_classes;
                _view.Eload_class_attributes += Eload_class_attributes;
                _view.Eupdate_class_attribute += Eupdate_class_attribute;
                _view.Eloadallattributes += Eload_all_attributes;
                bc_om_schemas sch = new bc_om_schemas();
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    sch.db_read();
                else
                {
                    sch.tmode = bc_cs_soap_base_class.tREAD;
                    object osch;
                    osch = (object)sch;

                    if (sch.transmit_to_server_and_receive(ref osch, true) == false)
                        return false;
                    sch = (bc_om_schemas)osch;
                }


                return _view.load_view(sch, read_only);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_classes", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }

        void Eload_schema_classes(object sender, Eload_schema_classesArgs e)
        {
            try
            {
                bc_om_dx_cp_entity_classes classes = new bc_om_dx_cp_entity_classes();
                classes.schema_id = e.schema_id;

                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    classes.db_read();
                else
                {
                    classes.tmode = bc_cs_soap_base_class.tREAD;
                    object oclasses;
                    oclasses = (object)classes;

                    if (classes.transmit_to_server_and_receive(ref oclasses, true) == false)
                        return;
                    classes = (bc_om_dx_cp_entity_classes)oclasses;
                }
                _view.load_class_links(classes);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_classes", "load_class_links_for_schema", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        void Eload_class_attributes(object sender, Eload_class_attributesArgs e)
        {
            load_class_attributes(e.class_id);
        }
        void load_class_attributes(long class_id)
        {
            try
            {
                bc_om_class_attributes ca = new bc_om_class_attributes();
              
                if (class_id > 0)
                {
                  ca.class_id = class_id;
                  ca.no_lists = true;
                  if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    ca.db_read();
                  else
                  {
                    ca.tmode = bc_cs_soap_base_class.tREAD;
                    object oca;
                    oca = (object)ca;

                    if (ca.transmit_to_server_and_receive(ref oca, true) == false)
                        return;
                    ca = (bc_om_class_attributes)oca;
                  }
                }
                else if (class_id==0)
                {
                    bc_om_user_attributes ua = new bc_om_user_attributes();
                    ua.user_id = 0;
                    ua.no_lists = true;
                    if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                        ua.db_read();
                    else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                    {
                        ua.tmode = bc_cs_soap_base_class.tREAD;
                        object opa = (object)ua;
                        if (ua.transmit_to_server_and_receive(ref opa, true) == false)
                            return;
                        ua = (bc_om_user_attributes)opa;
                        ca.attributes = ua.attributes;
                    }

                }
                else if (class_id == -6)
                {
                    bc_om_pub_type_attributes pa = new bc_om_pub_type_attributes();
                    pa.pt_id = 0;
                    pa.no_lists = true;
                   
                    if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                        pa.db_read();
                    else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                    {
                        pa.tmode = bc_cs_soap_base_class.tREAD;
                        object opa = (object)pa;
                        if (pa.transmit_to_server_and_receive(ref opa, true) == false)
                            return;

                        pa = (bc_om_pub_type_attributes)opa;
                        ca.attributes = pa.attributes;

                    }
                }
                else if (class_id==0)
                {
                    
                }
                _pca = ca;
                _view.load_class_attributes(ca);

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_classes", "load_class_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        void Eupdate_class_attribute(object sender, Eupdate_class_attributeArgs e)
        {
            try
            {
                   
                    if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                        e.ca.db_write();
                    else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                    {
                        e.ca.tmode = bc_cs_soap_base_class.tWRITE;
                        object opa = (object)e.ca;
                        if (e.ca.transmit_to_server_and_receive(ref opa, true) == false)
                            return;
                    }
                    load_class_attributes(e.ca.class_id);
              
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_classes", "update_class_attribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void Edelete_class_attribute (object sender, Eupdate_class_attributeArgs e)
       {
           bc_om_class_attribute ca = new bc_om_class_attribute();
           ca.class_id = e.ca.class_id;
           ca.attribute_id = e.ca.attribute_id;
           ca.write_mode = ca.write_mode = bc_om_class_attribute.DELETE_ATTRIBUTE;
           if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
               ca.db_write();
           else
           {
               ca.tmode = bc_cs_soap_base_class.tWRITE;
               object oca = (object)ca;
               if (ca.transmit_to_server_and_receive(ref oca, true) == false)
                   return;
           }
           load_class_attributes(e.ca.class_id);
      }

        void Eload_all_attributes(object sender, Eload_class_attributesArgs e)
        {
            bc_om_all_attributes allattributes = new bc_om_all_attributes();
             if (e.class_id==0)
                 allattributes.user_attrubutes=true;
             else if (e.class_id == -6)
                 allattributes.pub_type_attributes  = true;

            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                allattributes.db_read();
            else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
            {
                allattributes.tmode = bc_cs_soap_base_class.tREAD;
                object oallattributes = (object)allattributes;
                if (allattributes.transmit_to_server_and_receive(ref oallattributes, true) == false)
                    return;

                allattributes = (bc_om_all_attributes)oallattributes;
            }

            bc_dx_cp_edit aed = new bc_dx_cp_edit();
            Cbc_dx_cp_edit ced = new Cbc_dx_cp_edit();
            List<bc_om_entity> atts = new List<bc_om_entity>();
            bc_om_entity att;
            int i,j;
            bool found=false;
            for (i = 0; i < allattributes.attributes.Count; i++ )
            {
                found = false;
                for (j = 0; j < + _pca.attributes.Count; j++ )
                {
                    if (_pca.attributes[j].attribute_id==allattributes.attributes[i].attribute_id )
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    if ((allattributes.attributes[i].repeats < 2) ||  (allattributes.attributes[i].repeats == 2 && e.class_id >0))
                    {
                      att = new bc_om_entity();
                      att.id = allattributes.attributes[i].attribute_id;
                      att.name = allattributes.attributes[i].name;
                      atts.Add(att);
                    }
                }
            }
            
            if (ced.load_data(aed, "Assign attribute to " + e.class_name, "", "Please select an attribute", true, atts, false) == true)
            {
                aed.ShowDialog();
                if (ced.bsave== true)
                {
                    bc_om_class_attribute ca = new bc_om_class_attribute();
                    ca.class_id = e.class_id;
                    
                    ca.attribute_id = ced.id;
                    ca.order = _pca.attributes.Count + 1;
                    ca.permission = 3;
                    if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                        ca.db_write();
                    else
                    {
                        ca.tmode = bc_cs_soap_base_class.tWRITE;
                        object oca = (object)allattributes;
                        if (ca.transmit_to_server_and_receive(ref oca, true) == false)
                            return;
                    }
                    load_class_attributes(e.class_id);
                }
            }

        }


    }
    public interface Ibc_cp_dx_classes
    {
        bool load_view(bc_om_schemas schemas, bool read_only);
        void load_class_links(bc_om_dx_cp_entity_classes classes);
        void load_class_attributes(bc_om_class_attributes ca);
        event EventHandler<Eload_schema_classesArgs> Eload_schema_classes;
        event EventHandler<Eload_class_attributesArgs> Eload_class_attributes;
        event EventHandler<Eupdate_class_attributeArgs> Eupdate_class_attribute;
        event EventHandler<Eload_class_attributesArgs> Eloadallattributes;
      
    }
   

    public class Eload_schema_classesArgs : EventArgs
    {
        public long  schema_id { get; set; }
    }
    public class Eload_class_attributesArgs : EventArgs
    {
        public long  class_id { get; set; }
        public string class_name { get; set; }
    }
    public class Eupdate_class_attributeArgs : EventArgs
    {
        public  bc_om_dx_cp_entity_class_attribute  ca { get; set; }
    }
}
