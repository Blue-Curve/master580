using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Security.Principal;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.IO;
using System.Net;
using BlueCurve.Core.CS;
namespace bluecurve.core.ad.wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class bluecurve_core_ad_wcf : Ibluecurve_core_ad_wcf
    {

        public string TestAdConnect(string os_logon, string mac_address)
        {
            try
            {
                bc_cs_central_settings bcs = new bc_cs_central_settings(true);
                if  (bc_cs_central_settings.show_authentication_form != 2)
                {
                    return "Error: server authentication not set to Active Directory <prompt>2</prompt>";
                }
                bc_cs_db_services db = new bc_cs_db_services();
                List <bc_cs_db_services.bc_cs_sql_parameter> sparams = new  List <bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter param;
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "os_logon";
                param.value = os_logon;
                sparams.Add(param);
                param = new bc_cs_db_services.bc_cs_sql_parameter();
                param.name = "mac_address";
                param.value = mac_address;
                sparams.Add(param);
                bc_cs_security.certificate  certificate = new  bc_cs_security.certificate();
                certificate.user_id="ad authenticating: " + os_logon;
                object res;
                res = db.executesql_with_parameters("dbo.bc_core_set_ad", sparams, ref certificate);
                if (certificate.server_errors.Count > 0)
                    return "Error: " + certificate.server_errors[0].ToString();
                
                Array ares;
                

                ares = (Array)res;
                if (ares.GetUpperBound(1) == 0)
                    return ares.GetValue(0, 0).ToString();
                else
                    return "Error: nothing returned from SP"; 
             

            }
            catch (Exception e)
            {
                return "Error: " + e.Message.ToString();
            }
        }

    }
}
       
   


