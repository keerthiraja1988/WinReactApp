//-----------------------------------------------------------------------
// <copyright file="UserAuthenticationController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.UserAuth.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
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
        private readonly IMapper _mapper;

        private readonly ILogger<UserAuthenticationController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _configuration;

        private readonly IUserAuthenticationRepository _userAuthenticationRepository;

        public UserAuthenticationController(IMapper mapper, ILogger<UserAuthenticationController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IUserAuthenticationRepository userAuthenticationRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._logger = logger;
            this._configuration = configuration;
            this._userAuthenticationRepository = userAuthenticationRepository;
        }

        #region Authentication

        #region Register User

        [Route("registeruser")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUserAsync_v1_x([FromBody] RegisterUserResourseModel registerUserRM)
        {
            User user = new User();

            user = this._mapper.Map<User>(registerUserRM);

            user.CreatedIpAddress = this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            user.PasswordSalt = StringExtensions.CreateSalt();

            user.PasswordHash = CryptographyExtensions.CreateHash(registerUserRM.Password, user.PasswordSalt);

            var userId = await this._userAuthenticationRepository.RegisterUserAsync(user);
            if (userId == 0)
            {
                return this.BadRequest();
            }
            else
            {
                return this.Ok("User Registered Sucessfully!");
            }
        }

        #endregion Register User

        #region Login User

        [Route("LoginUser")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthTokenResourceModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUserAsync_v1_x([FromBody] LoginUserResourseModel loginUserRM)
        {
            AuthTokenResourceModel authToken = new AuthTokenResourceModel();

            var user = await this._userAuthenticationRepository.GetUserDetailsAsync(loginUserRM.EmailAddress.Trim());

            if (user == null)
            {
                authToken.ErrorMessage = "Email Address or Password is incorrect. Please try again.";
                return this.Unauthorized(authToken);
            }

            var passwordHash = CryptographyExtensions.CreateHash(loginUserRM.Password, user.PasswordSalt);

            if (passwordHash != user.PasswordHash)
            {
                authToken.ErrorMessage = "Email Address or Password is incorrect. Please try again.";
                return this.Unauthorized(authToken);
            }

            var expire = DateTime.Now.AddHours(3);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Expiration, expire.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            authClaims.Add(new Claim(ClaimTypes.Role, "Administrator"));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: this._configuration["JWT:ValidIssuer"],
                audience: this._configuration["JWT:ValidAudience"],
                expires: expire,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            authToken.AuthToken = new JwtSecurityTokenHandler().WriteToken(token);
            authToken.CreatedOn = DateTime.Now;
            authToken.ExpireOn = token.ValidTo;

            authToken.UserName = user.UserName;
            authToken.FirstName = user.FirstName;
            authToken.LastName = user.LastName;
            authToken.EmailAddress = user.EmailAddress;

            return this.Ok(authToken);
        }

        #endregion Login User

        #region Validate Authentication

        [Route("ValidateAuthentication")]
        [HttpGet]
        [Authorize]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        public IActionResult ValidateAuthentication_v1_x()
        {
            return this.Ok();
        }

        [Route("IsUserAdmin")]
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Authorize(Roles = "User")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        public IActionResult IsUserAdministrator_v1_x()
        {
            return this.Ok();
        }

        [Route("IsUserAdmin1")]
        [HttpGet]
        [Authorize(Roles = "Administrator1")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        public IActionResult IsUserAdministrator1_v1_x()
        {
            return this.Ok();
        }

        #endregion Validate Authentication

        #endregion Authentication
    }
}