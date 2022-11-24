namespace App075_2.Data.Psych {
    public class Player {
        public Player(string username, string? answer = null, int score = 0) {
            Username = username;
            Answer = answer == "" ? null : answer;
            Score = score;
        }

        public string Username { get; set; }
        public string? Answer { get; set; }
        public int Score { get; set; }
        
    }
}
