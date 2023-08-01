namespace IntrumWebApi.Models.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
