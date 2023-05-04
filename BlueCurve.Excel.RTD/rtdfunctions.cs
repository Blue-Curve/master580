
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Office.Interop.Excel;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using System.Threading;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;

//VBA snippts for ref
//Dim kk As Object
//Set kk = CreateObject("bluecurvertd.bc_rtd_from_vba")
//kk.set_refresh_now

namespace bluecurvertd
{
    // refresh all from vba button
    public class bc_rtd_from_vba : rtdbase
    {
        public void set_refresh_now()
        {

            bc_get_class_list.brefreshnow = true;
            bc_get_entities_for_class.brefreshnow = true;
            bc_get_financial_data.brefreshnow = true;
            bc_get_financial_data_advanced.brefreshnow = true;
            bc_get_historic_financial_data.brefreshnow = true;
            bc_get_financial_data_changes.brefreshnow = true;
 
            bc_get_parent_associations.brefreshnow = true;
            bc_get_child_associations.brefreshnow = true;

            bc_get_entity_id.brefreshnow = true;
            bc_get_entity_name.brefreshnow = true;
            bc_get_entities_for_item_value.brefreshnow = true;
            bc_currency_convert.brefreshnow = true;
            bc_get_entity_user_associations.brefreshnow = true;
            bc_get_user_entity_associations.brefreshnow = true;
            bc_get_year_and_period.brefreshnow = true;
            bc_get_linked_financial_data.brefreshnow = true;
            bc_get_financial_items.brefreshnow = true;
            bc_get_item_formula.brefreshnow = true;
            bc_get_scale_symbol.brefreshnow = true;
            bc_get_flexible_name.brefreshnow = true;

            bc_get_aggregated_data.brefreshnow = true;
            bc_get_aggregated_statistics.brefreshnow = true;
        }
    }

    
    // linear functions
    public class bc_get_class_list : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string dimensions = "value", string index = "1", string topic3 = "", string topic4 = "", string topic5 = "", string topic6 = "", string topic7 = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {
            if (dimensions == "")
                dimensions = "class_name";
            if (index == "")
                index = "0";
            sef = "<excel_function><type>5000</type><dimensions>" + dimensions + "</dimensions></excel_function>";
            return post(topicId, sef, index);
        }

    }
    public class bc_get_entities_for_class : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string lclass, string dimensions = "value", string index = "1", string topic4 = "", string topic5 = "", string topic6 = "", string topic7 = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {
            if (dimensions == "")
                dimensions = "name";
            if (index == "")
                index = "0";
            sef = "<excel_function><type>5001</type><class_id>" + lclass + "</class_id><dimensions>" + dimensions + "</dimensions></excel_function>";
            return post(topicId, sef, index);
        }
    }

    public class bc_get_financial_data : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string lclass, string entity, string item, string year, string period, string stage = "1", string contributor = "1", string ldate = "9-9-9999", string dimensions = "value", string unused1 = "", string unused2 = "", string unused3 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {

            if (period == "")
                period = "1";
            if (stage == "")
                stage = "1";
            if (contributor == "")
                contributor = "1";
            if (ldate == "")
                ldate = "9-9-9999";
            if (dimensions == "")
                dimensions = "value";

            sef = "<excel_function><type>5002</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><date_at>" + ldate + "</date_at><year>" + year + "</year><period>" + period + "</period><dimensions>" + dimensions + "</dimensions><attributename></attributename></excel_function>";
            return post(topicId, sef);
        }

    }

    public class bc_get_financial_data_advanced : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string lclass, string entity, string item, string year, string period, string data_type ="Fiscal",  string month ="",string stage = "1", string contributor = "1", string curr = "", string ldate = "9-9-9999", string dimensions = "value", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {

            if (period == "")
                period = "1";
            if (stage == "")
                stage = "1";
            if (contributor == "")
                contributor = "1";
            if (ldate == "")
                ldate = "9-9-9999";
            if (dimensions == "")
                dimensions = "value";
            if (data_type=="")
                data_type="Fiscal";

            sef = "<excel_function><type>5002</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><date_at>" + ldate + "</date_at><year>" + year + "</year><period>" + period + "</period><data_type>" + data_type + "</data_type><month>" + month + "</month><curr>" + curr + "</curr><dimensions>" + dimensions + "</dimensions><attributename></attributename></excel_function>";
            return post(topicId, sef);
        }

    }



    public class bc_get_historic_financial_data : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string lclass, string entity, string item, string year, string period, string stage = "1", string contributor = "1", string date_from = "1-1-1900", string date_to = "9-9-9999", string dimensions = "value", string index = "1", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {

            if (period == "")
                period = "1";
            if (stage == "")
                stage = "1";
            if (contributor == "")
                contributor = "1";
            if (date_from == "")
                date_from = "1-1-1900";
            if (date_to == "")
                date_to = "9-9-9999";
            if (dimensions == "")
                dimensions = "value";
            if (index == "")
                index = "0";
            sef = "<excel_function><type>5003</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><date_from>" + date_from + "</date_from><date_to>" + date_to + "</date_to><year>" + year + "</year><period>" + period + "</period><dimensions>" + dimensions + "</dimensions><attributename></attributename></excel_function>";
            return post(topicId, sef, index);
        }

    }
   
    
     public class bc_get_financial_data_changes : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string lclass, string entity, string item, string year, string period, string stage = "1", string contributor = "1", string date_from = "1-1-1900", string date_to = "9-9-9999", string dimensions = "value", string index = "1", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {

            if (period == "")
                period = "1";
            if (stage == "")
                stage = "1";
            if (contributor == "")
                contributor = "1";
            if (date_from == "")
                date_from = "1-1-1900";
            if (date_to == "")
                date_to = "9-9-9999";
            if (dimensions == "")
                dimensions = "value";
            sef = "<excel_function><type>5004</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><date_from>" + date_from + "</date_from><date_to>" + date_to + "</date_to><year>" + year + "</year><period>" + period + "</period><dimensions>" + dimensions + "</dimensions><attributename></attributename></excel_function>";
            return post(topicId, sef, index);
        }
    }


    public class bc_get_parent_associations : rtdbase
    {
        public static bool brefreshnow = false;

        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string lclass, string entity, string child_class, string schema_id = "1", string priority = "", string dimensions = "name", string index = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {
            if (schema_id=="")
                schema_id="1";
            if (priority=="")
                priority="all";
            if (dimensions=="")
                dimensions="name";
            if (index == "")
                index = "0";
            sef = "<excel_function><type>5005</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><ass_class_id>" + child_class + "</ass_class_id><schema_id>" + schema_id + "</schema_id><priority>" + priority + "</priority><dimensions>" + dimensions + "</dimensions></excel_function>";
            return post(topicId, sef, index);
        }
    }
  
     public class bc_get_child_associations : rtdbase
    {
         public  static bool brefreshnow = false;

         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
                 brefreshnow = false;
                 if (brefreshnow == true)
                 {
                     calcall();
                 }
         }



         protected override string GetData(int topicId, string lclass, string entity, string child_class, string schema_id = "1", string priority = "", string dimensions = "name", string index = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
        {
            if (schema_id=="")
                schema_id="1";
            if (priority=="")
                priority="all";
            if (dimensions=="")
                dimensions="name";
            if (index == "")
                index = "0";
            sef = "<excel_function><type>5006</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><ass_class_id>" + child_class + "</ass_class_id><schema_id>" + schema_id + "</schema_id><priority>" + priority + "</priority><dimensions>" + dimensions + "</dimensions></excel_function>";
            return post(topicId, sef, index);
        }
    }


     public class bc_get_entity_id : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string entity, string lclass, string dimensions = "name", string topic4 = "", string topic5 = "", string topic6 = "", string topic7 = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
             if (dimensions == "")
                 dimensions = "entity_id";
             sef = "<excel_function><type>5007</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><dimensions>" + dimensions + "</dimensions></excel_function>";
             return post(topicId, sef);
         }
     }
     public class bc_get_entity_name : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string entity, string dimensions = "name", string topic3 = "", string topic4 = "", string topic5 = "", string topic6 = "", string topic7 = "", string topic8 = "", string topic9 = "", string topic10 = "", string topic11 = "", string topic12 = "", string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
             if (dimensions == "")
                 dimensions = "name";

             sef = "<excel_function><type>5008</type><entity_id>" + entity + "</entity_id><dimensions>" + dimensions + "</dimensions></excel_function>";
             return post(topicId, sef);
         }
     }

     public class bc_get_entities_for_item_value : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string item, string value, string start_range, string end_range, string year, string period, string stage, string contributor, string date_at, string dimensions, string index, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
               if (period == "")
                 period = "1";
             if (stage == "")
                 stage = "1";
             if (contributor == "")
                 contributor = "1";
             if (date_at == "")
                 date_at = "9-9-9999";
             if (dimensions == "")
                 dimensions = "value";
             if (index == "")
                 index = "0";

             if (value == ">" || value == "<" || value == "between")
                 value = "";
             else if (value == "=")
             {
                 value = start_range;
                 start_range = "";
             }

             sef = "<excel_function><type>5009</type><class_id>" +lclass + "</class_id><item_id>" + item + "</item_id><value>" + value + "</value><start_range>" + start_range + "</start_range><end_range>" + end_range + "</end_range><year>" + year + "</year><period>" + period + "</period><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><date_at>" + date_at + "</date_at><dimensions>" + dimensions + "</dimensions></excel_function>";
             return post(topicId,sef,index);
         }
     }

     public class bc_currency_convert : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity, string item, string year, string period, string convert_value, string to_currency, string date_at, string dimensions, string stage, string contributor,  string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
              
             if (date_at == "")
                 date_at = "9-9-9999";
             if (dimensions == "")
                 dimensions = "value";
             
             sef = "<excel_function><type>5010</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><item_id>" + item + "</item_id><year>" + year + "</year><period>" + period + "</period><value>" + convert_value + "</value><currency>" + to_currency + "</currency><date_at>" + date_at + "</date_at><dimensions>" + dimensions + "</dimensions></excel_function>";
             
             return post(topicId,sef);
         }
     }

      public class bc_get_entity_user_associations : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity, string rating, string dimensions, string  index, string  topic6, string  topic7, string topic8, string topic9, string topic10, string topic11,  string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
              
             if (rating == "")
                 rating = "all";
             if (dimensions == "")
                 dimensions = "value";
             
             sef = "<excel_function><type>5012</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><value>" + rating + "</value><dimensions>" + dimensions + "</dimensions></excel_function>";
 
             return post(topicId,sef,index);
         }
     }

    public class bc_get_user_entity_associations : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string user, string dimensions, string index, string  topic5, string  topic6, string  topic7, string topic8, string topic9, string topic10, string topic11,  string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
              
             if (dimensions == "")
                 dimensions = "name";
             
             sef = "<excel_function><type>5013</type><class_id>" + lclass + "</class_id><user_id>" + user + "</user_id><dimensions>" + dimensions + "</dimensions></excel_function>";
             return post(topicId,sef,index);
         }
     }

    public class bc_get_year_and_period : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity_id, string period_end_item, string retrieve_type, string stage, string contributor, string dimensions, string attributename, string topic9, string topic10, string topic11, string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
               if (stage == "")
                 stage = "1";
               if (contributor == "")
                 contributor = "1";
               if (retrieve_type == "")
                 dimensions="year";

               sef = "<excel_function><type>5011</type><class_id>" + lclass + "</class_id><entity_id>" + entity_id + "</entity_id><item_id>" + period_end_item + "</item_id><value>" + retrieve_type + "</value><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><dimensions>" + dimensions + "</dimensions><attributename>" + attributename + "</attributename></excel_function>";
         
             
             return post(topicId,sef);
         }
     }


    public class bc_get_linked_financial_data : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity_id, string linked_class, string linked_entity, string item, string stage, string contributor, string date_at, string dimensions, string topic10, string topic11, string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
               if (stage == "")
                 stage = "1";
               if (contributor == "")
                 contributor = "1";
               if (date_at == "")
                 date_at="9-9-9999";

               sef = "<excel_function><type>5014</type><class_id>" + lclass + "</class_id><entity_id>" + entity_id + "</entity_id><lclass_id>" + linked_class + "</lclass_id><lentity_id>" + linked_entity + "</lentity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><date_at>" + date_at + "</date_at><dimensions>" + dimensions + "</dimensions></excel_function>";
             
             return post(topicId,sef);
         }
     }

  
    

    // aggregations



     // calculation library
    
 

    public class bc_get_financial_items : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity_id, string context, string section, string dimensions, string index, string topic7, string topic8, string topic9, string topic10, string topic11, string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
             if (context == "")
                 context = "1";
             if (dimensions == "")
                 dimensions = "name";
             if (section == "")
                 section = "All";
             if (index == "")
                 index = "0";
             string sef;
             sef = "<excel_function><type>10000</type><class_id>" + lclass + "</class_id><entity_id>" + entity_id + "</entity_id><context>" + context + "</context><section>" + section + "</section><dimensions>" + dimensions + "</dimensions></excel_function>";
             
             return post(topicId,sef,index);
         }
     }
    public class bc_get_item_formula : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity_id, string item_id, string attributename, string topic5, string topic6, string topic7, string topic8, string topic9, string topic10, string topic11, string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
             sef = "<excel_function><type>10001</type><class_id>" + lclass + "</class_id><entity_id>" + entity_id + "</entity_id><item_id>" + item_id + "</item_id><attributename>" + attributename + "</attributename></excel_function>";
             
             return post(topicId,sef);
         }
     }
    public class bc_get_scale_symbol : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 calcall();
                 brefreshnow = false;
             }
         }
         protected override string GetData(int topicId, string item, string factor, string topic3, string topic4, string topic5, string topic6, string topic7, string topic8, string topic9, string topic10, string topic11, string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
             sef = "<excel_function><type>10003</type><item_id>" + item + "</item_id><factor>" + factor + "</factor></excel_function>";
             return post(topicId,sef);
         }
     }

    public class bc_get_flexible_name : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string lclass, string entity, string  item, string contributor, string topic5, string topic6, string topic7, string topic8, string topic9, string topic10, string topic11, string topic12, string topic13 = "", string topic14 = "", string topic15 = "", string topic16 = "")
         {
             sef = "<excel_function><type>10002</type><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><item_id>" + item + "</item_id><contributor_id>" + contributor + "</contributor_id></excel_function>";
             return post(topicId,sef);
         }
     }


  
    // aggragtions
    public class bc_get_aggregated_data : rtdbase
     {
         public static bool brefreshnow = false;
         protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
         {
             if (brefreshnow == true)
             {
                 brefreshnow = false;
                 calcall();
             }
         }
         protected override string GetData(int topicId, string universe, string lclass, string entity, string dual_class, string dual_entity, string item, string year, string period, string stage, string contributor, string exch, string date_at, string dimensions,  string topic14 = "", string topic15 = "", string topic16 = "")
         {
             sef = "<excel_function><type>5040</type><universe>" + universe + "</universe><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><dclass_id>" + dual_class + "</dclass_id><dentity_id>" + dual_entity + "</dentity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><exch>" + exch + "</exch><date_at>" + date_at + "</date_at><year>" + year + "</year><period>" + period + "</period><dimensions>" + dimensions + "</dimensions></excel_function>";
             return post(topicId,sef);
         }
     }
    public class bc_get_aggregated_statistics : rtdbase
    {
        public static bool brefreshnow = false;
        protected override void refreshall(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (brefreshnow == true)
            {
                brefreshnow = false;
                calcall();
            }
        }
        protected override string GetData(int topicId, string universe, string lclass, string entity, string dual_class, string dual_entity, string item, string year, string period, string stage, string contributor, string exch, string date_at, string dimensions,  string topic14 = "", string topic15 = "", string topic16 = "")
        {
            sef = "<excel_function><type>5041</type><universe>" + universe + "</universe><class_id>" + lclass + "</class_id><entity_id>" + entity + "</entity_id><dclass_id>" + dual_class + "</dclass_id><dentity_id>" + dual_entity + "</dentity_id><item_id>" + item + "</item_id><stage_id>" + stage + "</stage_id><contributor_id>" + contributor + "</contributor_id><exch>" + exch + "</exch><date_at>" + date_at + "</date_at><year>" + year + "</year><period>" + period + "</period><dimensions>" + dimensions + "</dimensions></excel_function>";
            return post(topicId, sef);
        }
    }


}

