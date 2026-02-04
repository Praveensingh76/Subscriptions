using Subscriptions.Models;

namespace Subscriptions.Interface
{
    public interface IRepository
    {
        Task<int> CreateUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User?> LoginUserAsync(string email, string passwordHash);
        Task<int> CreateOrUpdateSubscriptionAsync(Subscription subscription);
        Task<Subscription> CheckSubscriptionAsync(int userId);
        Task<int> SaveUserResponseAsync(UserResponse userResponse);
        Task<IEnumerable<UserResponse>> GetUserResponsesAsync(int userId);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<IEnumerable<Question>> GetQuestionsByCategoryAsync(string category);
        Task<IEnumerable<Guidance>> GetGuidanceRecommendationsAsync(int userId);
        Task<int> DeactivateExpiredSubscriptionsAsync();

    }

}
