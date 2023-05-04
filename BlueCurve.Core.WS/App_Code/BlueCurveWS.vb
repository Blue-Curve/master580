Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://BlueCurve.Core.WS/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class BlueCurveWS
    Inherits System.Web.Services.WebService

    <WebMethod()> _
 Public Function generic_object_transmission(ByVal s As String) As String
        Dim bc_cs_settings As New bc_cs_central_settings(True)
        Dim o As New bc_cs_soap_base_class
        generic_object_transmission = o.receive_packet(s)
        GC.Collect()
    End Function
   
    '<WebMethod()> _
    'Public Function excel_function_transmission(ByVal s As String, ByVal logged_on_user_id As String, ByVal os_user_name As String) As String
    '    'Dim bc_cs_settings As New bc_cs_central_settings(True)
    '    'Dim o As New bc_om_excel_functions
    '    'excel_function_transmission = o.process_web_request(s, logged_on_user_id, os_user_name)
    '    'GC.Collect()
    'End Function

End Class
