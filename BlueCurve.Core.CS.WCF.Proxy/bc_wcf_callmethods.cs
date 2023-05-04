using System;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.CodeDom;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ServiceModel.Discovery;
using System.CodeDom.Compiler;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading;
using BlueCurve.Core.CS;
using System.Xml.Serialization;
namespace BlueCurve.Core.CS.WCF.Proxy
{
    
  

    public class bc_wcf_methods
    {


        bc_wcf_callmethod client = new bc_wcf_callmethod();
               
    
        public string ErrorText { get; set; }

        public object call_method(long callmethodid, string docid, ref BlueCurve.Core.CS.bc_cs_security.certificate certificate, long entity_id = 0, long pub_type_id = 0, long lang_id = 0, long stage_id = 0, long contributtor_id = 0, long accounting_id = 0, string data_at_date = null)
        {
           object Results = new object();
           Boolean blnConnectionOK  = new Boolean();
           List<Object> Args = new List<object>();
           string paramvalue;
           string procname;
          
           dynamic dyparamvalue;
           long ubound;

           bc_cs_central_settings objCentralSettings = new bc_cs_central_settings(true);
           // bc_cs_security.certificate certificate = new bc_cs_security.certificate();

           bc_cs_activity_log otrace = new bc_cs_activity_log("BlueCurve.Core.WCF.AM", "call_method", bc_cs_activity_codes.TRACE_ENTRY.ToString(), "", ref certificate);

