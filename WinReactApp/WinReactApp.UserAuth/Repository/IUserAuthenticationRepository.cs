namespace WinReactApp.UserAuth.Repository
{
    using Insight.Database;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WinReactApp.UserAuth.Domain;

    public interface IUserAuthenticationRepository
    {
        [Sql("[dbo].[P_RegisterUser]")]
        Task<long> RegisterUserAsync(User User);

        [Sql(@"SELECT ISNULL(COUNT(*),0)
                FROM  [dbo].[User]
        WHERE EmailAddress = @emailAddress")]
        int IsEmailAddressExists(string emailAddress);

        [Sql(@"SELECT ISNULL(COUNT(*),0)
                FROM  [dbo].[User]
        WHERE UserName = @userName")]
        int IsUserNameExists(string userName);
    }
}