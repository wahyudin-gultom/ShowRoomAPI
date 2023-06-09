using System.Security.Claims;

namespace ShowRoomAPI
{
    public interface IJwtBearerManager
    {
        string GenerateToken();

        ClaimsPrincipal GetAuthTokenResult(string token);
    }
}
