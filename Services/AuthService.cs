using System.IO;
using System.Text.Json;
using AlgoPuzzleBoard.MVC.Models;

namespace AlgoPuzzleBoard.MVC.Services
{
    public class AuthService
    {
		// Using the specific path requested by the user
		private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "users.txt");


		public AuthService()
        {
            // Ensure file exists
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public bool ValidateUser(string username, string password)
        {
            var users = GetUsers();
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            
            if (user == null) return false;
            
            // In a real app, use hashing. Here we compare plain text as requested for "simple" storage.
            return user.Password == password;
        }

        public bool UserExists(string username)
        {
            var users = GetUsers();
            return users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void RegisterUser(string username, string password)
        {
            var users = GetUsers();
            users.Add(new User { Username = username, Password = password });
            SaveUsers(users);
        }

        private List<User> GetUsers()
        {
            if (!File.Exists(_filePath)) return new List<User>();

            try
            {
                var json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json)) return new List<User>();
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
