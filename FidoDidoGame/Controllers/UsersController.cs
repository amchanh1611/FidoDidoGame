using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Modules.Users.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;
using Hangfire;

namespace FidoDidoGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IBackgroundJobClient hangfire;

        public UsersController(IUserService service, IBackgroundJobClient hangfire)
        {
            this.service = service;
            this.hangfire = hangfire;
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
        [HttpGet("Profile"), Authorize]
        public IActionResult Profile([FromQuery] ProfilesRequest request)
        {
            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.Profile(userId));
        }

        [HttpGet("FacebookOauth")]
        public IActionResult FacbookLogin()
        {
            AuthenticationProperties properties = new () { RedirectUri = Url.Action("FaceBookRedirect") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }
        [HttpGet("FacebookOauthRedirect")]
        public async Task<IActionResult> FaceBookRedirectAsync()
        {
            AuthenticateResult authResult = await Request.HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            long userId = long.Parse(authResult.Principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            #region if user doestn't exists, create new user 
            if (!service.FindById(userId))
            {
                string accessToken = authResult.Properties!.GetTokenValue("access_token")!;

                #region Get Picture 
                using HttpClient client = new();

                HttpResponseMessage pictureResponse = await client.GetAsync($"{FacebookDefaults.UserInformationEndpoint}/picture?redirect&access_token={accessToken}&height=720");

                string pictureContent = await pictureResponse.Content.ReadAsStringAsync();
                PictureInfo picture = JsonSerializer.Deserialize<PictureInfo>(pictureContent)!;
                #endregion

                CreateUserRequest request = new()
                {
                    Id = userId,
                    Name = authResult.Principal!.FindFirst(ClaimTypes.Name)!.Value,
                    NickName = authResult.Principal!.FindFirst(ClaimTypes.Name)!.Value,
                    Avatar = picture.Data!.Url
                };

                service.Create(request);
            }
            #endregion

            return Ok(service.JwtGenerateToken(userId));
        }
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            return Ok(service.RefreshToken(refreshToken));
        }
        [HttpGet("Reward")]
        public IActionResult GetReward()
        {
            
            service.GetReward();
            return Ok();
        }
    }
}
