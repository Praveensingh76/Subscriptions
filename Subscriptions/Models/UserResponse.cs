namespace Subscriptions.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public string Response { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
