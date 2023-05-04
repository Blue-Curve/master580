using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bc_core_components_objects;
using BlueCurve.Core.CS;
using System.Web.Script.Serialization;
namespace bc_core_components_svc
{

    
    public class log_email_readership
    {
        string _user;
        string _doc_id;
        public  log_email_readership(string user, string doc_id)
        {
            _user = user;
            _doc_id = doc_id;
        }
        public string log(int type)
        {
            try
            {
                List<bc_cs_db_services.bc_cs_sql_parameter> parameters = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "user";
                param.value = _user;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "doc_id";
                param.value = _doc_id;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "type_id";
                param.value = type;
                parameters.Add(param);

                bc_cs_db_services db = new bc_cs_db_services();
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                certificate.name = "log email readership";
                bc_cs_central_settings bcs = new bc_cs_central_settings(true);
                db.executesql_with_parameters("bc_core_log_email_readership", parameters, ref certificate);
                if (certificate.server_errors.Count > 0)
                    return certificate.server_errors[0].ToString();
                else
                    return "";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }
    }

    public class bc_core_web_authentication
    {

       
         

        public string  authenticate_user(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
        {
            try
            {

                bc_core_web_authentication_db gdb = new bc_core_web_authentication_db();
                object res=null;
                Array ares;

                if (credentials.eEncryption_type == ENCRYPTION_TYPE.SERVER)
                {
                    if (credentials.user_name != "from email" || credentials.user_name != "anon")
                    {
                        if (encrypt_password(ref credentials, ref certificate) == false)
                        {
                            certificate.error_state = true;
                            certificate.server_errors.Add("authenticate user error: failed to encrypt password");
                            return "0";
                        }
                    }
                }

                res = gdb.authenticate_user(credentials, ref certificate);
                if   (certificate.error_state == true)
                {
                    return "0";
                }
                ares = (Array)res;
                if (ares.GetUpperBound(0) == 1)
                {
                    int n;
                    bool isNumeric = int.TryParse(ares.GetValue(0, 0).ToString(), out n);
                    // AUG 2018 only log readership is valid logon
                    if (credentials.log_readership == true && ares.GetValue(0, 0).ToString() != "0" &&  isNumeric==true)
                    {
                        credentials.logged_in_id = ares.GetValue(0, 0).ToString();
                        gdb.log_readership(credentials, ref certificate);
                    }

                    return ares.GetValue(0, 0).ToString();
                }
                
                else 
                {
                    certificate.error_state = true;
                    certificate.server_errors.Add ( "authentication error: nothing returned from sp") ;
                    return "0";
                }
            }
            catch(Exception e)
            {
                 certificate.error_state = true;
                 certificate.server_errors.Add ( "authentication error:  bc_core_web_authentication:authenticate_user " + e.Message.ToString());
                 return "0";
            }

        }

        public string change_password(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
        {
            try
            {

                bc_core_web_authentication_db gdb = new bc_core_web_authentication_db();

                if (credentials.eEncryption_type==ENCRYPTION_TYPE.SERVER)
                {
                  if (credentials.user_name != "from email" || credentials.user_name != "anon")
                  {
                      bc_cs_security sec = new bc_cs_security();
                      credentials.password = sec.HashPassword(1, credentials.password, ref  certificate);
                  }
                }
                object res;
                Array ares;
                res=gdb.change_password(credentials, ref certificate);
                ares = (Array)res;
                if (ares.GetUpperBound(0) == 1)
                {
                    return ares.GetValue(0, 0).ToString();
                }
                else
                {
                    return "Success";
                }
            }
            catch (Exception e)
            {
                return "chnage password error:  bc_core_web_authentication:change_password " + e.Message.ToString();
            }

        }

        public string reset_password_request(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
        {
            try
            {

                bc_core_web_authentication_db gdb = new bc_core_web_authentication_db();
                object res;
                Array ares;
                res = gdb.reset_password(credentials, ref certificate);
                if (certificate.error_state == true)
                {
                    return certificate.server_errors[0].ToString();
                }
                ares = (Array)res;
                if (ares.GetUpperBound(0) == 1)
                {
                    return ares.GetValue(0, 0).ToString();
                }
                else
                {
                    return "Success";
                }
            }
            catch (Exception e)
            {
                return "reset password error:  bc_core_web_authentication:reset_password_request " + e.Message.ToString();
            }

        }
        public string set_password(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
        {
            try
            {

                bc_core_web_authentication_db gdb = new bc_core_web_authentication_db();
                object res;
                Array ares;
                if (credentials.eEncryption_type== ENCRYPTION_TYPE.SERVER )
                {
                    encrypt_password(ref credentials, ref certificate);
                }

                res = gdb.set_password(credentials, ref certificate);
                if (certificate.error_state == true)
                {
                    return certificate.server_errors[0].ToString();
                }
                ares = (Array)res;
                if (ares.GetUpperBound(0) == 1)
                {
                    return ares.GetValue(0, 0).ToString();
                }
                else
                {
                    return "Success";
                }
            }
            catch (Exception e)
            {
                return "reset password error:  bc_core_web_authentication:set_password " + e.Message.ToString();
            }

        }

        private bool encrypt_password(ref bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
        {
           
                bc_cs_security sec = new bc_cs_security();
                credentials.password = sec.HashPassword(1, credentials.password, ref  certificate);
                return true;
           
        }
        public class bc_core_web_authentication_db
        {
            bc_cs_db_services db = new bc_cs_db_services();
            public object authenticate_user(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
            {
               
                List <bc_cs_db_services.bc_cs_sql_parameter> parameters = new    List <bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "user";
                param.value = credentials.user_name;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "password";
                param.value = credentials.password;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
if (credentials.application== null)
{
    credentials.application = "";
}
                param.name = "application";
                param.value = credentials.application;
                parameters.Add(param);

                return db.executesql_with_parameters("bc_core_web_authenticate_user", parameters, ref certificate);
            }

            public object log_readership(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
            {

                List<bc_cs_db_services.bc_cs_sql_parameter> parameters = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "client_id";
                param.value = credentials.logged_in_id;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "name";
                param.value = credentials.user_name;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "type";
                param.value = credentials.readership_type;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "key";
                param.value = credentials.readership_id;
                parameters.Add(param);
                return db.executesql_with_parameters("bc_core_web_log_readership", parameters, ref certificate);
            }

            public object change_password(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
            {
                if (credentials.application == null)
                {
                    credentials.application = "";
                }
                List<bc_cs_db_services.bc_cs_sql_parameter> parameters = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "user_id";
                param.value = certificate.user_id;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "password";
                param.value = credentials.new_password;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "application";
                param.value = credentials.application;
                parameters.Add(param);
                return db.executesql_with_parameters("bc_core_web_change_password", parameters, ref certificate);
            }

            public object reset_password(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
            {
                if (credentials.application == null)
                {
                    credentials.application = "";
                }
                List<bc_cs_db_services.bc_cs_sql_parameter> parameters = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "user";
                param.value = credentials.user_name;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "email";
                param.value = credentials.email_address;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "application";
                param.value = credentials.application;
                parameters.Add(param);
                return db.executesql_with_parameters("bc_core_web_reset_password", parameters, ref certificate);
            }
            public object set_password(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
            {
                if (credentials.application == null)
                {
                    credentials.application = "";
                }
                List<bc_cs_db_services.bc_cs_sql_parameter> parameters = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "password";
                param.value = credentials.password;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "email";
                param.value = credentials.email_address;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "guid";
                param.value = credentials.guid;
                parameters.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "application";
                param.value = credentials.application;
                parameters.Add(param);
                return db.executesql_with_parameters("bc_core_web_set_password", parameters, ref certificate);
            }

            //public object user_id_from_name(bc_core_credentials credentials, ref bc_cs_security.certificate certificate)
            //{

            //    List<bc_cs_db_services.bc_cs_sql_parameter> parameters = new List<bc_cs_db_services.bc_cs_sql_parameter>();
            //    bc_cs_db_services.bc_cs_sql_parameter param;
            //    param = new bc_cs_db_services.bc_cs_sql_parameter();
            //    param.name = "user";
            //    param.value = credentials.user_name;
            //    parameters.Add(param);

            //    parameters.Add(param);
            //    return db.executesql_with_parameters("bc_core_web_authenticate_user_id_from_name", parameters, ref certificate);
            //}

            public object set_guid(long entity_id, long pub_type_id, long doc_id, ref bc_cs_security.certificate certificate)
            {
                return db.executesql("exec dbo.bc_core_web_products_set_guid " + entity_id.ToString() + "," + pub_type_id.ToString() + "," + certificate.user_id.ToString() + "," + doc_id.ToString(), ref certificate);
            }
            public object get_guid_parameters(string guid, ref bc_cs_security.certificate certificate)
            {
                return db.executesql("exec dbo.bc_core_web_products_get_values_for_guid '" + guid + "'", ref certificate);
            }

        }
    }
}