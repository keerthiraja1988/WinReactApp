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
    using WinReactApp.ManageUsers.Models;
    using WinReactApp.ManageUsers.ResourseModel;

    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/Address")]
    public class AddressController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly ILogger<AddressController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _configuration;

        private readonly DBContext _context;

        public AddressController(IMapper mapper, ILogger<AddressController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DBContext context)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._logger = logger;
            this._configuration = configuration;
            this._context = context;
        }

        #region Manage Address

        [Route("AddAddress")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> AddAddressAsync_v1_x(AddAddressResourseModel addAddressRM)
        {
            UserAddress userAddress = new UserAddress();

            userAddress = this._mapper.Map<UserAddress>(addAddressRM);

            var userId = Convert.ToInt64(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            userAddress.CreatedBy = userId;
            userAddress.ModifiedBy = userId;

            userAddress.CreatedIpAddress = this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            userAddress.ModifiedIpAddress = this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            userAddress.CreatedOn = DateTime.Now;
            userAddress.ModifiedOn = DateTime.Now;

            userAddress.IsActive = true;

            await this._context.UserAddresses.AddAsync(userAddress);

            this._context.SaveChanges();

            return this.Ok("Address added successfully");
        }

        #endregion Manage Address
    }
}