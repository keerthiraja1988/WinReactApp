using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WinReactApp.UserAuth.ResourseModel
{
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