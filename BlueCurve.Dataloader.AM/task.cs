using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCurve.Core.CS;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.IO.Compression;

namespace BlueCurve.Dataloader.AM
{
    
    public class task
    {
        enum ECHANNEL_TYPE_ID
        {
            FTP=1,
            SFTP=2,
            REST_POST=3,
            FILEONLY=4,
            SP_ONLY=5
        }

        int _status;
        string _err_text="";
        bc_cs_security.certificate _certificate;
    public void run (int task_id,  ref bc_cs_security.certificate certificate, ref int status, ref string err_text )
    {
        _certificate=certificate;
        _status = 3;
        try
        {
            //StreamWriter sw = new StreamWriter("c:/D/" + task_id.ToString() + "_" + instance_id.ToString() + ".txt");
            //sw.WriteLine(task_id.ToString() + "_" + instance_id.ToString());
            //sw.WriteLine(DateTime.UtcNow.ToString() );
            //sw.Close();
            //_status = status;

            _status = 3;
            object res;
            Array ares;
            bc_cs_db_services gdb = new bc_cs_db_services();
            res=gdb.executesql("exec dbo.bc_core_dl_get_task_config " + task_id.ToString(), ref certificate);
            ares = (Array)res;
           
            if (ares.GetUpperBound(1) !=0)
            {
                _err_text = "cant retrieve task config";
                 return;
            }
            bool input;
            string io_control_file_sp="";
            string filename_sp;
            ECHANNEL_TYPE_ID channel_type_id;
            string dir;
            string uri;
            string ftp_dir;
            string username;
            string password;
            int port;
            string fingerprint;
            bool delete_from_ftp;
            bool archive;
            bool decompress = false;
            bool compress= false;

            string unzippedFile;
            
            string proxymethod = "";
            string proxyhost = "";
            string proxyport= "";
            string proxyuser= "";
            string proxypassword= "";

            input = (bool)ares.GetValue(0, 0);
            io_control_file_sp = ares.GetValue(1, 0).ToString();
            filename_sp = ares.GetValue(2, 0).ToString();
            channel_type_id = (ECHANNEL_TYPE_ID)ares.GetValue(3, 0);
            dir = ares.GetValue(4, 0).ToString();
            ftp_dir = ares.GetValue(5, 0).ToString();
            uri = ares.GetValue(6, 0).ToString();
            username = ares.GetValue(7, 0).ToString();
            password = ares.GetValue(8, 0).ToString();
            port = (int)ares.GetValue(9, 0);
            fingerprint = ares.GetValue(10, 0).ToString();
            delete_from_ftp = (bool)ares.GetValue(11, 0);
            archive = (bool)ares.GetValue(12, 0);
            decompress = (bool)ares.GetValue(13, 0);
            compress = (bool)ares.GetValue(14, 0);

            if (ares.GetUpperBound(0) > 15)
            {
                proxymethod = ares.GetValue(15, 0).ToString();
                proxyhost = ares.GetValue(16, 0).ToString();
                proxyport = ares.GetValue(17, 0).ToString();
                proxyuser = ares.GetValue(18, 0).ToString();
                proxypassword = ares.GetValue(19, 0).ToString();
            }


            if (input == false)
            {
                string control_file_content="";
                string filename = "";

                if (io_control_file_sp=="")
                {
                    _err_text = "no control file specified";
                    return;
                }
                
                res = gdb.executesql_return_error_no_tran("exec " + io_control_file_sp + " " + task_id.ToString(), ref certificate, ref  _err_text);
                ares = (Array)res;

                if (_err_text !="")
                {
                    _err_text = "failed to execute control file sp " + io_control_file_sp + ": " + _err_text;
                    return;
                }
             
                if (channel_type_id==ECHANNEL_TYPE_ID.SP_ONLY)
                {
                    ares = (Array)res;
                    if (ares.GetUpperBound(1) != 0)
                    {
                        _err_text = "no return code from response sp " + io_control_file_sp;
                        return;
                    }
                    int sp_response_code;
                    try
                    {
                        sp_response_code = (int)ares.GetValue(0, 0);
                        if (sp_response_code != 0)
                        {
                            _err_text = ares.GetValue(1, 0).ToString();
                            return;
                        }
                    }
                    catch
                    {
                        _err_text = "invalid response from resposnse sp: " + io_control_file_sp;
                        return;
                    }
                    _status = 2;
                    return;
                }

              
                if (ares.GetUpperBound(1) != 0)
                {
                    _err_text = "cant retrieve control file";
                    return;
                }
                control_file_content = ares.GetValue(0, 0).ToString();

                // if no filename specified do not save request and response files to disk used for REST POST where file save is not necessary

                if (filename_sp != "")
                {

                    res = gdb.executesql_return_error_no_tran("exec " + filename_sp + " " + task_id.ToString(), ref certificate, ref  _err_text);
                    if (_err_text != "")
                    {
                        _status = 3;
                        _err_text = "failed to execute filename sp " + filename_sp + ": " + _err_text;
                        return;
                    }
                    ares = (Array)res;

                    if (ares.GetUpperBound(1) != 0)
                    {
                        _err_text = "cant retrieve filename";
                        return;
                    }
                    filename = ares.GetValue(0, 0).ToString();
                }
                switch (channel_type_id)
                {
                    case ECHANNEL_TYPE_ID.FILEONLY:
                        if (write_file(control_file_content,dir + filename)==false)
                        {
                            return;
                        }
                        break;
                    case ECHANNEL_TYPE_ID.FTP:
                        if (write_file(control_file_content,dir + filename)==false)
                        {
                            return;
                        }
                        bc_ca_dl_ftp ftp = new bc_ca_dl_ftp(uri, username, password, ftp_dir, port);
                        if (ftp.send_file(dir + filename, filename, ref  certificate) == false)
                        {
                            _err_text = ftp.err_text;
                            return;
                        }

                        break;
                    case ECHANNEL_TYPE_ID.SFTP:
                        if (write_file(control_file_content, dir + filename) == false)
                        {
                            return;
                        }
                        bc_ca_dl_sftp sftp = new bc_ca_dl_sftp(uri, username, password, ftp_dir, port,fingerprint,proxymethod,proxyhost,proxyport,proxyuser,proxypassword);
                        if (sftp.send_file(dir + filename, filename, ref  certificate) == false)
                        {
                            _err_text = sftp.err_text;
                            return;
                        }

                        break;
                    case ECHANNEL_TYPE_ID.REST_POST:
                        if (filename_sp != "")
                        {
                            if (write_file(control_file_content, dir + "request_" + filename) == false)
                            {
                                return;
                            }
                        }
                        bc_ca_dl_rest rest = new bc_ca_dl_rest(uri, control_file_content);
                        if (rest.send() == false)
                        {
                            _err_text = rest.err_text;
                            return;
                        }

                        if (filename_sp != "")
                        {
                            if (write_file(rest.response_text, dir + "response_" + filename) == false)
                            {
                                return;
                            }
                        }
                       
                        // pass response into response sp
                        string response_sp;
                        response_sp = io_control_file_sp.Trim() + "_response";
                        //convert json to xml
                        string xml = "";
                        xml = convert_json_to_xml(rest.response_text);
                        if (xml == "")
                        {
                            _err_text = "failed to convert json to xml";
                        }
                        if (filename_sp != "")
                        {
                            if (write_file(xml, dir + "response_xml_" + filename) == false)
                            {
                                return;
                            }
                        }

                        bc_cs_string_services jstr = new bc_cs_string_services(rest.response_text);
                        bc_cs_string_services xstr = new bc_cs_string_services(xml);
                      
                        
                        res = gdb.executesql_return_error_no_tran("exec " + response_sp + " " + task_id.ToString() + ",N'" + jstr.delimit_apostrophies() + "',N'" + xstr.delimit_apostrophies() + "'", ref certificate, ref  _err_text);
                        if (_err_text != "")
                        {
                          _err_text = "failed to execute rest write down sp " + response_sp + ": " + _err_text;
                          return;
                       }
                       ares = (Array)res;
                       if (ares.GetUpperBound(1) != 0)
                       {
                           _err_text = "no return code from response sp " + response_sp;
                              return;
                       }
                       int sp_response_code;
                       try
                       {
                           sp_response_code = (int)ares.GetValue(0, 0);
                           if (sp_response_code !=0)
                           {
                              _err_text = ares.GetValue(1, 0).ToString();
                              return;
                           }
                       }
                       catch
                       {
                          _err_text = "invalid response from resposnse sp: " + response_sp;
                       }
                      
                       break;
                }
            }
            else
            {

                string filename = "";

                if (io_control_file_sp == "")
                {
                    _err_text = "no data write sp specified";
                    return;
                }

                if (filename_sp == "")
                {
                    _err_text = "no filename sp specified";
                    return;
                }

                res = gdb.executesql_return_error_no_tran("exec " + filename_sp + " " + task_id.ToString(), ref certificate, ref  _err_text);
                if (_err_text != "")
                {
                    _status = 3;
                    _err_text = "failed to execute filename sp " + filename_sp + ": " + _err_text;
                    return;
                }
                ares = (Array)res;

                if (ares.GetUpperBound(1) != 0)
                {
                    _err_text = "cant retrieve filename";
                    return;
                }
                filename = ares.GetValue(0, 0).ToString();
             

                
                switch (channel_type_id)
                {
                    case ECHANNEL_TYPE_ID.FILEONLY:
                        if (filename.IndexOf("*") > 0)
                        {
                            if (read_file_wildcard(io_control_file_sp, dir ,filename, task_id, ref certificate) == false)
                            {
                                return;
                            }

                        }
                        else if (read_file(io_control_file_sp, dir + filename, task_id, ref certificate) == false)
                        {

                            return;
                        }
                        break;
                    case ECHANNEL_TYPE_ID.FTP:
                        bc_ca_dl_ftp ftp = new bc_ca_dl_ftp(uri, username, password, ftp_dir, port);

                        //if (filename == "LoadTop")
                        //{
                        //    filename = ftp.get_top_file(ftp_dir, ref certificate);
                        //}
                        
                        if (ftp.receive_file(filename, dir + filename, ref certificate, delete_from_ftp) == false)
                        {
                            _err_text = ftp.err_text;
                            return;
                        }
                        if (archive == true)
                        {
                            if (archive_file(dir, filename) == false)
                            {
                                return;
                            }
                        }

                        if (read_file(io_control_file_sp, dir + filename, task_id, ref certificate) == false)
                        {

                            return;
                        }
                        break;
                    case ECHANNEL_TYPE_ID.SFTP:
                        bc_ca_dl_sftp sftp = new bc_ca_dl_sftp(uri, username, password, ftp_dir, port, fingerprint, proxymethod, proxyhost, proxyport, proxyuser, proxypassword);

                        //if (filename == "LoadTop")
                        //{
                        //    filename = sftp.get_top_file(ftp_dir, ref certificate);
                        //}


                        // If wildcard was used get file name received
                        if (filename.Contains("*"))
                        {
                            if (sftp.receive_file(filename, dir, ref certificate, delete_from_ftp) == false)
                            {
                                _err_text = sftp.err_text;
                                return;
                            }

                            filename = sftp.received_file_name;
                        }
                        else
                        {
                            if (sftp.receive_file(filename, dir + filename, ref certificate, delete_from_ftp) == false)
                            {
                                _err_text = sftp.err_text;
                                return;
                            }

                        }
                                                  
                        
                        if (archive == true)
                        {
                            if (archive_file(dir, filename) == false)
                            {
                                return;
                            }
                        }
                        
             
                        // Decompress if required
                        if (decompress == true)
                        {
                            unzippedFile = Decompress(dir + filename);
                            filename = unzippedFile;
                        }
                        

                        if (read_file(io_control_file_sp, dir + filename, task_id, ref certificate) == false)
                        {

                            return;
                        }
                       
                                  
                        break;
                }
                
            }


             _status = 2;
        }
        catch (Exception e)
        {
            bc_cs_error_log err = new bc_cs_error_log("task", "run", bc_cs_error_codes.USER_DEFINED, e.Message.ToString(), ref certificate);
            _err_text = "task: run" + e.Message.ToString();
        }
        finally
        {
            status = _status;
            err_text = _err_text;
        }
    }
        private bool write_file(string content , string filename) 
        {
           
            try
            {
                StreamWriter sw = new StreamWriter(filename);
                sw.WriteLine(content);
                sw.Close();
                sw.Dispose();
                return true;
            }
            catch (Exception e)
            {
                bc_cs_error_log err = new bc_cs_error_log("task", "run", bc_cs_error_codes.USER_DEFINED, e.Message.ToString(), ref _certificate);
                _err_text = "task: write_file " + e.Message.ToString() + filename;
                return false;
            }
        }
        private bool archive_file(string dir, string filename)
        {
            // make copy with time date stamp
            string afn = dir + "Archive_" + DateTime.UtcNow.Day.ToString() + "_" + DateTime.UtcNow.Month.ToString() + "_" + DateTime.UtcNow.Year.ToString() + "_" + DateTime.UtcNow.Hour.ToString() + "_" + DateTime.UtcNow.Minute.ToString() + "_" + DateTime.UtcNow.Second.ToString() + "_" + filename;
            try
            {
                bc_cs_file_transfer_services fs = new bc_cs_file_transfer_services();
                fs.file_copy(dir + filename,  afn);
                return true;
            }
            catch (Exception ex)
            {
                _err_text = " failed to archive file " + afn + " : " + ex.Message.ToString();
                return false;
            }
       }

