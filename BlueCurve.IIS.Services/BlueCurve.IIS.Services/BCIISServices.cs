using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BlueCurve.IIS.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBCIISServices
    {
     


        [OperationContract]
        string AggregateUniverse(long run_id, long universe_id, int audit_id, DateTime audit_date);

        [OperationContract]
        agg_results AggregateUniverseDebug(long run_id, long universe_id, int audit_id, DateTime audit_date, long target_entity_id, long dual_entity_id, int debug_exch_method, string debug_calc_type,bool inc_constituents );

        [OperationContract]
        string CalcAll(long run_id, long entity_id, int audit_id, DateTime audit_date, int contributor_id);
        [OperationContract]
        string RunTask(int task_id);

        // TODO: Add your service operations here
    }

    [DataContract]
    public class agg_results
    {
        [DataMember]
        public List<agg_result> results= new List<agg_result>();
        [DataMember]
        public List<abc_calc_agg> abc_calc_agg = new List<abc_calc_agg>();
        [DataMember]
        public List<abc_calc_agg> abc_calc_agg_growths = new List<abc_calc_agg>();
        [DataMember]
        public List<abc_calc_agg> abc_calc_agg_cc = new List<abc_calc_agg>();
        [DataMember]
        public List<ttest_result> ttest = new  List<ttest_result>();
        [DataMember]
        public string error;
    }

//    int _year;
//    int _period_id;
//    long _item_id;
//    int _contributor_id;
//    int _workflow_stage;
//    long _universe_id;
//    long _target_entity_id;
//    long _dual_entity_id;
//    Nullable<decimal> _value;
//    Nullable<decimal> _lvalue;
//    Nullable<decimal> _rvalue;
//    int _num_years;
//    long _contributor_1_id;
//    long _contributor_2_id;
//    int _exch_type;
//    long _audit_id;
//    long _currency;

    

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
