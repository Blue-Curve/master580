using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace bluecurve.core.ad.wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
   
    public interface Ibluecurve_core_ad_wcf
    {
        [OperationContract]
        string TestAdConnect(string os_logon, string mac_address);
    }   
}
