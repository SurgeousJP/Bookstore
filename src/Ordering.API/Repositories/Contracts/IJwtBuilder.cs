namespace Ordering.API.Repositories.Contracts
{
    public interface IJwtBuilder
    {
        string GetToken(string userId);
        string ValidateToken(string token);
    }
}
