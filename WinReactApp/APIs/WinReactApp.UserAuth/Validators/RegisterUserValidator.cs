//-----------------------------------------------------------------------
// <copyright file="RegisterUserValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.UserAuth.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using FluentValidation;
    using WinReactApp.ResourceModel.UserAuth;
    using WinReactApp.UserAuth.Extensions.Custom;
    using WinReactApp.UserAuth.Repository;

    public class RegisterUserValidator : AbstractValidator<RegisterUserResourseModel>
    {
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;

        public RegisterUserValidator(IUserAuthenticationRepository userAuthenticationRepository)
        {
            this._userAuthenticationRepository = userAuthenticationRepository;

            this.RuleFor(x => x.UserName).Cascade(CascadeMode.Stop)
                  .NotNull().Length(6, 20)
                  .Must(this.UserNameExist).WithMessage("User Name is already registered with us.");

            this.RuleFor(x => x.EmailAddress).Cascade(CascadeMode.Stop)
                     .NotNull()
                    .EmailAddress().WithMessage("Please provide valid Email Address.")
                    .Must(this.EmailAddressExist).WithMessage("Email Address is already registered with us.");

            this.RuleFor(x => x.FirstName).NotNull().Length(6, 20);
            this.RuleFor(x => x.LastName).NotNull().Length(6, 20);
            this.RuleFor(x => x.Password)
                            .NotNull()
                            .Length(8, 30)
                            .Must(this.PasswordHasLowercase).WithMessage("Password should contains a lowercase.")
                            .Must(this.PasswordHasUppercase).WithMessage("Password should contains a uppercase.")
                            .Must(this.PasswordHasDigit).WithMessage("Password should contains a number.")
                            .Must(this.PasswordHasSymbol).WithMessage("Should contains a special character(eg. ! @ # $ % &.)");

            this.RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("Your Passwords do not match.");
        }

        private bool PasswordHasLowercase(string password)
        {
            var lowercase = new Regex("[a-z]+");

            return lowercase.IsMatch(password);
        }

        private bool PasswordHasUppercase(string password)
        {
            var uppercase = new Regex("[A-Z]+");

            return uppercase.IsMatch(password);
        }

        private bool PasswordHasDigit(string password)
        {
            var digit = new Regex("(\\d)+");

            return digit.IsMatch(password);
        }

        private bool PasswordHasSymbol(string password)
        {
            var symbol = new Regex("(\\W)+");

            return symbol.IsMatch(password);
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