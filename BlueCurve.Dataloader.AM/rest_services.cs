using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net;
using System.Text;
using System.IO;
using BlueCurve.Core.CS;
namespace BlueCurve.Dataloader.AM
{
    public class bc_ca_dl_rest
    {
        string _uri;
        string _json_data;
        public string err_text;
        public string response_text;

        public  bc_ca_dl_rest(string uri, string json_data)
        {
            _uri = uri;
            _json_data = json_data;
        }
        public bool send()
        {
            try
            {
                //send = false;

                HttpWebRequest request;
                HttpWebResponse response;
                Uri address;
                Byte[] byteData;
                Stream postStream = null;
               
                address = new Uri(_uri);
                
                request = (HttpWebRequest)WebRequest.Create(address);
                
                request.Timeout = 60000;
               
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.SendChunked = true;
               
                // Set type to POST  
                request.Method = "POST";
                request.ContentType = "application/json";
                
                // Create a byte array of the data we want to send  
                byteData = UTF8Encoding.UTF8.GetBytes(_json_data.ToString());
               
                // Set the content length in the request headers  
                request.ContentLength = byteData.Length;
             
                // Write data  
                try
                {
                    postStream = request.GetRequestStream();
                    postStream.Write(byteData, 0, byteData.Length);
                }
                catch (Exception err)
                {
                    err_text = "json via rest post failed1: " + err.Message.ToString();
                    return false;
                }
                finally
                {
                    if (postStream != null)
                        postStream.Close();
                }

                try
                {
                    // Get response  
                    response = (HttpWebResponse)request.GetResponse();
                    // Get the response stream into a reader  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    response_text = reader.ReadToEnd();
                    if (response != null)
                        response.Close();
                    return true;

                }
                catch (Exception err)
                {
                    err_text = "json via rest post failed2: " + err.Message.ToString();
                    return false;
                }
            }
            catch (Exception err)
            {
                err_text = "json via rest post failed end: " + err.Message.ToString();
                return false;
            }
        }
    }
}
