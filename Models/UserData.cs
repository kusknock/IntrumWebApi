namespace IntrumWebApi.Models
{
    public class UserData
    {
        public string? Id { get; set; }
        public string? Username { get; set; }

        public UserData(string? id, string? username)
        {
            Id = id;
            Username = username;
        }
    }
}
