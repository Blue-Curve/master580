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

public class bc_am_aggs_service_based
{

    public int audit_id;
    public DateTime audit_date;
    bc_cs_central_settings bcs = new bc_cs_central_settings(true);
    bc_cs_security.certificate certificate;
    bc_cs_db_services gdb = new bc_cs_db_services();
    List<calculation> calculations = new List<calculation>();
    List<year_period> year_periods;
    int target_count = 0;
    List<entity_target> gentity_target = new List<entity_target>();
    int thread_count;
    SynchronizedCollection<error_activity> timings = new SynchronizedCollection<error_activity>();
    public SynchronizedCollection<error_activity> errors = new SynchronizedCollection<error_activity>();
    List<target> targets = new List<target>();
    List<universe_formula> universe_formulas = new List<universe_formula>();
    List<string> calc_types = new List<string>();
    List<long> exc_contributors;
    List<local_currency_convert> local_currency_convert_rates = new List<local_currency_convert>();
    bool universe_has_weightings = false;
    int start_year;
    int end_year;
    List<entity_weighting_target> entity_weighting_targets;
    SynchronizedCollection<abc_calc_agg> gabc_calc_aggs;
    SynchronizedCollection<abc_calc_agg> gabc_calc_aggs_growths;
    SynchronizedCollection<abc_calc_agg> gabc_calc_aggs_cc;

    //List<abc_calc_agg> gabc_calc_aggs;
    //List<abc_calc_agg> gabc_calc_aggs_growths;
    //List<abc_calc_agg> gabc_calc_aggs_cc;
    public List<agg_result> debugallresults;
    public List<abc_calc_agg> debugabc_calc_aggs;
    public List<abc_calc_agg> debugabc_calc_aggs_growths;
    public List<abc_calc_agg> debugabc_calc_aggs_cc;
    public List<ttest_result> debuglttest_result;


    private readonly object monitor = new object();

