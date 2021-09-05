namespace WinReactApp.UserAuth.Extensions.AutoMapper
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using global::AutoMapper;

    using WinReactApp.UserAuth.Domain;

    using WinReactApp.UserAuth.ResourseModel;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "Reviewed")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Reviewed")]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            AllowNullDestinationValues = true;

            #region Authentication

            CreateMap<RegisterUserResourseModel, User>().ReverseMap();

            #endregion Authentication
        }
    }
}