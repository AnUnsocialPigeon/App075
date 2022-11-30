using Microsoft.AspNetCore.SignalR;

namespace App075_2.Server.Hubs {
    public class BingoHub : Hub {
        public async Task Submit(int id, bool on) {
            await Clients.Others.SendAsync("Submit", id, on);   
        }
    }
}
