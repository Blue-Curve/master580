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
using BlueCurve.CommonPlatform.AM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_cp_rf_entity : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_cp_dx_entity
    {
        public bc_dx_cp_rf_entity()
        {
            InitializeComponent();
        }


        bool _view_only;
        bc_om_schemas _schemas = new bc_om_schemas();
        bc_om_entity gentity;


        public event EventHandler<EloadentityArgs> Eloadentity;
        public event EventHandler<EloadclassArgs> Eloadclass;

        public event EventHandler<EloadentitylinksArgs> Eloadentitylinks;
        public event EventHandler<EassignArgs> Eassign;
        public event EventHandler<EupdateentityArgs> Eupdateentity;

        //public event EventHandler<EloadclassArgs> Eentitydblclick;
        bc_om_attribute gattribute;
       
        
        public Boolean load_view(bc_om_schemas schemas, bool view_only)
        {

            try
            {
                _view_only = view_only;
                _schemas = schemas;

                bc_dx_entity_search1.Eloadentity += Eload_entity;
                bc_dx_entity_search1.Eclasschanged += Eload_class;
                bc_dx_entity_search1.Einactiveactivechanged += Enoselection;
                bc_dx_entity_search1.Enoselection += Enoselection;
                tschema.SelectedPageChanged += Eload_links;
                
                dx_uc_attributes1.attribute_value_changed += Esaveattribute;
                dx_uc_attributes1.publish_attribute += Epublishattribute;
                dx_uc_attributes1.attribute_selection_changed += Eattributechanged;
                uxlinks.Click += Eauditlinks;
                dx_uc_attributes1.show_workflow = true;
                dx_uc_attributes1.delayed_input = true;
                dx_uc_attributes1.Enabled = true;
                passociations.Enabled = false;
                ribbonPage2.Visible = false;
                ribbonPage3.Visible = false;
                barButtonItem11.Enabled = false;
                bc_dx_entity_search1.load_data();
                int i;



                for (i = 0; i < schemas.schemas.Count; i++)
                {
                    if (i == 0)
                        tschema.TabPages[i].Text = schemas.schemas[i].schema_name;
                    else
                        tschema.TabPages.Add(schemas.schemas[i].schema_name);
                }
                for (i = 0; i < schemas.pref_types.Count; i++)
                {
                    tschema.TabPages.Add(schemas.pref_types[i].pref_type_name);
                }

                if (view_only == false)
                {
                    uxlinks.DoubleClick += Enavigate;
                }
                else
                {
                    Ribbon.ApplicationDocumentCaption = Ribbon.ApplicationDocumentCaption + " - View Only";
                 
                    barButtonItem1.Enabled = false;
                    barButtonItem2.Enabled = false;
                    barButtonItem3.Enabled = false;
                    barButtonItem6.Enabled = false;
                    barButtonItem7.Enabled = false;
                    dx_uc_attributes1.disable_edit();
                }
                return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }

        }
       
        public void reload_entities(string sel_entity_name)
        {
            bc_dx_entity_search1.load_class();
            bc_dx_entity_search1.select_entity(sel_entity_name);
        }
        void Eattributechanged(bc_om_attribute att)
        {
            barButtonItem10.Enabled=false;
            barButtonItem10.Caption =  "Attribute audit";
            gattribute = null;
            try
            {
                barButtonItem10.Caption = att.name + " Attribute audit";
                barButtonItem10.Enabled = true;
                gattribute = att;
            }
            catch
            {

            }
        }
        void  Esaveattribute(bc_om_attribute_value attval)
        {
            EventHandler<EupdateentityArgs> handler = Eupdateentity;
            if (handler != null)
            {
               
                EupdateentityArgs args = new EupdateentityArgs();
                args.entity = gentity;
                gentity.attribute_values.Clear();
                attval.entity_id=gentity.id;
                
                attval.value_changed = true;
                gentity.attribute_values.Add(attval);
                args.entity.write_mode = bc_om_entity.ONLY_ATTRIBUTES;
                args.reload = false;
                handler(this, args);
            }
        }
        void Epublishattribute(bc_om_attribute_value attval)
        {
            EventHandler<EupdateentityArgs> handler = Eupdateentity;
            if (handler != null)
            {
              
                EupdateentityArgs args = new EupdateentityArgs();
                args.entity = gentity;
                gentity.attribute_values.Clear();
                attval.entity_id = gentity.id;
                attval.value_changed = true;
                attval.publish_draft_value = true;
                gentity.attribute_values.Add(attval);
                args.entity.write_mode = bc_om_entity.ONLY_ATTRIBUTES;
                args.reload = false;
                handler(this, args);
            }
        }


        int navpageindex = -1;
        void  Enoselection(object sender, EventArgs e)
        {
          
            ribbonPage2.Visible = false;
            ribbonPage3.Visible = false;
            dx_uc_attributes1.Enabled = false;
            dx_uc_attributes1.clear_values();
            uxlinks.Nodes.Clear();
            passociations.Enabled = false;
        }
        void Enavigate(object sender, EventArgs e)
        {

            try
            {
                string mtitle = "";
                if (tschema.SelectedTabPageIndex >= _schemas.schemas.Count)
                {

                    mtitle = "Maintain Associations to  " + tschema.TabPages[tschema.SelectedTabPageIndex].Text;

                    EventHandler<EassignArgs> handler = Eassign;
                    if (handler != null)
                    {
                        EassignArgs args = new EassignArgs();
                        args.title = mtitle;
                        args.entity = gentity;
                        args.assign_class = 0;
                        args.area_id = EFIXEDENTITYCLASSES.USER;
                        args.pref_type_id = _schemas.pref_types[tschema.SelectedTabPageIndex - _schemas.schemas.Count].pref_type_id;


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
                else if (uxlinks.FocusedNode.GetValue(1).ToString() != "")
                {
                    navpageindex = tschema.SelectedTabPageIndex;
                    bc_dx_entity_search1.navigate(uxlinks.FocusedNode.GetValue(1).ToString(), uxlinks.FocusedNode.GetValue(0).ToString());
                }
                else
                {
                    mtitle = "Maintain " + gentity.name + " Associations to  " + uxlinks.FocusedNode.GetValue(0).ToString();
                }
                if (mtitle != "")
                {

                    EventHandler<EassignArgs> handler = Eassign;
                    if (handler != null)
                    {

                        EassignArgs args = new EassignArgs();
                        args.title = mtitle;
                        args.entity = gentity;
                        args.area_id = EFIXEDENTITYCLASSES.ENTITY;
                        args.assign_class = (long)uxlinks.FocusedNode.GetValue(2);
                        args.max_number = (long)uxlinks.FocusedNode.GetValue(3);
                        args.schema_id = _schemas.schemas[tschema.SelectedTabPageIndex].schema_id;

                        if (uxlinks.FocusedNode.StateImageIndex == 2)
                            args.parent = true;
                        else
                            args.parent = false;


                        int i;
                        bc_om_entity aent;
                        args.sel_entities = new List<bc_om_entity>();

                        for (i = 0; i < uxlinks.FocusedNode.Nodes.Count; i++)
                        {

                            aent = new bc_om_entity();
                            aent.name = uxlinks.FocusedNode.Nodes[i].GetValue(0).ToString();

                            aent.id = (long)uxlinks.FocusedNode.Nodes[i].GetValue(2);

                            args.sel_entities.Add(aent);

                        }
                        handler(this, args);
                    }

                }

            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }


        }

        void Eload_links(object sender, EventArgs e)
        {
            load_links();
        }
        public void load_links()
        {

            Cursor.Current = Cursors.WaitCursor;
            EventHandler<EloadentitylinksArgs> handler = Eloadentitylinks;
            if (handler != null)
            {

                EloadentitylinksArgs args = new EloadentitylinksArgs();
                args.entity_id = gentity.id;
                if (tschema.SelectedTabPageIndex > _schemas.schemas.Count - 1)
                    args.pref_type_id = _schemas.pref_types[tschema.SelectedTabPageIndex - _schemas.schemas.Count].pref_type_id;
                else
                    args.schema_id = _schemas.schemas[tschema.SelectedTabPageIndex].schema_id;
                handler(this, args);
            }
        }

        public void load_class_attributes(List<bc_om_attribute> attributes, string class_name)
        {
            
            ribbonPage1.Text = class_name;
            //disable_toolbar(false);
            if (_view_only==false)
              barButtonItem1.Enabled = true;
            barButtonItem1.Caption = "New " + class_name;


            dx_uc_attributes1.attributes = attributes;
            dx_uc_attributes1.values = new List<bc_om_attribute_value>();
            bc_om_attribute_value att_value;
            int i;
            for (i = 0; i < dx_uc_attributes1.attributes.Count; i++)
            {
                att_value = new bc_om_attribute_value();
                dx_uc_attributes1.values.Add(att_value);
            }
            dx_uc_attributes1.load_data();
        }

        void Eload_class(object sender, EloadclassArgs e)
        {
            
            EventHandler<EloadclassArgs> handler = Eloadclass;
            if (handler != null)
            {
                EloadclassArgs args = new EloadclassArgs();
                args = e;
                handler(this, args);
            }
        }

        void Eload_entity(object sender, EloadentityArgs e)
        {

            lload_entity(e.sentity);
        }

        public void load_entity(bc_om_entity entity)
        {
            try
            {
                gentity = entity;
                int i;
                bc_om_attribute_value av;
                for (i = 0; i < entity.attribute_values.Count; i++)
                {
                    av = new bc_om_attribute_value();
                    av = (bc_om_attribute_value)entity.attribute_values[i];

                    dx_uc_attributes1.values[i].value = av.value;
                    dx_uc_attributes1.values[i].published_value = av.published_value;
                    dx_uc_attributes1.values[i].steps = av.steps;
                }
                dx_uc_attributes1.load_data();

                dx_uc_attributes1.Enabled = true;
                passociations.Enabled = true;
                disable_toolbar(true);

                if (entity.inactive == false)
                {

                    ribbonPage2.Visible = true;

                    ribbon.SelectedPage = ribbonPage2;
                    ribbonPage2.Text = entity.name;
                }
                else
                {

                    ribbonPage3.Visible = true;
                    ribbon.SelectedPage = ribbonPage3;
                    ribbonPage3.Text = entity.name + " (inactive)";
                }

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "load_entity", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

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


        void lload_entity(bc_om_entity entity)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                bc_om_attribute_value av;

                int i;

                for (i = 0; i < dx_uc_attributes1.attributes.Count; i++)
                {
                    av = new bc_om_attribute_value();
                    av.entity_id = entity.id;
                    av.attribute_Id = dx_uc_attributes1.attributes[i].attribute_id;
                    av.submission_code = dx_uc_attributes1.attributes[i].submission_code;
                    if (dx_uc_attributes1.attributes[i].show_workflow==1)
                      av.show_workflow = true;
                    entity.attribute_values.Add(av);

                }
                EventHandler<EloadentityArgs> handler = Eloadentity;
                if (handler != null)
                {
                    EloadentityArgs args = new EloadentityArgs();
                    args.sentity = entity;
                    handler(this, args);
                }

                if (tschema.SelectedTabPageIndex == 0)
                    load_links();
                else
                    tschema.SelectedTabPageIndex = 0;

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "lload_entity", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public void load_entity_links(bc_om_cp_entity_links elinks)
        {
            try
            {
                uxlinks.BeginUpdate();
                uxlinks.Nodes.Clear();
                int i, j;
                if (elinks.pref_type_id > 0)
                {
                    for (i = 0; i < elinks.pref_users.Count; i++)
                    {
                        uxlinks.Nodes.Add();
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.pref_users[i].user_name);
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, "");
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(2, elinks.pref_users[i].id);
                        if (elinks.pref_users[i].inactive == false)
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 4;
                        else
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 0;
                    }
                }
                else
                {

                    for (i = 0; i < elinks.child_classes.Count; i++)
                    {
                        var queryp = from en in elinks.linked_entities
                                     where en.class_id == elinks.child_classes[i].class_id
                                     select new bc_om_entity
                                     {
                                         id = en.id,
                                         name = en.name,
                                         inactive = en.inactive
                                     };
                        List<bc_om_entity> sel_entity = queryp.ToList();


                        uxlinks.Nodes.Add();
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 2;
                        if (elinks.child_classes[i].class_type_Id > 0)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.child_classes[i].class_name + " (" + elinks.child_classes[i].class_type_Id.ToString() + ")");
                            if (sel_entity.Count !=elinks.child_classes[i].class_type_Id)
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 0;
                        }
                        else if (elinks.child_classes[i].class_type_Id == -1)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.child_classes[i].class_name + " (*)");
                            if (sel_entity.Count == 0)
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 0;
                        }
                        else
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.child_classes[i].class_name);
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, "");
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(2, elinks.child_classes[i].class_id);
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(3, elinks.child_classes[i].class_type_Id);
                      
                       
                        for (j = 0; j < sel_entity.Count; j++)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes.Add();
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].SetValue(0, sel_entity[j].name);
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].SetValue(1, elinks.child_classes[i].class_name);
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].SetValue(2, sel_entity[j].id);
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].Tag = sel_entity[j].id.ToString();
                            if (sel_entity[j].inactive == true)
                            {
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].StateImageIndex = 0;
                            }
                        }


                    }
                    for (i = 0; i < elinks.parent_classes.Count; i++)
                    {
                        var queryp = from en in elinks.linked_entities
                                     where en.class_id == elinks.parent_classes[i].class_id
                                     select new bc_om_entity
                                     {
                                         id = en.id,
                                         name = en.name,
                                         inactive = en.inactive
                                     };
                        List<bc_om_entity> sel_entity = queryp.ToList();

                        uxlinks.Nodes.Add();
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 3;

                        if (elinks.parent_classes[i].class_type_Id > 0)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.parent_classes[i].class_name + " (" + elinks.parent_classes[i].class_type_Id.ToString() + ")");
                            if (sel_entity.Count != elinks.parent_classes[i].class_type_Id)
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 0;
                        }
                        else if (elinks.parent_classes[i].class_type_Id == -1)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.parent_classes[i].class_name + " (*)");
                            if (sel_entity.Count == 0)
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 0;
                        }
                        else
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, elinks.parent_classes[i].class_name);
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, "");
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(2, elinks.parent_classes[i].class_id);
                     
                        
                        uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(3, elinks.parent_classes[i].class_type_Id);
                      
                        for (j = 0; j < sel_entity.Count; j++)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes.Add();
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].SetValue(0, sel_entity[j].name);
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].SetValue(1, elinks.parent_classes[i].class_name);
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].SetValue(2, sel_entity[j].id);
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].Tag = sel_entity[j].id.ToString();
                            if (sel_entity[j].inactive == true)
                            {
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].Nodes[j].StateImageIndex = 0;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "load_entity_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                uxlinks.ExpandAll();
                uxlinks.EndUpdate();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EupdateentityArgs> handler = Eupdateentity;
            if (handler != null)
            {
                EupdateentityArgs args = new EupdateentityArgs();
                args.entity =gentity;
                args.entity.write_mode = bc_om_entity.SET_INACTIVE;
                args.reload = true;
                handler(this, args);
            }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EupdateentityArgs> handler = Eupdateentity;
            if (handler != null)
            {
                EupdateentityArgs args = new EupdateentityArgs();
                args.entity = gentity;
                args.entity.write_mode = bc_om_entity.SET_ACTIVE;
                args.reload = true;
                handler(this, args);
            }
        }
        

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EupdateentityArgs> handler = Eupdateentity;
            if (handler != null)
            {
                EupdateentityArgs args = new EupdateentityArgs();
                args.entity = gentity;

                args.entity.write_mode = bc_om_entity.DELETE;
                args.reload = true;
                handler(this, args);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            load_audit();
        }
        void load_audit()
        {
            try
            {
               Cursor = Cursors.WaitCursor;
               bc_am_entity_user_audit fs = new bc_am_entity_user_audit();
               Cbc_am_entity_user_audit cfs = new Cbc_am_entity_user_audit();
               if (cfs.load_data(fs, gentity.id, bc_om_entity_user_audit.EKEY_TYPE.ENTITY, gentity.name) == true)
                 fs.ShowDialog();
            }
            catch
            {

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            load_audit();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            bc_dx_cp_edit fed = new bc_dx_cp_edit();
            Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
            if (Ced.load_data(fed, "Rename " + gentity.name, gentity.name, "Edit name") == true)
            {
                fed.ShowDialog();
                if (Ced.bsave==true)
                {
                    if (bc_dx_entity_search1.check_name_exists(Ced.text) == true)
                    {
                        bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        return;
                    }
                    EventHandler<EupdateentityArgs> handler = Eupdateentity;
                    if (handler != null)
                    {
                        EupdateentityArgs args = new EupdateentityArgs();
                        gentity.name = Ced.text;
                        args.entity = gentity;
                        args.entity.write_mode = bc_om_entity.UPDATE;
                        args.reload = true;
                        handler(this, args);
                    }


                }
            }
        }
       

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            { 
            bc_dx_cp_edit fed = new bc_dx_cp_edit();
            Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
            string enter_text = "Enter name";
            if (bc_dx_entity_search1.selected_class.entry_description != null)
               if (bc_dx_entity_search1.selected_class.entry_description != "")
                  enter_text=bc_dx_entity_search1.selected_class.entry_description;

            if (Ced.load_data(fed, "Add New " + bc_dx_entity_search1.selected_class.class_name, "", enter_text) == true)
            {
                fed.ShowDialog();
                if (Ced.bsave == true)
                {
                    if (bc_dx_entity_search1.check_name_exists(Ced.text) ==true)
                    {
                        bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        return;
                    }

                    EventHandler<EupdateentityArgs> handler = Eupdateentity;
                    if (handler != null)
                    {
                        EupdateentityArgs args = new EupdateentityArgs();
                        args.entity= new  bc_om_entity();
                        args.entity.name = Ced.text;
                        args.entity.class_id = bc_dx_entity_search1.selected_class.class_id;
                        args.entity.write_mode = bc_om_entity.INSERT_AND_SET_DEFAULT_ATTRIBUTES;
                        args.reload = true;
                        handler(this, args);
                    }


                }
            }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "barButtonItem1_ItemClick", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                 

                Cursor = Cursors.WaitCursor;
                bc_am_attribute_audit fs = new bc_am_attribute_audit();
                Cbc_am_attribute_audit cfs = new Cbc_am_attribute_audit();

                if (cfs.load_data(fs, gentity.name, "", "", gentity.id, bc_om_attribute_audit.EATTRIBUTE_TYPE.ENTITY, true, true) == true)
                    fs.ShowDialog();
                else
                {
                    bc_cs_message omsg = new bc_cs_message("Blue Curve", "No Audit Information.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                }
            }
            catch
            {

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
            Cursor = Cursors.WaitCursor;
            bc_am_attribute_audit fs = new bc_am_attribute_audit();
            Cbc_am_attribute_audit cfs = new Cbc_am_attribute_audit();

            if (cfs.load_data(fs, gentity.name, gattribute.name, gattribute.attribute_id.ToString(), gentity.id, bc_om_attribute_audit.EATTRIBUTE_TYPE.ENTITY, true, false) == true)
                fs.ShowDialog();
            else
            {
                bc_cs_message omsg = new bc_cs_message("Blue Curve", "No Audit Information.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
            }
            }
            catch
            {

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
           try
           {
               Cursor = Cursors.WaitCursor;
            bc_am_audit_entity_links fs = new bc_am_audit_entity_links();
            Cbc_am_audit_entity_links cs = new Cbc_am_audit_entity_links();
             if (tschema.SelectedTabPageIndex >= _schemas.schemas.Count)
             {
                 //user link audit
                 int pref_type_id;
                 string pref_type_name;
                 pref_type_id= _schemas.pref_types[tschema.SelectedTabPageIndex-_schemas.schemas.Count].pref_type_id;
                 pref_type_name = _schemas.pref_types[tschema.SelectedTabPageIndex - _schemas.schemas.Count].pref_type_name;
                 if (cs.load_data(fs, gentity.id, 0, 0, false, pref_type_name, gentity.name, "", pref_type_id, 0, bc_om_audit_links.EAUDIT_TYPE.USERS_FOR_PREF) == true)
                 {
                     Cursor =Cursors.Default;
                     fs.ShowDialog();
                 }

             }
             else
             {
                // entity link audit
                bool bparent = false;
                long linked_class_id;
                string linked_class_name;
                linked_class_id= (long)uxlinks.FocusedNode.GetValue(2);
                linked_class_name = uxlinks.FocusedNode.GetValue(0).ToString();
                if (uxlinks.FocusedNode.StateImageIndex == 3)
                    bparent = true;

                if (cs.load_data(fs, gentity.id, linked_class_id , _schemas.schemas[tschema.SelectedTabPageIndex].schema_id, bparent, linked_class_name , gentity.name, _schemas.schemas[tschema.SelectedTabPageIndex].schema_name, -1, 0, bc_om_audit_links.EAUDIT_TYPE.TAXONOMY)  == true)
                {
                    Cursor = Cursors.Default;
                    fs.ShowDialog();
                }
             }
           }
           catch (Exception ex)
           {
               bc_cs_security.certificate certificate = new bc_cs_security.certificate();
               bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_entities", "barButtonItem1_ItemClick", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           }
            finally
           {
               Cursor = Cursors.Default;
           }
        }

        private void Eauditlinks(object sender, EventArgs e)
        {
            try
            {
                barButtonItem11.Enabled = false;
                if (tschema.SelectedTabPageIndex >= _schemas.schemas.Count)
                {
                    //user link audit
                    barButtonItem11.Enabled = true;
                }
                else
                {
                    
                        if (uxlinks.FocusedNode.GetValue(1).ToString() == "")
                        {
                            barButtonItem11.Enabled = true;
                        }
                 
                }
            }
            catch
            {

            }
        }

       

        
    }
}