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
    public partial class bc_dx_cp_assign : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_assign
    {
        public event EventHandler<EsavelinksArgs> Esavelinks;


        List<bc_om_entity> _sel_ents = new List<bc_om_entity>();
        bc_om_entity _entity;
        long _schema_id;
        long _pref_type_id;
        long _class_id;
        bool _parent;
        long _max_number;
        EFIXEDENTITYCLASSES _area_id;
        //EASSIGNMODE _mode;
        public bc_dx_cp_assign()
        {
            InitializeComponent();
        }
        public Boolean load_view(EassignArgs data)
        {
            try
            {

                //_mode = mode;
                if (data.no_ordering== true)
                {
                    uxup.Visible = false;
                    uxdn.Visible = false;
                }

                this.Text = data.title;
                barButtonItem1.Enabled = false;
                _max_number = data.max_number;
                //if (data.max_number > 0)
                //    this.Text = data.title + " (" + data.max_number.ToString() + " selected)";
                //else if (data.max_number ==-1)
                //    this.Text = data.title + " (mandatory)";
                //else
                this.Text = data.title ;
                _sel_ents = data.sel_entities;
                _entity = data.entity;
                _schema_id = data.schema_id;
                _class_id = data.assign_class;
                _area_id = data.area_id;
                _parent = data.parent;
                _pref_type_id = data.pref_type_id;
                bc_dx_entity_search1.single_class_id = data.assign_class;
                bc_dx_entity_search1.hide_active_inactive = true;
                bc_dx_entity_search1.hide_class = true;
                bc_dx_entity_search1.hide_filter = true;
                bc_dx_entity_search1.sel_entities = _sel_ents;

                bc_dx_entity_search1.class_mode = data.area_id;
                
                bc_dx_entity_search1.load_data();
                  
                bc_dx_entity_search1.Eentitydblclick += Eentity_dblclick;
                uxsel.DoubleClick += Euxsel_dblclick;
                uxsel.SelectedIndexChanged += Euxsel_SelectedIndexChanged;
                uxup.Click += Euxup_Click;
                uxdn.Click += Euxdn_Click;


            load_sel_ents();
            return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_assign", "load_view", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }

        void load_sel_ents()
        {
            try
            { 
            int i;
            uxsel.Items.Clear();
            for (i=0; i < _sel_ents.Count; i++)
            {
                uxsel.Items.Add(_sel_ents[i].name);
            }
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_assign", "load_sel_ents", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            
            }
        }
        void Euxsel_dblclick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _sel_ents.RemoveAt(uxsel.SelectedIndex);
            load_sel_ents();
            bc_dx_entity_search1.load_entity_list(false, null, false, null, true);
            barButtonItem1.Enabled = true;
            Cursor = Cursors.Default;
        }
          
        void  Euxsel_SelectedIndexChanged(object sender, EventArgs e)
        {
            uxup.Enabled = false;
            uxdn.Enabled = false;
            if (uxsel.SelectedIndex > 0)
                uxup.Enabled = true;
            if (uxsel.SelectedIndex < uxsel.Items.Count-1)
                uxdn.Enabled = true;
        }

        void Eentity_dblclick(object sender,EloadentityArgs e)
        {
            Cursor=Cursors.WaitCursor;
            _sel_ents.Add(e.sentity);
            load_sel_ents();
            bc_dx_entity_search1.load_entity_list(false, null, false, null, true);
            barButtonItem1.Enabled = true;
            Cursor = Cursors.Default;
        }

        void Euxup_Click(object sender, EventArgs e)
        {
           
            bc_om_entity tent = new bc_om_entity();
            int i;
            i = uxsel.SelectedIndex;
            tent = _sel_ents[i];
            _sel_ents.RemoveAt(i);
            _sel_ents.Insert(i - 1, tent);
            load_sel_ents();
            uxsel.SelectedIndex = i - 1;
            barButtonItem1.Enabled = true;

        }
        void Euxdn_Click(object sender, EventArgs e)
        {
            bc_om_entity tent = new bc_om_entity();
            int i;
            i = uxsel.SelectedIndex;
            tent = _sel_ents[i];
            _sel_ents.RemoveAt(i);
            _sel_ents.Insert(i + 1, tent);
            load_sel_ents();
            uxsel.SelectedIndex = i + 1;
            barButtonItem1.Enabled = true;

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Hide();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                 bc_cs_message omsg;
            if (_max_number > 0 && _sel_ents.Count != _max_number)
            {
                if (_max_number==1)
                  omsg = new bc_cs_message("Blue Curve",  "1 item only must be selected!", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                else
                  omsg = new bc_cs_message("Blue Curve", _max_number.ToString() + " items must be selected!", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                return;
            }
            else if (_max_number ==-1 && _sel_ents.Count == 0)
            {
                 omsg = new bc_cs_message("Blue Curve", "At least 1 item must be selected!", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                return;
            }
            Cursor = Cursors.WaitCursor;

            EventHandler<EsavelinksArgs> handler = Esavelinks;
            if (handler != null)
            {

                EsavelinksArgs args = new EsavelinksArgs();
                args.mode=_area_id;
                switch (_area_id)
                {
                    case EFIXEDENTITYCLASSES.ENTITY:
                    case EFIXEDENTITYCLASSES.USER:
                    bc_om_cp_entity_links el = new bc_om_cp_entity_links();
                    el.entity_id=_entity.id;
                    el.submit_class_id = _class_id;
                    el.pref_type_id = _pref_type_id;  
                    el.schema_id=_schema_id;
                    el.linked_entities = _sel_ents;
                    el.parent = _parent;
                    args.entity_links = el;
                    break;

                  case EFIXEDENTITYCLASSES.BUS_AREA:
                  case EFIXEDENTITYCLASSES.ASSOC_USER:
                  case EFIXEDENTITYCLASSES.ROLE:
                    bc_om_cp_user_links ul = new bc_om_cp_user_links();
                    ul.user_id = _entity.id;
                    ul.area_entities = _sel_ents;
                    ul.pref_type_id = _pref_type_id;
                    ul.area_id = _area_id;
                    args.user_links = ul;
                  
                    break;
                    case EFIXEDENTITYCLASSES.CLASSIFY:
                    case EFIXEDENTITYCLASSES.CHANNEL:
                    case EFIXEDENTITYCLASSES.eMODULE:

                    bc_om_dx_pub_type_links pl = new bc_om_dx_pub_type_links();
                    pl.pub_type_id = _entity.id;
                    pl.link_type = _area_id;
                    pl.links= _sel_ents;
                    args.pt_links = pl;
                    break;
                    



                    default:
                    MessageBox.Show("Assign mode  not yet implemented: " + _area_id.ToString());
                    return;
                }


                handler(this, args);
                Hide();
              }
            }

            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_assign", "save", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return;
            }
            finally
            {
               Cursor = Cursors.Default;
            }
        }

        private void uxsel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uxup_EditValueChanged(object sender, EventArgs e)
        {

        }

      

      
    }
    public class Cbc_dx_cp_assign
    {
        Ibc_dx_cp_assign _view;
        public Boolean load_data(Ibc_dx_cp_assign view, EassignArgs data)
        {
            try
            {
                _view = view;
               _view.Esavelinks += save_links;
            
            return _view.load_view(data);
             }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_assign", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
        }
        void save_links(object sender, EsavelinksArgs e)
        {
            
            switch (e.mode)
            {
                case EFIXEDENTITYCLASSES.ENTITY:
                case EFIXEDENTITYCLASSES.USER:
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    e.entity_links.db_write();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    e.entity_links.tmode = bc_cs_soap_base_class.tWRITE;
                    object olinks = (object)e.entity_links;
                    if (e.entity_links.transmit_to_server_and_receive(ref olinks, true) == false)
                        return;

                }      
                break;

                case EFIXEDENTITYCLASSES.BUS_AREA:
                case EFIXEDENTITYCLASSES.ASSOC_USER:
                case EFIXEDENTITYCLASSES.ROLE:
                
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    e.user_links.db_write();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    e.user_links.tmode = bc_cs_soap_base_class.tWRITE;
                    object olinks = (object)e.user_links;
                    if (e.user_links.transmit_to_server_and_receive(ref olinks, true) == false)
                        return;
                }

                break;
                case EFIXEDENTITYCLASSES.CLASSIFY:
                case EFIXEDENTITYCLASSES.CHANNEL:
                case EFIXEDENTITYCLASSES.eMODULE:
                if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                    e.pt_links.db_write();
                else if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.SOAP)
                {
                    e.pt_links.tmode = bc_cs_soap_base_class.tWRITE;
                    object olinks = (object)e.pt_links;
                    if (e.pt_links.transmit_to_server_and_receive(ref olinks, true) == false)
                        return;
                }



                break;

                
                default:
                MessageBox.Show("Save mode not yet implemented: " + e.mode.ToString());
                
                
                break;
        }
            }
        }

    }
   public interface  Ibc_dx_cp_assign
   {
       Boolean load_view(EassignArgs data);
       event EventHandler<EsavelinksArgs> Esavelinks;
   }
   public class EsavelinksArgs : EventArgs
   {
       public bc_om_cp_entity_links entity_links { get; set; }
       public bc_om_cp_user_links user_links { get; set; }
       public bc_om_dx_pub_type_links pt_links { get; set; }
       public EFIXEDENTITYCLASSES mode { get; set; }
   }
   public class EassignArgs : EventArgs
   {
       public string title { get; set; }
       public bc_om_entity entity { get; set; }
       public long assign_class { get; set; }
       public List<bc_om_entity> sel_entities { get; set; }
       public long schema_id { get; set; }
       public bool parent { get; set; }
       public long pref_type_id { get; set; }
       public bool mandatory { get; set; }
       public long max_number { get; set; }
       public EFIXEDENTITYCLASSES area_id { get; set; }
       public bool no_ordering { get; set; }
   }
    //public enum EASSIGNMODE
    //{
    //    USER=0,
    //    ENTITY=1
    //}
