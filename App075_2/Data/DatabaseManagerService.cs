using System.Data.SQLite;

namespace App075_2.Data {
    public class DatabaseManagerService {
        public DatabaseManagerService() { }
        
        private static SQLiteConnection ConnectToDatabase() {
            SQLiteConnection c = new("Data Source=App075Database.db;Version=3;New=True;Compress=True;");
            c.Open();
            c.CreateCommand();
            return c;
        }
        public int GetCount_Database(string User) {
            SQLiteConnection conn = ConnectToDatabase();
            SQLiteCommand c = conn.CreateCommand();
            c.CommandText = $"SELECT Counter FROM {DB_Users_Table} WHERE (Username = {User})";
            SQLiteDataReader r = c.ExecuteReader();
            r.Read();
            int.TryParse(r.GetString(0), out int ret);
            r.Close();
            conn.Close();
            return ret;
        } 


        public int GetCount(string User) {
            if (!UserExists(User)) return -1;
            return int.Parse(File.ReadAllLines(FileName(User))[1]);
        }


        public void UpdateCount(string User, int Count) {
            if (!UserExists(User)) return;
            string[] file = File.ReadAllLines(FileName(User));
            file[1] = Count.ToString();
            File.WriteAllLines(FileName(User), file);
        }

        public string FileName(string User) => $"{Directory.GetCurrentDirectory()}\\Users\\{User}.txt";
        public bool UserExists(string User) => File.Exists(FileName(User));


        private const string DB_Users_Table = "users";
    }
}
