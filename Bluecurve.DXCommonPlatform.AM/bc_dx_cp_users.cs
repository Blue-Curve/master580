using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using DevExpress.XtraBars;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_cp_users : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_cp_dx_users
    {
        public bc_dx_cp_users()
        {
            InitializeComponent();
        }
        public event EventHandler<EloadentityArgs> Eloaduser;
        public event EventHandler<EloaduserlinksArgs> Eloaduserlinks;
        public event EventHandler<EassignArgs> Eassign;
        public event EventHandler<EupdateuserArgs> Eupdateuser;
        public event EventHandler<EUserArgs> Eupdateattribute;
        public bool load_view(bool view_only)
        {
            
            bc_dx_entity_search1.Eloadentity += Eload_user;
            bc_dx_entity_search1.Einactiveactivechanged += Enoselection;
            bc_dx_entity_search1.Enoselection += Enoselection;
            tschema.SelectedPageChanged += Eload_links;

            dx_uc_attributes1.attribute_value_changed += Esaveattribute;
            dx_uc_attributes1.attribute_selection_changed += Eattributechanged;
            dx_uc_attributes1.delayed_input = true;
            dx_uc_attributes1.show_workflow = false;
            dx_uc_attributes1.load_data();

            bc_dx_entity_search1.single_class_id = 0;
            bc_dx_entity_search1.hide_class = true;
            bc_dx_entity_search1.hide_filter = false;
            bc_dx_entity_search1.class_mode = EFIXEDENTITYCLASSES.USER;
            bc_dx_entity_search1.load_data();
            
            ribbonPage2.Visible = false;
            ribbonPage3.Visible = false;

            if (view_only == false)
            {
                Ribbon.ApplicationDocumentCaption = Ribbon.ApplicationDocumentCaption + " - View Only";
                uxlinks.DoubleClick += Enavigate;
            }
            else
            {
                
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem6.Enabled = false;
                barButtonItem7.Enabled = false;


                 dx_uc_attributes1.disable_edit();
            }

            return true;
        }

        void Eattributechanged(bc_om_attribute att)
        {
            //barButtonItem10.Enabled = false;
            //barButtonItem10.Caption = "Attribute audit";
            //gattribute = null;
            //try
            //{
            //    barButtonItem10.Caption = att.name + " Attribute audit";
            //    barButtonItem10.Enabled = true;
            //    gattribute = att;
            //}
            //catch
            //{

            //}
        }
        void Esaveattribute(bc_om_attribute_value attval)
        {

            EventHandler<EUserArgs> handler = Eupdateattribute;
            if (handler != null)
            {

                EUserArgs args = new EUserArgs();
                args.user_id = guser.id;
                args.attval = attval;
                handler(this, args);
            }
        }

        void Enavigate(object sender, EventArgs e)
        {

            try
            {
                string mtitle = "";
                if (tschema.SelectedTabPageIndex  < 3)
                {
                    mtitle = "Maintain " + tschema.TabPages[tschema.SelectedTabPageIndex].Text;

                    EventHandler<EassignArgs> handler = Eassign;
                    if (handler != null)
                    {
                        EassignArgs args = new EassignArgs();
                        args.no_ordering = true;
                        bc_om_entity lent = new bc_om_entity();
                        lent.id = guser.id;
                        args.title = mtitle;

                        args.entity = lent;
                        if (tschema.SelectedTabPageIndex == 0)
                        {
                            args.area_id = EFIXEDENTITYCLASSES.BUS_AREA;
                            args.assign_class = -1;
                        }
                        else if (tschema.SelectedTabPageIndex == 1)
                        {
                            args.area_id = EFIXEDENTITYCLASSES.ASSOC_USER;
                            args.assign_class = 0;

                        }
                        else if (tschema.SelectedTabPageIndex == 2)
                        {
                            args.area_id = EFIXEDENTITYCLASSES.ROLE;
                            args.assign_class = -2;
                        }
                        int i;
                        bc_om_entity aent;
                        args.sel_entities = new List<bc_om_entity>();

                        for (i = 0; i < uxlinks.Nodes.Count; i++)
                        {
                            aent = new bc_om_entity();
                            aent.name = uxlinks.Nodes[i].GetValue(0).ToString();
                            aent.id = (long)uxlinks.Nodes[i].GetValue(2);
                            args.sel_entities.Add(aent);
                        }
                        handler(this, args);
                    }

                }
                

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }


        }
       
       
        private void  Eload_user (object sender, EloadentityArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                bc_om_attribute_value av;

                int i;

                //for (i = 0; i < dx_uc_attributes1.attributes.Count; i++)
                //{
                //    av = new bc_om_attribute_value();
                //    av.entity_id = entity.id;
                //    av.attribute_Id = dx_uc_attributes1.attributes[i].attribute_id;
                //    av.submission_code = dx_uc_attributes1.attributes[i].submission_code;
                //    if (dx_uc_attributes1.attributes[i].show_workflow == 1)
                //        av.show_workflow = true;
                //    entity.attribute_values.Add(av);

                //}
                EventHandler<EloadentityArgs> handler = Eloaduser;
                if (handler != null)
                {
                    EloadentityArgs args = new EloadentityArgs();
                    args.sentity = e.sentity;
                    handler(this, args);
                }

                //if (tschema.SelectedTabPageIndex == 0)
                //    load_links();
                //else
                //    tschema.SelectedTabPageIndex = 0;

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_users", " Eload_user", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        bc_om_user guser;

        public void  load_user(bc_om_user user,List <bc_om_attribute> attributes)
        {
            try
            {
                guser = user;


                dx_uc_attributes1.attributes = attributes;
                dx_uc_attributes1.values.Clear();
                bc_om_attribute_value aval;
                int i;
                for (i = 0; i < attributes.Count; i++)
                {
                    aval = new bc_om_attribute_value();
                    aval.value = attributes[i].default_value;
                    dx_uc_attributes1.values.Add(aval);
                  
                }
                dx_uc_attributes1.load_data();

                dx_uc_attributes1.Enabled = true;
                //passociations.Enabled = true;
                disable_toolbar(true);

                if (user.inactive == false)
                {

                    ribbonPage2.Visible = true;

                    ribbon.SelectedPage = ribbonPage2;
                    ribbonPage2.Text = user.first_name + " " + user.surname;
                }
                else
                {

                    ribbonPage3.Visible = true;
                    ribbon.SelectedPage = ribbonPage3;
                    ribbonPage3.Text = user.first_name + " " + user.surname + " (inactive)";
                }

                if (tschema.SelectedTabPageIndex == 0)
                    load_links();
                else
                    tschema.SelectedTabPageIndex = 0;

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "load_user", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }

        public void load_user_links(bc_om_cp_user_links elinks)
        {
            try
            {
               
                uxlinks.BeginUpdate();
                uxlinks.Nodes.Clear();
                int i;
                for (i = 0; i < elinks.area_entities.Count; i++)
                    {
                        uxlinks.Nodes.Add();
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.area_entities[i].name);
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, "");
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(2, elinks.area_entities[i].id);
                        if (elinks.area_entities[i].inactive == false)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = -1;
                            if (elinks.area_entities[i].class_id == 0)
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 4;
                        }
                        else
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 0;
                    }

                
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_users", "load_user_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                uxlinks.ExpandAll();
                uxlinks.EndUpdate();
            }
        }
        private void disable_toolbar(Boolean not_new)
        {
            ribbonPage2.Visible = false;
            ribbonPage3.Visible = false;

            if (not_new == false)
            {
                barButtonItem1.Enabled = false;
                barButtonItem1.Caption = "New";
            }
        }

        void Enoselection(object sender, EventArgs e)
        {
           
            ribbonPage2.Visible = false;
            ribbonPage3.Visible = false;
            dx_uc_attributes1.Enabled = false;
            dx_uc_attributes1.clear_values();
        }


        void Eload_links(object sender, EventArgs e)
        {
            load_links();
        }
        public void load_links()
        {

            Cursor.Current = Cursors.WaitCursor;
            EventHandler<EloaduserlinksArgs> handler = Eloaduserlinks;
            if (handler != null)
            {
                EloaduserlinksArgs args = new EloaduserlinksArgs();
               
                args.user_id = guser.id;
                switch (tschema.SelectedTabPageIndex)
                {
                    case 0:
                        args.area_id = EFIXEDENTITYCLASSES.BUS_AREA;
                        break;
                    case 1:
                        args.area_id = EFIXEDENTITYCLASSES.ASSOC_USER;
                        break;
                    case 2:
                        args.area_id = EFIXEDENTITYCLASSES.ROLE;
                        break;
                    default:
                        args.area_id = EFIXEDENTITYCLASSES.PREF;
                        //args.pref_type_id= 
                        break;
                }

                handler(this, args);
            }
        }


        private void ribbon_Click(object sender, EventArgs e)
        {

        }
        public void reload_users(string sel_entity_name)
        {
            bc_dx_entity_search1.load_class();
            bc_dx_entity_search1.select_entity(sel_entity_name);
        }
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EupdateuserArgs> handler = Eupdateuser;
            if (handler != null)
            {
                EupdateuserArgs args = new EupdateuserArgs();
                args.user = guser;
                args.user.write_mode = bc_om_user.SET_INACTIVE;
                args.reload = true;
                handler(this, args);
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EupdateuserArgs> handler = Eupdateuser;
            if (handler != null)
            {
                EupdateuserArgs args = new EupdateuserArgs();
                args.user = guser;
                args.user.write_mode = bc_om_user.SET_ACTIVE;
                args.reload = true;
                handler(this, args);
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EupdateuserArgs> handler = Eupdateuser;
            if (handler != null)
            {
                EupdateuserArgs args = new EupdateuserArgs();
                args.user = guser;
                args.user.write_mode = bc_om_user.DELETE;
                args.reload = true;
                handler(this, args);
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bc_dx_cp_edit fed = new bc_dx_cp_edit();
                Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
                if (Ced.load_data(fed, "Add New User", "", "Enter first and surname") == true)
                {
                    fed.ShowDialog();
                    if (Ced.bsave == true)
                    {
                        if (bc_dx_entity_search1.check_name_exists(Ced.text) == true)
                        {
                            bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                            return;
                        }

                        EventHandler<EupdateuserArgs> handler = Eupdateuser;
                        if (handler != null)
                        {
                            EupdateuserArgs args = new EupdateuserArgs();
                            args.user = new bc_om_user();
                            int fnend;
                            fnend = Ced.text.IndexOf(" ");
                            if (fnend > 0)
                            {
                                args.user.first_name = Ced.text.Substring(0,fnend);
                                args.user.surname =Ced.text.Substring(fnend, Ced.text.Length-fnend);
                            }
                            else
                                args.user.first_name = Ced.text;

                            args.user.write_mode = bc_om_user.INSERT_AND_SET_DEFAULT_ATTRIBUTE;
                            args.reload = true;
                            handler(this, args);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_users", "barButtonItem1_ItemClick", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
  
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            bc_dx_cp_roles fp = new bc_dx_cp_roles();
            Cbc_dx_cp_roles cp = new Cbc_dx_cp_roles();
            if (cp.load_data(fp,false) == true)
            {
                fp.ShowDialog();
            }
        }
    }
}