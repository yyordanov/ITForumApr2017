using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Example3
{
    public class TraceHandler : DelegatingHandler
    {
        public TraceHandler()
            : base()
        {
        }
        public TraceHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Trace.WriteLine($"{DateTime.Now} - {request.RequestUri}", "Request Started");
            try
            {
                return base.SendAsync(request, cancellationToken);
            }
            finally
            {
                Trace.WriteLine($"{DateTime.Now} - {request.RequestUri}", "Request Completed");
            }
        }
    }
}