namespace WinReactApp.UserAuth.ResourseModel
{
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;

    public class LoginUserResourseModel
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}