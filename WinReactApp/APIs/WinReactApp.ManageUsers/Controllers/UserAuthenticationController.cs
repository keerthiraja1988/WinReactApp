//-----------------------------------------------------------------------
// <copyright file="UserAuthenticationController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.ManageUsers.Controllers
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
    using WinReactApp.ManageUsers.Extensions.Custom;
    using WinReactApp.ManageUsers.Extensions.Filters;

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

        public UserAuthenticationController(IMapper mapper, ILogger<UserAuthenticationController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._logger = logger;
            this._configuration = configuration;
        }

        #region Authentication

        #region Validate Authentication

        [Route("ValidateAuthentication")]
        [HttpGet]
        [Authorize]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IsUserAdministrator_v1_x()
        {
            return this.Ok();
        }

        [Route("IsUserAdmin1")]
        [HttpGet]
        [Authorize(Roles = "Administrator1")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IsUserAdministrator1_v1_x()
        {
            return this.Ok();
        }

        #endregion Validate Authentication

        #endregion Authentication
    }
}