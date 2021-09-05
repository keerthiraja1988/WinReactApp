//-----------------------------------------------------------------------
// <copyright file="AuthTokenResourceModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.ResourceModel.UserAuth
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

        public string ErrorMessage { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}