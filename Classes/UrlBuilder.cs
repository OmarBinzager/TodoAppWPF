using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Classes
{
    public class UrlBuilder
    {
        public static string BuildQueryUrl(string baseUrl, Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return baseUrl;

            var sb = new StringBuilder();
            sb.Append(baseUrl);
            sb.Append(baseUrl.Contains("?") ? "&" : "?");

            foreach (var kvp in parameters)
            {
                string key = WebUtility.UrlEncode(kvp.Key);
                string value = WebUtility.UrlEncode(kvp.Value ?? "");
                sb.Append($"{key}={value}&");
            }

            // Remove trailing '&'
            sb.Length--;

            return sb.ToString();
        }
    }
}
