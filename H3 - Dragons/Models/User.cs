using System.Text.Json.Serialization;

namespace H3___Dragons.Models
{
    public class User
    {
        [JsonPropertyName("Name")]
        public string DragonName { get; private set; }
        [JsonPropertyName("Password")]
        public string Password { get; private set; }
        [JsonPropertyName("Role")]
        public string Role { get; private set; }

        public User(string dragonName, string password, string role = "listener")
        {
            DragonName = dragonName;
            Password = password;
            Role = role;
        }
    }
}