           try
            {
               
               // load method details from database
               bc_wcf_methoddetails WCFmethods = new bc_wcf_methoddetails();

               //Get Data
               blnConnectionOK = objCentralSettings.check_connection(bc_cs_central_settings.connection_method, true);

               //Error bad database
               if (blnConnectionOK == false) 
               {
                   ErrorText = "BlueCurve.Core.WCF.AM failed to connect to a database. Please check the config file";
                   Results = "";
                   return Results;
               }
               

               //load all methods
               if (WCFmethods.methoddelist.Count == 0)
               {
                   if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                   {
                       WCFmethods.db_read();
                   }
               }

               //find the correct method
               bc_wcf_Params ServiceInfo = new bc_wcf_Params();
               foreach (bc_wcf_method sMethod in WCFmethods.methoddelist)
               {
                   if (sMethod.methodid == callmethodid)
                   {
                       //Set up the Params
                       ServiceInfo.MethodName = sMethod.MethodName;
                       ServiceInfo.ContractName = sMethod.ContractName;
                       ServiceInfo.WSDLUri = new Uri(sMethod.WSDLUri);
                       ServiceInfo.ServiceUri = new Uri(sMethod.ServiceUri);

                       //Load the arguments
                       bc_wcf_methodparams WCFparames = new bc_wcf_methodparams();
                       if (WCFparames.paramlist.Count == 0)
                       {
                           if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                           {
                               WCFparames.db_read(callmethodid);
                           }
                       }

                       foreach (bc_wcf_param sparam in WCFparames.paramlist)
                       {

                           //Int[] from config 
                           if (sparam.paramvaluetype == "int[]" && sparam.paramtype == "V")
                           {
                               paramvalue = sparam.paramvalue;
                               dyparamvalue = new int[10];
                               dyparamvalue = paramvalue.Split(',').Select(x => int.Parse(x)).ToArray();

                               Args.Add(dyparamvalue);
                           }

                           //Int[] from stored proc
                           if (sparam.paramvaluetype == "int[]" && sparam.paramtype == "S")
                           {

                               //Run the proc to get value
                               paramvalue = "";
                               dyparamvalue = new int[10];
                               procname = sparam.paramproc;

                               ubound = 0;
                               if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                               {
                                   dyparamvalue = WCFparames.run_procedure(procname, docid);
                                   ubound = dyparamvalue.GetUpperBound(1);
                               }

                               int[] newArray = new int[ubound + 1];
                               for (int i = 0; i <= ubound; i++)
                               {
                                   newArray[i] = dyparamvalue[0, i];
                               }

                               Args.Add(newArray);
                           }

                           //long[] from stored proc
                           if (sparam.paramvaluetype == "long[]" && sparam.paramtype == "S")
                           {

                               //Run the proc to get value
                               paramvalue = "";
                               dyparamvalue = new long[10];
                               procname = sparam.paramproc;

                               ubound = 0;
                               if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                               {
                                   dyparamvalue = WCFparames.run_procedure(procname, docid);
                                   ubound = dyparamvalue.GetUpperBound(1);
                               }

                               long[] newArray = new long[ubound + 1];
                               for (int i = 0; i <= ubound; i++)
                               {
                                   newArray[i] = dyparamvalue[0, i];
                               }

                               Args.Add(newArray);
                           }

                           //string[] from stored proc
                           if (sparam.paramvaluetype == "string[]" && sparam.paramtype == "S")
                           {

                               //Run the proc to get value
                               paramvalue = "";
                               dyparamvalue = new string[10];
                               procname = sparam.paramproc;

                               ubound = 0;
                               if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                               {
                                   dyparamvalue = WCFparames.run_procedure(procname, docid);
                                   ubound = dyparamvalue.GetUpperBound(1);
                               }

                               string[] newArray = new string[ubound + 1];
                               for (int i = 0; i <= ubound; i++)
                               {
                                   newArray[i] = dyparamvalue[0, i];
                               }

                               Args.Add(newArray);
                           }

                           
                           //Int from stored proc
                           if (sparam.paramvaluetype == "int" && sparam.paramtype == "S")
                           {
                               //Run the proc to get value
                               paramvalue = "";
                               dyparamvalue = new int();
                               procname = sparam.paramproc;

                               if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                               {
                                   dyparamvalue = WCFparames.run_procedure(procname, docid);
                               }

                               Args.Add(dyparamvalue[0, 0]);
                           }


                           //String from stored proc
                           if (sparam.paramvaluetype == "string" && sparam.paramtype == "S")
                           {
                               //Run the proc to get value
                               paramvalue = "";
                               dyparamvalue = "";
                               procname = sparam.paramproc;

                               if (bc_cs_central_settings.connection_method == bc_cs_central_settings.ADO)
                               {
                                   dyparamvalue = WCFparames.run_procedure(procname, docid);
                               }

                               Args.Add(dyparamvalue[0, 0]);
                           }

                           //String from stored proc
                           if (string.IsNullOrEmpty(sparam.paramvaluetype) == false && sparam.paramtype == "H")
                           {
                               //Run the proc to get value
                               paramvalue = "";
                               dyparamvalue = new long();
                               procname = sparam.paramproc;

                               switch (sparam.paramvaluetype)
                               {
                                   case "entity_id":
                                       dyparamvalue = new long();
                                       dyparamvalue = entity_id;
                                       break;
                                   case "pub_type_id":
                                       dyparamvalue = new long();
                                       dyparamvalue = pub_type_id;
                                       break;
                                   case "lang_id":
                                       dyparamvalue = new long();
                                       dyparamvalue = lang_id;
                                       break;
                                   case "stage_id":
                                       dyparamvalue = new long();
                                       dyparamvalue = stage_id;
                                       break;
                                   case "contributtor_id":
                                       dyparamvalue = new long();
                                       dyparamvalue = contributtor_id;
                                       break;
                                   case "accounting_id":
                                       dyparamvalue = new long();
                                       dyparamvalue = accounting_id;
                                       break;
                                   case "data_at_date":
                                       dyparamvalue = "";
                                       dyparamvalue = data_at_date;
                                       break;
                               }
                               Args.Add(dyparamvalue);

                           }
                       }

                       ServiceInfo.Args = Args;
                   }
               }

               // Error methoid not found
               if (ServiceInfo.MethodName  == "")
               {
                   ErrorText = "BlueCurve.Core.WCF.AM Supplied method id '" + callmethodid.ToString() + "' not found in database";
                   Results = "";
                   return Results;
               }
               

               //Run the method
               bc_cs_activity_log otracex = new bc_cs_activity_log("CALLMETHOD", "A", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);
               
               Results = client.Call(ServiceInfo, ref certificate);
                otracex = new bc_cs_activity_log("CALLMETHOD", "A", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);

               if (string.IsNullOrEmpty(client.MethodErrorText) == false)
               {
                   otracex = new bc_cs_activity_log("CALLMETHOD", "C", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);

                   bc_cs_error_log db_err = new bc_cs_error_log("BlueCurve.Core.WCF.AM", "call_method", bc_cs_error_codes.USER_DEFINED.ToString(), client.MethodErrorText, ref certificate);
                   otracex = new bc_cs_activity_log("CALLMETHOD", "D", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);

                   ErrorText = client.MethodErrorText;
                   otracex = new bc_cs_activity_log("CALLMETHOD", "E", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);

               }
               otracex = new bc_cs_activity_log("CALLMETHOD", "F", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);
              // serialize to xml string

               XmlSerializer xmlSerializer = new XmlSerializer(Results.GetType());
               string sresults="";
               using (StringWriter textWriter = new StringWriter())
               {
                   xmlSerializer.Serialize(textWriter, Results);
                   sresults=textWriter.ToString();
               }


               return sresults;
               
           }

