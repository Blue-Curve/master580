using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using BlueCurve.Core.OM;
using BlueCurve.Core.CS;
using bc_core_components_objects;

namespace nbc_core_document_composition
{
    public class bc_core_document_composition
    {
        bc_cs_security.certificate _certificate;
        long _doc_id;
        public bc_core_document_composition(long doc_id, ref bc_cs_security.certificate certificate)
        {
            _certificate = certificate;
            _doc_id = doc_id;
        }

        public string  json_get_document_composition()
        {
            try
            {
                object lores,pores;
                Array lres,pres;
                bc_core_document_composition_db db = new bc_core_document_composition_db();
                bc_core_document doc = new bc_core_document();
                doc.doc_id = _doc_id;
                bc_core_lead_paragraph lpara;
                bc_core_paragraph para;
                List<bc_core_lead_paragraph> llp= new List<bc_core_lead_paragraph>();
                List<bc_core_paragraph> lp;
                lores = db.get_lead_paragraphs(_doc_id, ref _certificate);
                lres = (Array)lores;
                int i,j;

                for (i = 0; i <= lres.GetUpperBound(1); i++)
                {

                    lpara = new bc_core_lead_paragraph();
                    lpara.text = lres.GetValue(1, i).ToString();
                    lpara.style = lres.GetValue(2, i).ToString();
                    lpara.page_number = (int)lres.GetValue(3, i);
                    lpara.is_image = (bool)lres.GetValue(4, i);
                    lpara.is_table = (bool)lres.GetValue(5, i);
                    pores = db.get_paragraphs((long)lres.GetValue(0, i), ref _certificate);
                    pres = (Array)pores;
                    lp = new List<bc_core_paragraph>();
                    for (j = 0; j <= pres.GetUpperBound(1); j++)
                    {
                        para = new bc_core_paragraph();
                        para.text = pres.GetValue(0, j).ToString();
                        para.style = pres.GetValue(1, j).ToString();
                        para.page_number = (int)pres.GetValue(2, j);
                        para.is_image = (bool)pres.GetValue(3, j);
                        para.is_table = (bool)pres.GetValue(4, j);
                        lp.Add(para);
                    }
                    lpara.paragraphs = lp;
                    llp.Add(lpara);
                }
                doc.lead_paragraphs=llp;

                return new JavaScriptSerializer().Serialize(doc); ;
            }
            catch (Exception e)
            {
                _certificate.error_state = true;
                _certificate.server_errors.Add("bc_core_document_composition:json_get_document_composition - " + _doc_id.ToString() + ": " + e.Message.ToString());
                return "";
            }
        }
        public bool  save_json_to_file_repos(string json)
        {
            string fn="";
            try
            {
                fn = bc_cs_central_settings.central_repos_path + "/json/" + _doc_id.ToString() + ".json";
                StreamWriter fs = new StreamWriter(fn);
                fs.WriteLine(json);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                _certificate.error_state = true;
                _certificate.server_errors.Add("bc_core_document_composition:save_json_to_file_repos - " + fn + ": " + e.Message.ToString());
                return false;
            }
        }
    }


    class bc_core_document_composition_db
    {
        bc_cs_db_services gdb = new bc_cs_db_services();
        public object get_lead_paragraphs(long Docid, ref bc_cs_security.certificate certificate)
        {
            string sql = "exec dbo.bc_core_sc_get_lead_paragraphs " + Docid.ToString();
            return gdb.executesql(sql, ref certificate);
        }
        public object get_paragraphs(long Componentid, ref bc_cs_security.certificate certificate)
        {
            string sql = "exec dbo.bc_core_sc_get_paragraphs " + Componentid.ToString();
            return gdb.executesql(sql, ref certificate);
        }
        
    }
}