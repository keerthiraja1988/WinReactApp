namespace WinReactApp.UserAuth.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WinReactApp.UserAuth.Domain;
    using WinReactApp.UserAuth.Extensions.Custom;
    using WinReactApp.UserAuth.Extensions.Filters;
    using WinReactApp.UserAuth.Repository;
    using WinReactApp.UserAuth.ResourseModel;

    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/UserAuth")]
    public class UserAuthenticationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IMapper _mapper;

        private readonly ILogger<UserAuthenticationController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserAuthenticationRepository _userAuthenticationRepository;

        public UserAuthenticationController(IMapper mapper, ILogger<UserAuthenticationController> logger, IHttpContextAccessor httpContextAccessor, IUserAuthenticationRepository userAuthenticationRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._logger = logger;
            this._userAuthenticationRepository = userAuthenticationRepository;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("Weather")]
        public IEnumerable<WeatherForecast> GetWeatherV1_0()
        {
            {
                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            }
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
        [Route("Weather")]
        public IEnumerable<WeatherForecast> GetWeatherV1_1()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        #region register

        [Route("registeruser")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUserAsync_v1_0([FromBody] RegisterUserResourseModel registerUserRM)
        {
            User user = new User();

            user = this._mapper.Map<User>(registerUserRM);

            user.CreatedIpAddress = this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            user.PasswordSalt = StringExtensions.CreateSalt();

            user.PasswordHash = CryptographyExtensions.CreateHash(registerUserRM.Password, user.PasswordSalt);

            var userId = await this._userAuthenticationRepository.RegisterUserAsync(user);
            if (userId == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok("User Registered Sucessfully!");
            }
        }

        //[HttpGet("IsEmailIdExists")]
        //[AllowAnonymous]
        //public async Task<IActionResult> IsEmailIdExistsAsync(string emailId)
        //{
        //    emailId = emailId.Trim().ToLower();

        //    bool isEmailIdExists = false;

        //    isEmailIdExists = await this._authenticationService.IsEmailIdExistsAsync(emailId);

        //    if (isEmailIdExists)
        //    {
        //        return this.Json(false);
        //    }
        //    else
        //    {
        //        return this.Json(true);
        //    }
        //}

        #endregion register
    }
}