//-----------------------------------------------------------------------
// <copyright file="AddAddressResourseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.ResourceModel.ManageUsers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GetAddressResourseModel
    {
        public long UserId { get; set; }

        public int AddressTypeId { get; set; }

        public int CountryId { get; set; }

        public string AddressName { get; set; }

        public string MobileNumber { get; set; }

        public string Pincode { get; set; }

        public string HouseNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Landmark { get; set; }

        public string TownOrCityName { get; set; }

        public string StateName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public string CreatedIpAddress { get; set; }

        public long? CreatedLogUserDeviceInfoId { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string ModifiedIpAddress { get; set; }

        public long? ModifiedLogUserDeviceInfoId { get; set; }
    }
}