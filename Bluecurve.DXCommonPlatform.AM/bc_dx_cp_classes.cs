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
    public partial class bc_dx_cp_classes : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_cp_dx_classes 
    {
        public bc_dx_cp_classes()
        {
            InitializeComponent();
        }
        bc_om_dx_cp_entity_classes _classes;
        bc_om_schemas _schemas;
        bool _view_only;
        public event EventHandler<Eload_schema_classesArgs> Eload_schema_classes;
        public event EventHandler<Eload_class_attributesArgs> Eload_class_attributes;
        public event EventHandler<Eupdate_class_attributeArgs> Eupdate_class_attribute;
        public event EventHandler<Eload_class_attributesArgs> Eloadallattributes;
      
        public bool load_view(bc_om_schemas schemas,bool read_only)
        {
            try
            {
                _view_only = read_only;
                if (read_only == true)
                {
                    ribbonPage1.Visible = false;
                    ribbonPage3.Visible = false;
                    ribbonPage1.Groups.Clear();
                    ribbonPage2.Groups.Clear();
                    ribbonPage3.Groups.Clear();
                    cfilter.ReadOnly = true;

                    Ribbon.ApplicationDocumentCaption = Ribbon.ApplicationDocumentCaption + " - View Only";
                }

            _schemas = schemas;
            tschema.SelectedPageChanged += Eload_links;
            cfilter.SelectedIndexChanged += Esetreadonly;
            uxschema.Properties.Items.Clear();
                
            int i;
            for (i = 0; i < schemas.schemas.Count; i ++ )
            {
                uxschema.Properties.Items.Add(schemas.schemas[i].schema_name);
            }
            if (uxschema.Properties.Items.Count > 0)
            {
                uxschema.SelectedIndex = 0;
            }
            return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            return false;
        }


        public  void load_class_attributes(bc_om_class_attributes ca)
        {
            try
            {
                uxattributes.BeginUpdate();
                uxattributes.ClearNodes();
                int i;
                for (i=0; i < ca.attributes.Count; i ++)
                {
                    uxattributes.Nodes.Add();
                    uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(0, ca.attributes[i].name);
                    if (ca.attributes[i].is_lookup==true)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Lookup");
                    else if (ca.attributes[i].repeats== 1)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Memo");
                    else if (ca.attributes[i].repeats == 2)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Step");
                    else if (ca.attributes[i].type_id==1)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Text");
                    else if (ca.attributes[i].type_id==2)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Number");
                    else if (ca.attributes[i].type_id == 3)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Boolean");
                    else if (ca.attributes[i].type_id == 4)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Date");
                    if (ca.attributes[i].submission_code==2 && ca.class_id > 0)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(2, "Yes");
                    else
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(2, "No");
                    if (ca.attributes[i].show_workflow == 1 && ca.class_id > 0)
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(3, "Yes");
                     else
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(3 ,"No");
                     if (ca.attributes[i].nullable==0)
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(4, "Yes");
                     else
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(4, "No");
                     if (ca.attributes[i].is_def==true)
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(5, "Yes");
                     else
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(5, "No");
                     if (ca.attributes[i].persmission==1)
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(6, "Yes");
                     else
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(6, "No");
                     if (ca.attributes[i].show_filter == true)
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(8, "Yes");
                     else
                         uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(8, "No");

                     uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(7, ca.attributes[i].attribute_id);
                }
                uxattributes.FocusedNode = null;
                if (uxattributes.Nodes.Count > 0)
                  uxattributes.FocusedNode = uxattributes.Nodes[0];
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "load_class_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                uxattributes.EndUpdate();
            }
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
        private void Esetreadonly(object sender, EventArgs e)
        {
            if (uxattributes.FocusedColumn.AbsoluteIndex == 6)
            {
                string ro = uxattributes.FocusedNode.GetValue(6).ToString();

                if (ro == "No")
                    update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.SET_READ_ONLY);
                else
                    update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.CLEAR_READ_ONLY);
            }
            else if (uxattributes.FocusedColumn.AbsoluteIndex == 8)
            {
                string ro = uxattributes.FocusedNode.GetValue(8).ToString();

                if (ro == "No")
                    update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.SET_FILTER);
                else
                    update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.CLEAR_FILTER);
            }
        }

        private void Eload_links(object sender, EventArgs e)
        {
            load_links();
        }
        void load_links()
        {
            try
            {
                uxlinks.ClearNodes();
                int i,j;
                for (i = 0; i < _classes.classes.Count - 1; i++)
                {
                    if (_classes.classes[i].class_name==uxclasses.FocusedNode.GetValue(0).ToString())
                    {
                        if (tschema.SelectedTabPageIndex == 0)
                        {

                            for (j = 0; j < _classes.classes[i].parent_classes.Count; j++)
                            {
                                if (_classes.classes[i].parent_classes[j].class_name != "ROOT")
                                {
                                    uxlinks.Nodes.Add();
                                    uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, _classes.classes[i].parent_classes[j].class_name);
                                    uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, _classes.classes[i].parent_classes[j].class_id);
                                    uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 3;
                                }
                            }
                        }
                        else
                        {

                            for (j = 0; j < _classes.classes[i].child_classes.Count; j++)
                            {
                                uxlinks.Nodes.Add();
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(0, _classes.classes[i].child_classes[j].class_name);
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].SetValue(1, _classes.classes[i].child_classes[j].class_id);
                                uxlinks.Nodes[uxlinks.Nodes.Count - 1].StateImageIndex = 5;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "load_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        private void uxschema_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (uxschema.SelectedIndex == -1)
                    return;
                EventHandler<Eload_schema_classesArgs> handler = Eload_schema_classes;
                if (handler != null)
                {
                    Eload_schema_classesArgs args = new Eload_schema_classesArgs();
                    args.schema_id = _schemas.schemas[uxschema.SelectedIndex].schema_id;
                    handler(this, args);
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "uxschema_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally 
            {
                Cursor = Cursors.Default;
            }
           
        }
        public void load_class_links(bc_om_dx_cp_entity_classes classes)
        {
            _classes = classes;
            load_class_tree();
        }
        void load_class_tree()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                uxclasses.BeginUpdate();
                uxclasses.ClearNodes();
                uxclasses.Nodes.Add();
                uxclasses.Nodes[0].SetValue(0, "Users");
                uxclasses.Nodes[0].SetValue(1, (long)EFIXEDENTITYCLASSES.USER);
                uxclasses.Nodes[0].StateImageIndex = 8;
                uxclasses.Nodes.Add();
                uxclasses.Nodes[1].SetValue(0, "Pub Types");
                uxclasses.Nodes[1].SetValue(1, (long)EFIXEDENTITYCLASSES.PUB_TYPE);
                uxclasses.Nodes[1].StateImageIndex = 9;
               
                int i,j;
                if (uxview.SelectedIndex == 2)
                {
                    for (i = 0; i < _classes.classes.Count; i++)
                    {
                        if (_classes.classes[i].class_name != "ROOT")
                        {
                            uxclasses.Nodes.Add();
                            uxclasses.Nodes[uxclasses.Nodes.Count - 1].SetValue(0, _classes.classes[i].class_name);
                            uxclasses.Nodes[uxclasses.Nodes.Count - 1].SetValue(1, _classes.classes[i].class_id);
                        }
                    }
                }
                else
                {
                    uxclasses.Nodes.Add();
                    uxclasses.Nodes[2].SetValue(0, "Linked Classes");

                    for (i = 0; i < _classes.classes.Count; i++)
                    {
                        if (uxview.SelectedIndex == 0 || uxview.SelectedIndex == -1)
                        {

                            if (_classes.classes[i].class_name != "ROOT" && _classes.classes[i].parent_classes.Count == 0 && _classes.classes[i].child_classes.Count != 0)
                            {

                                uxclasses.Nodes[2].Nodes.Add();
                                uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1].SetValue(0, _classes.classes[i].class_name);
                                uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1].SetValue(1, _classes.classes[i].class_id);
                               
                                propogate_class(uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1], _classes.classes[i].class_id, _classes, true);
                              
                            }
                            else if (_classes.classes[i].class_name == "ROOT")
                            {
                                for (j = 0; j < _classes.classes[i].child_classes.Count; j++)
                                {

                                    uxclasses.Nodes[2].Nodes.Add();
                                    uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1].SetValue(0, _classes.classes[i].child_classes[j].class_name);
                                    uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1].SetValue(1, _classes.classes[i].child_classes[j].class_id);
                                    propogate_class(uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1], _classes.classes[i].child_classes[j].class_id, _classes,true);
                                }
                            }
                        }
                        else
                        {
                            if (_classes.classes[i].child_classes.Count == 0 && _classes.classes[i].parent_classes.Count !=0)
                            {

                                uxclasses.Nodes[2].Nodes.Add();
                                uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1].SetValue(0, _classes.classes[i].class_name);
                                uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1].SetValue(1, _classes.classes[i].class_id);
                                for (j = 0; j < _classes.classes[i].parent_classes.Count; j++)
                                {
                                    propogate_class(uxclasses.Nodes[2].Nodes[uxclasses.Nodes[2].Nodes.Count - 1], _classes.classes[i].class_id, _classes,false);
                                }
                            }

                        }
                    }



                    uxclasses.Nodes.Add();
                    uxclasses.Nodes[3].SetValue(0, "Unlinked Classes");
                    for (i = 0; i < _classes.classes.Count; i++)
                    {
                        if (_classes.classes[i].class_name != "ROOT" && _classes.classes[i].child_classes.Count == 0 && _classes.classes[i].parent_classes.Count == 0)
                        {
                            uxclasses.Nodes[3].Nodes.Add();
                            uxclasses.Nodes[3].Nodes[uxclasses.Nodes[3].Nodes.Count - 1].SetValue(0, _classes.classes[i].class_name);
                            uxclasses.Nodes[3].Nodes[uxclasses.Nodes[3].Nodes.Count - 1].SetValue(1, _classes.classes[i].class_id);
                        }
                    }
                }
                uxclasses.FocusedNode = null;
                uxclasses.FocusedNode = uxclasses.Nodes[0];
                uxclasses.ExpandAll();
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "load_class_links", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                uxclasses.EndUpdate();
                Cursor = Cursors.Default;
            }
        }
        void propogate_class(DevExpress.XtraTreeList.Nodes.TreeListNode Node, long class_id, bc_om_dx_cp_entity_classes classes, bool down = true)
        {
          try
          { 

            int i,j;
             for (i = 0; i < classes.classes.Count; i++)
             {
                 if (classes.classes[i].class_id==class_id)
                 {
                     if (down == true)
                     {
                         for (j = 0; j < classes.classes[i].child_classes.Count; j++)
                         {
                             Node.Nodes.Add();
                             Node.Nodes[Node.Nodes.Count - 1].SetValue(0, classes.classes[i].child_classes[j].class_name);
                             Node.Nodes[Node.Nodes.Count - 1].SetValue(1, classes.classes[i].child_classes[j].class_id);
                             propogate_class(Node.Nodes[Node.Nodes.Count - 1], classes.classes[i].child_classes[j].class_id, classes, down);
                         }
                     }
                     else
                     {
                         for (j = 0; j < classes.classes[i].parent_classes.Count; j++)
                         {
                             if (classes.classes[i].parent_classes[j].class_name != "ROOT")
                             {
                                 Node.Nodes.Add();
                                 Node.Nodes[Node.Nodes.Count - 1].SetValue(0, classes.classes[i].parent_classes[j].class_name);
                                 Node.Nodes[Node.Nodes.Count - 1].SetValue(1, classes.classes[i].parent_classes[j].class_id);
                                 propogate_class(Node.Nodes[Node.Nodes.Count - 1], classes.classes[i].parent_classes[j].class_id, classes, down);
                             }
                         }
                     }
                 }
             }
          }
          catch (Exception ex)
          {
              bc_cs_security.certificate certificate = new bc_cs_security.certificate();
              bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "propogate_class", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
          }        
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_class_tree();
        }

        private void uxclasses_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                passociations.Enabled = false;
                barButtonItem5.Visibility=   BarItemVisibility.Never;
                barButtonItem6.Visibility=   BarItemVisibility.Never;
                ribbonPage2.Visible = false;
                int i;
                for (i = 2; i < uxclasses.Nodes.Count; i++)
                {
                    clear_selected_class(uxclasses.Nodes[i]);

                }

                if (uxclasses.FocusedNode != null && uxclasses.FocusedNode.GetValue(0)!=null)
                {
                    if (uxclasses.FocusedNode.GetValue(0).ToString() != "Linked Classes" && uxclasses.FocusedNode.GetValue(0).ToString() != "Unlinked Classed")
                    {
                       
                        ribbonPage2.Visible = true;
                        ribbonPage2.Text = "Attributes for " + uxclasses.FocusedNode.GetValue(0).ToString();

                        EventHandler<Eload_class_attributesArgs> handler = Eload_class_attributes;
                        if (handler != null)
                        {
                            Eload_class_attributesArgs args = new Eload_class_attributesArgs();
                            if (uxclasses.FocusedNode.GetValue(0).ToString() == "Users")
                                args.class_id = 0;
                            else if (uxclasses.FocusedNode.GetValue(0).ToString() == "Pub Types")
                                args.class_id = -6;
                            else
                                args.class_id = (long)uxclasses.FocusedNode.GetValue(1);
                            handler(this, args);
                        }
                    
                    }

                    if (uxclasses.FocusedNode.GetValue(0).ToString() != "Users" && uxclasses.FocusedNode.GetValue(0).ToString() != "Pub Types" && uxclasses.FocusedNode.GetValue(0).ToString() != "Linked Classes" && uxclasses.FocusedNode.GetValue(0).ToString() != "Unlinked Classed")
                    {
                        barButtonItem5.Visibility = BarItemVisibility.Always;
                        barButtonItem6.Visibility = BarItemVisibility.Always;
                        barButtonItem5.Caption = "Rename " + uxclasses.FocusedNode.GetValue(0).ToString();
                        barButtonItem6.Caption = "Make " + uxclasses.FocusedNode.GetValue(0).ToString() + " Inactive";
                        Ribbon.SelectedPage = ribbonPage3;
                        passociations.Enabled = true;

                        for (i = 2; i < uxclasses.Nodes.Count; i++)
                        {
                            set_selected_class(uxclasses.Nodes[i], uxclasses.FocusedNode.GetValue(0).ToString());
                           
                        }
                       
                        load_links();
                    }
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "uxclasses_FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
      
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }
        void clear_selected_class(DevExpress.XtraTreeList.Nodes.TreeListNode Node)
        {
            try
            {
             int i;
             for (i = 0; i < Node.Nodes.Count; i++)
             {
                 Node.Nodes[i].StateImageIndex = -1;
                 clear_selected_class(Node.Nodes[i]);
             }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "clear_selected_class", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        void set_selected_class(DevExpress.XtraTreeList.Nodes.TreeListNode Node, string class_name)
        {
            try
            { 
            int i,j;
            for (i = 0; i < Node.Nodes.Count; i++)
            {
                
              if (Node.Nodes[i].GetValue(0).ToString() == class_name)
              {
                  Node.Nodes[i].StateImageIndex = 1;
                  if (Node.Nodes[i].ParentNode.Level > 0)
                  { 
                   if (uxview.SelectedIndex==0)
                       Node.Nodes[i].ParentNode.StateImageIndex = 3;
                   else
                       Node.Nodes[i].ParentNode.StateImageIndex = 5;
                   }
                   for (j=0; j < Node.Nodes[i].Nodes.Count; j ++ )
                   {

                       if (uxview.SelectedIndex == 0)
                           Node.Nodes[i].Nodes[j].StateImageIndex = 5;
                       else
                           Node.Nodes[i].Nodes[j].StateImageIndex = 3;

                   }

              }
              set_selected_class(Node.Nodes[i], class_name);
             }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "set_selected_class", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void uxattributes_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                barButtonItem8.Enabled = false;
                barButtonItem9.Enabled = false;
                barButtonItem10.Enabled = false;
                uxattributes.Columns[8].OptionsColumn.ReadOnly = true;

                if (uxattributes.FocusedNode != null && uxattributes.FocusedNode.GetValue(0) != null)
                {
                    Ribbon.SelectedPage = ribbonPage2;
                    //if ((long)uxattributes.FocusedNode.GetValue(7) < 0 && uxclasses.FocusedNode.GetValue(0).ToString() == "Pub Types")
                    //    return;

                    barButtonItem8.Enabled = true;
                    if (uxattributes.Nodes.Count > 1)
                    {
                        if (uxattributes.Nodes[0]!= uxattributes.FocusedNode)
                            barButtonItem9.Enabled = true;
                        if ( uxattributes.Nodes[uxattributes.Nodes.Count-1]!=  uxattributes.FocusedNode )
                            barButtonItem10.Enabled = true;
                    }

                    if (uxattributes.FocusedNode.GetValue(1).ToString() == "Boolean" || uxattributes.FocusedNode.GetValue(1).ToString() == "Lookup")
                        uxattributes.Columns[8].OptionsColumn.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "uxattributes_FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            { 
            EventHandler<Eload_class_attributesArgs> handler = Eloadallattributes;
            if (handler != null)
            {
                Eload_class_attributesArgs args = new Eload_class_attributesArgs();
                args.class_id = (long)uxclasses.FocusedNode.GetValue(1);
                args.class_name = uxclasses.FocusedNode.GetValue(0).ToString();
                handler(this, args);
            }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "assign attribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
           update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.REMOVE);
        }
        void update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode mode)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                bc_om_dx_cp_entity_class_attribute ca = new bc_om_dx_cp_entity_class_attribute();
                ca.class_id = (long)uxclasses.FocusedNode.GetValue(1);
                ca.attribute_id = (long)uxattributes.FocusedNode.GetValue(7);
                ca.mode = mode;
                EventHandler<Eupdate_class_attributeArgs> handler = Eupdate_class_attribute;
                if (handler != null)
                {
                    Eupdate_class_attributeArgs args = new Eupdate_class_attributeArgs();
                    args.ca = ca;
                    handler(this, args);
                }

           
            int i;
            for (i = 0; i < uxattributes.Nodes.Count; i++)
            {
                if ((long)uxattributes.Nodes[i].GetValue(7) == ca.attribute_id)
                {
                    uxattributes.FocusedNode = uxattributes.Nodes[i];
                    break;
                }
            }
        }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_classes", "update_class_attribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
            finally
            {
                Cursor = Cursors.Default;
            }
           
        }


        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.MOVE_UP);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            update_class_attribute(bc_om_dx_cp_entity_class_attribute.Emode.MOVE_DOWN);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }
}