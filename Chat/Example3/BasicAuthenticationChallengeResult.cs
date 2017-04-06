using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Example3
{
    public class BasicAuthenticationChallengeResult : IHttpActionResult
    {
        public BasicAuthenticationChallengeResult(HttpAuthenticationChallengeContext context, string realm)
        {
            this.context = context;
            innerResult = context.Result;

            if (string.IsNullOrEmpty(realm))
            {
                realm = context.Request.RequestUri.Host;
            }
            this.realm = realm;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await innerResult.ExecuteAsync(cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var parameter = string.Format("realm=\"{0}\"", realm);
                var header = new AuthenticationHeaderValue(AuthenticationSchemes.Basic.ToString(), parameter);
                response.Headers.WwwAuthenticate.Add(header);
            }

            return response;
        }

        private readonly HttpAuthenticationChallengeContext context;
        private readonly IHttpActionResult innerResult;
        private readonly string realm;
    }
}
