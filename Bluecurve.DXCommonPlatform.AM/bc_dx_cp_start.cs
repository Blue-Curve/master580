using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using BlueCurve.Core.AS; 

namespace Bluecurve.DXCommonPlatform.AM
{
    public class bc_dx_cp_start
    {
        public void load()
        {
            bc_dx_progress fp;
            fp = new bc_dx_progress();
            
           
            try
            {
               
                fp.Show();

                bc_dx_logon fl = new bc_dx_logon();
                Cbc_dx_full_logon flo = new Cbc_dx_full_logon();
                flo.connect(fl);

               
                if (flo.bshow_form ==true)
                { 
                        fl.ShowDialog();
                }
                    
                if (flo.success == false)
                {
                    return;
                }

                MessageBox.Show(flo.user_name.ToString());

                bc_dx_cp_container fc = new bc_dx_cp_container();
                Cbc_dx_cp_container cc = new Cbc_dx_cp_container();
                if (cc.load_data(fc, flo.user_name, flo.role_name) == true)
                {
                    fp.Close();
                    fc.ShowDialog();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                fp.Close();
            }
        }
    }
}
