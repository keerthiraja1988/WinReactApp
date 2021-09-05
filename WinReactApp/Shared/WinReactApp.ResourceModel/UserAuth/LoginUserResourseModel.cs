//-----------------------------------------------------------------------
// <copyright file="LoginUserResourseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.ResourceModel.UserAuth
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserResourseModel
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}