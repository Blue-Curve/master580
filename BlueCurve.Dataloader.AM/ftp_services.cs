using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;
using WinSCP;
using BlueCurve.Core.CS;
namespace BlueCurve.Dataloader.AM
{
  

   public class bc_ca_dl_ftp
   {
        string _uri;
        String _username;
        String _password;
        String _dir;
        int _port;
        public String err_text;
        String response_text;

        public  bc_ca_dl_ftp(String uri, String username, String password, String dir, int port)
        {
            _uri = uri;
            _username = username;
            _password = password;
            _dir = dir;
            _port = port;
        }
      

        public bool send_file(String source_file ,String destination_file ,ref  bc_cs_security.certificate certificate) 
        {
            try
            {
      
                String _UploadPath;
                System.IO.FileInfo _FileInfo= new System.IO.FileInfo(source_file);

                if (_dir != "")
                    _UploadPath = _uri + "/" + _dir + "/" + destination_file;
                else
                    _UploadPath = _uri + "/" + destination_file;

                 FtpWebRequest clsRequest = (FtpWebRequest)WebRequest.Create(_UploadPath);
                 clsRequest.Method = WebRequestMethods.Ftp.UploadFile;

                clsRequest.Credentials = new System.Net.NetworkCredential(_username, _password);
                
                clsRequest.KeepAlive = false;
                clsRequest.Timeout = 60000;
              
                clsRequest.UseBinary = true;
                clsRequest.UsePassive = false;
                
                clsRequest.ContentLength = _FileInfo.Length;


                int buffLength = (int)_FileInfo.Length;
           
                byte[] buff= new byte[buffLength];

                System.IO.FileStream  _FileStream  = _FileInfo.OpenRead();


                System.IO.Stream _Stream = clsRequest.GetRequestStream();
              

                int contentLen = _FileStream.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    _Stream.Write(buff, 0, contentLen);
                    contentLen = _FileStream.Read(buff, 0, buffLength);

                }

                _Stream.Close();
                _Stream.Dispose();
                _FileStream.Close();
                _FileStream.Dispose();
                FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse();
                response_text = response.ToString();
                return true;
            }
                catch (Exception ex)
            {
                err_text = "ftp failed to send file " + source_file + ": " + destination_file + ": " + ex.Message.ToString();
                    return false;
                }
            }

        public bool receive_file(String source_file ,String destination_file , ref  bc_cs_security.certificate certificate, Boolean delete =false) 
        {
            String _DnloadPath="";
            err_text = "";
            try
            {
                 
                 if (_dir != "")
                     _DnloadPath = _uri + "/" + _dir + "/" + source_file;
                else
                     _DnloadPath = _uri + "/" + source_file;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_DnloadPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(_username, _password);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                StreamWriter writer = new StreamWriter(destination_file);
                writer.WriteLine(reader.ReadToEnd());
                
                writer.Close();
                writer.Dispose();
               
                
                reader.Close();
                reader.Dispose();
               
                
                if (delete== true)
                {
                    try
                    {
                        FtpWebRequest drequest = (FtpWebRequest)WebRequest.Create(_DnloadPath);
                        drequest.Credentials = new NetworkCredential(_username, _password);
                        drequest.Method = WebRequestMethods.Ftp.DeleteFile;
                        FtpWebResponse dresponse = (FtpWebResponse)drequest.GetResponse();
                        dresponse.Close();
                    }
                    catch (Exception ex)
                    {
                        err_text = "ftp failed to delete file from ftp " + _DnloadPath  + ex.Message.ToString();
                    }

                }
                response.Close();
                return true;
            }
                catch (Exception ex)
            {
                err_text = "ftp failed to receive file " + _DnloadPath + ": " + destination_file + ": " + ex.Message.ToString();
                    return false;
                }
            }


