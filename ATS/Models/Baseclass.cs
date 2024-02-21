using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using RestSharp;

namespace ATS.Models
{
    public class Baseclass
    {
        public static string InvokeServiceReq(string MethodName, string ParamInput, string MethodType)
        {
            string REQRES = string.Empty;
            try
            {
                string ServiceURL = ConfigurationManager.AppSettings["SERVICEURL"].ToString().Trim() + MethodName;
                var client = new RestClient(ServiceURL);
                client.Timeout = -1;
                var request = new RestRequest(MethodType == "POST" ? Method.POST : Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", ParamInput, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                REQRES = response.Content;
            }
            catch (Exception ex)
            {
                REQRES = string.Empty;
            }
            return REQRES;
        }
    }
}