    public void run(long universe_id, ref bc_cs_security.certificate gcertificate, int laudit_id, DateTime laudit_date, bool debug_mode = false, long target_entity_id = 0, long dual_entity_id = 0, int debug_exch_method = -1, string debug_calc_type = "")
    {
        certificate = gcertificate;
        error_activity log_activity;
        try
        {

            List<bc_cs_db_services.bc_cs_sql_parameter> sql_params;
            bc_cs_db_services.bc_cs_sql_parameter sql_param;
            audit_id = laudit_id;
            audit_date = laudit_date;
            long currency_id = 0;
            int exch_rate_method = 0;
            exc_contributors = new List<long>();
            log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Start Aggregating Universe", false, ref certificate);
            timings.Add(log_activity);
            // get info about universe

            object res;
            Array ares;
            String sql;
            int num_batches = 0;
            int i;
            entity_target ett;
            bool use_local_currency;
            use_local_currency = false;
            sql = "exec dbo.bc_core_aggs_services_get_universe_info " + universe_id.ToString();
            res = gdb.executesql(sql, ref certificate);
            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                if (i == 0)
                {
                    currency_id = (long)ares.GetValue(0, 0);
                    exch_rate_method = (int)ares.GetValue(1, 0);
                    start_year = (int)ares.GetValue(3, 0);
                    end_year = (int)ares.GetValue(4, 0);
                    use_local_currency = (bool)ares.GetValue(5, 0);
                }
                if ((long)ares.GetValue(2, i) != 0)
                    exc_contributors.Add((long)ares.GetValue(2, i));
            }

            sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "universe_id";
            sql_param.value = universe_id;
            sql_params.Add(sql_param);
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "num_per_batch";
            sql_param.value = bc_cs_central_settings.aggregation_service_entities_per_batch;
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
            sql_param.name = "target_entity_id";
            sql_param.value = target_entity_id;
            sql_params.Add(sql_param);
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "dual_entity_id";
            sql_param.value = dual_entity_id;
            sql_params.Add(sql_param);


            res = gdb.executesql_with_parameters_no_timeout("bc_core_aggs_services_get_entities_to_preset", sql_params, ref certificate);
            ares = (Array)res;



            log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Preset entities complete", false, ref certificate);
            timings.Add(log_activity);

            long pt = 0;
            long pdt = 0;
            target t;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                if (i == 0)
                {

                    num_batches = (int)ares.GetValue(0, 0);
                }
                ett = new entity_target();
                ett.entity_id = (long)ares.GetValue(3, i);
                ett.target_entity_id = (long)ares.GetValue(4, i);
                ett.dual_entity_id = (long)ares.GetValue(5, i);
                pt = ett.target_entity_id;
                pdt = ett.dual_entity_id;
                gentity_target.Add(ett);
            }
            // targets
            sql = "exec dbo.bc_core_aggs_services_get_targets " + universe_id.ToString() + "," + currency_id;
            res = gdb.executesql(sql, ref certificate);
            ares = (Array)res;


            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                t = new target();
                t.entity_id = (long)ares.GetValue(0, i);
                t.dual_entity_id = (long)ares.GetValue(1, i);
                t.currency_id = (long)ares.GetValue(2, i);
                targets.Add(t);
            }



            sql = "bc_core_aggs_services_get_calculations " + universe_id.ToString();
            res = gdb.executesql(sql, ref certificate);
            ares = (Array)res;

            calculation calc;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                calc = new calculation();

                calc.result_row_id = (long)ares.GetValue(0, i);
                calc.operand_1_id = (long)ares.GetValue(1, i);
                calc.operand_2_id = (long)ares.GetValue(2, i);
                calc.operand_3_id = (long)ares.GetValue(3, i);
                calc.operand_4_id = (long)ares.GetValue(4, i);
                calc.operand_5_id = (long)ares.GetValue(5, i);
                calc.operand_6_id = (long)ares.GetValue(6, i);
                calc.operand_7_id = (long)ares.GetValue(7, i);
                calc.operand_8_id = (long)ares.GetValue(8, i);
                calc.formula = (string)ares.GetValue(9, i);
                calc.num_years = (int)ares.GetValue(10, i);
                calc.contributor_1 = (int)ares.GetValue(11, i);
                calc.contributor_2 = (int)ares.GetValue(12, i);
                if ((long)ares.GetValue(13, i) > 0)
                {
                    calc.operand_1_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_1_is_weight = false;
                if ((long)ares.GetValue(14, i) > 0)
                {
                    calc.operand_2_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_2_is_weight = false;
                if ((long)ares.GetValue(15, i) > 0)
                {
                    calc.operand_3_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_3_is_weight = false;
                if ((long)ares.GetValue(16, i) > 0)
                {
                    calc.operand_4_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_4_is_weight = false;
                if ((long)ares.GetValue(17, i) > 0)
                {
                    calc.operand_5_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_5_is_weight = false;
                if ((long)ares.GetValue(18, i) > 0)
                    calc.operand_6_is_weight = true;
                else
                    calc.operand_6_is_weight = false;
                if ((long)ares.GetValue(19, i) > 0)
                {
                    calc.operand_7_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_7_is_weight = false;
                if ((long)ares.GetValue(20, i) > 0)
                {
                    calc.operand_8_is_weight = true;
                    universe_has_weightings = true;
                }
                else
                    calc.operand_8_is_weight = false;
                calc.op1_curr_type = (int)ares.GetValue(21, i);
                calc.op2_curr_type = (int)ares.GetValue(22, i);
                calc.op3_curr_type = (int)ares.GetValue(23, i);
                calc.op4_curr_type = (int)ares.GetValue(24, i);
                calc.op5_curr_type = (int)ares.GetValue(25, i);
                calc.op6_curr_type = (int)ares.GetValue(26, i);
                calc.op7_curr_type = (int)ares.GetValue(27, i);
                calc.op8_curr_type = (int)ares.GetValue(28, i);
                calc.calculation_id = (long)ares.GetValue(29, i);
                calc.calc_type = (string)ares.GetValue(30, i);
                if (calc.calc_type == "aggregate style" || calc.calc_type == "aggregate style use mean")
                {
                    calc.calc_type = "aggregate calenderized";

                }
                calculations.Add(calc);
            }

            if (universe_has_weightings == true)
            {
                entity_weighting_targets = new List<entity_weighting_target>();
                entity_weighting_target ewt;
                log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Start weightings", false, ref certificate);
                timings.Add(log_activity);
                sql = "bc_core_service_aggs_get_weightings " + universe_id.ToString();
                res = gdb.executesql_no_timeout(sql, ref certificate);
                ares = (Array)res;
                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    ewt = new entity_weighting_target();
                    ewt.target_entity_id = (long)ares.GetValue(0, i);
                    ewt.entity_id = (long)ares.GetValue(1, i);
                    ewt.parent_entity_id = (long)ares.GetValue(2, i);
                    ewt.attribute_id = (long)ares.GetValue(3, i);
                    ewt.weighting = (decimal)ares.GetValue(4, i);
                    entity_weighting_targets.Add(ewt);
                }
            }


            // linq equivalent formulas
            universe_formula ua;
            sql = "bc_core_service_aggs_linq_formulas " + universe_id.ToString();
            res = gdb.executesql(sql, ref certificate);
            ares = (Array)res;
            int k;
            bool calc_type_found;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                calc_type_found = false;
                ua = new universe_formula();
                ua.calculation_type = (string)ares.GetValue(0, i);
                ua.result_row_id = (long)ares.GetValue(1, i);
                ua.growth = (bool)ares.GetValue(2, i);
                ua.cross_contributor = (bool)ares.GetValue(3, i);
                ua.linq_formula = (string)ares.GetValue(4, i);
                ua.having = (string)ares.GetValue(5, i);
                ua.where = (string)ares.GetValue(6, i);
                ua.style_calculation_type = ua.calculation_type.ToLower();
                if (ua.calculation_type.ToLower() == "aggregate style" || ua.calculation_type.ToLower() == "aggregate style use mean")
                {
                    ua.calculation_type = "aggregate calenderized";
                }
                try
                {
                    ua.standard_deviation_mult = (double)ares.GetValue(7, i);
                }
                catch
                {
                    ua.standard_deviation_mult = 3.0;
                }

                universe_formulas.Add(ua);
                for (k = 0; k < calc_types.Count; k++)
                {
                    if (calc_types[k] == ua.calculation_type)
                    {
                        calc_type_found = true;
                    }
                }
                if (calc_type_found == false)
                {
                    if (debug_mode == false)
                        calc_types.Add(ua.calculation_type);
                    else
                    {
                        if (ua.calculation_type == debug_calc_type)
                            calc_types.Add(ua.calculation_type);
                    }

                }
            }

            log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Start seal", false, ref certificate);
            timings.Add(log_activity);

            if (debug_mode == false)
                seal_previous_resuls(universe_id);


            log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "End seal", false, ref certificate);
            timings.Add(log_activity);

            int ty;
            List<calculation> tcalculations;


            List<int> exch_rate_methods = new List<int>();
            if (debug_mode == true)
            {
                exch_rate_methods.Add(debug_exch_method);
            }
            else
            {
                if (exch_rate_method == 3)
                {
                    exch_rate_methods.Add(0);
                    exch_rate_methods.Add(2);
                }
                else
                    exch_rate_methods.Add(exch_rate_method);
            }
            int ex;
            for (ex = 0; ex < exch_rate_methods.Count; ex++)
            {
                exch_rate_method = exch_rate_methods[ex];
                log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Start Exchange Method: " + exch_rate_methods[ex].ToString(), false, ref certificate);
                timings.Add(log_activity);

                for (ty = 0; ty < calc_types.Count; ty++)
                {
                    if (use_local_currency == true)
                    {
                        local_currency_convert_rates.Clear();
                        //get universe to local rates for combinations
                        sql = "exec dbo.bc_core_aggs_services_get_local_rates " + universe_id.ToString() + "," + exch_rate_methods[ex].ToString() + ",'" + calc_types[ty].ToString() + "'," + currency_id.ToString();
                        res = gdb.executesql(sql, ref certificate);
                        ares = (Array)res;
                        local_currency_convert lc;
                        for (i = 0; i <= ares.GetUpperBound(1); i++)
                        {
                            lc = new local_currency_convert();
                            lc.currency_id = (long)ares.GetValue(0, i);
                            lc.year = (int)ares.GetValue(1, i);
                            lc.period_id = (int)ares.GetValue(2, i);
                            lc.rate = (decimal)ares.GetValue(3, i);
                            // needed for aggregate period where end dates are different
                            //lc.date_at
                            local_currency_convert_rates.Add(lc);
                        }
                    }


                    //gabc_calc_aggs = new List<abc_calc_agg>();
                    //gabc_calc_aggs_growths = new List<abc_calc_agg>();
                    //gabc_calc_aggs_cc = new List<abc_calc_agg>();

                    gabc_calc_aggs = new SynchronizedCollection<abc_calc_agg>();
                    gabc_calc_aggs_growths = new SynchronizedCollection<abc_calc_agg>();
                    gabc_calc_aggs_cc = new SynchronizedCollection<abc_calc_agg>();

                    tcalculations = new List<calculation>();
                    for (i = 0; i < calculations.Count; i++)
                    {
                        if (calculations[i].calc_type == calc_types[ty].ToString())
                        {
                            tcalculations.Add(calculations[i]);
                        }
                    }

                    log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Start Calc Type: " + calc_types[ty].ToString(), false, ref certificate);
                    timings.Add(log_activity);
                    //REM load distinct year, period, contrinbutors,workflow stage
                    sql = "bc_core_aggs_services_get_year_per_cont_stage_combinations_sp " + universe_id.ToString() + ",'" + calc_types[ty].ToString() + "'";
                    res = gdb.executesql(sql, ref certificate);
                    ares = (Array)res;
                    year_period year_period;
                    year_periods = new List<year_period>();
                    for (i = 0; i <= ares.GetUpperBound(1); i++)
                    {
                        year_period = new year_period();
                        year_period.year = (int)ares.GetValue(0, i);
                        year_period.period_id = (int)ares.GetValue(1, i);
                        year_period.contributor_id = (int)ares.GetValue(2, i);
                        year_period.workflow_stage = (int)ares.GetValue(3, i);
                        year_period.growth_only = (int)ares.GetValue(4, i);
                        year_periods.Add(year_period);
                    }



                    log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Start Threads", false, ref certificate);
                    timings.Add(log_activity);

                    // if exchange rate method  is 3 then oo this twice current and period average
                    //    REM throw a thread for each batch

                    List<Thread> athreads = new List<Thread>();
                    int b;
                    for (b = 0; b < num_batches; b++)
                    {
                        int j;
                        j = b + 1;
                        Thread thread = new Thread(() => dothetask(j, universe_id, exch_rate_method, calc_types[ty].ToString(), tcalculations, currency_id));
                        athreads.Add(thread);
                    }

                    //const int ACTIVE_THREADS = 30;
                    thread_count = 0;
                    for (i = 0; i < num_batches; i++)
                    {
                        while (thread_count >= bc_cs_central_settings.aggregation_service_num_active_threads)
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

                    log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "All Threads Started", false, ref certificate);
                    timings.Add(log_activity);


                    while (thread_count > 0)
                    {
                        Thread.Sleep(500);
                    }

                    log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "All Threads Complete", false, ref certificate);
                    timings.Add(log_activity);

                    if (errors.Count > 0)
                    {
                        log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Failure", false, ref certificate);
                        timings.Add(log_activity);
                        write_activity_log(universe_id);
                        write_error_log(universe_id);
                    }


                    // aggregate on a per target basis

                    target_count = targets.Count;

                    athreads = new List<Thread>();
                    for (b = 0; b < targets.Count; b++)
                    {
                        int j;
                        j = b;
                        Thread thread = new Thread(() => execute_target(targets[j].entity_id, targets[j].dual_entity_id, universe_id, currency_id, exch_rate_method, calc_types[ty].ToString(), targets[j].currency_id, universe_has_weightings, entity_weighting_targets, debug_mode));
                        athreads.Add(thread);
                    }


                    log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "All Threads Set For Target: " + targets.Count.ToString(), false, ref certificate);
                    timings.Add(log_activity);


                    thread_count = 0;
                    for (i = 0; i < targets.Count; i++)
                    {
                        while (thread_count >= bc_cs_central_settings.aggregation_service_num_active_threads)
                        {
                            Thread.Sleep(50);
                        }
                        Thread.Sleep(50);
                        lock (monitor)
                        {
                            thread_count = thread_count + 1;
                        }
                        athreads[i].Start();

                        log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "Thread Count Start: " + thread_count.ToString(), false, ref certificate);
                        timings.Add(log_activity);
                    }
                    while (target_count > 0)
                    {
                        Thread.Sleep(500);
                    }

                    gabc_calc_aggs.Clear();
                    gabc_calc_aggs_growths.Clear();
                    gabc_calc_aggs_cc.Clear();

                    log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "End Calc Type: " + calc_types[ty].ToString(), false, ref certificate);
                    timings.Add(log_activity);
                }
                log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "End Exchange Method: " + exch_rate_methods[ex].ToString(), false, ref certificate);
                timings.Add(log_activity);
            }

            log_activity = new error_activity("bc_am_aggs_serviced_based", "run", "End Aggregating Universe", false, ref certificate);
            timings.Add(log_activity);
            // write actvity and errors if any
            if (debug_mode == false)
            {
                write_activity_log(universe_id);
                write_error_log(universe_id);
            }

        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "run", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }

    void write_activity_log(long universe_id)
    {
        try
        {
            DataContext db;
            db = get_db_connection();
            Table<bc_core_aggs_log> results = db.GetTable<bc_core_aggs_log>();
            List<bc_core_aggs_log> res = new List<bc_core_aggs_log>();
            bc_core_aggs_log a;

            DateTime eda = new DateTime(9999, 09, 09);

            int i;
            for (i = 0; i < timings.Count; i++)
            {
                try
                {
                    a = new bc_core_aggs_log();
                    a.log_type = 1;
                    a.agg_universe_id = universe_id;
                    a.audit_id = audit_id;
                    a.batch_date = audit_date;
                    a.comment = timings[i].description;
                    a.log_date = timings[i].da;
                    a.sys_error = "From Service";
                    a.target_entity_id = timings[i].target_entity_id;
                    a.dual_entity_id = timings[i].dual_entity_id;
                    results.InsertOnSubmit(a);
                }
                catch (Exception e)
                {
                    error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "write_activity_log: " + i.ToString(), e.Message, true, ref certificate);
                    errors.Add(log_error);
                }
            }

            db.SubmitChanges();
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "write_activity_log", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }
    void write_error_log(long universe_id)
    {
        DataContext db;
        db = get_db_connection();

        Table<bc_core_aggs_log> results = db.GetTable<bc_core_aggs_log>();
        List<bc_core_aggs_log> res = new List<bc_core_aggs_log>();
        bc_core_aggs_log a;

        DateTime eda = new DateTime(9999, 09, 09);
        int i;
        for (i = 0; i < errors.Count; i++)
        {
            a = new bc_core_aggs_log();
            a.log_type = 0;
            a.agg_universe_id = universe_id;
            a.audit_id = audit_id;
            a.batch_date = audit_date;
            a.comment = errors[i].description;
            a.sys_error = errors[i].class_name + ":" + errors[i].method_name;
            a.log_date = errors[i].da;
            a.target_entity_id = errors[i].target_entity_id;
            a.dual_entity_id = errors[i].dual_entity_id;
            results.InsertOnSubmit(a);
        }

        db.SubmitChanges();


    }
    void execute_target(long entity_id, long dual_entity_id, long universe_id, long universe_currency_id, int exch_rate_method, string calc_type, long local_currency_id, bool has_weightings, List<entity_weighting_target> entity_weighting_targets, bool debug_only)
    {
        try
        {

            error_activity log_activity = new error_activity("bc_am_aggs_serviced_based", "execute_target", "Start Target: " + entity_id.ToString() + ": " + dual_entity_id.ToString(), false, ref certificate);
            timings.Add(log_activity);




            List<abc_calc_agg> labc_calc_aggs = new List<abc_calc_agg>();
            List<abc_calc_agg> labc_calc_aggs_growths = new List<abc_calc_agg>();
            List<abc_calc_agg> labc_calc_aggs_cc = new List<abc_calc_agg>();


            put_entities_into_target(ref gabc_calc_aggs, ref labc_calc_aggs, entity_id, dual_entity_id, local_currency_id);
            put_entities_into_target(ref gabc_calc_aggs_growths, ref labc_calc_aggs_growths, entity_id, dual_entity_id, local_currency_id);
            put_entities_into_target(ref gabc_calc_aggs_cc, ref labc_calc_aggs_cc, entity_id, dual_entity_id, local_currency_id);

            if (universe_currency_id != local_currency_id)
            {
                convert_to_local(ref labc_calc_aggs, entity_id, dual_entity_id, local_currency_id);
                convert_to_local(ref labc_calc_aggs_growths, entity_id, dual_entity_id, local_currency_id);
                convert_to_local(ref labc_calc_aggs_cc, entity_id, dual_entity_id, local_currency_id);
            }

            if (universe_has_weightings == true)
            {
                apply_weightings(ref labc_calc_aggs, entity_id, dual_entity_id, entity_weighting_targets);
                apply_weightings(ref labc_calc_aggs_growths, entity_id, dual_entity_id, entity_weighting_targets);
                apply_weightings(ref labc_calc_aggs_cc, entity_id, dual_entity_id, entity_weighting_targets);

            }

            cross_asset(ref labc_calc_aggs);
            cross_asset(ref labc_calc_aggs_growths);
            cross_asset(ref labc_calc_aggs_cc);




            remove_incomplete_calcs(ref labc_calc_aggs);
            remove_incomplete_calcs(ref labc_calc_aggs_growths);
            remove_incomplete_calcs(ref labc_calc_aggs_cc);

            set_growths(ref labc_calc_aggs_growths);
            set_cross_contributor(ref labc_calc_aggs_cc);

            execute(universe_id, universe_currency_id, labc_calc_aggs, labc_calc_aggs_growths, labc_calc_aggs_cc, entity_id, dual_entity_id, exch_rate_method, calc_type, local_currency_id, debug_only);
            if (debug_only == false)
            {
                labc_calc_aggs.Clear();
                labc_calc_aggs_cc.Clear();
                labc_calc_aggs_growths.Clear();
                labc_calc_aggs = null;
                labc_calc_aggs_cc = null;
                labc_calc_aggs_growths = null;
            }
            else
            {
                debugabc_calc_aggs = new List<abc_calc_agg>();
                debugabc_calc_aggs_growths = new List<abc_calc_agg>();
                debugabc_calc_aggs_cc = new List<abc_calc_agg>();
                debugabc_calc_aggs = labc_calc_aggs;
                debugabc_calc_aggs_growths = labc_calc_aggs_growths;
                debugabc_calc_aggs_cc = labc_calc_aggs_cc;
            }
            log_activity = new error_activity("bc_am_aggs_serviced_based", "execute_target", "End Target: " + entity_id.ToString() + ": " + dual_entity_id.ToString() + "Target Count End: " + (target_count - 1).ToString(), false, ref certificate);
            timings.Add(log_activity);
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "execute_target", entity_id.ToString() + ": " + dual_entity_id.ToString() + ": " + e.Message, true, ref certificate);
            errors.Add(log_error);
        }
        finally
        {
            lock (monitor)
            {
                thread_count = thread_count - 1;
                target_count = target_count - 1;
            }

        }
    }



    void execute(long universe_id, long universe_currency_id, List<abc_calc_agg> labc_calc_aggs, List<abc_calc_agg> labc_calc_aggs_growths, List<abc_calc_agg> labc_calc_aggs_cc, long target_entity_id, long dual_entity_id, int exch_rate_method, string calc_type, long local_currency_id, bool debug_only)
    {

        try
        {
            if (debug_only == false)
            {
                evaluate_statistics(universe_id, local_currency_id, 0, ref labc_calc_aggs, false, exch_rate_method);
                evaluate_statistics(universe_id, local_currency_id, 0, ref labc_calc_aggs_growths, true, exch_rate_method);
                evaluate_statistics(universe_id, local_currency_id, 0, ref labc_calc_aggs_cc, false, exch_rate_method);
            }


            int i;
            for (i = 0; i < universe_formulas.Count; i++)
            {
                if (universe_formulas[i].style_calculation_type == "aggregate style" || universe_formulas[i].style_calculation_type == "aggregate style use mean") ;
                {
                    evaluate_formula_ttest(universe_id, local_currency_id, 0, labc_calc_aggs, labc_calc_aggs_growths, labc_calc_aggs_cc, target_entity_id, dual_entity_id, exch_rate_method, calc_type, debug_only);
                    break;
                }
            }
            evaluate_formula(universe_id, local_currency_id, 0, labc_calc_aggs, labc_calc_aggs_growths, labc_calc_aggs_cc, target_entity_id, dual_entity_id, exch_rate_method, calc_type, debug_only);
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "execute", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }
    void set_growths(ref List<abc_calc_agg> stage)
    {
        try
        {


            var qrgrowths = from r in stage
                            join l in stage on r.entity_id equals l.entity_id
                            where r.year == l.year + r.num_years && r.contributor_id == l.contributor_id && r.result_row_id == l.result_row_id && r.period_id == l.period_id && r.workflow_stage == l.workflow_stage
                            select new abc_calc_agg
                            {
                                include_in_growthr = true,
                                target_entity_id = r.target_entity_id,
                                dual_entity_id = r.dual_entity_id,
                                entity_id = r.entity_id,
                                parent_entity_id = r.parent_entity_id,
                                year = r.year,
                                period_id = r.period_id,
                                workflow_stage = r.workflow_stage,
                                contributor_id = r.contributor_id,
                                result_row_id = r.result_row_id,
                                value_1 = r.value_1,
                                value_2 = r.value_2,
                                value_3 = r.value_3,
                                value_4 = r.value_4,
                                value_5 = r.value_5,
                                value_6 = r.value_6,
                                value_7 = r.value_7,
                                value_8 = r.value_8,
                                operand_1_id = r.operand_1_id,
                                operand_2_id = r.operand_2_id,
                                operand_3_id = r.operand_3_id,
                                operand_4_id = r.operand_4_id,
                                operand_5_id = r.operand_5_id,
                                operand_6_id = r.operand_6_id,
                                operand_7_id = r.operand_7_id,
                                operand_8_id = r.operand_8_id,
                                op1_curr_type = r.op1_curr_type,
                                op2_curr_type = r.op2_curr_type,
                                op3_curr_type = r.op3_curr_type,
                                op4_curr_type = r.op4_curr_type,
                                op5_curr_type = r.op5_curr_type,
                                op6_curr_type = r.op6_curr_type,
                                op7_curr_type = r.op7_curr_type,
                                op8_curr_type = r.op8_curr_type,
                                formula = r.formula,
                                num_years = r.num_years,
                                calculation_id = r.calculation_id,
                                operand_1_is_weight = r.operand_1_is_weight,
                                operand_2_is_weight = r.operand_2_is_weight,
                                operand_3_is_weight = r.operand_3_is_weight,
                                operand_4_is_weight = r.operand_4_is_weight,
                                operand_5_is_weight = r.operand_5_is_weight,
                                operand_6_is_weight = r.operand_6_is_weight,
                                operand_7_is_weight = r.operand_7_is_weight,
                                operand_8_is_weight = r.operand_8_is_weight,
                                class_id = r.class_id,
                                weighting = r.weighting,
                            };
            List<abc_calc_agg> rgrowths = qrgrowths.ToList<abc_calc_agg>();


            var qlgrowths = from r in stage
                            join l in stage on r.entity_id equals l.entity_id
                            where r.year == l.year + r.num_years && r.contributor_id == l.contributor_id && r.result_row_id == l.result_row_id && r.period_id == l.period_id && r.workflow_stage == l.workflow_stage
                            select new abc_calc_agg
                            {
                                include_in_growthl = true,
                                target_entity_id = r.target_entity_id,
                                dual_entity_id = r.dual_entity_id,
                                entity_id = r.entity_id,
                                parent_entity_id = r.parent_entity_id,
                                year = r.year - r.num_years,
                                period_id = r.period_id,
                                workflow_stage = r.workflow_stage,
                                contributor_id = r.contributor_id,
                                result_row_id = r.result_row_id,
                                value_1 = l.value_1,
                                value_2 = l.value_2,
                                value_3 = l.value_3,
                                value_4 = l.value_4,
                                value_5 = l.value_5,
                                value_6 = l.value_6,
                                value_7 = l.value_7,
                                value_8 = l.value_8,
                                operand_1_id = r.operand_1_id,
                                operand_2_id = r.operand_2_id,
                                operand_3_id = r.operand_3_id,
                                operand_4_id = r.operand_4_id,
                                operand_5_id = r.operand_5_id,
                                operand_6_id = r.operand_6_id,
                                operand_7_id = r.operand_7_id,
                                operand_8_id = r.operand_8_id,
                                op1_curr_type = r.op1_curr_type,
                                op2_curr_type = r.op2_curr_type,
                                op3_curr_type = r.op3_curr_type,
                                op4_curr_type = r.op4_curr_type,
                                op5_curr_type = r.op5_curr_type,
                                op6_curr_type = r.op6_curr_type,
                                op7_curr_type = r.op7_curr_type,
                                op8_curr_type = r.op8_curr_type,
                                formula = l.formula,
                                num_years = r.num_years,
                                calculation_id = r.calculation_id,
                                operand_1_is_weight = r.operand_1_is_weight,
                                operand_2_is_weight = r.operand_2_is_weight,
                                operand_3_is_weight = r.operand_3_is_weight,
                                operand_4_is_weight = r.operand_4_is_weight,
                                operand_5_is_weight = r.operand_5_is_weight,
                                operand_6_is_weight = r.operand_6_is_weight,
                                operand_7_is_weight = r.operand_7_is_weight,
                                operand_8_is_weight = r.operand_8_is_weight,
                                class_id = r.class_id,
                                weighting = r.weighting,
                            };
            List<abc_calc_agg> lgrowths = qlgrowths.ToList<abc_calc_agg>();

            stage.Clear();
            int i;
            for (i = 0; i <= rgrowths.Count - 1; i++)
            {
                stage.Add(rgrowths[i]);
            }
            for (i = 0; i <= lgrowths.Count - 1; i++)
            {
                stage.Add(lgrowths[i]);
            }
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "set_growths", e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }
    void set_cross_contributor(ref List<abc_calc_agg> stage)
    {
        try
        {
            //only include calcs in with both contributors remaining
            var validccr = from r in stage
                           join l in stage on r.entity_id equals l.entity_id
                           where r.year == l.year && r.contributor_id == l.contributor_1_id && l.contributor_id == r.contributor_2_id && r.result_row_id == l.result_row_id && r.period_id == l.period_id && r.workflow_stage == l.workflow_stage
                           select new abc_calc_agg
                           {
                               include_in_growthr = true,
                               target_entity_id = r.target_entity_id,
                               dual_entity_id = r.dual_entity_id,
                               entity_id = r.entity_id,
                               parent_entity_id = r.parent_entity_id,
                               year = r.year,
                               period_id = r.period_id,
                               workflow_stage = r.workflow_stage,
                               contributor_id = r.contributor_id,
                               result_row_id = r.result_row_id,
                               value_1 = r.value_1,
                               value_2 = r.value_2,
                               value_3 = r.value_3,
                               value_4 = r.value_4,
                               value_5 = r.value_5,
                               value_6 = r.value_6,
                               value_7 = r.value_7,
                               value_8 = r.value_8,
                               formula = r.formula,
                               operand_1_id = r.operand_1_id,
                               operand_2_id = r.operand_2_id,
                               operand_3_id = r.operand_3_id,
                               operand_4_id = r.operand_4_id,
                               operand_5_id = r.operand_5_id,
                               operand_6_id = r.operand_6_id,
                               operand_7_id = r.operand_7_id,
                               operand_8_id = r.operand_8_id,
                               op1_curr_type = r.op1_curr_type,
                               op2_curr_type = r.op2_curr_type,
                               op3_curr_type = r.op3_curr_type,
                               op4_curr_type = r.op4_curr_type,
                               op5_curr_type = r.op5_curr_type,
                               op6_curr_type = r.op6_curr_type,
                               op7_curr_type = r.op7_curr_type,
                               op8_curr_type = r.op8_curr_type,
                               num_years = r.num_years,
                               contributor_1_id = r.contributor_1_id,
                               contributor_2_id = r.contributor_2_id,
                               calculation_id = r.calculation_id,
                               operand_1_is_weight = r.operand_1_is_weight,
                               operand_2_is_weight = r.operand_2_is_weight,
                               operand_3_is_weight = r.operand_3_is_weight,
                               operand_4_is_weight = r.operand_4_is_weight,
                               operand_5_is_weight = r.operand_5_is_weight,
                               operand_6_is_weight = r.operand_6_is_weight,
                               operand_7_is_weight = r.operand_7_is_weight,
                               operand_8_is_weight = r.operand_8_is_weight,
                               class_id = r.class_id,
                               weighting = r.weighting,
                           };
            var abc_calc_aggs_ccr = validccr.ToList<abc_calc_agg>();
            //only include calcs in with both contributors remaining
            var validccl = from l in stage
                           join r in stage on l.entity_id equals r.entity_id
                           where r.year == l.year && r.contributor_id == l.contributor_1_id && l.contributor_id == r.contributor_2_id && r.result_row_id == l.result_row_id && r.period_id == l.period_id && r.workflow_stage == l.workflow_stage
                           select new abc_calc_agg
                           {
                               include_in_growthr = true,
                               target_entity_id = r.target_entity_id,
                               dual_entity_id = r.dual_entity_id,
                               entity_id = r.entity_id,
                               parent_entity_id = r.parent_entity_id,
                               year = r.year,
                               period_id = r.period_id,
                               workflow_stage = r.workflow_stage,
                               contributor_id = l.contributor_id,
                               result_row_id = r.result_row_id,
                               value_1 = l.value_1,
                               value_2 = l.value_2,
                               value_3 = l.value_3,
                               value_4 = l.value_4,
                               value_5 = l.value_5,
                               value_6 = l.value_6,
                               value_7 = l.value_7,
                               value_8 = l.value_8,
                               operand_1_id = r.operand_1_id,
                               operand_2_id = r.operand_2_id,
                               operand_3_id = r.operand_3_id,
                               operand_4_id = r.operand_4_id,
                               operand_5_id = r.operand_5_id,
                               operand_6_id = r.operand_6_id,
                               operand_7_id = r.operand_7_id,
                               operand_8_id = r.operand_8_id,
                               op1_curr_type = r.op1_curr_type,
                               op2_curr_type = r.op2_curr_type,
                               op3_curr_type = r.op3_curr_type,
                               op4_curr_type = r.op4_curr_type,
                               op5_curr_type = r.op5_curr_type,
                               op6_curr_type = r.op6_curr_type,
                               op7_curr_type = r.op7_curr_type,
                               op8_curr_type = r.op8_curr_type,
                               formula = l.formula,
                               num_years = r.num_years,
                               contributor_1_id = r.contributor_1_id,
                               contributor_2_id = r.contributor_2_id,
                               calculation_id = r.calculation_id,
                               operand_1_is_weight = r.operand_1_is_weight,
                               operand_2_is_weight = r.operand_2_is_weight,
                               operand_3_is_weight = r.operand_3_is_weight,
                               operand_4_is_weight = r.operand_4_is_weight,
                               operand_5_is_weight = r.operand_5_is_weight,
                               operand_6_is_weight = r.operand_6_is_weight,
                               operand_7_is_weight = r.operand_7_is_weight,
                               operand_8_is_weight = r.operand_8_is_weight,
                               class_id = r.class_id,
                               weighting = r.weighting,
                           };
            var abc_calc_aggs_ccl = validccl.ToList<abc_calc_agg>();
            stage.Clear();
            int i;
            for (i = 0; i < abc_calc_aggs_ccr.Count; i++)
            {
                stage.Add(abc_calc_aggs_ccr[i]);
            }
            for (i = 0; i < abc_calc_aggs_ccl.Count; i++)
            {
                stage.Add(abc_calc_aggs_ccl[i]);
            }
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "set_cross_contributor ", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }
    void apply_weightings(ref List<abc_calc_agg> stage, long entity_id, long dual_entity_id, List<entity_weighting_target> entity_weighting_targets)
    {
        try
        {
            //local_currency_convert_rates
            var query1 = from a in stage
                         join b in entity_weighting_targets on a.target_entity_id equals b.target_entity_id
                         where a.entity_id == b.entity_id
                         select new abc_calc_agg
                         {
                             entity_id = a.entity_id,
                             target_entity_id = a.target_entity_id,
                             dual_entity_id = a.dual_entity_id,
                             year = a.year,
                             period_id = a.period_id,
                             workflow_stage = a.workflow_stage,
                             contributor_id = a.contributor_id,
                             result_row_id = a.result_row_id,
                             //value_1 = (a.operand_1_is_weight == true && a.operand_1_id==b.attribute_id ? b.weighting : a.value_1),
                             value_1 = (a.operand_1_is_weight == true ? b.weighting : a.value_1),
                             value_2 = (a.operand_2_is_weight == true ? b.weighting : a.value_2),
                             value_3 = (a.operand_3_is_weight == true ? b.weighting : a.value_3),
                             value_4 = (a.operand_4_is_weight == true ? b.weighting : a.value_4),
                             value_5 = (a.operand_5_is_weight == true ? b.weighting : a.value_5),
                             value_6 = (a.operand_6_is_weight == true ? b.weighting : a.value_6),
                             value_7 = (a.operand_7_is_weight == true ? b.weighting : a.value_7),
                             value_8 = (a.operand_8_is_weight == true ? b.weighting : a.value_8),
                             operand_1_id = a.operand_1_id,
                             operand_2_id = a.operand_2_id,
                             operand_3_id = a.operand_3_id,
                             operand_4_id = a.operand_4_id,
                             operand_5_id = a.operand_5_id,
                             operand_6_id = a.operand_6_id,
                             operand_7_id = a.operand_7_id,
                             operand_8_id = a.operand_8_id,
                             formula = a.formula,
                             parent_entity_id = a.parent_entity_id,
                             num_years = a.num_years,
                             interval_type = a.interval_type,
                             interval = a.interval,
                             include_in_growthr = a.include_in_growthr,
                             include_in_growthl = a.include_in_growthl,
                             contributor_1_id = a.contributor_1_id,
                             contributor_2_id = a.contributor_2_id,
                             host_curr_id = a.host_curr_id,
                             calculation_id = a.calculation_id,
                             class_id = a.class_id,
                             weighting = (a.operand_1_is_weight == true ? b.weighting : a.operand_2_is_weight == true ? b.weighting : a.operand_3_is_weight == true ? b.weighting : a.operand_4_is_weight == true ? b.weighting : a.operand_5_is_weight == true ? b.weighting : a.operand_6_is_weight == true ? b.weighting : a.operand_7_is_weight == true ? b.weighting : a.operand_8_is_weight == true ? b.weighting : 0),
                             operand_1_is_weight = a.operand_1_is_weight,
                             operand_2_is_weight = a.operand_2_is_weight,
                             operand_3_is_weight = a.operand_3_is_weight,
                             operand_4_is_weight = a.operand_4_is_weight,
                             operand_5_is_weight = a.operand_5_is_weight,
                             operand_6_is_weight = a.operand_6_is_weight,
                             operand_7_is_weight = a.operand_7_is_weight,
                             operand_8_is_weight = a.operand_8_is_weight,
                         };

            stage = query1.ToList<abc_calc_agg>();
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "apply_weightings" + entity_id.ToString() + ": " + dual_entity_id.ToString(), e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }


    void convert_to_local(ref List<abc_calc_agg> stage, long entity_id, long dual_entity_id, long host_curr_id)
    {
        try
        {
            //local_currency_convert_rates
            var query1 = from a in stage
                         join b in local_currency_convert_rates on a.host_curr_id equals b.currency_id
                         where a.year == b.year && a.period_id == b.period_id
                         select new abc_calc_agg
                         {
                             entity_id = a.entity_id,
                             target_entity_id = a.target_entity_id,
                             dual_entity_id = a.dual_entity_id,
                             year = a.year,
                             period_id = a.period_id,
                             workflow_stage = a.workflow_stage,
                             contributor_id = a.contributor_id,
                             result_row_id = a.result_row_id,
                             value_1 = (a.op1_curr_type == 0 ? a.value_1 : a.value_1 * b.rate),
                             value_2 = (a.op2_curr_type == 0 ? a.value_2 : a.value_2 * b.rate),
                             value_3 = (a.op3_curr_type == 0 ? a.value_3 : a.value_3 * b.rate),
                             value_4 = (a.op4_curr_type == 0 ? a.value_4 : a.value_4 * b.rate),
                             value_5 = (a.op5_curr_type == 0 ? a.value_5 : a.value_5 * b.rate),
                             value_6 = (a.op6_curr_type == 0 ? a.value_6 : a.value_6 * b.rate),
                             value_7 = (a.op7_curr_type == 0 ? a.value_7 : a.value_7 * b.rate),
                             value_8 = (a.op8_curr_type == 0 ? a.value_8 : a.value_8 * b.rate),
                             operand_1_id = a.operand_1_id,
                             operand_2_id = a.operand_2_id,
                             operand_3_id = a.operand_3_id,
                             operand_4_id = a.operand_4_id,
                             operand_5_id = a.operand_5_id,
                             operand_6_id = a.operand_6_id,
                             operand_7_id = a.operand_7_id,
                             operand_8_id = a.operand_8_id,
                             formula = a.formula,
                             parent_entity_id = a.parent_entity_id,
                             num_years = a.num_years,
                             interval_type = a.interval_type,
                             interval = a.interval,
                             include_in_growthr = a.include_in_growthr,
                             include_in_growthl = a.include_in_growthl,
                             contributor_1_id = a.contributor_1_id,
                             contributor_2_id = a.contributor_2_id,
                             host_curr_id = host_curr_id,
                             calculation_id = a.calculation_id,
                             class_id = a.class_id,
                         };

            stage = query1.ToList<abc_calc_agg>();
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "convert_to_local" + entity_id.ToString() + ": " + dual_entity_id.ToString(), e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }
    void put_entities_into_target(ref SynchronizedCollection<abc_calc_agg> stage, ref List<abc_calc_agg> targetout, long entity_id, long dual_entity_id, long host_curr_id)
    {
        try
        {
            if (stage == null)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "put_entities_into_target" + entity_id.ToString() + ": " + dual_entity_id.ToString(), "stage null", true, ref certificate);
                errors.Add(log_error);
            }
            if (gentity_target == null)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "put_entities_into_target" + entity_id.ToString() + ": " + dual_entity_id.ToString(), "gentity_target null", true, ref certificate);
                errors.Add(log_error);
            }

            var query1 = from a in stage
                         join b in gentity_target on a.entity_id equals b.entity_id
                         where b.target_entity_id == entity_id && b.dual_entity_id == dual_entity_id
                         select new abc_calc_agg
                         {
                             entity_id = b.entity_id,
                             target_entity_id = b.target_entity_id,
                             dual_entity_id = b.dual_entity_id,
                             year = a.year,
                             period_id = a.period_id,
                             workflow_stage = a.workflow_stage,
                             contributor_id = a.contributor_id,
                             result_row_id = a.result_row_id,
                             value_1 = a.value_1,
                             value_2 = a.value_2,
                             value_3 = a.value_3,
                             value_4 = a.value_4,
                             value_5 = a.value_5,
                             value_6 = a.value_6,
                             value_7 = a.value_7,
                             value_8 = a.value_8,
                             operand_1_id = a.operand_1_id,
                             operand_2_id = a.operand_2_id,
                             operand_3_id = a.operand_3_id,
                             operand_4_id = a.operand_4_id,
                             operand_5_id = a.operand_5_id,
                             operand_6_id = a.operand_6_id,
                             operand_7_id = a.operand_7_id,
                             operand_8_id = a.operand_8_id,
                             formula = a.formula,
                             parent_entity_id = a.parent_entity_id,
                             num_years = a.num_years,
                             interval_type = a.interval_type,
                             interval = a.interval,
                             include_in_growthr = a.include_in_growthr,
                             include_in_growthl = a.include_in_growthl,
                             contributor_1_id = a.contributor_1_id,
                             contributor_2_id = a.contributor_2_id,
                             host_curr_id = host_curr_id,
                             calculation_id = a.calculation_id,
                             op1_curr_type = a.op1_curr_type,
                             op2_curr_type = a.op2_curr_type,
                             op3_curr_type = a.op3_curr_type,
                             op4_curr_type = a.op4_curr_type,
                             op5_curr_type = a.op5_curr_type,
                             op6_curr_type = a.op6_curr_type,
                             op7_curr_type = a.op7_curr_type,
                             op8_curr_type = a.op8_curr_type,
                             operand_1_is_weight = a.operand_1_is_weight,
                             operand_2_is_weight = a.operand_2_is_weight,
                             operand_3_is_weight = a.operand_3_is_weight,
                             operand_4_is_weight = a.operand_4_is_weight,
                             operand_5_is_weight = a.operand_5_is_weight,
                             operand_6_is_weight = a.operand_6_is_weight,
                             operand_7_is_weight = a.operand_7_is_weight,
                             operand_8_is_weight = a.operand_8_is_weight,
                             class_id = a.class_id,
                         };

            targetout = query1.ToList<abc_calc_agg>();

        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "put_entities_into_target " + entity_id.ToString() + ": " + dual_entity_id.ToString(), e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }

    void seal_previous_resuls(long universe_id)
    {
        try
        {

            List<bc_cs_db_services.bc_cs_sql_parameter> sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
            bc_cs_db_services.bc_cs_sql_parameter sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "universe_id";
            sql_param.value = universe_id;
            sql_params.Add(sql_param);
            sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
            sql_param.name = "audit_date";
            sql_param.value = audit_date;
            sql_params.Add(sql_param);

            gdb.executesql_with_parameters_no_timeout("bc_core_aggs_services_seal_previous_results", sql_params, ref certificate);
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "seal_previous_resuls", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }
    void remove_staging_data(long universe_id)
    {
        try
        {
            String sql;
            sql = "bc_core_aggs_service_clear_stage " + universe_id.ToString();
            gdb.executesql_no_timeout(sql, ref certificate);
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "remove_staging_data", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }


    void remove_excess_resuls(long universe_id, long audit_id)
    {
        try
        {
            String sql;
            sql = "exec dbo.bc_core_aggs_services_remove_excess_data " + universe_id.ToString() + "," + audit_id.ToString();
            gdb.executesql(sql, ref certificate);
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "remove_excess_resuls", e.Message, true, ref certificate);
            errors.Add(log_error);
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
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "get_db_connection", e.Message, true, ref certificate);
            errors.Add(log_error);
            return null;
        }
    }

    void evaluate_statistics(long universe_id, long currency_id, int exch_type, ref List<abc_calc_agg> stage, bool growth, int exch_rate_method)
    {
        try
        {
            List<bc_core_aggs_universe_statistics> stats = new List<bc_core_aggs_universe_statistics>();
            List<bc_core_aggs_universe_statistics> qresults;
            if (growth == false)
            {
                var querystats = from v in stage
                                 group v by new { v.target_entity_id, v.dual_entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.class_id } into g
                                 select new bc_core_aggs_universe_statistics
                                 {
                                     aggregation_universe_id = universe_id,
                                     audit_id = audit_id,
                                     exch_method = exch_type,
                                     target_entity_id = g.Key.target_entity_id,
                                     dual_entity_id = g.Key.dual_entity_id,
                                     year = g.Key.year,
                                     period_id = g.Key.period_id,
                                     result_row_id = g.Key.result_row_id,
                                     contributor_id = g.Key.contributor_id,
                                     workflow_stage = g.Key.workflow_stage,
                                     class_id = g.Key.class_id,
                                     entity_count = g.Count(),

                                 };
                qresults = querystats.ToList<bc_core_aggs_universe_statistics>();

            }
            else
            {
                var querystats = from v in stage
                                 where v.include_in_growthr == true
                                 group v by new { v.target_entity_id, v.dual_entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.class_id } into g
                                 select new bc_core_aggs_universe_statistics
                                 {
                                     aggregation_universe_id = universe_id,
                                     audit_id = audit_id,
                                     exch_method = exch_type,
                                     target_entity_id = g.Key.target_entity_id,
                                     dual_entity_id = g.Key.dual_entity_id,
                                     year = g.Key.year,
                                     period_id = g.Key.period_id,
                                     result_row_id = g.Key.result_row_id,
                                     contributor_id = g.Key.contributor_id,
                                     workflow_stage = g.Key.workflow_stage,
                                     class_id = g.Key.class_id,
                                     entity_count = g.Count(),
                                 };
                qresults = querystats.ToList<bc_core_aggs_universe_statistics>();
            }


            DataContext db;
            db = get_db_connection();

            Table<bc_core_aggs_universe_statistics> results = db.GetTable<bc_core_aggs_universe_statistics>();
            bc_core_aggs_universe_statistics a;




            DateTime eda = new DateTime(9999, 09, 09);
            int i, j;
            i = 0;
            while (i < qresults.Count)
            {
                for (j = 0; j < exc_contributors.Count; j++)
                {
                    if (qresults[i].contributor_id == exc_contributors[j])
                    {
                        qresults.RemoveAt(i);
                        i = i - 1;
                        break;
                    }
                }
                i = i + 1;
            }


            for (i = 0; i < qresults.Count; i++)
            {
                if ((qresults[i].year >= start_year && qresults[i].year <= end_year) || qresults[i].year == 9999)
                {
                    a = new bc_core_aggs_universe_statistics();
                    a.aggregation_universe_id = universe_id;
                    a.audit_id = audit_id;
                    a.exch_method = qresults[i].exch_method;
                    a.dual_entity_id = qresults[i].dual_entity_id;
                    a.target_entity_id = qresults[i].target_entity_id;
                    a.year = qresults[i].year;
                    a.period_id = qresults[i].period_id;
                    a.result_row_id = qresults[i].result_row_id;
                    a.contributor_id = qresults[i].contributor_id;
                    a.workflow_stage = qresults[i].workflow_stage;
                    a.entity_count = qresults[i].entity_count;
                    a.date_from = audit_date;
                    a.date_to = eda;
                    a.statistic_type_id = 1;
                    a.class_id = qresults[i].class_id;
                    if (a.class_id != 22 && a.class_id != 23)
                    {
                        int jj;
                        jj = 1;
                    }
                    a.exch_method = exch_rate_method;
                    results.InsertOnSubmit(a);
                }
            }

            db.SubmitChanges();
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "evalute_statistics", e.Message, true, ref certificate);
            errors.Add(log_error);
        }


    }

    void assign_to_results(ref List<agg_result> allresults, List<object> lresults, int exch_type, long currency_id, long target_entity_id, long dual_entity_id, int restype = 0)
    {
        try
        {

            List<agg_result> fresults = new List<agg_result>();
            agg_result aresult;
            int i;
            for (i = 0; i < lresults.Count; i++)
            {
                aresult = new agg_result();
                var row = lresults[i];
                var ttype = row.GetType();
                var p0 = ttype.GetProperty("result");
                switch (restype)
                {
                    case 0:
                        aresult.value = (decimal)(p0.GetValue(row));
                        break;
                    case 1:
                        aresult.rvalue = (decimal)(p0.GetValue(row));
                        p0 = ttype.GetProperty("num_years");
                        aresult.num_years = (int)(p0.GetValue(row));
                        break;
                    case 2:
                        aresult.lvalue = (decimal)(p0.GetValue(row));
                        p0 = ttype.GetProperty("num_years");
                        aresult.num_years = (int)(p0.GetValue(row));
                        break;
                    case 3:
                        aresult.value = (decimal)(p0.GetValue(row));
                        p0 = ttype.GetProperty("contributor_1_id");
                        aresult.contributor_1_id = (long)(p0.GetValue(row));
                        p0 = ttype.GetProperty("contributor_2_id");
                        aresult.contributor_2_id = (long)(p0.GetValue(row));
                        break;
                }
                //p0 = ttype.GetProperty("target_entity_id");
                aresult.target_entity_id = target_entity_id;
                //p0 = ttype.GetProperty("dual_entity_id");
                aresult.dual_entity_id = dual_entity_id;
                p0 = ttype.GetProperty("year");
                aresult.year = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("year");
                aresult.year = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("item_id");
                aresult.item_id = (long)(p0.GetValue(row));

                p0 = ttype.GetProperty("period_id");
                aresult.period_id = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("contributor_id");
                aresult.contributor_id = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("workflow_stage");
                aresult.workflow_stage = (int)(p0.GetValue(row));
                aresult.currency = currency_id;
                aresult.exch_type = exch_type;
                allresults.Add(aresult);

            }

        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "assing_to_results", e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }


    void ttest_assign_to_results(ref List<ttest_abc_calc_agg> allresults, List<object> lresults, string style_calculation_type, double standard_deviation_mult, int restype)
    {
        try
        {


            ttest_abc_calc_agg aresult;
            int i;
            for (i = 0; i < lresults.Count; i++)
            {
                aresult = new ttest_abc_calc_agg();
                var row = lresults[i];
                var ttype = row.GetType();
                var p0 = ttype.GetProperty("result");
                switch (restype)
                {
                    case 0:
                        aresult.linear_result = (decimal)(p0.GetValue(row));
                        break;
                    case 1:
                        aresult.rvalue = (decimal)(p0.GetValue(row));
                        p0 = ttype.GetProperty("num_years");
                        aresult.num_years = (int)(p0.GetValue(row));
                        break;
                    case 2:
                        aresult.lvalue = (decimal)(p0.GetValue(row));
                        p0 = ttype.GetProperty("num_years");
                        aresult.num_years = (int)(p0.GetValue(row));
                        break;
                    case 3:
                        aresult.linear_result = (decimal)(p0.GetValue(row));
                        p0 = ttype.GetProperty("contributor_1_id");
                        aresult.contributor_1_id = (long)(p0.GetValue(row));
                        p0 = ttype.GetProperty("contributor_2_id");
                        aresult.contributor_2_id = (long)(p0.GetValue(row));
                        break;
                }

                p0 = ttype.GetProperty("year");
                aresult.year = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("entity_id");
                aresult.entity_id = (long)(p0.GetValue(row));
                p0 = ttype.GetProperty("item_id");
                aresult.item_id = (long)(p0.GetValue(row));
                p0 = ttype.GetProperty("period_id");
                aresult.period_id = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("contributor_id");
                aresult.contributor_id = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("workflow_stage");
                aresult.workflow_stage = (int)(p0.GetValue(row));
                p0 = ttype.GetProperty("weighting");
                aresult.weighting = (decimal)(p0.GetValue(row));
                aresult.style_calculation_type = style_calculation_type;
                try
                {
                    aresult.standard_deviation_mult = standard_deviation_mult;
                }
                catch
                {
                    aresult.standard_deviation_mult = 3;
                }
                allresults.Add(aresult);
            }
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "ttest_assign_to_results", e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }

    void evaluate_formula(long universe_id, long currency_id, int exch_type, List<abc_calc_agg> labc_calc_aggs, List<abc_calc_agg> labc_calc_aggs_growths, List<abc_calc_agg> labc_calc_aggs_cc, long target_entity_id, long dual_entity_id, int exch_rate_method, string calc_type, bool debug_only)
    {
        try
        {
            int i, j;
            List<agg_result> allresults = new List<agg_result>();
            List<agg_result> resultsr;
            List<agg_result> resultsl;
            List<agg_result> resultscc;
            // dynamic linq
            string preamble = "new (";
            string group_by;


            string where;

            for (i = 0; i < universe_formulas.Count; i++)
            {
                if (universe_formulas[i].style_calculation_type == calc_type)
                {
                    try
                    {
                        if (universe_formulas[i].growth == false && universe_formulas[i].cross_contributor == false)
                        {
                            group_by = "as result,  key.year as year, key.result_row_id as item_id,  key.period_id as period_id, key.workflow_stage as workflow_stage, key.num_years as num_years, key.contributor_id as contributor_id )";
                            where = "result_row_id=" + universe_formulas[i].result_row_id + universe_formulas[i].where;
                            if (universe_formulas[i].having == "")
                            {
                                var t = labc_calc_aggs.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id }).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresults = t.Cast<object>();
                                List<object> lresults = oresults.ToList<object>();
                                assign_to_results(ref allresults, lresults, exch_type, currency_id, target_entity_id, dual_entity_id);
                            }
                            if (universe_formulas[i].having != "")
                            {
                                var t = labc_calc_aggs.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id }).Where(universe_formulas[i].having).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresults = t.Cast<object>();
                                List<object> lresults = oresults.ToList<object>();
                                assign_to_results(ref allresults, lresults, exch_type, currency_id, target_entity_id, dual_entity_id);
                            }

                        }
                        else if (universe_formulas[i].growth == true && universe_formulas[i].cross_contributor == false)
                        {
                            group_by = "as result,  key.year as year, key.result_row_id as item_id,  key.period_id as period_id, key.workflow_stage as workflow_stage, key.num_years as num_years, key.contributor_id as contributor_id )";

                            resultsr = new List<agg_result>();
                            resultsl = new List<agg_result>();
                            where = "result_row_id=" + universe_formulas[i].result_row_id + " and include_in_growthr == true" + universe_formulas[i].where;
                            if (universe_formulas[i].having == "")
                            {
                                var tr = labc_calc_aggs_growths.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id }).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresultsr = tr.Cast<object>();
                                List<object> lresultsr = oresultsr.ToList<object>();
                                assign_to_results(ref resultsr, lresultsr, exch_type, currency_id, target_entity_id, dual_entity_id, 1);
                            }
                            else
                            {
                                var tr = labc_calc_aggs_growths.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id }).Where(universe_formulas[i].having).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresultsr = tr.Cast<object>();
                                List<object> lresultsr = oresultsr.ToList<object>();
                                assign_to_results(ref resultsr, lresultsr, exch_type, currency_id, target_entity_id, dual_entity_id, 1);

                            }

                            where = "result_row_id=" + universe_formulas[i].result_row_id + " and include_in_growthl == true" + universe_formulas[i].where;
                            if (universe_formulas[i].having == "")
                            {
                                var tl = labc_calc_aggs_growths.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id }).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresultsl = tl.Cast<object>();
                                List<object> lresultsl = oresultsl.ToList<object>();
                                assign_to_results(ref resultsl, lresultsl, exch_type, currency_id, target_entity_id, dual_entity_id, 2);
                            }
                            else
                            {
                                var tl = labc_calc_aggs_growths.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id }).Where(universe_formulas[i].having).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresultsl = tl.Cast<object>();
                                List<object> lresultsl = oresultsl.ToList<object>();
                                assign_to_results(ref resultsl, lresultsl, exch_type, currency_id, target_entity_id, dual_entity_id, 2);
                            }

                            var gfinal = from n in resultsr
                                         join d in resultsl on n.year equals d.year + d.num_years
                                         where (d.lvalue != 0) && (n.rvalue / d.lvalue > 0) && (d.contributor_id == n.contributor_id) && (d.workflow_stage == n.workflow_stage) && (d.period_id == n.period_id) && (d.item_id == n.item_id) && (d.target_entity_id == n.target_entity_id) && (d.dual_entity_id == n.dual_entity_id)
                                         select new agg_result
                                         {
                                             universe_id = universe_id,
                                             currency = currency_id,
                                             audit_id = audit_id,
                                             exch_type = exch_type,
                                             target_entity_id = n.target_entity_id,
                                             dual_entity_id = n.dual_entity_id,
                                             year = n.year,
                                             period_id = n.period_id,
                                             item_id = n.item_id,
                                             contributor_id = n.contributor_id,
                                             workflow_stage = n.workflow_stage,
                                             value = (decimal)Math.Pow(Math.Abs(System.Convert.ToDouble(n.rvalue)) / Math.Abs(System.Convert.ToDouble(d.lvalue)), 1.0 / System.Convert.ToDouble(n.num_years)) - 1
                                         };
                            List<agg_result> gresultsfinal = gfinal.ToList<agg_result>();
                            for (j = 0; j < gresultsfinal.Count; j++)
                            {
                                allresults.Add(gresultsfinal[j]);
                            }

                        }
                        else if (universe_formulas[i].growth == false && universe_formulas[i].cross_contributor == true)
                        {
                            group_by = "as result,  key.year as year, key.result_row_id as item_id,  key.period_id as period_id, key.workflow_stage as workflow_stage, key.num_years as num_years, key.contributor_id as contributor_id , key.contributor_1_id as contributor_1_id, key.contributor_2_id as contributor_2_id)";

                            resultscc = new List<agg_result>();
                            where = "result_row_id=" + universe_formulas[i].result_row_id + universe_formulas[i].where; ;

                            if (universe_formulas[i].having == "")
                            {
                                var t = labc_calc_aggs_cc.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id, x.contributor_1_id, x.contributor_2_id }).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresults = t.Cast<object>();
                                List<object> lresults = oresults.ToList<object>();
                                assign_to_results(ref resultscc, lresults, exch_type, currency_id, target_entity_id, dual_entity_id, 3);
                            }
                            else
                            {
                                var t = labc_calc_aggs_cc.Where(where).GroupBy(x => new { x.year, x.result_row_id, x.calculation_id, x.period_id, x.workflow_stage, x.num_years, x.contributor_id, x.contributor_1_id, x.contributor_2_id }).Where(universe_formulas[i].having).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                                var oresults = t.Cast<object>();
                                List<object> lresults = oresults.ToList<object>();
                                assign_to_results(ref resultscc, lresults, exch_type, currency_id, target_entity_id, dual_entity_id, 3);

                            }

                            var qcrosscontributorfinal = from n in resultscc
                                                         join d in resultscc on n.contributor_id equals d.contributor_1_id
                                                         where (d.value != 0) && (n.contributor_2_id == d.contributor_id) && (n.year == d.year) && (d.workflow_stage == n.workflow_stage) && (d.period_id == n.period_id) && (d.item_id == n.item_id) && (d.target_entity_id == n.target_entity_id) && (d.dual_entity_id == n.dual_entity_id)
                                                         //where  (n.contributor_2_id == d.contributor_id) && (n.year == d.year) && (d.workflow_stage == n.workflow_stage) && (d.period_id == n.period_id) && (d.item_id == n.item_id) && (d.target_entity_id == n.target_entity_id) && (d.dual_entity_id == n.dual_entity_id)

                                                         select new agg_result
                                                         {
                                                             universe_id = universe_id,
                                                             currency = currency_id,
                                                             audit_id = audit_id,
                                                             exch_type = exch_type,
                                                             target_entity_id = n.target_entity_id,
                                                             dual_entity_id = n.dual_entity_id,
                                                             year = n.year,
                                                             period_id = n.period_id,
                                                             item_id = n.item_id,
                                                             contributor_id = n.contributor_id,
                                                             workflow_stage = n.workflow_stage,
                                                             value = (n.value / d.value) - 1
                                                         };
                            List<agg_result> rcrosscontributorfinal = qcrosscontributorfinal.ToList<agg_result>();
                            for (j = 0; j < rcrosscontributorfinal.Count; j++)
                            {
                                allresults.Add(rcrosscontributorfinal[j]);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "evaluate_formula", "failed to execute formula: " + universe_formulas[i].linq_formula + ":" + universe_formulas[i].having + ": " + universe_formulas[i].where + ": result row id: " + universe_formulas[i].result_row_id.ToString() + ": " + e.Message, true, ref certificate, target_entity_id, dual_entity_id);
                        errors.Add(log_error);
                    }
                }
            }

            DataContext db;
            db = get_db_connection();

            Table<bc_core_aggregated_data> results = db.GetTable<bc_core_aggregated_data>();
            bc_core_aggregated_data a;

            DateTime eda = new DateTime(9999, 09, 09);
            i = 0;
            while (i < allresults.Count)
            {
                for (j = 0; j < exc_contributors.Count; j++)
                {
                    if (allresults[i].contributor_id == exc_contributors[j])
                    {
                        allresults.RemoveAt(i);
                        i = i - 1;
                        break;
                    }
                }
                i = i + 1;
            }
            if (debug_only == false)
            {
                for (i = 0; i < allresults.Count; i++)
                {

                    if ((allresults[i].year >= start_year && allresults[i].year <= end_year) || allresults[i].year == 9999)
                    {
                        a = new bc_core_aggregated_data();
                        a.universe_id = universe_id;
                        a.currency = allresults[i].currency;
                        a.audit_id = audit_id;
                        a.exch_type = allresults[i].exch_type;
                        a.entity_id = allresults[i].target_entity_id;
                        a.dual_entity_id = allresults[i].dual_entity_id;
                        if (allresults[i].year < audit_date.Year)
                            a.e_a_flag = 0;
                        else
                            a.e_a_flag = 1;
                        a.year = allresults[i].year;
                        a.period_id = allresults[i].period_id;
                        a.item_id = allresults[i].item_id;
                        a.contributor_id = allresults[i].contributor_id;
                        a.workflow_stage = allresults[i].workflow_stage;
                        a.value = allresults[i].value.ToString();
                        a.date_from = audit_date;
                        a.date_to = eda;
                        a.comment = "from service: " + calc_type;
                        a.exch_type = exch_rate_method;
                        results.InsertOnSubmit(a);
                    }
                }
                allresults.Clear();
                allresults = null;
                db.SubmitChanges();
            }
            else
            {
                debugallresults = new List<agg_result>();
                for (i = 0; i < allresults.Count; i++)
                {
                    if ((allresults[i].year >= start_year && allresults[i].year <= end_year) || allresults[i].year == 9999)
                    {
                        debugallresults.Add(allresults[i]);
                    }
                }
            }
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "evaluate_formula", e.Message, true, ref certificate, target_entity_id, dual_entity_id);
            errors.Add(log_error);
        }

    }


    void evaluate_formula_ttest(long universe_id, long currency_id, int exch_type, List<abc_calc_agg> labc_calc_aggs, List<abc_calc_agg> labc_calc_aggs_growths, List<abc_calc_agg> labc_calc_aggs_cc, long target_entity_id, long dual_entity_id, int exch_rate_method, string calc_type, bool debug_only)
    {
        try
        {
            //standard deviation read this from database

            int i, j;
            List<ttest_abc_calc_agg> allresults = new List<ttest_abc_calc_agg>();
            List<ttest_abc_calc_agg> resultsr = new List<ttest_abc_calc_agg>();
            List<ttest_abc_calc_agg> resultsl = new List<ttest_abc_calc_agg>();
            List<ttest_abc_calc_agg> resultscc = new List<ttest_abc_calc_agg>();
            List<ttest_result> lttest_result;
            // dynamic linq
            string preamble = "new (";
            string group_by;


            string where;

            List<abc_calc_agg> labc_calc_aggs_linear;
            List<abc_calc_agg> labc_calc_aggs_linear_growths;
            List<abc_calc_agg> labc_calc_aggs_linear_cc;
            var cpquery = from v in labc_calc_aggs
                          join g in universe_formulas on v.result_row_id equals g.result_row_id
                          where (g.style_calculation_type == "aggregate style" || g.style_calculation_type == "aggregate style use mean")
                          select new abc_calc_agg
                          {
                              entity_id = v.entity_id,
                              parent_entity_id = v.parent_entity_id,
                              year = v.year,
                              period_id = v.period_id,
                              result_row_id = v.result_row_id,
                              contributor_id = v.contributor_id,
                              workflow_stage = v.workflow_stage,
                              value_1 = (v.operand_1_is_weight == false ? v.value_1 : 1),
                              value_2 = (v.operand_2_is_weight == false ? v.value_2 : 1),
                              value_3 = (v.operand_3_is_weight == false ? v.value_3 : 1),
                              value_4 = (v.operand_4_is_weight == false ? v.value_4 : 1),
                              value_5 = (v.operand_5_is_weight == false ? v.value_5 : 1),
                              value_6 = (v.operand_6_is_weight == false ? v.value_6 : 1),
                              value_7 = (v.operand_7_is_weight == false ? v.value_7 : 1),
                              value_8 = (v.operand_8_is_weight == false ? v.value_8 : 1),
                              operand_1_is_weight = v.operand_1_is_weight,
                              operand_2_is_weight = v.operand_2_is_weight,
                              operand_3_is_weight = v.operand_3_is_weight,
                              operand_4_is_weight = v.operand_4_is_weight,
                              operand_5_is_weight = v.operand_5_is_weight,
                              operand_6_is_weight = v.operand_6_is_weight,
                              operand_7_is_weight = v.operand_7_is_weight,
                              operand_8_is_weight = v.operand_8_is_weight,
                              weighting = v.weighting,
                          };
            labc_calc_aggs_linear = cpquery.ToList<abc_calc_agg>();
            var cpqueryg = from v in labc_calc_aggs_growths
                           join g in universe_formulas on v.result_row_id equals g.result_row_id
                           where (g.style_calculation_type == "aggregate style" || g.style_calculation_type == "aggregate style use mean")
                           select new abc_calc_agg
                           {
                               entity_id = v.entity_id,
                               parent_entity_id = v.parent_entity_id,
                               year = v.year,
                               period_id = v.period_id,
                               result_row_id = v.result_row_id,
                               contributor_id = v.contributor_id,
                               workflow_stage = v.workflow_stage,
                               value_1 = (v.operand_1_is_weight == false ? v.value_1 : 1),
                               value_2 = (v.operand_2_is_weight == false ? v.value_2 : 1),
                               value_3 = (v.operand_3_is_weight == false ? v.value_3 : 1),
                               value_4 = (v.operand_4_is_weight == false ? v.value_4 : 1),
                               value_5 = (v.operand_5_is_weight == false ? v.value_5 : 1),
                               value_6 = (v.operand_6_is_weight == false ? v.value_6 : 1),
                               value_7 = (v.operand_7_is_weight == false ? v.value_7 : 1),
                               value_8 = (v.operand_8_is_weight == false ? v.value_8 : 1),
                               operand_1_is_weight = v.operand_1_is_weight,
                               operand_2_is_weight = v.operand_2_is_weight,
                               operand_3_is_weight = v.operand_3_is_weight,
                               operand_4_is_weight = v.operand_4_is_weight,
                               operand_5_is_weight = v.operand_5_is_weight,
                               operand_6_is_weight = v.operand_6_is_weight,
                               operand_7_is_weight = v.operand_7_is_weight,
                               operand_8_is_weight = v.operand_8_is_weight,
                               weighting = v.weighting,
                               include_in_growthr = v.include_in_growthr,
                               include_in_growthl = v.include_in_growthl,
                               num_years = v.num_years,

                           };
            labc_calc_aggs_linear_growths = cpqueryg.ToList<abc_calc_agg>();
            var cpquerycc = from v in labc_calc_aggs_cc
                            join g in universe_formulas on v.result_row_id equals g.result_row_id
                            where (g.style_calculation_type == "aggregate style" || g.style_calculation_type == "aggregate style use mean")
                            select new abc_calc_agg
                            {
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                result_row_id = v.result_row_id,
                                contributor_id = v.contributor_id,
                                workflow_stage = v.workflow_stage,
                                value_1 = (v.operand_1_is_weight == false ? v.value_1 : 1),
                                value_2 = (v.operand_2_is_weight == false ? v.value_2 : 1),
                                value_3 = (v.operand_3_is_weight == false ? v.value_3 : 1),
                                value_4 = (v.operand_4_is_weight == false ? v.value_4 : 1),
                                value_5 = (v.operand_5_is_weight == false ? v.value_5 : 1),
                                value_6 = (v.operand_6_is_weight == false ? v.value_6 : 1),
                                value_7 = (v.operand_7_is_weight == false ? v.value_7 : 1),
                                value_8 = (v.operand_8_is_weight == false ? v.value_8 : 1),
                                operand_1_is_weight = v.operand_1_is_weight,
                                operand_2_is_weight = v.operand_2_is_weight,
                                operand_3_is_weight = v.operand_3_is_weight,
                                operand_4_is_weight = v.operand_4_is_weight,
                                operand_5_is_weight = v.operand_5_is_weight,
                                operand_6_is_weight = v.operand_6_is_weight,
                                operand_7_is_weight = v.operand_7_is_weight,
                                operand_8_is_weight = v.operand_8_is_weight,
                                weighting = v.weighting,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                            };
            labc_calc_aggs_linear_cc = cpquerycc.ToList<abc_calc_agg>();

            if (labc_calc_aggs_linear.Count == 0 && labc_calc_aggs_linear_growths.Count == 0 && labc_calc_aggs_linear_cc.Count == 0)
                return;

            // evaluate formula without weighting
            for (i = 0; i < universe_formulas.Count; i++)
            {
                try
                {
                    if (universe_formulas[i].style_calculation_type == "aggregate style" || universe_formulas[i].style_calculation_type == "aggregate style use mean")
                    {

                        if (universe_formulas[i].growth == false && universe_formulas[i].cross_contributor == false)
                        {
                            group_by = " as result,     weighting as weighting, entity_id as entity_id, year as year, result_row_id as item_id,  period_id as period_id, workflow_stage as workflow_stage,num_years as num_years,contributor_id as contributor_id )";
                            where = "result_row_id=" + universe_formulas[i].result_row_id + universe_formulas[i].where;
                            var t = labc_calc_aggs_linear.Where(where).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);
                            var oresults = t.Cast<object>();
                            List<object> lresults = oresults.ToList<object>();
                            ttest_assign_to_results(ref allresults, lresults, universe_formulas[i].style_calculation_type, universe_formulas[i].standard_deviation_mult, 0);
                        }
                        else if (universe_formulas[i].growth == true && universe_formulas[i].cross_contributor == false)
                        {
                            group_by = " as result,  weighting as weighting, entity_id as entity_id, year as year, result_row_id as item_id,  period_id as period_id, workflow_stage as workflow_stage,num_years as num_years,contributor_id as contributor_id )";
                            resultsr = new List<ttest_abc_calc_agg>();
                            resultsl = new List<ttest_abc_calc_agg>();
                            where = "result_row_id=" + universe_formulas[i].result_row_id + " and include_in_growthr == true" + universe_formulas[i].where;

                            var tr = labc_calc_aggs_linear_growths.Where(where).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);

                            var oresultsr = tr.Cast<object>();
                            List<object> lresultsr = oresultsr.ToList<object>();
                            ttest_assign_to_results(ref resultsr, lresultsr, universe_formulas[i].style_calculation_type, universe_formulas[i].standard_deviation_mult, 1);

                            where = "result_row_id=" + universe_formulas[i].result_row_id + " and include_in_growthl == true" + universe_formulas[i].where;
                            var tl = labc_calc_aggs_linear_growths.Where(where).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);

                            var oresultsl = tl.Cast<object>();
                            List<object> lresultsl = oresultsl.ToList<object>();
                            ttest_assign_to_results(ref resultsl, lresultsl, universe_formulas[i].style_calculation_type, universe_formulas[i].standard_deviation_mult, 2);



                            var gfinal = from n in resultsr
                                         join d in resultsl on n.year equals d.year + d.num_years
                                         where (n.entity_id == d.entity_id) && (d.lvalue != 0) && (n.rvalue / d.lvalue > 0) && (d.contributor_id == n.contributor_id) && (d.workflow_stage == n.workflow_stage) && (d.period_id == n.period_id) && (d.item_id == n.item_id)
                                         select new ttest_abc_calc_agg
                                         {
                                             year = n.year,
                                             period_id = n.period_id,
                                             item_id = n.item_id,
                                             contributor_id = n.contributor_id,
                                             workflow_stage = n.workflow_stage,
                                             weighting = n.weighting,
                                             entity_id = n.entity_id,
                                             linear_result = (decimal)Math.Pow(Math.Abs(System.Convert.ToDouble(n.rvalue)) / Math.Abs(System.Convert.ToDouble(d.lvalue)), 1.0 / System.Convert.ToDouble(n.num_years)) - 1,
                                             standard_deviation_mult = n.standard_deviation_mult
                                         };
                            List<ttest_abc_calc_agg> gresultsfinal = gfinal.ToList<ttest_abc_calc_agg>();
                            for (j = 0; j < gresultsfinal.Count; j++)
                            {
                                allresults.Add(gresultsfinal[j]);
                            }

                        }

                        else if (universe_formulas[i].growth == false && universe_formulas[i].cross_contributor == true)
                        {
                            group_by = " as result,  weighting as weighting, entity_id as entity_id, year as year, result_row_id as item_id,  period_id as period_id, workflow_stage as workflow_stage,num_years as num_years,contributor_id as contributor_id, contributor_1_id as contributor_1_id, contributor_2_id as contributor_2_id )";


                            resultscc = new List<ttest_abc_calc_agg>();
                            where = "result_row_id=" + universe_formulas[i].result_row_id + universe_formulas[i].where; ;
                            var t = labc_calc_aggs_linear_cc.Where(where).Select(preamble + universe_formulas[i].linq_formula.ToString() + group_by);

                            var oresults = t.Cast<object>();
                            List<object> lresults = oresults.ToList<object>();
                            ttest_assign_to_results(ref resultscc, lresults, universe_formulas[i].style_calculation_type, universe_formulas[i].standard_deviation_mult, 3);


                            var qcrosscontributorfinal = from n in resultscc
                                                         join d in resultscc on n.contributor_id equals d.contributor_1_id
                                                         where ((n.entity_id == d.entity_id) && n.contributor_2_id == d.contributor_id) && (n.year == d.year) && (d.workflow_stage == n.workflow_stage) && (d.period_id == n.period_id) && (d.item_id == n.item_id)
                                                         && d.linear_result != 0
                                                         select new ttest_abc_calc_agg
                                                         {

                                                             year = n.year,
                                                             period_id = n.period_id,
                                                             item_id = n.item_id,
                                                             contributor_id = n.contributor_id,
                                                             workflow_stage = n.workflow_stage,
                                                             weighting = n.weighting,
                                                             entity_id = n.entity_id,
                                                             contributor_1_id = n.contributor_1_id,
                                                             contributor_2_id = n.contributor_2_id,
                                                             linear_result = (n.linear_result / d.linear_result) - 1,
                                                             standard_deviation_mult = n.standard_deviation_mult
                                                         };
                            List<ttest_abc_calc_agg> rcrosscontributorfinal = qcrosscontributorfinal.ToList<ttest_abc_calc_agg>();
                            for (j = 0; j < rcrosscontributorfinal.Count; j++)
                            {
                                allresults.Add(rcrosscontributorfinal[j]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "evaluate_formula_ttest", "failed to execute formula: " + universe_formulas[i].linq_formula + ": " + universe_formulas[i].where + ": " + e.Message, true, ref certificate, target_entity_id, dual_entity_id);
                    errors.Add(log_error);
                }
            }



            // evaluate unweighted mean and sum of weights 
            var mquery = from v in allresults
                         group v by new { v.standard_deviation_mult, v.style_calculation_type, v.year, v.item_id, v.contributor_id, v.period_id, v.workflow_stage } into g
                         select new ttest_result
                         {
                             mean = g.Average(x => x.linear_result),
                             total_weight = g.Sum(x => x.weighting),
                             year = g.Key.year,
                             period_id = g.Key.period_id,
                             contributor_id = g.Key.contributor_id,
                             workflow_stage = g.Key.workflow_stage,
                             item_id = g.Key.item_id,
                             style_calculation_type = g.Key.style_calculation_type,
                             standard_deviation_mult = g.Key.standard_deviation_mult
                         };

            lttest_result = mquery.ToList<ttest_result>();
            // evalute standard deviation
            //step 1 evaluate distance from mean sqaured
            var sdquery1 = from v in allresults
                           join g in lttest_result on v.item_id equals g.item_id
                           where v.contributor_id == g.contributor_id && v.year == g.year
                           select new ttest_abc_calc_agg
                           {
                               entity_id = v.entity_id,
                               item_id = v.item_id,
                               year = v.year,
                               period_id = v.period_id,
                               contributor_id = v.contributor_id,
                               workflow_stage = v.workflow_stage,
                               linear_result = v.linear_result,
                               mean = g.mean,
                               variance = (decimal)Math.Pow(System.Convert.ToDouble(v.linear_result - g.mean), 2),
                               weighting = v.weighting,
                               total_weight = g.total_weight,
                               style_calculation_type = g.style_calculation_type,
                               standard_deviation_mult = g.standard_deviation_mult
                           };
            allresults = sdquery1.ToList<ttest_abc_calc_agg>();
            //step sum the values

            var sdquery2 = from v in allresults
                           group v by new { v.standard_deviation_mult, v.style_calculation_type, v.total_weight, v.year, v.item_id, v.contributor_id, v.period_id, v.workflow_stage, v.mean } into g
                           where g.Count() != 0
                           select new ttest_result
                           {
                               sd3 = g.Key.standard_deviation_mult * (Math.Pow(System.Convert.ToDouble(g.Sum(x => x.variance) / g.Count()), 0.5)),
                               //sd3 = 3 * (Math.Pow(System.Convert.ToDouble(g.Sum(x => x.variance) / g.Count()), 0.5)),

                               num_points = g.Count(),
                               mean = g.Key.mean,
                               year = g.Key.year,
                               period_id = g.Key.period_id,
                               contributor_id = g.Key.contributor_id,
                               workflow_stage = g.Key.workflow_stage,
                               item_id = g.Key.item_id,
                               total_weight = g.Key.total_weight,
                               style_calculation_type = g.Key.style_calculation_type
                           };

            lttest_result = sdquery2.ToList<ttest_result>();

            //winsorize results within mean + 3sds and mean - 3 sds
            // scale weight
            var wnquery1 = from v in allresults
                           join g in lttest_result on v.item_id equals g.item_id
                           where v.contributor_id == g.contributor_id && v.year == g.year && v.period_id == g.period_id && v.workflow_stage == g.workflow_stage && g.total_weight != 0
                           && g.total_weight != 0
                           select new ttest_abc_calc_agg
                           {
                               entity_id = v.entity_id,
                               item_id = v.item_id,
                               year = v.year,
                               period_id = v.period_id,
                               contributor_id = v.contributor_id,
                               workflow_stage = v.workflow_stage,
                               linear_result = v.linear_result,
                               winsorized_result = ((double)v.linear_result > ((double)g.mean + g.sd3) ? ((double)g.mean + g.sd3) :
                                                    (double)v.linear_result < ((double)g.mean - g.sd3) ? ((double)g.mean - g.sd3) :
                                                    (double)v.linear_result),
                               weighting = v.weighting,
                               scaled_weight = (double)(v.weighting / g.total_weight),
                               mean = v.mean,
                               style_calculation_type = v.style_calculation_type,
                           };
            allresults = wnquery1.ToList<ttest_abc_calc_agg>();

            // calculate weighted mean note this is just sum(winsorized result * scaled weight)
            var wmquery1 = from v in allresults
                           group v by new { v.style_calculation_type, v.mean, v.year, v.item_id, v.contributor_id, v.period_id, v.workflow_stage } into g
                           select new ttest_result
                           {
                               mean = g.Key.mean,
                               year = g.Key.year,
                               period_id = g.Key.period_id,
                               contributor_id = g.Key.contributor_id,
                               workflow_stage = g.Key.workflow_stage,
                               item_id = g.Key.item_id,
                               weighted_mean = g.Sum(x => (x.winsorized_result * x.scaled_weight)),
                               style_calculation_type = g.Key.style_calculation_type,
                           };
            lttest_result = wmquery1.ToList<ttest_result>();
            // evaluate sd this is the sq root of the sum of (scaled weight * ( winsorized_value - weighed mean))
            var wsdquery1 = from v in allresults
                            join g in lttest_result on v.item_id equals g.item_id
                            where v.contributor_id == g.contributor_id && v.year == g.year
                            select new ttest_abc_calc_agg
                            {
                                entity_id = v.entity_id,
                                item_id = v.item_id,
                                year = v.year,
                                period_id = v.period_id,
                                contributor_id = v.contributor_id,
                                workflow_stage = v.workflow_stage,
                                variance = (decimal)((double)v.scaled_weight * (Math.Pow(System.Convert.ToDouble(v.winsorized_result - g.weighted_mean), 2))),
                                weighted_mean = v.style_calculation_type == "aggregate style" ? g.weighted_mean : (double)g.mean,
                                scaled_weight_sq = Math.Pow(v.scaled_weight, 2),
                                style_calculation_type = v.style_calculation_type,
                            };
            allresults = wsdquery1.ToList<ttest_abc_calc_agg>();
            // final standard deviation     
            var wsdquery2 = from v in allresults
                            group v by new { v.weighted_mean, v.year, v.item_id, v.contributor_id, v.period_id, v.workflow_stage, v.mean } into g
                            where g.Sum(x => x.scaled_weight_sq) != 0
                            select new ttest_result
                            {
                                standard_deviation = (Math.Pow(System.Convert.ToDouble(g.Sum(x => x.variance)), 0.5)),
                                weighted_mean = g.Key.weighted_mean,
                                eff_number = 1 / g.Sum(x => x.scaled_weight_sq),
                                year = g.Key.year,
                                period_id = g.Key.period_id,
                                contributor_id = g.Key.contributor_id,
                                workflow_stage = g.Key.workflow_stage,
                                item_id = g.Key.item_id,
                            };

            lttest_result = wsdquery2.ToList<ttest_result>();



            if (debug_only == true)
            {

                debuglttest_result = new List<ttest_result>();
                debuglttest_result = lttest_result;
            }

            DataContext db;
            db = get_db_connection();

            Table<bc_core_aggregated_data_style> results = db.GetTable<bc_core_aggregated_data_style>();
            bc_core_aggregated_data_style a;

            DateTime eda = new DateTime(9999, 09, 09);
            i = 0;
            while (i < lttest_result.Count)
            {
                for (j = 0; j < exc_contributors.Count; j++)
                {
                    if (lttest_result[i].contributor_id == exc_contributors[j])
                    {
                        lttest_result.RemoveAt(i);
                        i = i - 1;
                        break;
                    }
                }
                i = i + 1;
            }
            if (debug_only == false)
            {
                for (i = 0; i < lttest_result.Count; i++)
                {

                    if ((lttest_result[i].year >= start_year && lttest_result[i].year <= end_year) || lttest_result[i].year == 9999)
                    {
                        a = new bc_core_aggregated_data_style();
                        a.universe_id = universe_id;
                        a.currency = currency_id;
                        a.audit_id = audit_id;
                        a.exch_type = exch_type;
                        a.entity_id = target_entity_id;
                        a.dual_entity_id = dual_entity_id;
                        if (lttest_result[i].year < audit_date.Year)
                            a.e_a_flag = 0;
                        else
                            a.e_a_flag = 1;
                        a.year = lttest_result[i].year;
                        a.period_id = lttest_result[i].period_id;
                        a.item_id = lttest_result[i].item_id;
                        a.contributor_id = lttest_result[i].contributor_id;
                        a.workflow_stage = lttest_result[i].workflow_stage;
                        a.mean = lttest_result[i].weighted_mean.ToString();
                        a.standard_deviation = lttest_result[i].standard_deviation.ToString();
                        a.effective_stocks = lttest_result[i].eff_number.ToString();
                        a.date_from = audit_date;
                        a.date_to = eda;
                        a.comment = "from service: " + calc_type;
                        a.exch_type = exch_rate_method;
                        results.InsertOnSubmit(a);
                    }
                }
                allresults.Clear();
                allresults = null;
                lttest_result.Clear();
                lttest_result = null;
                db.SubmitChanges();
            }
            else
            {
                debuglttest_result = new List<ttest_result>();
                for (i = 0; i < lttest_result.Count; i++)
                {
                    if ((lttest_result[i].year >= start_year && lttest_result[i].year <= end_year) || lttest_result[i].year == 9999)
                    {
                        debuglttest_result.Add(lttest_result[i]);
                    }
                }
            }
            labc_calc_aggs_linear.Clear();
            labc_calc_aggs_linear_growths.Clear();
            labc_calc_aggs_linear_cc.Clear();
        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "evaluate_formula_ttest", e.Message, true, ref certificate, target_entity_id, dual_entity_id);
            errors.Add(log_error);
        }

    }



    void dothetask(int batch_id, long universe_id, int exch_rate_method, string calc_type, List<calculation> tcalculations, long currency_id)
    {
        try
        {

            agg_batch a = new agg_batch();
            error_activity log_activity = new error_activity("bc_am_aggs_serviced_based", "dothetask", "Start Batch: " + batch_id.ToString() + " Thread Count: " + thread_count.ToString(), false, ref certificate);
            timings.Add(log_activity);

            a.load_data(batch_id, universe_id, ref gabc_calc_aggs, ref gabc_calc_aggs_growths, ref gabc_calc_aggs_cc, year_periods, tcalculations, ref thread_count, ref timings, ref errors, exch_rate_method, calc_type, currency_id, audit_id, audit_date, ref certificate);

            log_activity = new error_activity("bc_am_aggs_serviced_based", "dothetask", "End Batch: " + batch_id.ToString() + " Thread Count: " + (thread_count - 1).ToString(), false, ref certificate);
            timings.Add(log_activity);

        }
        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "dothetask", "batch: " + batch_id.ToString() + ":" + e.Message, true, ref certificate);
            errors.Add(log_error);
        }
        finally
        {
            lock (monitor)
            {
                thread_count = thread_count - 1;
            }
        }

    }
    void remove_incomplete_calcs(ref List<abc_calc_agg> stage)
    {
        try
        {
            var query = from s in stage
                        where ((s.value_1 != null && s.operand_1_id > 0) || s.operand_1_id == 0) && ((s.value_2 != null && s.operand_2_id > 0) || s.operand_2_id == 0)
                         && ((s.value_3 != null && s.operand_3_id > 0) || s.operand_3_id == 0) && ((s.value_4 != null && s.operand_4_id > 0) || s.operand_4_id == 0)
                          && ((s.value_5 != null && s.operand_5_id > 0) || s.operand_5_id == 0) && ((s.value_6 != null && s.operand_6_id > 0) || s.operand_6_id == 0)
                           && ((s.value_7 != null && s.operand_7_id > 0) || s.operand_7_id == 0) && ((s.value_8 != null && s.operand_8_id > 0) || s.operand_8_id == 0)
                        select new abc_calc_agg
                        {
                            target_entity_id = s.target_entity_id,
                            dual_entity_id = s.dual_entity_id,
                            entity_id = s.entity_id,
                            year = s.year,
                            period_id = s.period_id,
                            result_row_id = s.result_row_id,
                            contributor_id = s.contributor_id,
                            workflow_stage = s.workflow_stage,
                            value_1 = s.value_1,
                            value_2 = s.value_2,
                            value_3 = s.value_3,
                            value_4 = s.value_4,
                            value_5 = s.value_5,
                            value_6 = s.value_6,
                            value_7 = s.value_7,
                            value_8 = s.value_8,
                            num_years = s.num_years,
                            contributor_1_id = s.contributor_1_id,
                            contributor_2_id = s.contributor_2_id,
                            include_in_growthl = s.include_in_growthl,
                            include_in_growthr = s.include_in_growthr,
                            host_curr_id = s.host_curr_id,
                            calculation_id = s.calculation_id,
                            class_id = s.class_id,
                            weighting = s.weighting,
                            operand_1_is_weight = s.operand_1_is_weight,
                            operand_2_is_weight = s.operand_2_is_weight,
                            operand_3_is_weight = s.operand_3_is_weight,
                            operand_4_is_weight = s.operand_4_is_weight,
                            operand_5_is_weight = s.operand_5_is_weight,
                            operand_6_is_weight = s.operand_6_is_weight,
                            operand_7_is_weight = s.operand_7_is_weight,
                            operand_8_is_weight = s.operand_8_is_weight,
                        };
            stage = query.ToList<abc_calc_agg>();
        }

        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "remove_incomplete_calcs", e.Message, true, ref certificate);
            errors.Add(log_error);
        }
    }


    void cross_asset(ref List<abc_calc_agg> stage)
    {
        try
        {
            List<abc_calc_agg> allresults = new List<abc_calc_agg>();
            int i;
            //List<stage> allresults = new List<stage>();
            var query = from v in stage
                        where v.parent_entity_id != 0
                        && v.value_1 != null
                        group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g

                        select new abc_calc_agg
                        {
                            target_entity_id = g.Key.target_entity_id,
                            dual_entity_id = g.Key.dual_entity_id,
                            entity_id = g.Key.parent_entity_id,
                            year = g.Key.year,
                            period_id = g.Key.period_id,
                            result_row_id = g.Key.result_row_id,
                            contributor_id = g.Key.contributor_id,
                            workflow_stage = g.Key.workflow_stage,
                            host_curr_id = g.Key.host_curr_id,
                            value_1 = g.Sum(x => x.value_1),
                            calculation_id = g.Key.calculation_id,
                            class_id = g.Key.class_id,
                            //weighting = g.Key.weighting,
                            operand_1_is_weight = g.Key.operand_1_is_weight,
                            operand_2_is_weight = g.Key.operand_2_is_weight,
                            operand_3_is_weight = g.Key.operand_3_is_weight,
                            operand_4_is_weight = g.Key.operand_4_is_weight,
                            operand_5_is_weight = g.Key.operand_5_is_weight,
                            operand_6_is_weight = g.Key.operand_6_is_weight,
                            operand_7_is_weight = g.Key.operand_7_is_weight,
                            operand_8_is_weight = g.Key.operand_8_is_weight,
                        };

            List<abc_calc_agg> results1 = query.ToList<abc_calc_agg>();
            for (i = 0; i < results1.Count; i++)
            {
                allresults.Add(results1[i]);
            }
            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_2 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g

                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_2 = g.Sum(x => x.value_2),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results2 = query.ToList<abc_calc_agg>();
            for (i = 0; i < results2.Count; i++)
            {
                allresults.Add(results2[i]);
            }

            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_3 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g


                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_3 = g.Sum(x => x.value_3),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results3 = query.ToList<abc_calc_agg>();

            for (i = 0; i < results3.Count; i++)
            {
                allresults.Add(results3[i]);
            }

            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_4 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g


                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_4 = g.Sum(x => x.value_4),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results4 = query.ToList<abc_calc_agg>();
            for (i = 0; i < results4.Count; i++)
            {
                allresults.Add(results4[i]);
            }

            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_5 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g


                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_5 = g.Sum(x => x.value_5),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results5 = query.ToList<abc_calc_agg>();
            for (i = 0; i < results5.Count; i++)
            {
                allresults.Add(results5[i]);
            }

            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_6 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g


                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_6 = g.Sum(x => x.value_6),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results6 = query.ToList<abc_calc_agg>();

            for (i = 0; i < results6.Count; i++)
            {
                allresults.Add(results6[i]);
            }

            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_7 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g


                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_7 = g.Sum(x => x.value_7),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results7 = query.ToList<abc_calc_agg>();

            for (i = 0; i < results7.Count; i++)
            {
                allresults.Add(results7[i]);
            }

            query = from v in stage
                    where v.parent_entity_id != 0
                    && v.value_8 != null
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.parent_entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g


                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.parent_entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_8 = g.Sum(x => x.value_8),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        //weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,
                    };

            List<abc_calc_agg> results8 = query.ToList<abc_calc_agg>();
            for (i = 0; i < results8.Count; i++)
            {
                allresults.Add(results8[i]);
            }



            // merge into parent
            query = from v in allresults
                    group v by new { v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.weighting, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.entity_id, v.target_entity_id, v.dual_entity_id, v.host_curr_id, v.calculation_id, v.class_id } into g

                    select new abc_calc_agg
                    {
                        target_entity_id = g.Key.target_entity_id,
                        dual_entity_id = g.Key.dual_entity_id,
                        entity_id = g.Key.entity_id,
                        year = g.Key.year,
                        period_id = g.Key.period_id,
                        result_row_id = g.Key.result_row_id,
                        contributor_id = g.Key.contributor_id,
                        workflow_stage = g.Key.workflow_stage,
                        host_curr_id = g.Key.host_curr_id,
                        value_1 = g.Max(x => x.value_1),
                        value_2 = g.Max(x => x.value_2),
                        value_3 = g.Max(x => x.value_3),
                        value_4 = g.Max(x => x.value_4),
                        value_5 = g.Max(x => x.value_5),
                        value_6 = g.Max(x => x.value_6),
                        value_7 = g.Max(x => x.value_7),
                        value_8 = g.Max(x => x.value_8),
                        calculation_id = g.Key.calculation_id,
                        class_id = g.Key.class_id,
                        weighting = g.Key.weighting,
                        operand_1_is_weight = g.Key.operand_1_is_weight,
                        operand_2_is_weight = g.Key.operand_2_is_weight,
                        operand_3_is_weight = g.Key.operand_3_is_weight,
                        operand_4_is_weight = g.Key.operand_4_is_weight,
                        operand_5_is_weight = g.Key.operand_5_is_weight,
                        operand_6_is_weight = g.Key.operand_6_is_weight,
                        operand_7_is_weight = g.Key.operand_7_is_weight,
                        operand_8_is_weight = g.Key.operand_8_is_weight,

                    };
            //g.Sum(x => x.value_1) == 0 ? 0 : g.Sum(x => x.value_2) / g.Sum(x => x.value_1)
            allresults = query.ToList<abc_calc_agg>();


            query = from v in allresults
                    join s in stage on new { j1 = v.target_entity_id, j2 = v.dual_entity_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j7 = v.result_row_id, j8 = v.workflow_stage }
                      equals new { j1 = s.target_entity_id, j2 = s.dual_entity_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_id, j7 = s.result_row_id, j8 = s.workflow_stage }
                    where s.parent_entity_id == 0

                    select new abc_calc_agg
                    {
                        target_entity_id = s.target_entity_id,
                        dual_entity_id = s.dual_entity_id,
                        entity_id = s.entity_id,
                        year = s.year,
                        period_id = s.period_id,
                        result_row_id = s.result_row_id,
                        contributor_id = s.contributor_id,
                        workflow_stage = s.workflow_stage,
                        host_curr_id = s.host_curr_id,
                        value_1 = s.value_1,
                        value_2 = s.value_2,
                        value_3 = s.value_3,
                        value_4 = s.value_4,
                        value_5 = s.value_5,
                        value_6 = s.value_6,
                        value_7 = s.value_7,
                        value_8 = s.value_8,
                        raw_value_1 = v.value_1,
                        raw_value_2 = v.value_2,
                        raw_value_3 = v.value_3,
                        raw_value_4 = v.value_4,
                        raw_value_5 = v.value_5,
                        raw_value_6 = v.value_6,
                        raw_value_7 = v.value_7,
                        raw_value_8 = v.value_8,
                        operand_1_id = s.operand_1_id,
                        operand_2_id = s.operand_2_id,
                        operand_3_id = s.operand_3_id,
                        operand_4_id = s.operand_4_id,
                        operand_5_id = s.operand_5_id,
                        operand_6_id = s.operand_6_id,
                        operand_7_id = s.operand_7_id,
                        operand_8_id = s.operand_8_id,

                        num_years = s.num_years,
                        contributor_1_id = s.contributor_1_id,
                        contributor_2_id = s.contributor_2_id,
                        calculation_id = s.calculation_id,
                        class_id = s.class_id,
                        //weighting = s.weighting,
                        weighting = s.operand_1_is_weight == true ? s.value_1 : s.operand_2_is_weight == true ? s.value_2 : s.operand_3_is_weight == true ? s.value_3 : s.operand_4_is_weight == true ? s.value_4 : s.operand_5_is_weight == true ? s.value_5 : s.operand_6_is_weight == true ? s.value_6 : s.operand_7_is_weight == true ? s.value_7 : s.operand_8_is_weight == true ? s.value_8 : 0,
                        operand_1_is_weight = s.operand_1_is_weight,
                        operand_2_is_weight = s.operand_2_is_weight,
                        operand_3_is_weight = s.operand_3_is_weight,
                        operand_4_is_weight = s.operand_4_is_weight,
                        operand_5_is_weight = s.operand_5_is_weight,
                        operand_6_is_weight = s.operand_6_is_weight,
                        operand_7_is_weight = s.operand_7_is_weight,
                        operand_8_is_weight = s.operand_8_is_weight,
                    };
            allresults = query.ToList<abc_calc_agg>();
            // copy child values over
            for (i = 0; i < allresults.Count; i++)
            {
                if ((allresults[i].value_1 == null))
                {
                    allresults[i].value_1 = allresults[i].raw_value_1;
                }
                if ((allresults[i].value_2 == null))
                {
                    allresults[i].value_2 = allresults[i].raw_value_2;
                }
                if ((allresults[i].value_3 == null))
                {
                    allresults[i].value_3 = allresults[i].raw_value_3;
                }
                if ((allresults[i].value_4 == null))
                {
                    allresults[i].value_4 = allresults[i].raw_value_4;
                }
                if ((allresults[i].value_5 == null))
                {
                    allresults[i].value_5 = allresults[i].raw_value_5;
                }
                if ((allresults[i].value_6 == null))
                {
                    allresults[i].value_6 = allresults[i].raw_value_6;
                }
                if ((allresults[i].value_7 == null))
                {
                    allresults[i].value_7 = allresults[i].raw_value_7;
                }
                if ((allresults[i].value_8 == null))
                {
                    allresults[i].value_8 = allresults[i].raw_value_8;
                }
            }

            remove_incomplete_calcs(ref allresults);
            // now add to stage
            for (i = 0; i < allresults.Count; i++)
            {
                stage.Add(allresults[i]);
            }

        }

        catch (Exception e)
        {
            error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "cross_asset", e.Message, true, ref certificate);
            errors.Add(log_error);
        }

    }



    class agg_batch
    {
        bc_cs_security.certificate certificate;
        bc_cs_central_settings bcs = new bc_cs_central_settings(true);
        bc_cs_db_services gdb = new bc_cs_db_services();
        List<entity_period_tbl_row> entity_period_tbl = new List<entity_period_tbl_row>();
        List<abc_calc_agg> abc_calc_aggs = new List<abc_calc_agg>();
        List<abc_calc_agg> abc_calc_aggs_growths = new List<abc_calc_agg>();
        List<abc_calc_agg> abc_calc_aggs_cc = new List<abc_calc_agg>();
        List<entity_period_end_tbl_row> entity_period_tbl_info = new List<entity_period_end_tbl_row>();
        SynchronizedCollection<error_activity> timings;
        SynchronizedCollection<error_activity> errors;

        public void load_data(int batch_id, long universe_id, ref SynchronizedCollection<abc_calc_agg> lgabc_calc_aggs, ref SynchronizedCollection<abc_calc_agg> lgabc_calc_aggs_growths, ref SynchronizedCollection<abc_calc_agg> lgabc_calc_aggs_cc, List<year_period> lgyear_periods, List<calculation> lgcalculations, ref int thread_count, ref SynchronizedCollection<error_activity> gtimings, ref SynchronizedCollection<error_activity> gerrors, int exch_rate_method, string calc_type, long currency_id, int audit_id, DateTime audit_date, ref bc_cs_security.certificate gcertificate)
        {
            try
            {
                certificate = gcertificate;
                timings = gtimings;
                errors = gerrors;
                error_activity log_timings = new error_activity("agg_batch", "load_data", "Start Batch: " + batch_id.ToString(), false, ref certificate);
                timings.Add(log_timings);
                get_data_from_db(batch_id, universe_id, ref lgyear_periods, lgcalculations, exch_rate_method, calc_type, currency_id, audit_id, audit_date);




                //assign and apportion data
                assign_values(ref abc_calc_aggs);
                assign_values(ref abc_calc_aggs_growths);
                assign_values(ref abc_calc_aggs_cc);

                entity_period_tbl.Clear();

                remove_blanks_items(ref abc_calc_aggs);
                remove_blanks_items(ref abc_calc_aggs_growths);
                remove_blanks_items(ref abc_calc_aggs_cc);

                assign_to_master_list(batch_id, ref abc_calc_aggs, ref lgabc_calc_aggs);
                assign_to_master_list_growths(batch_id, ref abc_calc_aggs_growths, ref lgabc_calc_aggs_growths);
                assign_to_master_list_cc(batch_id, ref abc_calc_aggs_cc, ref lgabc_calc_aggs_cc);

            }

            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "cross_asset", e.Message, true, ref certificate);
                errors.Add(log_error);
            }

            finally
            {
                error_activity log_timings = new error_activity("agg_batch", "load_data", "End Batch: " + batch_id.ToString(), false, ref certificate);
                timings.Add(log_timings);

            }
        }


        public void get_data_from_db(int batch_id, long universe_id, ref List<year_period> lgyear_periods, List<calculation> lgcalculations, int exch_rate_method, string calc_type, long currency_id, int audit_id, DateTime audit_date)
        {
            try
            {
                List<entity> entities = new List<entity>();
                int i, j;

                object res;
                Array ares;
                String sql;

                sql = "exec dbo.bc_core_aggs_services_get_entities_in_batch " + batch_id.ToString() + "," + universe_id.ToString();
                res = gdb.executesql(sql, ref certificate);
                ares = (Array)res;
                entity ent;
                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    ent = new entity();
                    ent.entity_id = (long)ares.GetValue(0, i);
                    ent.parent_entity_id = (long)ares.GetValue(1, i);
                    ent.is_parent = (bool)ares.GetValue(2, i);
                    ent.class_id = (long)ares.GetValue(3, i);
                    entities.Add(ent);

                }

                entity_period_end_tbl_row info;
                bc_cs_db_services.bc_cs_sql_parameter sql_param;
                List<bc_cs_db_services.bc_cs_sql_parameter> sql_params = new List<bc_cs_db_services.bc_cs_sql_parameter>();
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "batch_id";
                sql_param.value = batch_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "universe_id";
                sql_param.value = universe_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "exch_rate_method";
                sql_param.value = exch_rate_method;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "calc_type";
                sql_param.value = calc_type;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "currency_id";
                sql_param.value = currency_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_id";
                sql_param.value = audit_id;
                sql_params.Add(sql_param);
                sql_param = new bc_cs_db_services.bc_cs_sql_parameter();
                sql_param.name = "audit_date";
                sql_param.value = audit_date;
                sql_params.Add(sql_param);

                res = gdb.executesql_with_parameters_no_timeout("bc_core_aggs_services_get_entities_info", sql_params, ref certificate);
                ares = (Array)res;


                //sql = "exec dbo.bc_core_aggs_services_get_entities_info " + batch_id.ToString() + "," + universe_id.ToString() + "," + exch_rate_method.ToString() + ",'" + calc_type + "'," + currency_id;
                //res = gdb.executesql_no_timeout(sql, ref certificate);
                ares = (Array)res;

                for (i = 0; i <= ares.GetUpperBound(1); i++)
                {
                    info = new entity_period_end_tbl_row();
                    info.entity_id = (long)ares.GetValue(0, i);
                    info.year = (int)ares.GetValue(1, i);
                    info.period_id = (int)ares.GetValue(2, i);
                    info.contributor_Id = (int)ares.GetValue(3, i);
                    info.workflow_stage = (int)ares.GetValue(4, i);
                    info.calenderized_portion_ratio = (decimal)ares.GetValue(5, i);
                    info.calenderized_year_offset = (int)ares.GetValue(6, i);
                    info.ecurr_ratio = (decimal)ares.GetValue(7, i);
                    info.tcurr_ratio = (decimal)ares.GetValue(8, i);
                    info.ecurr_denom_ratio = (decimal)ares.GetValue(9, i);
                    info.price_denom_ratio = (decimal)ares.GetValue(10, i);
                    entity_period_tbl_info.Add(info);

                }

                //  REM get data
                sql = "exec dbo.bc_core_aggs_services_get_data " + batch_id.ToString() + "," + universe_id.ToString() + ",'" + calc_type + "'," + audit_id.ToString();
                res = gdb.executesql_no_timeout(sql, ref certificate);
                ares = (Array)res;
                entity_period_tbl_row ept_row;
                string sval;
                try
                {
                    for (i = 0; i <= ares.GetUpperBound(1); i++)
                    {
                        ept_row = new entity_period_tbl_row();
                        ept_row.entity_id = (long)ares.GetValue(0, i);
                        ept_row.item_id = (long)ares.GetValue(1, i);
                        ept_row.year = (int)ares.GetValue(2, i);
                        ept_row.period_id = (int)ares.GetValue(3, i);
                        ept_row.contributor_Id = (int)ares.GetValue(4, i);
                        ept_row.workflow_stage = (int)ares.GetValue(5, i);
                        sval = (string)ares.GetValue(6, i);
                        ept_row.value = System.Convert.ToDecimal(sval);
                        entity_period_tbl.Add(ept_row);
                    }
                }
                catch (Exception e)
                {
                    Debug.Print(e.ToString());
                }
                var assportion = from e in entity_period_tbl
                                 join c in entity_period_tbl_info on e.entity_id equals c.entity_id
                                 where c.year == e.year && c.period_id == e.period_id && c.contributor_Id == e.contributor_Id && c.workflow_stage == e.workflow_stage
                                 select new entity_period_tbl_row
                                 {
                                     entity_id = e.entity_id,
                                     item_id = e.item_id,
                                     year = e.year,
                                     period_id = e.period_id,
                                     contributor_Id = e.contributor_Id,
                                     workflow_stage = e.workflow_stage,
                                     value = e.value,
                                     calendar_year_offset = c.calenderized_year_offset,
                                     calenderized_portion_ratio = c.calenderized_portion_ratio
                                 };

                //entity_period_tbl = assportion.ToList<entity_period_tbl_row>();
                var xstage = assportion.ToList<entity_period_tbl_row>();

                //int j;
                for (i = 0; i < entity_period_tbl.Count; i++)
                {

                    for (j = 0; j < xstage.Count; j++)
                    {
                        if (entity_period_tbl[i].entity_id == xstage[j].entity_id && entity_period_tbl[i].year == xstage[j].year && entity_period_tbl[i].contributor_Id == xstage[j].contributor_Id && entity_period_tbl[i].workflow_stage == xstage[j].workflow_stage && entity_period_tbl[i].item_id == xstage[j].item_id)
                        {
                            entity_period_tbl[i].calendar_year_offset = xstage[j].calendar_year_offset;
                            entity_period_tbl[i].calenderized_portion_ratio = xstage[j].calenderized_portion_ratio;
                            break;
                        }
                    }
                }

                // stage non growths
                var datastage = from e in entities
                                join c in lgcalculations on e.key equals c.key
                                join y in lgyear_periods on e.key equals y.key
                                where c.num_years == 0 && y.growth_only == 0 && c.contributor_1 == 0
                                orderby e.entity_id, c.result_row_id, y.year
                                select new abc_calc_agg
                                {
                                    target_entity_id = universe_id,
                                    entity_id = e.entity_id,
                                    parent_entity_id = e.parent_entity_id,
                                    year = y.year,
                                    period_id = y.period_id,
                                    workflow_stage = y.workflow_stage,
                                    contributor_id = y.contributor_id,
                                    result_row_id = c.result_row_id,
                                    operand_1_id = c.operand_1_id,
                                    operand_2_id = c.operand_2_id,
                                    operand_3_id = c.operand_3_id,
                                    operand_4_id = c.operand_4_id,
                                    operand_5_id = c.operand_5_id,
                                    operand_6_id = c.operand_6_id,
                                    operand_7_id = c.operand_7_id,
                                    operand_8_id = c.operand_8_id,
                                    operand_1_is_weight = c.operand_1_is_weight,
                                    operand_2_is_weight = c.operand_2_is_weight,
                                    operand_3_is_weight = c.operand_3_is_weight,
                                    operand_4_is_weight = c.operand_4_is_weight,
                                    operand_5_is_weight = c.operand_5_is_weight,
                                    operand_6_is_weight = c.operand_6_is_weight,
                                    operand_7_is_weight = c.operand_7_is_weight,
                                    operand_8_is_weight = c.operand_8_is_weight,
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
                                    value_1 = null,
                                    value_2 = null,
                                    value_3 = null,
                                    value_4 = null,
                                    value_5 = null,
                                    value_6 = null,
                                    value_7 = null,
                                    value_8 = null,
                                    calculation_id = c.calculation_id,
                                    class_id = e.class_id,

                                };

                abc_calc_aggs = datastage.ToList<abc_calc_agg>();



                // stage growths
                var datastagegrowths = from e in entities
                                       join c in lgcalculations on e.key equals c.key
                                       join y in lgyear_periods on e.key equals y.key
                                       where c.num_years != 0 && c.contributor_1 == 0
                                       select new abc_calc_agg
                                       {
                                           target_entity_id = universe_id,
                                           entity_id = e.entity_id,
                                           parent_entity_id = e.parent_entity_id,
                                           year = y.year,
                                           period_id = y.period_id,
                                           workflow_stage = y.workflow_stage,
                                           contributor_id = y.contributor_id,
                                           result_row_id = c.result_row_id,
                                           operand_1_id = c.operand_1_id,
                                           operand_2_id = c.operand_2_id,
                                           operand_3_id = c.operand_3_id,
                                           operand_4_id = c.operand_4_id,
                                           operand_5_id = c.operand_5_id,
                                           operand_6_id = c.operand_6_id,
                                           operand_7_id = c.operand_7_id,
                                           operand_8_id = c.operand_8_id,
                                           operand_1_is_weight = c.operand_1_is_weight,
                                           operand_2_is_weight = c.operand_2_is_weight,
                                           operand_3_is_weight = c.operand_3_is_weight,
                                           operand_4_is_weight = c.operand_4_is_weight,
                                           operand_5_is_weight = c.operand_5_is_weight,
                                           operand_6_is_weight = c.operand_6_is_weight,
                                           operand_7_is_weight = c.operand_7_is_weight,
                                           operand_8_is_weight = c.operand_8_is_weight,
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
                                           value_1 = null,
                                           value_2 = null,
                                           value_3 = null,
                                           value_4 = null,
                                           value_5 = null,
                                           value_6 = null,
                                           value_7 = null,
                                           value_8 = null,
                                           calculation_id = c.calculation_id,
                                           class_id = e.class_id,
                                       };
                abc_calc_aggs_growths = datastagegrowths.ToList<abc_calc_agg>();
                // stage cross contributor
                var datastagecc = from e in entities
                                  join c in lgcalculations on e.key equals c.key
                                  join y in lgyear_periods on e.key equals y.key
                                  where y.growth_only == 0 && c.contributor_1 > 0
                                  select new abc_calc_agg
                                  {
                                      target_entity_id = universe_id,
                                      entity_id = e.entity_id,
                                      parent_entity_id = e.parent_entity_id,
                                      year = y.year,
                                      period_id = y.period_id,
                                      workflow_stage = y.workflow_stage,
                                      contributor_id = y.contributor_id,
                                      result_row_id = c.result_row_id,
                                      operand_1_id = c.operand_1_id,
                                      operand_2_id = c.operand_2_id,
                                      operand_3_id = c.operand_3_id,
                                      operand_4_id = c.operand_4_id,
                                      operand_5_id = c.operand_5_id,
                                      operand_6_id = c.operand_6_id,
                                      operand_7_id = c.operand_7_id,
                                      operand_8_id = c.operand_8_id,
                                      operand_1_is_weight = c.operand_1_is_weight,
                                      operand_2_is_weight = c.operand_2_is_weight,
                                      operand_3_is_weight = c.operand_3_is_weight,
                                      operand_4_is_weight = c.operand_4_is_weight,
                                      operand_5_is_weight = c.operand_5_is_weight,
                                      operand_6_is_weight = c.operand_6_is_weight,
                                      operand_7_is_weight = c.operand_7_is_weight,
                                      operand_8_is_weight = c.operand_8_is_weight,
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
                                      value_1 = null,
                                      value_2 = null,
                                      value_3 = null,
                                      value_4 = null,
                                      value_5 = null,
                                      value_6 = null,
                                      value_7 = null,
                                      value_8 = null,
                                      calculation_id = c.calculation_id,
                                      class_id = e.class_id,
                                  };
                abc_calc_aggs_cc = datastagecc.ToList<abc_calc_agg>();

            }
            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "get_data_from_db: batch: " + batch_id.ToString(), e.Message, true, ref certificate);
                errors.Add(log_error);
            }

        }
        void assign_to_master_list(int batch_id, ref List<abc_calc_agg> stage, ref SynchronizedCollection<abc_calc_agg> lgabc_calc_aggs)
        {
            try
            {
                int i;
                for (i = 0; i <= stage.Count - 1; i++)
                {
                    lgabc_calc_aggs.Add(stage[i]);
                }
            }
            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "assign_to_master_list: batch: " + batch_id.ToString(), e.Message, true, ref certificate);
                errors.Add(log_error);
            }
        }
        void assign_to_master_list_growths(int batch_id, ref List<abc_calc_agg> stage, ref SynchronizedCollection<abc_calc_agg> lgabc_calc_aggs_growths)
        {
            try
            {
                int i;
                for (i = 0; i <= stage.Count - 1; i++)
                {
                    lgabc_calc_aggs_growths.Add(stage[i]);
                }
            }
            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "assign_to_master_list_growths: batch: " + batch_id.ToString(), e.Message, true, ref certificate);
                errors.Add(log_error);
            }
        }
        void assign_to_master_list_cc(int batch_id, ref List<abc_calc_agg> stage, ref SynchronizedCollection<abc_calc_agg> lgabc_calc_aggs_cc)
        {
            try
            {
                int i;
                for (i = 0; i <= stage.Count - 1; i++)
                {
                    lgabc_calc_aggs_cc.Add(stage[i]);
                }
            }
            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "assign_to_master_list_cc: batch: " + batch_id.ToString(), e.Message, true, ref certificate);
                errors.Add(log_error);
            }
        }
        void remove_blanks_items(ref List<abc_calc_agg> stage)
        {
            try
            {
                int i;
                i = 0;
                while (i < stage.Count)
                {
                    if ((stage[i].value_1 == null) && (stage[i].value_2 == null) && (stage[i].value_3 == null) && (stage[i].value_4 == null) && (stage[i].value_5 == null) && (stage[i].value_6 == null) && (stage[i].value_7 == null) && (stage[i].value_8 == null))
                    {
                        stage.RemoveAt(i);
                        i = i - 1;
                    }
                    i = i + 1;
                }
            }
            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "remove_blank_items", e.Message, true, ref certificate);
                errors.Add(log_error);
            }
        }
        void assign_values(ref List<abc_calc_agg> stage)
        {
            try
            {
                var allresults = new List<abc_calc_agg>();
                List<abc_calc_agg> results;

                // assign calendried portion ratio to stage

                var cpquery = from v in stage
                              join s in entity_period_tbl_info on new { j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                              equals new { j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }

                              select new abc_calc_agg
                              {
                                  entity_id = v.entity_id,
                                  parent_entity_id = v.parent_entity_id,
                                  year = v.year,
                                  period_id = v.period_id,
                                  result_row_id = v.result_row_id,
                                  contributor_id = v.contributor_id,
                                  workflow_stage = v.workflow_stage,
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
                                  num_years = v.num_years,
                                  contributor_1_id = v.contributor_1_id,
                                  contributor_2_id = v.contributor_2_id,
                                  calculation_id = v.calculation_id,
                                  operand_1_is_weight = v.operand_1_is_weight,
                                  operand_2_is_weight = v.operand_2_is_weight,
                                  operand_3_is_weight = v.operand_3_is_weight,
                                  operand_4_is_weight = v.operand_4_is_weight,
                                  operand_5_is_weight = v.operand_5_is_weight,
                                  operand_6_is_weight = v.operand_6_is_weight,
                                  operand_7_is_weight = v.operand_7_is_weight,
                                  operand_8_is_weight = v.operand_8_is_weight,
                                  calenderized_year_offset = s.calenderized_year_offset,
                                  class_id = v.class_id,
                              };
                stage = cpquery.ToList<abc_calc_agg>();

                int i;
                var query = from v in stage
                            join s in entity_period_tbl on new { j1 = v.operand_1_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                       equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                            select new abc_calc_agg
                            {
                                entity_id = v.entity_id,
                                parent_entity_id = v.parent_entity_id,
                                year = v.year,
                                period_id = v.period_id,
                                result_row_id = v.result_row_id,
                                contributor_id = v.contributor_id,
                                workflow_stage = v.workflow_stage,

                                raw_value_1 = s.value,
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
                                num_years = v.num_years,
                                contributor_1_id = v.contributor_1_id,
                                contributor_2_id = v.contributor_2_id,
                                calculation_id = v.calculation_id,
                                operand_1_is_weight = v.operand_1_is_weight,
                                operand_2_is_weight = v.operand_2_is_weight,
                                operand_3_is_weight = v.operand_3_is_weight,
                                operand_4_is_weight = v.operand_4_is_weight,
                                operand_5_is_weight = v.operand_5_is_weight,
                                operand_6_is_weight = v.operand_6_is_weight,
                                operand_7_is_weight = v.operand_7_is_weight,
                                operand_8_is_weight = v.operand_8_is_weight,
                                class_id = v.class_id,
                            };
                results = query.ToList<abc_calc_agg>();
                //int i;
                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_1_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                    equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }
                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_1_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }




                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_2_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            op1_curr_type = v.op1_curr_type,
                            op2_curr_type = v.op2_curr_type,
                            op3_curr_type = v.op3_curr_type,
                            op4_curr_type = v.op4_curr_type,
                            op5_curr_type = v.op5_curr_type,
                            op6_curr_type = v.op6_curr_type,
                            op7_curr_type = v.op7_curr_type,
                            op8_curr_type = v.op8_curr_type,
                            raw_value_2 = s.value,
                            operand_1_id = v.operand_1_id,
                            operand_2_id = v.operand_2_id,
                            operand_3_id = v.operand_3_id,
                            operand_4_id = v.operand_4_id,
                            operand_5_id = v.operand_5_id,
                            operand_6_id = v.operand_6_id,
                            operand_7_id = v.operand_7_id,
                            operand_8_id = v.operand_8_id,
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_2_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                       equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_2_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_3_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,

                            raw_value_3 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_3_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                         equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_3_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_4_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,

                            raw_value_4 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_4_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                        equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_4_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }

                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_5_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,

                            raw_value_5 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_5_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                         equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_5_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }

                //
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_6_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,

                            raw_value_6 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_6_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                              equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_6_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                //
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_7_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,

                            raw_value_7 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_7_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                             equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_7_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }



                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_8_id, j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                   equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,

                            raw_value_8 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                query = from v in stage
                        join s in entity_period_tbl on new { j1 = v.operand_8_id, j3 = v.entity_id, j4 = v.year + v.calenderized_year_offset, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                            equals new { j1 = s.item_id, j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }


                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
                            raw_value_8_1 = s.value,
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
                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                results = query.ToList<abc_calc_agg>();

                for (i = 0; i <= results.Count - 1; i++)
                {
                    allresults.Add(results[i]);
                }
                // now group these together
                query = from v in allresults
                        group v by new { v.op1_curr_type, v.op2_curr_type, v.op3_curr_type, v.op4_curr_type, v.op5_curr_type, v.op6_curr_type, v.op7_curr_type, v.op8_curr_type, v.parent_entity_id, v.entity_id, v.year, v.result_row_id, v.contributor_id, v.period_id, v.workflow_stage, v.num_years, v.contributor_1_id, v.contributor_2_id, v.operand_1_id, v.operand_2_id, v.operand_3_id, v.operand_4_id, v.operand_5_id, v.operand_6_id, v.operand_7_id, v.operand_8_id, v.calculation_id, v.operand_1_is_weight, v.operand_2_is_weight, v.operand_3_is_weight, v.operand_4_is_weight, v.operand_5_is_weight, v.operand_6_is_weight, v.operand_7_is_weight, v.operand_8_is_weight, v.class_id } into g
                        select new abc_calc_agg
                        {
                            entity_id = g.Key.entity_id,
                            parent_entity_id = g.Key.parent_entity_id,
                            year = g.Key.year,
                            period_id = g.Key.period_id,
                            result_row_id = g.Key.result_row_id,
                            contributor_id = g.Key.contributor_id,
                            workflow_stage = g.Key.workflow_stage,
                            raw_value_1 = g.Max(x => x.raw_value_1),
                            raw_value_2 = g.Max(x => x.raw_value_2),
                            raw_value_3 = g.Max(x => x.raw_value_3),
                            raw_value_4 = g.Max(x => x.raw_value_4),
                            raw_value_5 = g.Max(x => x.raw_value_5),
                            raw_value_6 = g.Max(x => x.raw_value_6),
                            raw_value_7 = g.Max(x => x.raw_value_7),
                            raw_value_8 = g.Max(x => x.raw_value_8),
                            raw_value_1_1 = g.Max(x => x.raw_value_1_1),
                            raw_value_2_1 = g.Max(x => x.raw_value_2_1),
                            raw_value_3_1 = g.Max(x => x.raw_value_3_1),
                            raw_value_4_1 = g.Max(x => x.raw_value_4_1),
                            raw_value_5_1 = g.Max(x => x.raw_value_5_1),
                            raw_value_6_1 = g.Max(x => x.raw_value_6_1),
                            raw_value_7_1 = g.Max(x => x.raw_value_7_1),
                            raw_value_8_1 = g.Max(x => x.raw_value_8_1),
                            operand_1_id = g.Key.operand_1_id,
                            operand_2_id = g.Key.operand_2_id,
                            operand_3_id = g.Key.operand_3_id,
                            operand_4_id = g.Key.operand_4_id,
                            operand_5_id = g.Key.operand_5_id,
                            operand_6_id = g.Key.operand_6_id,
                            operand_7_id = g.Key.operand_7_id,
                            operand_8_id = g.Key.operand_8_id,
                            op1_curr_type = g.Key.op1_curr_type,
                            op2_curr_type = g.Key.op2_curr_type,
                            op3_curr_type = g.Key.op3_curr_type,
                            op4_curr_type = g.Key.op4_curr_type,
                            op5_curr_type = g.Key.op5_curr_type,
                            op6_curr_type = g.Key.op6_curr_type,
                            op7_curr_type = g.Key.op7_curr_type,
                            op8_curr_type = g.Key.op8_curr_type,
                            num_years = g.Key.num_years,
                            contributor_1_id = g.Key.contributor_1_id,
                            contributor_2_id = g.Key.contributor_2_id,
                            calculation_id = g.Key.calculation_id,
                            operand_1_is_weight = g.Key.operand_1_is_weight,
                            operand_2_is_weight = g.Key.operand_2_is_weight,
                            operand_3_is_weight = g.Key.operand_3_is_weight,
                            operand_4_is_weight = g.Key.operand_4_is_weight,
                            operand_5_is_weight = g.Key.operand_5_is_weight,
                            operand_6_is_weight = g.Key.operand_6_is_weight,
                            operand_7_is_weight = g.Key.operand_7_is_weight,
                            operand_8_is_weight = g.Key.operand_8_is_weight,
                            class_id = g.Key.class_id,
                        };

                stage = query.ToList<abc_calc_agg>();


                // aportion data
                int k;


                query = from v in stage
                        join s in entity_period_tbl_info on new { j3 = v.entity_id, j4 = v.year, j5 = v.period_id, j6 = v.contributor_id, j8 = v.workflow_stage }
                    equals new { j3 = s.entity_id, j4 = s.year, j5 = s.period_id, j6 = s.contributor_Id, j8 = s.workflow_stage }
                    //where s.calenderized_portion_ratio != 0

                        select new abc_calc_agg
                        {
                            entity_id = v.entity_id,
                            parent_entity_id = v.parent_entity_id,
                            year = v.year,
                            period_id = v.period_id,
                            result_row_id = v.result_row_id,
                            contributor_id = v.contributor_id,
                            workflow_stage = v.workflow_stage,
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
                            value_1 =
           (
                v.op1_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_1 * s.calenderized_portion_ratio) + (v.raw_value_1_1 * (1 - s.calenderized_portion_ratio))) :
                v.op1_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_1 :
                v.op1_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_1 * s.calenderized_portion_ratio) + (v.raw_value_1_1 * (1 - s.calenderized_portion_ratio))) :
                v.op1_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_1 :
                v.op1_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_1 * s.calenderized_portion_ratio) + (v.raw_value_1_1 * (1 - s.calenderized_portion_ratio))) :
                v.op1_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_1 :
                v.op1_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_1 * s.calenderized_portion_ratio) + (v.raw_value_1_1 * (1 - s.calenderized_portion_ratio))) :
                v.op1_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_1 :
                v.op1_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_1 * s.calenderized_portion_ratio) + (v.raw_value_1_1 * (1 - s.calenderized_portion_ratio)) :
                v.raw_value_1
           ),
                            value_2 =
                          (
                              v.op2_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_2 * s.calenderized_portion_ratio) + (v.raw_value_2_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op2_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_2 :
                 v.op2_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_2 * s.calenderized_portion_ratio) + (v.raw_value_2_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op2_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_2 :
                 v.op2_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_2 * s.calenderized_portion_ratio) + (v.raw_value_2_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op2_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_2 :
                 v.op2_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_2 * s.calenderized_portion_ratio) + (v.raw_value_2_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op2_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_2 :
                 v.op2_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_2 * s.calenderized_portion_ratio) + (v.raw_value_2_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_2
                          ),
                            value_3 =
           (
                             v.op3_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_3 * s.calenderized_portion_ratio) + (v.raw_value_3_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op3_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_3 :
                 v.op3_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_3 * s.calenderized_portion_ratio) + (v.raw_value_3_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op3_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_3 :
                 v.op3_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_3 * s.calenderized_portion_ratio) + (v.raw_value_3_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op3_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_3 :
                 v.op3_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_3 * s.calenderized_portion_ratio) + (v.raw_value_3_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op3_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_3 :
                 v.op3_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_3 * s.calenderized_portion_ratio) + (v.raw_value_3_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_3
           ),
                            value_4 =
                          (
                              v.op4_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_4 * s.calenderized_portion_ratio) + (v.raw_value_4_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op4_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_4 :
                 v.op4_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_4 * s.calenderized_portion_ratio) + (v.raw_value_4_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op4_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_4 :
                 v.op4_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_4 * s.calenderized_portion_ratio) + (v.raw_value_4_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op4_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_4 :
                 v.op4_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_4 * s.calenderized_portion_ratio) + (v.raw_value_4_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op4_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_4 :
                 v.op4_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_4 * s.calenderized_portion_ratio) + (v.raw_value_4_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_4
                          ),
                            value_5 =
           (
                v.op5_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_5 * s.calenderized_portion_ratio) + (v.raw_value_5_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op5_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_5 :
                 v.op5_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_5 * s.calenderized_portion_ratio) + (v.raw_value_5_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op5_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_5 :
                 v.op5_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_5 * s.calenderized_portion_ratio) + (v.raw_value_5_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op5_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_5 :
                 v.op5_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_5 * s.calenderized_portion_ratio) + (v.raw_value_5_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op5_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_5 :
                 v.op5_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_5 * s.calenderized_portion_ratio) + (v.raw_value_5_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_5
           ),
                            value_6 =
                          (
                             v.op6_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_6 * s.calenderized_portion_ratio) + (v.raw_value_6_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op6_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_6 :
                 v.op6_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_6 * s.calenderized_portion_ratio) + (v.raw_value_6_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op6_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_6 :
                 v.op6_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_6 * s.calenderized_portion_ratio) + (v.raw_value_6_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op6_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_6 :
                 v.op6_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_6 * s.calenderized_portion_ratio) + (v.raw_value_6_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op6_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_6 :
                 v.op6_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_6 * s.calenderized_portion_ratio) + (v.raw_value_6_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_6
                          ),
                            value_7 =
           (
              v.op7_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_7 * s.calenderized_portion_ratio) + (v.raw_value_7_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op7_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_7 :
                 v.op7_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_7 * s.calenderized_portion_ratio) + (v.raw_value_7_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op7_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_7 :
                 v.op7_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_7 * s.calenderized_portion_ratio) + (v.raw_value_7_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op7_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_7 :
                 v.op7_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_7 * s.calenderized_portion_ratio) + (v.raw_value_7_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op7_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_7 :
                 v.op7_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_7 * s.calenderized_portion_ratio) + (v.raw_value_7_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_7
           ),
                            value_8 =
                          (
                               v.op8_curr_type == 1 && s.calenderized_portion_ratio != 1 ? s.ecurr_ratio * ((v.raw_value_8 * s.calenderized_portion_ratio) + (v.raw_value_8_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op8_curr_type == 1 && s.calenderized_portion_ratio == 1 ? s.ecurr_ratio * v.raw_value_8 :
                 v.op8_curr_type == 2 && s.calenderized_portion_ratio != 1 ? s.tcurr_ratio * ((v.raw_value_8 * s.calenderized_portion_ratio) + (v.raw_value_8_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op8_curr_type == 2 && s.calenderized_portion_ratio == 1 ? s.tcurr_ratio * v.raw_value_8 :
                 v.op8_curr_type == 3 && s.calenderized_portion_ratio != 1 ? s.ecurr_denom_ratio * ((v.raw_value_8 * s.calenderized_portion_ratio) + (v.raw_value_8_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op8_curr_type == 3 && s.calenderized_portion_ratio == 1 ? s.ecurr_denom_ratio * v.raw_value_8 :
                 v.op8_curr_type == 4 && s.calenderized_portion_ratio != 1 ? s.price_denom_ratio * ((v.raw_value_8 * s.calenderized_portion_ratio) + (v.raw_value_8_1 * (1 - s.calenderized_portion_ratio))) :
                 v.op8_curr_type == 4 && s.calenderized_portion_ratio == 1 ? s.price_denom_ratio * v.raw_value_8 :
                 v.op8_curr_type == 0 && s.calenderized_portion_ratio != 1 ? (v.raw_value_8 * s.calenderized_portion_ratio) + (v.raw_value_8_1 * (1 - s.calenderized_portion_ratio)) :
                 v.raw_value_8
                          ),


                            num_years = v.num_years,
                            contributor_1_id = v.contributor_1_id,
                            contributor_2_id = v.contributor_2_id,
                            calculation_id = v.calculation_id,
                            operand_1_is_weight = v.operand_1_is_weight,
                            operand_2_is_weight = v.operand_2_is_weight,
                            operand_3_is_weight = v.operand_3_is_weight,
                            operand_4_is_weight = v.operand_4_is_weight,
                            operand_5_is_weight = v.operand_5_is_weight,
                            operand_6_is_weight = v.operand_6_is_weight,
                            operand_7_is_weight = v.operand_7_is_weight,
                            operand_8_is_weight = v.operand_8_is_weight,
                            class_id = v.class_id,
                        };
                stage = query.ToList<abc_calc_agg>();

            }
            catch (Exception e)
            {
                error_activity log_error = new error_activity("bc_am_aggs_serviced_based", "assign_values", e.Message, true, ref certificate);
                errors.Add(log_error);
            }
        }
    }
}
class target
{
    long _entity_id;
    long _dual_entity_id;
    long _currency_id;
    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }
    public long dual_entity_id
    {
        get { return _dual_entity_id; }
        set { _dual_entity_id = value; }
    }
    public long currency_id
    {
        get { return _currency_id; }
        set { _currency_id = value; }
    }

}

