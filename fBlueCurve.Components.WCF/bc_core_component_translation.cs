using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using BlueCurve.Core.CS;
namespace bc_core_component_translation
{
    // tranlates components to JSON from
    public class bc_core_component_translation
    {
        // translates a BC table to JSON
        public string translate_bc_table_to_JSON(Array res)
        {
            int i, j;
            bc_core_components_objects.bc_core_table otable;
            bc_core_components_objects.bc_core_row orow;
            bc_core_components_objects.bc_core_cell ocell;
            bc_core_components_objects.bc_core_cell_paragraph ocell_para;
            List<bc_core_components_objects.bc_core_cell> ocells = new List<bc_core_components_objects.bc_core_cell>();
            List<bc_core_components_objects.bc_core_row> orows = new List<bc_core_components_objects.bc_core_row>();

            Int32 mcol = 0, mrow = 0;

           

            for (j = 0; j <= res.GetUpperBound(1); j++)
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

            for (j = 0; j <= res.GetUpperBound(1); j++)
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
            List<bc_core_components_objects.bc_core_cell_paragraph> cps;
            for (i = 0; i < avals.GetUpperBound(0); i++)
            {
                orow = new bc_core_components_objects.bc_core_row();
                for (j = 0; j < avals.GetUpperBound(1); j++)
                {
                    cps = new List<bc_core_components_objects.bc_core_cell_paragraph>();
                    ocell_para = new bc_core_components_objects.bc_core_cell_paragraph(avals[i, j], astyle[i, j], false);
                    cps.Add(ocell_para);
                    ocell = new bc_core_components_objects.bc_core_cell(cps);
                    orow.add_cell(ocell);
                }
                otable.add_row(orow);
            }

            return new JavaScriptSerializer().Serialize(otable);
        }
        // translates a BC table to JSON
      
        public bc_core_components_objects.bc_core_table translate_adhoc_table_to_JSON(Array res , ref bc_cs_security.certificate certificate)
        {

            try
            {
                int i, j;
                bc_core_components_objects.bc_core_table otable;
                bc_core_components_objects.bc_core_row orow;
                bc_core_components_objects.bc_core_cell ocell;
                List<bc_core_components_objects.bc_core_cell> ocells = new List<bc_core_components_objects.bc_core_cell>();
                List<bc_core_components_objects.bc_core_row> orows = new List<bc_core_components_objects.bc_core_row>();

                Int32 mcol = 0, mrow = 0, para;
                bool image = false;


                for (j = 0; j <= res.GetUpperBound(1); j++)
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
                List<bc_core_components_objects.bc_core_cell_paragraph> cparas;
                bc_core_components_objects.bc_core_cell_paragraph cpara;
                bc_core_components_objects.bc_core_cell ccell;
                bc_core_components_objects.bc_core_cell[,] avals = new bc_core_components_objects.bc_core_cell[mrow + 1, mcol + 1];
                //string[,] astyle = new string[mrow + 1, mcol + 1];

                // translate data into correct order row, col

                for (i = 0; i <= res.GetUpperBound(1); i++)
                {
                    val = (String)res.GetValue( 0,i);
                    mrow = (int)res.GetValue(1,i);
                    mcol = (int)res.GetValue(2,i);
                    style = (String)res.GetValue(3,i);
                    para = (int)res.GetValue(4,i);
                    image = (bool)res.GetValue(5,i);
                    
                    cpara = new bc_core_components_objects.bc_core_cell_paragraph(val, style, image);
                    ccell = avals[mrow - 1, mcol - 1];
                    if (ccell == null)
                    {
                        cparas = new List<bc_core_components_objects.bc_core_cell_paragraph>();
                        cparas.Add(cpara);
                        ccell = new bc_core_components_objects.bc_core_cell(cparas);

                    }
                    else
                    {
                        ccell.paragraphs.Add(cpara);
                    }
                    avals.SetValue(ccell, mrow - 1, mcol - 1);
                }
                // put into object that can be json serialized
                otable = new bc_core_components_objects.bc_core_table();
                for (i = 0; i < avals.GetUpperBound(0); i++)
                {
                    orow = new bc_core_components_objects.bc_core_row();
                    for (j = 0; j < avals.GetUpperBound(1); j++)
                    {
                        if (avals[i, j] == null)
                        {
                            cparas = new List<bc_core_components_objects.bc_core_cell_paragraph>();
                             ocell = new bc_core_components_objects.bc_core_cell(cparas);
                        }
                        else
                        {
                            ocell = new bc_core_components_objects.bc_core_cell(avals[i, j].paragraphs);
                        }
                        orow.add_cell(ocell);
                    }
                    otable.add_row(orow);
                }
                return otable;
            }



            catch (Exception e)
            {
                certificate.error_state = true;
                certificate.server_errors.Add("bc_core_component_translation:translate_Adhoc_table_to_JSON: " + e.Message.ToString());
                return null;

            }

        }
    }
}