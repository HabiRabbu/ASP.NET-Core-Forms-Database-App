namespace ISSProjectFINAL.Models
{
    public record User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessLevel { get; set; }
    }
}
