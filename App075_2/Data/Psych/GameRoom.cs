namespace App075_2.Data.Psych {
    public class GameRoom {
        public GameRoom(string username) {
            Username = username;
        }

        public List<Player> Players { get; set; } = new();
        public int GameState { get; set; } = 0;

        public string Username { get; set; }
        public string CurrentAnswer { get; set; } = "";
        public int Score { get; set; } = 0;

        public void AddAnswer(string user, string answer) {
            foreach (Player p in Players) {
                if (p.Username == user) {
                    p.Answer = answer;
                    return;
                }
            }
            //throw new Exception("User does not exist");
            Players.Add(new Player(user, answer));
        }

        public void Add(Player p) {
            if (PlayerExists(p.Username)) return;
            Players.Add(p);
        }
        public void Add(string user, string? answer = null, int score = 0) {
            if (PlayerExists(user)) return;
            Players.Add(new Player(user, answer, score));
        }

        private bool PlayerExists(string user) {
            foreach (Player p in Players)
                if (p.Username == user)
                    return true;
            
            return false;
        }

    }
}
