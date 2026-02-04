namespace Subscriptions.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDashboardViewModel
    {
        public Subscription Subscription { get; set; }
        public int CompletedQuestionsCount { get; set; }
        public int TotalQuestionsCount { get; set; }
        public double ProgressPercentage => (double)CompletedQuestionsCount / TotalQuestionsCount * 100;
        public string Guidance { get; set; }
    }

    public class QuestionnaireViewModel
    {
        public List<Question> Questions { get; set; }
    }


    public class RecommendationsViewModel
    {
        public string Recommendations { get; set; }
    }


}
