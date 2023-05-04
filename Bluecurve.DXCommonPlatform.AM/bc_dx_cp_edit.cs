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
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_cp_edit : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_edit
    {
        string _previous_value;

        List<bc_om_entity> _combo_vals;
        bool _combo;
        bool _allow_edit;
        public event EventHandler<Esaveeditargs> Esaveedit;
        public bc_dx_cp_edit()
        {
            InitializeComponent();
        }
        public bool load_view(string caption, string previous_value, string label, bool combo=false, List<bc_om_entity> combo_vals= null, bool allow_edit = false)
        {
            ceditcombo.TextChanged += ceditcombo_TextChanged;
            barButtonItem1.Enabled = false;
            Text = caption;
            lcaption.Text = label;
            _combo_vals=combo_vals;
            _combo = combo;
            _allow_edit = allow_edit;
            if (combo == false)
            {
                _previous_value = previous_value;
                uxedit.Text = previous_value;
            }
            else
            {

                uxedit.Visible=false;
                if (allow_edit== false)
                 cedit.Visible = true;
                else
                 ceditcombo.Visible = true;
               
                int i;
                for (i=0;  i < _combo_vals.Count;i++ )
                {
                    if (allow_edit == false)
                      cedit.Properties.Items.Add(_combo_vals[i].name);
                    else
                      ceditcombo.Properties.Items.Add(_combo_vals[i].name);
                }

            }
            return true;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Hide();
        }

        private void uxedit_EditValueChanged(object sender, EventArgs e)
        {
            barButtonItem1.Enabled = false;
            if ((_previous_value != uxedit.Text) && (uxedit.Text.Trim() != ""))
                barButtonItem1.Enabled = true;
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventHandler<Esaveeditargs> handler = Esaveedit;
            if (handler != null)
            {
                Esaveeditargs args = new Esaveeditargs();
                if (_combo == false)
                    args.text = uxedit.Text;
                else
                {
                    if (_allow_edit == false)
                    {
                        args.id = _combo_vals[cedit.SelectedIndex].id;
                        args.text = _combo_vals[cedit.SelectedIndex].name;
                    }
                    else
                    {
                        if (ceditcombo.SelectedIndex > -1)
                            args.id = _combo_vals[ceditcombo.SelectedIndex].id;
                        else
                            args.id = 0;
                        args.text = ceditcombo.Text;
                    }
                }
                handler(this, args);
                Hide();
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Hide();
        }

        private void cedit_SelectedIndexChanged(object sender, EventArgs e)
        {
            barButtonItem1.Enabled = false;
            if (cedit.SelectedIndex > -1)
                barButtonItem1.Enabled = true;
        }

        private void ceditcombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            barButtonItem1.Enabled = false;
            if (ceditcombo.SelectedIndex > -1 || ceditcombo.Text !="")
                barButtonItem1.Enabled = true;
        }
        private void ceditcombo_TextChanged(object sender, EventArgs e)
        {
            barButtonItem1.Enabled = false;
            if (ceditcombo.SelectedIndex > -1 || ceditcombo.Text != "")
                barButtonItem1.Enabled = true;
        }

    }

    public class Cbc_dx_cp_edit
    {
        Ibc_dx_cp_edit _view;
        public bool bsave = false;
        public string text;
        public long id;
        public bool load_data(Ibc_dx_cp_edit view, string caption, string previous_value, string label, bool combo = false, List<bc_om_entity> combo_vals = null, bool allow_edit = false)
        {
            _view = view;
            _view.Esaveedit += save;
            return _view.load_view(caption, previous_value,label, combo, combo_vals, allow_edit);
        }
        void save(object sender, Esaveeditargs e)
        {
            bsave = true;
            text = e.text;

            id = e.id;
        }
    }
    public interface Ibc_dx_cp_edit
    {
        bool load_view(string caption, string previous_value, string label, bool combo = false, List<bc_om_entity> combo_vals = null, bool allow_edit = false);
        event EventHandler<Esaveeditargs> Esaveedit;
    }
    public class Esaveeditargs : EventArgs
    {
        public string text { get; set; }
        public long id { get; set; }
    }
}