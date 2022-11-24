using App075_2.Data;

namespace App075_2.Pages {
    public partial class Counter {
        private bool? LoggedIn = null;
        private string Username = "";
        private int currentCount = 0;

        protected async override Task OnAfterRenderAsync(bool FirstRender) {
            if (FirstRender) {
                Username = await Cookie.GetValue(Globals.CookieUsername);
                string c = await Cookie.GetValue(Globals.CookieCounter);
                LoggedIn = Username != "";

                if (c != "" && c is null) _ = int.TryParse(c, out currentCount);
                StateHasChanged();
            }
        }

        private void IncrementCount() {
            currentCount++;
        }

    }
}
