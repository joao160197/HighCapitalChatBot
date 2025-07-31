using Microsoft.AspNetCore.Authorization;

namespace HighCapitalBot.API.Filters;

public class JwtAuthorizeAttribute : AuthorizeAttribute
{
    public JwtAuthorizeAttribute()
    {
        AuthenticationSchemes = "Bearer";
    }
}
