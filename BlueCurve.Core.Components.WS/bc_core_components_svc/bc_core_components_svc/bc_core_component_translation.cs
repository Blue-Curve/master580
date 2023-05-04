using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
namespace bc_core_component_translation
{
    // tranlates components to JSON from
    public class bc_core_component_translation
    {
        // translates a BC table to JSON
        public string translate_table_to_JSON(Array res)
        {
            int i, j;
            bc_core_components_objects.bc_core_table otable;
            bc_core_components_objects.bc_core_row orow;
            bc_core_components_objects.bc_core_cell ocell;
            List<bc_core_components_objects.bc_core_cell> ocells = new List<bc_core_components_objects.bc_core_cell>();
            List<bc_core_components_objects.bc_core_row> orows = new List<bc_core_components_objects.bc_core_row>();

            Int32 mcol = 0, mrow = 0;

            for (j = 0; j < res.GetUpperBound(1); j++)
            {
                for (i = 0; i < res.GetUpperBound(0); i++)
                {
                    if ((i == 1) && ((int)res.GetValue(i, j) > mrow))
                        mrow = (int)res.GetValue(i, j);
                    else if ((i == 2) && ((int)res.GetValue(i, j) > mcol))
                        mcol = (int)res.GetValue(i, j);
                }
            }
            string val = "", style = "";
            string[,] avals = new string[mrow + 1, mcol + 1];
            string[,] astyle = new string[mrow + 1, mcol + 1];

            // translate data into correct order row, col

            for (j = 0; j < res.GetUpperBound(1); j++)
            {
                for (i = 0; i < res.GetUpperBound(0); i++)
                {
                    if (i == 1)
                        mrow = (int)res.GetValue(i, j);
                    else if (i == 2)
                        mcol = (int)res.GetValue(i, j);
                    else if (i == 0)
                        val = (String)res.GetValue(i, j);
                    else if (i == 3)
                        style = (String)res.GetValue(i, j);
                }
                avals.SetValue(val, mrow - 1, mcol - 1);
                astyle.SetValue(style, mrow - 1, mcol - 1);
            }
            // put into object that can be json serialized
            otable = new bc_core_components_objects.bc_core_table();
            for (i = 0; i < avals.GetUpperBound(0); i++)
            {
                orow = new bc_core_components_objects.bc_core_row();
                for (j = 0; j < avals.GetUpperBound(1); j++)
                {
                    ocell = new bc_core_components_objects.bc_core_cell(avals[i, j], astyle[i, j]);
                    orow.add_cell(ocell);
                }
                otable.add_row(orow);
            }

            return new JavaScriptSerializer().Serialize(otable);
        }
    }
}