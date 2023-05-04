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

namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_progress : DevExpress.XtraEditors.XtraForm
    {
        public bc_dx_progress()
        {
            InitializeComponent();
            progressPanel1.Description= "BlueCurve v" + Application.ProductVersion;
             //DevExpress.Skins.SkinManager.EnableFormSkins();
            //DevExpress.UserSkins.BonusSkins.Register();
            //DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue");
        }

        private void progressPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}