using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Example3
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public BasicAuthenticationAttribute()
        {
        }
        public BasicAuthenticationAttribute(string realm)
        {
            Realm = realm;
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
        public string Realm { get; private set; }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var authHeader = context.Request.Headers.Authorization;
            var basicScheme = AuthenticationSchemes.Basic.ToString();
            if (authHeader != null && string.Compare(authHeader.Scheme, basicScheme, true) == 0)
            {
                string username;
                string password;
                GetUserNameAndPassword(authHeader, out username, out password);

                string storedPassword;
                if (users.TryGetValue(username.ToLower(), out storedPassword))
                {
                    if (password == storedPassword)
                    {
                        var identity = new ClaimsIdentity(basicScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, username));
                        context.Principal = new ClaimsPrincipal(identity);
                    }
                }

                if (context.Principal == null || !context.Principal.Identity.IsAuthenticated)
                {
                    var headers = new AuthenticationHeaderValue[] { };
                    context.ErrorResult = new UnauthorizedResult(headers, context.Request);
                }
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new BasicAuthenticationChallengeResult(context, Realm);
            return Task.FromResult(0);
        }

        private void GetUserNameAndPassword(AuthenticationHeaderValue authHeader, out string username, out string password)
        {
            username = null;
            password = null;
            if (!string.IsNullOrEmpty(authHeader.Parameter))
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                try
                {
                    var bytes = Convert.FromBase64String(authHeader.Parameter);
                    var decoded = encoding.GetString(bytes);
                    var parts = decoded.Split(':');
                    if (parts.Length == 2)
                    {
                        username = parts[0];
                        password = parts[1];
                    }
                }
                catch (FormatException)
                {
                    // this is not a valid base64 string, so user/pass cannot be retrieved and they remain null
                }
            }
        }

        private static readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            {"kiro", "kk" },
            {"dimo", "dd" },
        };
    }
}
