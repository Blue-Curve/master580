using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    class Cbc_dx_cp_attributes
    {
        Ibc_dx_cp_attributes _view;

        public bool load_data(Ibc_dx_cp_attributes view, bool read_only)
        {

            _view = view;
            _view.Eload_all_attributes += load_all_attributes;
            _view.Eupdate_attributes += update_attributes;
            bc_om_all_attributes allattributes = new bc_om_all_attributes();

            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                allattributes.db_read();
            else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
            {
                allattributes.tmode = bc_cs_soap_base_class.tREAD;
                object oallattributes = (object)allattributes;
                if (allattributes.transmit_to_server_and_receive(ref oallattributes, true) == false)
                    return false;

                allattributes = (bc_om_all_attributes)oallattributes;
            }

            return _view.load_view(allattributes,read_only);
          
        }
        bool load_attributes()
        {

            bc_om_all_attributes allattributes = new bc_om_all_attributes();

            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                allattributes.db_read();
            else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
            {
                allattributes.tmode = bc_cs_soap_base_class.tREAD;
                object oallattributes = (object)allattributes;
                if (allattributes.transmit_to_server_and_receive(ref oallattributes, true) == false)
                    return false;

                allattributes = (bc_om_all_attributes)oallattributes;
            }
            return _view.load_attributes(allattributes);
        }
        void load_all_attributes(object sender, EventArgs e)
        {
            load_attributes();
        }
        void update_attributes(object sender,Eload_update_attributesArgs args)
        {
           try
           {
               if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                   args.allattributes.db_write();
               else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
               {
                   args.allattributes.tmode = bc_cs_soap_base_class.tWRITE;
                   object oallattributes = (object)args.allattributes;
                   if (args.allattributes.transmit_to_server_and_receive(ref oallattributes, true) == false)
                       return;
                   // if delete see if delete failed
                   if (args.allattributes.write_mode == 1)
                   {
                       args.allattributes = (bc_om_all_attributes)oallattributes;
                       if (args.allattributes.delete_error_text!="")
                       {
                           bc_cs_message omsg = new bc_cs_message("Blue Curve", args.allattributes.delete_error_text, bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                       }
                   }
               }
           }

           catch (Exception ex)
           {
               bc_cs_security.certificate certificate = new bc_cs_security.certificate();
               bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "update_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           }
        }
    }
    interface Ibc_dx_cp_attributes
    {
        bool load_view(bc_om_all_attributes allattributes, bool read_only);
        bool load_attributes(bc_om_all_attributes allattributes);
        event EventHandler<EventArgs> Eload_all_attributes;
        event EventHandler<Eload_update_attributesArgs> Eupdate_attributes;
       
    }

    public class Eload_update_attributesArgs : EventArgs
    {
        public bc_om_all_attributes allattributes { get; set; }
    }
}
