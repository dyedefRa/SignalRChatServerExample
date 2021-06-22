using Microsoft.AspNetCore.SignalR;
using SignalRChatServerExample.Data;
using SignalRChatServerExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatServerExample.Hubs
{
    public class ChatHub : Hub
    {
        public async Task GetNickName(string nickName)
        {
            MyClient myClient = new MyClient()
            {
                ConnectionId = Context.ConnectionId,
                NickName = nickName
            };

            ClientSource.Clients.Add(myClient);
            await Clients.Others.SendAsync("clientJoined", nickName);
            //Nicknameleri göder.
            await Clients.All.SendAsync("allClients", ClientSource.Clients);
        }
    }
}
