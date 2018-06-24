namespace Azure.Repository.Interfaces
{
    public interface IUserRepository
    {
        bool HasValidCredentials(string userName, string password);
        string GetRoleByUserName(string userName);
    }
}