        public string get_top_file(String source_dir, ref bc_cs_security.certificate certificate)
        {
            String _DnloadPath = "";
            err_text = "";

            string filename = "";

           try
          {

             _DnloadPath = _uri + "/" + _dir;

            FtpWebRequest drequest = (FtpWebRequest)WebRequest.Create(_DnloadPath);
            drequest.Method = WebRequestMethods.Ftp.ListDirectory;
            drequest.Credentials = new NetworkCredential(_username, _password);
            
            FtpWebResponse dresponse = (FtpWebResponse)drequest.GetResponse();
            Stream responseStream = dresponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string directoryRaw = null;
            while (reader.Peek() != -1)
            {
                directoryRaw += reader.ReadLine() + "|";
            }
            string[] directoryList = directoryRaw.Split("|".ToCharArray());
            Array.Sort(directoryList);
            Array.Reverse(directoryList);

            filename = directoryList[0];  

            reader.Close();
            dresponse.Close();

            return filename;
         }
          
            catch (Exception ex)
         {
            err_text = "ftp failed to read directory " + _DnloadPath + ": " + ex.Message.ToString();
            return "Error";

         }

        }


      }

     }

  public class bc_ca_dl_sftp
   {
        string _uri;
        String _username;
        String _password;
        String _dir;
        int _port;
        public String err_text;
        String response_text;
        String _fingerprint;
        public string received_file_name;

        String _proxymethod;
        String _proxyhost;
        String _proxyport;
        String _proxyuser;
        String _proxypassword;

        public bc_ca_dl_sftp(String uri, String username, String password, String dir, int port, String fingerprint, String proxymethod, String proxyhost, String proxyport, String proxyuser, String proxypassword)
        {
            _uri = uri;
            _username = username;
            _password = password;
            _dir = dir;
            _port = port;
            _fingerprint=fingerprint;

            _proxymethod = proxymethod;
            _proxyhost = proxyhost;
            _proxyport = proxyport;
            _proxyuser = proxyuser;
            _proxypassword = proxypassword;
   
        }
      

        public bool send_file(string source_file, string  destination_file, ref bc_cs_security.certificate certificate) 
  {
  try
  {
                // Setup session options
               
                SessionOptions sessionOptions  = new SessionOptions();
                
                sessionOptions.Protocol = Protocol.Sftp;
                sessionOptions.HostName = _uri;
                sessionOptions.UserName = _username;
                sessionOptions.Password = _password;


                sessionOptions.SshHostKeyFingerprint = _fingerprint;
                sessionOptions.PortNumber = _port;
                
      
                // Proxy settings
                if (_proxymethod != "") 
                {
                    if (_proxymethod != "") sessionOptions.AddRawSettings("ProxyMethod", _proxymethod);
                    if (_proxyhost != "") sessionOptions.AddRawSettings("ProxyHost", _proxyhost);
                    if ( _proxyport != "") sessionOptions.AddRawSettings("ProxyPort", _proxyport);
                    if (_proxyuser != "") sessionOptions.AddRawSettings("ProxyUsername", _proxyuser);
                    if (_proxypassword != "") sessionOptions.AddRawSettings("ProxyPassword", _proxypassword);
                }

               
                    Session session = new Session();
                    //Connect
                    session.Open(sessionOptions);

                    //Upload files
                    //Dim transferOptions As New TransferOptions
                    TransferOptions transferOptions  = new  TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    transferOptions.ResumeSupport.State = TransferResumeSupportState.Off;

                    //Dim transferResult As TransferOperationResult
                    TransferOperationResult transferResult;
                    if (_dir == "")
                       // transferResult = session.PutFiles(source_file, "\" + destination_file, false, transferOptions);
                        transferResult = session.PutFiles(source_file, "/" + destination_file, false, transferOptions);
                    else
                        transferResult = session.PutFiles(source_file, _dir + destination_file, false, transferOptions);
                    

                    //Log Any errors
                    int i;
                    for (i=0; i<transferResult.Failures.Count;i++)
                    {
                       bc_cs_activity_log ocomm = new bc_cs_activity_log("bc_cs_ns_sftp", "send", bc_cs_activity_codes.COMMENTARY.ToString(), "sftp failure: " + transferResult.Failures[i].Message.ToString() ,  ref certificate);
                    }

                    // Throw on any error
                    transferResult.Check();
                    session.Close();

                   bc_cs_activity_log ocomms = new bc_cs_activity_log("bc_cs_ns_sftp", "send", bc_cs_activity_codes.COMMENTARY.ToString(), "sftp file sent: " + _dir + destination_file, ref certificate);

                return true;
             }

            catch (Exception ex)
            {
                 
                err_text = "sftp failed to send file " + source_file + ": " + destination_file + ": " + ex.Message.ToString();
                return false;
        }
        }

        public bool receive_file(String source_file ,String destination_file , ref  bc_cs_security.certificate certificate, Boolean delete =false) 
        {
          
            err_text = "";
            try
            {
                          

                 // Setup session options

                 SessionOptions sessionOptions = new SessionOptions();

                 sessionOptions.Protocol = Protocol.Sftp;
                 sessionOptions.HostName = _uri;
                 sessionOptions.UserName = _username;
                 sessionOptions.Password = _password;
                
                 sessionOptions.SshHostKeyFingerprint = _fingerprint;
                 sessionOptions.PortNumber = _port;

                 // Proxy settings
                 if (_proxymethod != "")
                 {
                     if (_proxymethod != "") sessionOptions.AddRawSettings("ProxyMethod", _proxymethod);
                     if (_proxyhost != "") sessionOptions.AddRawSettings("ProxyHost", _proxyhost);
                     if (_proxyport != "") sessionOptions.AddRawSettings("ProxyPort", _proxyport);
                     if (_proxyuser != "") sessionOptions.AddRawSettings("ProxyUsername", _proxyuser);
                     if (_proxypassword != "") sessionOptions.AddRawSettings("ProxyPassword", _proxypassword);
                 }

                 Session session = new Session();
                 //Connect
                 session.Open(sessionOptions);

                 //Upload files
                 //Dim transferOptions As New TransferOptions
                 TransferOptions transferOptions = new TransferOptions();
                 transferOptions.TransferMode = TransferMode.Binary;
                 transferOptions.ResumeSupport.State = TransferResumeSupportState.Off;
                                 
                 //Dim transferResult As TransferOperationResult
                 TransferOperationResult transferResult;
                 if (_dir == "")
                     transferResult = session.GetFiles(source_file, destination_file,delete, transferOptions);
                 else
                     transferResult = session.GetFiles(_dir + source_file, destination_file, delete, transferOptions);

                
                 //Log Any errors
                 int i;
                 for (i = 0; i < transferResult.Failures.Count; i++)
                 {
                     bc_cs_activity_log ocomm = new bc_cs_activity_log("bc_cs_ns_sftp", "receive", bc_cs_activity_codes.COMMENTARY.ToString(), "sftp failure: " + transferResult.Failures[i].Message.ToString(), ref certificate);
                 }

                 // Throw on any error
                 transferResult.Check();

                // Get the name of the file 
                 foreach (TransferEventArgs transfer in transferResult.Transfers)
                 {
                     received_file_name = transfer.FileName;
                 }

                
                 session.Close();

                 bc_cs_activity_log ocomms = new bc_cs_activity_log("bc_cs_ns_sftp", "receive", bc_cs_activity_codes.COMMENTARY.ToString(), "sftp file sent: " + _dir + destination_file, ref certificate);

                 return true;
                
                //if (delete== true)
                //{
                //    try
                //    {
                //        FtpWebRequest drequest = (FtpWebRequest)WebRequest.Create(_DnloadPath);
                //        drequest.Credentials = new NetworkCredential(_username, _password);
                //        drequest.Method = WebRequestMethods.Ftp.DeleteFile;
                //        FtpWebResponse dresponse = (FtpWebResponse)drequest.GetResponse();
                //        dresponse.Close();
                //    }
                //    catch (Exception ex)
                //    {
                //        err_text = "ftp failed to delete file from ftp " + _DnloadPath  + ex.Message.ToString();
                //    }

                //}
                //response.Close();
                //return true;
            }
                catch (Exception ex)
            {
                err_text = "sftp failed to receive file " + source_file + " from : " + destination_file + ": " + ex.Message.ToString();
                    return false;
                }
            }

      

        public string get_top_file(String source_dir, ref bc_cs_security.certificate certificate)
        {
           
            err_text = "";

            string filename = "";

            try
            {

                // Setup session options

                SessionOptions sessionOptions = new SessionOptions();

                sessionOptions.Protocol = Protocol.Sftp;
                sessionOptions.HostName = _uri;
                sessionOptions.UserName = _username;
                sessionOptions.Password = _password;
                sessionOptions.SshHostKeyFingerprint = _fingerprint;
                sessionOptions.PortNumber = _port;
                
                Session session = new Session();
                //Connect
                session.Open(sessionOptions);

                filename = session.ListDirectory(source_dir).Files.OrderByDescending(file => file.Name).First().Name;

                session.Close();
                
                return filename;

            }

            catch (Exception ex)
            {
                err_text = "sftp failed to read directory: " + ex.Message.ToString();
                return "Error";

            }

        }
            

        }
      

