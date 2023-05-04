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
    public class Cbc_cp_dx_users
    {
        Ibc_cp_dx_users _view;
        public bool load_data( Ibc_cp_dx_users view, bool view_only)
        {
            try
            {
            _view = view;
            _view.Eloaduser += load_user;
            _view.Eloaduserlinks += get_user_class_links;
            _view.Eassign += load_assign_screen;
            _view.Eupdateuser += update_user;
            _view.Eupdateattribute += Eupdateattribute;
            return _view.load_view(view_only);
             }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_cp_dx_users", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }

        void load_user(object sender, EloadentityArgs e)
        {
            try
            {
                bc_om_user user = new bc_om_user();
                user.id = e.sentity.id;
                bool inactive = e.sentity.inactive;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    user.db_read();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    user.tmode = bc_cs_soap_base_class.tREAD;
                    object ouser = (object)user;
                    if (user.transmit_to_server_and_receive(ref ouser, true) == false)
                        return;

                    user = (bc_om_user)ouser;
                }
                user.inactive = inactive;
               
                bc_om_user_attributes ua = new bc_om_user_attributes();
                ua.user_id = user.id;
                ua.no_lists = false;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    ua.db_read();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    ua.tmode = bc_cs_soap_base_class.tREAD;
                    object opa = (object)ua;
                    if (ua.transmit_to_server_and_receive(ref opa, true) == false)
                        return;
                    ua = (bc_om_user_attributes)opa;
                  
                }


                _view.load_user(user, ua.attributes);

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_entity", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        void get_user_class_links(object sender, EloaduserlinksArgs e)
        {
            try
            {

                bc_om_cp_user_links ul = new bc_om_cp_user_links();
                ul.user_id = e.user_id;
                ul.area_id = e.area_id;
                ul.pref_type_id = e.pref_type_id;

                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    ul.db_read();
                else
                {
                    ul.tmode = bc_cs_soap_base_class.tREAD;
                    object oul;
                    oul = (object)ul;

                    if (ul.transmit_to_server_and_receive(ref oul, true) == false)
                        return;
                    ul = (bc_om_cp_user_links)oul;

                }
                _view.load_user_links(ul);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "get_entity_class_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
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


        void update_user(object sender, EupdateuserArgs e)
        {
            try
            {
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    e.user.db_write();
                else
                {
                  
                    e.user.tmode = bc_cs_soap_base_class.tWRITE;
                    object ouser;
                    ouser = (object)e.user;

                    if (e.user.transmit_to_server_and_receive(ref ouser, true) == false)
                        return;
                    e.user = (bc_om_user)ouser;
                }
                if (e.reload == true)
                {
                    _view.reload_users(e.user.first_name + " " + e.user.surname);
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "update_user", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        void Eupdateattribute(object sender, EUserArgs e)
        {
            try
            {
                bc_om_user_attribute ua = new bc_om_user_attribute();
                ua.user_id = e.user_id;
                ua.att_val = e.attval;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    ua.db_write();
                else
                {
                    ua.tmode = bc_cs_soap_base_class.tWRITE;
                    object opubtype = (object)ua;
                    if (ua.transmit_to_server_and_receive(ref opubtype, true) == false)
                        return;
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "Eupdateattribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
    }
    public interface Ibc_cp_dx_users
    {
        bool load_view(bool view_only);
        void load_user(bc_om_user user, List<bc_om_attribute> attributes);
        void load_user_links(bc_om_cp_user_links elinks);
        void load_links();
        void reload_users(string sel_user_name);
        event EventHandler<EloadentityArgs> Eloaduser;
        event EventHandler<EloaduserlinksArgs> Eloaduserlinks;
        event EventHandler<EassignArgs> Eassign;
        event EventHandler<EupdateuserArgs> Eupdateuser;
        event EventHandler<EUserArgs> Eupdateattribute;
    }

   
    public class EUserArgs : EventArgs
    {
        public long user_id { get; set; }
        public bc_om_attribute_value attval  { get; set; }
    }

    public class EupdateuserArgs : EventArgs
    {
        public bc_om_user user { get; set; }
        public bool reload { get; set; }
    }

    public class EloaduserlinksArgs : EventArgs
    {
        public long user_id { get; set; }
        public EFIXEDENTITYCLASSES area_id { get; set; }
        public int pref_type_id { get; set; }
    }

}