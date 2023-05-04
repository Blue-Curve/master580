using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.NetworkInformation;
public class bc_am_calcs_service_poll
{
    bc_cs_security.certificate _certificate;
    public bc_am_calcs_service_poll(bc_cs_security.certificate certificate)
    {
        _certificate = certificate;
    }

    public void service_poll()
    {
        try
        {

            bc_cs_db_services db = new bc_cs_db_services();

            String sql;
            object res;
            Array ares;
            if (bc_cs_central_settings.calc_service_run_under_IIS == false)
                sql = "exec dbo.bc_core_get_next_queued_calc_request '" + _certificate.client_mac_address + "'";
            else
                sql = "exec dbo.bc_core_get_next_queued_calc_request  '" + _certificate.client_mac_address + " under IIS'";
            res = db.executesql(sql, ref  _certificate);
            ares = (Array)res;
            int i;

            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                long entity_id = (long)ares.GetValue(0, 0);
                long id = (long)ares.GetValue(1, 0);
                int audit_id = (int)ares.GetValue(2, 0);
                DateTime audit_date = (DateTime)ares.GetValue(3, 0);
                int contributor_id = (int)ares.GetValue(4, 0);
                // update universe state
                if (bc_cs_central_settings.calc_service_run_entities_concurrently == false)
                {
                    if (bc_cs_central_settings.calc_service_run_under_IIS == false)
                        run_calcs(entity_id, id, audit_id, audit_date, contributor_id);
                    else
                        run_calcs_under_IIS(entity_id , id, audit_id, audit_date,contributor_id);
                }
                else
                {
                    Thread thread;
                    if (bc_cs_central_settings.calc_service_run_under_IIS == false)
                        thread = new Thread(() => run_calcs(entity_id, id, audit_id, audit_date, contributor_id));
                    else
                        thread = new Thread(() => run_calcs_under_IIS(entity_id, id, audit_id, audit_date, contributor_id));
                    thread.Start();
                }
            }
        }

        catch (Exception ex)
        {
            bc_cs_error_log err = new bc_cs_error_log("bc_cs_calcs", "service_poll", bc_cs_error_codes.USER_DEFINED, ex.Message, ref _certificate);
        }
    }

    public void run_calcs(long entity_id,long id, int audit_id, DateTime audit_date, int contributor_id)
    {
        bc_cs_security.certificate certificate = new bc_cs_security.certificate();
        try
        {
            bc_cs_db_services db = new bc_cs_db_services();
            List<bc_cs_db_services.bc_cs_sql_parameter> sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
            bc_cs_db_services.bc_cs_sql_parameter sql_param;

            bc_cs_max_address_services ma = new bc_cs_max_address_services();
            certificate.client_mac_address = ma.get_mac_address();
            certificate.user_id = "BlueCurveCalcServices: " + entity_id.ToString() +  ": " + certificate.client_mac_address;
            bc_am_calcs_serviced_based rcalcs = new bc_am_calcs_serviced_based();
            rcalcs.run(entity_id, ref certificate, audit_id, audit_date, contributor_id);
            if (certificate.server_errors.Count > 0)
            {
                string serr;
                bc_cs_string_services fs = new bc_cs_string_services(certificate.server_errors[0].ToString());
                serr = fs.delimit_apostrophies();
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "id";
                sql_param.value = id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "status";
                sql_param.value = 4;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_id";
                sql_param.value = audit_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_date";
                sql_param.value = audit_date;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "err_text";
                sql_param.value = serr;
                sql_params.Add(sql_param);
            }
            else
            {
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "id";
                sql_param.value = id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "status";
                sql_param.value = 3;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_id";
                sql_param.value = audit_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_date";
                sql_param.value = audit_date;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "err_text";
                sql_param.value = "";
                sql_params.Add(sql_param);
            }
            db.executesql_with_parameters_no_timeout("bc_core_service_calcs_complete", sql_params, ref certificate);

            rcalcs = null;
            GC.Collect();
        }
        catch (Exception ex)
        {
            bc_cs_error_log err = new bc_cs_error_log("bc_cs_calcs", "run_calcs_under_iis", bc_cs_error_codes.USER_DEFINED, ex.Message, ref certificate);
        }
    }
    public void run_calcs_under_IIS(long entity_id, long id, int audit_id, DateTime audit_date,int contributor_id)
    {
        bc_cs_security.certificate certificate = new bc_cs_security.certificate();
        try
        {
            bc_cs_max_address_services ma = new bc_cs_max_address_services();
            certificate.client_mac_address = ma.get_mac_address();
            certificate.user_id = "BlueCurveCalcServices under IIS: " + entity_id.ToString() +  ": " + certificate.client_mac_address;
          
            BlueCurve.Aggregations.AM.ServiceReference1.BCIISServicesClient s = new BlueCurve.Aggregations.AM.ServiceReference1.BCIISServicesClient();

            s.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(9999999);
            EndpointAddress ea = new EndpointAddress("http://localhost/BCIISServices/BCIISServices.svc");
            s.Endpoint.Address = ea;
            string err="";
            try
            {
                bc_cs_activity_log comm = new bc_cs_activity_log("bc_cs_calcs", "run_calcs_under_iis", bc_cs_activity_codes.COMMENTARY.ToString(), "Start invoking under IIS universe: " + entity_id.ToString(), ref certificate);

                err = s.CalcAll(id,entity_id, audit_id, audit_date,contributor_id);
                comm = new bc_cs_activity_log("bc_cs_calcs", "run_calcs_under_iis", bc_cs_activity_codes.COMMENTARY.ToString(), "End invoking under IIS universe: " + entity_id.ToString() , ref certificate);

            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            bc_cs_db_services.bc_cs_sql_parameter sql_param;
            List<bc_cs_db_services.bc_cs_sql_parameter> sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
            if (err != "")
            {
                string serr;
                bc_cs_string_services fs = new bc_cs_string_services(err);
                serr = fs.delimit_apostrophies();
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "id";
                sql_param.value = id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "status";
                sql_param.value = 4;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_id";
                sql_param.value = audit_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_date";
                sql_param.value = audit_date;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "err_text";
                sql_param.value = serr;
                sql_params.Add(sql_param);
            }
            else
            {
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "id";
                sql_param.value = id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "status";
                sql_param.value = 3;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_id";
                sql_param.value = audit_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_date";
                sql_param.value = audit_date;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "err_text";
                sql_param.value = "";
                sql_params.Add(sql_param);
            }
            bc_cs_db_services db = new bc_cs_db_services();
            db.executesql_with_parameters_no_timeout("bc_core_service_calcs_complete", sql_params, ref certificate);

        }
        catch (Exception ex)
        {
            bc_cs_error_log err = new bc_cs_error_log("bc_cs_calcs", "run_calcs_under_iis", bc_cs_error_codes.USER_DEFINED, ex.Message, ref certificate);
        }
    }

}