class entity_period_end_tbl_row
{
    long _entity_id;
    int _year;
    int _period_id;
    int _contributor_Id;
    int _workflow_stage;
    decimal _calenderized_portion_ratio;
    int _calenderized_year_offset;
    decimal _ecurr_ratio;
    decimal _tcurr_ratio;
    decimal _ecurr_denom_ratio;
    decimal _price_denom_ratio;




    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }
    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }
    public int contributor_Id
    {
        get { return _contributor_Id; }
        set { _contributor_Id = value; }
    }
    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }
    public decimal calenderized_portion_ratio
    {
        get { return _calenderized_portion_ratio; }
        set { _calenderized_portion_ratio = value; }
    }
    public int calenderized_year_offset
    {
        get { return _calenderized_year_offset; }
        set { _calenderized_year_offset = value; }
    }
    public decimal ecurr_ratio
    {
        get { return _ecurr_ratio; }
        set { _ecurr_ratio = value; }
    }
    public decimal tcurr_ratio
    {
        get { return _tcurr_ratio; }
        set { _tcurr_ratio = value; }
    }
    public decimal ecurr_denom_ratio
    {
        get { return _ecurr_denom_ratio; }
        set { _ecurr_denom_ratio = value; }
    }
    public decimal price_denom_ratio
    {
        get { return _price_denom_ratio; }
        set { _price_denom_ratio = value; }
    }


}

