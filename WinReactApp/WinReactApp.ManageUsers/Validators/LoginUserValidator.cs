//-----------------------------------------------------------------------
// <copyright file="LoginUserValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.ManageUsers.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentValidation;
    using WinReactApp.ManageUsers.Extensions.Custom;
    using WinReactApp.ManageUsers.ResourseModel;

    public class LoginUserValidator : AbstractValidator<LoginUserResourseModel>
    {
        public LoginUserValidator()
        {
            this.RuleFor(x => x.EmailAddress).NotNull()
                    .EmailAddress().WithMessage("Please provide valid Email Address.");

            this.RuleFor(x => x.Password).NotNull();
        }
    }
}