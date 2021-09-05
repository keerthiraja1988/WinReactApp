//-----------------------------------------------------------------------
// <copyright file="IUserAuthenticationRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.UserAuth.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Insight.Database;
    using WinReactApp.UserAuth.Domain;

    public interface IUserAuthenticationRepository
    {
        #region register

        [Sql("[dbo].[P_RegisterUser]")]
        Task<long> RegisterUserAsync(User user);

        [Sql(@"SELECT ISNULL(COUNT(*),0)
                FROM  [dbo].[User]
        WHERE EmailAddress = @emailAddress")]
        int IsEmailAddressExists(string emailAddress);

        [Sql(@"SELECT ISNULL(COUNT(*),0)
                FROM  [dbo].[User]
        WHERE UserName = @userName")]
        int IsUserNameExists(string userName);

        #endregion register

        #region Login

        [Sql(@"SELECT *
              FROM [dbo].[User]
              WHERE EmailAddress = @emailAddress")]
        Task<User> GetUserDetailsAsync(string emailAddress);

        #endregion Login
    }
}