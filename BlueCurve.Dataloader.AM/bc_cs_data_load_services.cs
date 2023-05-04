using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BlueCurve.Core.CS;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace BlueCurve.Dataloader.AM
{
    public class bc_cs_data_load_services
    {
        bc_cs_security.certificate _certificate;
        public bc_cs_data_load_services(bc_cs_security.certificate certificate)
        {
            _certificate = certificate;
        }
        public void service_poll()
        {
            try
            {
                bc_cs_db_services gdb = new bc_cs_db_services();
                object res;
                Array ares;
                 Thread thread;
                res = gdb.executesql("exec dbo.bc_core_dl_poll_for_tasks", ref  _certificate);
                ares = (Array)res;
                int i;
                
                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    int task_id = (int)ares.GetValue(0,i);
                  if( bc_cs_central_settings.data_load_run_under_IIS== false)
                    thread = new Thread(() => run_task(task_id));
                  else
                    thread = new Thread(() => run_task_under_IIS(task_id));
                  thread.Start();
                }
           
            }
            catch (Exception e)
            {
              bc_cs_error_log err = new bc_cs_error_log("bc_cs_data_load_services", "service_poll", bc_cs_error_codes.USER_DEFINED, e.Message.ToString(), ref _certificate);
            }
        }
        void run_task(int task_id)
        {
            try 
            { 
            task task = new task();
            int status=1;
            string err_text="";
            task.run(task_id, ref _certificate, ref  status, ref err_text);
            bc_cs_db_services gdb = new bc_cs_db_services();
            bc_cs_string_services sf = new bc_cs_string_services(err_text);

            gdb.executesql("exec dbo.bc_core_dl_log_task_status " + task_id.ToString() + "," + status.ToString() + ",'" + sf.delimit_apostrophies() + "'", ref _certificate);
            }
            catch (Exception e)
            {
                bc_cs_error_log err = new bc_cs_error_log("bc_cs_data_load_services", "run_task", bc_cs_error_codes.USER_DEFINED, e.Message.ToString(), ref _certificate);
            }
        }
      
         
        void run_task_under_IIS(int task_id)
        {
           
            try
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_max_address_services ma = new bc_cs_max_address_services();
                certificate.client_mac_address = ma.get_mac_address();
                certificate.user_id = "BlueCurveCalcServices under IIS: " + task_id.ToString() + ": " + certificate.client_mac_address;

                BlueCurve.Dataloader.AM.ServiceReference1.BCIISServicesClient s = new BlueCurve.Dataloader.AM.ServiceReference1.BCIISServicesClient();

                s.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(9999999);
                EndpointAddress ea = new EndpointAddress("http://localhost/BCIISServices/BCIISServices.svc");
                s.Endpoint.Address = ea;
             
                try
                {
                    bc_cs_activity_log comm = new bc_cs_activity_log("bc_cs_calcs", "run_calcs_under_iis", bc_cs_activity_codes.COMMENTARY.ToString(), "Start invoking under IIS universe: " + task_id.ToString(), ref certificate);
                    string serr;
                    serr = s.RunTask(task_id);
                    if (serr != "")
                    {
                        bc_cs_error_log err = new bc_cs_error_log("bc_cs_data_load_services", "run_task_under_IIS", bc_cs_error_codes.USER_DEFINED, serr, ref _certificate);
      
                    }
                    comm = new bc_cs_activity_log("bc_cs_calcs", "run_tasks_under_iis", bc_cs_activity_codes.COMMENTARY.ToString(), "End invoking under IIS universe: " + task_id.ToString(), ref certificate);

                }
                catch (Exception ex)
                {
                    bc_cs_error_log err = new bc_cs_error_log("bc_cs_data_load_services", "run_task_under_IIS", bc_cs_error_codes.USER_DEFINED, ex.Message.ToString(), ref _certificate);
      
                }
            }
            catch (Exception ex)
            {
                bc_cs_error_log err = new bc_cs_error_log("bc_cs_data_load_services", "run_task_under_IIS", bc_cs_error_codes.USER_DEFINED, ex.Message.ToString(), ref _certificate);
      
            }
         }

      }
   }
