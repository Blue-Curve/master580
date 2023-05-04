using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueCurve.Aggregations;
using BlueCurve.Core.AS;
namespace BlueCurve.Aggregations.AP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bc_am_load obcload = new bc_am_load("Authenticate Only");
            BlueCurve.Aggregations.AM.Cbc_am_aggs_preview bap = new BlueCurve.Aggregations.AM.Cbc_am_aggs_preview();
            bap.load_data();
        }
    }
}