           catch (Exception e)
           {
               bc_cs_error_log db_err = new bc_cs_error_log("bc_wcf_methods", "call_method", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);

               ErrorText = "bc_wcf_methods call_method "  + e.Message;
               Results = "";
               return Results;
           }

           finally
           {
               otrace = new bc_cs_activity_log("BlueCurve.Core.WCF.AM", "call_method", bc_cs_activity_codes.TRACE_EXIT.ToString(), "", ref certificate);
           } 

        }

     }
    
    


   class bc_wcf_Params
    {
        public Uri WSDLUri { get; set; }
        public Uri ServiceUri { get; set; }
        public string ContractName { get; set; }
        public string MethodName { get; set; }
        public List<Object> Args { get; set; }
    }


    class bc_wcf_callmethod 
    {

        public string MethodErrorText { get; set; }
        public bc_compiled_proxies Proxies = new bc_compiled_proxies();
        
        public object Call(bc_wcf_Params svc, ref BlueCurve.Core.CS.bc_cs_security.certificate certificate)  
        {

            List<object> ParamsList = svc.Args;
            object Results = new object();
            Boolean proxyfound = false;
            bc_cs_activity_log otracex = new bc_cs_activity_log("bc_wcf_callmethod ", "Call", bc_cs_activity_codes.TRACE_ENTRY.ToString(), "", ref certificate);


