using Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RedirectoR {

    using AppFunc = Func<IDictionary<string, object>, Task>;

    public partial class Startup {

        IDictionary<string, string> RedirectUrlLinks = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) 
        { 
            { "", "http://www.tugberkugurlu.com/" },
            { "tgbrk", "http://www.tugberkugurlu.com" },
            { "twitter", "http://twitter.com/tourismgeek" },
            { "linkedin", "http://www.linkedin.com/in/tugberk" },
            { "web-api", "http://www.tugberkugurlu.com/archive/getting-started-with-asp-net-web-api-tutorials-videos-samples" },
            { "webapi", "http://www.tugberkugurlu.com/archive/getting-started-with-asp-net-web-api-tutorials-videos-samples" },
            { "mugla1203", "http://www.tugberkugurlu.com/archive/slides-of-my-ms-web-platform-and-asp-net-mvc-101-talks" },
            { "ApiKeyAuth", "http://www.tugberkugurlu.com/archive/api-key-authorization-through-query-string-in-asp-net-web-api-authorizationfilterattribute" },
            { "mvc4async", "http://www.tugberkugurlu.com/archive/my-take-on-task-base-asynchronous-programming-in-c-sharp-5-0-and-asp-net-mvc-web-applications" }
        };

        public void Configuration(IAppBuilder app) 
        {
            app.Use(new Func<AppFunc, AppFunc>(ignoreNextApp => (AppFunc)Invoke));
        }

        public Task Invoke(IDictionary<string, object> environment) 
        {
            IDictionary<string, string[]> responseHeaders = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
            responseHeaders["Content-Length"] = new[] { "0" };

            string redirectUrl;
            string path = environment["owin.RequestPath"] as string;
            if (RedirectUrlLinks.TryGetValue(path.TrimStart('/'), out redirectUrl)) {

                responseHeaders["Location"] = new[] { redirectUrl };
                environment["owin.ResponseStatusCode"] = 301;
            }
            else {

                environment["owin.ResponseStatusCode"] = 404;
            }

            return Task.FromResult(0);
        }
    }
}