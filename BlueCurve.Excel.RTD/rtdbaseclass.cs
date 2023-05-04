
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Office.Interop.Excel;

using System.Threading;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using BlueCurve.Insight.AM;
using BlueCurve.Core.AS;

    public abstract class rtdbase : IRtdServer
    {
        protected IRTDUpdateEvent m_callback;
        
        protected Dictionary<int, string> m_topics1;
        protected Dictionary<int, string> m_topics2;
        protected Dictionary<int, string> m_topics3;
        protected Dictionary<int, string> m_topics4;
        protected Dictionary<int, string> m_topics5;
        protected Dictionary<int, string> m_topics6;
        protected Dictionary<int, string> m_topics7;
        protected Dictionary<int, string> m_topics8;
        protected Dictionary<int, string> m_topics9;
        protected Dictionary<int, string> m_topics10;
        protected Dictionary<int, string> m_topics11;
        protected Dictionary<int, string> m_topics12;
        protected Dictionary<int, string> m_topics13;
        protected Dictionary<int, string> m_topics14;
        protected Dictionary<int, string> m_topics15;
        protected Dictionary<int, string> m_topics16;
        protected Dictionary<int, string> m_res;

        SynchronizedCollection<int>  pending_topics = new SynchronizedCollection<int>();
       
        protected Dictionary<string,  Dictionary<int, string>> cache;

        private readonly object monitor = new object();
        protected string sef;
       

        DateTime last_cache_set;
      
        protected int thread_count = 0;

        System.Timers.Timer updtimer;
        System.Timers.Timer pendingtopictimer;

        protected void calcall()
        {
            //clear the cache
            cache.Clear();
            // run each topic in parallel
           
            foreach (int topicId in m_topics1.Keys)
            {
                //Thread.Sleep(bc_am_insight_formats.excel_rtd_thread_sleep);
               
                Thread rtdthread = new Thread(() => evaluate_func(topicId));
                rtdthread.Start();
            }
        }   
       

        protected virtual void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
        }

        private void realtimerefresh(Object source, System.Timers.ElapsedEventArgs e)
        {
           
                    calcall();
        }

        private void callpendingtopic(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                pendingtopictimer.Stop();
                if (pending_topics.Count > 0)
                {
                    //bc_cs_message msg = new bc_cs_message("Blue Curve", pending_topics[0].ToString(), bc_cs_message.MESSAGE);
                    int tid;
                    tid = pending_topics[0];
                    Thread rtdthread = new Thread(() => evaluate_func(tid));
                    rtdthread.Start();
                    pending_topics.RemoveAt(0);
                    //Thread.Sleep(50);
                }
                //while (pending_topics.Count > 0)
                //{
                //    //bc_cs_message msg = new bc_cs_message("Blue Curve", pending_topics[0].ToString(), bc_cs_message.MESSAGE);
                //    int tid;
                //    tid=pending_topics[0];
                //    Thread rtdthread = new Thread(() => evaluate_func(tid));
                //    rtdthread.Start();
                //    pending_topics.RemoveAt(0);
                //    //Thread.Sleep(50);
                //}
                pendingtopictimer.Start();
            }
            catch (Exception err)
            {
                bc_cs_central_settings.activity_file = "on";
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurve.Excel.RTD.callpendingtopic", "Pending topic" , bc_cs_activity_codes.COMMENTARY.ToString(), err.Message.ToString(), ref certificate);
                bc_cs_central_settings.activity_file = "off";
            }
        }
        private void callupdatenotify(Object source, System.Timers.ElapsedEventArgs e)
        {
            bool success = false;
            while (success == false)
            {
                try
                {
                    m_callback.UpdateNotify();
                    updtimer.Stop();
                    success = true;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }
        
        //}
        public int ServerStart(IRTDUpdateEvent callback)
        {
           
           

            bc_cs_central_settings bcs;

            if (bc_cs_central_settings.loaded_for_rtd == false)
                bcs = new bc_cs_central_settings(true);

            if (bc_cs_central_settings.logged_on_user_id==0)
            {
                bc_am_load bc_load = new bc_am_load("Excel Functions", true);
            }

          
            System.Timers.Timer refreshalltimer = new System.Timers.Timer();
            refreshalltimer.Interval = 1000;
            refreshalltimer.Elapsed += refreshall;
            refreshalltimer.Start();

            bc_am_insight_formats inf = new bc_am_insight_formats();
            if (bc_am_insight_formats.excel_rtd_enable_real_time_update == true)
            {
                System.Timers.Timer realtimerefreshtimer = new System.Timers.Timer();
                refreshalltimer.Interval = bc_am_insight_formats.excel_rtd_real_time_update_interval;
                refreshalltimer.Elapsed += realtimerefresh;
                refreshalltimer.Start();
            }
            updtimer = new System.Timers.Timer();
            updtimer.Interval = 1000;
            updtimer.Elapsed += callupdatenotify;

            pendingtopictimer = new System.Timers.Timer();
            pendingtopictimer.Interval = 3;
            pendingtopictimer.Elapsed += callpendingtopic;
            pendingtopictimer.Start();

            thread_count = 0;
            m_callback = callback;


           
            cache = new Dictionary<string, Dictionary<int, string>>();

            m_topics1 = new Dictionary<int, string>();
            m_topics2 = new Dictionary<int, string>();
            m_topics3 = new Dictionary<int, string>();
            m_topics4 = new Dictionary<int, string>();
            m_topics5 = new Dictionary<int, string>();
            m_topics6 = new Dictionary<int, string>();
            m_topics7 = new Dictionary<int, string>();
            m_topics8 = new Dictionary<int, string>();
            m_topics9 = new Dictionary<int, string>();
            m_topics10 = new Dictionary<int, string>();
            m_topics11 = new Dictionary<int, string>();
            m_topics12 = new Dictionary<int, string>();
            m_topics13 = new Dictionary<int, string>();
            m_topics14 = new Dictionary<int, string>();
            m_topics15 = new Dictionary<int, string>();
            m_topics16 = new Dictionary<int, string>();
            m_res = new Dictionary<int, string>();
            
            return 1;
        }

        public void ServerTerminate()
        {
            //bc_cs_message msg = new bc_cs_message("Blue Curve", "Terminate", bc_cs_message.MESSAGE);
        }

        public  object ConnectData(int topicId,
                              ref Array strings,
                              ref bool newValues)
        {

            try
            {

                DateTime d = new DateTime();
                d = DateTime.Now;
                d = d.AddSeconds(-30);


                if (last_cache_set < d)
                {
                    cache.Clear();
                    //bc_cs_message msg = new bc_cs_message("Blue Curve", "clear cache", bc_cs_message.MESSAGE);
                }

                last_cache_set = DateTime.Now;
                int i;
                i = strings.Length;
                if (i > 0)
                    m_topics1[topicId] = strings.GetValue(0).ToString();
                else
                    m_topics1[topicId] = "";
                if (i > 1)
                    m_topics2[topicId] = strings.GetValue(1).ToString();
                else
                    m_topics2[topicId] = "";
                if (i > 2)
                    m_topics3[topicId] = strings.GetValue(2).ToString();
                else
                    m_topics3[topicId] = "";
                if (i > 3)
                    m_topics4[topicId] = strings.GetValue(3).ToString();
                else
                    m_topics4[topicId] = "";
                if (i > 4)
                    m_topics5[topicId] = strings.GetValue(4).ToString();
                else
                    m_topics5[topicId] = "";
                if (i > 5)
                    m_topics6[topicId] = strings.GetValue(5).ToString();
                else
                    m_topics6[topicId] = "";
                if (i > 6)
                    m_topics7[topicId] = strings.GetValue(6).ToString();
                else
                    m_topics7[topicId] = "";
                if (i > 7)
                    m_topics8[topicId] = strings.GetValue(7).ToString();
                else
                    m_topics8[topicId] = "";
                if (i > 8)
                    m_topics9[topicId] = strings.GetValue(8).ToString();
                else
                    m_topics9[topicId] = "";
                if (i > 9)
                    m_topics10[topicId] = strings.GetValue(9).ToString();
                else
                    m_topics10[topicId] = "";
                if (i > 10)
                    m_topics11[topicId] = strings.GetValue(10).ToString();
                else
                    m_topics11[topicId] = "";
                if (i > 11)
                    m_topics12[topicId] = strings.GetValue(11).ToString();
                else
                    m_topics12[topicId] = "";
                if (i > 12)
                    m_topics13[topicId] = strings.GetValue(12).ToString();
                else
                    m_topics13[topicId] = "";
                if (i > 13)
                    m_topics14[topicId] = strings.GetValue(13).ToString();
                else
                    m_topics14[topicId] = "";
                if (i > 14)
                    m_topics15[topicId] = strings.GetValue(14).ToString();
                else
                    m_topics15[topicId] = "";
                if (i > 15)
                    m_topics16[topicId] = strings.GetValue(15).ToString();
                else
                    m_topics16[topicId] = "";
                

                m_res[topicId] = "set: " + topicId.ToString() + "...";


                pending_topics.Add(topicId);
                
                //Thread rtdthread = new Thread(() => evaluate_func(topicId));
                //rtdthread.Start();
                
                    
                return "evaluating...";
            }
            catch (Exception e)
            {
                
                bc_cs_central_settings.activity_file = "on";
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurve.Excel.RTD.bc_get_financial_data", "ConnectData: " + topicId.ToString(), bc_cs_activity_codes.COMMENTARY.ToString(), e.Message.ToString(), ref certificate);
                bc_cs_central_settings.activity_file = "off";
                return "connect err...";

            }
        }
        public Array RefreshData(ref int topicCount)
        {
          
            object[,] data = new object[2, m_topics1.Count];
            try
            {


                int index = 0;

                foreach (int topicId in m_topics1.Keys)
                {
                    data[0, index] = topicId;
                    try
                    {
                        data[1, index] = m_res[topicId].ToString();
                    }
                    catch 
                    {
                        data[1, index] = "refresh err...";
                    }

                    ++index;
                }



                topicCount = m_topics1.Count;
            }
            catch (Exception e)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurve.Excel.RTD.bc_get_financial_data", "RefreshData", bc_cs_activity_codes.COMMENTARY.ToString(), e.Message.ToString(), ref certificate);
            }
        
            return data;
        }

        public int Heartbeat()
        {
            return 100;
        }
        private void evaluate_func(int topicId)
        {
            try
            {
                if (bc_cs_central_settings.logged_on_user_id == 0)
                {
                    m_res[topicId] = "no connection";
                }
                else
                {
                    m_res[topicId] = GetData(topicId,m_topics1[topicId].ToString(), m_topics2[topicId].ToString(), m_topics3[topicId].ToString(), m_topics4[topicId].ToString(), m_topics5[topicId].ToString(), m_topics6[topicId].ToString(), m_topics7[topicId].ToString(), m_topics8[topicId].ToString(), m_topics9[topicId].ToString(), m_topics10[topicId].ToString(), m_topics11[topicId].ToString(), m_topics12[topicId].ToString(), m_topics13[topicId].ToString(), m_topics14[topicId].ToString(), m_topics15[topicId].ToString(), m_topics16[topicId].ToString());
                }
                
                updtimer.Stop();
                updtimer.Start();
            }
            catch (Exception e)
            {
                bc_cs_central_settings.activity_file = "on";
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurve.Excel.RTD.bc_get_financial_data", "evaluate_func: " + topicId.ToString(), bc_cs_activity_codes.COMMENTARY.ToString(), e.Message.ToString(), ref certificate);
                bc_cs_central_settings.activity_file = "off";
            }
            finally
            {
                thread_count = thread_count - 1;
            }
        }

        public void DisconnectData(int topicId)
        {
            try
            {
                m_topics1.Remove(topicId);
                m_topics2.Remove(topicId);
                m_topics3.Remove(topicId);
                m_topics4.Remove(topicId);
                m_topics5.Remove(topicId);
                m_topics6.Remove(topicId);
                m_topics7.Remove(topicId);
                m_topics8.Remove(topicId);
                m_topics9.Remove(topicId);
                m_topics10.Remove(topicId);
                m_topics11.Remove(topicId);
                m_topics12.Remove(topicId);
                m_topics13.Remove(topicId);
                m_topics14.Remove(topicId);
                m_topics15.Remove(topicId);
                m_topics16.Remove(topicId);
                m_res.Remove(topicId);

            }
            catch (Exception e)
            {
                bc_cs_central_settings.activity_file = "on";
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_activity_log ocomm = new bc_cs_activity_log("BlueCurve.Excel.RTD.bc_get_financial_data", "DisconnectData: " + topicId.ToString(), bc_cs_activity_codes.COMMENTARY.ToString(), e.Message.ToString(), ref certificate);
                bc_cs_central_settings.activity_file = "off";
            }
        }
       bool get_value_from_cache(string key, int index, ref string val)
       {
           try
           {
               Dictionary<int, string> it = new Dictionary<int, string>();
               it = cache[key];
               val = "";
               if (it.Count == 0)
                   return false;
               else if (it.Count < index)
                   return true;
               else
               {
                   val = it[index].ToString();
                   return true;
               }
           }
           catch
           {
               return false;
           }
       }
        protected string post(int topicid, string sef, string index ="0")
        {

            // rest post to dedicate service
            string logging = "";
            try
            {
                int iindex=0;
                  try
                  {
                      iindex = Int32.Parse(index);
                  }
                  catch
                  {
                      return "err: index must be numeric";
                  }


                  if (iindex > 1)
                  {
                      string val="";
                      if (get_value_from_cache(sef, iindex, ref val ) == true)
                          return val;

                        
                      else
                      {
                         
                          Thread.Sleep(5);
                          while (get_value_from_cache(sef, iindex, ref val ) == false)
                          {
                              Thread.Sleep(5);
                          }
                          return val;
                         

                      }

                  }
                  else
                  {
                      //bc_cs_message msg = new bc_cs_message("Blue Curve", sef, bc_cs_message.MESSAGE);
                      cache.Remove(sef);
                      logging = bc_cs_central_settings.activity_file;
                      bc_cs_central_settings.activity_file = "off";

                      if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                      {
                        
                          bc_cs_db_services.bc_cs_sql_parameter param;
                          List<bc_cs_db_services.bc_cs_sql_parameter> aparams = new List<bc_cs_db_services.bc_cs_sql_parameter>();

                          param = new bc_cs_db_services.bc_cs_sql_parameter();
                          param.name = "xml_text";
                          aparams.Add(param);
                          param.value = sef;
                          param = new bc_cs_db_services.bc_cs_sql_parameter();
                          param.name = "user_id";
                          param.value = bc_cs_central_settings.logged_on_user_id;
                          aparams.Add(param);


                          bc_cs_db_services db = new bc_cs_db_services();
                          bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                          certificate.user_id = bc_cs_central_settings.logged_on_user_id.ToString();
                          object res;
                          res = db.executesql_with_parameters("dbo.bcc_core_v5_excel_function", aparams, ref certificate);
                          if (certificate.server_errors.Count > 0)
                          {
                              return "db err: " + certificate.server_errors[0];
                          }
                          Array ares;
                          ares = (Array)res;
                          string adores;
                          try
                          {
                              adores= ares.GetValue(0, 0).ToString();
                              if (iindex == 1)
                              {
                                  //convert to list
                                  string sep = ";";
                                  string[] feat = adores.Split(sep.ToCharArray());
                                  int k;
                                  Dictionary<int, string> it = new Dictionary<int, string>();
                                  for (k=0; k< feat.Length; k++)
                                  {
                                      it[k+1] = feat[k].ToString();
                                  }
                                  cache[sef] = it;
                                  if (feat.Length > 0)
                                    return (feat[0]);
                                  else
                                      return "";
                                  //return get_list_element(adores, iindex);
                              }
                              else
                                  return adores;
                          }
                          catch 
                          {
                              return " ";
                          }
                      }
                      else
                      {

                          bc_om_rest_ef reef = new bc_om_rest_ef();
                          reef.ef = sef;
                          reef.user_id = bc_cs_central_settings.logged_on_user_id;
                          reef.index = iindex;

                          string json = "";
                          JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();
                          JsonSerializer.MaxJsonLength = Int32.MaxValue;
                          json = JsonSerializer.Serialize(reef);


                          json = "{\"function\":" + json + "}";
                          if (bc_am_insight_formats.excel_rtd_rest_url == "")
                              bc_am_insight_formats.excel_rtd_rest_url = bc_cs_central_settings.soap_server;
                          bc_cs_ns_json_post rp = new bc_cs_ns_json_post(bc_am_insight_formats.excel_rtd_rest_url + "Rest_excel_function_tranmission", json);
                          bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                          rp.send(certificate);


                          if (rp.err_text == "" || rp.err_text == null)
                          {
                              rp.response_text = rp.response_text.Substring(0, rp.response_text.Length - 2);
                              rp.response_text = rp.response_text.Substring(6, rp.response_text.Length - 6);
                              rp.response_text=rp.response_text.Replace("\\/","/");
                              if (iindex == 1)
                              {
                                  //bc_cs_message msg = new bc_cs_message("Blue Curve", sef, bc_cs_message.MESSAGE);
                                  string sep = ";";
                                  string[] feat = rp.response_text.Split(sep.ToCharArray());
                                  int k;
                                  Dictionary<int, string> it = new Dictionary<int, string>();
                                  for (k = 0; k < feat.Length; k++)
                                  {
                                      it[k + 1] = feat[k].ToString();
                                  }
                                  cache[sef] = it;
                                  if (feat.Length > 0)
                                      return (feat[0]);
                                  else
                                      return "";
                                
                              }
                              else
                                  return rp.response_text;

                          }
                          else
                          {
                              return "err from web service: " + rp.err_text + " URI: "  + bc_am_insight_formats.excel_rtd_rest_url;
                          }
                         
                          rp = null;
                          GC.Collect();
                      }
                  }
            }
            catch (Exception e)
            {

                return "err from rtd server: " + e.Message.ToString();
            }
            finally
            {

                bc_cs_central_settings.activity_file = logging;
            }
        }


        protected virtual string GetData(int topic_id, string topic1 = "", string topic2 = "", string topic3 = "", string topic4 = "", string topic5 = "", string topic6 = "", string topic7 = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {
            return "";
        }

       
     
    }

