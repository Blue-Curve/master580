using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public class Cbc_dx_cp_roles
    {
        Ibc_dx_cp_roles _view;
        public bool load_data(Ibc_dx_cp_roles view, bool view_only)
        {
            try
            {
                _view = view;
                _view.Eloadrole += Eloadrole;
                _view.Esetstageroleaccess += Esetstageroleaccess;
                _view.Esetappaccess += Esetappaccess;
                _view.Enewupdaterole += Enewupdaterole;

               

                return _view.load_view(view_only);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_roles", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }
        void Esetappaccess(object sender, EsetccessArgs e)
        {
            try
            {
                
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    e.afr.db_write();
                else
                {
                    e.afr.tmode = bc_cs_soap_base_class.tWRITE;
                    object osr = (object)e.afr;
                    if (e.afr.transmit_to_server_and_receive(ref osr, true) == false)
                        return;
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_roles", "Esetappaccess", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void Eloadrole(object sender, EloadentityArgs e)
        {
            try
            {
                bc_om_stages stages = new bc_om_stages();
                stages.tmode = bc_cs_soap_base_class.tREAD;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    stages.db_read();
                else
                {
                    stages.tmode = bc_cs_soap_base_class.tREAD;
                    object ostages = (object)stages;
                    if (stages.transmit_to_server_and_receive(ref ostages, true) == false)
                        return;
                    stages = (bc_om_stages)ostages;
                }


                bc_om_dx_stage_role_access sr = new bc_om_dx_stage_role_access();
                sr.role_id = e.sentity.id;
                sr.tmode = bc_cs_soap_base_class.tREAD;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    sr.db_read();
                else
                {
                    sr.tmode = bc_cs_soap_base_class.tREAD;
                    object osr = (object)sr;
                    if (sr.transmit_to_server_and_receive(ref osr, true) == false)
                        return;
                    sr = (bc_om_dx_stage_role_access)osr;

                }


                bc_om_dx_cp_apps_for_role ra = new bc_om_dx_cp_apps_for_role();
                ra.role_id = e.sentity.id;
                ra.tmode = bc_cs_soap_base_class.tREAD;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    ra.db_read();
                else
                {
                    ra.tmode = bc_cs_soap_base_class.tREAD;
                    object ora = (object)ra;
                    if (ra.transmit_to_server_and_receive(ref ora, true) == false)
                        return;
                    ra = (bc_om_dx_cp_apps_for_role)ora;
                }
                _view.load_stage_role_and_access(sr, ra, stages);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_roles", "Eloadrole", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void Esetstageroleaccess(object sender, EstageaccessArgs args)
        {
            try
            {
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    args.sra.db_write();
                else
                {
                    args.sra.tmode = bc_cs_soap_base_class.tWRITE;
                    object osr = (object)args.sra;
                    if (args.sra.transmit_to_server_and_receive(ref osr, true) == false)
                        return;
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_roles", "Esetstageroleaccess", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void Enewupdaterole(object sender, EnewupdateroleArgs args)
        {
            try
            {
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    args.role.db_write();
                else
                {
                    args.role.tmode = bc_cs_soap_base_class.tWRITE;
                    object osr = (object)args.role;
                    if (args.role.transmit_to_server_and_receive(ref osr, true) == false)
                        return;
                    args.role = (bc_om_dx_role)osr;
                }
                if (args.role.write_error_text != "")
                {
                    bc_cs_message omsg = new bc_cs_message("Blue Curve", args.role.write_error_text, bc_cs_message.MESSAGE, false, false, "Yed", "No", true);
                }
                _view.reload_roles(args.role.name);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_roles", "Enewupdaterole", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
    }
    public interface Ibc_dx_cp_roles
    {
        bool load_view(bool view_only);
        void load_stage_role_and_access(bc_om_dx_stage_role_access sr, bc_om_dx_cp_apps_for_role ra, bc_om_stages stages);
        void reload_roles(string sel_role_name);
        event EventHandler<EloadentityArgs> Eloadrole;
        event EventHandler<EstageaccessArgs> Esetstageroleaccess;
        event EventHandler<EsetccessArgs> Esetappaccess;
        event EventHandler<EnewupdateroleArgs> Enewupdaterole;
    }

    public class EstageaccessArgs
    {
        public bc_om_dx_stage_role_access.bc_om_dx_stage_role sra;
    }
    public class EsetccessArgs
    {
        public bc_om_dx_cp_apps_for_role.bc_om_dx_cp_app_for_role afr;
    }
    public class EnewupdateroleArgs
    {
        public bc_om_dx_role role;
    }

}
