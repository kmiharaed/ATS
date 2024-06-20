namespace PlayerService.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public int TeamId { get; set; }
    }
}
