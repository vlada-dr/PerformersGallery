using System;
using System.Collections.Generic;
using System.Web;

namespace PerformersGallery.Helpers
{
    public class RequestsHelper
    {
        public static string BuildQuery(string baseUrl, Dictionary<string, string> queryParams)
        {
            var builder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var (key, value) in queryParams) query[key] = value;
            builder.Query = query.ToString();
            return builder.ToString();
        }
    }
}