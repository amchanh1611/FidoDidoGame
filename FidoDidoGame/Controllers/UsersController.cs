using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Modules.Users.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FidoDidoGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            service.Create(request);
            return Ok();
        }
        [HttpPut("{userId}")]
        public IActionResult Update([FromRoute] int userId, [FromBody] UpdateUserRequest request)
        {
            service.Update(userId, request);
            return Ok();
        }
        [HttpGet("Profile/{userId}")]
        public IActionResult Profile([FromRoute] int userId, [FromQuery] ProfilesRequest request)
        {
            return Ok(service.Profile(userId));
        }
        [HttpGet("FacebookOauth")]
        public async Task<IActionResult> FacbookLoginAsync()
        {
            string? authScheme = FacebookDefaults.AuthenticationScheme;

            // Try to authenticate.
            AuthenticateResult authResult = await Request.HttpContext.AuthenticateAsync(authScheme);
            if (!authResult.Succeeded
                || authResult?.Principal == null
                || !authResult.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(authResult.Properties.GetTokenValue("access_token")))
            {
                // Challenge the Facebook OAuth 2.0 provider.
                await Request.HttpContext.ChallengeAsync(authScheme, new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    IssuedUtc = DateTimeOffset.UtcNow,
                    // We provide this API's own path here so that the final redirection can go
                    // to this method.
                    RedirectUri = Url.Action("FacbookLoginAsync")
                });

                // Get the location response header.
                if (Response.Headers.TryGetValue("location", out var locationResponseHeader))
                {
                    return Redirect(locationResponseHeader);
                }
                return Unauthorized();
            }
            return Ok();
        }
    }
}
