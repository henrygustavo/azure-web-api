namespace Azure.Repository.Implementations
{
    using Azure.Repository.Interfaces;

    public class UserRepository : IUserRepository
    {

        public bool HasValidCredentials(string userName, string password)
        {
            return userName == "admin" && password == "admin";
        }
    }
}