            try
            {

                //Import WSDL
                WsdlImporter imptr = ImportWSDL(svc.WSDLUri);
                if (string.IsNullOrEmpty(this.MethodErrorText) != true)
                {
                    return Results;
                }
                
                //Extract Service and Data Contract Descriptions
                Collection<ContractDescription> svcCtrDesc = imptr.ImportAllContracts();
               

                //Compile the description to assembly
                var assembly = GetAssembly(svcCtrDesc);
                if (assembly == null) return Results;

                //Extract all end points available on the WSDL
                IDictionary<string, IEnumerable<ServiceEndpoint>> allEP = GetEndPointsOfEachServiceContract(imptr, svcCtrDesc);
                if (allEP.Count == 0)
                {
                    MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod Call: " + "No end points where found. Please check the wsdl is correct";
                    return Results;
                }

                
                IEnumerable<ServiceEndpoint> currentSvcEP;
                if (allEP.TryGetValue(svc.ContractName, out currentSvcEP))
                {

                    proxyfound = false;
                    var proxy = new object();                 
                    //Find if the proxy is in the Proxies list and use if it is
                    foreach (bc_wcf_proxy sproxy in Proxies.proxylist)
                    {
                        if (sproxy.ContractName == svc.ContractName)
                        {
                            proxyfound = true;
                            proxy = sproxy.proxy;
                        }
                    }
                    
                    //If Proxy not already present build it.
                    if (proxyfound == false)
                    {
                    
                       try
                         {
                            //Find the endpoint of the service to which the proxy needs to contact
                             var svcEPTest = currentSvcEP.First(x => x.ListenUri.AbsoluteUri == svc.ServiceUri.AbsoluteUri);
                         }
                      catch
                        {
                          MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod Call: " + "Url " + svc.ServiceUri.AbsoluteUri + " was not found. Please check the url is correct";
                           return Results;
                        }
                        
                        var svcEP = currentSvcEP.First(x => x.ListenUri.AbsoluteUri == svc.ServiceUri.AbsoluteUri);

                        //Generate proxy
                        proxy = GetProxy(svc.ContractName, svcEP, assembly);
                    
                        //Add proxy to proxy ist
                        bc_wcf_proxy proxyitem = new bc_wcf_proxy();
                        proxyitem.proxy = proxy;
                        proxyitem.ContractName = svc.ContractName;
                        Proxies.proxylist.Add(proxyitem);
                    }


                    bc_cs_activity_log otrace = new bc_cs_activity_log("Call", "1 Deserialize each argument to object", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);

                    //Deserialize each argument to object
                    List<object> pls = new List<object>();
                    foreach (var pl in ParamsList)
                    {
                        object clrObj = null;
                        try
                        {
                            clrObj = Deserialize(pl.ToString(), assembly);
                        }
                        catch
                        {
                            clrObj = pl;
                        }
                        pls.Add(clrObj);
                    }

                    otrace = new bc_cs_activity_log("Call", "2 test for method", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);


                    //Find opration contract on the proxy and invoke the required method
                    var mtype = proxy.GetType().GetMethod(svc.MethodName);
                    if (mtype == null)
                    {
                       MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod Call: " + "Method " + svc.MethodName + " was not found. Please check the method name is correct";
                       return Results;
                    }

                    otrace = new bc_cs_activity_log("CALL", "3 make the call", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);


                    Results = proxy.GetType().GetMethod(svc.MethodName).Invoke(proxy, pls.ToArray());

                    otrace = new bc_cs_activity_log("Call", "4 results got", bc_cs_activity_codes.COMMENTARY.ToString(), "", ref certificate);
                   
                
                }
                else
                {
                    MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod Call: " + "Contract " + svc.ContractName+ " was not found in the WSDL. Please check the contract name is correct";
                }
                

                return Results;

            }

            catch (Exception e)
            {
                bc_cs_error_log db_err = new bc_cs_error_log("BlueCurve.Core.WCF.AM", "call", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);
         
                MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod Call " + e.Message;
                Results = "";
                return Results;
            }
            finally
            {
                otracex = new bc_cs_activity_log("bc_wcf_callmethod ", "Call", bc_cs_activity_codes.TRACE_EXIT.ToString(), "", ref certificate);

            }
        }

        private Assembly GetAssembly(Collection<ContractDescription> svcCtrDesc)
        {

            try
            {
                CodeCompileUnit ccu = GetServiceAndDataContractCompileUnitFromWSDL(svcCtrDesc);
                CompilerResults rslt = GenerateContractsAssemblyInMemory(new CodeCompileUnit[] { ccu });
                if (!rslt.Errors.HasErrors)
                    return rslt.CompiledAssembly;
                MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod GetAssembly failed to get assembly";
                return null;
            }

            catch (Exception e)
            {
                MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod GetAssembly " + e.Message;
                return null;
            }
         }

        private object GetProxy(string ctrName, ServiceEndpoint svcEP, Assembly assembly)
        {
           