class entity_period_tbl_row
{
    long _entity_id;
    long _item_id;
    int _year;
    int _period_id;
    int _contributor_Id;
    int _workflow_stage;
    decimal _portion_ratio;
    int _calendar_year_offset;
    decimal _calenderized_portion_ratio;
    decimal _conversion_exch_ratio;
    decimal _value;
    decimal _caldendar_value;
    decimal _curr_type;
    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }
    public long item_id
    {
        get { return _item_id; }
        set { _item_id = value; }
    }
    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }

    public int contributor_Id
    {
        get { return _contributor_Id; }
        set { _contributor_Id = value; }
    }
    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }
    public decimal portion_ratio
    {
        get { return _portion_ratio; }
        set { _portion_ratio = value; }
    }
    public int calendar_year_offset
    {
        get { return _calendar_year_offset; }
        set { _calendar_year_offset = value; }
    }
    public decimal calenderized_portion_ratio
    {
        get { return _calenderized_portion_ratio; }
        set { _calenderized_portion_ratio = value; }
    }
    public decimal conversion_exch_ratio
    {
        get { return _conversion_exch_ratio; }
        set { _conversion_exch_ratio = value; }
    }
    public decimal value
    {
        get { return _value; }
        set { _value = value; }
    }
    public decimal caldendar_value
    {
        get { return _caldendar_value; }
        set { _caldendar_value = value; }
    }
    public decimal curr_type
    {
        get { return _curr_type; }
        set { _curr_type = value; }
    }

}

