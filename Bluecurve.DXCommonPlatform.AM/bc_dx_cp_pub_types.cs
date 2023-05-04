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
    public partial class bc_dx_cp_pub_types : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_cp_dx_pub_types
    {
        public bc_dx_cp_pub_types()
        {
            InitializeComponent();
        }

        public event EventHandler<EloadentityArgs> Eloadpub_type;
        public event EventHandler<EpubtypeArgs> EAmmend_Pubtypes;
        public event EventHandler<EpubtypeArgs> Eupdateattribute;
        public event EventHandler<EloadpubtypelinksArgs> Eloadentitylinks;
        public event EventHandler<EassignArgs> Eassign;
        public event EventHandler<Eupdatetaxparams> Eupdate_params;
        public bool load_view(bool view_only,List<bc_om_attribute> attributes)
        {
            

            dx_uc_attributes1.delayed_input = true;
            dx_uc_attributes1.show_workflow = false;
            
            bc_om_attribute_value va;
            dx_uc_attributes1.attributes = attributes;
            int i;
            for (i = 0; i < attributes.Count ;i++ )
            {
                va = new bc_om_attribute_value();
                dx_uc_attributes1.values.Add(va);
            }
            dx_uc_attributes1.load_data();


            bc_dx_entity_search1.Eloadentity += Eloadpubtype;
            bc_dx_entity_search1.Einactiveactivechanged += Enoselection;
            bc_dx_entity_search1.Enoselection += Enoselection;

            dx_uc_attributes1.attribute_value_changed += Esaveattribute;
            //dx_uc_attributes1.publish_attribute += Epublishattribute;
            dx_uc_attributes1.attribute_selection_changed += Eattributechanged;

            repositoryItemComboBox1.SelectedIndexChanged += classify_param_changed;
            repositoryItemComboBox2.SelectedIndexChanged += classify_param_changed;

            tschema.SelectedPageChanged += ETabPagechanged;
            uxlinks.FocusedNodeChanged += nodechanged;
            bc_dx_entity_search1.single_class_id = 0;
            bc_dx_entity_search1.hide_class = true;
            bc_dx_entity_search1.hide_filter = false;
            bc_dx_entity_search1.class_mode = EFIXEDENTITYCLASSES.PUB_TYPE;
            bc_dx_entity_search1.load_data();

            if (view_only == true)
            {
                Ribbon.ApplicationDocumentCaption = Ribbon.ApplicationDocumentCaption + " - View Only";
                dx_uc_attributes1.disable_edit();
            }
            else
            {
                uxlinks.DoubleClick += Enavigate;
            }
            return true;
        }
        void classify_param_changed(object sender, EventArgs e)
        {
            try
            {
                object editval;
                DevExpress.XtraEditors.ComboBoxEdit s;
                s = (DevExpress.XtraEditors.ComboBoxEdit)sender;
                editval = s.EditValue;
            

                EventHandler<Eupdatetaxparams> handler = Eupdate_params;
                Eupdatetaxparams args = new Eupdatetaxparams();
                if (handler != null)
                {
                    args.values = new bc_om_dx_ext_ax_params();
                    args.values.pub_type_id = gpubtype.id;
                    args.values.class_id = (long)uxlinks.FocusedNode.GetValue(2);

                    if (s.Properties.Name == "repositoryItemComboBox1")
                    {
                        if (editval.ToString() == "Mandatory")
                        {
                            args.values.mandatory = true;
                            uxlinks.Columns[4].OptionsColumn.AllowEdit = true;
                            uxlinks.FocusedNode.SetValue(4, "at least one");
                            args.values.max_number = 0;
                        }
                        else
                        {
                            args.values.mandatory = false;
                            args.values.max_number = 0;
                            uxlinks.Columns[4].OptionsColumn.AllowEdit = false;
                            uxlinks.FocusedNode.SetValue(4, "na");
                        }
                        
                            args.values.max_number = 0;
                       
                    }
                    else
                    {
                        if (uxlinks.FocusedNode.GetValue(3).ToString() == "Mandatory")
                            args.values.mandatory = true;
                        else
                            args.values.mandatory = false;
                        if (editval.ToString().ToString() == "at least 1")
                            args.values.max_number = 0;
                        else
                            args.values.max_number = s.SelectedIndex;
                    }
                    handler(this, args);
                }
            }

            
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "classify_param_changed", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return;
            }
        }
        void Enavigate(object sender, EventArgs e)
        {

            try
            {
                string mtitle = "";
                mtitle = "Set  " + tschema.TabPages[tschema.SelectedTabPageIndex].Text;

               EventHandler<EassignArgs> handler = Eassign;
                    if (handler != null)
                    {
                        EassignArgs args = new EassignArgs();
                        args.title = mtitle;
                        args.entity = new bc_om_entity();
                        args.entity.id = gpubtype.id;
                        args.entity.name = gpubtype.name;
                        args.assign_class = 0;
                        switch  (tschema.SelectedTabPageIndex)
                        {
                            case 0:
                                args.area_id = EFIXEDENTITYCLASSES.CLASSIFY;
                                break;
                            case 1:
                                args.area_id = EFIXEDENTITYCLASSES.CHANNEL;
                                args.no_ordering = true;
                                break;
                            case 2:
                                args.area_id = EFIXEDENTITYCLASSES.eMODULE;
                                args.no_ordering = true;
                                break;
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
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "Enavigate", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return;
            }


        }


        public void reload_pub_types(string sel_pub_type_name)
        {
            bc_dx_entity_search1.load_class();
            bc_dx_entity_search1.select_entity(sel_pub_type_name);
        }
        bc_om_attribute gattribute;
        void Eattributechanged(bc_om_attribute att)
        {
            barButtonItem10.Enabled = false;
            barButtonItem10.Caption = "Attribute audit";
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
        void Esaveattribute(bc_om_attribute_value attval)
        {

            EventHandler<EpubtypeArgs> handler = Eupdateattribute;
            if (handler != null)
            {

                EpubtypeArgs args = new EpubtypeArgs();
                args.spubtype = gpubtype;
                args.attribute = attval;
                handler(this, args);
            }
        }

        //void Eload_pubtype(object sender, EloadentityArgs e)
        //{
        //    barButtonItem4.Enabled = true;
        //    barButtonItem5.Enabled = true;
        //    barButtonItem9.Enabled = true;


        //}
    
        private void   nodechanged(object sender, EventArgs args)
        {
           uxlinks.Columns[4].OptionsColumn.AllowEdit = false;
            if (uxlinks.FocusedNode.GetValue(3)=="Mandatory")
                uxlinks.Columns[4].OptionsColumn.AllowEdit = true;
        }
        private void ETabPagechanged(object sender, EventArgs args)
        {
            load_links();
        }
        
        public void load_links()
        {
            try
            { 
            uxlinks.Columns[3].Visible = false;
            uxlinks.Columns[4].Visible = false;
            uxlinks.Columns[5].Visible = false;
            uxlinks.OptionsView.ShowColumns = false;
            if (tschema.SelectedTabPageIndex==0)
            {
                uxlinks.Columns[3].Visible = true;
                uxlinks.Columns[4].Visible = true;
                //uxlinks.Columns[5].Visible = true;
                uxlinks.OptionsView.ShowColumns = true;
                uxlinks.Columns[0].Caption = "Class";
                uxlinks.Columns[3].Caption = "Required";
                uxlinks.Columns[4].Caption = "Precise Number";
                uxlinks.Columns[5].Caption = "Dependent Upon";
            }

            Cursor.Current = Cursors.WaitCursor;
            EventHandler<EloadpubtypelinksArgs> handler = Eloadentitylinks;
            if (handler != null)
            {

                EloadpubtypelinksArgs largs = new EloadpubtypelinksArgs();
                largs.pub_type_id = gpubtype.id;
                switch (tschema.SelectedTabPageIndex)
                {
                   case 0:
                        largs.link_type_id = EFIXEDENTITYCLASSES.CLASSIFY;
                       break;
                   case 1:
                       largs.link_type_id = EFIXEDENTITYCLASSES.CHANNEL;
                        break;
                   case 2:
                        largs.link_type_id = EFIXEDENTITYCLASSES.eMODULE;
                   break;
               }
                handler(this, largs);
            }
          
            }
            
           
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "load_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return;
            }
           
      }

        private void Eloadpubtype(object sender, EloadentityArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                EventHandler<EloadentityArgs> handler = Eloadpub_type;
                if (handler != null)
                {
                    EloadentityArgs args = new EloadentityArgs();
                    args.sentity = e.sentity;
                    handler(this, args);
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", " Eloadpubtype", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        bc_om_dx_cp_pub_type gpubtype;

        public void load_pub_type(bc_om_dx_cp_pub_type pubtype)
        {
            try
            {
                gpubtype = pubtype;
                
                int i;

                
                dx_uc_attributes1.attributes = pubtype.config.attributes.attributes;



                bc_om_attribute_value av;
                dx_uc_attributes1.values.Clear();
                for (i = 0; i < pubtype.config.attributes.attributes.Count; i++ )
                {
                    av = new bc_om_attribute_value();
                    av.value = pubtype.config.attributes.attributes[i].default_value;
                    dx_uc_attributes1.values.Add(av);
                }
                    
                 dx_uc_attributes1.load_data();

                dx_uc_attributes1.Enabled = true;
                //passociations.Enabled = true;
                //disable_toolbar(true);

                if (pubtype.inactive == false)
                {
                    ribbonPage3.Visible = true;
                    ribbon.SelectedPage = ribbonPage3;
                    ribbonPage3.Text = pubtype.name;
                    dx_uc_attributes1.enable_edit();
                    
                }
                else
                {
                    ribbonPage4.Visible = true;
                    ribbon.SelectedPage = ribbonPage4;
                    ribbonPage4.Text = pubtype.name + " (inactive)";
                    dx_uc_attributes1.disable_edit();
                }
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem6.Enabled = true;
                barButtonItem7.Enabled = true;
                barButtonItem8.Enabled = true;
                barButtonItem9.Enabled = true;
                barButtonItem10.Enabled = true;
                barButtonItem11.Enabled = true;
                tschema.SelectedTabPageIndex = 2;
                tschema.SelectedTabPageIndex = 0;

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pubtypes", "load_pub_type", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }




        void Enoselection(object sender, EventArgs e)
        {
            barButtonItem4.Enabled = false;
            barButtonItem5.Enabled = false;
            barButtonItem9.Enabled = false;
            barButtonItem10.Enabled = false;

            ribbonPage3.Visible = false;
            ribbonPage4.Visible = false;
          
        }

        private void ribbonStatusBar_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            bc_dx_cp_channel_config cf = new bc_dx_cp_channel_config();
            Cbc_dx_cp_channel_config ff = new Cbc_dx_cp_channel_config();
            if (ff.load_data(cf) == true)
            {
                cf.ShowDialog();
                return;
            }
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EpubtypeArgs> handler = EAmmend_Pubtypes;
            if (handler != null)
            {
                EpubtypeArgs args = new EpubtypeArgs();
                args.spubtype = gpubtype;
                args.spubtype.inactive = true;
                args.spubtype.write_mode = bc_om_dx_cp_pub_type.SET_INACTIVE;
                //args.reload = true;
                handler(this, args);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            bc_dx_cp_edit fed = new bc_dx_cp_edit();
            Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
            if (Ced.load_data(fed, "Rename " + gpubtype.name, gpubtype.name, "Edit name") == true)
            {
                fed.ShowDialog();
                if (Ced.bsave == true)
                {
                    if (bc_dx_entity_search1.check_name_exists(Ced.text) == true)
                    {
                        bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        return;
                    }
                    EventHandler<EpubtypeArgs> handler = EAmmend_Pubtypes;
                    if (handler != null)
                    {
                        EpubtypeArgs args = new EpubtypeArgs();
                        args.spubtype = gpubtype;
                        args.spubtype.name = Ced.text;
                        args.spubtype.inactive = true;
                        args.spubtype.write_mode = bc_om_dx_cp_pub_type.UPDATE_PT;
                        //args.reload = true;
                        handler(this, args);
                    }
                }
            }
        }
     

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            EventHandler<EpubtypeArgs> handler = EAmmend_Pubtypes;
            if (handler != null)
            {
                EpubtypeArgs args = new EpubtypeArgs();
                args.spubtype = gpubtype;
                args.spubtype.inactive = true;
                args.spubtype.write_mode = bc_om_dx_cp_pub_type.SET_ACTIVE;
                //args.reload = true;
                handler(this, args);
            
        }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<EpubtypeArgs> handler = EAmmend_Pubtypes;
            if (handler != null)
            {
                EpubtypeArgs args = new EpubtypeArgs();
                args.spubtype = gpubtype;
                args.spubtype.inactive = true;
                args.spubtype.write_mode = bc_om_dx_cp_pub_type.DELETE;
                //args.reload = true;
                handler(this, args);

            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        public void load_pub_type_links(bc_om_dx_pub_type_links pl)
        {
            try
            { 
            uxlinks.BeginUpdate();
            try
            {
                uxlinks.Nodes.Clear();
            }
            catch { }
            int i, bj;
                for (i = 0; i < pl.links.Count; i++)
                {
                    uxlinks.Nodes.Add();
                    uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, pl.links[i].name);
                    uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, "");
                    uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(2, pl.links[i].id);
                    if (pl.link_type== EFIXEDENTITYCLASSES.CLASSIFY)
                    {
                        if (pl.links[i].inactive == true)
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(3, "Mandatory");
                            if (pl.links[i].class_id == 0)
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(4, "at least 1");
                            else
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(4, pl.links[i].class_id.ToString() + " exactly") ;
                        }
                        else
                        {
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(3, "Optional");
                            uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(4, "na");
                        }
                      
                    }
                }
             uxlinks.EndUpdate();
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_cp_dx_pub_types", "load_pub_type_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
            }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bc_dx_cp_edit fed = new bc_dx_cp_edit();
                Cbc_dx_cp_edit Ced = new Cbc_dx_cp_edit();
                if (Ced.load_data(fed, "Add New Publication Type ", "", "Enter name") == true)
                {
                    fed.ShowDialog();
                    if (Ced.bsave == true)
                    {
                        if (bc_dx_entity_search1.check_name_exists(Ced.text) == true)
                        {
                            bc_cs_message omsg = new bc_cs_message("Blue Curve ", Ced.text + " already exists please try again.", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                            return;
                        }

                        EventHandler<EpubtypeArgs> handler = EAmmend_Pubtypes;
                        if (handler != null)
                        {
                            EpubtypeArgs args = new EpubtypeArgs();
                            args.spubtype = new bc_om_dx_cp_pub_type();
                            args.spubtype.name = Ced.text;

                            args.spubtype.write_mode = bc_om_dx_cp_pub_type.INSERT_AND_SET_DEFAULT_ATTRIBUTE;
                            //args.reload = true;
                            handler(this, args);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_pub_types", "barButtonItem1_ItemClick", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {

            bc_dx_cp_process_config fpc = new bc_dx_cp_process_config();
            Cbc_dx_cp_process_config Cpc = new Cbc_dx_cp_process_config();
            Cursor = Cursors.WaitCursor;
            if (Cpc.load_data(fpc,gpubtype.id, gpubtype.name,false)== true)
            {
                Cursor = Cursors.Default;
                fpc.ShowDialog();
            }
            Cursor = Cursors.Default;
        }

        private void bc_dx_entity_search1_Load(object sender, EventArgs e)
        {

        }
       

    }

    public class EpubtypeArgs : EventArgs
    {
        public bc_om_dx_cp_pub_type spubtype { get; set; }
        public bc_om_attribute_value attribute  { get; set; }
    }
    public class EloadpubtypelinksArgs : EventArgs
    {
        public long pub_type_id { get; set; }
        public EFIXEDENTITYCLASSES link_type_id { get; set; }
    }
     public class Eupdatetaxparams : EventArgs
    {
        public bc_om_dx_ext_ax_params values { get; set; }
    }
}