//-----------------------------------------------------------------------
// <copyright file="RegisterUserResourseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.UserAuth.ResourseModel
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    public class RegisterUserResourseModel
    {
        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}