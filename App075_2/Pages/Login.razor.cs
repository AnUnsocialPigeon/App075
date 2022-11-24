using App075_2.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace App075_2.Pages {
    public partial class Login {
        [Inject]
        IJSRuntime JsRuntime { get; set; }
        private bool? LoggedIn = null;
        private string Username = "";

        protected async override Task OnAfterRenderAsync(bool FirstRender) {
            if (FirstRender) {
                Username = await Cookie.GetValue(Globals.CookieUsername);
                LoggedIn = Username != "";
                StateHasChanged();
            }
        }

        private async void SignInBTN_Clicked() {
            if (Username != "") {
                DatabaseManagerService d = new();
                if (!d.UserExists(Username)) {
                    await JsRuntime.InvokeVoidAsync("alert", $"{d.FileName(Username)} : User does not exist"); // Alert
                    StateHasChanged();
                    return;
                }

                await Cookie.SetValue(Globals.CookieUsername, Username);
                await Cookie.SetValue(Globals.CookieCounter, d.GetCount(Username).ToString());

                LoggedIn = true;
                StateHasChanged();
            }
        }
        private async void SignOutBTN_Clicked() {
            await Cookie.SetValue(Globals.CookieUsername, "");
            await Cookie.SetValue(Globals.CookieCounter, "");
            Username = "";
            LoggedIn = false;
            StateHasChanged();
        }


    }
}
