using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    
    public partial class bc_dx_cp_process_config : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_process_config
    {
        bc_om_pub_type_workflow opt;
        bc_om_dx_process_config config;
        long _pt_id;
        bool _view_only = false;
        public bc_dx_cp_process_config()
        {
            InitializeComponent();
        }

        enum EUPDATE_TYPE
        {
           ADD_STAGE,
           DELETE_STAGE,
           UPDATE_APPROVERS,
           ADD_ACTION,
           DELETE_ACTION,
           MOVE_ACTION
        }

        public event EventHandler<EsaveprocessArgs> save;
        public event EventHandler<EsaveprocessArgs> copy;

        

        void set_default_process()
        {
            workflow.Nodes.Clear();
            opt.nstage.current_stage=1;
            opt.nstage.current_stage_name="Draft";
            populate_tree(false);
            barButtonItem3.Visibility = BarItemVisibility.Always;
            barButtonItem3.Caption = "Add route from " + workflow.Nodes[0].GetValue(0).ToString();
        }

        public bool load_process(bc_om_pub_type_workflow _opt)
        {
            try
            {
                opt = _opt;
                opt.id = _pt_id;
                workflow.Nodes.Clear();
                if (opt.nstage.routes != null)
                {
                    populate_tree(false);
                }
                else
                {
                    set_default_process();
                }
                return true;

            }
            
            catch (Exception e)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "load_process", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message.ToString(), ref certificate);
 
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        void workflow_BeforeCollapse(object sender, DevExpress.XtraTreeList.BeforeCollapseEventArgs e) 
        { 
            e.CanCollapse = false; 
        }


        public bool load_view(long pub_type_id, string pub_type_name, bc_om_pub_type_workflow _opt, bc_om_dx_process_config _config, bool view_only)
        {
            try
            {
                _view_only = view_only;
                _pt_id = pub_type_id;
                workflow.FocusedNodeChanged += row_changed;
                workflow.DoubleClick += workflow_double_click;
                workflow.BeforeCollapse += workflow_BeforeCollapse;
                workflow.Click += workflow_click;

                cboapprovers.EditValueChanged += approvers_changed;
                cborole.EditValueChanged += set_approvers;
                cboallowauthor.EditValueChanged += set_approvers;
                if (view_only== false)
                  Ribbon.ApplicationCaption = "Process Configuration for - " + pub_type_name;
                else
                  Ribbon.ApplicationCaption = "Process Configuration for - " + pub_type_name + " (View Only)";
                opt = _opt;
                config = _config;
                //cborole.Items.Add("na");
                cborole.Items.Add("all");
                int i;
                for (i = 0; i < config.roles.Count; i++)
                {
                    cborole.Items.Add(config.roles[i].name);
                }
                if (opt.nstage.routes != null)
                {
                    populate_tree(false);
                }
                else
                {
                    set_default_process();
                }

                barButtonItem3.Visibility = BarItemVisibility.Always;
                barButtonItem3.Caption = "Add route from " + workflow.Nodes[0].GetValue(0).ToString();
                ribbonPage1.Groups[2].Visible = false;
                if (view_only==true)
                {
                    ribbonPage1.Visible = false;
                    ribbonPage2.Visible = false;
                    ribbonPage3.Visible = false;
                    ribbonPage4.Visible = true;
                    workflow.Columns[3].OptionsColumn.ReadOnly = true;
                    workflow.Columns[4].OptionsColumn.ReadOnly = true;
                    workflow.Columns[5].OptionsColumn.ReadOnly = true;
                }
                else
                    ribbonPage4.Visible = false;
                
                return true;
            }
            catch (Exception e)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message.ToString(), ref certificate);

                return false;
            }
        }

        void approvers_changed(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ComboBoxEdit s;
            s =  (DevExpress.XtraEditors.ComboBoxEdit)sender;
           

           workflow.Columns[4].OptionsColumn.AllowEdit = false;
           workflow.Columns[5].OptionsColumn.AllowEdit = false;
           if (s.EditValue=="0")
           {
               workflow.FocusedNode.SetValue(4, "na");
               workflow.FocusedNode.SetValue(5, "na");
           }
           else if (s.OldEditValue=="0")
           {
               workflow.Columns[4].OptionsColumn.AllowEdit = true;
               workflow.Columns[5].OptionsColumn.AllowEdit = true;
               workflow.FocusedNode.SetValue(4, "allow");
               workflow.FocusedNode.SetValue(5, "all");
           }
           set_approvers(sender, e);
        }

        void set_approvers(object sender, EventArgs e)
        {
            try
            {
                bool sfound = false;
                int i,j;
                object editval;
                DevExpress.XtraEditors.ComboBoxEdit s;
                s =  (DevExpress.XtraEditors.ComboBoxEdit)sender;
                editval = s.EditValue;
                int approvers=0;
                int author_approval=0;
                long approval_role=0;
                if (s.Properties.Name == "cboapprovers")
                {
                    approvers=s.SelectedIndex;
                    if (approvers == 0)
                    {
                        author_approval = 0;
                        approval_role = 0;
                    }
                    else
                    {
                        author_approval = 0;
                        if (workflow.FocusedNode.GetValue(4).ToString() == "disallow")
                            author_approval = 1;
                    }
                    approval_role=0;
                    for (j=0; j < config.roles.Count;j++)
                    {
                        if (config.roles[j].name== workflow.FocusedNode.GetValue(5).ToString())
                        {
                          approval_role=config.roles[j].id;
                          break;
                        }
                    }
                }
                else if (s.Properties.Name == "cboallowauthor")
                {
                    int sapp;
                    Int32.TryParse(workflow.FocusedNode.GetValue(3).ToString(), out sapp);
                    approvers = sapp;
                    author_approval = 0;
                   
                    if (s.SelectedIndex == 1)
                         author_approval = 1;

                    approval_role = 0;
                    for (j = 0; j < config.roles.Count; j++)
                    {
                        if (config.roles[j].name == workflow.FocusedNode.GetValue(5).ToString())
                        {
                            approval_role = config.roles[j].id;
                            break;
                        }
                    }

                }
                else if (s.Properties.Name == "cborole")
                {
                    int sapp;
                    Int32.TryParse(workflow.FocusedNode.GetValue(3).ToString(), out sapp);
                    approvers = sapp;
                   
                    author_approval = 0;
                    if (workflow.FocusedNode.GetValue(4).ToString() == "disallow")
                        author_approval = 1;

                    if (s.SelectedIndex == 0)
                        approval_role = 0;
                    else
                        approval_role = config.roles[s.SelectedIndex - 1].id;
                }

                if (workflow.FocusedNode.ParentNode == workflow.Nodes[0])
                {
                    for (i = 0; i < opt.nstage.routes.Count; i++)
                    {
                        if (opt.nstage.routes[i].current_stage_name == workflow.FocusedNode.GetValue(11).ToString())
                        {
                            opt.nstage.routes[i].approvers = approvers;
                            opt.nstage.routes[i].author_approval = author_approval;
                            opt.nstage.routes[i].approval_role = approval_role;
                            
                            break;
                        }
                    }

                }
                else
                {
                     find_stage(workflow.FocusedNode.GetValue(11).ToString() , workflow.FocusedNode.GetValue(10).ToString() , ref opt.nstage.routes, ref sfound, EUPDATE_TYPE.UPDATE_APPROVERS, approvers, author_approval, approval_role, 0, false);
                }
            

                DevExpress.XtraTreeList.Nodes.TreeListNode current_node;
                current_node = workflow.FocusedNode;
                populate_tree(barCheckItem1.Checked);
                workflow.FocusedNode = current_node;

            }

            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "et_approvers", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }


        }

        void row_changed(object sender, EventArgs e)
        {
            erow_changed();
        }

        void erow_changed()
        {
            try
            {
                if (workflow.FocusedNode == null)
                    return;
                barButtonItem3.Visibility = BarItemVisibility.Never;
                barButtonItem4.Visibility = BarItemVisibility.Never;
                barButtonItem5.Visibility = BarItemVisibility.Never;
                barButtonItem6.Visibility = BarItemVisibility.Never;
                barButtonItem7.Visibility = BarItemVisibility.Never;
                barButtonItem8.Visibility = BarItemVisibility.Never;
                //barButtonItem3.Caption="Add";
                //barButtonItem4.Caption="Remove";

                workflow.Columns[3].OptionsColumn.AllowEdit = false;
                workflow.Columns[4].OptionsColumn.AllowEdit = false;
                workflow.Columns[5].OptionsColumn.AllowEdit = false;

                if (workflow.FocusedNode.GetValue(0) != null)
                {
                    ribbonPage1.Groups[1].Visible = true;
                    ribbonPage1.Groups[2].Visible = false;
                    barButtonItem3.Visibility = BarItemVisibility.Always;
                    barButtonItem3.Caption = "Add route from " + workflow.FocusedNode.GetValue(0).ToString();
                    try
                    {
                        barButtonItem4.Caption = "Remove " + workflow.FocusedNode.ParentNode.GetValue(0).ToString() + " to " + workflow.FocusedNode.GetValue(0).ToString();
                        barButtonItem4.Visibility = BarItemVisibility.Always;
                    }
                    catch { }
                }
                else if (workflow.FocusedNode.GetValue(1) != null)
                {
                    ribbonPage1.Groups[1].Visible = false;
                    ribbonPage1.Groups[2].Visible = true;
                    barButtonItem5.Visibility = BarItemVisibility.Always;
                    barButtonItem5.Caption = "Add action " + workflow.FocusedNode.GetValue(1).ToString();
                    ribbonPage1.Groups[2].Text = "Action";
                    workflow.Columns[3].OptionsColumn.AllowEdit = true;
                    if (workflow.FocusedNode.GetValue(3).ToString() !="0")
                    {
                        workflow.Columns[4].OptionsColumn.AllowEdit = true;
                        workflow.Columns[5].OptionsColumn.AllowEdit = true;
                    }
                }
                else if (workflow.FocusedNode.GetValue(2) != null)
                {
                    ribbonPage1.Groups[1].Visible = false;
                    ribbonPage1.Groups[2].Visible = true;
                    ribbonPage1.Groups[2].Text = "Action";
                    barButtonItem6.Visibility = BarItemVisibility.Always;
                    barButtonItem6.Caption = "Remove " + workflow.FocusedNode.GetValue(2).ToString().Trim();
                    if (workflow.FocusedNode.PrevNode!=null)
                        barButtonItem7.Visibility = BarItemVisibility.Always;
                    if (workflow.FocusedNode.NextNode != null)
                        barButtonItem8.Visibility = BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "row_changed", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }
        void populate_tree(bool stages_only)
        {
            try
            {
                workflow.BeginUpdate();
                workflow.Nodes.Clear();
                if (opt.nstage.current_stage_name == "")
                {
                    opt.nstage.current_stage_name = "Draft";
                    opt.nstage.current_stage = 0;
                }
                workflow.Nodes.Add();
                workflow.Nodes[0].SetValue(0,opt.nstage.current_stage_name);
                workflow.Nodes[0].SetValue(6, opt.nstage.current_stage);
                workflow.Nodes[0].Tag = 0;
                add_node(workflow.Nodes[0], ref opt.nstage.routes, 0, stages_only);
            }
            catch (Exception ex)
            {
              
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "populate_tree", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
            finally
            {
                set_nav();
                workflow.ExpandAll();
                workflow.EndUpdate();
                if (workflow.Nodes.Count > 0)
                {
                    barButtonItem3.Visibility = BarItemVisibility.Always;
                    barButtonItem3.Caption = "Add from " + workflow.Nodes[0].GetValue(0).ToString();
                }
            }
          
        }
        void add_node(DevExpress.XtraTreeList.Nodes.TreeListNode start_node, ref List<bc_om_pub_type_workflow_stage> lstage, int tag, bool stages_only)
    {
        int i,j,k;
        int ncount=0;
       try
       {
           if (lstage == null)
               return;
           for (i = 0; i < lstage.Count; i++)
           {
               string ax = "";
               string sfrom = "";
               sfrom = start_node.GetValue(0).ToString();

               if (sfrom.IndexOf("[") > 0)
               {
                   sfrom = sfrom.Substring(0, sfrom.IndexOf("[") - 1);
               }
               if (stages_only == false)
               {
                   start_node.Nodes.Add();
                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(1, sfrom + " to " + lstage[i].current_stage_name);

                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(10, sfrom);
                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(11, lstage[i].current_stage_name); 

                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(3, "0");
                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(4, "na");
                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(5, "na");


                   if (lstage[i].approvers > 0)
                   {
                       start_node.Nodes[start_node.Nodes.Count - 1].SetValue(3, lstage[i].approvers.ToString());
                       if (lstage[i].author_approval == 1)
                           start_node.Nodes[start_node.Nodes.Count - 1].SetValue(4, "disallow");
                       else
                           start_node.Nodes[start_node.Nodes.Count - 1].SetValue(4, "allow");
                       if (lstage[i].approval_role > 0)
                       {
                           for (j = 0; j < config.roles.Count; j++)
                           {
                               if (lstage[i].approval_role == config.roles[j].id)
                               {
                                   start_node.Nodes[start_node.Nodes.Count - 1].SetValue(5, config.roles[j].name);
                                   break;
                               }
                           }

                       }
                       else
                           start_node.Nodes[start_node.Nodes.Count - 1].SetValue(5, "all");
                   }


                   ncount = ncount + 1;
                   bc_om_workflow_action action;
                   for (j = 0; j < lstage[i].actions.Count; j++)
                   {
                       for (k = 0; k < config.actions.Count; k++)
                       {
                           if (config.actions[k].id == (long)lstage[i].actions[j])
                           {
                               start_node.Nodes[start_node.Nodes.Count - 1].Nodes.Add();
                               start_node.Nodes[start_node.Nodes.Count - 1].Nodes[start_node.Nodes[start_node.Nodes.Count - 1].Nodes.Count - 1].SetValue(2, "  " + config.actions[k].name);
                               start_node.Nodes[start_node.Nodes.Count - 1].Nodes[start_node.Nodes[start_node.Nodes.Count - 1].Nodes.Count - 1].SetValue(7, config.actions[k].id);
                               break;
                           }
                       }
                   }

                   if (ax != "")
                   {
                       ax = "         [" + ax + "]";
                   }

               }
              
               start_node.Nodes.Add();
               start_node.Nodes[start_node.Nodes.Count - 1].SetValue(0, lstage[i].current_stage_name);
               start_node.Nodes[start_node.Nodes.Count - 1].SetValue(6, lstage[i].current_stage);
               tag = tag + 1;
               start_node.Nodes[start_node.Nodes.Count - 1].Tag = tag;

               if (start_node.Nodes !=null && lstage[i].routes != null)
               {
                   add_node(start_node.Nodes[ncount], ref lstage[i].routes, tag, stages_only);
               }
               ncount = ncount + 1;
              
               }
          }
          catch(Exception e)
          {
              bc_cs_security.certificate certificate = new bc_cs_security.certificate();
              bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "add_node", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message.ToString(), ref certificate);
          }
    }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            



            Close();
        }
        //ad route
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
            bool stage_found = false;
            if (workflow.Nodes[0].GetValue(0).ToString() == workflow.FocusedNode.GetValue(0).ToString())
                workflow.FocusedNode = workflow.Nodes[0];
            else
                navigate_to_stage(workflow.Nodes[0], workflow.FocusedNode.GetValue(0).ToString(), ref   stage_found);
          
            bc_dx_cp_edit fed = new bc_dx_cp_edit();
            Cbc_dx_cp_edit ced = new Cbc_dx_cp_edit();
            List<bc_om_entity> vals = new  List<bc_om_entity> ();
            bc_om_entity val;
           
            int i,j;
            List<string> existing_routes = new List<string>();
            for (i=0; i < workflow.FocusedNode.Nodes.Count; i ++)
            {
                if (workflow.FocusedNode.Nodes[i].GetValue(0) != null)
                {
                    existing_routes.Add(workflow.FocusedNode.Nodes[i].GetValue(0).ToString());
                }
            }
            bool found;
            for (i=0; i < config.stages.Count; i++)
            {
                found = false;
                if (config.stages[i].name == workflow.FocusedNode.GetValue(0).ToString())
                {
                    found = true;
            
                }
                for (j = 0; j < existing_routes.Count;j++ )
                {
                    if (existing_routes[j] == config.stages[i].name)
                    {
                        found = true;
                        break;
                    }
                  
                }


                if (found == false)
                {
                    vals.Add(config.stages[i]);
                }
            }

            if (ced.load_data(fed, "Add route from " + workflow.FocusedNode.GetValue(0).ToString() , "", "Choose Route or add a new Route", true, vals, true) == false)
                return;
            else
                fed.ShowDialog();

            if (ced.bsave == false)
                return;

            //check new route doesnt already exist
            if (ced.id == 0)
            {
                for (i = 0; i < config.stages.Count; i++)
                {
                    if (config.stages[i].name.ToLower() == ced.text.ToLower())
                    {
                        bc_cs_message msg = new bc_cs_message("Blue Curve", "New Stage Name " + ced.text + " already exists!", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        return;
                    }
                }

            }
            bool sfound = false;
            if (opt.nstage.current_stage_name == workflow.FocusedNode.GetValue(0).ToString())
            {
                bc_om_pub_type_workflow_stage nws = new bc_om_pub_type_workflow_stage();
                nws.current_stage = (int)ced.id;
                nws.current_stage_name = ced.text;
                if (opt.nstage.routes == null)
                    opt.nstage.routes = new List<bc_om_pub_type_workflow_stage>();
                opt.nstage.routes.Add(nws);
            }
            else
                find_stage(ced.text, workflow.FocusedNode.GetValue(0).ToString(), ref opt.nstage.routes, ref sfound, EUPDATE_TYPE.ADD_STAGE, 0, 0, 0, 0, false);
             
            populate_tree(barCheckItem1.Checked);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "add_stage", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }

        private void find_stage(string new_route_name, string current_stage_name, ref List<bc_om_pub_type_workflow_stage> st, ref bool found, EUPDATE_TYPE mode, int approvers, int author_approval, long approval_role, long action_id, bool move_up)
        {
            try
            {
            int i,j,k;
            if (found == true)
                return;
            if (st == null)
                return;
            for (i=0;i < st.Count; i ++)
            {
                if (found == true)
                    return;
                if (st[i].current_stage_name == current_stage_name)
                {
                    switch (mode)
                    {
                        case EUPDATE_TYPE.ADD_STAGE:
                          bc_om_pub_type_workflow_stage nws = new bc_om_pub_type_workflow_stage();
                          nws.current_stage_name = new_route_name;
                          if (st[i].routes == null)
                            st[i].routes = new List<bc_om_pub_type_workflow_stage>();
                            st[i].routes.Add(nws);
                            break;

                        case EUPDATE_TYPE.DELETE_STAGE:
                            if (st[i].routes != null)
                            {
                                for (j = 0; j < st[i].routes.Count; j++)
                                {
                                    if (st[i].routes[j].current_stage_name == new_route_name)
                                    {
                                        st[i].routes.RemoveAt(j);
                                        break;
                                    }

                                }
                            }
                            break;
                        case EUPDATE_TYPE.UPDATE_APPROVERS:
                        {
                            if (st[i].routes != null)
                            {
                                for (j = 0; j < st[i].routes.Count; j++)
                                {
                                    if (st[i].routes[j].current_stage_name == new_route_name)
                                    {
                                        st[i].routes[j].approvers = approvers;
                                        st[i].routes[j].author_approval = author_approval;
                                        st[i].routes[j].approval_role = approval_role;
                                        break;
                                    }

                                }
                            }
                            break;
                        
                        }
                        case EUPDATE_TYPE.ADD_ACTION:
                        {
                            if (st[i].routes != null)
                            {
                                for (j = 0; j < st[i].routes.Count; j++)
                                {
                                    if (st[i].routes[j].current_stage_name == new_route_name)
                                    {
                                        st[i].routes[j].actions.Add(action_id);
                                       
                                        break;
                                    }

                                }
                            }
                            break;

                        }

                        case EUPDATE_TYPE.DELETE_ACTION:
                        {
                            if (st[i].routes != null)
                            {
                                for (j = 0; j < st[i].routes.Count; j++)
                                {
                                    if (st[i].routes[j].current_stage_name == new_route_name)
                                    {
                                        
                                        for (k = 0; k < st[i].routes[j].actions.Count; k++)
                                        {
                                            if ((long)st[i].routes[j].actions[k] == action_id)
                                            {
                                                st[i].routes[j].actions.RemoveAt(k);
                                                break;
                                            }
                                        }
                                        break;
                                    }

                                }
                            }
                            break;
                        }

                        case EUPDATE_TYPE.MOVE_ACTION:
                        {
                            if (st[i].routes != null)
                            {
                                for (j = 0; j < st[i].routes.Count; j++)
                                {
                                    if (st[i].routes[j].current_stage_name == new_route_name)
                                    {

                                        for (k = 0; k < st[i].routes[j].actions.Count; k++)
                                        {
                                            if ((long)st[i].routes[j].actions[k] == action_id)
                                            {
                                                st[i].routes[j].actions.RemoveAt(k);
                                                if (move_up == true)
                                                    st[i].routes[j].actions.Insert(k - 1, action_id);
                                                else
                                                    st[i].routes[j].actions.Insert(k + 1, action_id);
                                                break;

                                            }
                                        }
                                        break;
                                    }

                                }
                            }
                            break;
                        }

                    }
                    
                    found = true;
                    return;
                }
                if (found==false)
                    find_stage(new_route_name, current_stage_name, ref st[i].routes, ref found, mode, approvers, author_approval, approval_role, action_id, move_up);
            }
             }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "find stage", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }
        // remove stage
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bool sfound = false;
                int i;
                if (workflow.FocusedNode.ParentNode== workflow.Nodes[0])
                {
                    for (i=0; i < opt.nstage.routes.Count; i++)
                    {
                        if (opt.nstage.routes[i].current_stage_name==workflow.FocusedNode.GetValue(0).ToString())
                        {
                            opt.nstage.routes.RemoveAt(i);
                            break;
                        }
                    }
                    
                }
                else
                    find_stage(workflow.FocusedNode.GetValue(0).ToString(), workflow.FocusedNode.ParentNode.GetValue(0).ToString(), ref opt.nstage.routes, ref sfound, EUPDATE_TYPE.DELETE_STAGE, 0, 0, 0, 0, false);
                  populate_tree(barCheckItem1.Checked);
            }
           
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "remove stage", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }
        // add action
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
              

                bc_dx_cp_edit fed = new bc_dx_cp_edit();
                Cbc_dx_cp_edit ced = new Cbc_dx_cp_edit();
                List<bc_om_entity> vals = new List<bc_om_entity>();
                bc_om_entity val;

                int i, j;
                List<string> existing_actions = new List<string>();
                for (i = 0; i < workflow.FocusedNode.Nodes.Count; i++)
                {
                    if (workflow.FocusedNode.Nodes[i].GetValue(2) != null)
                    {
                        existing_actions.Add(workflow.FocusedNode.Nodes[i].GetValue(2).ToString().Trim());
                    }
                }
                bool found;
                for (i = 0; i < config.actions.Count; i++)
                {
                    found = false;
                
                    for (j = 0; j < existing_actions.Count; j++)
                    {
                        if (existing_actions[j] == config.actions[i].name)
                        {
                            found = true;
                            break;
                        }

                    }


                    if (found == false)
                    {
                        vals.Add(config.actions[i]);
                    }
                }
                if (ced.load_data(fed, "Add Action " + workflow.FocusedNode.GetValue(10).ToString() + " to "  + workflow.FocusedNode.GetValue(11).ToString(), "", "Choose Action", true, vals) == false)
                    return;
                else
                    fed.ShowDialog();

                if (ced.bsave == false)
                    return;
             
               
                bool sfound = false;
                if (workflow.FocusedNode.ParentNode == workflow.Nodes[0])
                {
                    for (i = 0; i < opt.nstage.routes.Count; i++)
                    {
                        if (opt.nstage.routes[i].current_stage_name == workflow.FocusedNode.GetValue(11).ToString())
                        {
                            opt.nstage.routes[i].actions.Add(ced.id);
                            break;
                        }
                    }

                }
                
                else
                  find_stage(workflow.FocusedNode.GetValue(11).ToString(), workflow.FocusedNode.GetValue(10).ToString(), ref opt.nstage.routes, ref sfound, EUPDATE_TYPE.ADD_ACTION, 0, 0, 0,  ced.id, false);
                reload_tree();
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "add action", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }
        void reload_tree()
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode current_node;
            current_node = workflow.FocusedNode;
            populate_tree(barCheckItem1.Checked);
            workflow.FocusedNode = current_node;

        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int i, j;
                long action_id;
                action_id = (long)workflow.FocusedNode.GetValue(7);
               

                bool sfound = false;
                if (workflow.FocusedNode.ParentNode.ParentNode == workflow.Nodes[0])
                {
                    for (i = 0; i < opt.nstage.routes.Count; i++)
                    {
                        if (opt.nstage.routes[i].current_stage_name == workflow.FocusedNode.ParentNode.GetValue(11).ToString())
                        {
                            for (j=0; j < opt.nstage.routes[i].actions.Count; j ++ )
                            {
                                if ((long)opt.nstage.routes[i].actions[j] == action_id)
                                {
                                    opt.nstage.routes[i].actions.RemoveAt(j);
                                    break;
                                }
                            }
                            break;
                        }
                    }

                }

                else
                    find_stage(workflow.FocusedNode.ParentNode.GetValue(11).ToString(), workflow.FocusedNode.ParentNode.GetValue(10).ToString(), ref opt.nstage.routes, ref sfound, EUPDATE_TYPE.DELETE_ACTION, 0, 0, 0, action_id, false);
 



                reload_tree();
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "remove action", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            move_action(true);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            move_action(false);
        }
        void move_action(bool up)
        {
             try
            {
                int i, j;
                long action_id;
                action_id = (long)workflow.FocusedNode.GetValue(7);
               

                bool sfound = false;
                if (workflow.FocusedNode.ParentNode.ParentNode == workflow.Nodes[0])
                {
                    for (i = 0; i < opt.nstage.routes.Count; i++)
                    {
                        if (opt.nstage.routes[i].current_stage_name == workflow.FocusedNode.ParentNode.GetValue(11).ToString())
                        {
                            for (j=0; j < opt.nstage.routes[i].actions.Count; j ++ )
                            {
                                if ((long)opt.nstage.routes[i].actions[j] == action_id)
                                {
                                    opt.nstage.routes[i].actions.RemoveAt(j);
                                    if (up == true)
                                      opt.nstage.routes[i].actions.Insert(j - 1, action_id);
                                    else
                                      opt.nstage.routes[i].actions.Insert(j + 1, action_id);
                                    break;
                                }
                            }
                            break;
                        }
                    }

                }

                else
                    find_stage(workflow.FocusedNode.ParentNode.GetValue(11).ToString(), workflow.FocusedNode.ParentNode.GetValue(10).ToString(), ref opt.nstage.routes, ref sfound, EUPDATE_TYPE.MOVE_ACTION, 0, 0, 0, action_id,up);

                reload_tree();
               
                if (up == true)
                    workflow.FocusedNode = workflow.FocusedNode.PrevNode;
                else
                    workflow.FocusedNode = workflow.FocusedNode.NextNode;
            }
             catch (Exception ex)
             {
                 bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                 bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "move action", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
             }
        }
        
        private void next_stage(DevExpress.XtraTreeList.Nodes.TreeListNode node, ref bc_om_pub_type_workflow_stage ws)
        {
            try
            {
         
            bc_om_pub_type_workflow_stage nws;
            int i,j;
            List<long> actions= new List<long>();
            ws.routes = new List<bc_om_pub_type_workflow_stage>();
            for (i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].GetValue(1) != null)
                {
                    if (node.Nodes[i].Nodes.Count > 0)
                    {
                        for (j = 0; j < node.Nodes[i].Nodes.Count; j++)
                        {
                            if (node.Nodes[i].Nodes[j].GetValue(7) != null)
                            {
                                actions.Add((long)node.Nodes[i].Nodes[j].GetValue(7));
                            }
                        }
                    }
                }
              
                if (node.Nodes[i].GetValue(0) != null)
                {
                    nws = new bc_om_pub_type_workflow_stage();
                    nws.current_stage_name = node.Nodes[i].GetValue(0).ToString(); ;
                    ws.routes.Add(nws);
                    for (j = 0; j < actions.Count; j++ )
                          ws.routes[ws.routes.Count - 1].actions.Add(actions[j].ToString());
                    actions.Clear();
                    next_stage(node.Nodes[i], ref nws);
                }
            }
        }
          catch(Exception e)
          {
               bc_cs_security.certificate certificate = new bc_cs_security.certificate();
              bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "next stage", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message.ToString(), ref certificate);
   
          }
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

            try
            {
                Cursor = Cursors.WaitCursor;

               
                EventHandler<EsaveprocessArgs> handler = save;
                if (handler != null)
                {
                    EsaveprocessArgs args = new EsaveprocessArgs();
                    args.workflow = opt;
                    handler(this, args);
                }
                Cursor = Cursors.Default;
                Close();
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "save", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }

        private void clear_selection(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            int i;
            for (i=0; i < node.Nodes.Count; i ++)
            {
                node.Nodes[i].StateImageIndex = -1;
                //if (node.Nodes[i].GetValue(0) !=null)
                //    node.Nodes[i].StateImageIndex = -1;
                //else if (node.Nodes[i].GetValue(1) != null)
                //    node.Nodes[i].StateImageIndex = 6;
                //else if (node.Nodes[i].GetValue(2) !=null)
                //    node.Nodes[i].StateImageIndex = 7;
                clear_selection(node.Nodes[i]);
            }
        }

        private void workflow_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            set_nav();
        }
        void set_nav()
        {
           
            try
            {
                if (workflow.Nodes.Count == 0)
                    return;
                workflow.Nodes[0].StateImageIndex = -1;
                clear_selection(workflow.Nodes[0]);
                if (workflow.FocusedNode.GetValue(0) == null)
                    return;

                if (workflow.FocusedNode.Nodes.Count == 0 && workflow.Nodes[0].Nodes.Count > 0)
                {
                      workflow.FocusedNode.StateImageIndex = 2;
                }
                else
                    workflow.FocusedNode.StateImageIndex = 1;

                if (workflow.FocusedNode.ParentNode!=null)
                  workflow.FocusedNode.ParentNode.StateImageIndex = 3;



                int i;
                for (i = 0; i < workflow.FocusedNode.Nodes.Count; i++)
                {
                    if (workflow.FocusedNode.Nodes[i].GetValue(0) != null)
                        workflow.FocusedNode.Nodes[i].StateImageIndex = 0;
                }
            }
            catch (Exception e)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "set nav", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message.ToString(), ref certificate);
   
            }
        }
        void navigate_to_stage(DevExpress.XtraTreeList.Nodes.TreeListNode node, string stage_name, ref bool stage_found, bool do_not_set = false )
        {

            try

            { 
            int i;
            


            for (i = 0; i < node.Nodes.Count; i++)
            {
                if (stage_found == true)
                    return;

                if (node.Nodes[i].GetValue(0) != null)
                {
                  
                    if (node.Nodes[i].GetValue(0).ToString() == stage_name)
                    {
                        //if (workflow.FocusedNode != node.Nodes[i])
                        //{
                            stage_found = true;
                            //if (do_not_set == false)
                            if (workflow.FocusedNode == node.Nodes[i])
                                workflow.FocusedNode.StateImageIndex = 4;
                            else

                                workflow.FocusedNode = node.Nodes[i];
                            return;
                        //}
                    }
                    else
                        navigate_to_stage(node.Nodes[i], stage_name, ref stage_found, do_not_set);
                }
               
            }
            }
            catch (Exception e)
            {
                 bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "navigate_to_stage", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message.ToString(), ref certificate);
   
            }

        }

        void workflow_click(object sender, EventArgs e)
        {
            if (_view_only == true)
                return;
            MouseEventArgs f;
            f = (MouseEventArgs)e;
            if (f.Button == MouseButtons.Right)
            {
                //if (workflow.FocusedNode.GetValue(2) != null && workflow.FocusedColumn.AbsoluteIndex==2)
                //{
                    System.Drawing.Point dp = new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y);
                    popupmenuaction.ShowPopup(dp);
                //}
            }
        }
       


        void workflow_double_click(object sender, EventArgs e)
        {
            
            try
            {
                if (workflow.Nodes[0].GetValue(0) == null || workflow.FocusedNode.GetValue(0) == null)
                    return;
            if (workflow.Nodes[0].GetValue(0).ToString() == workflow.FocusedNode.GetValue(0).ToString())
            {
                workflow.FocusedNode = workflow.Nodes[0];
                return;
            }


            if (workflow.FocusedNode.StateImageIndex==2)
            {
                bool stage_found = false;
                navigate_to_stage(workflow.Nodes[0], workflow.FocusedNode.GetValue(0).ToString(), ref   stage_found);
            }
            }
            catch (Exception ex)
            {
                 bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "workflow double click", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                populate_tree(barCheckItem1.Checked);
            }
            catch (Exception ex)
            {
                 bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "barCheckItem1", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }

        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
            bc_dx_cp_edit fed = new bc_dx_cp_edit();
            Cbc_dx_cp_edit ced = new Cbc_dx_cp_edit();
            List<bc_om_entity> vals = new  List<bc_om_entity> ();
            bc_om_entity val;
            bc_om_pub_type pt;
            int i;
            for (i=0; i < config.pub_types.pubtype.Count-1; i++)
            {
                val = new bc_om_entity();

                pt = (bc_om_pub_type)config.pub_types.pubtype[i];

                val.id = pt.id;
                val.name = pt.name;
                vals.Add(val);
            }
            if (ced.load_data(fed, "Copy Process From Publication", "", "Choose Publication", true, vals) == false)
                return;
            else
                fed.ShowDialog();

            if (ced.bsave == false)
                return;

            bc_cs_message omsg = new bc_cs_message("Blue Curve: ", "Existing process will be removed. Continue with the copy from publication: " + ced.text, bc_cs_message.MESSAGE, true, false, "Yes", "No", true);
            if (omsg.cancel_selected == true)
                return;

            Cursor = Cursors.WaitCursor;
            EventHandler<EsaveprocessArgs> handler = copy;
            if (handler != null)
            {
                EsaveprocessArgs args = new EsaveprocessArgs();
                args.workflow = new bc_om_pub_type_workflow();
                args.workflow.id = ced.id;
                handler(this, args);
            }
             }
            catch (Exception ex)
            {
                 bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "barButtonItem9_ItemClick", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
   
            }

        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            opt.nstage.routes.Clear();
            set_default_process();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            opt.nstage.routes.Clear();
            set_default_process();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
    }
    public class Cbc_dx_cp_process_config
    {
        Ibc_dx_cp_process_config _view;
        public bool load_data(Ibc_dx_cp_process_config view, long pub_type_id, string pub_type_name, bool view_only)
        {

            try
            {
            _view = view;
            _view.save += save;
            _view.copy += copy;



            bc_om_pub_type_workflow opt = new bc_om_pub_type_workflow();
            if (load_process(pub_type_id, ref opt) == false)
              return false;
            
            bc_om_dx_process_config config = new  bc_om_dx_process_config();
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
              config.db_read();
            else
            {
              config.tmode = bc_cs_soap_base_class.tREAD;
              object oconfig;
              oconfig = (object)config;

              if (config.transmit_to_server_and_receive(ref oconfig, true) == false)
                return false;
              config = (bc_om_dx_process_config)oconfig;
           }

            return view.load_view(pub_type_id, pub_type_name, opt, config, view_only);

             }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_process_config", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
               return false;
            }
        }
        void copy(object sender, EsaveprocessArgs args)
        {
            try
            {
            bc_om_pub_type_workflow opt = new bc_om_pub_type_workflow();
            if  (load_process(args.workflow.id, ref opt) == true)
                _view.load_process(opt);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_process_config", "copy", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
               
            }
        }
        bool load_process(long pub_type_id, ref  bc_om_pub_type_workflow opt)
        {
            try
            {
            opt = new bc_om_pub_type_workflow();
            opt.bwithactions_and_approvers = true;
            opt.id = pub_type_id;
            opt.bwithactions_and_approvers = true;
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                opt.db_read();
            else
            {
                opt.tmode = bc_cs_soap_base_class.tREAD;
                object oopt;
                oopt = (object)opt;

                if (opt.transmit_to_server_and_receive(ref oopt, true) == false)
                    return false;
                opt = (bc_om_pub_type_workflow)oopt;
            }
            return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_process_config", "load_process", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }
        void save(object sender, EsaveprocessArgs args)
        {
            try
            {
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    args.workflow.db_write();
                else
                {
                    args.workflow.tmode = bc_cs_soap_base_class.tWRITE;
                    object oworkflow;
                    oworkflow = (object)args.workflow;
                    args.workflow.transmit_to_server_and_receive(ref oworkflow, true);
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_process_config", "save", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }
    }
    public interface Ibc_dx_cp_process_config
    {
        bool load_view(long pub_type_id, string pub_type_name, bc_om_pub_type_workflow opt, bc_om_dx_process_config config, bool view_only);
        bool load_process(bc_om_pub_type_workflow opt);
        event EventHandler<EsaveprocessArgs> save;
        event EventHandler<EsaveprocessArgs> copy;
    }
    public class EsaveprocessArgs : EventArgs
    {
        public bc_om_pub_type_workflow workflow { get; set; }
    }
}