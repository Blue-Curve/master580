using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using bc_core_component_translation;
using bc_component_metadata;
using nbc_core_document_composition;
using System.Web.Script.Serialization;

namespace bc_core_components_svc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    
    public class ComponentsService : IComponentsService
    {
      

        public string TestService(int value)
        {
            return string.Format("Paul You entered: {0}", value);
        }


        public string TestComponent(bc_core_component component)
        {
            return new JavaScriptSerializer().Serialize(component);
        }

        public string GetJsonForBCComponent(long entity_id, int type, long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 1, DateTime data_at_date = default(DateTime), long pub_type_id = 0, List <bc_core_component_parameter> parameters=null)
        {
            return GetJson(bc_component_mode.COMPONENT, entity_id, type, 0, 0, 0, "", stage_id, contributor_id, user_id, language_id, data_at_date, pub_type_id, parameters);
        }


        public string GetJsonForBCComponentInTemplate(long entity_id, int type, long template_id, long comp_id, long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 0, DateTime data_at_date = default(DateTime), long pub_type_id=0)
        {
            return GetJson(bc_component_mode.TEMPLATE, entity_id, type, 0, comp_id, template_id, "", stage_id, contributor_id, user_id, language_id, data_at_date, pub_type_id,null);
        }


        public string GetJsonForBCComponentInDocument(long entity_id, int type, long doc_id, string locator = "", long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 0, DateTime data_at_date = default(DateTime), long pub_type_id=0)
        {
            return GetJson(bc_component_mode.DOCUMENT, entity_id, type, doc_id, 0, 0, locator, stage_id, contributor_id, user_id, language_id, data_at_date, pub_type_id,null);
        }
        public string GetJsonForDocumentCompostion(long doc_id, bool save_to_file = false)
        {
            try
            {
                bc_cs_central_settings bcs = new bc_cs_central_settings(true);
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                certificate.user_id = "0";
                certificate.os_name = "anonomous";
                bc_core_document_composition dc = new bc_core_document_composition(doc_id, ref certificate);
                string sjson;
                sjson = dc.json_get_document_composition();

                // JL: If the sjson file is empty do not save to file.
                if (sjson.Length <= 0)
                {
                    save_to_file = false;
                }


                if (certificate.error_state == false)
                {
                    if (save_to_file== true)
                    {
                        if (dc.save_json_to_file_repos(sjson) == false)
                        {
                            return certificate.server_errors[0].ToString();
                        }
                    }
                    return sjson;
                }
                else
                    return certificate.server_errors[0].ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
        public string GetJson(bc_component_mode mode, long entity_id, int type, long doc_id = 0, long comp_id = 0, long template_id = 0, string locator = "", long stage_id = 8, long contributor_id = 1, long user_id = 0, long language_id = 1, DateTime data_at_date = default(DateTime), long pub_type_id = 0,List <bc_core_component_parameter> parameters = null)
        {
            try
            {
                bc_cs_central_settings bcs = new bc_cs_central_settings(true);
                bc_om_refresh_component rc = new bc_om_refresh_component();
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                certificate.user_id = "0";
                certificate.os_name = "anonomous";

                rc.entity_id = entity_id;
                rc.type = type;

                if (stage_id == 0)
                    stage_id = 8;
                if (language_id == 0)
                    language_id = 1;
                if (contributor_id == 0)
                    contributor_id = 1;

                
                // now get the rest of the parameters required for the component
                bc_component_metadata.bc_component_metadata cm = new bc_component_metadata.bc_component_metadata(rc, template_id,comp_id,doc_id, locator, ref certificate);
                cm.set_metadata(mode,parameters);

                if (certificate.error_state == false)
                {
                    // set default date time if default supplied
                    DateTime default_da = new DateTime(0001, 01, 01);
                    if (default_da == data_at_date)
                    {
                        data_at_date = new DateTime(9999, 09, 09);
                    }
                    // get rest of parameters required from DB
                    bool b = true;
                    ArrayList c = null;
                    ArrayList d = null;
                    rc.db_read(pub_type_id, 1, stage_id, language_id, data_at_date,"", 1, 1, ref b, null, ref c, ref d, certificate);
                    if (certificate.error_state == false)
                    {
                        Array res;
                        res = (Array)rc.refresh_value;
                        bc_core_component_translation.bc_core_component_translation ct = new bc_core_component_translation.bc_core_component_translation();
                        return ct.translate_bc_table_to_JSON(res);
                    }
                    else
                        return certificate.server_errors[0].ToString();
                }
                else
                    return certificate.server_errors[0].ToString();
          
            }
                
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
