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

        public void Configuration(IAppBuilder app)  {

            app.Use(new Func<AppFunc, AppFunc>(ignoreNextApp => (AppFunc)Invoke));
        }

        public Task Invoke(IDictionary<string, object> environment) 
        {
            byte[] responseBytes = ASCIIEncoding.UTF8.GetBytes(
                string.Format("Serviced request on {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()));

            Stream responseStream = (Stream)environment["owin.ResponseBody"];
            IDictionary<string, string[]> responseHeaders = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
            responseHeaders["Content-Length"] = new[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
            responseHeaders["Content-Type"] = new[] { "text/plain" };
            return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}