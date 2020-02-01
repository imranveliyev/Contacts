using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Hubs
{
    public class NotificationHub : Hub
    {
        //public async Task SendNotification(string type, string text) 
        //{
        //    await Clients.All.SendAsync("notification", type, text);
        //}
    }
}
