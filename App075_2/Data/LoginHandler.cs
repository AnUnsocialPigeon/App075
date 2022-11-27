using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static App075_2.Interfaces.CookieClass;

namespace App075_2.Data {
    public class LoginHandler {

        public LoginHandler() {
            CookieInterface = new(JsRuntime);
        }

        [Inject]
        IJSRuntime JsRuntime { get; set; }
        [Inject]
        private Cookie? CookieInterface { get; set; }

        public string Username { get => _username ?? ""; set => _username = value; }
        private string? _username { get; set; }
        public bool? LoggedIn => _username is null ? null : _username != "";
        
        public int Count { get; set; } = 0;


        public bool? Login(string user) {
            Username = user;
            return LoggedIn;
        }
    }
}
