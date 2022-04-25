using FastMember;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TeamChartSync
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            Client();
        }

        public static void Client()
        {
            //https://dcazds410.westpharma.net:52000/sap/opu/odata/SAP/ZMYSAP_LOGON_SRV/?sap-client=200
            //var result = client.GetAsync("http://dcazdhr10.westpharma.net:8080/sap/opu/odata/sap/ZHR_TEAM_CHART_SRV/ZHR_TEAM_CHART?$format=json");
            WebRequest req = WebRequest.Create(@"http://dcazdhr10.westpharma.net:8080/sap/opu/odata/sap/ZHR_TEAM_CHART_SRV/ZHR_TEAM_CHART?$format=json");
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("dnetcon:DNETCON1!"));
            //req.Credentials = new NetworkCredential("username", "password");
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;
            WebHeaderCollection header = response.Headers;

            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                var abc = JObject.Parse(reader.ReadToEnd());
                string path = "$..results";
                var properties = abc.SelectToken(path).ToString();
                List<OrgChartModel> responseText = JsonConvert.DeserializeObject<List<OrgChartModel>>(properties);

                using (var bcp = new SqlBulkCopy(@"Server=tcp:enterprisecrm-sqlserv.database.windows.net,1433;Initial Catalog=enterprisecrm-db-dv_test_Copy;Persist Security Info=False;User ID=enterpriseusadmin;Password=W3stSql@dminPwd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    using (var reader1 = ObjectReader.Create(responseText))
                    {
                        bcp.DestinationTableName = "OrgChartModel";
                        bcp.WriteToServer(reader1);
                    }
                }
            }
        }

        public void trial()
        {
            //Public Function GetSAPSSOTicket(sPortalURL As String, ByRef Ticket As String, ByRef ErrorMsg As String) As Boolean
            Dim offset As Long
    GetSSOTicket = False
    ErrorMsg = ""
    Ticket = ""
    Const MYSAPSSO2 As String = "MYSAPSSO2="
On Error GoTo Err1
    'contact the sap portal
    Dim req As New WinHttp.WinHttpRequest
    req.Open "GET", sPortalURL, False
    req.SetAutoLogonPolicy AutoLogonPolicy_Always
    req.Send
    Dim S As String
    S = req.GetAllResponseHeaders()
    'parse the ticket out of the response
    offset = InStr(1, S, MYSAPSSO2, vbTextCompare)
    If offset <= 0 Then
        ErrorMsg = "The Portal Server returned an empty ticket. Authentication failed."
        GoSub Cleanup
        Exit Function
    End If
    S = Mid(S, offset + Len(MYSAPSSO2))
    offset = InStr(1, S, ";")
    S = Left(S, offset - 1)
    Ticket = S
    'complete
On Error GoTo 0
    'success
    GoSub Cleanup
    GetSSOTicket = True
    Exit Function
Cleanup:
            Set req = Nothing
    Return
Err1:
    'some error
    GoSub Cleanup
    ErrorMsg = Err.Description
End Function
            }
    }
}