        private bool read_file_wildcard(string write_down_sp, string dir ,string filename, int task_id, ref bc_cs_security.certificate certificate)
        {

            try
            {
                string[] mfilename;
                mfilename = Directory.GetFiles(
                  @dir,
                  filename, 
                  SearchOption.TopDirectoryOnly);
                if (mfilename.Length != 1)
                {
                    _err_text = "file not found with wildcard " + filename;
                    return false;
                }

                StreamReader sr = new StreamReader(mfilename[0]);
                string content;
                content = sr.ReadToEnd();
                sr.Close();
                // pass data into SP
                bc_cs_string_services jstr = new bc_cs_string_services(content);
                object res;
                Array ares;
                bc_cs_db_services gdb = new bc_cs_db_services();
                res = gdb.executesql_return_error_no_tran("exec " + write_down_sp + " " + task_id.ToString() + ",N'" + jstr.delimit_apostrophies() + "'", ref certificate, ref  _err_text);
                if (_err_text != "")
                {
                    _err_text = "failed to execute write down sp " + write_down_sp + ": " + _err_text;
                    return false;
                }
                ares = (Array)res;
                if (ares.GetUpperBound(1) != 0)
                {
                    _err_text = "no return code from response sp " + write_down_sp;
                    return false; ;
                }
                int sp_response_code;
                try
                {
                    sp_response_code = (int)ares.GetValue(0, 0);
                    if (sp_response_code != 0)
                    {
                        _err_text = ares.GetValue(1, 0).ToString();
                        return false; ;
                    }
                }
                catch
                {
                    _err_text = "invalid response from resposnse sp: " + write_down_sp;
                }
                return true;
            }
            catch (Exception e)
            {
                bc_cs_error_log err = new bc_cs_error_log("task", "run", bc_cs_error_codes.USER_DEFINED, e.Message.ToString(), ref _certificate);
                _err_text = "task: read_file" + e.Message.ToString();
                return false;
            }
        }
  

