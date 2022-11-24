using App075_2.Data;
using App075_2.Data.Psych;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
namespace App075_2.Pages {
    public partial class Psych {
        [Inject]
        private NavigationManager Navigation { get; set; }
        private HubConnection? hubConnection;
        private bool Host { get; set; } = true;
        private bool Loading { get; set; } = true;
        private GameRoom GameInfo = new(""); // "" = null username as far as concerned
        private bool? LoggedIn = null;


        protected override async Task OnAfterRenderAsync(bool FirstRender) {
            if (!FirstRender) return;

            // Login setup
            GameInfo = new(await Cookie.GetValue(Globals.CookieUsername));
            LoggedIn = GameInfo.Username != "";
            if (!(LoggedIn ?? false)) {
                StateHasChanged();
                return;
            }

            // Hub setup
            hubConnection = new HubConnectionBuilder()
                    .WithUrl(Navigation.ToAbsoluteUri("/psych"))
                    .Build();
            AddNetworkLogic();
            await hubConnection.StartAsync();
            StateHasChanged();

            // Getting game info
            await hubConnection.SendAsync("JoinRequest", GameInfo.Username);

            Loading = false;
            StateHasChanged();
        }

        private void AddNetworkLogic() {
            // Joining logic
            hubConnection.On<string>("JoinRequest", (user) => {
                hubConnection.SendAsync("JoinResponse", user, GameInfo.Username, GameInfo.CurrentAnswer, GameInfo.Score);
                GameInfo.Add(new Player(user));
                InvokeAsync(StateHasChanged);
            });
            hubConnection.On<string, string, int>("JoinResponse", (userFrom, answer, score) => {
                GameInfo.Add(new Player(userFrom, answer, score));
                InvokeAsync(StateHasChanged);
            });

            // TODO: HOST LOGIC

            // Answer Logic
            hubConnection.On<string, string>("AnswerSubmit", (user, answer) => {
                GameInfo.AddAnswer(user, answer);
            });

            // Gamestate Logic
            hubConnection.On<int>("GameStateUpdate", (gamesate) => GameInfo.GameState = gamesate);
        }
    }
}
