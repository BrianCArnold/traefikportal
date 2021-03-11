using System;
using System.Collections.Generic;

namespace TraefikPortal
{
    public class TraefikEndPoint
    {
        public IEnumerable<string> EntryPoints { get; set; } 
        public IEnumerable<string> Middlewares { get; set; } 
        public IEnumerable<string> Using { get; set; } 
        public long Priority { get; set; }
        public string Service { get; set; }
        public string Rule { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public TLS TLS  { get; set; }
    }
    public class TLS 
    {
        public string CertResolver { get; set; }
        public IEnumerable<Domain> Domains { get; set; }
    }
    public class Domain 
    {
        public string Main { get; set; }
    }
}
