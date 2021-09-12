namespace WinReactApp.ResourceModel.UserAuth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserDetailsResourceModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime Expiration { get; set; }

        public string Jti { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}