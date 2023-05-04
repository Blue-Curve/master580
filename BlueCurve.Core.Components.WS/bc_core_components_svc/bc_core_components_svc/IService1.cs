using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;

namespace bc_core_components_svc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
   // [ServiceContract]
     [ServiceContract]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  
    public interface IComponentsService
    {
        
        [OperationContract]
         string TestService(int value);

        [OperationContract]
        string TestComponent(bc_core_component component);
       
        [OperationContract]
        string GetJsonForBCComponent(long entity_id, int type,  long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 0,DateTime data_at_date = default(DateTime),long pub_type_id=0,bc_core_component_parameters parameters=null);

        [OperationContract]
        string GetJsonForBCComponentInTemplate(long entity_id, int type, long template_id, long comp_id, long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 0, DateTime data_at_date = default(DateTime), long pub_type_id=0);

        [OperationContract]
        string GetJsonForBCComponentInDocument(long entity_id, int type, long doc_id, string locator = "", long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 0, DateTime data_at_date = default(DateTime), long pub_type_id=0);

        [OperationContract]
        string GetJsonForDocumentCompostion(long doc_id, bool save_to_file = false);




      
    }

    [DataContract]
    public class bc_core_component
    {
        int _type;
        long _entity_id=0;
        long _stage_id=8;
        long _contributor_id=1;
        long _user_id=0;
        long _language_id=1;
        DateTime _data_at_date;
        long _pub_type_id=0;
        bc_core_component_parameters _parameters;

        [DataMember]
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        [DataMember]
        public long stage_id
        {
            get { return _stage_id; }
            set { _stage_id = value; }
        }
        [DataMember]
        public long contributor_id
        {
            get { return _contributor_id; }
            set { _contributor_id = value; }
        }
        [DataMember]
        public long user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }
        [DataMember]
        public long entity_id
        {
            get { return _entity_id; }
            set { _entity_id = value; }
        }
        [DataMember]
        public long language_id
        {
            get { return _language_id; }
            set { _language_id = value; }
        }
        [DataMember]
        public long pub_type_id
        {
            get { return _pub_type_id; }
            set { _pub_type_id = value; }
        }
        [DataMember]
        public DateTime data_at_date
        {
            get { return _data_at_date; }
            set { _data_at_date = value; }
        }
        public bc_core_component_parameters parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
    }

    [DataContract]
    public class bc_core_component_parameter
    {
        bc_core_components_objects.SYSTEM_DEFINED _system_defiend;
        string _value;

        [DataMember]
        public bc_core_components_objects.SYSTEM_DEFINED system_defiend
        {
            get { return _system_defiend; }
            set {  _system_defiend=value; }
        }
        [DataMember]
        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
    [DataContract]
    public class bc_core_component_parameters
    {
       

        List<bc_core_component_parameter> _lparameters;

       

        [DataMember]
        public List<bc_core_component_parameter> lparameters
        {
            get { return _lparameters; }
            set { _lparameters = value; }

        }
        
    }
   
}
