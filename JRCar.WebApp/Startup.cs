using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace JRCar.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
