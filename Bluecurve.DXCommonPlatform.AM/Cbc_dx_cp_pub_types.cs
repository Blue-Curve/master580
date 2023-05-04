using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public class Cbc_cp_dx_pub_types
    {
        Ibc_cp_dx_pub_types _view;
        public bool load_data(Ibc_cp_dx_pub_types view, bool view_only)
        {
            try
            {
                _view = view;
                _view.Eloadpub_type += load_pubtype;
                _view.EAmmend_Pubtypes += ammend_pub_type;
                _view.Eupdateattribute += updateattribute;
                _view.Eloadentitylinks += load_links;
                _view.Eassign += load_assign_screen;
                _view.Eupdate_params += update_tax_params;

                bc_om_pub_type_attributes pa = new bc_om_pub_type_attributes();
                
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    pa.db_read();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    pa.tmode = bc_cs_soap_base_class.tREAD;
                    object opa = (object)pa;
                    if (pa.transmit_to_server_and_receive(ref opa, true) == false)
                        return false;
                    pa = (bc_om_pub_type_attributes)opa;
                }    
              
                

                return _view.load_view(view_only, pa.attributes);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_pub_types", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }
        void load_links(object sender, EloadpubtypelinksArgs e)
        {
            try
            { 
            bc_om_dx_pub_type_links pl = new bc_om_dx_pub_type_links();
            pl.pub_type_id = e.pub_type_id;
            pl.link_type = e.link_type_id;
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                pl.db_read();
            else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
            {
                pl.tmode = bc_cs_soap_base_class.tREAD;
                object opl = (object)pl;
                if ( pl.transmit_to_server_and_receive(ref opl, true) == false)
                    return;
                pl = (bc_om_dx_pub_type_links)opl;
            }
            _view.load_pub_type_links(pl);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_pub_types", "load_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
              
            }
        }

        void load_pubtype(object sender, EloadentityArgs e)
        {
            try
            {
                bc_om_dx_cp_pub_type pubtype = new bc_om_dx_cp_pub_type();

                pubtype.id = e.sentity.id;
                pubtype.config.attributes.pt_id = pubtype.id;
                bool inactive = e.sentity.inactive;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    pubtype.config.attributes.db_read();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    pubtype.config.attributes.tmode = bc_cs_soap_base_class.tREAD;
                    object opubtype = (object)pubtype.config.attributes;
                    if (pubtype.config.attributes.transmit_to_server_and_receive(ref opubtype, true) == false)
                        return;

                    pubtype.config.attributes = (bc_om_pub_type_attributes)opubtype;
                    
                }
                pubtype.name = e.sentity.name;
                pubtype.inactive = e.sentity.inactive;
                _view.load_pub_type(pubtype);

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_entity", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void ammend_pub_type(object sender, EpubtypeArgs e)
        {
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                e.spubtype.db_write();
            else
            {
                e.spubtype.tmode = bc_cs_soap_base_class.tWRITE;
                object opubtype = (object)e.spubtype;
                if (e.spubtype.transmit_to_server_and_receive(ref opubtype, true) == false)
                    return;

               
            }
            _view.reload_pub_types(e.spubtype.name);
        }
        void updateattribute(object sender, EpubtypeArgs e)
        {
            bc_om_pub_type_attribute pta = new bc_om_pub_type_attribute();
            pta.pub_type_id = e.spubtype.id;
            pta.att_val=e.attribute;
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                pta.db_write();
            else
            {
                pta.tmode = bc_cs_soap_base_class.tWRITE;
                object opubtype = (object)pta;
                if (pta.transmit_to_server_and_receive(ref opubtype, true) == false)
                    return;
            }
            
        }
        void load_assign_screen(object sender, EassignArgs e)
        {
            try
            {

                bc_dx_cp_assign fa = new bc_dx_cp_assign();
                Cbc_dx_cp_assign ca = new Cbc_dx_cp_assign();
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

        void update_tax_params(object sender, Eupdatetaxparams args)
        {
            try
            {
                
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                args.values.db_write();
            else
            {
                args.values.tmode = bc_cs_soap_base_class.tWRITE;
                object ovalues = (object)args.values;
                if (args.values.transmit_to_server_and_receive(ref ovalues, true) == false)
                    return;
            }
            }
           
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "update_tax_params", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return;
            }
        }
    }
    public interface Ibc_cp_dx_pub_types
    {
        bool load_view(bool view_only, List<bc_om_attribute> attributes);
        void load_pub_type(bc_om_dx_cp_pub_type pa);
        void reload_pub_types(string sel_pub_type);
        void load_pub_type_links(bc_om_dx_pub_type_links pl);
        void load_links();
        event EventHandler<EloadentityArgs> Eloadpub_type;
        event EventHandler<EpubtypeArgs> EAmmend_Pubtypes;
        event EventHandler<EpubtypeArgs> Eupdateattribute;
        event EventHandler<EloadpubtypelinksArgs> Eloadentitylinks;
        event EventHandler<EassignArgs> Eassign;
        event EventHandler<Eupdatetaxparams> Eupdate_params;
    }
}
