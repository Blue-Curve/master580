using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using System.Threading;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Data;
public class bc_am_calcs_serviced_based
{


    int audit_id;
    int contributor_id;
    DateTime audit_date;
    bc_cs_central_settings bcs = new bc_cs_central_settings(true);
    bc_cs_security.certificate certificate;
    bc_cs_db_services gdb = new bc_cs_db_services();
    List<template> child_templates = new List<template>();
    List<template> parent_templates = new List<template>();

    int thread_count;

    SynchronizedCollection<calcs_error_activity> timings = new SynchronizedCollection<calcs_error_activity>();
    public SynchronizedCollection<calcs_error_activity> errors = new SynchronizedCollection<calcs_error_activity>();

    SynchronizedCollection<db_entity_period_tbl> alltemplateresults = new SynchronizedCollection<db_entity_period_tbl>();


    private readonly object monitor = new object();
    public void run(long entity_id, ref  bc_cs_security.certificate gcertificate, int laudit_id, DateTime laudit_date, int lcontributor_id, bool debug_mode = false, long target_entity_id = 0, long dual_entity_id = 0, int debug_exch_method = -1, string debug_calc_type = "")
    {
        certificate = gcertificate;
        calcs_error_activity log_activity;
        try
        {

            List<bc_cs_db_services.bc_cs_sql_parameter> sql_params;
            bc_cs_db_services.bc_cs_sql_parameter sql_param;
            audit_id = laudit_id;
            audit_date = laudit_date;
            contributor_id = lcontributor_id;
            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "Start Calcs " + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);

            object res;
            Array ares;


            int i;
            // evaluate all templates to execite and set entities into batches

            sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "entity_id";
            sql_param.value = entity_id;
            sql_params.Add(sql_param);
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "num_per_batch";
            sql_param.value = bc_cs_central_settings.calc_service_entities_per_batch;
            sql_params.Add(sql_param);
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "audit_id";
            sql_param.value = audit_id;
            sql_params.Add(sql_param);
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "audit_date";
            sql_param.value = audit_date;
            sql_params.Add(sql_param);

            res = gdb.executesql_with_parameters_no_timeout("bc_core_service_calcs_set_template_entities", sql_params, ref certificate);
            ares = (Array)res;

            template ltemplate;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                ltemplate = new template();
                ltemplate.num_batches = (int)ares.GetValue(0, i);
                ltemplate.template_id = (int)ares.GetValue(1, i);
                ltemplate.parent = (bool)ares.GetValue(2, i);
                if (ltemplate.parent == false)
                    child_templates.Add(ltemplate);
                else
                    parent_templates.Add(ltemplate);
            }

            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "Preset template entity complete" + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);

            // execute each child template in parallel
            List<Thread> athreads = new List<Thread>();



            for (i = 0; i < child_templates.Count; i++)
            {
                int template_id;
                int num_batch;
                template_id = child_templates[i].template_id;
                num_batch = child_templates[i].num_batches;
                Thread thread = new Thread(() => calc_template(false, template_id, num_batch));
                athreads.Add(thread);
            }
            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "All Child templates threads set" + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);
            thread_count = 0;

            for (i = 0; i < child_templates.Count; i++)
            {
                while (thread_count >= bc_cs_central_settings.calc_service_num_active_threads)
                {
                    Thread.Sleep(50);
                }
                Thread.Sleep(50);
                lock (monitor)
                {
                    thread_count = thread_count + 1;
                }
                athreads[i].Start();
            }

            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "All Child templates threads started" + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);

            // wait until all child tenplates complete 
            while (thread_count > 0)
            {
                Thread.Sleep(500);
            }




            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "Child templates complete" + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);


            bulkdatainsert md = new bulkdatainsert();
            log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "Started db write child: " + audit_id.ToString(), false, ref certificate, audit_id, 0, 0, 0);
            timings.Add(log_activity);
            md.BulkInsertEPTEAVTS(alltemplateresults, audit_id, 0, ref certificate, ref errors);
            alltemplateresults.Clear();
            log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "End db write  child: " + audit_id.ToString(), false, ref certificate, audit_id, 0, 0, 0);
            timings.Add(log_activity);


            // execute each parent template in parallel
            athreads = new List<Thread>();




            for (i = 0; i < parent_templates.Count; i++)
            {
                int template_id;
                int num_batch;
                template_id = parent_templates[i].template_id;
                num_batch = parent_templates[i].num_batches;
                Thread thread = new Thread(() => calc_template(true, template_id, num_batch));

                athreads.Add(thread);
            }


            thread_count = 0;

            for (i = 0; i < parent_templates.Count; i++)
            {
                while (thread_count >= bc_cs_central_settings.calc_service_num_active_threads)
                {
                    Thread.Sleep(50);
                }
                Thread.Sleep(50);
                lock (monitor)
                {
                    thread_count = thread_count + 1;
                }
                athreads[i].Start();
            }

            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "All Parent templates threads started" + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);

            // wait until all child tenplates complete 
            while (thread_count > 0)
            {
                Thread.Sleep(500);
            }

            log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "run", "Parent templates complete" + entity_id.ToString(), false, ref certificate, 0, 0, 0, 0);
            timings.Add(log_activity);

            md = new bulkdatainsert();
            log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "Started db write parent: " + audit_id.ToString(), false, ref certificate, audit_id, 0, 0, 0);
            timings.Add(log_activity);
            md.BulkInsertEPTEAVTS(alltemplateresults, audit_id, 0, ref certificate, ref errors);
            log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "End db write  parent:: " + audit_id.ToString(), false, ref certificate, audit_id, 0, 0, 0);
            timings.Add(log_activity);


            log_activity = new calcs_error_activity("class_calc_template", "run_template", "All batches for template" + audit_id.ToString() + "Complete.", false, ref certificate, audit_id, 0, 0, 0);
            timings.Add(log_activity);

            write_activity_log(entity_id);
            write_error_log(entity_id);


        }
        catch (Exception e)
        {
            calcs_error_activity log_error = new calcs_error_activity("bc_am_calcs_serviced_based", "run", e.Message, true, ref certificate, 0, 0, 0, 0);
            errors.Add(log_error);
        }
    }

    void calc_template(bool is_parent, int template_id, int num_batches)
    {
        try
        {
            calcs_error_activity log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "calc_template", "Start Template: " + template_id.ToString() + " Thread Count: " + thread_count.ToString(), false, ref certificate, template_id, 0, 0, 0);
            timings.Add(log_activity);

            class_calc_template cb = new class_calc_template();
            cb.run_template(is_parent, audit_id, audit_date, contributor_id, template_id, num_batches, ref timings, ref errors, ref certificate, ref alltemplateresults);

        }
        catch (Exception e)
        {
            calcs_error_activity log_error = new calcs_error_activity("bc_am_calc_serviced_based", "calc_template", "Template: " + template_id.ToString() + ":" + e.Message, true, ref certificate, template_id, 0, 0, 0);
            errors.Add(log_error);
        }
        finally
        {
            calcs_error_activity log_activity = new calcs_error_activity("bc_am_aggs_serviced_based", "calc_template", "End Template: " + template_id.ToString() + " Thread Count: " + thread_count.ToString(), false, ref certificate, template_id, 0, 0, 0);
            timings.Add(log_activity);
            lock (monitor)
            {
                thread_count = thread_count - 1;
            }
        }
    }
    class class_calc_template
    {
        private readonly object monitor = new object();
        int thread_count;
        SynchronizedCollection<calcs_error_activity> timings;
        SynchronizedCollection<calcs_error_activity> errors;
        bc_cs_security.certificate certificate;
        int template_id;
        int audit_id;
        int contributor_id;
        DateTime audit_date;
        List<calculation> calculations = new List<calculation>();
        List<template_calc_batch> template_calc_batches = new List<template_calc_batch>();
        bool is_parent;
        SynchronizedCollection<db_entity_period_tbl> alltemplateresults;

        public void run_template(bool lis_parent, int laudit_id, DateTime laudit_date, int lcontributor_id, int ltemplate_id, int num_batches, ref SynchronizedCollection<calcs_error_activity> ltimings, ref SynchronizedCollection<calcs_error_activity> lerrors, ref bc_cs_security.certificate lcertificate, ref SynchronizedCollection<db_entity_period_tbl> lalltemplateresults)
        {
            try
            {
                alltemplateresults = lalltemplateresults; ;
                is_parent = lis_parent;
                audit_id = laudit_id;
                audit_date = laudit_date;
                template_id = ltemplate_id;
                contributor_id = lcontributor_id;
                timings = ltimings;
                errors = lerrors;
                certificate = lcertificate;
                //// load calcs




                object res;
                Array ares;
                String sql;
                bc_cs_db_services gdb = new bc_cs_db_services();

                sql = "exec dbo.bc_core_calcs_services_get_calculation_template_batches " + audit_id.ToString() + "," + template_id.ToString();
                res = gdb.executesql(sql, ref certificate);
                ares = (Array)res;
                int i;
                template_calc_batch ltemplate_calc_batch;
                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    ltemplate_calc_batch = new template_calc_batch();
                    ltemplate_calc_batch.type = (int)ares.GetValue(0, i);
                    ltemplate_calc_batch.order = (int)ares.GetValue(1, i);
                    template_calc_batches.Add(ltemplate_calc_batch);
                }


                sql = "exec dbo.bc_core_calcs_services_get_calculations " + audit_id.ToString() + "," + template_id.ToString();
                res = gdb.executesql(sql, ref certificate);
                ares = (Array)res;

                calculation calc;
                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    calc = new calculation();
                    calc.calc_order = (int)ares.GetValue(0, i);
                    calc.result_row_id = (long)ares.GetValue(1, i);
                    calc.operand_1_id = (long)ares.GetValue(2, i);
                    calc.operand_2_id = (long)ares.GetValue(3, i);
                    calc.operand_3_id = (long)ares.GetValue(4, i);
                    calc.operand_4_id = (long)ares.GetValue(5, i);
                    calc.operand_5_id = (long)ares.GetValue(6, i);
                    calc.operand_6_id = (long)ares.GetValue(7, i);
                    calc.operand_7_id = (long)ares.GetValue(8, i);
                    calc.operand_8_id = (long)ares.GetValue(9, i);
                    calc.formula = (string)ares.GetValue(10, i);
                    calc.num_years = (int)ares.GetValue(11, i);
                    calc.contributor_1 = (int)ares.GetValue(12, i);
                    calc.contributor_2 = (int)ares.GetValue(13, i);
                    calc.interval_type = (int)ares.GetValue(14, i);
                    calc.interval = (int)ares.GetValue(15, i);
                    calc.op1_curr_type = (int)ares.GetValue(16, i);
                    calc.op2_curr_type = (int)ares.GetValue(17, i);
                    calc.op3_curr_type = (int)ares.GetValue(18, i);
                    calc.op4_curr_type = (int)ares.GetValue(19, i);
                    calc.op5_curr_type = (int)ares.GetValue(20, i);
                    calc.op6_curr_type = (int)ares.GetValue(21, i);
                    calc.op7_curr_type = (int)ares.GetValue(22, i);
                    calc.op8_curr_type = (int)ares.GetValue(23, i);
                    calc.calculation_id = (long)ares.GetValue(24, i);
                    calc.icalc_type = (int)ares.GetValue(25, i);
                    calc.calc_type = (string)ares.GetValue(26, i);
                    calc.linq_formula = (string)ares.GetValue(27, i);
                    calc.linq_where = (string)ares.GetValue(28, i);
                    calc.historic = (bool)ares.GetValue(29, i);
                    calc.estimate = (bool)ares.GetValue(30, i);
                    calc.cumulative = (bool)ares.GetValue(31, i);
                    calc.result_curr_type = (int)ares.GetValue(32, i);
                    calc.static_to_period = (bool)ares.GetValue(33, i);
                    calc.op_zero_for_null = (bool)ares.GetValue(34, i);
                    calc.average = (bool)ares.GetValue(35, i);
                    calc.calc_lib = (int)ares.GetValue(36, i);
                    calculations.Add(calc);
                }


                //// run batches
                List<Thread> athreads = new List<Thread>();


                for (i = 0; i < num_batches; i++)
                {
                    int num_batch;
                    num_batch = i + 1;

                    Thread thread = new Thread(() => run_template_batch(num_batch));
                    athreads.Add(thread);
                }
                calcs_error_activity log_activity = new calcs_error_activity("class_calc_template", "run_template", "All batches for template: " + template_id.ToString() + "Set.", false, ref certificate, template_id, 0, 0, 0);
                timings.Add(log_activity);
                thread_count = 0;

                for (i = 0; i < num_batches; i++)
                {
                    while (thread_count >= bc_cs_central_settings.calc_service_num_active_threads)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(50);
                    lock (monitor)
                    {
                        thread_count = thread_count + 1;
                    }
                    athreads[i].Start();
                }


                log_activity = new calcs_error_activity("class_calc_template", "run_template", "All batches for template: " + template_id.ToString() + "Started.", false, ref certificate, template_id, 0, 0, 0);
                timings.Add(log_activity);


                // wait until all child tenplates complete 
                while (thread_count > 0)
                {
                    Thread.Sleep(500);
                }

               

            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "run_template", "batch: " + template_id.ToString() + ":" + e.Message, true, ref certificate, template_id, 0, 0, 0);
                errors.Add(log_error);
            }
        }

        void run_template_batch(int batch_num)
        {
            try
            {
                //List<db_entity_period_tbl> allresults = new List<db_entity_period_tbl>();

                List<abc_calc> normal = new List<abc_calc>();
                List<abc_calc> growths = new List<abc_calc>();
                List<abc_calc> cc = new List<abc_calc>();
                List<abc_calc> mom = new List<abc_calc>();

                List<db_entity_period_tbl> entity_period_tbl = new List<db_entity_period_tbl>();
                List<entity_info> entity_period_tbl_info = new List<entity_info>();

                calcs_error_activity log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "Started Template: " + template_id.ToString() + " batch: " + batch_num.ToString(), false, ref certificate, template_id, 0, 0, batch_num);
                timings.Add(log_activity);


                // load data  db
                get_data_from_db(batch_num, ref entity_period_tbl_info, ref entity_period_tbl);
                // run calc batche in correct ordr

                int i;
                for (i = 0; i < template_calc_batches.Count; i++)
                {
                    calc_template_batch(is_parent, batch_num, template_calc_batches[i].type, template_calc_batches[i].order, ref entity_period_tbl, entity_period_tbl_info);
                }
                //write results to db
                log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "Started db seal: " + template_id.ToString() + " batch: " + batch_num.ToString(), false, ref certificate, template_id, 0, 0, batch_num);
                timings.Add(log_activity);
                //// seal previous results
                List<bc_cs_db_services.bc_cs_sql_parameter> sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                bc_cs_db_services.bc_cs_sql_parameter sql_param;
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_id";
                sql_param.value = audit_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_date";
                sql_param.value = audit_date;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "batch_id";
                sql_param.value = batch_num;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "template_id";
                sql_param.value = template_id;
                sql_params.Add(sql_param);
                bc_cs_db_services gdb = new bc_cs_db_services();
                bool success = false;
                object res;
                while (success == false)
                {
                    res = gdb.executesql_with_parameters_no_timeout("bc_core_calcs_services_seal_data", sql_params, ref certificate);
                    if (res != null)
                        success = true;
                    else
                        Thread.Sleep(500);
                }

                bool quit_loop;
                quit_loop = false;
                res = null;
                gdb.rp_dl = false;
                gdb.success = false;
                while (quit_loop == false)
                {
                    res = gdb.executesql_with_parameters_no_timeout_cp_dl("bc_core_calcs_services_seal_data", sql_params, ref certificate);
                    if (gdb.success == true)
                        quit_loop = true;
                    else if (gdb.rp_dl == true)
                        Thread.Sleep(50);
                    else
                    {
                        quit_loop = true;
                    }
                }


                for (i = 0; i < entity_period_tbl.Count; i++)
                {
                    if (entity_period_tbl[i].result_row == true)
                        alltemplateresults.Add(entity_period_tbl[i]);
                }
                //bulkdatainsert md = new bulkdatainsert();
                //log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "Started db write: " + template_id.ToString() + " batch: " + batch_num.ToString(), false, ref certificate, template_id, 0, 0, batch_num);
                //timings.Add(log_activity);
                //md.BulkInsertEPTEAVTS (allresults, template_id, batch_num, ref certificate, ref errors);
                //log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "End db write: " + template_id.ToString() + " batch: " + batch_num.ToString(), false, ref certificate, template_id, 0, 0, batch_num);
                //timings.Add(log_activity);
            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "run_template_batch", "Template: " + template_id.ToString() + " batch: " + batch_num.ToString() + ":" + e.Message, true, ref certificate, template_id, 0, 0, batch_num);
                errors.Add(log_error);
            }
            finally
            {
                calcs_error_activity log_activity = new calcs_error_activity("class_calc_template", "run_template_batch", "Complete Template: " + template_id.ToString() + " batch: " + batch_num.ToString(), false, ref certificate, template_id, 0, 0, batch_num);
                timings.Add(log_activity);
                lock (monitor)
                {
                    thread_count = thread_count - 1;
                }
            }
        }
        void calc_template_batch(bool is_parent, int batch_id, int calc_type, int calc_order, ref List<db_entity_period_tbl> entity_period_tbl, List<entity_info> entity_period_tbl_info)
        {
            try
            {

                List<abc_calc> calcs;

                List<abc_calc> stagenormal = new List<abc_calc>();
                List<abc_calc> stagegrowths = new List<abc_calc>();
                List<abc_calc> stagecc = new List<abc_calc>();
                List<abc_calc> stagemom = new List<abc_calc>();

                if ((calc_type == 0) || (calc_type == 2))
                {

                    var datastage = from c in calculations
                                    join e in entity_period_tbl_info on c.key equals e.key
                                    where e.year == 9999
                                    & c.icalc_type == calc_type & c.calc_order == calc_order
                                    & ((c.contributor_1 == 0 && c.interval_type == 0) ||
                                 (
                                      c.contributor_1 > 0 && c.interval_type == 0 && c.num_years > 0
                                 ))

                                    orderby e.entity_id, c.result_row_id, e.year
                                    select new abc_calc
                                    {
                                        calc_type = c.calc_type,
                                        entity_id = e.entity_id,
                                        parent_entity_id = e.parent_entity_id,
                                        year = e.year,
                                        period_id = e.period_id,
                                        workflow_stage = e.workflow_stage,
                                        contributor_id = e.contributor_id,
                                        result_row_id = c.result_row_id,
                                        operand_1_id = c.operand_1_id,
                                        operand_2_id = c.operand_2_id,
                                        operand_3_id = c.operand_3_id,
                                        operand_4_id = c.operand_4_id,
                                        operand_5_id = c.operand_5_id,
                                        operand_6_id = c.operand_6_id,
                                        operand_7_id = c.operand_7_id,
                                        operand_8_id = c.operand_8_id,

                                        op1_curr_type = c.op1_curr_type,
                                        op2_curr_type = c.op2_curr_type,
                                        op3_curr_type = c.op3_curr_type,
                                        op4_curr_type = c.op4_curr_type,
                                        op5_curr_type = c.op5_curr_type,
                                        op6_curr_type = c.op6_curr_type,
                                        op7_curr_type = c.op7_curr_type,
                                        op8_curr_type = c.op8_curr_type,
                                        formula = c.formula,
                                        num_years = c.num_years,
                                        contributor_1_id = c.contributor_1,
                                        contributor_2_id = c.contributor_2,
                                        interval_type = c.interval_type,
                                        interval = c.interval,
                                        calculation_id = c.calculation_id,
                                        current_price = e.current_price,
                                        price_denominator = e.price_denominator,

                                        trading_denominator = e.trading_denominator_current,
                                        p_c_ratio = e.p_c_ratio_current,
                                        p_t_ratio = e.p_t_ratio_current,
                                        t_c_ratio = e.t_c_ratio_current,
                                        cumulative_denominator = e.cumulative_denominator_current,

                                        currency_denominator = e.currency_denominator,
                                        historic = c.historic,
                                        estimate = c.estimate,
                                        cumulative = c.cumulative,
                                        result_curr_type = c.result_curr_type,
                                        exc_from_cumulative = e.exc_from_cumulative,
                                        static_to_period = c.static_to_period,
                                        op_zero_for_null = c.op_zero_for_null
                                    };


                    calcs = datastage.ToList<abc_calc>();

                    if (calcs.Count > 0)
                    {
                        assign_normal(is_parent, calcs, ref entity_period_tbl, ref stagenormal, batch_id, calc_type, calc_order);
                    }
                    datastage = from c in calculations
                                join e in entity_period_tbl_info on c.key equals e.key
                                where e.year == 9999
                                & c.icalc_type == calc_type & c.calc_order == calc_order
                                & c.num_years == 0 & c.contributor_1 > 0 && c.interval_type == 0

                                orderby e.entity_id, c.result_row_id, e.year
                                select new abc_calc
                                {
                                    calc_type = c.calc_type,
                                    entity_id = e.entity_id,
                                    parent_entity_id = e.parent_entity_id,
                                    year = e.year,
                                    period_id = e.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    result_row_id = c.result_row_id,
                                    operand_1_id = c.operand_1_id,
                                    operand_2_id = c.operand_2_id,
                                    operand_3_id = c.operand_3_id,
                                    operand_4_id = c.operand_4_id,
                                    operand_5_id = c.operand_5_id,
                                    operand_6_id = c.operand_6_id,
                                    operand_7_id = c.operand_7_id,
                                    operand_8_id = c.operand_8_id,

                                    op1_curr_type = c.op1_curr_type,
                                    op2_curr_type = c.op2_curr_type,
                                    op3_curr_type = c.op3_curr_type,
                                    op4_curr_type = c.op4_curr_type,
                                    op5_curr_type = c.op5_curr_type,
                                    op6_curr_type = c.op6_curr_type,
                                    op7_curr_type = c.op7_curr_type,
                                    op8_curr_type = c.op8_curr_type,
                                    formula = c.formula,
                                    num_years = c.num_years,
                                    contributor_1_id = c.contributor_1,
                                    contributor_2_id = c.contributor_2,
                                    interval_type = c.interval_type,
                                    interval = c.interval,
                                    calculation_id = c.calculation_id,
                                    current_price = e.current_price,
                                    price_denominator = e.price_denominator,

                                    trading_denominator = e.trading_denominator_current,
                                    p_c_ratio = e.p_c_ratio_current,
                                    p_t_ratio = e.p_t_ratio_current,
                                    t_c_ratio = e.t_c_ratio_current,
                                    cumulative_denominator = e.cumulative_denominator_current,

                                    currency_denominator = e.currency_denominator,
                                    historic = c.historic,
                                    estimate = c.estimate,
                                    cumulative = c.cumulative,
                                    result_curr_type = c.result_curr_type,
                                    exc_from_cumulative = e.exc_from_cumulative,
                                    static_to_period = c.static_to_period
                                };

                    calcs = datastage.ToList<abc_calc>();

                    if (calcs.Count > 0)
                    {
                        assign_cc(calcs, ref entity_period_tbl, ref stagecc, batch_id, calc_type, calc_order);
                    }

                    datastage = from c in calculations
                                join e in entity_period_tbl_info on c.key equals e.key
                                where e.year == 9999
                                & c.icalc_type == calc_type & c.calc_order == calc_order
                                & c.num_years == 0 & c.contributor_1 == 0 && c.interval_type > 0

                                orderby e.entity_id, c.result_row_id, e.year
                                select new abc_calc
                                {
                                    calc_type = c.calc_type,
                                    entity_id = e.entity_id,
                                    parent_entity_id = e.parent_entity_id,
                                    year = e.year,
                                    period_id = e.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    result_row_id = c.result_row_id,
                                    operand_1_id = c.operand_1_id,
                                    operand_2_id = c.operand_2_id,
                                    operand_3_id = c.operand_3_id,
                                    operand_4_id = c.operand_4_id,
                                    operand_5_id = c.operand_5_id,
                                    operand_6_id = c.operand_6_id,
                                    operand_7_id = c.operand_7_id,
                                    operand_8_id = c.operand_8_id,

                                    op1_curr_type = c.op1_curr_type,
                                    op2_curr_type = c.op2_curr_type,
                                    op3_curr_type = c.op3_curr_type,
                                    op4_curr_type = c.op4_curr_type,
                                    op5_curr_type = c.op5_curr_type,
                                    op6_curr_type = c.op6_curr_type,
                                    op7_curr_type = c.op7_curr_type,
                                    op8_curr_type = c.op8_curr_type,
                                    formula = c.formula,
                                    num_years = c.num_years,
                                    contributor_1_id = c.contributor_1,
                                    contributor_2_id = c.contributor_2,
                                    interval_type = c.interval_type,
                                    interval = c.interval,
                                    calculation_id = c.calculation_id,
                                    current_price = e.current_price,
                                    price_denominator = e.price_denominator,

                                    trading_denominator = e.trading_denominator_current,
                                    p_c_ratio = e.p_c_ratio_current,
                                    p_t_ratio = e.p_t_ratio_current,
                                    t_c_ratio = e.t_c_ratio_current,
                                    cumulative_denominator = e.cumulative_denominator_current,

                                    currency_denominator = e.currency_denominator,
                                    historic = c.historic,
                                    estimate = c.estimate,
                                    cumulative = c.cumulative,
                                    result_curr_type = c.result_curr_type,
                                    exc_from_cumulative = e.exc_from_cumulative,
                                    static_to_period = c.static_to_period
                                };

                    calcs = datastage.ToList<abc_calc>();

                    if (calcs.Count > 0)
                    {
                        assign_mom(calcs, ref entity_period_tbl, ref stagemom, batch_id, calc_type, calc_order);
                    }
                }
                else
                {

                    var datastage = from c in calculations
                                    join e in entity_period_tbl_info on c.key equals e.key
                                    where e.year != 9999
                                    & c.icalc_type == calc_type & c.calc_order == calc_order
                                    & ((c.historic == false && c.estimate == false) || (c.historic == true && e.e_a_flag == false) || (c.estimate == true && e.e_a_flag == true))
                                   & c.num_years == 0 && c.contributor_1 == 0 && c.interval_type == 0
                                    orderby e.entity_id, c.result_row_id, e.year
                                    select new abc_calc
                                    {
                                        calc_type = c.calc_type,
                                        entity_id = e.entity_id,
                                        parent_entity_id = e.parent_entity_id,
                                        year = e.year,
                                        period_id = e.period_id,
                                        workflow_stage = e.workflow_stage,
                                        contributor_id = e.contributor_id,
                                        result_row_id = c.result_row_id,
                                        operand_1_id = c.operand_1_id,
                                        operand_2_id = c.operand_2_id,
                                        operand_3_id = c.operand_3_id,
                                        operand_4_id = c.operand_4_id,
                                        operand_5_id = c.operand_5_id,
                                        operand_6_id = c.operand_6_id,
                                        operand_7_id = c.operand_7_id,
                                        operand_8_id = c.operand_8_id,

                                        op1_curr_type = c.op1_curr_type,
                                        op2_curr_type = c.op2_curr_type,
                                        op3_curr_type = c.op3_curr_type,
                                        op4_curr_type = c.op4_curr_type,
                                        op5_curr_type = c.op5_curr_type,
                                        op6_curr_type = c.op6_curr_type,
                                        op7_curr_type = c.op7_curr_type,
                                        op8_curr_type = c.op8_curr_type,
                                        formula = c.formula,
                                        num_years = c.num_years,
                                        contributor_1_id = c.contributor_1,
                                        contributor_2_id = c.contributor_2,
                                        interval_type = c.interval_type,
                                        interval = c.interval,
                                        calculation_id = c.calculation_id,
                                        current_price = e.current_price,
                                        price_denominator = e.price_denominator,
                                        trading_denominator = (c.estimate == true ? e.trading_denominator_current : e.trading_denominator_period_end),
                                        p_c_ratio = (c.estimate == true ? e.p_c_ratio_current : e.p_c_ratio_period_end),
                                        p_t_ratio = (c.estimate == true ? e.p_t_ratio_current : e.p_t_ratio_period_end),
                                        t_c_ratio = (c.estimate == true ? e.t_c_ratio_current : e.t_c_ratio_period_end),
                                        cumulative_denominator = (c.estimate == true ? e.cumulative_denominator_current : e.cumulative_denominator_period_end),
                                        currency_denominator = e.currency_denominator,
                                        historic = c.historic,
                                        estimate = c.estimate,
                                        cumulative = c.cumulative,
                                        e_a_flag = e.e_a_flag,
                                        result_curr_type = c.result_curr_type,
                                        exc_from_cumulative = e.exc_from_cumulative,
                                        op_zero_for_null = c.op_zero_for_null
                                    };
                    calcs = datastage.ToList<abc_calc>();

                    //assign normal
                    if (calcs.Count > 0)
                    {
                        assign_normal(is_parent, calcs, ref entity_period_tbl, ref stagenormal, batch_id, calc_type, calc_order);
                    }
                    datastage = from c in calculations
                                join e in entity_period_tbl_info on c.key equals e.key
                                where e.year != 9999
                                & c.icalc_type == calc_type & c.calc_order == calc_order
                                & ((c.historic == false && c.estimate == false) || (c.historic == true && e.e_a_flag == false) || (c.estimate == true && e.e_a_flag == true))
                                & c.num_years > 0 && c.contributor_1 == 0 && c.interval_type == 0
                                orderby e.entity_id, c.result_row_id, e.year
                                select new abc_calc
                                {
                                    calc_type = c.calc_type,
                                    entity_id = e.entity_id,
                                    parent_entity_id = e.parent_entity_id,
                                    year = e.year,
                                    period_id = e.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    result_row_id = c.result_row_id,
                                    operand_1_id = c.operand_1_id,
                                    operand_2_id = c.operand_2_id,
                                    operand_3_id = c.operand_3_id,
                                    operand_4_id = c.operand_4_id,
                                    operand_5_id = c.operand_5_id,
                                    operand_6_id = c.operand_6_id,
                                    operand_7_id = c.operand_7_id,
                                    operand_8_id = c.operand_8_id,

                                    op1_curr_type = c.op1_curr_type,
                                    op2_curr_type = c.op2_curr_type,
                                    op3_curr_type = c.op3_curr_type,
                                    op4_curr_type = c.op4_curr_type,
                                    op5_curr_type = c.op5_curr_type,
                                    op6_curr_type = c.op6_curr_type,
                                    op7_curr_type = c.op7_curr_type,
                                    op8_curr_type = c.op8_curr_type,
                                    formula = c.formula,
                                    num_years = c.num_years,
                                    contributor_1_id = c.contributor_1,
                                    contributor_2_id = c.contributor_2,
                                    interval_type = c.interval_type,
                                    interval = c.interval,
                                    calculation_id = c.calculation_id,
                                    current_price = e.current_price,
                                    price_denominator = e.price_denominator,
                                    trading_denominator = (c.estimate == true ? e.trading_denominator_current : e.trading_denominator_period_end),
                                    p_c_ratio = (c.estimate == true ? e.p_c_ratio_current : e.p_c_ratio_period_end),
                                    p_t_ratio = (c.estimate == true ? e.p_t_ratio_current : e.p_t_ratio_period_end),
                                    t_c_ratio = (c.estimate == true ? e.t_c_ratio_current : e.t_c_ratio_period_end),
                                    cumulative_denominator = (c.estimate == true ? e.cumulative_denominator_current : e.cumulative_denominator_period_end),
                                    currency_denominator = e.currency_denominator,
                                    historic = c.historic,
                                    estimate = c.estimate,
                                    cumulative = c.cumulative,
                                    e_a_flag = e.e_a_flag,
                                    result_curr_type = c.result_curr_type,
                                    exc_from_cumulative = e.exc_from_cumulative
                                };
                    calcs = datastage.ToList<abc_calc>();
                    //growths 
                    if (calcs.Count > 0)
                    {
                        assign_growths(calcs, ref entity_period_tbl, ref stagegrowths, batch_id, calc_type, calc_order);
                    }

                    datastage = from c in calculations
                                join e in entity_period_tbl_info on c.key equals e.key
                                where e.year != 9999
                                & c.icalc_type == calc_type & c.calc_order == calc_order
                                & ((c.historic == false && c.estimate == false) || (c.historic == true && e.e_a_flag == false) || (c.estimate == true && e.e_a_flag == true))
                                & c.num_years == 0 && c.contributor_1 > 0 && c.interval_type == 0
                                orderby e.entity_id, c.result_row_id, e.year
                                select new abc_calc
                                {
                                    calc_type = c.calc_type,
                                    entity_id = e.entity_id,
                                    parent_entity_id = e.parent_entity_id,
                                    year = e.year,
                                    period_id = e.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    result_row_id = c.result_row_id,
                                    operand_1_id = c.operand_1_id,
                                    operand_2_id = c.operand_2_id,
                                    operand_3_id = c.operand_3_id,
                                    operand_4_id = c.operand_4_id,
                                    operand_5_id = c.operand_5_id,
                                    operand_6_id = c.operand_6_id,
                                    operand_7_id = c.operand_7_id,
                                    operand_8_id = c.operand_8_id,

                                    op1_curr_type = c.op1_curr_type,
                                    op2_curr_type = c.op2_curr_type,
                                    op3_curr_type = c.op3_curr_type,
                                    op4_curr_type = c.op4_curr_type,
                                    op5_curr_type = c.op5_curr_type,
                                    op6_curr_type = c.op6_curr_type,
                                    op7_curr_type = c.op7_curr_type,
                                    op8_curr_type = c.op8_curr_type,
                                    formula = c.formula,
                                    num_years = c.num_years,
                                    contributor_1_id = c.contributor_1,
                                    contributor_2_id = c.contributor_2,
                                    interval_type = c.interval_type,
                                    interval = c.interval,
                                    calculation_id = c.calculation_id,
                                    current_price = e.current_price,
                                    price_denominator = e.price_denominator,
                                    trading_denominator = (c.estimate == true ? e.trading_denominator_current : e.trading_denominator_period_end),
                                    p_c_ratio = (c.estimate == true ? e.p_c_ratio_current : e.p_c_ratio_period_end),
                                    p_t_ratio = (c.estimate == true ? e.p_t_ratio_current : e.p_t_ratio_period_end),
                                    t_c_ratio = (c.estimate == true ? e.t_c_ratio_current : e.t_c_ratio_period_end),
                                    cumulative_denominator = (c.estimate == true ? e.cumulative_denominator_current : e.cumulative_denominator_period_end),
                                    currency_denominator = e.currency_denominator,
                                    historic = c.historic,
                                    estimate = c.estimate,
                                    cumulative = c.cumulative,
                                    e_a_flag = e.e_a_flag,
                                    result_curr_type = c.result_curr_type,
                                    exc_from_cumulative = e.exc_from_cumulative
                                };
                    calcs = datastage.ToList<abc_calc>();

                    if (calcs.Count > 0)
                    {
                        assign_cc(calcs, ref entity_period_tbl, ref stagecc, batch_id, calc_type, calc_order);
                    }
                    // MOMENTUM TBD
                    datastage = from c in calculations
                                join e in entity_period_tbl_info on c.key equals e.key
                                where e.year != 9999
                                & c.icalc_type == calc_type & c.calc_order == calc_order
                                & ((c.historic == false && c.estimate == false) || (c.historic == true && e.e_a_flag == false) || (c.estimate == true && e.e_a_flag == true))
                                & c.num_years == 0 && c.contributor_1 == 0 && c.interval_type > 0
                                orderby e.entity_id, c.result_row_id, e.year
                                select new abc_calc
                                {
                                    calc_type = c.calc_type,
                                    entity_id = e.entity_id,
                                    parent_entity_id = e.parent_entity_id,
                                    year = e.year,
                                    period_id = e.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    result_row_id = c.result_row_id,
                                    operand_1_id = c.operand_1_id,
                                    operand_2_id = c.operand_2_id,
                                    operand_3_id = c.operand_3_id,
                                    operand_4_id = c.operand_4_id,
                                    operand_5_id = c.operand_5_id,
                                    operand_6_id = c.operand_6_id,
                                    operand_7_id = c.operand_7_id,
                                    operand_8_id = c.operand_8_id,

                                    op1_curr_type = c.op1_curr_type,
                                    op2_curr_type = c.op2_curr_type,
                                    op3_curr_type = c.op3_curr_type,
                                    op4_curr_type = c.op4_curr_type,
                                    op5_curr_type = c.op5_curr_type,
                                    op6_curr_type = c.op6_curr_type,
                                    op7_curr_type = c.op7_curr_type,
                                    op8_curr_type = c.op8_curr_type,
                                    formula = c.formula,
                                    num_years = c.num_years,
                                    contributor_1_id = c.contributor_1,
                                    contributor_2_id = c.contributor_2,
                                    interval_type = c.interval_type,
                                    interval = c.interval,
                                    calculation_id = c.calculation_id,
                                    current_price = e.current_price,
                                    price_denominator = e.price_denominator,
                                    trading_denominator = (c.estimate == true ? e.trading_denominator_current : e.trading_denominator_period_end),
                                    p_c_ratio = (c.estimate == true ? e.p_c_ratio_current : e.p_c_ratio_period_end),
                                    p_t_ratio = (c.estimate == true ? e.p_t_ratio_current : e.p_t_ratio_period_end),
                                    t_c_ratio = (c.estimate == true ? e.t_c_ratio_current : e.t_c_ratio_period_end),
                                    cumulative_denominator = (c.estimate == true ? e.cumulative_denominator_current : e.cumulative_denominator_period_end),
                                    currency_denominator = e.currency_denominator,
                                    historic = c.historic,
                                    estimate = c.estimate,
                                    cumulative = c.cumulative,
                                    e_a_flag = e.e_a_flag,
                                    result_curr_type = c.result_curr_type,
                                    exc_from_cumulative = e.exc_from_cumulative
                                };
                    calcs = datastage.ToList<abc_calc>();

                    if (calcs.Count > 0)
                    {
                        assign_mom(calcs, ref  entity_period_tbl, ref stagemom, batch_id, calc_type, calc_order);
                    }
                }



                // get calculations in batch
                int i;
                List<calculation> lcalculationsnormal = new List<calculation>();
                List<calculation> lcalculationsgrowths = new List<calculation>();
                List<calculation> lcalculationscc = new List<calculation>();
                List<calculation> lcalculationsmom = new List<calculation>();
                List<calculation> lcalculationslib = new List<calculation>();

                // normal calcs
                if (calc_type == 1)
                {
                    for (i = 0; i < calculations.Count; i++)
                    {
                        if ((calculations[i].icalc_type == calc_type) && (calculations[i].calc_order == calc_order) && calculations[i].num_years == 0 && calculations[i].contributor_1 == 0 && calculations[i].interval_type == 0)
                        {
                            if (calculations[i].calc_lib > 0)
                            {
                                calculations[i].stage_type = 0;
                                lcalculationslib.Add(calculations[i]);
                            }
                            else
                                lcalculationsnormal.Add(calculations[i]);
                        }
                    }
                }
                else if (calc_type == 2)
                {
                    for (i = 0; i < calculations.Count; i++)
                    {
                        //if (calculations[i].icalc_type == calc_type && calculations[i].calc_order == calc_order && calculations[i].interval_type == 0 && ((calculations[i].contributor_1 == 0 && calculations[i].num_years == 0) || (calculations[i].contributor_1 > 0 && calculations[i].num_years > 0)))
                        if (calculations[i].icalc_type == calc_type && calculations[i].calc_order == calc_order)
                        {
                            if (calculations[i].calc_lib > 0)
                            {
                                calculations[i].stage_type = 0;
                                lcalculationslib.Add(calculations[i]);
                            }
                            else
                                lcalculationsnormal.Add(calculations[i]);
                        }
                    }
                }
                else
                {
                    for (i = 0; i < calculations.Count; i++)
                    {
                        if ((calculations[i].icalc_type == calc_type) && (calculations[i].calc_order == calc_order) && calculations[i].contributor_1 == 0 && calculations[i].interval_type == 0)
                        {
                            if (calculations[i].calc_lib > 0)
                            {
                                calculations[i].stage_type = 0;
                                lcalculationslib.Add(calculations[i]);
                            }
                            else
                                lcalculationsnormal.Add(calculations[i]);
                        }
                    }
                }
                // growth calcs
                if (calc_type == 1)
                {
                    for (i = 0; i < calculations.Count; i++)
                    {
                        if ((calculations[i].icalc_type == calc_type) && (calculations[i].calc_order == calc_order) && calculations[i].num_years > 0 && calculations[i].contributor_1 == 0 && calculations[i].interval_type == 0)
                        {
                            if (calculations[i].calc_lib > 0)
                            {
                                calculations[i].stage_type = 1;
                                lcalculationslib.Add(calculations[i]);
                            }
                            else
                                lcalculationsgrowths.Add(calculations[i]);
                        }
                    }
                }
                // cc calcs
                for (i = 0; i < calculations.Count; i++)
                {
                    if ((calculations[i].icalc_type == calc_type) && (calculations[i].calc_order == calc_order) && calculations[i].num_years == 0 && calculations[i].contributor_1 > 0 && calculations[i].interval_type == 0)
                    {
                        if (calculations[i].calc_lib > 0)
                        {
                            calculations[i].stage_type = 2;
                            lcalculationslib.Add(calculations[i]);
                        }
                        else
                            lcalculationscc.Add(calculations[i]);
                    }
                }
                // mom calcs
                for (i = 0; i < calculations.Count; i++)
                {
                    if ((calculations[i].icalc_type == calc_type) && (calculations[i].calc_order == calc_order) && calculations[i].num_years == 0 && calculations[i].contributor_1 == 0 && calculations[i].interval_type > 0)
                    {
                        if (calculations[i].calc_lib > 0)
                        {
                            calculations[i].stage_type = 3;
                            lcalculationslib.Add(calculations[i]);
                        }
                        else
                            lcalculationsmom.Add(calculations[i]);
                    }
                }
                // execute formula
                List<object> lresults = null;

                // library formulas
                for (i = 0; i < lcalculationslib.Count; i++)
                {
                    List<db_entity_period_tbl> customres;
                    custom_lib cl = new custom_lib();
                    if (lcalculationslib[i].stage_type == 0)
                    {
                        customres = cl.calculate(lcalculationslib[i].calc_lib, lcalculationslib[i].result_row_id, ref  stagenormal, audit_id, audit_date, template_id, calc_order, calc_type, batch_id, ref errors, ref certificate);
                    }
                    else if (lcalculationslib[i].stage_type == 1)
                    {
                        customres = cl.calculate(lcalculationslib[i].calc_lib, lcalculationslib[i].result_row_id, ref  stagegrowths, audit_id, audit_date, template_id, calc_order, calc_type, batch_id, ref errors, ref certificate);
                    }
                    else if (lcalculationslib[i].stage_type == 2)
                    {
                        customres = cl.calculate(lcalculationslib[i].calc_lib, lcalculationslib[i].result_row_id, ref  stagecc, audit_id, audit_date, template_id, calc_order, calc_type, batch_id, ref errors, ref certificate);
                    }
                    else
                    {
                        customres = cl.calculate(lcalculationslib[i].calc_lib, lcalculationslib[i].result_row_id, ref  stagemom, audit_id, audit_date, template_id, calc_order, calc_type, batch_id, ref errors, ref certificate);
                    }
                    assign_to_results(false, lcalculationslib[i], ref entity_period_tbl, null, customres, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationslib[i].calculation_id, lcalculationslib[i].historic, lcalculationslib[i].estimate, lcalculationslib[i].cumulative, lcalculationslib[i].static_to_period, entity_period_tbl_info);

                }

                // growths
                for (i = 0; i < lcalculationsgrowths.Count; i++)
                {
                    if (lcalculationsgrowths[i].average == true)
                    {
                        var cav = from v in stagegrowths
                                  where v.result_row_id == lcalculationsgrowths[i].result_row_id

                                  select new db_entity_period_tbl
                                  {

                                      //(isnull(value_1,0) + isnull(value_2,0)) /case when value_1 is not null and value_2 is not null then 2 when value_1 is null and value_2 is null then null else 1 end
                                      dvalue = v.value_1 != null && v.value_2 != null ? (v.value_1 + v.value_2) / 2 : (v.value_1 == null ? v.value_2 : v.value_1),

                                      entity_id = v.entity_id,
                                      row_id = v.result_row_id,
                                      year = v.year,
                                      period_id = v.period_id,
                                      workflow_stage = v.workflow_stage,
                                      contributor_id = v.contributor_id,
                                      acc_standard = 1,
                                      e_a_flag = v.e_a_flag,
                                      audit_id = audit_id,
                                      date_from = audit_date,
                                      user_id = 0,
                                      comment = "from service template " + template_id.ToString() + " : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                      date_to = new DateTime(9999, 09, 09),
                                      result_curr_type = v.result_curr_type,
                                      p_t_ratio = v.p_t_ratio,
                                      p_c_ratio = v.p_c_ratio,
                                      exc_from_cumulative = v.exc_from_cumulative
                                  };
                        var gresults = cav.ToList<db_entity_period_tbl>();
                        assign_to_results(false, lcalculationsgrowths[i], ref entity_period_tbl, null, gresults, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationsgrowths[i].calculation_id, lcalculationsgrowths[i].historic, lcalculationsgrowths[i].estimate, lcalculationsgrowths[i].cumulative, lcalculationsgrowths[i].static_to_period, entity_period_tbl_info);

                    }

                    else if (lcalculationsgrowths[i].num_years > 1)
                    {
                        var cgrowths = from v in stagegrowths
                                       where v.result_row_id == lcalculationsgrowths[i].result_row_id
                                       && v.value_1 > 0 && v.value_2 > 0
                                       select new db_entity_period_tbl
                                       {
                                           dvalue = ((decimal)Math.Pow(Math.Abs(System.Convert.ToDouble(v.value_2)) / Math.Abs(System.Convert.ToDouble(v.value_1)), 1.0 / System.Convert.ToDouble(v.num_years)) - 1),
                                           entity_id = v.entity_id,
                                           row_id = v.result_row_id,
                                           year = v.year,
                                           period_id = v.period_id,
                                           workflow_stage = v.workflow_stage,
                                           contributor_id = v.contributor_id,
                                           acc_standard = 1,
                                           e_a_flag = v.e_a_flag,
                                           audit_id = audit_id,
                                           date_from = audit_date,
                                           user_id = 0,
                                           comment = "from service template " + template_id.ToString() + " : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                           date_to = new DateTime(9999, 09, 09),
                                           result_curr_type = v.result_curr_type,
                                           p_t_ratio = v.p_t_ratio,
                                           p_c_ratio = v.p_c_ratio,
                                           exc_from_cumulative = v.exc_from_cumulative
                                       };
                        var gresults = cgrowths.ToList<db_entity_period_tbl>();
                        assign_to_results(false, lcalculationsgrowths[i], ref entity_period_tbl, null, gresults, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationsgrowths[i].calculation_id, lcalculationsgrowths[i].historic, lcalculationsgrowths[i].estimate, lcalculationsgrowths[i].cumulative, lcalculationsgrowths[i].static_to_period, entity_period_tbl_info);
                    }
                    else
                    {
                        var cgrowths = from v in stagegrowths
                                       where v.result_row_id == lcalculationsgrowths[i].result_row_id
                                       //&& v.value_1 != 0 && v.value_1 != null 
                                       && v.value_1 != 0 && v.value_1 != null && v.value_2 != null
                                       select new db_entity_period_tbl
                                       {
                                           dvalue = ((decimal)(System.Convert.ToDouble(v.value_2 - v.value_1) / Math.Abs(System.Convert.ToDouble(v.value_1)))),
                                           //dvalue = (v.value_2 - v.value_1) / v.value_1,

                                           entity_id = v.entity_id,
                                           row_id = v.result_row_id,
                                           year = v.year,
                                           period_id = v.period_id,
                                           workflow_stage = v.workflow_stage,
                                           contributor_id = v.contributor_id,
                                           acc_standard = 1,
                                           e_a_flag = v.e_a_flag,
                                           audit_id = audit_id,
                                           date_from = audit_date,
                                           user_id = 0,
                                           comment = "from service template " + template_id.ToString() + " : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                           date_to = new DateTime(9999, 09, 09),
                                           result_curr_type = v.result_curr_type,
                                           p_t_ratio = v.p_t_ratio,
                                           p_c_ratio = v.p_c_ratio,
                                           exc_from_cumulative = v.exc_from_cumulative
                                       };
                        var gresults = cgrowths.ToList<db_entity_period_tbl>();
                        assign_to_results(false, lcalculationsgrowths[i], ref entity_period_tbl, null, gresults, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationsgrowths[i].calculation_id, lcalculationsgrowths[i].historic, lcalculationsgrowths[i].estimate, lcalculationsgrowths[i].cumulative, lcalculationsgrowths[i].static_to_period, entity_period_tbl_info);



                    }
                }

                // cc 
                for (i = 0; i < lcalculationscc.Count; i++)
                {
                    var mcc = from v in stagecc
                              where v.result_row_id == lcalculationscc[i].result_row_id
                              && v.value_1 != 0
                              select new db_entity_period_tbl
                              {
                                  dvalue = ((v.value_2 / v.value_1) - 1),
                                  entity_id = v.entity_id,
                                  row_id = v.result_row_id,
                                  year = v.year,
                                  period_id = v.period_id,
                                  workflow_stage = v.workflow_stage,
                                  contributor_id = v.contributor_id,
                                  acc_standard = 1,
                                  e_a_flag = v.e_a_flag,
                                  audit_id = audit_id,
                                  date_from = audit_date,
                                  user_id = 0,
                                  comment = "from service template " + template_id.ToString() + " : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                  date_to = new DateTime(9999, 09, 09),
                                  result_curr_type = v.result_curr_type,
                                  p_t_ratio = v.p_t_ratio,
                                  p_c_ratio = v.p_c_ratio,
                                  exc_from_cumulative = v.exc_from_cumulative
                              };
                    var gmmcc = mcc.ToList<db_entity_period_tbl>();
                    assign_to_results(false, lcalculationscc[i], ref entity_period_tbl, null, gmmcc, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationscc[i].calculation_id, lcalculationscc[i].historic, lcalculationscc[i].estimate, lcalculationscc[i].cumulative, lcalculationscc[i].static_to_period, entity_period_tbl_info);

                }
                // momentum
                for (i = 0; i < lcalculationsmom.Count; i++)
                {
                    var mcc = from v in stagemom
                              where v.result_row_id == lcalculationsmom[i].result_row_id
                              && v.value_1 != 0 && v.value_2 / v.value_1 != null

                              select new db_entity_period_tbl
                              {
                                  dvalue = ((v.value_2 / v.value_1) - 1),
                                  entity_id = v.entity_id,
                                  row_id = v.result_row_id,
                                  year = v.year,
                                  period_id = v.period_id,
                                  workflow_stage = v.workflow_stage,
                                  contributor_id = v.contributor_id,
                                  acc_standard = 1,
                                  e_a_flag = v.e_a_flag,
                                  audit_id = audit_id,
                                  date_from = audit_date,
                                  user_id = 0,
                                  comment = "from service template " + template_id.ToString() + " : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                  date_to = new DateTime(9999, 09, 09),
                                  result_curr_type = v.result_curr_type,
                                  p_t_ratio = v.p_t_ratio,
                                  p_c_ratio = v.p_c_ratio,
                                  exc_from_cumulative = v.exc_from_cumulative
                              };
                    var gmmcc = mcc.ToList<db_entity_period_tbl>();
                    assign_to_results(false, lcalculationsmom[i], ref entity_period_tbl, null, gmmcc, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationsmom[i].calculation_id, lcalculationsmom[i].historic, lcalculationsmom[i].estimate, lcalculationsmom[i].cumulative, lcalculationsmom[i].static_to_period, entity_period_tbl_info);

                }
                // custom formula
                for (i = 0; i < lcalculationsnormal.Count; i++)
                {
                    //if (lcalculationsnormal[i].calc_lib > 0)
                    //{
                    //    List<db_entity_period_tbl> customres;
                    //    custom_lib cl = new custom_lib();
                    //    //calculate(int lib_id, int result_row_id, ref List<abc_calc> stage, int audit_id, DateTime audit_date, int template_id, int calc_order)


                    //    customres = cl.calculate(lcalculationsnormal[i].calc_lib, lcalculationsnormal[i].result_row_id, ref  stagenormal, audit_id, audit_date, template_id, calc_order, calc_type, batch_id, ref errors, ref certificate);
                    //    assign_to_results(false, lcalculationsnormal[i], ref allresults, null, customres, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationsnormal[i].calculation_id, lcalculationsnormal[i].historic, lcalculationsnormal[i].estimate, lcalculationsnormal[i].cumulative, lcalculationsnormal[i].static_to_period, entity_period_tbl_info);

                    //}
                    //else
                    //{
                    string where;
                    string preamble = "new (";
                    string fields = "";
                    if (lcalculationsnormal[i].cumulative == false)
                    {
                        fields = " as result,  calc_type as calc_type,  e_a_flag as e_a_flag,entity_id as entity_id, year as year, result_row_id as item_id,  period_id as period_id, workflow_stage as workflow_stage,  contributor_id as contributor_id )";
                    }
                    else
                    {
                        fields = " as result, exc_from_cumulative as exc_from_cumulative, parent_entity_id as  parent_entity_id, result_curr_type as result_curr_type, p_c_ratio as p_c_ratio,     p_t_ratio as p_t_ratio,calc_type as calc_type,  e_a_flag as e_a_flag,entity_id as entity_id, year as year, result_row_id as item_id,  period_id as period_id, workflow_stage as workflow_stage,  contributor_id as contributor_id )";
                    }
                    //where = "result_row_id=" + lcalculationsnormal[i].result_row_id;
                    where = "calculation_id=" + lcalculationsnormal[i].calculation_id;
                    if (lcalculationsnormal[i].linq_where != "")
                    {
                        where = where + "  " + lcalculationsnormal[i].linq_where;
                    }

                    try
                    {
                        var t = stagenormal.Where(where).Select(preamble + lcalculationsnormal[i].linq_formula + fields);
                        var oresults = t.Cast<object>();
                        lresults = oresults.ToList<object>();


                        assign_to_results(true, lcalculationsnormal[i], ref entity_period_tbl, lresults, null, audit_id, contributor_id, audit_date, false, template_id, batch_id, calc_type, calc_order, lcalculationsnormal[i].calculation_id, lcalculationsnormal[i].historic, lcalculationsnormal[i].estimate, lcalculationsnormal[i].cumulative, lcalculationsnormal[i].static_to_period, entity_period_tbl_info);

                    }
                    catch (Exception e)
                    {
                        calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "calc_template_batch", "Erorr executing formula : " + lcalculationsnormal[i].result_row_id.ToString() + ": " + lcalculationsnormal[i].linq_formula + " :  Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                        errors.Add(log_error);
                    }

                    //}
                }

            }

            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "calc_template_batch", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                errors.Add(log_error);
            }
        }

        void remove_row_from_all_results(ref List<db_entity_period_tbl> allresults, long item_id, bool bstatic, int template, int batch_id, int calc_type, int calc_order, long calculation_id, bool historic, bool estimate, bool cumulaive)
        {
            try
            {

                // if ths row has been calculated in a lower nbatch remove lower batch records
                if (historic == true)
                {
                    var cquery = from v in allresults
                                 where ((v.row_id != item_id) || (v.row_id == item_id && v.e_a_flag == true) || v.result_row == false)

                                 select new db_entity_period_tbl
                                 {
                                     result_row = v.result_row,
                                     value = v.value,
                                     dvalue = v.dvalue,
                                     p_c_ratio = v.p_c_ratio,
                                     p_t_ratio = v.p_t_ratio,
                                     exc_from_cumulative = v.exc_from_cumulative,
                                     result_curr_type = v.result_curr_type,
                                     parent_entity_id = v.parent_entity_id,
                                     entity_id = v.entity_id,
                                     row_id = v.row_id,
                                     year = v.year,
                                     period_id = v.period_id,
                                     workflow_stage = v.workflow_stage,
                                     contributor_id = v.contributor_id,
                                     acc_standard = 1,
                                     e_a_flag = v.e_a_flag,
                                     audit_id = v.audit_id,
                                     date_from = v.date_from,
                                     user_id = 0,
                                     comment = v.comment,
                                     date_to = new DateTime(9999, 09, 09),
                                     interval_type = v.interval_type,
                                     interval_val = v.interval_val,

                                 };
                    allresults = cquery.ToList<db_entity_period_tbl>();

                }
                else if (estimate == true)
                {
                    var cquery = from v in allresults
                                 where ((v.row_id != item_id) || (v.row_id == item_id && v.e_a_flag == false) || v.result_row == false)

                                 select new db_entity_period_tbl
                                 {
                                     result_row = v.result_row,
                                     value = v.value,
                                     dvalue = v.dvalue,
                                     p_c_ratio = v.p_c_ratio,
                                     p_t_ratio = v.p_t_ratio,
                                     exc_from_cumulative = v.exc_from_cumulative,
                                     result_curr_type = v.result_curr_type,
                                     parent_entity_id = v.parent_entity_id,
                                     entity_id = v.entity_id,
                                     row_id = v.row_id,
                                     year = v.year,
                                     period_id = v.period_id,
                                     workflow_stage = v.workflow_stage,
                                     contributor_id = v.contributor_id,
                                     acc_standard = 1,
                                     e_a_flag = v.e_a_flag,
                                     audit_id = v.audit_id,
                                     date_from = v.date_from,
                                     user_id = 0,
                                     comment = v.comment,
                                     date_to = new DateTime(9999, 09, 09),
                                     interval_type = v.interval_type,
                                     interval_val = v.interval_val,
                                 };
                    allresults = cquery.ToList<db_entity_period_tbl>();
                }
                else
                {
                    var cquery = from v in allresults
                                 where (v.row_id != item_id || v.result_row == false)

                                 select new db_entity_period_tbl
                                 {
                                     result_row = v.result_row,
                                     value = v.value,
                                     dvalue = v.dvalue,
                                     p_c_ratio = v.p_c_ratio,
                                     p_t_ratio = v.p_t_ratio,
                                     exc_from_cumulative = v.exc_from_cumulative,
                                     result_curr_type = v.result_curr_type,
                                     parent_entity_id = v.parent_entity_id,
                                     entity_id = v.entity_id,
                                     row_id = v.row_id,
                                     year = v.year,
                                     period_id = v.period_id,
                                     workflow_stage = v.workflow_stage,
                                     contributor_id = v.contributor_id,
                                     acc_standard = 1,
                                     e_a_flag = v.e_a_flag,
                                     audit_id = v.audit_id,
                                     date_from = v.date_from,
                                     user_id = 0,
                                     comment = v.comment,
                                     date_to = new DateTime(9999, 09, 09),
                                     interval_type = v.interval_type,
                                     interval_val = v.interval_val,
                                 };
                    allresults = cquery.ToList<db_entity_period_tbl>();
                }



            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "remove_row_from_all_results", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + "calc_id: " + calculation_id.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                errors.Add(log_error);
            }
        }

        void assign_to_results(bool dynamic, calculation lcalculation, ref List<db_entity_period_tbl> allresults, List<object> lresults, List<db_entity_period_tbl> gmcresults, int audit_id, long contributor_id, DateTime date_from, bool bstatic, int template, int batch_id, int calc_type, int calc_order, long calculation_id, bool historic, bool estimate, bool cumulative, bool static_to_period, List<entity_info> entity_period_tbl_info)
        {
            try
            {

                //cumulative = false;
                remove_row_from_all_results(ref allresults, lcalculation.result_row_id, false, template_id, batch_id, calc_type, calc_order, lcalculation.calculation_id, lcalculation.historic, lcalculation.estimate, lcalculation.cumulative);

                List<db_entity_period_tbl> cum_calcs = new List<db_entity_period_tbl>();
                List<db_entity_period_tbl> estatic_to_period = new List<db_entity_period_tbl>(); ;

                db_entity_period_tbl aresult;
                DateTime eda = new DateTime(9999, 09, 09);
                int i;

                bool e_a_flag = false;

                if (dynamic == false)
                {
                    for (i = 0; i < gmcresults.Count; i++)
                    {
                        if (gmcresults[i].dvalue == null)
                            // N/A
                            continue;
                        gmcresults[i].value = gmcresults[i].dvalue.ToString();
                        gmcresults[i].dvalue = gmcresults[i].dvalue;
                        allresults.Add(gmcresults[i]);


                        if (static_to_period == true)
                        {
                            estatic_to_period.Add(gmcresults[i]);
                        }
                        if (cumulative == true)


                            try
                            {
                                switch (gmcresults[i].result_curr_type)
                                {
                                    case 1:
                                        gmcresults[i].dcumvalue = gmcresults[i].p_c_ratio > 0 ? gmcresults[i].dvalue / gmcresults[i].p_c_ratio : null;
                                        break;
                                    case 2:
                                        gmcresults[i].dcumvalue = gmcresults[i].p_t_ratio > 0 ? gmcresults[i].dvalue / gmcresults[i].p_t_ratio : null;
                                        break;
                                    default:
                                        gmcresults[i].dcumvalue = gmcresults[i].dvalue;
                                        break;
                                }


                                cum_calcs.Add(gmcresults[i]);
                            }
                            catch (Exception e)
                            {

                            }


                    }
                }

                else
                {
                    for (i = 0; i < lresults.Count; i++)
                    {

                        var row = lresults[i];
                        var ttype = row.GetType();


                        var p0 = ttype.GetProperty("e_a_flag");
                        e_a_flag = (bool)(p0.GetValue(row));


                        aresult = new db_entity_period_tbl();
                        aresult.audit_id = audit_id;
                        aresult.date_from = date_from;
                        aresult.date_to = eda;
                        aresult.user_id = 0;
                        aresult.contributor_id = contributor_id;
                        aresult.acc_standard = 1;
                        p0 = ttype.GetProperty("calc_type");
                        aresult.comment = "from service template " + template_id.ToString() + " : batch: " + calc_order.ToString() + " type: " + (string)(p0.GetValue(row));
                        p0 = ttype.GetProperty("year");
                        aresult.year = (int)(p0.GetValue(row));
                        p0 = ttype.GetProperty("workflow_stage");
                        aresult.workflow_stage = (int)(p0.GetValue(row));
                        p0 = ttype.GetProperty("period_id");
                        aresult.period_id = (long)(p0.GetValue(row));
                        p0 = ttype.GetProperty("entity_id");
                        aresult.entity_id = (long)(p0.GetValue(row));
                        p0 = ttype.GetProperty("item_id");
                        aresult.row_id = (long)(p0.GetValue(row));

                        aresult.e_a_flag = e_a_flag;
                        p0 = ttype.GetProperty("result");
                        try
                        {
                            aresult.dvalue = (decimal)(p0.GetValue(row));
                            aresult.value = aresult.dvalue.ToString();
                            allresults.Add(aresult);

                            if (static_to_period == true)
                            {
                                estatic_to_period.Add(aresult);
                            }
                        }
                        catch
                        {
                            // N/A
                        }
                        if (cumulative == true)
                        {

                            p0 = ttype.GetProperty("exc_from_cumulative");
                            aresult.exc_from_cumulative = (bool)(p0.GetValue(row));
                            p0 = ttype.GetProperty("p_t_ratio");
                            aresult.p_t_ratio = (decimal)(p0.GetValue(row));
                            p0 = ttype.GetProperty("p_c_ratio");
                            aresult.p_c_ratio = (decimal)(p0.GetValue(row));
                            p0 = ttype.GetProperty("parent_entity_id");
                            aresult.parent_entity_id = (long)(p0.GetValue(row));
                            p0 = ttype.GetProperty("result_curr_type");
                            aresult.result_curr_type = (int)(p0.GetValue(row));
                            try
                            {
                                switch (aresult.result_curr_type)
                                {
                                    case 1:
                                        aresult.dcumvalue = aresult.p_c_ratio > 0 ? aresult.dvalue / aresult.p_c_ratio : null;
                                        break;
                                    case 2:
                                        aresult.dcumvalue = aresult.p_t_ratio > 0 ? aresult.dvalue / aresult.p_t_ratio : null;
                                        break;
                                    default:
                                        aresult.dcumvalue = aresult.dvalue;
                                        break;
                                }


                                cum_calcs.Add(aresult);
                            }
                            catch
                            {

                            }

                        }



                    }
                }

                List<db_entity_period_tbl> cumres = new List<db_entity_period_tbl>();

                if (cumulative == true)
                {
                    var queryi = from v in cum_calcs
                                 group v by new { v.parent_entity_id, v.year, v.period_id, v.workflow_stage, v.contributor_id, v.row_id, v.comment, v.date_to } into g
                                 select new db_entity_period_tbl
                                 {
                                     parent_entity_id = g.Key.parent_entity_id,
                                     year = g.Key.year,
                                     period_id = g.Key.period_id,
                                     workflow_stage = g.Key.workflow_stage,
                                     contributor_id = g.Key.contributor_id,
                                     e_a_flag = g.Min(x => x.e_a_flag)
                                 };

                    List<db_entity_period_tbl> cpinfo;
                    cpinfo = queryi.ToList<db_entity_period_tbl>();

                    var query = from v in cum_calcs
                                join c in cpinfo on new { j1 = v.parent_entity_id, j2 = v.year, j3 = v.period_id, j4 = v.workflow_stage, j5 = v.contributor_id }
                                equals new { j1 = c.parent_entity_id, j2 = c.year, j3 = c.period_id, j4 = c.workflow_stage, j5 = c.contributor_id }
                                where v.exc_from_cumulative == false && v.dcumvalue != null
                                group v by new { v.parent_entity_id, v.year, v.period_id, v.workflow_stage, v.contributor_id, v.row_id, v.comment, v.date_to, c.e_a_flag } into g

                                select new db_entity_period_tbl
                                {
                                    value = g.Sum(x => x.dcumvalue).ToString(),
                                    entity_id = g.Key.parent_entity_id,
                                    year = g.Key.year,
                                    period_id = g.Key.period_id,
                                    workflow_stage = g.Key.workflow_stage,
                                    contributor_id = g.Key.contributor_id,
                                    row_id = g.Key.row_id,
                                    acc_standard = 1,
                                    date_from = audit_date,
                                    date_to = g.Key.date_to,
                                    comment = g.Key.comment,
                                    e_a_flag = g.Key.e_a_flag,
                                    user_id = 0,
                                    audit_id = audit_id
                                };

                    cumres = query.ToList<db_entity_period_tbl>();
                    for (i = 0; i < cumres.Count; i++)
                    {
                        if (cumres[i].value != "")
                        {
                            allresults.Add(cumres[i]);

                        }
                        //Debug.Print(cumres[i].entity_id.ToString() + ":" + cumres[i].row_id.ToString() + ":" + cumres[i].year.ToString() + ":" + cumres[i].workflow.ToString());
                    }

                }

                if (static_to_period == true)
                {

                    var query = from v in entity_period_tbl_info
                                join e in estatic_to_period on v.entity_id equals e.entity_id
                                where e.workflow_stage == v.workflow_stage && e.contributor_id == v.contributor_id && v.year != 9999
                                && ((historic == false && estimate == false) || (historic == true && v.e_a_flag == false) || (estimate == true && v.e_a_flag == true))

                                select new db_entity_period_tbl
                                {
                                    value = e.value,
                                    dvalue = e.dvalue,
                                    entity_id = e.entity_id,
                                    year = v.year,
                                    period_id = v.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    row_id = lcalculation.num_years,
                                    acc_standard = 1,
                                    date_from = audit_date,
                                    date_to = e.date_to,
                                    comment = e.comment,
                                    e_a_flag = v.e_a_flag,
                                    user_id = 0,
                                    audit_id = audit_id

                                };
                    var spres = query.ToList<db_entity_period_tbl>();
                    for (i = 0; i < spres.Count; i++)
                    {
                        allresults.Add(spres[i]);

                    }

                    if (cumulative == true)
                    {
                        var query1 = from v in entity_period_tbl_info
                                     where v.year != 9999
                                       && ((historic == false && estimate == false) || (historic == true && v.e_a_flag == false) || (estimate == true && v.e_a_flag == true))

                                     group v by new { v.parent_entity_id, v.year, v.period_id, v.workflow_stage, v.contributor_id } into g
                                     select new entity_info
                                     {

                                         entity_id = g.Key.parent_entity_id,
                                         year = g.Key.year,
                                         period_id = g.Key.period_id,
                                         workflow_stage = g.Key.workflow_stage,
                                         contributor_id = g.Key.contributor_id,
                                         //e_a_flag = g.Max(x => x.e_a_flag),
                                         e_a_flag = g.Min(x => x.e_a_flag)
                                     };
                        var spresp = query1.ToList<entity_info>();
                        query = from v in spresp
                                join e in cumres on v.entity_id equals e.entity_id
                                where e.workflow_stage == v.workflow_stage && e.contributor_id == v.contributor_id && v.year != 9999
                                where e.value != ""
                                select new db_entity_period_tbl
                                {
                                    value = e.value,
                                    dvalue = e.dvalue,
                                    entity_id = e.entity_id,
                                    year = v.year,
                                    period_id = v.period_id,
                                    workflow_stage = e.workflow_stage,
                                    contributor_id = e.contributor_id,
                                    row_id = lcalculation.num_years,
                                    acc_standard = 1,
                                    date_from = audit_date,
                                    date_to = e.date_to,
                                    comment = e.comment,
                                    e_a_flag = e.e_a_flag,
                                    user_id = 0,
                                    audit_id = audit_id

                                };
                        spres = query.ToList<db_entity_period_tbl>();
                        for (i = 0; i < spres.Count; i++)
                        {
                            Debug.Print(spres[i].entity_id.ToString() + ":" + spres[i].row_id.ToString() + ":" + spres[i].year.ToString() + spres[i].workflow_stage.ToString());

                            allresults.Add(spres[i]);

                        }

                    }
                }



            }

            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "assign_to_results", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + "calc_id: " + calculation_id.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);

                errors.Add(log_error);
            }
        }


        void assign_normal(bool is_parent, List<abc_calc> calcs, ref List<db_entity_period_tbl> entity_period_tbl, ref List<abc_calc> stage, int batch_id, int calc_type, int calc_order)
        {
            try
            {



                List<abc_calc> results;
                int i;

                //entity_period_tbl.Clear();
                //db_entity_period_tbl k = new db_entity_period_tbl();
                //k.workflow_stage=8;
                //k.year=2012;
                //k.period_id=1;
                //k.contributor_id=1;
                //k.entity_id=23007;
                //    k.row_id=1011;
                //    k.dvalue = 888;
                //allresults.Add(k);




                var query = from v in calcs
                            join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_1_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                            equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                            from s in ts.DefaultIfEmpty()
                            where v.operand_1_id != 0
                            select new abc_calc
                            {
                                value_1 = s == null ? null : s.dvalue,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,

                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                results = query.ToList<abc_calc>();

                for (
                    i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }



                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_2_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                    equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()

                        where v.operand_2_id != 0
                        select new abc_calc
                        {

                            value_2 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }

                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_3_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                     equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()
                        where v.operand_3_id != 0
                        select new abc_calc
                        {

                            value_3 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_4_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                     equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()
                        where v.operand_4_id != 0
                        select new abc_calc
                        {

                            value_4 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_5_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                     equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()
                        where v.operand_5_id != 0
                        select new abc_calc
                        {

                            value_5 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_6_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                    equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()
                        where v.operand_6_id != 0
                        select new abc_calc
                        {

                            value_6 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_7_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                     equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()
                        where v.operand_7_id != 0
                        select new abc_calc
                        {

                            value_7 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_8_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                    equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage } into ts
                        from s in ts.DefaultIfEmpty()
                        where v.operand_8_id != 0
                        select new abc_calc
                        {

                            value_8 = s == null ? null : s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                //////////////////////////////////
                if (is_parent == false)
                {
                    query = from v in calcs

                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_1_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_1_id != 0 && v.op1_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_1 = v.op1_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }


                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_2_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_2_id != 0 && v.op2_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_2 = v.op2_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_3_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_3_id != 0 && v.op3_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_3 = v.op3_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_4_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_4_id != 0 && v.op4_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_4 = v.op4_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_5_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_5_id != 0 && v.op5_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_5 = v.op5_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_6_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_6_id != 0 && v.op6_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_6 = v.op6_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_7_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0 && v.operand_7_id != 0 && v.op7_curr_type < 2
                            select new abc_calc
                            {
                                pvalue_7 = v.op7_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                    query = from v in calcs
                            join p in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_8_id, j3 = v.parent_entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j12 = p.interval_type, j1 = p.row_id, j3 = p.entity_id, j4 = p.year, j5 = p.period_id, j6 = p.contributor_id, j8 = p.workflow_stage }
                            where v.p_c_ratio != 0
                           && v.operand_8_id != 0 && v.op8_curr_type < 2

                            select new abc_calc
                            {
                                pvalue_8 = v.op8_curr_type == 0 ? p.dvalue : p.dvalue * v.p_c_ratio,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                operand_3_id = v.operand_3_id,
                                operand_4_id = v.operand_4_id,
                                operand_5_id = v.operand_5_id,
                                operand_6_id = v.operand_6_id,
                                operand_7_id = v.operand_7_id,
                                operand_8_id = v.operand_8_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                op3_curr_type = v.op3_curr_type,
                                op4_curr_type = v.op4_curr_type,
                                op5_curr_type = v.op5_curr_type,
                                op6_curr_type = v.op6_curr_type,
                                op7_curr_type = v.op7_curr_type,
                                op8_curr_type = v.op8_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period,
                                op_zero_for_null = v.op_zero_for_null

                            };
                    results = query.ToList<abc_calc>();

                    for (
                        i = 0; i <= results.Count - 1; i++)
                    {
                        stage.Add(results[i]);
                    }
                }



                if (is_parent == false)
                    query = from v in stage
                            group v by new { v.op_zero_for_null, v.static_to_period, v.exc_from_cumulative, v.result_curr_type, v.e_a_flag, v.historic, v.estimate, v.cumulative, v.cumulative_denominator, v.currency_denominator, v.formula, v.current_price, v.price_denominator, v.trading_denominator, v.p_c_ratio, v.p_t_ratio, v.t_c_ratio, v.calc_type, v.op1_curr_type, v.op2_curr_type, v.op3_curr_type, v.op4_curr_type, v.op5_curr_type, v.op6_curr_type, v.op7_curr_type, v.op8_curr_type, v.parent_entity_id, v.entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.contributor_1_id, v.contributor_2_id, v.operand_1_id, v.operand_2_id, v.operand_3_id, v.operand_4_id, v.operand_5_id, v.operand_6_id, v.operand_7_id, v.operand_8_id, v.calculation_id } into g
                            select new abc_calc
                            {
                                value_1 = g.Max(x => x.value_1) == null ? g.Max(x => x.pvalue_1) : g.Max(x => x.value_1),
                                value_2 = g.Max(x => x.value_2) == null ? g.Max(x => x.pvalue_2) : g.Max(x => x.value_2),
                                value_3 = g.Max(x => x.value_3) == null ? g.Max(x => x.pvalue_3) : g.Max(x => x.value_3),
                                value_4 = g.Max(x => x.value_4) == null ? g.Max(x => x.pvalue_4) : g.Max(x => x.value_4),
                                value_5 = g.Max(x => x.value_5) == null ? g.Max(x => x.pvalue_5) : g.Max(x => x.value_5),
                                value_6 = g.Max(x => x.value_6) == null ? g.Max(x => x.pvalue_6) : g.Max(x => x.value_6),
                                value_7 = g.Max(x => x.value_7) == null ? g.Max(x => x.pvalue_7) : g.Max(x => x.value_7),
                                value_8 = g.Max(x => x.value_8) == null ? g.Max(x => x.pvalue_8) : g.Max(x => x.value_8),
                                calc_type = g.Key.calc_type,
                                entity_id = g.Key.entity_id,
                                parent_entity_id = g.Key.parent_entity_id,
                                year = g.Key.year,
                                period_id = g.Key.period_id,
                                workflow_stage = g.Key.workflow_stage,
                                contributor_id = g.Key.contributor_id,
                                result_row_id = g.Key.result_row_id,
                                op1_curr_type = g.Key.op1_curr_type,
                                op2_curr_type = g.Key.op2_curr_type,
                                op3_curr_type = g.Key.op3_curr_type,
                                op4_curr_type = g.Key.op4_curr_type,
                                op5_curr_type = g.Key.op5_curr_type,
                                op6_curr_type = g.Key.op6_curr_type,
                                op7_curr_type = g.Key.op7_curr_type,
                                op8_curr_type = g.Key.op8_curr_type,
                                formula = g.Key.formula,
                                calculation_id = g.Key.calculation_id,
                                current_price = g.Key.current_price,
                                price_denominator = g.Key.price_denominator,
                                trading_denominator = g.Key.trading_denominator,
                                p_c_ratio = g.Key.p_c_ratio,
                                p_t_ratio = g.Key.p_t_ratio,
                                t_c_ratio = g.Key.t_c_ratio,
                                cumulative_denominator = g.Key.cumulative_denominator,
                                currency_denominator = g.Key.currency_denominator,
                                historic = g.Key.historic,
                                estimate = g.Key.estimate,
                                cumulative = g.Key.cumulative,
                                e_a_flag = g.Key.e_a_flag,
                                num_years = g.Key.num_years,
                                result_curr_type = g.Key.result_curr_type,
                                exc_from_cumulative = g.Key.exc_from_cumulative,
                                static_to_period = g.Key.static_to_period,
                                op_zero_for_null = g.Key.op_zero_for_null
                            };
                else
                    query = from v in stage
                            group v by new { v.op_zero_for_null, v.static_to_period, v.exc_from_cumulative, v.result_curr_type, v.e_a_flag, v.historic, v.estimate, v.cumulative, v.cumulative_denominator, v.currency_denominator, v.formula, v.current_price, v.price_denominator, v.trading_denominator, v.p_c_ratio, v.p_t_ratio, v.t_c_ratio, v.calc_type, v.op1_curr_type, v.op2_curr_type, v.op3_curr_type, v.op4_curr_type, v.op5_curr_type, v.op6_curr_type, v.op7_curr_type, v.op8_curr_type, v.parent_entity_id, v.entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.contributor_1_id, v.contributor_2_id, v.operand_1_id, v.operand_2_id, v.operand_3_id, v.operand_4_id, v.operand_5_id, v.operand_6_id, v.operand_7_id, v.operand_8_id, v.calculation_id } into g
                            select new abc_calc
                            {
                                value_1 = g.Max(x => x.value_1),
                                value_2 = g.Max(x => x.value_2),
                                value_3 = g.Max(x => x.value_3),
                                value_4 = g.Max(x => x.value_4),
                                value_5 = g.Max(x => x.value_5),
                                value_6 = g.Max(x => x.value_6),
                                value_7 = g.Max(x => x.value_7),
                                value_8 = g.Max(x => x.value_8),
                                calc_type = g.Key.calc_type,
                                entity_id = g.Key.entity_id,
                                parent_entity_id = g.Key.parent_entity_id,
                                year = g.Key.year,
                                period_id = g.Key.period_id,
                                workflow_stage = g.Key.workflow_stage,
                                contributor_id = g.Key.contributor_id,
                                result_row_id = g.Key.result_row_id,
                                op1_curr_type = g.Key.op1_curr_type,
                                op2_curr_type = g.Key.op2_curr_type,
                                op3_curr_type = g.Key.op3_curr_type,
                                op4_curr_type = g.Key.op4_curr_type,
                                op5_curr_type = g.Key.op5_curr_type,
                                op6_curr_type = g.Key.op6_curr_type,
                                op7_curr_type = g.Key.op7_curr_type,
                                op8_curr_type = g.Key.op8_curr_type,
                                formula = g.Key.formula,
                                calculation_id = g.Key.calculation_id,
                                current_price = g.Key.current_price,
                                price_denominator = g.Key.price_denominator,
                                trading_denominator = g.Key.trading_denominator,
                                p_c_ratio = g.Key.p_c_ratio,
                                p_t_ratio = g.Key.p_t_ratio,
                                t_c_ratio = g.Key.t_c_ratio,
                                cumulative_denominator = g.Key.cumulative_denominator,
                                currency_denominator = g.Key.currency_denominator,
                                historic = g.Key.historic,
                                estimate = g.Key.estimate,
                                cumulative = g.Key.cumulative,
                                e_a_flag = g.Key.e_a_flag,
                                num_years = g.Key.num_years,
                                result_curr_type = g.Key.result_curr_type,
                                exc_from_cumulative = g.Key.exc_from_cumulative,
                                static_to_period = g.Key.static_to_period,
                                op_zero_for_null = g.Key.op_zero_for_null
                            };
                stage = query.ToList<abc_calc>();

                // see if null operands should be zero
                query = from v in stage
                        select new abc_calc
                        {
                            //value_1 = v.value_1 == null && v.op_zero_for_null == true ? 0 : v.value_1,
                            //value_2 = v.value_2 == null && v.op_zero_for_null == true ? 0 : v.value_2,
                            //value_3 = v.value_3 == null && v.op_zero_for_null == true ? 0 : v.value_3,
                            //value_4 = v.value_4 == null && v.op_zero_for_null == true ? 0 : v.value_4,
                            //value_5 = v.value_5 == null && v.op_zero_for_null == true ? 0 : v.value_5,
                            //value_6 = v.value_6 == null && v.op_zero_for_null == true ? 0 : v.value_6,
                            //value_7 = v.value_7 == null && v.op_zero_for_null == true ? 0 : v.value_7,
                            //value_8 = v.value_8 == null && v.op_zero_for_null == true ? 0 : v.value_8,
                            value_1 = v.value_1 ,
                            value_2 = v.value_2 ,
                            value_3 = v.value_3,
                            value_4 = v.value_4 ,
                            value_5 = v.value_5 ,
                            value_6 = v.value_6,
                            value_7 = v.value_7 ,
                            value_8 = v.value_8,
                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            //formula = v.formula,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            num_years = v.num_years,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period,
                            op_zero_for_null = v.op_zero_for_null
                        };
                stage = query.ToList<abc_calc>();


            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "assign_normal", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                errors.Add(log_error);

            }
        }

        void assign_growths(List<abc_calc> calcs, ref List<db_entity_period_tbl> entity_period_tbl, ref List<abc_calc> stage, int batch_id, int calc_type, int calc_order)
        {
            try
            {

                // populate values from db (ept)
                // straight calc   
                List<abc_calc> results;
                int i;

                var query = from v in calcs
                            join s in entity_period_tbl on new { j10 = v.interval_type, j4 = v.year - v.num_years, j1 = v.operand_1_id, j3 = v.entity_id, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                            equals new { j10 = s.interval_type, j4 = s.year, j1 = s.row_id, j3 = s.entity_id, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage }



                            select new abc_calc
                            {

                                value_1 = s.dvalue,


                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,

                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,

                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period
                            };
                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j10 = v.interval_type, j1 = v.operand_2_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j10 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage }


                        select new abc_calc
                        {
                            value_2 = s.dvalue,

                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,

                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,

                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period
                        };
                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }

                query = from v in stage
                        group v by new { v.static_to_period, v.exc_from_cumulative, v.result_curr_type, v.e_a_flag, v.historic, v.estimate, v.cumulative, v.cumulative_denominator, v.currency_denominator, v.formula, v.current_price, v.price_denominator, v.trading_denominator, v.p_c_ratio, v.p_t_ratio, v.t_c_ratio, v.calc_type, v.op1_curr_type, v.op2_curr_type, v.op3_curr_type, v.op4_curr_type, v.op5_curr_type, v.op6_curr_type, v.op7_curr_type, v.op8_curr_type, v.parent_entity_id, v.entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.contributor_1_id, v.contributor_2_id, v.operand_1_id, v.operand_2_id, v.operand_3_id, v.operand_4_id, v.operand_5_id, v.operand_6_id, v.operand_7_id, v.operand_8_id, v.calculation_id } into g
                        select new abc_calc
                        {
                            value_1 = g.Max(x => x.value_1),
                            value_2 = g.Max(x => x.value_2),
                            calc_type = g.Key.calc_type,
                            entity_id = g.Key.entity_id,
                            parent_entity_id = g.Key.parent_entity_id,
                            year = g.Key.year,
                            period_id = g.Key.period_id,
                            workflow_stage = g.Key.workflow_stage,
                            contributor_id = g.Key.contributor_id,
                            result_row_id = g.Key.result_row_id,
                            op1_curr_type = g.Key.op1_curr_type,
                            op2_curr_type = g.Key.op2_curr_type,
                            op3_curr_type = g.Key.op3_curr_type,
                            op4_curr_type = g.Key.op4_curr_type,
                            op5_curr_type = g.Key.op5_curr_type,
                            op6_curr_type = g.Key.op6_curr_type,
                            op7_curr_type = g.Key.op7_curr_type,
                            op8_curr_type = g.Key.op8_curr_type,
                            formula = g.Key.formula,
                            calculation_id = g.Key.calculation_id,
                            current_price = g.Key.current_price,
                            price_denominator = g.Key.price_denominator,
                            trading_denominator = g.Key.trading_denominator,
                            p_c_ratio = g.Key.p_c_ratio,
                            p_t_ratio = g.Key.p_t_ratio,
                            t_c_ratio = g.Key.t_c_ratio,
                            cumulative_denominator = g.Key.cumulative_denominator,
                            currency_denominator = g.Key.currency_denominator,
                            historic = g.Key.historic,
                            estimate = g.Key.estimate,
                            cumulative = g.Key.cumulative,
                            e_a_flag = g.Key.e_a_flag,
                            num_years = g.Key.num_years,
                            result_curr_type = g.Key.result_curr_type,
                            exc_from_cumulative = g.Key.exc_from_cumulative,
                            static_to_period = g.Key.static_to_period
                        };
                stage = query.ToList<abc_calc>();

            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "assign_growths", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                errors.Add(log_error);

            }
        }
        void assign_cc(List<abc_calc> calcs, ref List<db_entity_period_tbl> entity_period_tbl, ref List<abc_calc> stage, int batch_id, int calc_type, int calc_order)
        {
            try
            {

                // populate values from db (ept)
                // straight calc   
                List<abc_calc> results;
                int i;

                var query = from v in calcs
                            join s in entity_period_tbl on new { j2 = v.contributor_2_id, j1 = v.operand_1_id, j3 = v.entity_id, j5 = v.period_id, j6 = v.year, j8 = v.workflow_stage }
                       equals new { j2 = s.contributor_id, j1 = s.row_id, j3 = s.entity_id, j5 = s.period_id, j6 = s.year, j8 = s.workflow_stage }
                            where s.interval_type == 0

                            select new abc_calc
                            {
                                value_1 = s.dvalue,
                                calc_type = v.calc_type,
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                workflow_stage = v.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period

                            };
                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j12 = v.interval_type, j1 = v.operand_2_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }

                    equals new { j12 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage }


                        select new abc_calc
                        {

                            value_2 = s.dvalue,


                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period
                        };

                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in stage
                        group v by new { v.static_to_period, v.exc_from_cumulative, v.result_curr_type, v.e_a_flag, v.historic, v.estimate, v.cumulative, v.cumulative_denominator, v.currency_denominator, v.formula, v.current_price, v.price_denominator, v.trading_denominator, v.p_c_ratio, v.p_t_ratio, v.t_c_ratio, v.calc_type, v.op1_curr_type, v.op2_curr_type, v.op3_curr_type, v.op4_curr_type, v.op5_curr_type, v.op6_curr_type, v.op7_curr_type, v.op8_curr_type, v.parent_entity_id, v.entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.contributor_1_id, v.contributor_2_id, v.operand_1_id, v.operand_2_id, v.operand_3_id, v.operand_4_id, v.operand_5_id, v.operand_6_id, v.operand_7_id, v.operand_8_id, v.calculation_id } into g
                        select new abc_calc
                        {
                            value_1 = g.Max(x => x.value_1),
                            value_2 = g.Max(x => x.value_2),
                            calc_type = g.Key.calc_type,
                            entity_id = g.Key.entity_id,
                            parent_entity_id = g.Key.parent_entity_id,
                            year = g.Key.year,
                            period_id = g.Key.period_id,
                            workflow_stage = g.Key.workflow_stage,
                            contributor_id = g.Key.contributor_id,
                            result_row_id = g.Key.result_row_id,
                            op1_curr_type = g.Key.op1_curr_type,
                            op2_curr_type = g.Key.op2_curr_type,
                            op3_curr_type = g.Key.op3_curr_type,
                            op4_curr_type = g.Key.op4_curr_type,
                            op5_curr_type = g.Key.op5_curr_type,
                            op6_curr_type = g.Key.op6_curr_type,
                            op7_curr_type = g.Key.op7_curr_type,
                            op8_curr_type = g.Key.op8_curr_type,
                            formula = g.Key.formula,
                            calculation_id = g.Key.calculation_id,
                            current_price = g.Key.current_price,
                            price_denominator = g.Key.price_denominator,
                            trading_denominator = g.Key.trading_denominator,
                            p_c_ratio = g.Key.p_c_ratio,
                            p_t_ratio = g.Key.p_t_ratio,
                            t_c_ratio = g.Key.t_c_ratio,
                            cumulative_denominator = g.Key.cumulative_denominator,
                            currency_denominator = g.Key.currency_denominator,
                            historic = g.Key.historic,
                            estimate = g.Key.estimate,
                            cumulative = g.Key.cumulative,
                            e_a_flag = g.Key.e_a_flag,
                            num_years = g.Key.num_years,
                            result_curr_type = g.Key.result_curr_type,
                            exc_from_cumulative = g.Key.exc_from_cumulative,
                            static_to_period = g.Key.static_to_period
                        };

                stage = query.ToList<abc_calc>();
            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "assign_cc", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                errors.Add(log_error);

            }
        }
        void assign_mom(List<abc_calc> calcs, ref List<db_entity_period_tbl> entity_period_tbl, ref List<abc_calc> stage, int batch_id, int calc_type, int calc_order)
        {
            try
            {


                List<abc_calc> results;
                int i;

                var query = from v in calcs
                            join s in entity_period_tbl on new { j9 = v.interval_type, j10 = v.interval, j1 = v.operand_1_id, j2 = v.contributor_id, j3 = v.entity_id, j5 = v.period_id, j6 = v.year, j8 = v.workflow_stage }
                       equals new { j9 = s.interval_type, j10 = s.interval_val, j1 = s.row_id, j2 = s.contributor_id, j3 = s.entity_id, j5 = s.period_id, j6 = s.year, j8 = s.workflow_stage }


                            where s.interval_type == v.interval_type && s.interval_val == v.interval

                            select new abc_calc
                            {
                                value_1 = s.dvalue,
                                calc_type = v.calc_type,
                                entity_id = s.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = s.period_id,
                                workflow_stage = s.workflow_stage,
                                contributor_id = v.contributor_id,
                                result_row_id = v.result_row_id,
                                operand_1_id = v.operand_1_id,
                                operand_2_id = v.operand_2_id,
                                op1_curr_type = v.op1_curr_type,
                                op2_curr_type = v.op2_curr_type,
                                formula = v.formula,
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                interval_type = v.interval_type,
                                interval = v.interval,
                                calculation_id = v.calculation_id,
                                current_price = v.current_price,
                                price_denominator = v.price_denominator,
                                trading_denominator = v.trading_denominator_current,
                                p_c_ratio = v.p_c_ratio,
                                p_t_ratio = v.p_t_ratio,
                                t_c_ratio = v.t_c_ratio,
                                cumulative_denominator = v.cumulative_denominator,
                                currency_denominator = v.currency_denominator,
                                historic = v.historic,
                                estimate = v.estimate,
                                cumulative = v.cumulative,
                                e_a_flag = v.e_a_flag,
                                result_curr_type = v.result_curr_type,
                                exc_from_cumulative = v.exc_from_cumulative,
                                static_to_period = v.static_to_period
                            };
                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in calcs
                        join s in entity_period_tbl on new { j10 = v.default_interval_type, j1 = v.operand_2_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j10 = s.interval_type, j1 = s.row_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j8 = s.workflow_stage }


                        select new abc_calc
                        {
                            value_2 = s.dvalue,
                            calc_type = v.calc_type,
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            workflow_stage = v.workflow_stage,
                            contributor_id = v.contributor_id,
                            result_row_id = v.result_row_id,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            formula = v.formula,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            interval_type = v.interval_type,
                            interval = v.interval,
                            calculation_id = v.calculation_id,
                            current_price = v.current_price,
                            price_denominator = v.price_denominator,
                            trading_denominator = v.trading_denominator_current,
                            p_c_ratio = v.p_c_ratio,
                            p_t_ratio = v.p_t_ratio,
                            t_c_ratio = v.t_c_ratio,
                            cumulative_denominator = v.cumulative_denominator,
                            currency_denominator = v.currency_denominator,
                            historic = v.historic,
                            estimate = v.estimate,
                            cumulative = v.cumulative,
                            e_a_flag = v.e_a_flag,
                            result_curr_type = v.result_curr_type,
                            exc_from_cumulative = v.exc_from_cumulative,
                            static_to_period = v.static_to_period
                        };
                results = query.ToList<abc_calc>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    stage.Add(results[i]);
                }
                query = from v in stage
                        group v by new { v.static_to_period, v.exc_from_cumulative, v.result_curr_type, v.e_a_flag, v.historic, v.estimate, v.cumulative, v.cumulative_denominator, v.currency_denominator, v.formula, v.current_price, v.price_denominator, v.trading_denominator, v.p_c_ratio, v.p_t_ratio, v.t_c_ratio, v.calc_type, v.op1_curr_type, v.op2_curr_type, v.op3_curr_type, v.op4_curr_type, v.op5_curr_type, v.op6_curr_type, v.op7_curr_type, v.op8_curr_type, v.parent_entity_id, v.entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.contributor_1_id, v.contributor_2_id, v.operand_1_id, v.operand_2_id, v.operand_3_id, v.operand_4_id, v.operand_5_id, v.operand_6_id, v.operand_7_id, v.operand_8_id, v.calculation_id } into g
                        select new abc_calc
                        {
                            value_1 = g.Max(x => x.value_1),
                            value_2 = g.Max(x => x.value_2),
                            calc_type = g.Key.calc_type,
                            entity_id = g.Key.entity_id,
                            parent_entity_id = g.Key.parent_entity_id,
                            year = g.Key.year,
                            period_id = g.Key.period_id,
                            workflow_stage = g.Key.workflow_stage,
                            contributor_id = g.Key.contributor_id,
                            result_row_id = g.Key.result_row_id,
                            op1_curr_type = g.Key.op1_curr_type,
                            op2_curr_type = g.Key.op2_curr_type,
                            op3_curr_type = g.Key.op3_curr_type,
                            op4_curr_type = g.Key.op4_curr_type,
                            op5_curr_type = g.Key.op5_curr_type,
                            op6_curr_type = g.Key.op6_curr_type,
                            op7_curr_type = g.Key.op7_curr_type,
                            op8_curr_type = g.Key.op8_curr_type,
                            formula = g.Key.formula,
                            calculation_id = g.Key.calculation_id,
                            current_price = g.Key.current_price,
                            price_denominator = g.Key.price_denominator,
                            trading_denominator = g.Key.trading_denominator,
                            p_c_ratio = g.Key.p_c_ratio,
                            p_t_ratio = g.Key.p_t_ratio,
                            t_c_ratio = g.Key.t_c_ratio,
                            cumulative_denominator = g.Key.cumulative_denominator,
                            currency_denominator = g.Key.currency_denominator,
                            historic = g.Key.historic,
                            estimate = g.Key.estimate,
                            cumulative = g.Key.cumulative,
                            e_a_flag = g.Key.e_a_flag,
                            num_years = g.Key.num_years,
                            result_curr_type = g.Key.result_curr_type,
                            exc_from_cumulative = g.Key.exc_from_cumulative,
                            static_to_period = g.Key.static_to_period
                        };
                stage = query.ToList<abc_calc>();
            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("class_calc_template", "assign_mom", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " : " + e.Message, true, ref certificate, template_id, calc_order, calc_type, batch_id);
                errors.Add(log_error);

            }
        }


        void get_data_from_db(int batch_id, ref List<entity_info> entity_period_tbl_info, ref List<db_entity_period_tbl> entity_period_tbl)
        {
            try
            {
                bc_cs_db_services gdb = new bc_cs_db_services();

                int i;

                object res = null;
                Array ares;
                String sql;

                bool quit_loop = false;


                sql = "exec dbo.bc_core_calcs_services_get_entities_info " + audit_id.ToString() + "," + template_id.ToString() + "," + batch_id.ToString();
                quit_loop = false;
                res = null;
                gdb.rp_dl = false;
                gdb.success = false;
                while (quit_loop == false)
                {
                    res = gdb.executesql_no_timeout_retry_cp_dl(sql, ref certificate);
                    if (gdb.success == true)
                        quit_loop = true;
                    else if (gdb.rp_dl == true)
                        Thread.Sleep(50);
                    else
                    {
                        quit_loop = true;
                    }
                }

                ares = (Array)res;
                entity_info ent_info;
                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    ent_info = new entity_info();
                    ent_info.entity_id = (long)ares.GetValue(0, i);
                    ent_info.parent_entity_id = (long)ares.GetValue(1, i);
                    ent_info.year = (int)ares.GetValue(2, i);
                    ent_info.period_id = (int)ares.GetValue(3, i);
                    ent_info.e_a_flag = (bool)ares.GetValue(4, i);
                    ent_info.workflow_stage = (int)ares.GetValue(5, i);
                    //if ((decimal)ares.GetValue(6, i) > 0)
                    ent_info.price_denominator = (decimal)ares.GetValue(6, i);
                    //if ((decimal)ares.GetValue(7, i) > 0)
                    ent_info.currency_denominator = (decimal)ares.GetValue(7, i);
                    if ((decimal)ares.GetValue(8, i) > 0)
                        ent_info.current_price = (decimal)ares.GetValue(8, i);
                    //if ((decimal)ares.GetValue(9, i) > 0)
                    ent_info.trading_denominator_current = (decimal)ares.GetValue(9, i);
                    //if ((decimal)ares.GetValue(10, i) > 0)
                    ent_info.trading_denominator_period_end = (decimal)ares.GetValue(10, i);
                    if ((decimal)ares.GetValue(11, i) > 0)
                        ent_info.cumulative_denominator_current = (decimal)ares.GetValue(11, i);
                    if ((decimal)ares.GetValue(12, i) > 0)
                        ent_info.cumulative_denominator_period_end = (decimal)ares.GetValue(12, i);
                    //if ((decimal)ares.GetValue(13, i) > 0)
                    ent_info.p_c_ratio_current = (decimal)ares.GetValue(13, i);
                    //if ((decimal)ares.GetValue(14, i) > 0)
                    ent_info.p_c_ratio_period_end = (decimal)ares.GetValue(14, i);
                    //if ((decimal)ares.GetValue(15, i) > 0)
                    ent_info.t_c_ratio_current = (decimal)ares.GetValue(15, i);
                    //if ((decimal)ares.GetValue(16, i) > 0)
                    ent_info.t_c_ratio_period_end = (decimal)ares.GetValue(16, i);
                    //if ((decimal)ares.GetValue(17, i) > 0)
                    ent_info.p_t_ratio_current = (decimal)ares.GetValue(17, i);
                    //if ((decimal)ares.GetValue(18, i) > 0)
                    ent_info.p_t_ratio_period_end = (decimal)ares.GetValue(18, i);
                    ent_info.contributor_id = (int)ares.GetValue(19, i);
                    ent_info.exc_from_cumulative = (bool)ares.GetValue(20, i);
                    entity_period_tbl_info.Add(ent_info);
                }


                ////  REM get data
                sql = "exec dbo.bc_core_calcs_services_get_data " + audit_id.ToString() + "," + template_id.ToString() + "," + batch_id.ToString();
                quit_loop = false;
                res = null;
                gdb.rp_dl = false;
                gdb.success = false;
                while (quit_loop == false)
                {
                    res = gdb.executesql_no_timeout_retry_cp_dl(sql, ref certificate);
                    if (gdb.success == true)
                        quit_loop = true;
                    else if (gdb.rp_dl == true)
                        Thread.Sleep(50);
                    else
                    {
                        quit_loop = true;
                    }
                }
                ares = (Array)res;
                db_entity_period_tbl ept_row;
                string sval;

                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    //ept_row = new entity_period_tbl_row();
                    ept_row = new db_entity_period_tbl();
                    ept_row.result_row = false;
                    ept_row.entity_id = (long)ares.GetValue(0, i);
                    ept_row.row_id = (long)ares.GetValue(1, i);
                    ept_row.year = (int)ares.GetValue(2, i);
                    ept_row.period_id = (int)ares.GetValue(3, i);
                    ept_row.contributor_id = (int)ares.GetValue(4, i);
                    ept_row.workflow_stage = (int)ares.GetValue(5, i);
                    sval = (string)ares.GetValue(6, i);
                    ept_row.dvalue = System.Convert.ToDecimal(sval);
                    ept_row.e_a_flag = (bool)ares.GetValue(7, i);
                    ept_row.interval_type = (int)ares.GetValue(8, i);
                    ept_row.interval_val = (int)ares.GetValue(9, i);
                    sval = (string)ares.GetValue(6, i);
                    entity_period_tbl.Add(ept_row);
                }
            }

            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("get_data_from_db", "get_data_from_db: template: " + template_id.ToString() + " batch: " + batch_id.ToString(), e.Message, true, ref certificate, template_id, 0, 0, batch_id);
                errors.Add(log_error);
            }

        }
        DataContext get_db_connection()
        {
            try
            {
                string strConnection;

                if (bc_cs_central_settings.connection_type.ToLower() == "trusted")
                    strConnection = "Server=" + bc_cs_central_settings.servername + ";Database=" + bc_cs_central_settings.dbname + ";Trusted_Connection=True;;Pooling=true";
                else
                    strConnection = "Data Source=SQLOLEDB;Server=" + bc_cs_central_settings.servername + ";Database=" + bc_cs_central_settings.dbname + ";UID=" + bc_cs_central_settings.username + ";PWD=" + bc_cs_central_settings.password + ";Pooling=true";

                DataContext db = new DataContext(strConnection);
                db.CommandTimeout = 3600;
                return db;
            }
            catch (Exception e)
            {
                calcs_error_activity log_error = new calcs_error_activity("bc_am_aggs_serviced_based", "get_db_connection", e.Message, true, ref certificate, 0, 0, 0, 0);
                errors.Add(log_error);
                return null;
            }
        }
    }
    DataContext get_db_connection()
    {
        try
        {
            bc_cs_db_services gdb = new bc_cs_db_services();
            string strConnection;

            if (bc_cs_central_settings.connection_type.ToLower() == "trusted")
                strConnection = "Server=" + bc_cs_central_settings.servername + ";Database=" + bc_cs_central_settings.dbname + ";Trusted_Connection=True;;Pooling=true";
            else
                strConnection = "Data Source=SQLOLEDB;Server=" + bc_cs_central_settings.servername + ";Database=" + bc_cs_central_settings.dbname + ";UID=" + bc_cs_central_settings.username + ";PWD=" + bc_cs_central_settings.password + ";Pooling=true";

            DataContext db = new DataContext(strConnection);
            db.CommandTimeout = 3600;
            return db;
        }
        catch (Exception e)
        {
            calcs_error_activity log_error = new calcs_error_activity("bc_am_aggs_serviced_based", "get_db_connection", e.Message, true, ref certificate, 0, 0, 0, 0);
            errors.Add(log_error);
            return null;
        }
    }

    void write_activity_log(long universe_id)
    {
        try
        {
            DataContext db;
            db = get_db_connection();
            Table<db_bc_core_calcs_log> results = db.GetTable<db_bc_core_calcs_log>();
            db_bc_core_calcs_log a;

            DateTime eda = new DateTime(9999, 09, 09);

            int i;
            for (i = 0; i < timings.Count; i++)
            {
                try
                {
                    a = new db_bc_core_calcs_log();
                    a.log_type = 0;
                    a.audit_id = audit_id;
                    a.audit_date = audit_date;
                    a.comment = timings[i].description;
                    a.logdate = timings[i].da.ToUniversalTime();
                    a.template_id = timings[i].template_id;
                    a.batch_type = timings[i].batch_type;
                    a.batch_ord = timings[i].batch_ord;
                    //a.entity_batch = timings[i].entity_batch;
                    results.InsertOnSubmit(a);
                }
                catch (Exception e)
                {
                    calcs_error_activity log_error = new calcs_error_activity("bc_am_aggs_serviced_based", "write_activity_log: " + i.ToString(), e.Message, true, ref certificate, 0, 0, 0, 0);
                    errors.Add(log_error);
                }
            }

            db.SubmitChanges();
        }
        catch (Exception e)
        {
            calcs_error_activity log_error = new calcs_error_activity("bc_am_aggs_serviced_based", "write_activity_log", e.Message, true, ref certificate, 0, 0, 0, 0);
            errors.Add(log_error);
        }

    }
    void write_error_log(long entity_id)
    {
        DataContext db;
        db = get_db_connection();

        Table<db_bc_core_calcs_log> results = db.GetTable<db_bc_core_calcs_log>();
        db_bc_core_calcs_log a;

        DateTime eda = new DateTime(9999, 09, 09);
        int i;
        for (i = 0; i < errors.Count; i++)
        {
            a = new db_bc_core_calcs_log();
            a.audit_id = audit_id;
            a.audit_date = audit_date;
            a.comment = errors[i].description;
            a.logdate = errors[i].da.ToUniversalTime();
            a.log_type = 1;
            a.template_id = errors[i].template_id;
            a.batch_type = errors[i].batch_type;
            a.batch_ord = errors[i].batch_ord;
            //a.entity_batch = errors[i].entity_batch;
            results.InsertOnSubmit(a);
        }

        db.SubmitChanges();


    }

    // classes
    class template
    {
        public int template_id;
        public bool parent;
        public int num_batches;

    }

    public class calculation
    {
        public int stage_type = 0;
        public long key = 1;
        public long result_row_id;
        public bool op_zero_for_null;
        public string linq_formula;
        public string linq_where;
        public long operand_1_id;
        public long operand_2_id;
        public long operand_3_id;
        public long operand_4_id;
        public long operand_5_id;
        public long operand_6_id;
        public long operand_7_id;
        public long operand_8_id;
        public string formula;
        public int num_years;
        public int contributor_1;
        public int contributor_2;
        public int interval_type;
        public int interval;
        public int op1_curr_type;
        public int op2_curr_type;
        public int op3_curr_type;
        public int op4_curr_type;
        public int op5_curr_type;
        public int op6_curr_type;
        public int op7_curr_type;
        public int op8_curr_type;
        public long calculation_id;
        public string calc_type;
        public int calc_order;
        public int icalc_type;
        public bool cumulative = false;
        public bool estimate = false;
        public bool historic = false;
        public int result_curr_type;
        public bool static_to_period = false;
        public bool average = false;
        public int calc_lib = 0;
    }

    class entity_info
    {
        public long key = 1;
        public long entity_id;
        public long parent_entity_id;
        public int year;
        public int period_id;
        public bool e_a_flag;
        public int workflow_stage;
        public int contributor_id;
        public Nullable<decimal> price_denominator = null;
        public Nullable<decimal> trading_denominator_current = null;
        public Nullable<decimal> trading_denominator_period_end = null;
        public Nullable<decimal> cumulative_denominator_current = null;
        public Nullable<decimal> cumulative_denominator_period_end = null;
        public Nullable<decimal> p_c_ratio_current = null;
        public Nullable<decimal> p_c_ratio_period_end = null;
        public Nullable<decimal> t_c_ratio_current = null;
        public Nullable<decimal> t_c_ratio_period_end = null;
        public Nullable<decimal> p_t_ratio_current = null;
        public Nullable<decimal> p_t_ratio_period_end = null;
        public Nullable<decimal> current_price = null;
        public Nullable<decimal> currency_denominator = null;
        public bool exc_from_cumulative = false;


    }
    class template_calc_batch
    {
        public int order;
        public int type;
    }
    class abc_calc
    {
        public int default_interval_type = 0;
        public bool op_zero_for_null;
        public long entity_id;
        public long parent_entity_id;
        public bool e_a_flag = false;
        public int year;
        public long period_id;
        public int workflow_stage;
        public long contributor_id;
        public long result_row_id;
        public string calc_type;
        public bool cumulative = false;
        public bool estimate = false;
        public bool historic = false;
        public bool static_to_period = false;
        public Nullable<decimal> current_price = null;

        public Nullable<decimal> price_denominator = null;

        public Nullable<decimal> currency_denominator = null;

        public Nullable<decimal> trading_denominator_current = null;
        public Nullable<decimal> trading_denominator_period_end = null;
        public Nullable<decimal> cumulative_denominator_current = null;
        public Nullable<decimal> cumulative_denominator_period_end = null;
        public Nullable<decimal> p_c_ratio_current = null;
        public Nullable<decimal> p_c_ratio_period_end = null;
        public Nullable<decimal> t_c_ratio_current = null;
        public Nullable<decimal> t_c_ratio_period_end = null;
        public Nullable<decimal> p_t_ratio_current = null;
        public Nullable<decimal> p_t_ratio_period_end = null;

        public Nullable<decimal> trading_denominator = null;

        public Nullable<decimal> cumulative_denominator = null;
        public Nullable<decimal> p_c_ratio = null;
        public Nullable<decimal> t_c_ratio = null;
        public Nullable<decimal> p_t_ratio = null;



        public Nullable<decimal> value_1 = null;
        public Nullable<decimal> value_2 = null;
        public Nullable<decimal> value_3 = null;
        public Nullable<decimal> value_4 = null;
        public Nullable<decimal> value_5 = null;
        public Nullable<decimal> value_6 = null;
        public Nullable<decimal> value_7 = null;
        public Nullable<decimal> value_8 = null;
        public Nullable<decimal> pvalue_1 = null;
        public Nullable<decimal> pvalue_2 = null;
        public Nullable<decimal> pvalue_3 = null;
        public Nullable<decimal> pvalue_4 = null;
        public Nullable<decimal> pvalue_5 = null;
        public Nullable<decimal> pvalue_6 = null;
        public Nullable<decimal> pvalue_7 = null;
        public Nullable<decimal> pvalue_8 = null;
        public Nullable<decimal> cvalue_1 = null;
        public Nullable<decimal> cvalue_2 = null;
        public Nullable<decimal> cvalue_3 = null;
        public Nullable<decimal> cvalue_4 = null;
        public Nullable<decimal> cvalue_5 = null;
        public Nullable<decimal> cvalue_6 = null;
        public Nullable<decimal> cvalue_7 = null;
        public Nullable<decimal> cvalue_8 = null;
        public long operand_1_id;
        public long operand_2_id;
        public long operand_3_id;
        public long operand_4_id;
        public long operand_5_id;
        public long operand_6_id;
        public long operand_7_id;
        public long operand_8_id;

        public string formula;

        public int num_years;

        public int interval_type;
        public int interval;

        public long contributor_1_id;
        public long contributor_2_id;

        public int op1_curr_type;
        public int op2_curr_type;
        public int op3_curr_type;
        public int op4_curr_type;
        public int op5_curr_type;
        public int op6_curr_type;
        public int op7_curr_type;
        public int op8_curr_type;
        public int result_curr_type;
        public long calculation_id;
        public bool exc_from_cumulative = false;
    }
    // database tables

    [Table(Name = "entity_period_tbl")]
    public partial class db_entity_period_tbl
    {
        public bool result_row = true;
        [Column(IsPrimaryKey = true)]
        public long entity_id;
        [Column(IsPrimaryKey = true)]
        public int year;
        [Column(IsPrimaryKey = true)]
        public long period_id;
        [Column]
        public DateTime date_from;
        [Column(IsPrimaryKey = true)]
        public DateTime date_to;
        [Column(IsPrimaryKey = true)]
        public Boolean e_a_flag;
        [Column(IsPrimaryKey = true)]
        public long contributor_id;
        [Column(IsPrimaryKey = true)]
        public long row_id;
        [Column]
        public string value;
        [Column(IsPrimaryKey = true)]
        public int workflow_stage;
        [Column]
        public int acc_standard;
        [Column]
        public string comment;
        [Column]
        public long user_id;
        [Column]
        public int audit_id;

        public Nullable<decimal> dvalue = null;
        public long parent_entity_id;
        public Nullable<decimal> dcumvalue = null;
        public Nullable<decimal> p_c_ratio = null;
        public Nullable<decimal> p_t_ratio = null;
        public int result_curr_type;
        public bool exc_from_cumulative = false;
        public int interval_type = 0;
        public int interval_val = 0;


    }


    [Table(Name = "entity_attribute_values_time_series")]
    public partial class db_entity_attribute_values_time_series
    {
        [Column(IsPrimaryKey = true)]
        public long entity_Id;
        [Column(IsPrimaryKey = true)]
        public long attribute_id;
        [Column(IsPrimaryKey = true)]
        public DateTime date_from;
        [Column]
        public DateTime date_to;
        [Column]
        public string value;
        [Column(IsPrimaryKey = true)]
        public long acc_standard;
        [Column(IsPrimaryKey = true)]
        public long workflow_stage;
        [Column]
        public string comment;
        [Column(IsPrimaryKey = true)]
        public long contributor_id;
        [Column]
        public long user_id;
        [Column]
        public int audit_id;

        public Nullable<decimal> dvalue = null;
        public long parent_entity_id;
        public Nullable<decimal> dcumvalue = null;
        public Nullable<decimal> p_c_ratio = null;
        public Nullable<decimal> p_t_ratio = null;
        public int result_curr_type;
    }
    [Table(Name = "bc_core_calc_log")]
    public partial class db_bc_core_calcs_log
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id;
        [Column]
        public int audit_id;
        [Column]
        public DateTime audit_date;
        [Column]
        public DateTime logdate;
        [Column]
        public int log_type;
        [Column]
        public int template_id;
        [Column]
        public int batch_type;
        [Column]
        public int batch_ord;
        [Column]
        public string comment;
        //[Column]
        //public int entity_batch;
    }
    //CUSTOM CALCULATION LIBRARY
    class custom_lib
    {
        public List<db_entity_period_tbl> calculate(int lib_id, long result_row_id, ref List<abc_calc> stage, int audit_id, DateTime audit_date, int template_id, int calc_order, int calc_type, int batch_id, ref SynchronizedCollection<calcs_error_activity> errors, ref bc_cs_security.certificate certificate)
        {

            List<db_entity_period_tbl> res = null;
            try
            {
                switch (lib_id)
                {
                    case 1:
                        //TSR
                        //CASE WHEN value_1 IS NULL THEN NULL WHEN value_2 IS NULL THEN NULL ELSE (POWER((value_1/current_price),(cast(1 as decimal) / cast(value_2 as decimal)))-1) + ((value_3/ currency_denominator)/ (current_price / trading_denominator)) END where isnull(current_price,0) <> 0 and isnull(value_2,0) <> 0
                        var cl = from v in stage
                                 where v.result_row_id == result_row_id
                                 //where v.value_2 != 0 && v.current_price != 0 && v.currency_denominator != 0 && v.trading_denominator != 0
                                 && v.value_2 > 0 && v.current_price != 0 && v.currency_denominator != 0 && v.trading_denominator != 0
                                 
                                 && v.value_1 != null && v.value_2 != null && v.value_3 != null && v.currency_denominator != null
                                 && v.trading_denominator != null && v.current_price != null
                                 select new db_entity_period_tbl
                                 {
                                     dvalue = v.value_1 == null || v.value_2 == null ? 0 : (decimal)(((decimal)Math.Pow(System.Convert.ToDouble(v.value_1) / System.Convert.ToDouble(v.current_price), 1.0 / System.Convert.ToInt16 (v.value_2)) - 1) + (v.value_3 / v.currency_denominator) / (v.current_price / v.trading_denominator)),
                                     entity_id = v.entity_id,
                                     row_id = v.result_row_id,
                                     year = v.year,
                                     period_id = v.period_id,
                                     workflow_stage = v.workflow_stage,
                                     contributor_id = v.contributor_id,
                                     acc_standard = 1,
                                     e_a_flag = v.e_a_flag,
                                     audit_id = audit_id,
                                     date_from = audit_date,
                                     user_id = 0,
                                     comment = "from service template " + template_id.ToString() + " : custom calc 1 : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                     date_to = new DateTime(9999, 09, 09),
                                     result_curr_type = v.result_curr_type,
                                     p_t_ratio = v.p_t_ratio,
                                     p_c_ratio = v.p_c_ratio,
                                     exc_from_cumulative = v.exc_from_cumulative
                                 };
                        res = cl.ToList<db_entity_period_tbl>();
                        // remove 0 as null
                        var query = from v in res
                                    where v.dvalue != 0
                                    select new db_entity_period_tbl
                                    {
                                        dvalue = v.dvalue,
                                        entity_id = v.entity_id,
                                        row_id = v.row_id,
                                        year = v.year,
                                        period_id = v.period_id,
                                        workflow_stage = v.workflow_stage,
                                        contributor_id = v.contributor_id,
                                        acc_standard = 1,
                                        e_a_flag = v.e_a_flag,
                                        audit_id = audit_id,
                                        date_from = audit_date,
                                        user_id = 0,
                                        comment = v.comment,
                                        date_to = new DateTime(9999, 09, 09),
                                        result_curr_type = v.result_curr_type,
                                        p_t_ratio = v.p_t_ratio,
                                        p_c_ratio = v.p_c_ratio,
                                        exc_from_cumulative = v.exc_from_cumulative
                                    };
                        res = query.ToList<db_entity_period_tbl>();
                        break;
                    case 2:
                        //IBES 2089
                        //CASE WHEN value_2 = 35137 THEN value_1
                        //      WHEN ISNULL(value_2, 35137) <> 35137 THEN value_3

                        cl = from v in stage
                             where v.result_row_id == result_row_id
                             select new db_entity_period_tbl
                             {
                                 dvalue = v.value_2 == 35137 ? (v.value_1) : v.value_2 == null ? null : v.value_3,
                                 entity_id = v.entity_id,
                                 row_id = v.result_row_id,
                                 year = v.year,
                                 period_id = v.period_id,
                                 workflow_stage = v.workflow_stage,
                                 contributor_id = v.contributor_id,
                                 acc_standard = 1,
                                 e_a_flag = v.e_a_flag,
                                 audit_id = audit_id,
                                 date_from = audit_date,
                                 user_id = 0,
                                 comment = "from service template " + template_id.ToString() + " : custom calc 2 : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                 date_to = new DateTime(9999, 09, 09),
                                 result_curr_type = v.result_curr_type,
                                 p_t_ratio = v.p_t_ratio,
                                 p_c_ratio = v.p_c_ratio,
                                 exc_from_cumulative = v.exc_from_cumulative
                             };
                        res = cl.ToList<db_entity_period_tbl>();

                        break;
                    case 3:
                        //5022,5075 CASE when cast(isnull(value_1,0) as decimal (30,5)) > 0 THEN value_1 ELSE value_2 END
                        cl = from v in stage
                             where v.result_row_id == result_row_id
                             select new db_entity_period_tbl
                             {
                                 dvalue = v.value_1 > 0 ? (v.value_1) : v.value_2,
                                 entity_id = v.entity_id,
                                 row_id = v.result_row_id,
                                 year = v.year,
                                 period_id = v.period_id,
                                 workflow_stage = v.workflow_stage,
                                 contributor_id = v.contributor_id,
                                 acc_standard = 1,
                                 e_a_flag = v.e_a_flag,
                                 audit_id = audit_id,
                                 date_from = audit_date,
                                 user_id = 0,
                                 comment = "from service template " + template_id.ToString() + " : custom calc 3 : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                 date_to = new DateTime(9999, 09, 09),
                                 result_curr_type = v.result_curr_type,
                                 p_t_ratio = v.p_t_ratio,
                                 p_c_ratio = v.p_c_ratio,
                                 exc_from_cumulative = v.exc_from_cumulative
                             };
                        res = cl.ToList<db_entity_period_tbl>();

                        query = from v in res
                                where v.dvalue != null
                                select new db_entity_period_tbl
                                {
                                    dvalue = v.dvalue,
                                    entity_id = v.entity_id,
                                    row_id = v.row_id,
                                    year = v.year,
                                    period_id = v.period_id,
                                    workflow_stage = v.workflow_stage,
                                    contributor_id = v.contributor_id,
                                    acc_standard = 1,
                                    e_a_flag = v.e_a_flag,
                                    audit_id = audit_id,
                                    date_from = audit_date,
                                    user_id = 0,
                                    comment = v.comment,
                                    date_to = new DateTime(9999, 09, 09),
                                    result_curr_type = v.result_curr_type,
                                    p_t_ratio = v.p_t_ratio,
                                    p_c_ratio = v.p_c_ratio,
                                    exc_from_cumulative = v.exc_from_cumulative
                                };
                        res = query.ToList<db_entity_period_tbl>();
                        break;


                    case 4:
                        //5025,5028,5029,5034,5053,5464 (isnull(value_1,0) + isnull(value_2,0)) /case when value_1 is not null and value_2 is not null then 2 when value_1 is null and value_2 is null then null else 1 end
                        cl = from v in stage
                             where v.result_row_id == result_row_id
                             select new db_entity_period_tbl
                             {
                                 dvalue = v.value_1 != null && v.value_2 != null ? (v.value_1 + v.value_2) / 2 : v.value_1 == null && v.value_2 != null ? v.value_2 : v.value_1 != null && v.value_2 == null ? v.value_1 : null,
                                 entity_id = v.entity_id,
                                 row_id = v.result_row_id,
                                 year = v.year,
                                 period_id = v.period_id,
                                 workflow_stage = v.workflow_stage,
                                 contributor_id = v.contributor_id,
                                 acc_standard = 1,
                                 e_a_flag = v.e_a_flag,
                                 audit_id = audit_id,
                                 date_from = audit_date,
                                 user_id = 0,
                                 comment = "from service template " + template_id.ToString() + " : custom calc 4 : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                 date_to = new DateTime(9999, 09, 09),
                                 result_curr_type = v.result_curr_type,
                                 p_t_ratio = v.p_t_ratio,
                                 p_c_ratio = v.p_c_ratio,
                                 exc_from_cumulative = v.exc_from_cumulative
                             };
                        res = cl.ToList<db_entity_period_tbl>();
                        query = from v in res
                                where v.dvalue != null
                                select new db_entity_period_tbl
                                {
                                    dvalue = v.dvalue,
                                    entity_id = v.entity_id,
                                    row_id = v.row_id,
                                    year = v.year,
                                    period_id = v.period_id,
                                    workflow_stage = v.workflow_stage,
                                    contributor_id = v.contributor_id,
                                    acc_standard = 1,
                                    e_a_flag = v.e_a_flag,
                                    audit_id = audit_id,
                                    date_from = audit_date,
                                    user_id = 0,
                                    comment = v.comment,
                                    date_to = new DateTime(9999, 09, 09),
                                    result_curr_type = v.result_curr_type,
                                    p_t_ratio = v.p_t_ratio,
                                    p_c_ratio = v.p_c_ratio,
                                    exc_from_cumulative = v.exc_from_cumulative
                                };
                        res = query.ToList<db_entity_period_tbl>();
                        break;
                    case 5:
                        // template 25 premium growth fixed income
                        cl = from v in stage
                             where v.result_row_id == result_row_id
                             select new db_entity_period_tbl
                             {
                                 dvalue = v.value_1 != null ? v.value_1 : v.value_2 != null ? v.value_2 : v.value_3,
                                 entity_id = v.entity_id,
                                 row_id = v.result_row_id,
                                 year = v.year,
                                 period_id = v.period_id,
                                 workflow_stage = v.workflow_stage,
                                 contributor_id = v.contributor_id,
                                 acc_standard = 1,
                                 e_a_flag = v.e_a_flag,
                                 audit_id = audit_id,
                                 date_from = audit_date,
                                 user_id = 0,
                                 comment = "from service template " + template_id.ToString() + " : custom calc 5 : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                 date_to = new DateTime(9999, 09, 09),
                                 result_curr_type = v.result_curr_type,
                                 p_t_ratio = v.p_t_ratio,
                                 p_c_ratio = v.p_c_ratio,
                                 exc_from_cumulative = v.exc_from_cumulative
                             };
                        res = cl.ToList<db_entity_period_tbl>();
                        query = from v in res
                                where v.dvalue != null
                                select new db_entity_period_tbl
                                {
                                    dvalue = v.dvalue,
                                    entity_id = v.entity_id,
                                    row_id = v.row_id,
                                    year = v.year,
                                    period_id = v.period_id,
                                    workflow_stage = v.workflow_stage,
                                    contributor_id = v.contributor_id,
                                    acc_standard = 1,
                                    e_a_flag = v.e_a_flag,
                                    audit_id = audit_id,
                                    date_from = audit_date,
                                    user_id = 0,
                                    comment = v.comment,
                                    date_to = new DateTime(9999, 09, 09),
                                    result_curr_type = v.result_curr_type,
                                    p_t_ratio = v.p_t_ratio,
                                    p_c_ratio = v.p_c_ratio,
                                    exc_from_cumulative = v.exc_from_cumulative
                                };
                        res = query.ToList<db_entity_period_tbl>();
                        break;
                    
                    case 6:
                        cl = from v in stage
                             where v.result_row_id == result_row_id && v.value_1 != null
                             select new db_entity_period_tbl
                             {
                                 dvalue = v.value_1,
                                 entity_id = v.entity_id,
                                 row_id = v.result_row_id,
                                 year = v.year,
                                 period_id = v.period_id,
                                 workflow_stage = v.workflow_stage,
                                 contributor_id = v.contributor_id,
                                 acc_standard = 1,
                                 e_a_flag = v.e_a_flag,
                                 audit_id = audit_id,
                                 date_from = audit_date,
                                 user_id = 0,
                                 comment = "from service template " + template_id.ToString() + " : custom calc 6 : batch: " + calc_order.ToString() + " type: " + v.calc_type,
                                 date_to = new DateTime(9999, 09, 09),
                                 result_curr_type = v.result_curr_type,
                                 p_t_ratio = v.p_t_ratio,
                                 p_c_ratio = v.p_c_ratio,
                                 exc_from_cumulative = v.exc_from_cumulative
                             };
                        res = cl.ToList<db_entity_period_tbl>();
                        break;

                }
                return res;
            }
            catch (Exception e)
            {

                calcs_error_activity log_error = new calcs_error_activity("bc_am_aggs_serviced_based", "custom calc", "Template: " + template_id.ToString() + " batch: " + batch_id.ToString() + " type: " + calc_type.ToString() + " order: " + calc_order.ToString() + " :  Result row: " + result_row_id.ToString() + ": " + e.Message, true, ref certificate, template_id, calc_type, calc_order, batch_id);
                errors.Add(log_error);
                return null;
            }
        }
    }

    class bulkdatainsert
    {
        public void BulkInsertEPTEAVTS(SynchronizedCollection<db_entity_period_tbl> entities, int template_id, int entity_batch, ref bc_cs_security.certificate certificate, ref SynchronizedCollection<calcs_error_activity> errors)
        //public void BulkInsertEPTEAVTS(List<db_entity_period_tbl> entities, int template_id, int entity_batch, ref bc_cs_security.certificate certificate, ref SynchronizedCollection<calcs_error_activity> errors)
        {
            try
            {
                string cs;
                if (bc_cs_central_settings.connection_type.ToLower() == "trusted")
                    cs = "Server=" + bc_cs_central_settings.servername + ";Database=" + bc_cs_central_settings.dbname + ";Trusted_Connection=True;;Pooling=true";
                else
                    cs = "Data Source=SQLOLEDB;Server=" + bc_cs_central_settings.servername + ";Database=" + bc_cs_central_settings.dbname + ";UID=" + bc_cs_central_settings.username + ";PWD=" + bc_cs_central_settings.password + ";Pooling=true";





                var tableept = new DataTable();
                tableept.Columns.Add("entity_id");
                tableept.Columns.Add("year");
                tableept.Columns.Add("period_id");
                tableept.Columns.Add("date_from");
                tableept.Columns.Add("date_to");
                tableept.Columns.Add("e_a_flag");
                tableept.Columns.Add("contributor_id");
                tableept.Columns.Add("row_id");
                tableept.Columns.Add("value");
                tableept.Columns.Add("workflow_stage");
                tableept.Columns.Add("acc_standard");
                tableept.Columns.Add("comment");
                tableept.Columns.Add("user_id");
                tableept.Columns.Add("audit_id");

                var tableeavts = new DataTable();
                tableeavts.Columns.Add("entity_Id");
                tableeavts.Columns.Add("attribute_id");
                tableeavts.Columns.Add("date_from");
                tableeavts.Columns.Add("date_to");
                tableeavts.Columns.Add("value");
                tableeavts.Columns.Add("acc_standard");
                tableeavts.Columns.Add("workflow_stage");
                tableeavts.Columns.Add("comment");
                tableeavts.Columns.Add("contributor_id");
                tableeavts.Columns.Add("user_id");
                tableeavts.Columns.Add("audit_id");


                int i;
                DataRow dr;
                for (i = 0; i < entities.Count; i++)
                {
                    if (entities[i].year != 9999)
                    {
                        dr = tableept.NewRow();
                        dr.SetField(tableept.Columns[0], entities[i].entity_id);
                        dr.SetField(tableept.Columns[1], entities[i].year);
                        dr.SetField(tableept.Columns[2], entities[i].period_id);
                        dr.SetField(tableept.Columns[3], entities[i].date_from);
                        dr.SetField(tableept.Columns[4], entities[i].date_to);
                        if (entities[i].e_a_flag == false)
                            dr.SetField(tableept.Columns[5], 0);
                        else
                            dr.SetField(tableept.Columns[5], 1);
                        dr.SetField(tableept.Columns[6], entities[i].contributor_id);
                        dr.SetField(tableept.Columns[7], entities[i].row_id);
                        dr.SetField(tableept.Columns[8], entities[i].value);
                        dr.SetField(tableept.Columns[9], entities[i].workflow_stage);
                        dr.SetField(tableept.Columns[10], entities[i].acc_standard);
                        dr.SetField(tableept.Columns[11], entities[i].comment);
                        dr.SetField(tableept.Columns[12], entities[i].user_id);
                        dr.SetField(tableept.Columns[13], entities[i].audit_id);
                        tableept.Rows.Add(dr);
                    }
                    else
                    {
                        dr = tableeavts.NewRow();
                        dr.SetField(tableeavts.Columns[0], entities[i].entity_id);
                        dr.SetField(tableeavts.Columns[1], entities[i].row_id);
                        dr.SetField(tableeavts.Columns[2], entities[i].date_from);
                        dr.SetField(tableeavts.Columns[3], entities[i].date_to);
                        dr.SetField(tableeavts.Columns[4], entities[i].value);
                        dr.SetField(tableeavts.Columns[5], entities[i].acc_standard);
                        dr.SetField(tableeavts.Columns[6], entities[i].workflow_stage);
                        dr.SetField(tableeavts.Columns[7], entities[i].comment);
                        dr.SetField(tableeavts.Columns[8], entities[i].contributor_id);
                        dr.SetField(tableeavts.Columns[9], entities[i].user_id);
                        dr.SetField(tableeavts.Columns[10], entities[i].audit_id);
                        tableeavts.Rows.Add(dr);

                    }
                }

                var connept = new SqlConnection(cs);
                var conneavts = new SqlConnection(cs);

                connept.Open();

                var bulkCopyept = new SqlBulkCopy(connept)
                {
                    DestinationTableName = "entity_period_tbl"
                };
                bulkCopyept.BulkCopyTimeout = 3600;
                bulkCopyept.WriteToServer(tableept);
                connept.Close();

                conneavts.Open();
                var bulkCopyeavts = new SqlBulkCopy(conneavts)
                {
                    DestinationTableName = "entity_attribute_values_time_series"
                };
                bulkCopyeavts.BulkCopyTimeout = 3600;
                bulkCopyeavts.WriteToServer(tableeavts);

                conneavts.Close();
            }
            catch (Exception e)
            {

                calcs_error_activity log_error = new calcs_error_activity("bc_am_aggs_serviced_based", "BulkInsertEPTEAVTS", "Template: " + template_id.ToString() + " entity batch: " + entity_batch.ToString() + ": " + e.Message, true, ref certificate, template_id, 0, 0, 0);
                errors.Add(log_error);
            }
        }

    }





}
public class calcs_error_activity
{
    string _class_name;
    string _method_name;
    DateTime _da = DateTime.UtcNow;
    string _description;
    int _entity_batch;
    int _template_id;
    int _batch_type;
    int _batch_ord;


