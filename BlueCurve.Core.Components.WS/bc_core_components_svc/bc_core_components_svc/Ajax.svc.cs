// AJAX Enabled Web Services
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace bc_core_components_svc
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AjaxComponentsService
    {
        //Ajax realtime components
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string AjaxGetJsonForBCComponent(bc_core_component component)
        {
            try
            {
                ComponentsService cs = new ComponentsService();
                return cs.GetJson(bc_component_metadata.bc_component_mode.COMPONENT, component.entity_id, component.type, 0, 0, 0, "", component.stage_id, component.contributor_id, component.user_id, component.language_id, component.data_at_date, component.pub_type_id, component.parameters);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message.ToString();
            }

        }
        //Ajax document composition (componetize)
        [WebInvoke(Method = "POST",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string AjaxGetJsonBCDocumentComposition(long doc_id, bool save_to_file = false)
        {
            try   
            {
                ComponentsService cs = new ComponentsService();
                return cs.GetJsonForDocumentCompostion(doc_id,save_to_file);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message.ToString();
            }

        }

    }
}
