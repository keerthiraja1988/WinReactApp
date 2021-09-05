//-----------------------------------------------------------------------
// <copyright file="LoginUserResourseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.UserAuth.ResourseModel
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    public class LoginUserResourseModel
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}