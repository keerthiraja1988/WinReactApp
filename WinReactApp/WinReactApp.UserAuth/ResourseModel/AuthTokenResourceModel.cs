namespace WinReactApp.UserAuth.ResourseModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthTokenResourceModel
    {
        public string AuthToken { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpireOn { get; set; }
    }
}