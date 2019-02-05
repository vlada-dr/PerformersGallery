using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PerformersGallery.Helpers
{
    public class RequestsHelper
    {
        public static string BuildQuery(string baseUrl, Dictionary<string, string> queryParams)
        {
            var builder = new UriBuilder(baseUrl);
           // builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach(var item in queryParams)
            {
                query[item.Key] = item.Value;
            }
            builder.Query = query.ToString();
            return builder.ToString();
        }
    }
}
