using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace bc_core_components_svc
{
    public class bc_core_rest_generic_object_transmission : bc_cs_soap_base_class
    {
        public bc_cs_soap_base_class.cs_object_packet receive_packet(bc_cs_soap_base_class.cs_object_packet tpacket)
        {
            bc_cs_soap_base_class.cs_object_packet return_packet = new bc_cs_soap_base_class.cs_object_packet();
            try
            {
                certificate = new bc_cs_security.certificate();
                certificate.user_id = "receiving via rest...";
                certificate.authentication_mode = tpacket.certificate.authentication_mode;
                certificate.authentication_token = tpacket.certificate.authentication_token;
                certificate.name = tpacket.certificate.name;
                certificate.password = tpacket.certificate.password;
                certificate.os_name = tpacket.certificate.os_name;
                certificate.client_mac_address = tpacket.certificate.client_mac_address;
                certificate.override_username_password = tpacket.certificate.override_username_password;
                certificate.server_errors.Clear();
                packet_code = tpacket.packet_code;
                if (authenticate_user() == true)
                {

                    if (certificate.authentication_mode == 2)
                    {
                        bc_cs_db_services gbc_db = new bc_cs_db_services();
                        string ssql;
                        ssql = "delete from bc_core_ad_tokens where token='" + certificate.authentication_token + "'";
                        gbc_db.executesql(ssql, ref certificate);
                    }

                    int i;


                    bc_cs_soap_base_class new_object;
                    bc_cs_security bcs = new bc_cs_security();

                    if (tpacket.use_rest_compression == true)
                        //oobject = read_data_from_string(bcs.decompress_xml(return_packet.received_object, certificate), certificate, Me.packet_code)


                        new_object = (bc_cs_soap_base_class)read_data_from_string(bcs.decompress_xml(tpacket.sent_object, certificate), ref certificate, tpacket.packet_code);

                    else
                        new_object = (bc_cs_soap_base_class)read_data_from_string(tpacket.sent_object, ref certificate, tpacket.packet_code);
                    new_object.certificate = certificate;

                    //REM call polymorphic function to start object specific processing
                    if (bc_cs_central_settings.log_user_session == true)
                    {

                        var properties = OperationContext.Current.IncomingMessageProperties;
                        var endpointProperty = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                        if (endpointProperty != null)
                        {
                            var ip = endpointProperty.Address;
                            bcs.log_user_session(ip, ref certificate);
                        }
                        else
                            bcs.log_user_session("", ref certificate);
                    }
                    new_object.process_object();
                    if (new_object.certificate.server_errors.Count > 0)
                    {
                        new_object.no_send_back = true;
                        return_packet.transmission_state = 1;
                        for (i = 0; i <= new_object.certificate.server_errors.Count - 1; i++)
                        {
                            return_packet.server_errors.Add(new_object.certificate.server_errors[i].ToString());
                        }
                    }
                    else
                        return_packet.transmission_state = 0;

                    
                    if (new_object.no_send_back == false)
                    {
                        return_packet.no_send_back = false;


                        if (tpacket.use_rest_compression == true)
                            return_packet.received_object = bcs.compress_xml(new_object.write_data_to_string(ref certificate, tpacket.packet_code), ref certificate);
                        else
                            return_packet.received_object = new_object.write_data_to_string(ref certificate, tpacket.packet_code);


                    }
                    else
                    {
                        return_packet.no_send_back = true;
                    }

                }
                else
                {
                    return_packet.transmission_state = 2;
                    if (bc_cs_central_settings.log_user_session == true)
                    {
                        string comment;
                        if (certificate.authentication_mode == 0)
                            comment = "authenication failed os name [" + certificate.os_name + "]";
                        else
                            comment = "authenication failed user name [" + certificate.name + "]";
                        var properties = OperationContext.Current.IncomingMessageProperties;
                        var endpointProperty = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                        bc_cs_security bcs = new bc_cs_security();
                        if (endpointProperty != null)
                        {
                            var ip = endpointProperty.Address;
                            bcs.log_user_session(comment + ": ip: " + ip.ToString(), ref certificate);
                        }
                        else
                            bcs.log_user_session(comment, ref certificate);
                    }

                }
            }
            catch (Exception e)
            {
                bc_cs_error_log err = new bc_cs_error_log("BlueCurve.Components.WCF", "generic_object_transmission_rest", bc_cs_error_codes.USER_DEFINED, e.InnerException.ToString(), ref certificate);
                return_packet.transmission_state = 1;
            }
            return return_packet;
        }
        }
    
}