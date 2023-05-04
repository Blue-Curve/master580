using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
   

    public partial class bc_dx_entity_search : UserControl
    {

        //public enum ESEARCH_MODE
        //{
        //    ENTITIES=1,
        //    USERS=0,
        //    BUS_AREA=-1,
        //    ROLES=-2,
        //    PUB_TYPES=-3
        //};
       

        public bc_dx_entity_search()
        {
            InitializeComponent();
        }

        bc_om_entity_classes _classes = new bc_om_entity_classes();
        List<bc_om_entity> _entities;
        List<bc_om_filter_attribute_type> _filter_attributes;
        long gclass_id;

        public event EventHandler<EloadentityArgs> Eloadentity;
        public event EventHandler<EloadentityArgs> Eentitydblclick;
        public event EventHandler<EloadclassArgs> Eclasschanged;
        public event EventHandler<EventArgs> Einactiveactivechanged;
        public event EventHandler<EventArgs> Enoselection;

        public long single_class_id = 0;
        public Boolean hide_class = false;
        public Boolean hide_filter = false;
        public Boolean hide_active_inactive = false;
        public EFIXEDENTITYCLASSES class_mode = EFIXEDENTITYCLASSES.ENTITY;

        public List<bc_om_entity> sel_entities = new List<bc_om_entity>();
        public bc_om_entity_class selected_class;
        Boolean loading = false;


        Boolean no_search = false;
        string naventity ="";
        public void navigate( string class_name ,  string entity_name)
        {
           rallmine.SelectedIndex = 0; 
           naventity = entity_name;
           uxclass.Text = class_name;
           
        }
        

        public Boolean load_view( bc_om_entity_classes classes)
        {

            try
            {
                loading = true;
                timer1.Stop();
                timer1.Tick += new EventHandler(timer1_Tick);
                pictureEdit2.Click += new EventHandler(pictureEdit2_Click);
                
                tsearch.TextChanged += new EventHandler (tsearch_TextChanged);
                rallmine.SelectedIndexChanged += new EventHandler(rallmine_SelectedIndexChanged);
                uxfilter.SelectedIndexChanged += new EventHandler(uxfilter_SelectedIndexChanged);
                uxfilteroptions.SelectedIndexChanged += new EventHandler(uxfilteroptions_SelectedIndexChanged);
                pictureEdit2.Click += new EventHandler(pictureEdit2_Click);
                lentities.SelectedIndexChanged += new EventHandler (lentities_SelectedIndexChanged);
                lentities.DoubleClick += new EventHandler ( lentities_dblclick);
             
               
              
                _classes = classes;
                bc_om_entity_class lclass;
                int i;


                for (i = 0; i < _classes.classes.Count; i++)
                {
                    lclass = (bc_om_entity_class)_classes.classes[i];
                    if (class_mode != EFIXEDENTITYCLASSES.ENTITY)
                    {
                        uxclass.Enabled  = false;
                        uxclass.Properties.Items.Add(lclass.class_name);
                    }
                    else
                    {
                     
                        if (single_class_id == 0)
                        {

                            if (lclass.inactive == false)
                                uxclass.Properties.Items.Add(lclass.class_name);
                        }
                        else if (lclass.class_id == single_class_id)
                        {
                            uxclass.Properties.Items.Add(lclass.class_name);
                            uxclass.Enabled = false;
                        }
                    }
               
                    if (uxclass.Properties.Items.Count > 0)
                    {
                        uxclass.SelectedIndex = 0;
                    }
                }
                if (hide_class == true)
                    uxclass.Visible = false;
                if (hide_active_inactive == true)
                    rallmine.Visible = false;

                if (hide_filter == true)
                    pfilter.Visible = false;
                    //uxfilter.Visible = false;
                System.Drawing.Point tdp = new System.Drawing.Point();
                tdp.Y = uxclass.Location.Y;

                if (hide_class == true)
                {
                    if (hide_filter==false)
                    {
                        tdp.X= pfilter.Location.X;
                        pfilter.Location=tdp;
                        if (hide_active_inactive==false)
                        {
                            tdp.Y =  pfilter.Location.Y + pfilter.Height + 10;
                            tdp.X= rallmine.Location.X;
                            rallmine.Location= tdp;

                            tdp.Y =  rallmine.Location.Y + rallmine.Height + 10;
                            tdp.X= lentities.Location.X;
                            lentities.Location=tdp;
                            lentities.Height =  lentities.Height- uxclass.Height;
                        }
                        else
                        {
                            tdp.Y = pfilter.Location.Y + pfilter.Height + 10;
                            tdp.X = lentities.Location.X;
                            lentities.Location = tdp;
                            lentities.Height = lentities.Height - uxclass.Height - rallmine.Height;
                        }
                    }
                    else
                    {
                        if (hide_active_inactive == false)
                        {
                            tdp.Y = uxclass.Location.Y;
                            tdp.X = rallmine.Location.X;
                            rallmine.Location = tdp;

                            tdp.Y = rallmine.Location.Y + rallmine.Height + 10;
                            tdp.X = lentities.Location.X;
                            lentities.Location = tdp;
                            lentities.Height = lentities.Height + uxclass.Height + pfilter.Height;
                        }
                        else
                        {
                            tdp.Y = uxclass.Location.Y;
                            tdp.X = lentities.Location.X;
                            lentities.Location = tdp;
                            lentities.Height = lentities.Height + pfilter.Height + uxclass.Height + rallmine.Height;

                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
            finally
            {

                loading = false;
                if (lentities.Items.Count > 0)
                {
                    lentities.SelectedIndex = -1;
                    lentities.SelectedIndex = 0;
                }
            }

        }
        public void load_entities(List<bc_om_entity> entities, List<bc_om_filter_attribute_type> filters)
        {
            try
            { 
            _entities = entities;
            _filter_attributes = filters;

         
            int i;
            pfilter.Enabled = false; 
            //uxfilter.Enabled = false;
            uxfilter.Properties.Items.Clear();
            uxfilter.Text = "";

            if (_filter_attributes != null)
            {

                for (i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                    {
                        pfilter.Enabled = true;
                        //uxfilter.Enabled = true;
                        uxfilter.Properties.Items.Add("No Filter");
                    }
                    uxfilter.Properties.Items.Add(filters[i].attribute_name);
                }
                if (filters.Count > 0)
                    uxfilter.SelectedIndex = 0;
            } 
            load_entity_list(false, null,false,null);

            }


            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_entities", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
          
        }
        public void load_entity_list(Boolean search_mode, List<long> found_search_entities, Boolean filter_mode, List<long> found_filter_entities, bool clear_search = false)
        {
            if (clear_search== true && tsearch.Text !="")
            {
                tsearch.Text = "";
                return;
            }

            int lid = -1;
            try
            {
             
                rallmine.Visible = true;
              
                lentities.BeginUpdate();
                lentities.Items.Clear();
                int activestate = 0;
                if (rallmine.SelectedIndex == 1)
                    activestate = 1;
            

                List<bc_om_entity> showentities = new List<bc_om_entity>();
                showentities = _entities;


                if (activestate == 0)
                {


                    var queryp = from en in showentities
                                 where en.inactive == false
                                 select new bc_om_entity
                                 {
                                     id = en.id,
                                     name = en.name,
                                     inactive = false
                                 };

                    showentities = queryp.ToList();
                }
                else if (activestate == 1)
                {
                    var queryp = from en in showentities
                                 where en.inactive == true
                                 select new bc_om_entity
                                 {
                                     id = en.id,
                                     name = en.name,
                                     inactive = true
                                 };

                    showentities = queryp.ToList();

                }

                if (search_mode == true)
                {
                    
                    var queryp = from en in showentities
                                 join a in found_search_entities on en.id equals a
                                 select new bc_om_entity
                                 {
                                     id = en.id,
                                     name = en.name,
                                     inactive = en.inactive
                                 };

                    showentities = queryp.ToList();
                   
                }

                if (filter_mode == true)
                {

                    var queryp = from en in showentities
                                 join a in found_filter_entities on en.id equals a
                                 select new bc_om_entity
                                 {
                                     id = en.id,
                                     name = en.name,
                                     inactive = en.inactive
                                 };

                    showentities = queryp.ToList();
                }
              
                // elimate selected entities
               if (sel_entities.Count > 0)
               {
                   var queryp = from en in showentities
                                join a in sel_entities on en.id equals a.id into ts
                                from a in ts.DefaultIfEmpty()
                                select new bc_om_entity
                                {
                                    id = a == null?en.id:0,
                                    name = en.name,
                                    inactive = en.inactive
                                };
                  showentities = queryp.ToList();
               }
               

               int i;
             
              
               for (i = 0; i < showentities.Count; i++)
               {
                   
                   if (showentities[i].id != 0)
                     lentities.Items.Add(showentities[i].name);
                   if (naventity == showentities[i].name)
                       lid = i;
               }
              
              

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_entity_List", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                no_search = false;
                lentities.EndUpdate();
                if (lid > -1)
                  lentities.SelectedIndex = lid;
                naventity = "";
                Cursor.Current = Cursors.Default;
            }
        }
      
        public void select_entity(string name)
        {
            int i;
            for (i = 0; i < lentities.Items.Count; i++)
            {
                if (lentities.Items[i].ToString() == name)
                {
                    lentities.SelectedIndex = i;
                    break;
                }

            }
            
        }
        private void uxclass_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            load_class();
        }
        public void load_class()
        {
           
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                no_search = true;
                tsearch.Text = "";

                if (uxclass.SelectedIndex == -1)
                    return;
                int i;
                bc_om_entities_of_a_class entities = new bc_om_entities_of_a_class();
                switch (class_mode)
                {
                    case (EFIXEDENTITYCLASSES.USER) :
                    case (EFIXEDENTITYCLASSES.ASSOC_USER):
                            gclass_id = 0;
                        
                            bc_om_users users = new bc_om_users();
                            users.inactive = true;
                            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                            {
                                users.db_read();
                            }
                            else
                            {

                                users.tmode = bc_cs_soap_base_class.tREAD;
                                object ousers = (object)users;
                                if (users.transmit_to_server_and_receive(ref ousers, true) == false)
                                    return;
                                users = (bc_om_users)ousers;
                            }

                            bc_om_entity uentity;
                            bc_om_user uuser;
                            for (i = 0; i < users.user.Count; i++)
                            {
                                uentity = new bc_om_entity();
                                uentity.class_id = 0;
                                uuser = (bc_om_user)users.user[i];
                                uentity.id = uuser.id;
                                uentity.name = uuser.first_name + " " + uuser.surname;
                                uentity.inactive = uuser.inactive;
                                entities.entities.Add(uentity);
                            }
                            // get filter attributes
                        
                           bc_om_non_entity_filter_attributes nfa = new bc_om_non_entity_filter_attributes();
                         
                           if (class_mode==EFIXEDENTITYCLASSES.USER)
                           {
                             nfa.class_id=0;
                             if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                             {
                                nfa.db_read();
                             }
                             else
                             {
                               nfa.tmode = bc_cs_soap_base_class.tREAD;
                               object onfa = (object)nfa;
                               if (nfa.transmit_to_server_and_receive(ref onfa, true) == false)
                                   return;
                               nfa = (bc_om_non_entity_filter_attributes)onfa;
                             }
                           }
                        
                           load_entities(entities.entities, nfa.filter_attributes_types);
                         break;

                    case EFIXEDENTITYCLASSES.BUS_AREA:
                    case EFIXEDENTITYCLASSES.ROLE:
                          gclass_id = (long)class_mode;
                          entities.class_id = gclass_id;
                         
                          if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                          {
                              entities.db_read();
                          }
                          else
                          {

                              entities.tmode = bc_cs_soap_base_class.tREAD;
                              object oentities = (object)entities;
                              if (entities.transmit_to_server_and_receive(ref oentities, true) == false)
                                  return;
                              entities = (bc_om_entities_of_a_class)oentities;
                          }
                          load_entities(entities.entities, null);
                         
                        break;

                    //case EFIXEDENTITYCLASSES.ROLE:

                    //    break;

                    case EFIXEDENTITYCLASSES.PUB_TYPE:
                        gclass_id = -6;
                        bc_om_pub_types pub_types = new bc_om_pub_types();
                            pub_types.get_inactive = true;
                            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                            {
                                pub_types.db_read();
                            }
                            else
                            {

                                pub_types.tmode = bc_cs_soap_base_class.tREAD;
                                object opub_types = (object)pub_types;
                                if (pub_types.transmit_to_server_and_receive(ref opub_types, true) == false)
                                    return;
                                pub_types = (bc_om_pub_types)opub_types;
                            }

                            //bc_om_entity uentity;
                            bc_om_pub_type upub_type;
                            for (i = 0; i < pub_types.pubtype.Count; i++)
                            {
                                uentity = new bc_om_entity();
                                uentity.class_id = 5;
                                upub_type = (bc_om_pub_type)pub_types.pubtype[i];
                                uentity.id = upub_type.id;
                                uentity.name = upub_type.name;
                                uentity.inactive = upub_type.inactive;
                                entities.entities.Add(uentity);
                            }
                            
                             nfa = new bc_om_non_entity_filter_attributes();
                             nfa.class_id=-6;
                             if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                             {
                                nfa.db_read();
                             }
                             else
                             {
                               nfa.tmode = bc_cs_soap_base_class.tREAD;
                               object onfa = (object)nfa;
                               if (nfa.transmit_to_server_and_receive(ref onfa, true) == false)
                                   return;
                               nfa = (bc_om_non_entity_filter_attributes)onfa;
                             }
                             load_entities(entities.entities, nfa.filter_attributes_types);
                         break;

                        

                    case EFIXEDENTITYCLASSES.ENTITY:
                       
                      bc_om_entity_class lclass;
                      for (i = 0; i < _classes.classes.Count; i++)
                      {
                        lclass = (bc_om_entity_class)_classes.classes[i];
                        selected_class = lclass;
                        if (lclass.class_name == uxclass.Text)
                        {
                          EventHandler<EloadclassArgs> handler = Eclasschanged;
                          if (handler != null)
                          {
                            EloadclassArgs args = new EloadclassArgs();
                            args.sclass = lclass;
                            handler(this, args);
                          }

                          gclass_id = lclass.class_id;
                          entities.class_id = gclass_id;
                          entities.inactive = false;
                          if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                          {
                              entities.db_read();
                          }
                          else
                          {

                              entities.tmode = bc_cs_soap_base_class.tREAD;
                              object oentities = (object)entities;
                              if (entities.transmit_to_server_and_receive(ref oentities, true) == false)
                                  return;
                              entities = (bc_om_entities_of_a_class)oentities;
                          }
                          load_entities(entities.entities, entities.filter_attributes_types);
                          return;
                         }
                       }
                        break;
                    case EFIXEDENTITYCLASSES.CLASSIFY:
                    case EFIXEDENTITYCLASSES.CHANNEL:
                    case EFIXEDENTITYCLASSES.eMODULE:
                        bc_om_dx_pub_type_link_items li = new bc_om_dx_pub_type_link_items();
                        li.link_type = class_mode;
                        if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                        {
                            li.db_read();
                        }
                        else
                        {
                            li.tmode = bc_cs_soap_base_class.tREAD;
                            object oli = (object)li;
                            if (li.transmit_to_server_and_receive(ref oli, true) == false)
                                return;
                            li = (bc_om_dx_pub_type_link_items)oli;
                        }

                        load_entities(li.items, null);
                        break;

                    default:
                        MessageBox.Show("class mode: " + class_mode.ToString() + " not implemened");
                        break;
                    }                    
                }
            


            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_entities", "load_class", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                no_search = false;
            }
        }

        public bool check_name_exists(string name)
        {
            var queryp = from en in _entities
                         where en.name.ToLower() == name.ToLower()
                         select new bc_om_entity
                         {
                             id = en.id,
                             name = en.name,
                             inactive = en.inactive
                         };
            List<bc_om_entity> sel_entity = queryp.ToList();
            if (sel_entity.Count > 0)
                 return true;
                else
                return false;
            

        }

        private void lentities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading == true)
                return;
            try
            {
                if (lentities.SelectedIndex == -1)
                {
                    EventHandler<EventArgs> handler = Enoselection;
                    if (handler != null)
                    {
                        EventArgs args = new EventArgs();
                        handler(this, args);
                    }
                    return;
                }

            Cursor.Current = Cursors.WaitCursor;
            var queryp = from en in _entities
                         where en.name == lentities.Text
                         select new bc_om_entity
                         {
                             id = en.id,
                             name=en.name,
                             inactive=en.inactive
                         };
            List<bc_om_entity> sel_entity = queryp.ToList();

            if (sel_entity.Count == 1)
            {
             
                EventHandler<EloadentityArgs> handler = Eloadentity;
                if (handler != null)
                {
                    EloadentityArgs args = new EloadentityArgs();
                    args.sentity = sel_entity[0];
                    handler(this, args);
                }
              }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "lentities_SelectedIndexChangedy", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void lentities_dblclick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var queryp = from en in _entities
                             where en.name == lentities.Text
                             select new bc_om_entity
                             {
                                 id = en.id,
                                 name = en.name,
                                 inactive = en.inactive
                             };
                List<bc_om_entity> sel_entity = queryp.ToList();

                if (sel_entity.Count == 1)
                {

                    EventHandler<EloadentityArgs> handler = Eentitydblclick;
                    if (handler != null)
                    {
                        EloadentityArgs args = new EloadentityArgs();
                        args.sentity = sel_entity[0];
                        handler(this, args);
                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "lentities_SelectedIndexChangedy", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void pictureEdit2_Click(object sender, EventArgs e) 
        {
            tsearch.Text = "";
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
           
            if (no_search == true)
                return;
           
            if (uxfilter.SelectedIndex > 0)
                uxfilter.SelectedIndex = 0;
         
            timer1.Stop();
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (tsearch.Text.Trim()=="")
            {
                load_entity_list(false, null, false, null);
                return;
            }
           
            Cursor.Current = Cursors.WaitCursor;
            List<long> found_entities = new List<long>();
            if (class_mode == EFIXEDENTITYCLASSES.ENTITY)
            {

                // server search to include extended search
                bc_om_real_time_search search_results = new bc_om_real_time_search();
                search_results.class_id = gclass_id;
                search_results.search_text = tsearch.Text;
                search_results.mine = false;
                search_results.inactive = false;
                search_results.filter_attribute_id = 0;
                search_results.results_as_ids = true;
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    search_results.db_read();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    object osearch_results = (object)search_results;
                    search_results.tmode = bc_cs_soap_base_class.tREAD;
                    if (search_results.transmit_to_server_and_receive(ref osearch_results, true) == false)
                        return;
                    search_results = (bc_om_real_time_search)osearch_results;
                }
                int i;
                for (i = 0; i < search_results.resultsids.Count; i++)
                    found_entities.Add(search_results.resultsids[i]);
            }
            else
            {
                //client search
              
                List<bc_om_entity> resnt ;
                var queryp = from en in _entities
                                where en.name.ToUpper().Contains(tsearch.Text.ToUpper())
                                select new bc_om_entity
                                {
                                    id=en.id
                                };
                resnt = queryp.ToList();
               
                int i;
                for (i = 0; i < resnt.Count; i++)
                    found_entities.Add(resnt[i].id);


            }
            load_entity_list(true, found_entities, false, null);
            
            Cursor.Current = Cursors.Default;
        }

        private void rallmine_SelectedIndexChanged(object sender, EventArgs e)
        {

               EventHandler<EventArgs> handler = Einactiveactivechanged;
               if (handler != null)
               {
                   EventArgs args = new EventArgs();
                   handler(this, args);
               }


            if (rallmine.SelectedIndex == 0 && uxfilter.Properties.Items.Count>0)
            {
                pfilter.Enabled = true;
                //uxfilter.Enabled =true;
            }
            else 
            {
                pfilter.Enabled = false;
                uxfilteroptions.SelectedIndex = -1;
                uxfilter.SelectedIndex = -1;
            }
          


          
            if (uxfilter.SelectedIndex > -1)
            {
              
                if (rallmine.SelectedIndex == 1)
                    //uxfilter.Enabled = false;
                    pfilter.Enabled = false;
             

            }

            if (tsearch.Text != "")
            {
                no_search = true;
                tsearch.Text = "";
            }
           
           load_entity_list(false, null, false, null);

           no_search = false;
        }

        private void uxfilteroptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (uxfilter.SelectedIndex == -1 || uxfilteroptions.SelectedIndex == -1)
                    return;
                else if (uxfilter.SelectedIndex == 0)
                {
                    no_search = true;
                    tsearch.Text = "";
                    load_entity_list(false, null, false, null);
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    no_search = true;
                    tsearch.Text = "";

                    bc_om_dx_filter_attribute_values fa = new bc_om_dx_filter_attribute_values();
                    fa.class_id = gclass_id;
                    int i;
                    for (i = 0; i < _filter_attributes.Count; i++)
                    {
                        if (_filter_attributes[i].attribute_name == uxfilter.Text)
                        {
                            fa.attribute_id = _filter_attributes[i].attribute_id;
                            fa.attribute_value = _filter_attributes[i].filter_lookup[uxfilteroptions.SelectedIndex].id;
                            break;
                        }
                    }

                    if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                        fa.db_read();
                    else
                    {
                        fa.tmode = bc_cs_soap_base_class.tREAD;
                        object ofa;
                        ofa = (object)fa;

                        if (fa.transmit_to_server_and_receive(ref ofa, true) == false)
                            return;
                        fa = (bc_om_dx_filter_attribute_values)ofa;

                        load_entity_list(false, null, true, fa.results);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_entity_search", "uxfilteroptions_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
       
            }
            finally 
            { 
                Cursor.Current = Cursors.Default; 
            }
        }
   

        private void uxfilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            uxfilteroptions.Enabled = false;
            uxfilteroptions.SelectedIndex = -1;
            if (uxfilter.SelectedIndex == -1)
                return;
            else if (uxfilter.SelectedIndex == 0)
            {
                no_search = true;
                tsearch.Text = "";
                load_entity_list(false, null, false, null);
            }
            else
            {
                uxfilteroptions.Properties.Items.Clear();
                int i,j;
                for (i = 0; i < _filter_attributes.Count; i++)
                {
                    if (_filter_attributes[i].attribute_name == uxfilter.Text)
                    {
                     for (j=0; j < _filter_attributes[i].filter_lookup.Count; j ++)
                     {
                         uxfilteroptions.Properties.Items.Add(_filter_attributes[i].filter_lookup[j].name);
                     }
                   }
                }
                uxfilteroptions.Enabled = true;
             }

        }
   
      
        public Boolean show_class;
        public Boolean show_filter;
        public Boolean show_active_inactive;
        public long class_id;

        public Boolean load_data()
        {
            try
            {

                bc_om_entity_class oclass;
                bc_om_entity_classes classes = new bc_om_entity_classes();
                switch (class_mode)
                {
                    case EFIXEDENTITYCLASSES.USER:
                        oclass = new bc_om_entity_class();
                        oclass.class_id=0;
                        oclass.class_name="Users";
                        classes.classes.Add(oclass);
                        break;
                         case EFIXEDENTITYCLASSES.BUS_AREA:
                        oclass = new bc_om_entity_class();
                        oclass.class_id=-1;
                        oclass.class_name="Bus Area";
                        classes.classes.Add(oclass);
                        break;
                         case EFIXEDENTITYCLASSES.ROLE :
                        oclass = new bc_om_entity_class();
                        oclass.class_id=-2;
                        oclass.class_name="Role";
                        classes.classes.Add(oclass);
                        break;
                         case EFIXEDENTITYCLASSES.LANGUAGE:
                        oclass = new bc_om_entity_class();
                        oclass.class_id=-3;
                        oclass.class_name="Langauge";
                        classes.classes.Add(oclass);;
                        break;
                    case EFIXEDENTITYCLASSES.OFFICE:
                        oclass = new bc_om_entity_class();
                        oclass.class_id=-4;
                        oclass.class_name="Office";
                        classes.classes.Add(oclass);
                        break;

                    case EFIXEDENTITYCLASSES.PUB_TYPE:
                        oclass = new bc_om_entity_class();
                        oclass.class_id=-5;
                        oclass.class_name="Pub Type";
                        classes.classes.Add(oclass);
                        break;
                    case EFIXEDENTITYCLASSES.CLASSIFY:
                        oclass = new bc_om_entity_class();
                        oclass.class_id = -6;
                        oclass.class_name = "Classify";
                        classes.classes.Add(oclass);
                        break;
                    case EFIXEDENTITYCLASSES.CHANNEL:
                        oclass = new bc_om_entity_class();
                        oclass.class_id = -7;
                        oclass.class_name = "Channel";
                        classes.classes.Add(oclass);
                        break;
                    case EFIXEDENTITYCLASSES.eMODULE:
                        oclass = new bc_om_entity_class();
                        oclass.class_id =-8;
                        oclass.class_name = "Module";
                        classes.classes.Add(oclass);
                        break;

                    default:
                        classes.class_only = true;
                        if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                          classes.db_read();
                        else
                        {
                          classes.tmode = bc_cs_soap_base_class.tREAD;
                          object oclasses;
                          oclasses = (object)classes;

                          if (classes.transmit_to_server_and_receive(ref oclasses, true) == false)
                            return false;
                          classes = (bc_om_entity_classes)oclasses;
                        }
                        break;
                }


                //if (user_mode == true)
                //{
                //  bc_om_entity_class ouserclass = new bc_om_entity_class();
                //    ouserclass.class_id=0;
                //    ouserclass.class_name="Users";
                //    classes.classes.Add(ouserclass);
                //}

                //else
                //{
                //    classes.class_only = true;
                //    if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                //        classes.db_read();
                //    else
                //    {
                //        classes.tmode = bc_cs_soap_base_class.tREAD;
                //        object oclasses;
                //        oclasses = (object)classes;

                //        if (classes.transmit_to_server_and_receive(ref oclasses, true) == false)
                //            return false;
                //        classes = (bc_om_entity_classes)oclasses;
                //    }
                //}
                return load_view(classes);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_entity_search", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }
        private void lentities_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void rallmine_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void uxfilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void uxfilteroptions_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

      

       

       
    }
    
    public class EloadentityArgs : EventArgs
    {
        public bc_om_entity sentity { get; set; }
    }
  

    public class EloadclassArgs : EventArgs
    {
        public  bc_om_entity_class sclass { get; set; }
    }
   
}
