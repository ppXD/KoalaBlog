using KoalaBlog.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;

namespace KoalaBlog.WebApiHost
{
    partial class WebApiHostWinService : ServiceBase
    {
        private static HttpSelfHostServer server;

        public WebApiHostWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            Close();   
        }

        public static bool Start()
        {
            var baseAddress = AppSettingConfig.BaseAddress;
            Uri baseUri;
            if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out baseUri))
            {
                Console.WriteLine("无效地址：" + baseAddress);
                return false;
            }
            try
            {
                var config = new HttpSelfHostConfiguration(baseUri);
                config.MaxReceivedMessageSize = int.MaxValue;
                config.Services.Replace(typeof(IAssembliesResolver), new WebApiLoader());
                config.Formatters.Clear();
                config.Formatters.Add(new JsonMediaTypeFormatter());
                //解决EF的Entity循环问题
                config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                WebApi.WebApiConfig.Register(config);
                server = new HttpSelfHostServer(config);
                server.OpenAsync().Wait();
            }
            catch (Exception ex)
            {
                var temp = ex;
                Console.WriteLine(temp.Message);
                while (temp.InnerException != null)
                {
                    Console.WriteLine(temp.Message);
                    temp = temp.InnerException;
                }
                return false;
            }
            return true;
        }

        public static void Close()
        {
            if(server != null)
            {
                server.CloseAsync();
            }
        }
    }
}
