namespace WinReactApp.ManageUsers.Validators
{
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WinReactApp.ManageUsers.ResourseModel;

    public class AddAddressValidator : AbstractValidator<AddAddressResourseModel>
    {
        public AddAddressValidator()
        {
            this.RuleFor(x => x.AddressTypeId).NotNull()
                         .Must(x => x > 0 && x < 5).WithMessage("Please provide valid Address Type.");

            this.RuleFor(x => x.CountryId).NotNull()
                  .Must(x => x > 0 && x < 50).WithMessage("Please provide valid Country.");

            this.RuleFor(x => x.FullName).NotNull().Length(6, 30);

            this.RuleFor(x => x.MobileNumber).NotNull().Length(9, 30);

            this.RuleFor(x => x.Pincode).NotNull().Length(5, 30);

            this.RuleFor(x => x.HouseNumber).NotNull().Length(1, 35);

            this.RuleFor(x => x.AddressLine1).NotNull().Length(5, 35);

            this.RuleFor(x => x.AddressLine2).NotNull().Length(5, 35);

            this.RuleFor(x => x.Landmark).NotNull().Length(4, 35);

            this.RuleFor(x => x.TownOrCityName).NotNull().Length(4, 35);

            this.RuleFor(x => x.StateName).NotNull().Length(4, 35);
        }
    }
}