using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public class Cbc_dx_cp_container
    {
        Ibc_dx_cp_container _view;
        public bool load_data(Ibc_dx_cp_container view,string user_name, string role)
        {
          _view=view;
          // get functional areas for user
         
          bc_om_dx_cp_apps_for_role ra = new bc_om_dx_cp_apps_for_role();

         
          ra.tmode = bc_cs_soap_base_class.tREAD;
          ra.user_mode = true;
          if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
              ra.db_read();
          else
          {
              ra.tmode = bc_cs_soap_base_class.tREAD;
              object ora = (object)ra;
              if (ra.transmit_to_server_and_receive(ref ora, true) == false)
                  return false;
              ra = (bc_om_dx_cp_apps_for_role)ora;
          }
          if (ra.apps.Count==0)
          {
              bc_cs_message omsg = new bc_cs_message("Blue Curve", "No Common Platform Applications Configured for Users Role", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
              return false;
          }
          else
              return _view.load_view(ra,user_name,role);
        }
    }
    public interface Ibc_dx_cp_container
    {
        bool load_view(bc_om_dx_cp_apps_for_role ar, string user_name, string role);
    }
}
