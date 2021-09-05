namespace WinReactApp.UserAuth.Validators
{
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WinReactApp.UserAuth.Extensions.Custom;
    using WinReactApp.UserAuth.Repository;
    using WinReactApp.UserAuth.ResourseModel;

    public class LoginUserValidator : AbstractValidator<LoginUserResourseModel>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.EmailAddress).NotNull()
                    .EmailAddress().WithMessage("Please provide valid Email Address.");

            RuleFor(x => x.Password).NotNull();
        }
    }
}