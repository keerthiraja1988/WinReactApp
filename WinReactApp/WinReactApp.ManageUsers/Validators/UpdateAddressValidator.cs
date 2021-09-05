namespace WinReactApp.ManageUsers.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentValidation;
    using WinReactApp.ManageUsers.Models;
    using WinReactApp.ManageUsers.ResourseModel;

    public class UpdateAddressValidator : AbstractValidator<UpdateAddressResourseModel>
    {
        private readonly DBContext _context;

        public UpdateAddressValidator(DBContext context)
        {
            this._context = context;

            this.RuleFor(x => x.UserAddressId).Cascade(CascadeMode.Stop)
                     .NotNull().WithMessage("Invalid User Address Id.")
                     .Must(x => x > 0 && x < long.MaxValue).WithMessage("User Address Id should between 0 to " + long.MaxValue)
                      .Must((fooArgs, userAddressId) =>
                        this.UserAddressIdExistsForUserId(fooArgs.UserId, fooArgs.UserAddressId))
                        .WithMessage("User Address Id not found for User Id.");

            this.RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
                     .NotNull().WithMessage("Invalid User Id.")
                     .Must(x => x > 0 && x < long.MaxValue).WithMessage("User Id should between 0 to " + long.MaxValue)
                     .Must(this.UserIdExists).WithMessage("User Id not found.");

            this.RuleFor(x => x.AddressTypeId).Cascade(CascadeMode.Stop)
                         .NotNull().WithMessage("Please provide valid Address Type Id.")
                         .Must(x => x > 0 && x < int.MaxValue).WithMessage("Address Type Id should between 0 to " + int.MaxValue)
                         .Must(this.AddressTypeExists).WithMessage("Address Type Id not found.")
                         .DependentRules(() =>
                         {
                             this.RuleFor(x => x.AddressName).Cascade(CascadeMode.Stop)
                                 .NotNull().Length(6, 30)
                                 .Must((fooArgs, userId, userAddressId) =>
                                        this.AddressNameExists(fooArgs.AddressName, fooArgs.UserId, fooArgs.UserAddressId))
                                        .WithMessage("Address Name already exists for the user.");
                         });

            this.RuleFor(x => x.CountryId).Cascade(CascadeMode.Stop)
                  .NotNull().WithMessage("Please provide valid Country Id.")
                  .Must(x => x > 0 && x < int.MaxValue).WithMessage("Country Id should between 0 to " + int.MaxValue)
                  .Must(this.CountryExists).WithMessage("Country Id not found.");

            this.RuleFor(x => x.MobileNumber).NotNull().Length(9, 30);

            this.RuleFor(x => x.Pincode).NotNull().Length(5, 30);

            this.RuleFor(x => x.HouseNumber).NotNull().Length(1, 35);

            this.RuleFor(x => x.AddressLine1).NotNull().Length(5, 35);

            this.RuleFor(x => x.AddressLine2).NotNull().Length(5, 35);

            this.RuleFor(x => x.Landmark).NotNull().Length(4, 35);

            this.RuleFor(x => x.TownOrCityName).NotNull().Length(4, 35);

            this.RuleFor(x => x.StateName).NotNull().Length(4, 35);
        }

        private bool UserAddressIdExistsForUserId(long userId, long userAddressId)
        {
            if (this._context.UserAddresses.Any(x => x.UserAddressId == userAddressId && x.UserId == userId && x.IsActive == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AddressNameExists(string addressName, long userId, long userAddressId)
        {
            if (this._context.UserAddresses.Any(x => x.AddressName == addressName
                                                    && x.UserId == userId
                                                    && x.UserAddressId != userAddressId
                                                    && x.AddressName == addressName
                                                    && x.IsActive == true))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool UserIdExists(long userId)
        {
            if (this._context.Users.Any(x => x.UserId == userId && x.IsActive == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CountryExists(int countryId)
        {
            if (this._context.Countries.Any(x => x.CountryId == countryId && x.IsActive == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AddressTypeExists(int addressTypeId)
        {
            if (this._context.AddressTypes.Any(x => x.AddressTypeId == addressTypeId && x.IsActive == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}