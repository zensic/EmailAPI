using EmailAPI.TokenAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmailAPI.Filters
{
  public class TokenAuthFilter : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // Grab token manager from context
      var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

      var result = true;

      // Check whether the header has authorization key
      if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        result = false;

      string token = string.Empty;
      if (result)
      {
        // Verify the authorization token, set result to false if fails
        token = context.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value;
        if (!tokenManager.VerifyToken(token))
          result = false;
      }

      // If authorization key or token verification fails, let them know that they're not authorized
      if (!result)
      {
        context.ModelState.AddModelError("Unauthorized", "You are not authorized.");
        context.Result = new UnauthorizedObjectResult(context.ModelState);
      }
    }
  }
}