class entity
{
    long _key = 1;
    long _entity_id;
    long _class_id;
    bool _is_parent;
    long _parent_entity_id;





    public long key
    {
        get { return _key; }
        set { _key = value; }
    }

    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }
    public long class_id
    {
        get { return _class_id; }
        set { _class_id = value; }
    }

    public bool is_parent
    {
        get { return _is_parent; }
        set { _is_parent = value; }
    }

    public long parent_entity_id
    {
        get { return _parent_entity_id; }
        set { _parent_entity_id = value; }
    }
}

public class year_period
{
    long _key = 1;
    int _year;
    int _period_id;
    int _contributor_id;
    int _workflow_stage;
    int _growth_only = 0;

    public int growth_only
    {
        get { return _growth_only; }
        set { _growth_only = value; }
    }


    public long key
    {
        get { return _key; }
        set { _key = value; }
    }

    public int year
    {
        get { return _year; }
        set { _year = value; }
    }

    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }

    public int contributor_id
    {
        get { return _contributor_id; }
        set { _contributor_id = value; }
    }

    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }

}
public class calculation
{
    long _key = 1;
    long _result_row_id;

    long _operand_1_id;
    long _operand_2_id;
    long _operand_3_id;
    long _operand_4_id;
    long _operand_5_id;
    long _operand_6_id;
    long _operand_7_id;
    long _operand_8_id;
    bool _operand_1_is_weight = false;
    bool _operand_2_is_weight = false;
    bool _operand_3_is_weight = false;
    bool _operand_4_is_weight = false;
    bool _operand_5_is_weight = false;
    bool _operand_6_is_weight = false;
    bool _operand_7_is_weight = false;
    bool _operand_8_is_weight = false;
    string _formula;
    int _num_years;
    int _contributor_1;
    int _contributor_2;
    int _interval_type;
    int _interval;
    int _op1_curr_type;
    int _op2_curr_type;
    int _op3_curr_type;
    int _op4_curr_type;
    int _op5_curr_type;
    int _op6_curr_type;
    int _op7_curr_type;
    int _op8_curr_type;
    long _calculation_id;
    string _calc_type;

