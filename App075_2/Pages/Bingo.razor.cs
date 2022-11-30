using App075_2.Data;
using App075_2.Data.Bingo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace App075_2.Pages {
    public partial class Bingo {
        [Inject]
        private NavigationManager Navigation { get; set; }
        private HubConnection? hubConnection;
        LoginHandler LoginHandler = new();
        private bool Loading = true;

        private BingoDatabaseManagementSystem DB = new("BingoBoard");
        private BingoBoard BingoBoard = new();

        protected override async Task OnAfterRenderAsync(bool FirstRender) {
            if (!FirstRender) return;
            if (!(LoginHandler.Login(await Cookie.GetValue(Globals.CookieUsername)) ?? false)) {
                StateHasChanged();
                return;
            }

            // Hub setup
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/bingo"))
                .Build();

            AddNetworkLogic();

            // Add bingo board
            BingoBoard = DB.BingoBoard;

            // Start the hub
            await hubConnection.StartAsync();

            Loading = false;
            StateHasChanged();
        }

        private void AddNetworkLogic() {
            hubConnection.On<int, bool>("Submit", (id, on) => {
                BingoBoard.UpdateTile(id, on);
                StateHasChanged();
            });
        }

        private void ToggleOption(int id) {
            BingoBoard.ToggleTile(id);
            DB.UpdateBingoBoard(id, BingoBoard[id].Enabled);
        }
    }
}
