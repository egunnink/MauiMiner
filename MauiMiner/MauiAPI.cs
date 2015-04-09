using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MauiMiner
{
    public static class MauiAPI
    {
        public static List<JToken> GetCourse(string courseId)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/course/" + courseId;
            try
            {
                string result = GetJsonFromUrl(url);
                if (result[0] == '[')
                {
                    return JArray.Parse(result).ToList();
                }
                else
                {
                    return new List<JToken>() { JToken.Parse(result) };
                }
            }
            catch (WebException e)
            {
                //Console.WriteLine("Web error -> course: {0} message: {1}", courseId, e.Message);
                return new List<JToken>();
            }
        }

        public static List<JToken> GetCourseSections(int session, string courseNumber)
        {
            string[] parts = courseNumber.Split(':');
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections?json={{sessionId: {0}, courseSubject: '{1}', courseNumber: '{2}'}}&pageStart=0&pageSize=2147483647&",
                session, parts[0], parts[1]);
            try
            {
                string result = GetJsonFromUrl(url);
                return JToken.Parse(result)["payload"].ToList();
            }
            catch (WebException e)
            {
                //Console.WriteLine("Web error -> session: {0} course: {1} message: {2}", session, courseNumber, e.Message);
                return new List<JToken>();
            }
        }

        public static JToken GetCurrentSession()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sessions/current";
            return JToken.Parse(GetJsonFromUrl(url));
        }

        public static List<JToken> GetSessionRange(int fromId, int step)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sessions/range?from=" + fromId + "&steps=" + step;
            return JArray.Parse(GetJsonFromUrl(url)).ToList();
        }

        private static string GetJsonFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            using (StreamReader r = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return r.ReadToEnd();
            }
        }
    }
}
