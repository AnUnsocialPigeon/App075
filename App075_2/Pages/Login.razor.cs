using App075_2.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace App075_2.Pages {
    public partial class Login {
        [Inject]
        IJSRuntime JsRuntime { get; set; }
        private LoginHandler LoginHandler = new();
        private string Username { get; set; } = "";


        protected async override Task OnAfterRenderAsync(bool FirstRender) {
            if (!FirstRender) return;
            LoginHandler.Login(await Cookie.GetValue(Globals.CookieUsername));
            StateHasChanged();
        }

        private async void SignInBTN_Clicked() {
            if (Username != "") {
                DatabaseManagerService d = new();

                // Failed to login
                if (!d.UserExists(Username)) {
                    await JsRuntime.InvokeVoidAsync("alert", "Username or Password is not valid"); // Alert
                    StateHasChanged();
                    return;
                }

                LoginHandler.Username = Username;
                await Cookie.SetValue(Globals.CookieUsername, LoginHandler.Username);
                await Cookie.SetValue(Globals.CookieCounter, d.GetCount(LoginHandler.Username).ToString());
                StateHasChanged();
            }
        }
        private async void SignOutBTN_Clicked() {
            await Cookie.SetValue(Globals.CookieUsername, "");
            await Cookie.SetValue(Globals.CookieCounter, "");
            LoginHandler = new();
            Username = "";
            // Refresh the webpage
            await OnAfterRenderAsync(true);
        }


    }
}