    public long calculation_id
    {
        get { return _calculation_id; }
        set { _calculation_id = value; }
    }


    public int op1_curr_type
    {
        get { return _op1_curr_type; }
        set { _op1_curr_type = value; }
    }
    public int op2_curr_type
    {
        get { return _op2_curr_type; }
        set { _op2_curr_type = value; }
    }
    public int op3_curr_type
    {
        get { return _op3_curr_type; }
        set { _op3_curr_type = value; }
    }
    public int op4_curr_type
    {
        get { return _op4_curr_type; }
        set { _op4_curr_type = value; }
    }
    public int op5_curr_type
    {
        get { return _op5_curr_type; }
        set { _op5_curr_type = value; }
    }
    public int op6_curr_type
    {
        get { return _op6_curr_type; }
        set { _op6_curr_type = value; }
    }
    public int op7_curr_type
    {
        get { return _op7_curr_type; }
        set { _op7_curr_type = value; }
    }
    public int op8_curr_type
    {
        get { return _op8_curr_type; }
        set { _op8_curr_type = value; }
    }
    public long key
    {
        get { return _key; }
        set { _key = value; }
    }
    public string calc_type
    {
        get { return _calc_type; }
        set { _calc_type = value; }
    }

    public long result_row_id
    {
        get { return _result_row_id; }
        set { _result_row_id = value; }
    }

