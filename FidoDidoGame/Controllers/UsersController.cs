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
using FidoDidoGame.Modules.Ranks.Services;
using FidoDidoGame.Modules.Rank.Entities;

namespace FidoDidoGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IBackgroundJobClient hangfire;
        private readonly IRankService rankService;

        public UsersController(IUserService service, IBackgroundJobClient hangfire, IRankService rankService)
        {
            this.service = service;
            this.hangfire = hangfire;
            this.rankService = rankService;
        }

        [HttpPost]
        [Authorize(Roles = "Develop")]
        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            service.Create(request);
            return Ok();
        }

        [HttpPut("{userId}"), Authorize]
        public IActionResult Update([FromBody] UpdateUserRequest request)
        {
            long userId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
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
                    Avatar = picture.Data!.Url,
                    RoleId = 3
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
        [Authorize(Roles = "Admin")]
        public IActionResult GetReward()
        {
            List<Event> events = rankService.GetEvent();

            foreach(Event item in events) 
            {
                hangfire.Schedule(() => service.GetReward(item.Id), item.DateEnd);
            }

            return Ok();
        }

        [HttpPost("Role")]
        [Authorize(Roles = "Develop")]
        public IActionResult CreateRole(CreateRoleRequest request)
        {
            service.CreateRole(request);
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequest request)
        {
            return Ok(service.Login(request));
        }
    }
}