          try
          {

             //Set binding settings 
             TimeSpan bctimespan = new TimeSpan(0, 0, 20, 0);

             BasicHttpBinding binding = new BasicHttpBinding();
             binding.Name = "bcbinding";
             binding.MaxReceivedMessageSize = 2147483647;
             binding.MaxBufferSize = 2147483647;
             binding.ReceiveTimeout = bctimespan;
             binding.CloseTimeout = bctimespan;
             binding.SendTimeout = bctimespan;
             binding.OpenTimeout = bctimespan;
             binding.ReaderQuotas.MaxArrayLength = 2147483647;
             binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
             binding.ReaderQuotas.MaxStringContentLength = 2147483647;
             binding.ReaderQuotas.MaxDepth = 2147483647;
             binding.ReaderQuotas.MaxBytesPerRead = 2147483647;

             Type prxyT = assembly.GetTypes().First(t => t.IsClass && t.GetInterface(ctrName) != null && t.GetInterface(typeof(ICommunicationObject).Name) != null);

             object proxy = assembly.CreateInstance(prxyT.Name, false, System.Reflection.BindingFlags.CreateInstance,
                                     null, new object[] { binding, svcEP.Address }, CultureInfo.CurrentCulture, null);

             //object proxy = assembly.CreateInstance(prxyT.Name, false, System.Reflection.BindingFlags.CreateInstance,
             // null, new object[] { svcEP.Binding, svcEP.Address }, CultureInfo.CurrentCulture, null);


            return proxy;
          }
          catch (Exception e)
          {
              MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod GetProxy " + e.Message;
              return null;
          }


        }
        private WsdlImporter ImportWSDL(Uri wsdlLoc)
        {
            try
            {
                MetadataExchangeClient mexC = new MetadataExchangeClient(wsdlLoc, MetadataExchangeClientMode.HttpGet);
                mexC.ResolveMetadataReferences = true;
                MetadataSet metaSet = mexC.GetMetadata();
                return new WsdlImporter(metaSet);
            }

            catch (Exception e)
            {
                MethodErrorText = "BlueCurve.Core.WCF.AM:bc_wcf_callmethod ImportWSDL: " + e.Message;
                return null;
            }
        }



