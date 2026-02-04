using Dapper;
using Microsoft.Extensions.Configuration;
using Subscriptions.Interface;
using Subscriptions.Models;
using System.Data;
using System.Data.SqlClient;

namespace Subscriptions.Repo
{
    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        private string conn;

        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateUser(User user)
        {
            using (var connection = new SqlConnection(conn))
            {
                connection.Open();
                var parameters = new
                {
                    user.Username,
                    user.Email,
                    user.PasswordHash,
                    user.SubscriptionStatus,
                    QueryType = "Register"
                };
                return await connection.ExecuteAsync("sp_User", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<User?> LoginUserAsync(string email, string passwordHash)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    Email = email,
                    PasswordHash = passwordHash,
                    QueryType = "Login"
                };
                return await connection.QueryFirstOrDefaultAsync<User>("sp_User", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(conn))
            {
                connection.Open();
                var parameters = new { Email = email };
                return await connection.QuerySingleOrDefaultAsync<User>("sp_User", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> CreateOrUpdateSubscriptionAsync(Subscription subscription)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    UserId = subscription.UserId,
                    PlanType = subscription.PlanType,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    QueryType = "Create"
                }; return await connection.ExecuteAsync("sp_Subscription", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<Subscription?> CheckSubscriptionAsync(int userId)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    UserId = userId,
                    QueryType = "Check"
                }; return await connection.QueryFirstOrDefaultAsync<Subscription>("sp_Subscription", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<int> DeactivateExpiredSubscriptionsAsync()
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    QueryType = "Deactivate"
                };
                return await connection.ExecuteAsync("sp_Subscription", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    QueryType = "GetAllQuestions"
                };
                return await connection.QueryAsync<Question>("sp_Assessment", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<Question>> GetQuestionsByCategoryAsync(string category)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    Category = category,
                    QueryType = "GetQuestionsByCategory"
                }; return await connection.QueryAsync<Question>("sp_Assessment", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<int> SaveUserResponseAsync(UserResponse response)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    UserId = response.UserId,
                    QuestionId = response.QuestionId,
                    Response = response.Response,
                    QueryType = "SaveUserResponse"
                }; return await connection.ExecuteAsync("sp_Assessment", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<UserResponse>> GetUserResponsesAsync(int userId)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    UserId = userId,
                    QueryType = "GetUserResponses"
                }; return await connection.QueryAsync<UserResponse>("sp_Assessment", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<Guidance>> GetGuidanceRecommendationsAsync(int userId)
        {
            using (var connection = new SqlConnection(conn))
            {
                var parameters = new
                {
                    UserId = userId,
                    QueryType = "GetGuidanceRecommendations"
                }; return await connection.QueryAsync<Guidance>("sp_Assessment", parameters, commandType: CommandType.StoredProcedure);
            }
        }


    }
}