        private bool read_file(string write_down_sp, string filename, int task_id, ref bc_cs_security.certificate certificate)
        {

            try
            {
                

                StreamReader sr = new StreamReader(filename);
                string content;
                content=sr.ReadToEnd();
                sr.Close();
                // pass data into SP
                bc_cs_string_services jstr = new bc_cs_string_services(content);
                object res;
                Array ares;
                bc_cs_db_services gdb = new bc_cs_db_services();
                res = gdb.executesql_return_error_no_tran("exec " + write_down_sp + " " + task_id.ToString() + ",N'" + jstr.delimit_apostrophies() + "'", ref certificate, ref  _err_text);
                if (_err_text != "")
                {
                    _err_text = "failed to execute write down sp " + write_down_sp + ": " + _err_text;
                    return false;
                }
                ares = (Array)res;
                if (ares.GetUpperBound(1) != 0)
                {
                    _err_text = "no return code from response sp " + write_down_sp;
                    return false; ;
                }
                int sp_response_code;
                try
                {
                    sp_response_code = (int)ares.GetValue(0, 0);
                    if (sp_response_code != 0)
                    {
                        _err_text = ares.GetValue(1, 0).ToString();
                        return false; ;
                    }
                }
                catch
                {
                    _err_text = "invalid response from resposnse sp: " + write_down_sp;
                }
                return true;
            }
            catch (Exception e)
            {
                bc_cs_error_log err = new bc_cs_error_log("task", "run", bc_cs_error_codes.USER_DEFINED, e.Message.ToString(), ref _certificate);
                _err_text = "task: read_file" + e.Message.ToString();
                return false;
            }
        }
        string convert_json_to_xml (string json)
        {
             XDocument xml;
                   try
                   {
                       using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                       {
                        
                           var quotas = new XmlDictionaryReaderQuotas();
                           var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                           xml = XDocument.Load(jsonReader);
                           return xml.ToString();
                       }
                   }
                   catch (Exception e)
                   {
                     return "";
                   }
        }

              
        public static string Decompress(string zipFile)
        {
            
            FileInfo fileToDecompress = new FileInfo(zipFile);

            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
            string currentFileName = fileToDecompress.FullName;
            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

            using (FileStream decompressedFileStream = File.Create(newFileName))
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedFileStream);
                }
            }

            string returnname = fileToDecompress.Name.Remove(fileToDecompress.Name.Length - fileToDecompress.Extension.Length);
                           
            return returnname;
        }
    }



    }
}
