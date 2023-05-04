using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_cp_attribute : DevExpress.XtraEditors.XtraForm, Ibc_dx_cp_attribute
    {
        public event EventHandler<EsaveattributeArgs> Esave_attribute;
        public event EventHandler<EdeleteattributeArgs> Edelete_attribute;
        public event EventHandler ELoadAll_attributes;

        enum Eattribute_types
        {
            TEXT=1,
            NUMBER=2,
            BOOLEAN=3,
            DATE=5,
            LOOKUP=10,
            MEMO=11
        }

        List<bc_om_attribute> _attributes;

        public bc_dx_cp_attribute()
        {
            InitializeComponent();
        }
        void clear_errors()
        {
            gerrs.Clear();
            uxerrors.Items.Clear();
        }

        public Boolean load_view(List<bc_om_attribute> attributes)
        {
            repositoryItemComboBox1.SelectedIndexChanged += set_changed_combo;
            repositoryItemCheckEdit1.CheckedChanged += set_changed_check;
            repositoryItemCheckEdit2.CheckedChanged += set_changed_check;
            repositoryItemCheckEdit3.CheckedChanged += set_changed_check;
            repositoryItemCheckEdit4.CheckedChanged += set_changed_check;
            repositoryItemTextEdit1.EditValueChanged += set_changed_edit;
            repositoryItemTextEdit2.EditValueChanged += set_changed_edit;
            repositoryItemTextEdit3.EditValueChanged += set_changed_edit;
            repositoryItemTextEdit4.EditValueChanged += set_changed_edit;
            
            uxerrors.Items.Clear();
            
            ribbonPage1.Visible = true;
            ribbonPage2.Visible = false;

            try 
            {
                 load_attributes(attributes);
                    return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attribute", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
           
        }
      
        
        void set_changed_combo(object sender, EventArgs e)
        {
           DevExpress.XtraEditors.ComboBoxEdit s;
           s = (DevExpress.XtraEditors.ComboBoxEdit)sender;
           String ev;
           ev = s.EditValue.ToString();
           check_correct(ev);
        }
        void set_changed_edit(object sender, EventArgs e)
        {
           DevExpress.XtraEditors.TextEdit s;
           s = (DevExpress.XtraEditors.TextEdit)sender;
           String ev;
           ev = s.EditValue.ToString();
           check_correct(ev);
        }
        void set_changed_check(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit s;
            s = (DevExpress.XtraEditors.CheckEdit)sender;
            String ev;
            ev = s.EditValue.ToString();
            check_correct(ev);
        }
        class c_err_tx
        {
            public long attribute_id;
            public List<string> err_tx = new List<string>();
        }
        List<c_err_tx> gerrs = new List<c_err_tx>();
        void check_correct(string val)
        {
            try
            {
                
                ribbonPage1.Visible = false;
                ribbonPage2.Visible = true;
                long attribute_id;
                try { attribute_id = (long)uxatts.FocusedNode.GetValue(9); }
                catch { attribute_id = 0; }


                c_err_tx cerr = new c_err_tx();
                cerr.attribute_id = attribute_id;
                string attribute_name="";
                string attribute_type="";
                string lookup_sp;
                try { attribute_name = uxatts.FocusedNode.GetValue(0).ToString(); } catch { }
                try { attribute_type = uxatts.FocusedNode.GetValue(1).ToString(); }catch { }
                try { lookup_sp = uxatts.FocusedNode.GetValue(1).ToString(); } catch { }

                try
                {
                    if (uxatts.FocusedColumn.Caption == "Name")
                    {
                        attribute_name = val;
                    }
                    else if (uxatts.FocusedColumn.Caption == "Type")
                    {
                        attribute_type = val;
                    }
                }
                catch { }

                if (attribute_name == "")
                    cerr.err_tx.Add("name must be entered ");
                if (attribute_type == "")
                    cerr.err_tx.Add("type must be selected ");

                if (attribute_id == 0)
                    gerrs.Clear();
                else
                {
                    int i;
                    for(i=0; i < gerrs.Count;i++)
                    {
                        if (gerrs[i].attribute_id==attribute_id)
                        {
                            gerrs.RemoveAt(i);
                            break;
                        }
                    }

                }


                gerrs.Add(cerr);


                if (cerr.err_tx.Count > 0)
                {
                    uxatts.FocusedNode.StateImageIndex = 0;
                }
                else
                    uxatts.FocusedNode.StateImageIndex = 1;

               
               load_errors();
               
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attribute", "check_correct", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
        }
        bool gerror_loading = false;
        void load_errors()
        {
            
            gerror_loading = true;
            try
            { 
             uxerrors.Items.Clear();
             int i,j;
             for (i = 0; i < gerrs.Count; i++)
                 for (j = 0; j < gerrs[i].err_tx.Count; j++)
                 uxerrors.Items.Add(gerrs[i].err_tx[j]);
            
            if (uxerrors.Items.Count > 0)
               barButtonItem12.Enabled = false;  
            else
              barButtonItem12.Enabled = true;  

           }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attribute", "load_errors", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
            finally
            {
                gerror_loading = false;
            }
        }

        void load_attributes(List<bc_om_attribute> attributes)
        {

            try 
            {
                clear_errors();
                _attributes=attributes;
                int i;
                uxatts.BeginUpdate();
                uxatts.Nodes.Clear();
                for (i = 0; i < attributes.Count;i++ )
                {
                    uxatts.Nodes.Add();
                    uxatts.Nodes[i].SetValue(9, attributes[i].attribute_id );
                    uxatts.Nodes[i].SetValue(0, attributes[i].name);
                    if (attributes[i].repeats==1)
                    {
                        uxatts.Nodes[i].SetValue(1, "Memo");
                        uxatts.Nodes[i].SetValue(3, attributes[i].length);
                    }
                    else if (attributes[i].is_lookup == true)
                    {
                        uxatts.Nodes[i].SetValue(1, "Lookup");
                        uxatts.Nodes[i].SetValue(2, attributes[i].lookup_sql);
                    }
                    else
                    {
                    switch (attributes[i].type_id)
                    {
                        case (long)Eattribute_types.TEXT:
                            uxatts.Nodes[i].SetValue(1, "Text");
                            break;
                        case (long)Eattribute_types.NUMBER:
                            uxatts.Nodes[i].SetValue(1, "Number");
                            break;
                        case (long)Eattribute_types.BOOLEAN:
                            uxatts.Nodes[i].SetValue(1, "Boolean");
                            break;
                        case (long)Eattribute_types.DATE:
                            uxatts.Nodes[i].SetValue(1, "Date");
                            break;
                     }
                   }
                    uxatts.Nodes[i].SetValue(4,false);
                    uxatts.Nodes[i].SetValue(5, false);
                    uxatts.Nodes[i].SetValue(6, false);
                    uxatts.Nodes[i].SetValue(7, false);
                    if (attributes[i].submission_code == 2)
                        uxatts.Nodes[i].SetValue(4, true);
                    if (attributes[i].show_workflow == 1)
                        uxatts.Nodes[i].SetValue(5, true);
                    if (attributes[i].nullable == 0)
                        uxatts.Nodes[i].SetValue(6, true);
                    if (attributes[i].is_def == true)
                    {
                        uxatts.Nodes[i].SetValue(7, true);
                        uxatts.Nodes[i].SetValue(8, attributes[i].def_sql);
                    }
                    
                }
                    
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attribute", "load_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
               
            }
            finally
            {
                uxatts.Enabled = true;
                uxatts.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        //void value_changed(object sender, EventArgs e)
        //{
        //    uxatts.FocusedNode.StateImageIndex = 1;

        //    MessageBox.Show("changed");
        //}
       void save_attributes()
        {
           try
           { 
            int i;
            bc_om_all_attributes changed_attributes= new  bc_om_all_attributes();
            bc_om_attribute att;
            string type;
            for (i = 0; i < uxatts.Nodes.Count; i++)
            {
                if (uxatts.Nodes[i].StateImageIndex == 1)
                {
                    att = new bc_om_attribute();
                    att.attribute_id = (long)uxatts.Nodes[i].GetValue(9);
                    att.name = uxatts.Nodes[i].GetValue(0).ToString();
                    type = uxatts.Nodes[i].GetValue(1).ToString();
                    if (type == "Text")
                        att.type_id = (long)Eattribute_types.TEXT;
                    else if (type == "Number")
                        att.type_id = (long)Eattribute_types.NUMBER;
                    else if (type == "Boolean")
                        att.type_id = (long)Eattribute_types.BOOLEAN;
                    else if (type == "Date")
                        att.type_id = (long)Eattribute_types.DATE;
                    else if (type == "Lookup")
                    {
                        att.type_id = (long)Eattribute_types.TEXT;
                        att.is_lookup = true;
                        att.lookup_sql = uxatts.Nodes[i].GetValue(2).ToString();
                    }
                    else if (type == "Memo")
                    {
                        att.type_id = (long)Eattribute_types.TEXT;
                        att.repeats = 1;
                        att.length = (int)uxatts.Nodes[i].GetValue(3);
                    }
                    if ((Boolean)uxatts.Nodes[i].GetValue(4) == true)
                        att.submission_code = 2;
                    else
                        att.submission_code = 1;
                    if ((Boolean)uxatts.Nodes[i].GetValue(5) == true)
                        att.show_workflow = 1;
                    else
                        att.show_workflow = 0;
                    if ((Boolean)uxatts.Nodes[i].GetValue(6) == true)
                        att.nullable = 0;
                    else
                        att.nullable = 1;
                    if ((Boolean)uxatts.Nodes[i].GetValue(7) == true)
                    {
                        att.is_def = true;
                        att.def_sql = uxatts.Nodes[i].GetValue(8).ToString();
                    }
                    else
                        att.is_def = false;


                    changed_attributes.attributes.Add(att);
                    EventHandler<EsaveattributeArgs> handler = Esave_attribute;
                    if (handler != null)
                    {
                        EsaveattributeArgs args = new EsaveattributeArgs();
                        args.attributes = changed_attributes;
                        handler(this, args);
                    }

                }
              }
           
            }
               
           catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_attribute", "save_attributes", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
          
            }

        }
        //save
       private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
       {
           save_attributes();
       }
        //cancel
       private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
       {

           Cursor.Current = Cursors.WaitCursor;
           uxatts.Enabled = false;
           EventHandler handler = ELoadAll_attributes;
           if (handler != null)
           {
               handler(this, null);
           }
       }

       private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
       {
           clear_errors();
           uxatts.Nodes.Clear();
           uxatts.Nodes.Add();
           uxatts.Nodes[0].SetValue(9, 0);
           ribbonPage1.Visible = false;
           ribbonPage2.Visible = true;
           check_correct("");

       }

       private void uxerrors_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (uxerrors.SelectedIndex == -1)
               return;
           if (gerror_loading== true)
               return;
           int i,j;
           int co=0;
           long attribute_id = 0;
           for (i = 0; i < gerrs.Count; i++)
           {
             
               for (j = 0; j < gerrs[i].err_tx.Count; j++)
               {
                   if (co == uxerrors.SelectedIndex)
                   {
                       attribute_id = gerrs[i].attribute_id;
                    
                   }
                   co = co+1;
               }
           }
          
           for (i = 0; i < uxatts.Nodes.Count; i++)
           {
               if ((long)uxatts.Nodes[i].GetValue(9) == attribute_id)
               {
                   uxatts.Nodes[i].Selected = true;
               }
           }
       }
    }
    public class Cbc_dx_cp_attribute
    {
        Ibc_dx_cp_attribute _view;
        public Boolean load_data(Ibc_dx_cp_attribute view)
        {
            try
            {
                _view = view;
                _view.Esave_attribute += save_attribute;
                _view.Edelete_attribute += delete_attribute;
                _view.ELoadAll_attributes += load_all_attributes;

                return load_attributes();
               
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_attribute", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }
        void save_attribute(object sender, EsaveattributeArgs e)
        {


            load_attributes();
        }
        void delete_attribute(object sender, EdeleteattributeArgs e)
        {

        }
        void load_all_attributes(object sender, EventArgs e)
        {
            load_attributes();
        }
        Boolean load_attributes()
        {

            bc_om_all_attributes allattributes = new bc_om_all_attributes();

            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                allattributes.db_read();
            else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
            {
                allattributes.tmode = bc_cs_soap_base_class.tREAD;
                object oallattributes = (object)allattributes;
                if (allattributes.transmit_to_server_and_receive(ref oallattributes, true) == false)
                    return false;

                allattributes = (bc_om_all_attributes)oallattributes;
            }
            return _view.load_view(allattributes.attributes);
        }
    }
    public interface Ibc_dx_cp_attribute
    {
        Boolean load_view(List<bc_om_attribute> attributes);
        event EventHandler<EsaveattributeArgs> Esave_attribute;
        event EventHandler<EdeleteattributeArgs> Edelete_attribute;
        event EventHandler ELoadAll_attributes;

    }
    public class EsaveattributeArgs
    {
       public  bc_om_all_attributes attributes;
    }
    public class EdeleteattributeArgs
    {
        public long attribute_id;
    }
}