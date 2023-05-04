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
using BlueCurve.CommonPlatform.AM;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_cp_container : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_container
    {
        public bc_dx_cp_container()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue");


        }
       private void form_closing(object sender, FormClosingEventArgs e)
        {
            closeall();
        }

           
       private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
       {
           closeall();
       }
       private void closeall()
       {
           try {fe.Close();}   catch  {}
           try {fs.Close();}   catch  {}
           try {fu.Close();}   catch  {}
           try {fa.Close();}   catch  {}
           try {fc.Close();}   catch  {}
           try {fr.Close();}   catch  {}
            
            Hide();
        }
        bc_dx_cp_rf_entity fe;
        bc_dx_sp_scheduler fs;
        bc_dx_cp_users fu;
        bc_dx_cp_pub_types fp;
        bc_dx_cp_attributes fa;
        bc_dx_cp_classes fc;
        bc_dx_cp_roles fr;



        public bool load_view(bc_om_dx_cp_apps_for_role ar, string user_name, string role)
        {
            try
            {

                Cursor = Cursors.WaitCursor;
                ribbon.ApplicationDocumentCaption = "Blue Curve Common Platform - " + user_name + " (" + role + ")";
                barButtonItem8.Visibility=BarItemVisibility.Never;

                if (bc_cs_central_settings.show_authentication_form == 1 || bc_cs_central_settings.override_username_password==true)
                {
                    barButtonItem8.Visibility = BarItemVisibility.Always;
                }

                ribbon.SelectedPage = ribbonPage1;
                barCheckItem1.Visibility= BarItemVisibility.Never;
             
                barCheckItem3.Visibility= BarItemVisibility.Never;
                barCheckItem4.Visibility= BarItemVisibility.Never;
                barCheckItem7.Visibility = BarItemVisibility.Never;

                Ribbon.SelectedPageChanged += ribbon_page_changed;
                ribbonPage2.Visible = false;
                ribbonPage3.Visible = false;
                ribbonPage4.Visible = false;
                FormClosing += form_closing;
                bool read_only;
                int i;
                for (i=0; i < ar.apps.Count; i ++)
                {
                   
                    if (ar.apps[i].app_id == bc_om_dx_cp_apps_for_role.ECP_ROLES.SCHEDULER && ar.apps[i].access_level > 0)
                    {
                        ribbonPage4.Visible = true;
                        Ribbon.SelectedPage = ribbonPage4;

                        fs = new bc_dx_sp_scheduler();
                        Cbc_dx_cp_scheduler cs = new Cbc_dx_cp_scheduler();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (cs.load_data(fs, read_only) == true)
                        {
                            fs.TopLevel = false;
                            fs.Parent = this;
                            fs.Dock = DockStyle.Fill;
                            show_form("bc_dx_sp_scheduler");
                        }
                        else
                            return false;
                      
                        barCheckItem2.Enabled = true;
                        barCheckItem2.Checked = true;

                    }
                    else if (ar.apps[i].app_id == bc_om_dx_cp_apps_for_role.ECP_ROLES.cSTRUCTURE && ar.apps[i].access_level > 0)
                    {
                        ribbonPage3.Visible = true;
                        Ribbon.SelectedPage = ribbonPage3;
                        fa = new bc_dx_cp_attributes();
                        Cbc_dx_cp_attributes ca = new Cbc_dx_cp_attributes();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (ca.load_data(fa, read_only) == true)
                        {
                          
                            fa.TopLevel = false;
                            fa.Parent = this;
                            fa.Dock = DockStyle.Fill;
                            show_form("bc_dx_cp_attributes");
                        }
                        else
                            return false;
                     

                        fc = new bc_dx_cp_classes();
                        Cbc_cp_dx_classes cc = new Cbc_cp_dx_classes();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (cc.load_data(fc, read_only) == true)
                        {
                            fc.TopLevel = false;
                            fc.Parent = this;
                            fc.Dock = DockStyle.Fill;
                        }
                        else
                            return false;

                    }

                    else if (ar.apps[i].app_id == bc_om_dx_cp_apps_for_role.ECP_ROLES.ROLES && ar.apps[i].access_level > 0)
                    {
                        ribbonPage2.Visible = true;
                        Ribbon.SelectedPage = ribbonPage2;
                        fr = new bc_dx_cp_roles();
                        Cbc_dx_cp_roles cr = new Cbc_dx_cp_roles();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (cr.load_data(fr, read_only) == true)
                        {
                            fr.TopLevel = false;
                            fr.Parent = this;
                            fr.Dock = DockStyle.Fill;
                            barCheckItem7.Visibility = BarItemVisibility.Always;
                            show_form("bc_dx_cp_roles");
                          
                        }
                        else
                            return false;
                    }
                    else if (ar.apps[i].app_id == bc_om_dx_cp_apps_for_role.ECP_ROLES.USERS && ar.apps[i].access_level > 0)
                    {
                        ribbonPage2.Visible = true;
                        Ribbon.SelectedPage = ribbonPage2;
                        fu = new bc_dx_cp_users();
                        Cbc_cp_dx_users cu = new Cbc_cp_dx_users();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (cu.load_data(fu, read_only) == true)
                        {
                            fu.TopLevel = false;
                            fu.Parent = this;
                            fu.Dock = DockStyle.Fill;
                            barCheckItem3.Visibility = BarItemVisibility.Always;
                            show_form("bc_dx_cp_users");


                        }
                        else
                            return false;
                    }
                    else if (ar.apps[i].app_id == bc_om_dx_cp_apps_for_role.ECP_ROLES.PUBTYPES && ar.apps[i].access_level > 0)
                    {
                        ribbonPage2.Visible = true;
                        Ribbon.SelectedPage = ribbonPage2;
                        fp = new bc_dx_cp_pub_types();
                        Cbc_cp_dx_pub_types cp = new Cbc_cp_dx_pub_types();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (cp.load_data(fp, read_only) == true)
                        {
                            fp.TopLevel = false;
                            fp.Parent = this;
                            fp.Dock = DockStyle.Fill;
                            barCheckItem4.Visibility = BarItemVisibility.Always;
                            show_form("bc_dx_cp_pub_types");


                        }
                        else
                            return false;
                    }
                    else if (ar.apps[i].app_id == bc_om_dx_cp_apps_for_role.ECP_ROLES.ENTITIES && ar.apps[i].access_level > 0)
                    {
                        ribbonPage2.Visible = true;
                        Ribbon.SelectedPage = ribbonPage2;
                        fe = new bc_dx_cp_rf_entity();
                        Cbc_cp_dx_entity ce = new Cbc_cp_dx_entity();
                        read_only = false;
                        if (ar.apps[i].access_level == 1)
                            read_only = true;
                        if (ce.load_data(fe, read_only) == true)
                        {
                            fe.TopLevel = false;
                            fe.Parent = this;
                            fe.Dock = DockStyle.Fill;
                            barCheckItem1.Visibility = BarItemVisibility.Always;
                            show_form("bc_dx_cp_rf_entity");

                        }
                        else
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_container", "load_viewy", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        void show_form(string name)
        {
            int i;
            for (i=0; i < Controls.Count; i++)
            {
                if (Controls[i].Name == "bc_dx_cp_rf_entity" || Controls[i].Name == "bc_dx_cp_pub_types" || Controls[i].Name == "bc_dx_cp_users" || Controls[i].Name == "bc_dx_cp_roles" || Controls[i].Name == "bc_dx_cp_attributes" || Controls[i].Name == "bc_dx_cp_rf_classes" || Controls[i].Name == "bc_dx_sp_scheduler")
                  Controls[i].Hide();
                if (Controls[i].Name == name)
                {
                    Controls[i].Show();
                }
            }

        }

        void ribbon_page_changed(object sender, EventArgs e)
       {
           show_form("");
           switch (Ribbon.SelectedPage.PageIndex)
           {
             
               case 1:
                   if (barCheckItem1.Checked == true)
                       show_form("bc_dx_cp_rf_entity");
                   else if (barCheckItem3.Checked == true)
                       show_form("bc_dx_cp_users");
                   else if (barCheckItem4.Checked == true)
                       show_form("bc_dx_cp_pub_types");
                   else if (barCheckItem7.Checked == true)
                       show_form("bc_dx_cp_roles");
                   else if (barCheckItem1.Visibility == BarItemVisibility.Always)
                       barCheckItem1.Checked = true;
                   else if (barCheckItem3.Visibility == BarItemVisibility.Always)
                       barCheckItem3.Checked = true;
                   else if (barCheckItem4.Visibility == BarItemVisibility.Always)
                       barCheckItem4.Checked = true;
                   else if (barCheckItem7.Visibility == BarItemVisibility.Always)
                       barCheckItem7.Checked = true;
                   break;
               case 2:
                   if (barCheckItem5.Checked == true)
                       show_form("bc_dx_cp_attributes");
                   else if (barCheckItem6.Checked == true)
                       show_form("bc_dx_cp_classes");
                   else if (barCheckItem5.Visibility == BarItemVisibility.Always)
                       barCheckItem5.Checked = true;
                   else if (barCheckItem6.Visibility == BarItemVisibility.Always)
                       barCheckItem6.Checked = true;
                   break;
               case 3:
                   show_form("bc_dx_sp_scheduler");
                   break;
           }

       }

       
        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barCheckItem1.Checked == true)
            {
                show_form("bc_dx_cp_rf_entity");
                barCheckItem3.Checked = false;
                barCheckItem4.Checked = false;
                barCheckItem7.Checked = false;
            }
           
        }

       

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barCheckItem3.Checked == true)
            {
                show_form("bc_dx_cp_users");
                barCheckItem1.Checked = false;
                barCheckItem4.Checked = false;
                barCheckItem7.Checked = false;
            }
       
            



        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {

            if (barCheckItem4.Checked == true)
            {
                show_form("bc_dx_cp_pub_types");
                barCheckItem1.Checked = false;
                barCheckItem3.Checked = false;
                barCheckItem7.Checked = false;
            }
          
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        

        private void barCheckItem5_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barCheckItem5.Checked == true)
            {
                show_form("bc_dx_cp_attributes");
                barCheckItem6.Checked = false;
            }
           
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barCheckItem6_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barCheckItem6.Checked == true)
            {
                show_form("bc_dx_cp_classes");
                barCheckItem5.Checked = false;
            }
           
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (barCheckItem3.Checked == true)
            {
                show_form("bc_dx_cp_roles");
                barCheckItem1.Checked = false;
                barCheckItem3.Checked = false;
                barCheckItem4.Checked = false;

            }

            
            
        }

        private void barCheckItem7_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barCheckItem7.Checked == true)
            {
                show_form("bc_dx_cp_roles");
                barCheckItem1.Checked = false;
                barCheckItem3.Checked = false;
                barCheckItem4.Checked = false;
            }
        }

        private void barButtonItem8_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Cbc_dx_full_logon lo = new Cbc_dx_full_logon();
            lo.remove_credentials_file();
            
            closeall();

        }
      
    }
   

}
