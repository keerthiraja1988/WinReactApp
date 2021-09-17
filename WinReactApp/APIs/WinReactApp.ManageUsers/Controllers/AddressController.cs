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
    [Route("api/Address")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [Route("GetAllAddresses")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserAddress>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
        public async Task<IActionResult> GetAllAddressesAsync_v1_x()
        {
            List<UserAddress> userAddresses = new List<UserAddress>();

            userAddresses = await this._context.UserAddresses.Where(x => x.IsActive == true).ToListAsync();

            return this.Ok(userAddresses);
        }

        [Route("GetAddressesForUser")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserAddress>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAddressesForUserAsync_v1_x(long userId)
        {
            if (userId < 0)
            {
                return this.BadRequest("Please provide User Id.");
            }

            var userAddresses = await this._context.UserAddresses.Where(x => x.UserId == userId && x.IsActive == true).ToListAsync();

            if (userAddresses != null && userAddresses.Count > 0)
            {
                return this.Ok(userAddresses);
            }
            else
            {
                return this.NoContent();
            }
        }

        [Route("GetAddresses")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserAddress>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAddressesAsync_v1_x(long userAddressId)
        {
            if (userAddressId < 0)
            {
                return this.BadRequest("Please provide User Address Id.");
            }

            var userAddresses = await this._context.UserAddresses.Where(x => x.UserAddressId == userAddressId && x.IsActive == true).ToListAsync();

            if (userAddresses != null && userAddresses.Count > 0)
            {
                return this.Ok(userAddresses);
            }
            else
            {
                return this.NoContent();
            }
        }

        [Route("AddAddress")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseAddressResourseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
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

            long userAddressId = userAddress.UserAddressId;

            return this.Ok(new ResponseAddressResourseModel { UserAddressId = userAddressId, Message = "Address added successfully" });
        }

        [Route("UpdateAddress")]
        [HttpPut]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseAddressResourseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
        public async Task<IActionResult> UpdateAddressAsync_v1_x(UpdateAddressResourseModel updateAddressRM)
        {
            var userAddress = await this._context.UserAddresses
                           .Where(x => x.UserAddressId == updateAddressRM.UserAddressId && x.IsActive == true)
                           .FirstOrDefaultAsync();

            userAddress.AddressTypeId = updateAddressRM.AddressTypeId;
            userAddress.CountryId = updateAddressRM.CountryId;
            userAddress.AddressName = updateAddressRM.AddressName;
            userAddress.Pincode = updateAddressRM.Pincode;
            userAddress.HouseNumber = updateAddressRM.HouseNumber;
            userAddress.MobileNumber = updateAddressRM.MobileNumber;
            userAddress.AddressLine1 = updateAddressRM.AddressLine1;
            userAddress.AddressLine2 = updateAddressRM.AddressLine2;
            userAddress.Landmark = updateAddressRM.Landmark;
            userAddress.TownOrCityName = updateAddressRM.TownOrCityName;
            userAddress.StateName = updateAddressRM.StateName;

            var userId = Convert.ToInt64(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            userAddress.ModifiedBy = userId;

            userAddress.ModifiedIpAddress = this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            userAddress.ModifiedOn = DateTime.Now;

            this._context.UserAddresses.Update(userAddress);

            await this._context.SaveChangesAsync();

            return this.Ok(new ResponseAddressResourseModel { UserAddressId = userAddress.UserAddressId, Message = "Address updated successfully" });
        }

        [Route("DeleteAddress")]
        [HttpDelete]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseAddressResourseModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAddressAsync_v1_x(long userAddressId)
        {
            if (userAddressId > 0)
            {
            }
            else
            {
                return this.BadRequest("Please provide valid User Address Id.");
            }

            var userAddress = await this._context.UserAddresses
                           .Where(x => x.UserAddressId == userAddressId && x.IsActive == true)
                           .FirstOrDefaultAsync();

            if (userAddress == null)
            {
                return this.NoContent();
            }

            this._context.UserAddresses.Remove(userAddress);

            await this._context.SaveChangesAsync();

            return this.Ok(new ResponseAddressResourseModel { UserAddressId = userAddress.UserAddressId, Message = "Address deleted successfully" });
        }

        #endregion Manage Address

        #region Address Types

        [Route("GetCountries")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Country>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetCountriesAsync_v1_x()
        {
            List<Country> countries = new List<Country>();

            countries = await this._context.Countries.Where(x => x.IsActive == true).ToListAsync();

            if (countries != null && countries.Count > 0)
            {
                return this.Ok(countries);
            }
            else
            {
                return this.NoContent();
            }
        }

        [Route("GetAddressTypes")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressTypes>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAddressTypesAsync_v1_x()
        {
            List<AddressTypes> addressTypes = new List<AddressTypes>();

            addressTypes = await this._context.AddressTypes.Where(x => x.IsActive == true).ToListAsync();

            if (addressTypes != null && addressTypes.Count > 0)
            {
                return this.Ok(addressTypes);
            }
            else
            {
                return this.NoContent();
            }
        }

        #endregion Address Types
    }
}