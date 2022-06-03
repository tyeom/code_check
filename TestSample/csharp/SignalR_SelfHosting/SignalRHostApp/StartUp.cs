using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalRHostApp
{
    public class StartUp
    {
        public void Configuration(IAppBuilder MyApp)
        {
            //MyApp.MapSignalR();
            MyApp.Map("/signalrTest", map =>
                {
                    map.RunSignalR(new HubConfiguration { EnableJSONP = true });
                });
        }
    }
}
