using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BlueCurve.Core.CS;
using System.IO;
using BlueCurve.Aggregations.AM;
using BlueCurve.Dataloader.AM;
namespace BlueCurve.Core.Services
{
    public partial class BlueCurveServices : ServiceBase
    {
        public BlueCurveServices()
        {
            InitializeComponent();
        }

        Thread emailthread;
        Thread emailpreviewthread;
        Thread maillistthread;
        Thread distributionthread;
        Thread aggregationthread;
        Thread calcthread;
        Thread dataloadthread;
        int EMAIL_POLL_INTERVAL = 5000;
        int EMAIL_PREVIEW_POLL_INTERVAL = 5000;
        int MAIL_LIST_POLL_INTERVAL = 5000;
        int DISTRIBUTION_POLL_INTERVAL = 5000;
        int AGGREGATION_POLL_INTERVAL = 5000;
        int CALC_POLL_INTERVAL = 7000;
        int DATA_LOAD_INTERVAL = 5000;
        protected override void OnStart(string[] args)
        {

            try
            {
                
                bc_cs_central_settings bcs = new bc_cs_central_settings(true);
                bc_cs_central_settings.server_flag = 1;
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                certificate.user_id = "BlueCurveServices";
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Services", ref certificate);
                // email service
                if (bc_cs_central_settings.email_service_enabled == true)
                {
                    EMAIL_POLL_INTERVAL = bc_cs_central_settings.email_service_poll_interval;
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Email Services: Polling " + EMAIL_POLL_INTERVAL.ToString() + " ms concurrent thread: " + bc_cs_central_settings.email_service_maximum_concurrent_threads.ToString(), ref certificate);
                    emailthread = new Thread(new ThreadStart(EmailThreadFunction));
                    emailthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Email Services Disabled", ref certificate);
                //email preview service
                if (bc_cs_central_settings.email_preview_service_enabled == true)
                {
                    EMAIL_PREVIEW_POLL_INTERVAL = bc_cs_central_settings.email_preview_service_poll_interval;
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Email Preview Services: Polling " + EMAIL_POLL_INTERVAL.ToString(), ref certificate);
                    emailpreviewthread = new Thread(new ThreadStart(EmailPreviewThreadFunction));
                    emailpreviewthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Email Preview Services Disabled", ref certificate);
                //mailing list service
                if (bc_cs_central_settings.mailing_list_service_enabled == true)
                {
                    MAIL_LIST_POLL_INTERVAL = bc_cs_central_settings.mailing_list_service_poll_interval;
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Mailing List Services: Polling " + MAIL_LIST_POLL_INTERVAL.ToString(), ref certificate);
                    maillistthread = new Thread(new ThreadStart(maillistThreadFunction));
                    maillistthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Mail List Services Disabled", ref certificate);


                //distribution service
                if (bc_cs_central_settings.distribution_service_enabled == true)
                {
                    DISTRIBUTION_POLL_INTERVAL = bc_cs_central_settings.distribution_service_poll_interval;
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve  Distribution Services: Polling " + DISTRIBUTION_POLL_INTERVAL.ToString(), ref certificate);
                    distributionthread = new Thread(new ThreadStart(DistributionThreadFunction));
                    distributionthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Distribution Services Disabled", ref certificate);
                if (bc_cs_central_settings.aggregation_service_enabled == true)
                {
                    AGGREGATION_POLL_INTERVAL = bc_cs_central_settings.aggregation_service_poll_interval;
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Aggregation Services: Polling " + AGGREGATION_POLL_INTERVAL.ToString(), ref certificate);
                    aggregationthread = new Thread(new ThreadStart(AggregationThreadFunction));
                    aggregationthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Aggregation Services Disabled", ref certificate);

                if (bc_cs_central_settings.calc_service_enabled == true)
                {
                   CALC_POLL_INTERVAL = bc_cs_central_settings.calc_service_poll_interval;
                   ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Calc Services: Polling " + CALC_POLL_INTERVAL.ToString(), ref certificate);
                   calcthread = new Thread(new ThreadStart(CalcThreadFunction));
                   calcthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Calc Services Disabled", ref certificate);

                if (bc_cs_central_settings.data_load_service_enabled == true)
                {
                    DATA_LOAD_INTERVAL = bc_cs_central_settings.data_load_poll_interval;
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Starting Blue Curve Data load Service: Polling " + DATA_LOAD_INTERVAL.ToString(), ref certificate);
                    dataloadthread = new Thread(new ThreadStart(DataLoadThreadFunction));
                    dataloadthread.Start();
                }
                else
                    ocomm = new bc_cs_activity_log("BlueCurveServices", "Start", bc_cs_activity_codes.COMMENTARY.ToString(), "Data load Services Disabled", ref certificate);

            }
            catch (Exception e)
            {
                StreamWriter fss = new StreamWriter("c:\\bluecurve\\BlueCurveServiceErrors.txt", true);
                fss.WriteLine(e.InnerException.ToString());
                fss.Close();
            }
        }

        protected override void OnStop()
        {
            try
            {
                emailthread.Abort();
            }
            catch
            {

            }

        }
        protected void EmailThreadFunction()
        {


            bool i;
            i = true;
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            certificate.user_id = "BlueCurveEmailServices";
            bc_cs_email_services email = new bc_cs_email_services(certificate);
            while (i == true)
            {
                Thread.Sleep(EMAIL_POLL_INTERVAL);
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "EmailThreadFunction", bc_cs_activity_codes.COMMENTARY.ToString(), "poll email", ref certificate);
                email.service_poll();
            }
        }
        protected void EmailPreviewThreadFunction()
        {
            bool i;
            i = true;
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            certificate.user_id = "BlueCurveEmailPreviewServices";
            bc_cs_email_preview_services email_preview = new bc_cs_email_preview_services(ref certificate);
            while (i == true)
            {
                Thread.Sleep(EMAIL_PREVIEW_POLL_INTERVAL);
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "EmailPreviewThreadFunction", bc_cs_activity_codes.COMMENTARY.ToString(), "poll email preview", ref certificate);
                email_preview.service_poll();
            }
        }
        protected void maillistThreadFunction()
        {
            bool i;
            i = true;
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            certificate.user_id = "BlueCurveMailListServices";
            bc_cs_distribution_list dist_list = new bc_cs_distribution_list(certificate);
            while (i == true)
            {
                Thread.Sleep(MAIL_LIST_POLL_INTERVAL);
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "BlueCurveMailListServices", bc_cs_activity_codes.COMMENTARY.ToString(), "poll mail list", ref certificate);
                dist_list.service_poll();
            }
        }
        protected void DistributionThreadFunction()
        {
            bool i;
            i = true;
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            certificate.user_id = "BlueCurveDistributionServices";
            bc_cs_reach_distribute reach_distribute = new bc_cs_reach_distribute(certificate);

            while (i == true)
            {
                Thread.Sleep(DISTRIBUTION_POLL_INTERVAL);
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "BlueCurveDistributionServices", bc_cs_activity_codes.COMMENTARY.ToString(), "poll distribution", ref certificate);
                reach_distribute.service_poll();
            }
        }
        protected void AggregationThreadFunction()
         {
             bc_cs_security.certificate certificate = new bc_cs_security.certificate();
             try
             {
                 bool i;
                 i = true;
                
                 
                 bc_cs_max_address_services ma = new bc_cs_max_address_services();
                 certificate.client_mac_address = ma.get_mac_address();
                 certificate.user_id = "BlueCurveAggregationServices: " + certificate.client_mac_address;
                 bc_am_aggs_service_poll aggs = new bc_am_aggs_service_poll(certificate);
                 
                 while (i == true)
                 {
                     Thread.Sleep(AGGREGATION_POLL_INTERVAL);
                     bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "BlueCurveAggregationServices: " + certificate.client_mac_address, bc_cs_activity_codes.COMMENTARY.ToString(), "poll aggregation threads: " + bc_cs_central_settings.aggregation_service_num_active_threads.ToString() + " entities: " + bc_cs_central_settings.aggregation_service_entities_per_batch.ToString(), ref certificate);
                     certificate.server_errors.Clear();
                     certificate.error_state = false;
                     aggs.service_poll();
                 }
             }
            catch (Exception e)
            {
                 bc_cs_error_log oerr = new bc_cs_error_log("BlueCurveServices", "BlueCurveAggregationServices", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);
            }
        }


        protected void CalcThreadFunction()
        {
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            try
            {
                bool i;
                i = true;


                bc_cs_max_address_services ma = new bc_cs_max_address_services();
                certificate.client_mac_address = ma.get_mac_address();
                certificate.user_id = "BlueCurveCalcServices: " + certificate.client_mac_address;
                bc_am_calcs_service_poll calcs = new bc_am_calcs_service_poll(certificate);

                while (i == true)
                {
                    Thread.Sleep(CALC_POLL_INTERVAL);
                    bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "BlueCurveCalcServices: " + certificate.client_mac_address, bc_cs_activity_codes.COMMENTARY.ToString(), "poll calcs threads: " + bc_cs_central_settings.calc_service_num_active_threads.ToString() + " entities: " + bc_cs_central_settings.calc_service_entities_per_batch.ToString() + "interval: " + CALC_POLL_INTERVAL.ToString(), ref certificate);
                    certificate.server_errors.Clear();
                    certificate.error_state = false;
                    calcs.service_poll();
                }
            }
            catch (Exception e)
            {
                bc_cs_error_log oerr = new bc_cs_error_log("BlueCurveServices", "BlueCurveCalcServices", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);
            }
        }

