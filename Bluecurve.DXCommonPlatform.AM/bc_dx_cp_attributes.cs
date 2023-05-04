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


    public partial class bc_dx_cp_attributes : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_attributes
    {
        bool _view_only;
        enum Eattribute_types
        {
            TEXT = 1,
            NUMBER = 2,
            BOOLEAN = 3,
            DATE = 5,
            LOOKUP = 10,
            MEMO = 11,
            STEP = 12
        }
       public event EventHandler<EventArgs> Eload_all_attributes;
       public event EventHandler<Eload_update_attributesArgs> Eupdate_attributes;
     
        bc_om_all_attributes _allattributes;
        public bc_dx_cp_attributes()
        {
            InitializeComponent();
        }
        public bool load_view(bc_om_all_attributes allattributes, bool read_only)
        {
            try
            {
                _view_only = read_only;
                if (read_only== true)
                {
                    //ribbonPage1.Visible = false;
                    ribbonPage1.Groups.Clear();

                    Ribbon.ApplicationDocumentCaption = Ribbon.ApplicationDocumentCaption + " - View Only";
                  
                    barButtonItem1.Enabled = false;
                    reposattname.ReadOnly = true;
                    reposdefsql.ReadOnly = true;
                    reposlength.ReadOnly = true;
                    reposlookupsql.ReadOnly = true;
                    repostype.ReadOnly = true;
                    reposmandat.ReadOnly = true;
                    reposworkflow.ReadOnly = true;
                    reposauditted.ReadOnly = true;
                    reposdef.ReadOnly = true;
                }
               
                ribbonPageGroup2.Visible = false;
                 _allattributes = allattributes;

                if (read_only == false)
                {
                    barButtonItem5.Visibility = BarItemVisibility.Always;
                    reposattname.EditValueChanged += name_changed;
                    reposdefsql.EditValueChanged += name_changed;
                    reposlength.EditValueChanged += name_changed;
                    reposlookupsql.EditValueChanged += name_changed;
                    repostype.SelectedIndexChanged += combo_changed;
                    reposmandat.SelectedIndexChanged += combo_changed;
                    reposworkflow.SelectedIndexChanged += combo_changed;
                    reposauditted.SelectedIndexChanged += combo_changed;
                    reposdef.SelectedIndexChanged += combo_changed;
                }
                    return load_attributes(_allattributes);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
            finally
            {
              
            }
        }
        public bool load_attributes(bc_om_all_attributes allattributes)
        {
            try
            {
                ribbonPageGroup2.Visible = false;
                if (_view_only==false)
                  barButtonItem5.Visibility = BarItemVisibility.Always;
                _allattributes = allattributes;
                uxattributes.BeginUpdate();
                uxattributes.ClearNodes();
             
                int i;
                for (i = 0; i < _allattributes.attributes.Count; i++)
                {
                    

                    uxattributes.Nodes.Add();
                    uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(0, _allattributes.attributes[i].name);
                    uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(3, "");
                    uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(8, "");
                    if (_allattributes.attributes[i].repeats == 1)
                    {
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Memo");
                    }
                    else if (_allattributes.attributes[i].repeats == 2)
                    {
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Step");
                    }
                    else if (_allattributes.attributes[i].is_lookup == true && _allattributes.attributes[i].repeats==0)
                    {
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Lookup");
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(3, _allattributes.attributes[i].lookup_sql);
                    }
                   
                    else
                    {
                        switch ((Eattribute_types)_allattributes.attributes[i].type_id)
                        {
                            case Eattribute_types.TEXT:
                                uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Text");
                                break;
                            case Eattribute_types.NUMBER:
                                uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Number");
                                break;
                            case Eattribute_types.BOOLEAN:
                                uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Boolean");
                                break;
                            case Eattribute_types.DATE:
                                uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(1, "Date");
                                break;
                        }

                    }
                    uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(2, _allattributes.attributes[i].length);

                    if (_allattributes.attributes[i].submission_code== 2)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(4, "Yes");
                    else
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(4, "No");

                    if (_allattributes.attributes[i].show_workflow==1)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(5, "Yes");
                    else
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(5, "No");

                    if (_allattributes.attributes[i].nullable == 0)
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(6, "Yes");
                    else
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(6, "No");


                    if (_allattributes.attributes[i].is_def  == true)
                    {
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(7, "Yes");
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(8, _allattributes.attributes[i].def_sql);
                    }
                    else
                        uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(7, "No");
                    uxattributes.Nodes[uxattributes.Nodes.Count - 1].SetValue(9, _allattributes.attributes[i].attribute_id);
                }
                uxattributes.FocusedNode = null;
                if (uxattributes.Nodes.Count > 0)
                    uxattributes.FocusedNode = uxattributes.Nodes[0];
                return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "load_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
            finally
            {
                uxattributes.EndUpdate();
            }
        }
        private void name_changed(object sender,  EventArgs e)
        {
          
            try
            {
                if (uxattributes.FocusedNode.GetValue(9).ToString() == "0")
                    return;
                DevExpress.XtraEditors.TextEdit s;
                s = (DevExpress.XtraEditors.TextEdit)sender;


              
                update_attribute(s.Properties.Name, s.EditValue.ToString());
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "name_changed", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }

        }
        private void combo_changed(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.ComboBoxEdit s;
                s = (DevExpress.XtraEditors.ComboBoxEdit)sender;
                if (s.SelectedIndex == -1)
                    return;
                if (uxattributes.FocusedNode.GetValue(9).ToString() == "0")
                {
                    if (s.Properties.Name == "repostype" && s.EditValue.ToString() == "Lookup")
                        uxattributes.Columns[3].OptionsColumn.AllowEdit = true;
                    else if (s.Properties.Name == "repostype" && s.EditValue.ToString() != "Lookup")
                        uxattributes.Columns[3].OptionsColumn.AllowEdit = false;
                    if (s.Properties.Name == "reposdef" && s.EditValue.ToString() == "Yes")
                        uxattributes.Columns[8].OptionsColumn.AllowEdit = true;
                    else if (s.Properties.Name == "reposdef" && s.EditValue.ToString() != "YEs")
                        uxattributes.Columns[8].OptionsColumn.AllowEdit = false;

                    return;
                }
           
            update_attribute(s.Properties.Name, s.EditValue.ToString());
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "combo_changed", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }
        void update_attribute(string name, string value)
        {
            try
            {
             
               
            bc_cs_message omsg;
            uxattributes.FocusedNode.StateImageIndex = -1;
               

            bc_om_attribute oatt = new bc_om_attribute();
            int i;
            for (i = 0; i < uxattributes.Nodes.Count - 1; i++ )
            {
                if (uxattributes.Nodes[i].GetValue(9).ToString() == uxattributes.FocusedNode.GetValue(9).ToString())
                {
                    if (set_attribute(ref oatt, i)== false)
                       return;
                    break;
                }
            }
           

            if (name== "reposattname")
            {
                if (value.Trim()=="")
                {
                    omsg = new bc_cs_message("Blue Curve", "name must be entered", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                    uxattributes.FocusedNode.StateImageIndex = 0;
                    return;
                }
                
                for (i=0; i < _allattributes.attributes.Count-1;i++)
                {
                    if (_allattributes.attributes[i].name.ToLower()== value.Trim().ToLower())
                    {
                        omsg = new bc_cs_message("Blue Curve", "name " + value + " already in use", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        uxattributes.FocusedNode.StateImageIndex = 0;
                        return;
                    }
                }
                oatt.name = value;
            }
            else if (name =="reposlength")
            {
                if (uxattributes.FocusedNode.GetValue(1).ToString() == "Memo")
                {
                    if (value.Trim()=="" || value == "0")
                    {
                        omsg = new bc_cs_message("Blue Curve", "Length must be greater than zero", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        uxattributes.FocusedNode.StateImageIndex = 0;
                        return;
                    }

                }
                int x;
                Int32.TryParse(value, out x);
                oatt.length = x;
            }
            else if (name == "reposlookupsql")
            {
                oatt.lookup_sql="";
                if (uxattributes.FocusedNode.GetValue(1).ToString() == "Lookup")
                {
                    if (value.Trim()=="")
                    {
                        omsg = new bc_cs_message("Blue Curve", "Lookup SQL must be entered", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        uxattributes.FocusedNode.StateImageIndex = 0;
                        return;
                    }
                    oatt.lookup_sql = value;
                }


            }
            else if (name == "reposdefsql")
            {
                oatt.def_sql = "";
                if (uxattributes.FocusedNode.GetValue(7).ToString() == "Yes")
                {
                    if (value.Trim() == "")
                    {
                        omsg = new bc_cs_message("Blue Curve", "Default SQL must be entered", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        uxattributes.FocusedNode.StateImageIndex = 0;
                        return;
                    }
                    oatt.def_sql = value;
                }


            }

            else if (name == "repostype")
            {
                if (value != "lookup")
                {
                    oatt.lookup_sql = "";
                    oatt.is_lookup = false;
                    uxattributes.FocusedNode.SetValue(3, "");
                    uxattributes.Columns[3].OptionsColumn.AllowEdit = false;
                }
                if (value != "Memo" &&   value != "Step")
                {
                    oatt.repeats = 0;
                }

                if (value == "Text" || value == "Lookup" || value == "Memo" || value == "Step")
                {
                    oatt.type_id = 1;
                    if (value == "Lookup")
                    {
                        oatt.is_lookup = true;
                        oatt.type_id = 1;
                        uxattributes.Columns[3].OptionsColumn.AllowEdit = true;
                        if (uxattributes.FocusedNode.GetValue(3).ToString().Trim() == "")
                        {
                            omsg = new bc_cs_message("Blue Curve", "lookup SQL must be entered", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                            uxattributes.FocusedNode.StateImageIndex = 0;
                            return;
                        }
                    }
                    else if (value == "Memo")
                    {
                        oatt.repeats = 1;
                        oatt.type_id = 1;
                        if (uxattributes.FocusedNode.GetValue(2).ToString() == "0")
                        {
                            omsg = new bc_cs_message("Blue Curve", "Length must be greater than zero", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                            uxattributes.FocusedNode.StateImageIndex = 0;
                            return;
                        }

                    }
                    else if (value == "Step")
                    {
                        oatt.repeats = 2;
                        oatt.type_id = 1;

                    }
                }

                else if (value == "Number")
                    oatt.type_id = 2;
                else if (value == "Date")
                    oatt.type_id = 5;
                else if (value == "Boolean")
                    oatt.type_id = 3;



            }
            else if (name == "reposauditted")
                if (value == "Yes")
                    oatt.submission_code = 2;
                else
                    oatt.submission_code = 1;
            else if (name == "reposworkflow")
                if (value == "Yes")
                    oatt.show_workflow = 1;
                else
                    oatt.show_workflow = 0;
            else if (name == "reposmandat")
                if (value == "Yes")
                    oatt.nullable = 0;
                else
                    oatt.nullable = 1;
            else if (name == "reposdef")
                if (value == "Yes")
                {
                    oatt.is_def = true;
                    uxattributes.Columns[8].OptionsColumn.AllowEdit = true;
                    if (uxattributes.FocusedNode.GetValue(8).ToString() == "")
                    {
                        omsg = new bc_cs_message("Blue Curve", "Default SQL must be entered", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        uxattributes.FocusedNode.StateImageIndex = 0;
                        return;
                    }
                }
                else
                {
                    oatt.is_def = false;
                    oatt.def_sql = "";
                    uxattributes.FocusedNode.SetValue(8, "");
                    uxattributes.Columns[8].OptionsColumn.AllowEdit = false;
                }



            update_attribute_parameter(oatt);
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "update_attribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);

            }
        }
        private void update_attribute_parameter(bc_om_attribute oatt)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                EventHandler<Eload_update_attributesArgs> handler = Eupdate_attributes;
                if (handler != null)
                {
                    Eload_update_attributesArgs args = new Eload_update_attributesArgs();
                    args.allattributes = new bc_om_all_attributes();
                    args.allattributes.write_mode = 2;
                    args.allattributes.attributes.Add(oatt);
                    handler(this, args);

                    //reload_attributes();
                    //int i;
                    //for (i = 0; i < uxattributes.Nodes.Count; i++)
                    //{
                    //    if (oatt.attribute_id == (long)uxattributes.Nodes[i].GetValue(9))
                    //    {
                    //        uxattributes.Nodes[i].Selected = true;
                    //        break;
                    //    }


                    //}
                    
                }

            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "update_attribute_parameter", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                uxattributes.BeginUpdate();
                if (ribbonPageGroup2.Visible == false)
                {
                    ribbonPageGroup2.Visible = true;
                    uxattributes.Nodes.Clear();
                    barButtonItem5.Visibility = BarItemVisibility.Never;
                
                }
                uxattributes.FocusedNode = null;
                uxattributes.Nodes.Add();
                uxattributes.FocusedNode = uxattributes.Nodes[uxattributes.Nodes.Count - 1];
                uxattributes.FocusedNode.SetValue(0, "[Enter New Name Here]");
                uxattributes.FocusedNode.SetValue(1, "Text");
                uxattributes.FocusedNode.SetValue(2, "0");
                uxattributes.FocusedNode.SetValue(3, "");
                uxattributes.FocusedNode.SetValue(4, "No");
                uxattributes.FocusedNode.SetValue(5, "No");
                uxattributes.FocusedNode.SetValue(6, "No");
                uxattributes.FocusedNode.SetValue(7, "No");
                uxattributes.FocusedNode.SetValue(8, "");
                uxattributes.FocusedNode.SetValue(9, 0);
                barButtonItem2.Enabled = true;
                barButtonItem4.Enabled = true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "new_attribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           
            }
            finally
            {
                uxattributes.EndUpdate();
            }

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            ribbonPageGroup2.Visible = false;
            barButtonItem5.Visibility = BarItemVisibility.Always;
            reload_attributes();
        }
        void reload_attributes()
        {
           
            try
            {
                Cursor = Cursors.WaitCursor;
                EventHandler<EventArgs> handler = Eload_all_attributes;
                if (handler != null)
                {
                    handler(this,null);
                }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "reload_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxattributes.Nodes.Remove(uxattributes.FocusedNode);
            if (uxattributes.Nodes.Count==0)
            {
                barButtonItem2.Enabled = false;
                barButtonItem4.Enabled = false;
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                uxattributes.FocusedNode = null;
                uxattributes.FocusedNode = uxattributes.Nodes[0];
           
                // check data
                int i,j;
                string errmsg = "";
                bool err, overallerr;
                overallerr = false;


                for (i = 0; i < uxattributes.Nodes.Count; i ++ )
                {
                    err = false;
                    if (uxattributes.Nodes[i].GetValue(0).ToString() == "[Enter New Name Here]" || uxattributes.Nodes[i].GetValue(0).ToString().Trim()== "")
                    {
                        err = true;
                        uxattributes.Nodes[i].StateImageIndex = 0;
                        errmsg = errmsg + Environment.NewLine + "row " + (i+1).ToString() + " enter name";
                        overallerr = true;
                        uxattributes.Nodes[i].StateImageIndex = 0;
                        continue;
                   
                    }
                    for (j=0; j< _allattributes.attributes.Count; j++)
                    {
                        if (_allattributes.attributes[j].name.ToLower() == uxattributes.Nodes[i].GetValue(0).ToString().ToLower())
                        {
                            err = true;
                            errmsg = errmsg + Environment.NewLine  + _allattributes.attributes[j].name + " already exists";
                         }
                        if (err == true)
                        {
                            overallerr = true;
                            uxattributes.Nodes[i].StateImageIndex = 0;
                            continue;
                        }
                    }
                    for (j = 0; j < uxattributes.Nodes.Count; j ++ )
                    {
                        if(j!=i && uxattributes.Nodes[i].GetValue(0).ToString().ToLower()== uxattributes.Nodes[j].GetValue(0).ToString().ToLower())
                        {
                            err = true;
                            errmsg = errmsg + Environment.NewLine + uxattributes.Nodes[i].GetValue(0).ToString() + " already exists";
                        }
                        if (err == true)
                        {
                            overallerr = true;
                            uxattributes.Nodes[i].StateImageIndex = 0;
                            continue;
                        }
                    }

                        if (uxattributes.Nodes[i].GetValue(1).ToString() == "Lookup")
                        {
                            if (uxattributes.Nodes[i].GetValue(3) == null)
                            {
                                err = true;
                                errmsg = errmsg + Environment.NewLine + uxattributes.Nodes[i].GetValue(0).ToString() + " lookup SQL must be entered";

                            }
                            else if (uxattributes.Nodes[i].GetValue(3).ToString().Trim() == "")
                            {
                                err = true;
                                errmsg = errmsg + Environment.NewLine + uxattributes.Nodes[i].GetValue(0).ToString() + " lookup SQL must be entered";

                            }
                        }

                    if (uxattributes.Nodes[i].GetValue(1).ToString() == "Memo")
                    {
                        if (uxattributes.Nodes[i].GetValue(2).ToString() == "0")
                        {
                            err = true;
                            errmsg = errmsg + Environment.NewLine + uxattributes.Nodes[i].GetValue(0).ToString() + " length must be greater than 0";
                        
                        }
                        
                    }

                    if (uxattributes.Nodes[i].GetValue(7).ToString() == "Yes")
                    {
                        if (uxattributes.Nodes[i].GetValue(8) == null)
                        {
                            err = true;
                            errmsg = errmsg + Environment.NewLine + uxattributes.Nodes[i].GetValue(0).ToString() + " default SQL must be entered";

                        }
                        else if (uxattributes.Nodes[i].GetValue(8).ToString().Trim() == "")
                        {
                            err = true;
                            errmsg = errmsg + Environment.NewLine + uxattributes.Nodes[i].GetValue(0).ToString() + " default SQL must be entered";

                        }

                    }


                    if (err==true)
                    {
                        uxattributes.Nodes[i].StateImageIndex = 0;
                        overallerr=true;
                    }
                    else
                        uxattributes.Nodes[i].StateImageIndex = 1;
                        
                }
                if (overallerr == true)
                {
                    errmsg = "Please correct the following error: " + errmsg;
                    bc_cs_message msg = new bc_cs_message("Blue Curve", errmsg, bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                    return;
                }

                    EventHandler<Eload_update_attributesArgs> handler = Eupdate_attributes;
                    if (handler != null)
                    {
                        Eload_update_attributesArgs args = new Eload_update_attributesArgs();
                        args.allattributes = new bc_om_all_attributes();
                        args.allattributes.write_mode = 0;
                        args.allattributes.attributes = new List<bc_om_attribute>();
                        bc_om_attribute oatt;
                        for (i = 0; i < uxattributes.Nodes.Count;i++ )
                        {
                            oatt= new bc_om_attribute();
                            if (set_attribute(ref oatt, i) == false)
                                return;
                            args.allattributes.attributes.Add(oatt);
                        }

                        handler(this, args);
                    }

                ribbonPageGroup2.Visible = false;
                if (_view_only== true)
                  barButtonItem5.Visibility = BarItemVisibility.Always;
                reload_attributes();
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "save", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }

       bool set_attribute (ref bc_om_attribute oatt, int index)
        {
            try
            {
                if (uxattributes.Nodes[index].GetValue(9).ToString() == "0")
                    oatt.attribute_id = 0;
                else
                    oatt.attribute_id = (long)uxattributes.Nodes[index].GetValue(9);
                oatt.lookup_sql = "";
                oatt.name = uxattributes.Nodes[index].GetValue(0).ToString();
                if (uxattributes.Nodes[index].GetValue(1).ToString() == "Text")
                oatt.type_id = 1;
                else if (uxattributes.Nodes[index].GetValue(1).ToString() == "Number")
                oatt.type_id = 2;
                else if (uxattributes.Nodes[index].GetValue(1).ToString() == "Date")
                oatt.type_id = 5;
                else if (uxattributes.Nodes[index].GetValue(1).ToString() == "Boolean")
                oatt.type_id = 3;
                else if (uxattributes.Nodes[index].GetValue(1).ToString() == "Lookup")
                {
                  oatt.type_id = 1;
                  oatt.is_lookup=true;
                  oatt.lookup_sql = uxattributes.Nodes[index].GetValue(3).ToString();
                }
                else if (uxattributes.Nodes[index].GetValue(1).ToString() == "Memo")
            {
                oatt.type_id = 1;
                oatt.repeats = 1;
            }
                else if (uxattributes.Nodes[index].GetValue(1).ToString() == "Step")
                {
                    oatt.type_id = 1;
                    oatt.repeats = 2;
                }
            
            int x;
            Int32.TryParse(uxattributes.Nodes[index].GetValue(2).ToString(), out x);

            oatt.length = x;
            oatt.submission_code = 1;
            if (uxattributes.Nodes[index].GetValue(4).ToString() == "Yes")
                oatt.submission_code = 2;
            if (uxattributes.Nodes[index].GetValue(1).ToString() == "Step")
            {
                oatt.submission_code = 10;
            }

            oatt.show_workflow = 0;
            if (uxattributes.Nodes[index].GetValue(5).ToString() == "Yes")
                oatt.show_workflow = 1;
            oatt.nullable = 1;
            if (uxattributes.Nodes[index].GetValue(6).ToString() == "Yes")
                oatt.nullable = 0; 
            
            oatt.is_def = false;
            oatt.def_sql = "";
            if (uxattributes.Nodes[index].GetValue(7).ToString() == "Yes")
            {
                oatt.is_def = true;
                oatt.def_sql = uxattributes.Nodes[index].GetValue(8).ToString(); 
            }
            return true;
              }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "set_attribute", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }

       private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
       {
           try
           {
               EventHandler<Eload_update_attributesArgs> handler = Eupdate_attributes;
               if (handler != null)
               {
                   Eload_update_attributesArgs args = new Eload_update_attributesArgs();
                   args.allattributes = new bc_om_all_attributes();
                   args.allattributes.write_mode = 1;
                   bc_om_attribute oatt= new bc_om_attribute();
                   oatt.attribute_id= (long)uxattributes.FocusedNode.GetValue(9);
                   args.allattributes.attributes.Add(oatt);

                   handler(this, args);
                   reload_attributes();
               }

           }
           catch (Exception ex)
           {
               bc_cs_security.certificate certificate = new bc_cs_security.certificate();
               bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "delete", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           }
       }
       private void uxattributes_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
       {

           try
           {
               if (_view_only == true)
               {
                   return;
               }
               barButtonItem5.Enabled = false;
               if (uxattributes.FocusedNode != null)
               {
                   barButtonItem5.Enabled = true;
                   uxattributes.Columns[3].OptionsColumn.AllowEdit = false;
                   uxattributes.Columns[8].OptionsColumn.AllowEdit = false;
                   if (uxattributes.FocusedNode.GetValue(1) != null)
                   {
                       if (uxattributes.FocusedNode.GetValue(1).ToString() == "Lookup")
                           uxattributes.Columns[3].OptionsColumn.AllowEdit = true;
                       if (uxattributes.FocusedNode.GetValue(7).ToString() == "Yes")
                           uxattributes.Columns[8].OptionsColumn.AllowEdit = true;
                   }

               }
           }
           catch (Exception ex)
           {
               bc_cs_security.certificate certificate = new bc_cs_security.certificate();
               bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attributes", "focused node changed", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
           }
       }


    }
}