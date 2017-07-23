using System;
using System.Linq;
using System.Net;
using System.Threading;
using RestSharp;

namespace OctopusSimulation
{
    internal class Program
    {
        private static RestResponseCookie _cookie;

        private static void Main(string[] args)
        {
            var loginResponseBody = IsAuthenticated();
            if (loginResponseBody)
            {
                Console.WriteLine("Adding a record now ..");
                Thread.Sleep(2000);
                var createATimeSheetRecordResponse = CreateATimeSheetRecord();
                Console.WriteLine(createATimeSheetRecordResponse);
                Console.Read();
            }
            else
            {
                Console.WriteLine("Not Authenticated!");
            }
            Console.ReadLine();
        }

        private static bool IsAuthenticated()
        {
            var client = new RestClient("https://www.octopus-hr.co.uk/portal/login.asp?ajax=t");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("Username", "qaworkstest", ParameterType.GetOrPost);
            request.AddParameter("Password", "qaworks123456789", ParameterType.GetOrPost);

            var response = client.Execute(request);
            _cookie = response.Cookies.SingleOrDefault(x => x.Name == "ASPSESSIONIDAGSAARRB");
            return !response.Content.Contains("Your username or password is not valid.");
        }

        private static string CreateATimeSheetRecord()
        {
            var cookieJar = new CookieContainer();
            if (_cookie != null)
                cookieJar.Add(new Cookie(_cookie.Name, _cookie.Value, _cookie.Path, _cookie.Domain));
            var client =
                new RestClient("https://www.octopus-hr.co.uk/portal/time/time_pf.aspx")
                {
                    CookieContainer = cookieJar
                };


            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("1", "1", ParameterType.GetOrPost);
            request.AddParameter("DateFrom_Date", "23/07/2017", ParameterType.GetOrPost);
            request.AddParameter("DateFrom_Time", "09:00:00", ParameterType.GetOrPost);
            request.AddParameter("DateTo_Date", "23/07/2017", ParameterType.GetOrPost);
            request.AddParameter("DateTo_Time", "17:30:00", ParameterType.GetOrPost);
            request.AddParameter("Amount", "", ParameterType.GetOrPost);
            request.AddParameter("Break", "", ParameterType.GetOrPost);
            request.AddParameter("Hours", "", ParameterType.GetOrPost);
            request.AddParameter("Rate", "", ParameterType.GetOrPost);
            request.AddParameter("Status", "", ParameterType.GetOrPost);
            request.AddParameter("ApprovalStatus", "", ParameterType.GetOrPost);
            request.AddParameter("Total", "", ParameterType.GetOrPost);
            request.AddParameter("Notes", "", ParameterType.GetOrPost);
            request.AddParameter("BreakFrom", "23/07/2017 12:00:00", ParameterType.GetOrPost);
            request.AddParameter("BreakTo", "23/07/2017 13:00:00", ParameterType.GetOrPost);
            request.AddParameter("TypeID1", "146324", ParameterType.GetOrPost);
            request.AddParameter("TypeID2", "270375", ParameterType.GetOrPost);
            request.AddParameter("TypeID3", "312888", ParameterType.GetOrPost);
            request.AddParameter("rndval", "1500821927726", ParameterType.GetOrPost);


            var response = client.Execute(request);
            return response.Content;
        }
    }
}