//-----------------------------------------------------------------------
// <copyright file="AddressController.cs" company="PlaceholderCompany">
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
    using System.Net.Mime;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using WinReactApp.ManageUsers.Extensions.Custom;
    using WinReactApp.ManageUsers.Extensions.Filters;
    using WinReactApp.ManageUsers.Models;
    using WinReactApp.ManageUsers.Validators;
    using WinReactApp.ResourceModel.ManageUsers;

    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/User")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly ILogger<UserController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _configuration;

        private readonly DBContext _context;

        public UserController(IMapper mapper, ILogger<UserController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DBContext context)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._logger = logger;
            this._configuration = configuration;
            this._context = context;
        }

        #region User Details

        [Route("GetAllUsers")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserAddress>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllUsersAsync_v1_x()
        {
            List<User> users = new List<User>();

            users = await this._context.Users.Where(x => x.IsActive == true).ToListAsync();

            if (users != null && users.Count > 0)
            {
                return this.Ok(users);
            }
            else
            {
                return this.NoContent();
            }
        }

        #endregion User Details
    }
}