        private Dictionary<string, IEnumerable<ServiceEndpoint>> GetEndPointsOfEachServiceContract(WsdlImporter imptr, Collection<ContractDescription> svcCtrDescs)
        {
            ServiceEndpointCollection allEP = imptr.ImportAllEndpoints();
            var ctrEP = new Dictionary<string, IEnumerable<ServiceEndpoint>>();
            foreach (ContractDescription svcCtrDesc in svcCtrDescs)
            {
                List<ServiceEndpoint> eps = allEP.Where(x => x.Contract.Name == svcCtrDesc.Name).ToList();
                ctrEP.Add(svcCtrDesc.Name, eps);
            }
            return ctrEP;
        }
        private CodeCompileUnit GetServiceAndDataContractCompileUnitFromWSDL(Collection<ContractDescription> svcCtrDescs)
        {
            ServiceContractGenerator svcCtrGen = new ServiceContractGenerator();
            foreach (ContractDescription ctrDesc in svcCtrDescs)
            {
                svcCtrGen.GenerateServiceContractType(ctrDesc);
            }
            return svcCtrGen.TargetCompileUnit;
        }
        private object Deserialize(string xml, Assembly assembly)
        {
            Type ctr = GetDataContractType(xml, assembly);
            return Deserialize(xml, ctr);
        }
        private object Deserialize(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer d = new DataContractSerializer(toType);
                return d.ReadObject(stream);
            }
        }
        private Type GetDataContractType(string xml, Assembly assembly)
        {
            var serializedXML = ConvertToXML(xml);
            var match = assembly.GetTypes().First(x => x.Name == serializedXML.Root.Name.LocalName);
            return match;
        }
        private XDocument ConvertToXML(string xml)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                return XDocument.Load(stream);
            }
        }
        private CompilerResults GenerateContractsAssemblyInMemory(params CodeCompileUnit[] codeCompileUnits)
        {
            // Generate a code file for the contracts 
            CodeGeneratorOptions opts = new CodeGeneratorOptions();
            opts.BracingStyle = "C";
            CodeDomProvider pro = CodeDomProvider.CreateProvider("C#");
            // Compile the code file to an in-memory assembly
            // Don't forget to add all WCF-related assemblies as references
            CompilerParameters prms = new CompilerParameters(new string[] { "System.dll", "System.ServiceModel.dll", 
                "System.Runtime.Serialization.dll"});
            prms.GenerateInMemory = true;
            prms.GenerateExecutable = false;
            return pro.CompileAssemblyFromDom(prms, codeCompileUnits);
        }
    }

     

    class bc_wcf_methoddetails : bc_cs_soap_base_class
    {

        //ArrayList methoddelist = new ArrayList();
        public List<bc_wcf_method> methoddelist = new List<bc_wcf_method>();
              

        public void db_read()
        {
            bc_cs_activity_log otrace = new bc_cs_activity_log("bc_wcf_methoddetails", "db_read", bc_cs_activity_codes.TRACE_ENTRY.ToString(), "", ref certificate);

            try
            {
                bc_wcf_method oinput;
                object res;
                Array ares;
                var expectedType = typeof(object[,]);

                bc_wcf_methoddetails_db db_methodlist = new bc_wcf_methoddetails_db();
                res = db_methodlist.ReadAllMethods(ref certificate);
                ares = (Array)res;

                int i;
                for (i = 0; i < ares.GetUpperBound(1) + 1; i++)
                {
                    oinput = new bc_wcf_method();
                    oinput.methodid = (long)ares.GetValue(0, i);
                    oinput.MethodName = ares.GetValue(1, i).ToString();
                    oinput.ContractName = ares.GetValue(2, i).ToString();
                    oinput.WSDLUri = ares.GetValue(3, i).ToString();
                    oinput.ServiceUri = ares.GetValue(4, i).ToString();

                    methoddelist.Add(oinput);
                }
            }

            catch (Exception e)
            {
                bc_cs_error_log db_err = new bc_cs_error_log("bc_wcf_methoddetails", "db_read", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);
            }
            finally
            {
                otrace = new bc_cs_activity_log("bc_wcf_methoddetails", "db_read", bc_cs_activity_codes.TRACE_EXIT.ToString(), "", ref certificate);
            }

        }

    }

  class bc_wcf_methoddetails_db
  {
    bc_cs_db_services gbc_db = new bc_cs_db_services();

    public object ReadAllMethods(ref bc_cs_security.certificate certificate)
    {
        object Results = new object();
        string Sql = null;
        Sql = "bc_core_wcf_get_methods";
        Results = gbc_db.executesql(Sql, ref certificate);

        var collection = Results as System.Collections.IEnumerable;
        
        //return collection
        //    .Cast<object>()
        //    .Select(x => x.ToString())
        //    .ToArray();

        return Results;
    }
  }
  
       

    class bc_wcf_method : bc_cs_soap_base_class
    {

        public long write_mode ;
        public long methodid;
        public string MethodName;
        public string ContractName;
        public string WSDLUri;
        public string ServiceUri;

        //public long write_mode { get; set; }
        //public long methodid { get; set; }
        //public string MethodName { get; set; }
        //public string ContractName { get; set; }
        //public string WSDLUri { get; set; }
        //public string ServiceUri { get; set; }
          
        public const int  UPDATE = 1;

        //public bc_wcf_method(int method_id,string method_name, string contract_name,string wsdl_uri, string service_uri)
        //{
        //    methodid = method_id;
        //    MethodName = method_name;
        //    ContractName = contract_name;
        //    WSDLUri = wsdl_uri;
        //    ServiceUri = service_uri;
        //}

           

    //Public Sub db_read()

    //End Sub
  
}
  

    public class bc_wcf_methodparams : bc_cs_soap_base_class
    {

    //ArrayList methoddelist = new ArrayList();
    public List<bc_wcf_param> paramlist = new List<bc_wcf_param>();
    

    public void  db_read(long methodid)
    {
      bc_cs_activity_log otrace = new bc_cs_activity_log("bc_wcf_methodparame", "db_read", bc_cs_activity_codes.TRACE_ENTRY.ToString(), "", ref certificate);

      try
      {
          bc_wcf_param oinput;
          object res;
          Array ares;
          var expectedType = typeof(object[,]);

          bc_wcf_methodparams_db db_paramlist = new bc_wcf_methodparams_db();
          res = db_paramlist.ReadParams(methodid, ref certificate);
          ares = (Array)res;
         
          int i;
          for (i = 0; i < ares.GetUpperBound(1) + 1; i++)
          {
              oinput = new bc_wcf_param();
              oinput.paramid = (long)ares.GetValue(0, i);
              oinput.methodid = (long)ares.GetValue(1, i);
              oinput.paramname = ares.GetValue(2, i).ToString();
              oinput.paramtype = ares.GetValue(3, i).ToString();
              oinput.paramvaluetype = ares.GetValue(4, i).ToString();
              oinput.paramvalue = ares.GetValue(5, i).ToString();
              oinput.paramproc= ares.GetValue(6, i).ToString();
              paramlist.Add(oinput);
          }
         
      }
     
      catch (Exception e)
      {
          bc_cs_error_log db_err = new bc_cs_error_log("bc_wcf_methoddetails", "db_read", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);
      }
      finally
      {
            otrace = new bc_cs_activity_log("bc_wcf_methoddetails", "db_read", bc_cs_activity_codes.TRACE_EXIT.ToString(), "", ref certificate);
      }
    }


    public Array run_procedure(string procname, string docid)
    {
        bc_cs_activity_log otrace = new bc_cs_activity_log("bc_wcf_methodparame", "run_procedure", bc_cs_activity_codes.TRACE_ENTRY.ToString(), "", ref certificate);

        try
        {
            object res;
            Array ares;
            var expectedType = typeof(object[,]);
            Array returnvalue; 


            bc_wcf_methodparams_db db_param = new bc_wcf_methodparams_db();
            res = db_param.RunProc(procname, docid, ref certificate);
            ares = (Array)res;
            returnvalue = ares;
            return returnvalue;
        }

        catch (Exception e)
        {
            bc_cs_error_log db_err = new bc_cs_error_log("bc_wcf_methoddetails", "run_procedure", bc_cs_error_codes.USER_DEFINED.ToString(), e.Message, ref certificate);

            int[] returnvalue = new int[] { 0 };
            return returnvalue;  
           
        }
        finally
        {
            otrace = new bc_cs_activity_log("bc_wcf_methoddetails", "run_procedure", bc_cs_activity_codes.TRACE_EXIT.ToString(), "", ref certificate);
        }
    }


    }

    public class bc_wcf_methodparams_db
    {
        bc_cs_db_services gbc_db = new bc_cs_db_services();

        public object ReadParams(long methodid, ref bc_cs_security.certificate certificate)
        {
            object Results = new object();
            string Sql = null;
            Sql = "bc_core_wcf_get_methodsparams " + Convert.ToString(methodid);
            Results = gbc_db.executesql(Sql, ref certificate);

            var collection = Results as System.Collections.IEnumerable;
            
            return Results;
        }

        public object RunProc(string procname, string docid, ref bc_cs_security.certificate certificate)
        {
            object Results = new object();
            string Sql = null;
            Sql = procname + " '" + docid + "'";
            Results = gbc_db.executesql(Sql, ref certificate);

            var collection = Results as System.Collections.IEnumerable;

            return Results;
        }
    }

    
    public class bc_wcf_param : bc_cs_soap_base_class
    {
        public long write_mode;
        public long paramid;
        public long methodid;
        public string paramname;
        public string paramtype;
        public string paramvaluetype;
        public string paramvalue;
        public string paramproc;
        public const int UPDATE = 1;
    }


    public class bc_compiled_proxies
    {
        //Public Shared proxies As New ArrayList
        public List<bc_wcf_proxy> proxylist = new List<bc_wcf_proxy>();
    }

    public class bc_wcf_proxy : bc_cs_soap_base_class
    {
        public object proxy;
        public string ContractName;
    }

}
