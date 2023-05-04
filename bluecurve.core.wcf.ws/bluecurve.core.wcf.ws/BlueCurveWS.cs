using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BlueCurve.Core.CS;

namespace bluecurve.core.wcf.ws
{
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface BlueCurveWS
    {

        [OperationContract]
        cs_object_packet generic_object_transmission_wcf(cs_object_packet value);

        [OperationContract]
        string password_management(bc_cs_logon ingot);
       


        //[OperationContract]
        //string excel_function_transmission_wcf(string s, string logged_on_user_id, string os_user_name);
    }


    [DataContract]
    public class cs_object_packet
    {
        int _packet_number;
        string _packet_code;
        int _number_of_packets;
        /*byte[] _packet;*/
        int _transmission_state = 0;
        string _received_object = "";
        string _sent_object = "";
        bool _no_send_back;
        bc_cs_security.certificate _certificate;
        List<string> _server_errors=new List<string>();

        [DataMember]
        public List<string> server_errors
        {
            get { return _server_errors; }
            set { _server_errors = value; }
        }


        [DataMember]
        public int packet_number
        {
            get { return _packet_number; }
            set { _packet_number = value; }
        }
        [DataMember]
        public string packet_code
        {
            get { return _packet_code; }
            set { _packet_code = value; }
        }
        [DataMember]
        public int number_of_packets
        {
            get { return _number_of_packets; }
            set { _number_of_packets = value; }
        }
        //[DataMember]
        //public byte[] packet
        //{
        //    get { return _packet; }
        //    set { _packet = value; }
        //}

        [DataMember]
        public int transmission_state
        {
            get { return _transmission_state; }
            set { _transmission_state = value; }
        }
        [DataMember]
        public string sent_object
        {
            get { return _sent_object; }
            set { _sent_object = value; }
        }


        [DataMember]
        public string received_object
        {
            get { return _received_object; }
            set { _received_object = value; }
        }

        //Public server_errors As New ArrayList
        [DataMember]
        public bool no_send_back
        {
            get { return _no_send_back; }
            set { _no_send_back = value; }
        }
         [DataMember]
        public bc_cs_security.certificate certificate
        {
            get { return _certificate; }
            set { _certificate = value; }
        }

    }
}