        protected void DataLoadThreadFunction()
        {


            bool i;
            i = true;
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            certificate.user_id = "BlueCurveDataLoadServices";
            bc_cs_data_load_services dls = new bc_cs_data_load_services (certificate);
            while (i == true)
            {
                Thread.Sleep(DATA_LOAD_INTERVAL);
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurveServices", "DataLoadThreadFunction", bc_cs_activity_codes.COMMENTARY.ToString(), "poll data load", ref certificate);
                dls.service_poll();
            }
        }

        protected void test_connection()
        {

            string json;
            bc_cs_security.certificate certficate = new bc_cs_security.certificate();
            StreamWriter sw = new StreamWriter("c:\\testconn\\log.txt");
            sw.WriteLine("start");

            json = "{\"token\" : \"c77c31a6f2d4433e8887c34e42da67c7\",\"user\" : { \"forename\": \"Paul\", \"surname\": \"Rose\",\"email\": \"paul.rose@bluecurvelimited.com\",\"phone\": \"1234\"}}";
            sw.WriteLine(json);
            bc_cs_ns_json_post rp = new bc_cs_ns_json_post("https://www.rsrchx.com/api/1/add_author",json);
            rp.send(certficate);
            sw.WriteLine(rp.response_text);
            sw.WriteLine(rp.err_text);
            sw.WriteLine("start");
            sw.Close();
        }

    }
}
