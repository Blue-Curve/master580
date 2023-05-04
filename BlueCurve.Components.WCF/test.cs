using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using BlueCurve.Core.CS;
namespace bc_core_components_svc
{
    public class test
    {
        public string test_rest_post(string token, report_data report_data, string pdf_file)
        {
         try
            {
                StreamWriter sw = new StreamWriter("c:/TEST_JSON/test/test.txt");
                try
                {
                    

                    sw.WriteLine(token);
                    var JsonSerializer = new JavaScriptSerializer();
                    JsonSerializer.MaxJsonLength = Int32.MaxValue;
                    sw.WriteLine(JsonSerializer.Serialize(report_data));
                    bc_cs_file_transfer_services fs= new bc_cs_file_transfer_services();
                    bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                    Byte[] docbyte;
                    docbyte = Convert.FromBase64String(pdf_file);
                    fs.write_bytestream_to_document("c:/TEST_JSON/test/test.docx", docbyte, ref certificate);

                }
                catch
                {

                }
                finally {
                sw.Close();
                }
                return "success:" + token;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
    }
    public class report_data
    {
        string _title;
        string _subtitle;
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string subtitle
        {
            get { return _subtitle; }
            set { _subtitle = value; }
        }
    }
}