using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Threading;
using BlueCurve.Core.CS;
using BlueCurve.Dataloader.AM;
namespace BlueCurve.IIS.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class BCIISServices : IBCIISServices
    {
      
        public string AggregateUniverse(long run_id,long universe_id, int audit_id, DateTime audit_date)
        {
            bc_cs_central_settings bcs = new bc_cs_central_settings(true);  
            bc_cs_security.certificate certificate = new  bc_cs_security.certificate();
            bc_am_aggs_service_based aggs = new bc_am_aggs_service_based();

            bc_cs_max_address_services ma = new bc_cs_max_address_services();
            certificate.client_mac_address = ma.get_mac_address();
            certificate.user_id = "BlueCurveAggUniverseServices Under IIS: " + universe_id.ToString() + ": " + certificate.client_mac_address;

            aggs.run(universe_id, ref certificate,audit_id, audit_date);
           
            if (certificate.server_errors.Count > 0)
            {
                return certificate.server_errors[0].ToString();
            }
            else
            {
                return "";
            }
        }

        public agg_results AggregateUniverseDebug(long run_id, long universe_id, int audit_id, DateTime audit_date, long target_entity_id, long dual_entity_id, int debug_exch_method, string debug_calc_type, bool inc_constituents)
        {
            bc_cs_central_settings bcs = new bc_cs_central_settings(true);
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            bc_am_aggs_service_based aggs = new bc_am_aggs_service_based();

            bc_cs_max_address_services ma = new bc_cs_max_address_services();
            certificate.client_mac_address = ma.get_mac_address();
            certificate.user_id = "BlueCurveAggUniverseServicesPreview Under IIS: " + universe_id.ToString() + ": " + certificate.client_mac_address;

            aggs.run(universe_id, ref certificate, audit_id, audit_date, true, target_entity_id, dual_entity_id, debug_exch_method, debug_calc_type);
            agg_results results= new agg_results ();

            results.results = aggs.debugallresults;
            results.ttest = aggs.debuglttest_result;
            if (inc_constituents == true)
            {
                results.abc_calc_agg = aggs.debugabc_calc_aggs;
                results.abc_calc_agg_growths = aggs.debugabc_calc_aggs_growths;
                results.abc_calc_agg_cc = aggs.debugabc_calc_aggs_cc;
            }
            results.error = "";
            
            if (certificate.server_errors.Count > 0)
            {
                results.error= certificate.server_errors[0].ToString();
            }

            return results;
            
           // return results;
        }

        public string CalcAll(long run_id, long entity_id, int audit_id,  DateTime audit_date, int contributor_id)
        {
            bc_cs_central_settings bcs = new bc_cs_central_settings(true);
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            bc_am_calcs_serviced_based calcs = new bc_am_calcs_serviced_based();

            bc_cs_max_address_services ma = new bc_cs_max_address_services();
            certificate.client_mac_address = ma.get_mac_address();
            certificate.user_id = "BlueCurveCalcServices Under IIS: " + entity_id.ToString()  + ": " + certificate.client_mac_address;

            calcs.run(entity_id, ref certificate, audit_id, audit_date,contributor_id);

            if (certificate.server_errors.Count > 0)
            {
                return certificate.server_errors[0].ToString();
            }
            else
            {
                return "";
            }
        }
        public string RunTask(int task_id)
        {
            bc_cs_central_settings bcs = new bc_cs_central_settings(true);
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();

            
            
            bc_cs_max_address_services ma = new bc_cs_max_address_services();
            certificate.client_mac_address = ma.get_mac_address();
            certificate.user_id = "BlueCurveDataLoadServices Under IIS: " + task_id.ToString() + ": " + certificate.client_mac_address;

            task task = new task();
            int status = 1;
            string err_text = "";
            task.run(task_id, ref certificate, ref  status, ref err_text);
            bc_cs_db_services gdb = new bc_cs_db_services();
            bc_cs_string_services sf = new bc_cs_string_services(err_text);
            gdb.executesql("exec dbo.bc_core_dl_log_task_status " + task_id.ToString() + "," + status.ToString() + ",'" + sf.delimit_apostrophies() + "'", ref certificate);


            if (certificate.server_errors.Count > 0)
            {
                return certificate.server_errors[0].ToString();
            }
            else
            {
                return "";
            }
        }

    }
}
