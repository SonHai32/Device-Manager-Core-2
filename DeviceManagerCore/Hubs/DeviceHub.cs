using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace DeviceManagerCore.Hubs
{
    public class DeviceHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine(Context.User.Identity.Name + "has connected");
            return base.OnConnectedAsync();
        }
    }
}
