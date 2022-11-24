using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;


namespace App075_2.Pages {
    public partial class ChatApp {
        [Inject]
        NavigationManager Navigation { get; set; }

        private HubConnection? hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;

        private const string chathubGroup = "chathub";

        protected override async Task OnAfterRenderAsync(bool FirstRender) {
            if (FirstRender) {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(Navigation.ToAbsoluteUri($"/{chathubGroup}"))
                    .Build();

                hubConnection.On<string, string>("ReceiveMessage", (user, message) => {
                    var encodedMsg = $"{user}: {message}";
                    messages.Add(encodedMsg);
                    InvokeAsync(StateHasChanged);
                });

                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", chathubGroup);
                StateHasChanged();
            }
        }


        private async Task Send() {
            if (hubConnection is not null) {
                await hubConnection.SendAsync("SendMessageToGroup", chathubGroup, messageInput);
                //StateHasChanged();
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync() {
            if (hubConnection is not null) {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
