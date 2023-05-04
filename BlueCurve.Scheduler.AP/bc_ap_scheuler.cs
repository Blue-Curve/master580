using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.IO;
using System.Runtime.Serialization;
using BlueCurve.Core.CS;
using BlueCurve.CommonPlatform.AM;
using Bluecurve.DXCommonPlatform.AM;

    public class bc_ap_scheuler
    {
        public bc_ap_scheuler()
        {

            //REM if copy running dont start another
            if (check_prev_instance() == false)
            {
                bc_ap_scheduler_start ap = new bc_ap_scheduler_start();
            }
            

        }
        bool check_prev_instance()
        {

            //check_prev_instance = False
            bc_cs_central_settings bcs = new bc_cs_central_settings(true);
            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.connection_method;
            if (bc_cs_central_settings.multiple_instance == true)
            {
                return false;
            }
            String aModuleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            String aProcName = System.IO.Path.GetFileNameWithoutExtension(aModuleName);

            //Dim aModuleName As String = Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName

            //Dim aProcName As String = System.IO.Path.GetFileNameWithoutExtension(aModuleName)

            if (System.Diagnostics.Process.GetProcessesByName(aProcName).Length > 1)
                return true;
            else
                return false;
            

        }

        public class bc_ap_scheduler_start
        {

            public bc_ap_scheduler_start()
            {
                try
                {
                    bc_dx_cp_start cs = new bc_dx_cp_start();
                    cs.load();
                    {
                        
                    }
                }
                catch (Exception ex)
                {
                    bc_cs_message msg = new bc_cs_message("Blue Curve Process", "System Failed To Load. Contact System Administrator!", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                }
                finally
                {

                }

            }

        }
    }
