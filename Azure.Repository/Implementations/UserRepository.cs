namespace Azure.Repository.Implementations
{
    using Interfaces;

    public class UserRepository : IUserRepository
    {

        public bool HasValidCredentials(string userName, string password)
        {
            return (userName == "admin" && password == "admin") ||
                   (userName == "testuser" && password == "testuser");
        }

        public string GetRoleByUserName(string userName)
        {
            return userName == "admin" ? "admin" : "member";
        }
    }
}
