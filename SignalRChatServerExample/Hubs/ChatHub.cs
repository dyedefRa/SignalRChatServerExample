using Microsoft.AspNetCore.SignalR;
using SignalRChatServerExample.Data;
using SignalRChatServerExample.Models;
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
            //Nicknameleri gönder.
            await Clients.All.SendAsync("allClients", ClientSource.Clients);
        }

        //Genel mesaj fonk.
        public async Task SendMessageAsync(string message, string clientName)
        {
            MyClient senderClient = ClientSource.Clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (clientName == "Tümü")
            {
                await Clients.Others.SendAsync("receiveMessage", message, senderClient.NickName);
            }
            else
            {
                MyClient client = ClientSource.Clients.FirstOrDefault(x => x.NickName == clientName);
                await Clients.Client(client.ConnectionId).SendAsync("receiveMessage", message, senderClient.NickName);
            }

        }

        //Grup oluştur
        public async Task CreateGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            GroupSource.Groups.Add(new MyGroup() { GroupName = groupName });

            await Clients.All.SendAsync("addedGroup", groupName);
        }

        //Gruba katil
        public async Task JoinGroup(IEnumerable<string> groups)
        {
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
        }
    }
}