    public calcs_error_activity(string lclass_name, string lmethod_name, string ldescription, bool berr, ref bc_cs_security.certificate certificate, int template_id, int batch_type, int batch_ord, int entity_batch)
    {
        bc_cs_error_log err;
        _class_name = lclass_name;
        _method_name = lmethod_name;
        _da = DateTime.Now;
        _description = ldescription;
        _template_id = template_id;
        _batch_type = batch_type;
        _batch_ord = batch_ord;
        _entity_batch = entity_batch;
        if (berr == true)
            err = new bc_cs_error_log(lclass_name, lmethod_name, bc_cs_error_codes.USER_DEFINED, ldescription, ref certificate);
        if (bc_cs_central_settings.server_logging == 1)
        {
            bc_cs_activity_log log = new bc_cs_activity_log(lclass_name, lmethod_name, bc_cs_activity_codes.COMMENTARY.ToString(), ldescription, ref certificate);
        }

        //Debug.Print(ldescription + ": " + _da.ToString());
    }

    public string class_name
    {
        get { return _class_name; }
        set { _class_name = value; }
    }
    public string description
    {
        get { return _description; }
        set { _description = value; }
    }
    public string method_name
    {
        get { return _method_name; }
        set { _method_name = value; }
    }
    public DateTime da
    {
        get { return _da; }
        set { _da = value; }
    }
    public int template_id
    {
        get { return _template_id; }
        set { _template_id = value; }
    }
    public int entity_batch
    {
        get { return _entity_batch; }
        set { _entity_batch = value; }
    }
    public int batch_type
    {
        get { return _batch_type; }
        set { _batch_type = value; }
    }
    public int batch_ord
    {
        get { return _batch_ord; }
        set { _batch_ord = value; }
    }

}
