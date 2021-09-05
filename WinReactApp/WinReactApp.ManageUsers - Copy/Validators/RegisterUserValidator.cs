//-----------------------------------------------------------------------
// <copyright file="RegisterUserValidator.cs" company="PlaceholderCompany">
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
    using WinReactApp.ManageUsers.Repository;
    using WinReactApp.ManageUsers.ResourseModel;

    public class RegisterUserValidator : AbstractValidator<RegisterUserResourseModel>
    {
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;

        public RegisterUserValidator(IUserAuthenticationRepository userAuthenticationRepository)
        {
            this._userAuthenticationRepository = userAuthenticationRepository;

            this.RuleFor(x => x.UserName).NotNull().Length(6, 20)
                  .Must(this.UserNameExist).WithMessage("User Name is already registered with us.");
            this.RuleFor(x => x.EmailAddress).NotNull()
                    .EmailAddress().WithMessage("Please provide valid Email Address.")
                    .Must(this.EmailAddressExist).WithMessage("Email Address is already registered with us.");
            this.RuleFor(x => x.FirstName).NotNull().Length(6, 20);
            this.RuleFor(x => x.LastName).NotNull().Length(6, 20);
            this.RuleFor(x => x.Password).NotNull().Length(8, 30).Must(x => CryptographyExtensions.HasValidPassword(x)).WithMessage(@"Your password does not meet the requirements!!
                           <br>Password should contains a lowercase.
                           <br>Should contains a uppercase.
                           <br>Should contains a number.
                           <br>Should contains a special character(eg. ! @ # $ % &");

            this.RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("Your Passwords do not match.");
        }

        private bool EmailAddressExist(string emailAddress)
        {
            var count = this._userAuthenticationRepository.IsEmailAddressExists(emailAddress);

            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool UserNameExist(string userName)
        {
            var count = this._userAuthenticationRepository.IsUserNameExists(userName);

            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}