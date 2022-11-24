using Microsoft.AspNetCore.SignalR;

namespace App075_2.Server.Hubs {
    public class PsychHub : Hub {
        public async Task JoinRequest(string user) {
            await Clients.All.SendAsync("JoinRequest", user);
        }
        public async Task JoinResponse(string userTo, string userFrom, string message, int score) { 
            await Clients.User(userTo).SendAsync("JoinResponse", userFrom, message, score);
        }
        public async Task AnswerSubmit(string user, string answer) {
            await Clients.All.SendAsync("AnswerSubmit", user, answer);
        }

        public async Task GameStateUpdate(int gamestate) {
            await Clients.Others.SendAsync("GameStateUpdate", gamestate);
        }

    }
}

