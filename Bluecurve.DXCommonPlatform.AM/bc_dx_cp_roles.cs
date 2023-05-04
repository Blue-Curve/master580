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
    public partial class bc_dx_cp_roles : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_roles
    {
        public long grole_id;
        public event EventHandler<EloadentityArgs>  Eloadrole;
        public event EventHandler<EstageaccessArgs> Esetstageroleaccess;
        public event EventHandler<EsetccessArgs> Esetappaccess;
        public event EventHandler<EnewupdateroleArgs> Enewupdaterole;

        
        bool _view_only;
        public bc_dx_cp_roles()
        {
            InitializeComponent();
        }
      
        public bool load_view(bool view_only)
        {
            try
            {
                ribbonPage2.Visible = false;
                ribbonPage3.Visible = false;
                _view_only = view_only;
              
                if (view_only == true)
                {
                    Ribbon.ApplicationDocumentCaption = Ribbon.ApplicationDocumentCaption + " - View Only";
                    toggle_view_only(true);
                    barButtonItem1.Enabled = false;
                    barButtonItem2.Enabled = false;
                    barButtonItem3.Enabled = false;
                    barButtonItem4.Enabled = false;
                    barButtonItem5.Enabled = false;
                }
                else
                {
                    //cview.SelectedIndexChanged += Esetview;
                    repositoryItemImageComboBox3.SelectedIndexChanged += Esetview;
                    bc_dx_entity_search1.Einactiveactivechanged += Enoselection;
                    bc_dx_entity_search1.Enoselection += Enoselection;
                    cpaccess.SelectedIndexChanged += Ecpaccesschange;
                    customaccess.SelectedIndexChanged += Ecustomaccesschange;
                }
                bc_dx_entity_search1.Eloadentity += Eload_Role;

               


                bc_dx_entity_search1.single_class_id = 0;
                bc_dx_entity_search1.hide_class = true;
                bc_dx_entity_search1.hide_filter = true;
                bc_dx_entity_search1.class_mode = EFIXEDENTITYCLASSES.ROLE;
                bc_dx_entity_search1.load_data();

               

                
              
                return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "Eload_user", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }

        void toggle_view_only(bool mode)
        {
            if (_view_only == true && mode == false)
                return;
            if (mode == true)
            {
                repositoryItemImageComboBox3.ReadOnly = true;
                cpaccess.ReadOnly = true;
                customaccess.ReadOnly = true;
            }
            else
            {
                repositoryItemImageComboBox3.ReadOnly = false;
                cpaccess.ReadOnly = false;
                customaccess.ReadOnly = false;
            }

        }
        public void reload_roles(string sel_role_name)
        {
            bc_dx_entity_search1.load_class();
            bc_dx_entity_search1.select_entity(sel_role_name);
        }
        private void Enoselection(object sender, EventArgs e)
        {
            ribbonPage2.Visible = false;
            ribbonPage3.Visible = false;
            uxcpapps.ClearNodes();
            uxcustomapps.ClearNodes();
            int i;
            for (i = 0; i < uxstagerole.Nodes.Count; i++)
            {
                uxstagerole.Nodes[i].SetValue(1, -1);
                uxstagerole.Nodes[i].SetValue(2, -1);
                uxstagerole.Nodes[i].SetValue(3, -1);
                uxstagerole.Nodes[i].SetValue(4, -1);
                uxstagerole.Nodes[i].SetValue(5, -1);
                uxstagerole.Nodes[i].SetValue(6, -1);
                uxstagerole.Nodes[i].SetValue(7, -1);
                uxstagerole.Nodes[i].SetValue(8, -1);
                uxstagerole.Nodes[i].SetValue(9, -1);
            }
            uxstagerole.Enabled = false;
            xtraTabControl1.Enabled = false;
        }
        public void load_stage_role_and_access(bc_om_dx_stage_role_access sr, bc_om_dx_cp_apps_for_role sa, bc_om_stages stages)
        {
           try
           {
             

               uxstagerole.BeginUpdate();
               uxcpapps.BeginUpdate();
               uxcustomapps.BeginUpdate();

               uxstagerole.ClearNodes();
               int i, j;

               
               for (i = 0; i < stages.stages.Count; i++)
               {
                   uxstagerole.Nodes.Add();
                   uxstagerole.Nodes[uxstagerole.Nodes.Count - 1].SetValue(0, stages.stages[i].name);
                   uxstagerole.Nodes[uxstagerole.Nodes.Count - 1].SetValue(10, stages.stages[i].id);
               }
              
               for (i=0; i < uxstagerole.Nodes.Count; i++)
               {
                   uxstagerole.Nodes[i].SetValue(1, "0");
                   //uxstagerole.Nodes[i].SetValue(1, "No");
                   uxstagerole.Nodes[i].SetValue(2, "0");
                   uxstagerole.Nodes[i].SetValue(3, "0");
                   uxstagerole.Nodes[i].SetValue(4, "0");
                   uxstagerole.Nodes[i].SetValue(5, "0");
                   uxstagerole.Nodes[i].SetValue(6, "0");
                   uxstagerole.Nodes[i].SetValue(7, "0");
                   uxstagerole.Nodes[i].SetValue(8, "0");
                   uxstagerole.Nodes[i].SetValue(9, "0");
                   for (j = 0; j < sr.stage_roles.Count; j++ )
                   {
                       if ((long)uxstagerole.Nodes[i].GetValue(10)== sr.stage_roles[j].stage_id)
                       {
                           if (sr.stage_roles[j].access == "F")
                               //uxstagerole.Nodes[i].SetValue(1, "Yes");
                                 uxstagerole.Nodes[i].SetValue(1, "1");
                           else if (sr.stage_roles[j].access == "M")
                               uxstagerole.Nodes[i].SetValue(2, "1");
                           else if (sr.stage_roles[j].access == "V")
                               uxstagerole.Nodes[i].SetValue(3, "1");
                           else if (sr.stage_roles[j].access == "R")
                               uxstagerole.Nodes[i].SetValue(4, "1");
                           else if (sr.stage_roles[j].access == "W")
                               uxstagerole.Nodes[i].SetValue(5, "1");
                           else if (sr.stage_roles[j].access == "L")
                               uxstagerole.Nodes[i].SetValue(6, "1");
                           else if (sr.stage_roles[j].access == "A")
                               uxstagerole.Nodes[i].SetValue(7, "1");
                           else if (sr.stage_roles[j].access == "D")
                               uxstagerole.Nodes[i].SetValue(8, "1");
                           else if (sr.stage_roles[j].access == "E")
                               uxstagerole.Nodes[i].SetValue(9, "1");
                       }
                   }
               }
               uxcpapps.ClearNodes();
               uxcustomapps.ClearNodes();
               for (i=0; i < sa.apps.Count; i++)
               {
                   if (sa.apps[i].custom==false)
                   {
                       uxcpapps.Nodes.Add();
                       uxcpapps.Nodes[uxcpapps.Nodes.Count-1].SetValue(0,sa.apps[i].app_name);
                       switch (sa.apps[i].access_level)
                       {
                           case 0:
                             uxcpapps.Nodes[uxcpapps.Nodes.Count - 1].SetValue(1, "None");
                                 break;
                           case 1:
                             uxcpapps.Nodes[uxcpapps.Nodes.Count-1].SetValue(1,"View");
                                 break;
                           case 2:
                             uxcpapps.Nodes[uxcpapps.Nodes.Count-1].SetValue(1,"Full");
                                 break;
                       }
                       uxcpapps.Nodes[uxcpapps.Nodes.Count-1].SetValue(2, sa.apps[i].app_id);
                   }
                   else
                   {
                       
                       uxcustomapps.Nodes.Add();
                       uxcustomapps.Nodes[uxcustomapps.Nodes.Count - 1].SetValue(0, sa.apps[i].app_name);
                       switch (sa.apps[i].access_level)
                       {
                           case 0:
                               uxcustomapps.Nodes[uxcustomapps.Nodes.Count - 1].SetValue(1, "None");
                               break;
                           case 1:
                               uxcustomapps.Nodes[uxcustomapps.Nodes.Count - 1].SetValue(1, "View");
                               break;
                           case 2:
                               uxcustomapps.Nodes[uxcustomapps.Nodes.Count - 1].SetValue(1, "Full");
                               break;
                       }
                       uxcustomapps.Nodes[uxcustomapps.Nodes.Count - 1].SetValue(2, sa.apps[i].app_id);
                   }
               }
           }
           catch (Exception ex)
           {
              bc_cs_security.certificate certificate = new bc_cs_security.certificate();
              bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "load_stage_role_and_access", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           }
           finally
           {
               uxstagerole.EndUpdate();
               uxcpapps.EndUpdate();
               uxcustomapps.EndUpdate();
           }
        }

        private void Ecpaccesschange (object sender,EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                  DevExpress.XtraEditors.ComboBoxEdit s;
                s = (  DevExpress.XtraEditors.ComboBoxEdit)sender;
               
                bc_om_dx_cp_apps_for_role.bc_om_dx_cp_app_for_role afr = new bc_om_dx_cp_apps_for_role.bc_om_dx_cp_app_for_role();
                afr.app_id = (bc_om_dx_cp_apps_for_role.ECP_ROLES)uxcpapps.FocusedNode.GetValue(2);
                afr.access_level = s.SelectedIndex;
                afr.role_id = grole_id;

                EventHandler<EsetccessArgs> handler = Esetappaccess;
                if (handler != null)
                {
                    EsetccessArgs args = new EsetccessArgs();
                    args.afr = afr;
                    handler(this, args);
                }
                
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "Ecpaccesschange ", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void Ecustomaccesschange(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DevExpress.XtraEditors.ComboBoxEdit s;
                s = (DevExpress.XtraEditors.ComboBoxEdit)sender;

                bc_om_dx_cp_apps_for_role.bc_om_dx_cp_app_for_role afr = new bc_om_dx_cp_apps_for_role.bc_om_dx_cp_app_for_role();
                afr.app_id = (bc_om_dx_cp_apps_for_role.ECP_ROLES)uxcustomapps.FocusedNode.GetValue(2);
                afr.access_level = s.SelectedIndex;
                afr.role_id = grole_id;

                EventHandler<EsetccessArgs> handler = Esetappaccess;
                if (handler != null)
                {
                    EsetccessArgs args = new EsetccessArgs();
                    args.afr = afr;
                    handler(this, args);
                }

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "Ecpaccesschange ", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void Esetview(object sender,EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DevExpress.XtraEditors.ImageComboBoxEdit s;
                s = (DevExpress.XtraEditors.ImageComboBoxEdit)sender;
                
                
                bc_om_dx_stage_role_access.bc_om_dx_stage_role sra = new bc_om_dx_stage_role_access.bc_om_dx_stage_role ();
                sra.role_id=grole_id;
                sra.add=false;
                if (s.SelectedIndex==1)
                    sra.add = true;
                //if (s.EditValue.ToString()=="No")
                //    sra.add=false;

                sra.access=uxstagerole.FocusedColumn.Tag.ToString();
                sra.stage_id=(long)uxstagerole.FocusedNode.GetValue(10);
                EventHandler<EstageaccessArgs> handler = Esetstageroleaccess;
                if (handler != null)
                {
                    EstageaccessArgs args = new EstageaccessArgs();
                    args.sra = sra;
                    handler(this, args);
                }
            }
           
           catch (Exception ex)
           {
              bc_cs_security.certificate certificate = new bc_cs_security.certificate();
              bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "Esetview", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void Eload_Role(object sender, EloadentityArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                EventHandler<EloadentityArgs> handler = Eloadrole;
                if (handler != null)
                {
                    EloadentityArgs args = new EloadentityArgs();
                    args.sentity = e.sentity;
                    handler(this, args);
                }
                grole_id = e.sentity.id;
                ribbonPage2.Visible = false;
                ribbonPage3.Visible = false;
                if (e.sentity.inactive == false)
                {

                    ribbonPage2.Visible = true;

                    ribbon.SelectedPage = ribbonPage2;
                    ribbonPage2.Text = e.sentity.name;
                    toggle_view_only(false);
                }
                else
                {

                    ribbonPage3.Visible = true;
                    ribbon.SelectedPage = ribbonPage3;
                    ribbonPage3.Text = e.sentity.name + " (inactive)";
                    toggle_view_only(true);
                }
                uxstagerole.Enabled = true;
                xtraTabControl1.Enabled = true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", " Eload_Role", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bc_dx_cp_edit fed = new bc_dx_cp_edit();
                Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
                string enter_text = "Enter name";
               

                if (Ced.load_data(fed, "Add New Role", "", enter_text) == true)
                {
                    fed.ShowDialog();
                    if (Ced.bsave == true)
                    {
                        if (bc_dx_entity_search1.check_name_exists(Ced.text) == true)
                        {
                            bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                            return;
                        }

                        EventHandler<EnewupdateroleArgs> handler = Enewupdaterole;
                        if (handler != null)
                        {
                            EnewupdateroleArgs args = new EnewupdateroleArgs();
                            args.role = new bc_om_dx_role();
                            args.role.updatemode = bc_om_dx_role.EROLEUPDTEMODE.ADD;
                            args.role.role_id = 0;
                            args.role.name = Ced.text;
                            handler(this, args);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "new", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EnewupdateroleArgs> handler = Enewupdaterole;
            if (handler != null)
            {
                EnewupdateroleArgs args = new EnewupdateroleArgs();
                args.role = new bc_om_dx_role();
                args.role.updatemode = bc_om_dx_role.EROLEUPDTEMODE.INACTIVE;
                args.role.role_id = grole_id;
                handler(this, args);
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bc_dx_cp_edit fed = new bc_dx_cp_edit();
                Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
                string enter_text = "Enter name";


                if (Ced.load_data(fed, "Change Role Name", ribbonPage2.Text, enter_text) == true)
                {
                    fed.ShowDialog();
                    if (Ced.bsave == true)
                    {
                        if (bc_dx_entity_search1.check_name_exists(Ced.text) == true)
                        {
                            bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                            return;
                        }

                        EventHandler<EnewupdateroleArgs> handler = Enewupdaterole;
                        if (handler != null)
                        {
                            EnewupdateroleArgs args = new EnewupdateroleArgs();
                            args.role = new bc_om_dx_role();
                            args.role.updatemode = bc_om_dx_role.EROLEUPDTEMODE.RENAME;
                            args.role.role_id = grole_id;
                            args.role.name = Ced.text;
                            handler(this, args);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_roles", "rename", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EnewupdateroleArgs> handler = Enewupdaterole;
            if (handler != null)
            {
                EnewupdateroleArgs args = new EnewupdateroleArgs();
                args.role = new bc_om_dx_role();
                args.role.updatemode = bc_om_dx_role.EROLEUPDTEMODE.ACTIVE;
                args.role.role_id = grole_id;
                handler(this, args);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            bc_cs_message omsg = new bc_cs_message("Blue Curve", "Are you sure you wish to delete role: " + ribbonPage3.Text, bc_cs_message.MESSAGE, true,false,  "Yes", "No", true);
            if (omsg.cancel_selected == true)
                return;
            EventHandler<EnewupdateroleArgs> handler = Enewupdaterole;
            if (handler != null)
            {
                EnewupdateroleArgs args = new EnewupdateroleArgs();
                args.role = new bc_om_dx_role();
                args.role.updatemode = bc_om_dx_role.EROLEUPDTEMODE.DELETE;
                args.role.role_id = grole_id;
                handler(this, args);
            }
        }
    }
 
}