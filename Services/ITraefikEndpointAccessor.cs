using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TraefikPortal;

namespace TraefikPortal 
{
        
    public interface ITraefikEndpointAccessor 
    {
        IEnumerable<TraefikEndPoint> GetEndpoints();
    }

    public class HttpTraefikEndpointAccessor : ITraefikEndpointAccessor
    {
        public HttpTraefikEndpointAccessor(WebClient webClient)
        {
            WebClient = webClient;
            this.traefikRootUrl = Environment.GetEnvironmentVariable("TRAEFIK_ROOT_URL");
        }

        private readonly WebClient WebClient;
        private readonly string traefikRootUrl;
        private string routersUrl => traefikRootUrl + "/api/http/routers";

        public IEnumerable<TraefikEndPoint> GetEndpoints()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TraefikEndPoint[]>(WebClient.DownloadString(routersUrl));
        }
    }

    public interface IDashboardItemProvider 
    {
        IEnumerable<DashboardItem> GetDashboardItems();
    }

    public class TraefikDashboardItemProvider : IDashboardItemProvider
    {
        private readonly ITraefikEndpointAccessor endpointAccessor;

        public TraefikDashboardItemProvider(ITraefikEndpointAccessor endpointAccessor)
        {
            this.endpointAccessor = endpointAccessor;
        }
        private string Caps(string str) {
            if (str == null) return string.Empty;
            if (str.Length > 1){
                return str.First().ToString().ToUpper() + str.Skip(1).Aggregate("", (p,n) => p + n);
            }
            return str.ToUpper();
        }
        public IEnumerable<DashboardItem> GetDashboardItems()
        {
            var traefikEndPoints = endpointAccessor.GetEndpoints();
            var endPoints = traefikEndPoints.Where(t => !t.Name.EndsWith("@internal")).GroupBy(t => string.Join(' ', t.Name.Split("@").First().Split('-', '_').Select(Caps)));
            return endPoints.Select(p =>
                {
                    var rule = p.FirstOrDefault()?.Rule;
                    var hosts = Regex.Matches(rule, "Host[^(]*\\(`([^`]+)`\\)");
                    var paths = Regex.Matches(rule, "Path[^(]*\\(`([^`]+)`\\)");
                    
                    var host = hosts?.Cast<Match>()?.FirstOrDefault()?.Groups?.Cast<Group>()?.Skip(1)?.FirstOrDefault()?.Value;
                    var path = paths?.Cast<Match>()?.FirstOrDefault()?.Groups?.Cast<Group>()?.Skip(1)?.FirstOrDefault()?.Value;
                    var url = host == null ? "" : ("https://" + host) + (path ?? "");
                    return new DashboardItem{
                        Name = p.Key,
                        Url = url
                    };
                }
             );
        }
    }

}