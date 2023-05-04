using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using System.ServiceModel.Activation;
namespace bluecurve.core.wcf.ws
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)] 
    public class Service1 : BlueCurveWS
    {

        public cs_object_packet generic_object_transmission_wcf(bluecurve.core.wcf.ws.cs_object_packet value)
        {
            bc_cs_central_settings bcs = new bc_cs_central_settings(true);
            bc_cs_wcf_receive_packet rp = new bc_cs_wcf_receive_packet();
            try
            {
                 return rp.receive_packet(value);
            }
            catch (Exception e)
            {
                bc_cs_error_log err = new bc_cs_error_log("bluecurve.core.wcf.ws", "generic_object_transmission_wcf", bc_cs_error_codes.USER_DEFINED, e.InnerException.ToString(), ref rp.certificate);
                value.transmission_state = 1;
                return (value);
            }
           
        }

        public string password_management(bc_cs_logon  ingot)
        {
            try
            {
                bc_cs_central_settings bcs = new bc_cs_central_settings(true);
                if (ingot.mode == 1)
                    ingot.reset_password_request(ref ingot.certificate);
                else if (ingot.mode == 2)
                    ingot.change_password(ref ingot.certificate);
                else
                    return "invalid mode";

                if (ingot.certificate.error_state == true)
                    return ingot.certificate.server_errors[0].ToString();
                else
                    return "0";
            }
            catch (Exception e)
            {
                return "password management general error: " +  e.Message.ToString();
            }
        }
    }
}
