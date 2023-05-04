using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace bluecurve.core.wcf.ws
{
    public class bc_cs_wcf_receive_packet : bc_cs_soap_base_class
    {
        public bluecurve.core.wcf.ws.cs_object_packet receive_packet(bluecurve.core.wcf.ws.cs_object_packet tpacket)
        {
            bluecurve.core.wcf.ws.cs_object_packet return_packet = new bluecurve.core.wcf.ws.cs_object_packet();

            try
            {
                certificate = new bc_cs_security.certificate();
                certificate.user_id = "receiving...";
                certificate.authentication_mode = tpacket.certificate.authentication_mode;
                certificate.authentication_token = tpacket.certificate.authentication_token;
                certificate.name = tpacket.certificate.name;
                certificate.password = tpacket.certificate.password;
                certificate.os_name = tpacket.certificate.os_name;
                certificate.client_mac_address = tpacket.certificate.client_mac_address;
                certificate.server_errors.Clear();
            
                packet_code = tpacket.packet_code;
                if (authenticate_user() == true)
                {
                    bc_cs_soap_base_class new_object;
                    bc_cs_security ocomp = new bc_cs_security();
                    string id;
                    bc_cs_file_transfer_services fs = new bc_cs_file_transfer_services();
                    id = tpacket.packet_code + "_" + certificate.user_id;
                    string complete_string = "";

                    if ((tpacket.number_of_packets > 0) && (tpacket.packet_number + 1 < tpacket.number_of_packets))
                    {
                        StreamWriter sw = new StreamWriter("c:\\bluecurve\\packet" + id + "_" + (tpacket.packet_number + 1).ToString() + ".txt");
                        sw.WriteLine(tpacket.sent_object);
                        sw.Close();
                    }

                    if (tpacket.packet_number + 1 == tpacket.number_of_packets)
                    // check if last packet if so  rebuild object from packets
                    {
                        if (certificate.authentication_mode == 2)
                        {
                            bc_cs_db_services gbc_db = new bc_cs_db_services();
                            string ssql;
                            ssql = "delete from bc_core_ad_tokens where token='" + certificate.authentication_token + "'";
                            gbc_db.executesql(ssql, ref certificate);
                        }

                        bc_cs_activity_log comm = new bc_cs_activity_log("bc_cs_wcf_receive_packet", "receive_packet", "3", "All Packets Received of:" + tpacket.number_of_packets.ToString(), ref certificate);
                        int i;
                        for (i = 1; i < tpacket.number_of_packets; i++)
                        {
                            StreamReader sfs = new StreamReader("c:\\bluecurve\\packet" + id + "_" + i.ToString() + ".txt");
                            complete_string = complete_string + sfs.ReadToEnd();
                            sfs.Close();
                        }
                        //                REM house keep files
                        for (i = 1; i < tpacket.number_of_packets; i++)
                        {
                            fs.delete_file("c:\\bluecurve\\packet" + id + "_" + i.ToString() + ".txt");
                        }

                        complete_string = complete_string + tpacket.sent_object;
                        //REM uncompress input and derialize into correct polymorphic object
                        new_object = (bc_cs_soap_base_class)read_data_from_string(ocomp.decompress_xml(complete_string, certificate), ref certificate, tpacket.packet_code);
                        new_object.certificate = certificate;

                        //REM call polymorphic function to start object specific processing
                        if (bc_cs_central_settings.log_user_session == true)
                        {

                            var properties = OperationContext.Current.IncomingMessageProperties;
                            var endpointProperty = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                            bc_cs_security bcs = new bc_cs_security();
                            if (endpointProperty != null)
                            {
                                var ip = endpointProperty.Address;
                                bcs.log_user_session(ip, ref certificate);
                            }
                            else
                                bcs.log_user_session("", ref certificate);
                        }
                        new_object.process_object();

                      
                        if (new_object.no_send_back == false)
                        {
                            return_packet.no_send_back = false;
                            if (new_object.certificate.error_state == true)
                            {
                                return_packet.transmission_state = 1;
                            }
                            bc_cs_security bcs = new bc_cs_security();
                            return_packet.received_object = bcs.compress_xml(new_object.write_data_to_string(ref certificate, tpacket.packet_code), ref certificate);
                            if (certificate.error_state == true)
                            {
                                return_packet.transmission_state = 1;
                                for (i = 0; i <= certificate.server_errors.Count - 1; i++)
                                {
                                    return_packet.server_errors.Add(certificate.server_errors[i].ToString());
                                }
                            }
                            else
                                return_packet.transmission_state = 0;
                        }
                        else
                        {
                            if (certificate.error_state == true)
                                return_packet.transmission_state = 1;
                            else
                                return_packet.transmission_state = 0;
                            return_packet.no_send_back = true;
                        }
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
                bc_cs_error_log err = new bc_cs_error_log("bluecurve.core.wcf.ws", "generic_object_transmission_wcf", bc_cs_error_codes.USER_DEFINED, e.InnerException.ToString(), ref certificate);
                return_packet.transmission_state = 1;
            }
            return return_packet;
        }



        //public cs_object_packet receive_packet(cs_object_packet tpacket)
        //{
        //    cs_object_packet return_packet=new cs_object_packet();

        //    try
        //    {
        //        certificate= new bc_cs_security.certificate();
        //        certificate.user_id = "receiving...";
        //        certificate.authentication_mode = tpacket.certificate.authentication_mode;
        //        certificate.authentication_token = tpacket.certificate.authentication_token;
        //        certificate.name= tpacket.certificate.name;
        //        certificate.password= tpacket.certificate.password;
        //        certificate.os_name=tpacket.certificate.os_name;
        //        certificate.server_errors.Clear();
        //        packet_code=tpacket.packet_code;
        //        if (authenticate_user() == true)
        //        {
        //            bc_cs_soap_base_class new_object;
        //            bc_cs_security ocomp = new bc_cs_security();
        //            string id;
        //            bc_cs_file_transfer_services fs = new bc_cs_file_transfer_services();
        //            id = tpacket.packet_code + "_" + certificate.user_id;
        //            string complete_string = "";
                        
        //            fs.write_bytestream_to_document("c:\\bluecurve\\packet" + id + "_" + (tpacket.packet_number + 1).ToString() + ".txt", tpacket.packet, ref certificate);
                    
        //            if (tpacket.packet_number + 1 == tpacket.number_of_packets)
        //            // check if last packet if so  rebuild object from packets
        //            {
        //               bc_cs_activity_log comm = new bc_cs_activity_log("bc_cs_wcf_receive_packet", "receive_packet", "3", "All Packets Received of:" + tpacket.number_of_packets.ToString(), ref certificate);
        //               int i;
        //               for (i = 1; i <= tpacket.number_of_packets; i++)
        //               {
        //                 StreamReader sfs = new StreamReader("c:\\bluecurve\\packet" + id + "_" + i.ToString() + ".txt");
        //                 complete_string = complete_string + sfs.ReadToEnd();
        //                 sfs.Close();
        //               }
        //               //                REM house keep files
        //               for (i = 1; i <= tpacket.number_of_packets; i++)
        //               {
        //                 fs.delete_file("c:\\bluecurve\\packet" + id + "_" + i.ToString() + ".txt");
        //               }
                       
        //               //REM uncompress input and derialize into correct polymorphic object
        //               new_object = (bc_cs_soap_base_class)read_data_from_string(ocomp.decompress_xml(complete_string, certificate), ref certificate, tpacket.packet_code);
        //               new_object.certificate = certificate;

        //               //REM call polymorphic function to start object specific processing
    
        //               new_object.process_object();
                    
        //               if (bc_cs_central_settings.show_authentication_form == 2) 
        //               {
        //                 return_packet.certificate.ad_excel_token = certificate.ad_excel_token;
        //               }
        //               if (new_object.no_send_back == false) 
        //               {
        //                 return_packet.no_send_back = false;
        //                 if (new_object.certificate.error_state == true )
        //                 {
        //                   return_packet.transmission_state = 1;
        //                 }
        //                 bc_cs_security bcs = new bc_cs_security();
        //                 return_packet.received_object = bcs.compress_xml(new_object.write_data_to_string(ref certificate, tpacket.packet_code), ref certificate);
        //                 if (certificate.error_state == true)
        //                 {
        //                     return_packet.transmission_state = 1;
        //                     for (i = 0; i <= certificate.server_errors.Count - 1; i++)
        //                     {
        //                         return_packet.server_errors.Add(certificate.server_errors[i].ToString());
        //                     }
        //                 }
        //                 else
        //                     return_packet.transmission_state = 0;
        //                }
        //                else
        //                {
        //                  if (certificate.error_state == true)
        //                    return_packet.transmission_state = 1;
        //                  else
        //                     return_packet.transmission_state = 0;
        //                  return_packet.no_send_back = true;
        //                }
        //           }
        //        }
        //        else
        //            return_packet.transmission_state = 2;

        //    }
        //    catch (Exception e)
        //    {
        //        bc_cs_error_log err = new bc_cs_error_log("bluecurve.core.wcf.ws", "generic_object_transmission_wcf", bc_cs_error_codes.USER_DEFINED, e.InnerException.ToString(), ref certificate);
        //        return_packet.transmission_state = 1;
        //    }
        //    return return_packet;
        //}
    }
}