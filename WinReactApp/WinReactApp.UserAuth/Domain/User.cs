namespace WinReactApp.UserAuth.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class User
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public string CreatedIpAddress { get; set; }

        public long? CreatedLogUserDeviceInfoId { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string ModifiedIpAddress { get; set; }

        public long? ModifiedLogUserDeviceInfoId { get; set; }
    }
}