    public bool operand_1_is_weight
    {
        get { return _operand_1_is_weight; }
        set { _operand_1_is_weight = value; }
    }
    public bool operand_2_is_weight
    {
        get { return _operand_2_is_weight; }
        set { _operand_2_is_weight = value; }
    }
    public bool operand_3_is_weight
    {
        get { return _operand_3_is_weight; }
        set { _operand_3_is_weight = value; }
    }
    public bool operand_4_is_weight
    {
        get { return _operand_4_is_weight; }
        set { _operand_4_is_weight = value; }
    }
    public bool operand_5_is_weight
    {
        get { return _operand_5_is_weight; }
        set { _operand_5_is_weight = value; }
    }
    public bool operand_6_is_weight
    {
        get { return _operand_6_is_weight; }
        set { _operand_6_is_weight = value; }
    }
    public bool operand_7_is_weight
    {
        get { return _operand_7_is_weight; }
        set { _operand_7_is_weight = value; }
    }
    public bool operand_8_is_weight
    {
        get { return _operand_8_is_weight; }
        set { _operand_8_is_weight = value; }
    }
    public long operand_1_id
    {
        get { return _operand_1_id; }
        set { _operand_1_id = value; }
    }
    public long operand_2_id
    {
        get { return _operand_2_id; }
        set { _operand_2_id = value; }
    }
    public long operand_3_id
    {
        get { return _operand_3_id; }
        set { _operand_3_id = value; }
    }
    public long operand_4_id
    {
        get { return _operand_4_id; }
        set { _operand_4_id = value; }
    }
    public long operand_5_id
    {
        get { return _operand_5_id; }
        set { _operand_5_id = value; }
    }
    public long operand_6_id
    {
        get { return _operand_6_id; }
        set { _operand_6_id = value; }
    }
    public long operand_7_id
    {
        get { return _operand_7_id; }
        set { _operand_7_id = value; }
    }
    public long operand_8_id
    {
        get { return _operand_8_id; }
        set { _operand_8_id = value; }
    }
    public string formula
    {
        get { return _formula; }
        set { _formula = value; }
    }

    public int num_years
    {
        get { return _num_years; }
        set { _num_years = value; }
    }
    public int contributor_1
    {
        get { return _contributor_1; }
        set { _contributor_1 = value; }
    }
    public int contributor_2
    {
        get { return _contributor_2; }
        set { _contributor_2 = value; }
    }
    public int interval_type
    {
        get { return _interval_type; }
        set { _interval_type = value; }
    }
    public int interval
    {
        get { return _interval; }
        set { _interval = value; }
    }
}
public class abc_calc_agg
{
    long _target_entity_id;
    long _dual_entity_id;
    long _entity_id;
    long _class_id;
    int _year;
    int _period_id;
    int _workflow_stage;
    int _contributor_id;
    long _result_row_id;
    Nullable<decimal> _raw_value_1 = null;
    Nullable<decimal> _raw_value_2 = null;
    Nullable<decimal> _raw_value_3 = null;
    Nullable<decimal> _raw_value_4 = null;
    Nullable<decimal> _raw_value_5 = null;
    Nullable<decimal> _raw_value_6 = null;
    Nullable<decimal> _raw_value_7 = null;
    Nullable<decimal> _raw_value_8 = null;
    Nullable<decimal> _raw_value_1_1 = null;
    Nullable<decimal> _raw_value_2_1 = null;
    Nullable<decimal> _raw_value_3_1 = null;
    Nullable<decimal> _raw_value_4_1 = null;
    Nullable<decimal> _raw_value_5_1 = null;
    Nullable<decimal> _raw_value_6_1 = null;
    Nullable<decimal> _raw_value_7_1 = null;
    Nullable<decimal> _raw_value_8_1 = null;
    Nullable<decimal> _value_1 = null;
    Nullable<decimal> _value_2 = null;
    Nullable<decimal> _value_3 = null;
    Nullable<decimal> _value_4 = null;
    Nullable<decimal> _value_5 = null;
    Nullable<decimal> _value_6 = null;
    Nullable<decimal> _value_7 = null;
    Nullable<decimal> _value_8 = null;
    long _operand_1_id;
    long _operand_2_id;
    long _operand_3_id;
    long _operand_4_id;
    long _operand_5_id;
    long _operand_6_id;
    long _operand_7_id;
    long _operand_8_id;
    bool _operand_1_is_weight;
    bool _operand_2_is_weight;
    bool _operand_3_is_weight;
    bool _operand_4_is_weight;
    bool _operand_5_is_weight;
    bool _operand_6_is_weight;
    bool _operand_7_is_weight;
    bool _operand_8_is_weight;
    string _formula;
    long _parent_entity_id;
    int _num_years;

    int _interval_type;
    int _interval;
    bool _include_in_growthr = false;
    bool _include_in_growthl = false;
    long _contributor_1_id;
    long _contributor_2_id;

    int _op1_curr_type;
    int _op2_curr_type;
    int _op3_curr_type;
    int _op4_curr_type;
    int _op5_curr_type;
    int _op6_curr_type;
    int _op7_curr_type;
    int _op8_curr_type;
    long _host_curr_id;
    long _calculation_id;

    int _calenderized_year_offset;
    DateTime _period_end_date;
    Nullable<decimal> _weighting = null;

    public long calculation_id
    {
        get { return _calculation_id; }
        set { _calculation_id = value; }
    }
    public long class_id
    {
        get { return _class_id; }
        set { _class_id = value; }
    }
    public DateTime period_end_date
    {
        get { return _period_end_date; }
        set { _period_end_date = value; }
    }


    public long host_curr_id
    {
        get { return _host_curr_id; }
        set { _host_curr_id = value; }
    }

    public int op1_curr_type
    {
        get { return _op1_curr_type; }
        set { _op1_curr_type = value; }
    }
    public int op2_curr_type
    {
        get { return _op2_curr_type; }
        set { _op2_curr_type = value; }
    }
    public int op3_curr_type
    {
        get { return _op3_curr_type; }
        set { _op3_curr_type = value; }
    }
    public int op4_curr_type
    {
        get { return _op4_curr_type; }
        set { _op4_curr_type = value; }
    }
    public int op5_curr_type
    {
        get { return _op5_curr_type; }
        set { _op5_curr_type = value; }
    }
    public int op6_curr_type
    {
        get { return _op6_curr_type; }
        set { _op6_curr_type = value; }
    }
    public int op7_curr_type
    {
        get { return _op7_curr_type; }
        set { _op7_curr_type = value; }
    }
    public int op8_curr_type
    {
        get { return _op8_curr_type; }
        set { _op8_curr_type = value; }
    }


    public long contributor_1_id
    {
        get { return _contributor_1_id; }
        set { _contributor_1_id = value; }
    }
    public long contributor_2_id
    {
        get { return _contributor_2_id; }
        set { _contributor_2_id = value; }
    }
    public bool include_in_growthr
    {
        get { return _include_in_growthr; }
        set { _include_in_growthr = value; }
    }
    public bool include_in_growthl
    {
        get { return _include_in_growthl; }
        set { _include_in_growthl = value; }
    }

