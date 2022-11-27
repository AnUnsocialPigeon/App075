using Microsoft.AspNetCore.SignalR;

namespace App075_2.Server.Hubs {
    public class PsychHub : Hub {
        private Dictionary<string, string> UserIDToClientID = new Dictionary<string, string>();
        public async Task JoinRequest(string userID, string user) {
            //// BAD
            //if (!UserIDToClientID.TryAdd(userID, Context.ConnectionId)) {
            //    UserIDToClientID[userID] = Context.ConnectionId;
            //} 
            await Clients.All.SendAsync("JoinRequest", userID, user);
        }
        public async Task JoinResponse(string UserID, string userFrom, string message, int score) {
            // Bad. Do this with users. Not all. Cant figure out how to do to users.
            // Docs: https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections
            // https://stackoverflow.com/questions/27946102/signalr-user-is-not-registered-in-client-users
            await Clients.Others.SendAsync("JoinResponse", userFrom, message, score);
        }
        public async Task AnswerSubmit(string user, string answer) {
            await Clients.All.SendAsync("AnswerSubmit", user, answer);
        }

        public async Task GameStateUpdate(int gamestate) {
            await Clients.Others.SendAsync("GameStateUpdate", gamestate);
        }

    }
}