    public long target_entity_id
    {
        get { return _target_entity_id; }
        set { _target_entity_id = value; }
    }
    public long dual_entity_id
    {
        get { return _dual_entity_id; }
        set { _dual_entity_id = value; }
    }

    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }
    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }
    public int item_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }
    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }
    public int contributor_id
    {
        get { return _contributor_id; }
        set { _contributor_id = value; }
    }
    public long result_row_id
    {
        get { return _result_row_id; }
        set { _result_row_id = value; }
    }

    public Nullable<decimal> raw_value_1
    {
        get { return _raw_value_1; }
        set { _raw_value_1 = value; }
    }
    public Nullable<decimal> raw_value_2
    {
        get { return _raw_value_2; }
        set { _raw_value_2 = value; }
    }
    public Nullable<decimal> raw_value_3
    {
        get { return _raw_value_3; }
        set { _raw_value_3 = value; }
    }
    public Nullable<decimal> raw_value_4
    {
        get { return _raw_value_4; }
        set { _raw_value_4 = value; }
    }
    public Nullable<decimal> raw_value_5
    {
        get { return _raw_value_5; }
        set { _raw_value_5 = value; }
    }
    public Nullable<decimal> raw_value_6
    {
        get { return _raw_value_6; }
        set { _raw_value_6 = value; }
    }
    public Nullable<decimal> raw_value_7
    {
        get { return _raw_value_7; }
        set { _raw_value_7 = value; }
    }
    public Nullable<decimal> raw_value_8
    {
        get { return _raw_value_8; }
        set { _raw_value_8 = value; }
    }
    public Nullable<decimal> raw_value_1_1
    {
        get { return _raw_value_1_1; }
        set { _raw_value_1_1 = value; }
    }
    public Nullable<decimal> raw_value_2_1
    {
        get { return _raw_value_2_1; }
        set { _raw_value_2_1 = value; }
    }
    public Nullable<decimal> raw_value_3_1
    {
        get { return _raw_value_3_1; }
        set { _raw_value_3_1 = value; }
    }
    public Nullable<decimal> raw_value_4_1
    {
        get { return _raw_value_4_1; }
        set { _raw_value_4_1 = value; }
    }
    public Nullable<decimal> raw_value_5_1
    {
        get { return _raw_value_5_1; }
        set { _raw_value_5_1 = value; }
    }
    public Nullable<decimal> raw_value_6_1
    {
        get { return _raw_value_6_1; }
        set { _raw_value_6_1 = value; }
    }
    public Nullable<decimal> raw_value_7_1
    {
        get { return _raw_value_7_1; }
        set { _raw_value_7_1 = value; }
    }
    public Nullable<decimal> raw_value_8_1
    {
        get { return _raw_value_8_1; }
        set { _raw_value_8_1 = value; }
    }
    public Nullable<decimal> value_8_1
    {
        get { return _raw_value_8_1; }
        set { _raw_value_8_1 = value; }
    }
    public Nullable<decimal> value_1
    {
        get { return _value_1; }
        set { _value_1 = value; }
    }
    public Nullable<decimal> value_2
    {
        get { return _value_2; }
        set { _value_2 = value; }
    }
    public Nullable<decimal> value_3
    {
        get { return _value_3; }
        set { _value_3 = value; }
    }
    public Nullable<decimal> value_4
    {
        get { return _value_4; }
        set { _value_4 = value; }
    }
    public Nullable<decimal> value_5
    {
        get { return _value_5; }
        set { _value_5 = value; }
    }
    public Nullable<decimal> value_6
    {
        get { return _value_6; }
        set { _value_6 = value; }
    }
    public Nullable<decimal> value_7
    {
        get { return _value_7; }
        set { _value_7 = value; }
    }
    public Nullable<decimal> value_8
    {
        get { return _value_8; }
        set { _value_8 = value; }
    }
    public Nullable<decimal> weighting
    {
        get { return _weighting; }
        set { _weighting = value; }
    }

    public long operand_1_id
    {
        get { return _operand_1_id; }
        set { _operand_1_id = value; }
    }
    public long operand_2_id
    {
        get { return _operand_2_id; }
        set { _operand_2_id = value; }
    }
    public long operand_3_id
    {
        get { return _operand_3_id; }
        set { _operand_3_id = value; }
    }
    public long operand_4_id
    {
        get { return _operand_4_id; }
        set { _operand_4_id = value; }
    }
    public long operand_5_id
    {
        get { return _operand_5_id; }
        set { _operand_5_id = value; }
    }
    public long operand_6_id
    {
        get { return _operand_6_id; }
        set { _operand_6_id = value; }
    }
    public long operand_7_id
    {
        get { return _operand_7_id; }
        set { _operand_7_id = value; }
    }
    public long operand_8_id
    {
        get { return _operand_8_id; }
        set { _operand_8_id = value; }
    }

    public bool operand_1_is_weight
    {
        get { return _operand_1_is_weight; }
        set { _operand_1_is_weight = value; }
    }
    public bool operand_2_is_weight
    {
        get { return _operand_2_is_weight; }
        set { _operand_2_is_weight = value; }
    }
    public bool operand_3_is_weight
    {
        get { return _operand_3_is_weight; }
        set { _operand_3_is_weight = value; }
    }
    public bool operand_4_is_weight
    {
        get { return _operand_4_is_weight; }
        set { _operand_4_is_weight = value; }
    }
    public bool operand_5_is_weight
    {
        get { return _operand_5_is_weight; }
        set { _operand_5_is_weight = value; }
    }
    public bool operand_6_is_weight
    {
        get { return _operand_6_is_weight; }
        set { _operand_6_is_weight = value; }
    }
    public bool operand_7_is_weight
    {
        get { return _operand_7_is_weight; }
        set { _operand_7_is_weight = value; }
    }
    public bool operand_8_is_weight
    {
        get { return _operand_8_is_weight; }
        set { _operand_8_is_weight = value; }
    }

    public string formula
    {
        get { return _formula; }
        set { _formula = value; }
    }
    public long parent_entity_id
    {
        get { return _parent_entity_id; }
        set { _parent_entity_id = value; }
    }
    public int num_years
    {
        get { return _num_years; }
        set { _num_years = value; }
    }

    public int interval_type
    {
        get { return _interval_type; }
        set { _interval_type = value; }
    }
    public int interval
    {
        get { return _interval; }
        set { _interval = value; }
    }
    public int calenderized_year_offset
    {
        get { return _calenderized_year_offset; }
        set { _calenderized_year_offset = value; }
    }


}

public class agg_result
{

    int _year;
    int _period_id;
    long _item_id;
    int _contributor_id;
    int _workflow_stage;
    long _universe_id;
    long _target_entity_id;
    long _dual_entity_id;
    Nullable<decimal> _value;
    Nullable<decimal> _lvalue;
    Nullable<decimal> _rvalue;
    int _num_years;
    long _contributor_1_id;
    long _contributor_2_id;
    int _exch_type;
    long _audit_id;
    long _currency;

    public long contributor_1_id
    {
        get { return _contributor_1_id; }
        set { _contributor_1_id = value; }
    }
    public long contributor_2_id
    {
        get { return _contributor_2_id; }
        set { _contributor_2_id = value; }
    }
    public int exch_type
    {
        get { return _exch_type; }
        set { _exch_type = value; }
    }
    public long audit_id
    {
        get { return _audit_id; }
        set { _audit_id = value; }
    }
    public long currency
    {
        get { return _currency; }
        set { _currency = value; }
    }


    public long target_entity_id
    {
        get { return _target_entity_id; }
        set { _target_entity_id = value; }
    }
    public long dual_entity_id
    {
        get { return _dual_entity_id; }
        set { _dual_entity_id = value; }
    }

    public int num_years
    {
        get { return _num_years; }
        set { _num_years = value; }
    }

    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }
    public long item_id
    {
        get { return _item_id; }
        set { _item_id = value; }
    }
    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }
    public int contributor_id
    {
        get { return _contributor_id; }
        set { _contributor_id = value; }
    }

    public long universe_id
    {
        get { return _universe_id; }
        set { _universe_id = value; }
    }


    public Nullable<decimal> value
    {
        get { return _value; }
        set { _value = value; }
    }
    public Nullable<decimal> lvalue
    {
        get { return _lvalue; }
        set { _lvalue = value; }
    }
    public Nullable<decimal> rvalue
    {
        get { return _rvalue; }
        set { _rvalue = value; }
    }
}

public class local_currency_convert
{
    long _currency_id;
    int _year;
    int _period_id;
    decimal _rate;
    DateTime _date_at;

    public DateTime date_at
    {
        get { return _date_at; }
        set { _date_at = value; }
    }
    public long currency_id
    {
        get { return _currency_id; }
        set { _currency_id = value; }
    }
    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }
    public decimal rate
    {
        get { return _rate; }
        set { _rate = value; }
    }
}

public class entity_weighting_target
{
    long _entity_id;
    long _parent_entity_id;
    long _target_entity_id;
    long _attribute_id;
    decimal _weighting;

    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }

    public long parent_entity_id
    {
        get { return _parent_entity_id; }
        set { _parent_entity_id = value; }
    }

    public long target_entity_id
    {
        get { return _target_entity_id; }
        set { _target_entity_id = value; }
    }
    public long attribute_id
    {
        get { return _attribute_id; }
        set { _attribute_id = value; }
    }
    public decimal weighting
    {
        get { return _weighting; }
        set { _weighting = value; }
    }
}

public class entity_target
{
    long _entity_id;
    long _target_entity_id;
    long _dual_entity_id;
    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }
    public long target_entity_id
    {
        get { return _target_entity_id; }
        set { _target_entity_id = value; }
    }
    public long dual_entity_id
    {
        get { return _dual_entity_id; }
        set { _dual_entity_id = value; }
    }
}
public class universe_formula
{
    double _standard_deviation_mult;
    long _result_row_id;
    string _db_formula;
    string _linq_formula;
    bool _growth;
    bool _cross_contributor;
    string _where;
    string _having;
    string _calculation_type;
    string _style_calculation_type;

    public double standard_deviation_mult
    {
        get { return _standard_deviation_mult; }
        set { _standard_deviation_mult = value; }
    }


    public string where
    {
        get { return _where; }
        set { _where = value; }
    }

    public string having
    {
        get { return _having; }
        set { _having = value; }
    }

    public string style_calculation_type
    {
        get { return _style_calculation_type; }
        set { _style_calculation_type = value; }
    }
    public string calculation_type
    {
        get { return _calculation_type; }
        set { _calculation_type = value; }
    }

    public long result_row_id
    {
        get { return _result_row_id; }
        set { _result_row_id = value; }
    }
    public string db_formula
    {
        get { return _db_formula; }
        set { _db_formula = value; }
    }
    public string linq_formula
    {
        get { return _linq_formula; }
        set { _linq_formula = value; }
    }
    public bool growth
    {
        get { return _growth; }
        set { _growth = value; }
    }
    public bool cross_contributor
    {
        get { return _cross_contributor; }
        set { _cross_contributor = value; }
    }
}
public class error_activity
{
    string _class_name;
    string _method_name;
    DateTime _da;
    string _description;
    long _target_entity_id;
    long _dual_entity_id;


    public error_activity(string lclass_name, string lmethod_name, string ldescription, bool berr, ref bc_cs_security.certificate certificate, long target_entity_id = 0, long dual_entity_id = 0)
    {
        bc_cs_error_log err;
        _class_name = lclass_name;
        _method_name = lmethod_name;
        _da = DateTime.Now;
        _description = ldescription;
        _target_entity_id = target_entity_id;
        _dual_entity_id = dual_entity_id;
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
    public long target_entity_id
    {
        get { return _target_entity_id; }
        set { _target_entity_id = value; }
    }
    public long dual_entity_id
    {
        get { return _dual_entity_id; }
        set { _dual_entity_id = value; }
    }
}

[Table(Name = "bc_core_aggregated_data")]
public partial class bc_core_aggregated_data
{
    [Column(IsPrimaryKey = true)]
    public long entity_id;
    [Column(IsPrimaryKey = true)]
    public long dual_entity_id;
    [Column(IsPrimaryKey = true)]
    public int year;
    [Column(IsPrimaryKey = true)]
    public int period_id;
    [Column]
    public DateTime date_from;
    [Column(IsPrimaryKey = true)]
    public DateTime date_to;
    [Column(IsPrimaryKey = true)]
    public int e_a_flag;
    [Column(IsPrimaryKey = true)]
    public long contributor_id;
    [Column(IsPrimaryKey = true)]
    public long item_id;
    [Column]
    public string value;
    [Column(IsPrimaryKey = true)]
    public long workflow_stage;
    [Column(IsPrimaryKey = true)]
    public long acc_standard;
    [Column]
    public string comment;
    [Column]
    public long user_id;
    [Column(IsPrimaryKey = true)]
    public int exch_type;
    [Column]
    public long audit_id;
    [Column]
    public long currency;
    [Column(IsPrimaryKey = true)]
    public long universe_id;
}

[Table(Name = "bc_core_aggregated_data_style")]
public partial class bc_core_aggregated_data_style
{
    [Column(IsPrimaryKey = true)]
    public long entity_id;
    [Column(IsPrimaryKey = true)]
    public long dual_entity_id;
    [Column(IsPrimaryKey = true)]
    public int year;
    [Column(IsPrimaryKey = true)]
    public int period_id;
    [Column]
    public DateTime date_from;
    [Column(IsPrimaryKey = true)]
    public DateTime date_to;
    [Column(IsPrimaryKey = true)]
    public int e_a_flag;
    [Column(IsPrimaryKey = true)]
    public long contributor_id;
    [Column(IsPrimaryKey = true)]
    public long item_id;
    [Column]
    public string mean;
    [Column]
    public string standard_deviation;
    [Column]
    public string effective_stocks;
    [Column(IsPrimaryKey = true)]
    public long workflow_stage;
    [Column(IsPrimaryKey = true)]
    public long acc_standard;
    [Column]
    public string comment;
    [Column]
    public long user_id;
    [Column(IsPrimaryKey = true)]
    public int exch_type;
    [Column]
    public long audit_id;
    [Column]
    public long currency;
    [Column(IsPrimaryKey = true)]
    public long universe_id;
}
[Table(Name = "bc_core_aggs_universe_statistics")]
public partial class bc_core_aggs_universe_statistics
{
    [Column]
    public long audit_id;
    [Column(IsPrimaryKey = true)]
    public DateTime date_from;
    [Column(IsPrimaryKey = true)]
    public DateTime date_to;
    [Column(IsPrimaryKey = true)]
    public long target_entity_id;
    [Column(IsPrimaryKey = true)]
    public long result_row_id;
    [Column(IsPrimaryKey = true)]
    public int year;
    [Column(IsPrimaryKey = true)]
    public long period_id;
    [Column(IsPrimaryKey = true)]
    public int workflow_stage;
    [Column(IsPrimaryKey = true)]
    public long contributor_id;
    [Column(IsPrimaryKey = true)]
    public long class_id;
    [Column]
    public int entity_count;
    [Column(IsPrimaryKey = true)]
    public long aggregation_universe_id;
    [Column]
    public int statistic_type_id;
    [Column(IsPrimaryKey = true)]
    public long dual_entity_id;
    [Column(IsPrimaryKey = true)]
    public int exch_method;
}

[Table(Name = "bc_core_aggs_log")]
public partial class bc_core_aggs_log
{
    [Column(IsPrimaryKey = true, IsDbGenerated = true)]
    public long id;
    [Column]
    public int log_type;
    [Column]
    public long agg_universe_id;
    [Column]
    public long target_entity_id;
    [Column]
    public DateTime batch_date;
    [Column]
    public DateTime log_date;
    [Column]
    public string comment;
    [Column]
    public string sys_error;
    [Column]
    public int audit_id;
    [Column]
    public long dual_entity_id;
}
[Table(Name = "bc_core_service_abc_calc_aggs")]
public partial class bc_core_service_abc_calc_aggs
{
    [Column(IsPrimaryKey = true, IsDbGenerated = true)]
    public long id;
    [Column]
    public long universe_id;
    [Column]
    public string mac_address;
    [Column]
    public long entity_id;
    [Column]
    public int year;
    [Column]
    public int period_id;
    [Column]
    public long workflow_stage;
    [Column]
    public long contributor_id;
    [Column]
    public long result_row_id;
    [Column]
    public decimal value_1;
    [Column]
    public decimal value_2;
    [Column]
    public decimal value_3;
    [Column]
    public decimal value_4;
    [Column]
    public decimal value_5;
    [Column]
    public decimal value_6;
    [Column]
    public decimal value_7;
    [Column]
    public decimal value_8;
    [Column]
    public string formula;
    [Column]
    public long calculation_id;
    [Column]
    public bool include_in_growth_l;
    [Column]
    public bool include_in_growth_r;
    [Column]
    public bool growth;
    [Column]
    public int exch_type;
    [Column]
    public long contributor1_id;
    [Column]
    public long contributor2_id;
    [Column]
    public long target_entity_id;
    [Column]
    public long dual_entity_id;
    [Column]
    public bool momentum;
    [Column]
    public long host_curr_id;
    [Column]
    public bool value_1_null = true;
    [Column]
    public bool value_2_null = true;
    [Column]
    public bool value_3_null = true;
    [Column]
    public bool value_4_null = true;
    [Column]
    public bool value_5_null = true;
    [Column]
    public bool value_6_null = true;
    [Column]
    public bool value_7_null = true;
    [Column]
    public bool value_8_null = true;

}


public class ttest_abc_calc_agg
{
    double _standard_deviation_mult;
    long _entity_id;
    int _year;
    int _period_id;
    int _contributor_id;
    int _workflow_stage;
    long _item_id;
    decimal _linear_result;
    decimal _variance;
    double _winsorized_result;
    decimal _weighting;
    decimal _total_weight;
    double _scaled_weight;
    double _scaled_weight_sq;
    decimal _residual;
    decimal _mean;
    double _weighted_mean;
    double _sd3;
    string _style_calculation_type;
    int _num_years;
    decimal _rvalue;
    decimal _lvalue;
    long _contributor_1_id;
    long _contributor_2_id;

    public double standard_deviation_mult
    {
        get { return _standard_deviation_mult; }
        set { _standard_deviation_mult = value; }
    }

    public int num_years
    {
        get { return _num_years; }
        set { _num_years = value; }
    }

    public string style_calculation_type
    {
        get { return _style_calculation_type; }
        set { _style_calculation_type = value; }
    }
    public long entity_id
    {
        get { return _entity_id; }
        set { _entity_id = value; }
    }


    public long item_id
    {
        get { return _item_id; }
        set { _item_id = value; }
    }

    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }

    public long contributor_1_id
    {
        get { return _contributor_1_id; }
        set { _contributor_1_id = value; }
    }
    public long contributor_2_id
    {
        get { return _contributor_2_id; }
        set { _contributor_2_id = value; }
    }

    public int contributor_id
    {
        get { return _contributor_id; }
        set { _contributor_id = value; }
    }
    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }
    public decimal linear_result
    {
        get { return _linear_result; }
        set { _linear_result = value; }
    }
    public decimal rvalue
    {
        get { return _rvalue; }
        set { _rvalue = value; }
    }
    public decimal lvalue
    {
        get { return _lvalue; }
        set { _lvalue = value; }
    }
    public decimal total_weight
    {
        get { return _total_weight; }
        set { _total_weight = value; }
    }
    public decimal mean
    {
        get { return _mean; }
        set { _mean = value; }
    }

    public double weighted_mean
    {
        get { return _weighted_mean; }
        set { _weighted_mean = value; }
    }

    public decimal variance
    {
        get { return _variance; }
        set { _variance = value; }
    }
    public double winsorized_result
    {
        get { return _winsorized_result; }
        set { _winsorized_result = value; }
    }
    public decimal weighting
    {
        get { return _weighting; }
        set { _weighting = value; }
    }
    public double scaled_weight
    {
        get { return _scaled_weight; }
        set { _scaled_weight = value; }
    }
    public double scaled_weight_sq
    {
        get { return _scaled_weight_sq; }
        set { _scaled_weight_sq = value; }
    }
    public decimal residual
    {
        get { return _residual; }
        set { _residual = value; }
    }
    public double sd3
    {
        get { return _sd3; }
        set { _sd3 = value; }
    }

}
public class ttest_result
{
    double _standard_deviation_mult;
    long _item_id;

    int _year;
    int _period_id;
    int _contributor_id;
    int _workflow_stage;
    decimal _total_weight;
    decimal _mean;
    double _weighted_mean;
    double _standard_deviation;
    double _sd3;
    double _eff_number;
    int _num_points;
    string _style_calculation_type;

    public double standard_deviation_mult
    {
        get { return _standard_deviation_mult; }
        set { _standard_deviation_mult = value; }
    }


    public string style_calculation_type
    {
        get { return _style_calculation_type; }
        set { _style_calculation_type = value; }
    }

    public int num_points
    {
        get { return _num_points; }
        set { _num_points = value; }
    }
    public int year
    {
        get { return _year; }
        set { _year = value; }
    }
    public int period_id
    {
        get { return _period_id; }
        set { _period_id = value; }
    }
    public int contributor_id
    {
        get { return _contributor_id; }
        set { _contributor_id = value; }
    }
    public int workflow_stage
    {
        get { return _workflow_stage; }
        set { _workflow_stage = value; }
    }
    public double standard_deviation
    {
        get { return _standard_deviation; }
        set { _standard_deviation = value; }
    }

    public decimal total_weight
    {
        get { return _total_weight; }
        set { _total_weight = value; }
    }
    public decimal mean
    {
        get { return _mean; }
        set { _mean = value; }
    }
    public double weighted_mean
    {
        get { return _weighted_mean; }
        set { _weighted_mean = value; }
    }
    public double sd3
    {
        get { return _sd3; }
        set { _sd3 = value; }
    }
    public double eff_number
    {
        get { return _eff_number; }
        set { _eff_number = value; }
    }
    public long item_id
    {
        get { return _item_id; }
        set { _item_id = value; }
    }

}
