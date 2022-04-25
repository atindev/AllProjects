using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WAS.Application.Common.Profile
{
    public class ADUser: AutoMapper.Profile
    {
        public ADUser()
        {
            CreateMap<Microsoft.Graph.User, Models.ADUser>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.GivenName))
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => src.Surname))
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DisplayName) ? src.UserPrincipalName : src.DisplayName))
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => src.UserPrincipalName))
                .ForMember(
                    dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(
                    dest => dest.Designation,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(
                    dest => dest.BusinessPhones,
                    opt => opt.MapFrom(src => PhoneValidation(src.BusinessPhones.FirstOrDefault())))
                .ForMember(
                    dest => dest.MobilePhone,
                    opt => opt.MapFrom(src => PhoneValidation(src.MobilePhone)))
                .ForMember(
                    dest => dest.Location,
                    opt => opt.MapFrom(src => src.OfficeLocation))
                .ForMember(
                    dest => dest.UserPrincipalName,
                    opt => opt.MapFrom(src => src.UserPrincipalName))
                .ForMember(
                    dest => dest.PostalCode,
                    opt => opt.MapFrom(src => PhoneValidation(src.PostalCode)))
                .ForMember(
                    dest => dest.EmployeeType,
                    opt => opt.MapFrom(src => src.OnPremisesExtensionAttributes.ExtensionAttribute3))
                .ForMember(
                    dest => dest.EmployeeGroup,
                    opt => opt.MapFrom(src => src.OnPremisesExtensionAttributes.ExtensionAttribute4))
                .ForMember(
                    dest => dest.CostCenter,
                    opt => opt.MapFrom(src => src.OnPremisesExtensionAttributes.ExtensionAttribute10))
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.OnPremisesSamAccountName))
                ;
        }

        private static string PhoneValidation(string phoneNumber)
        {
            if (!String.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            }
            return phoneNumber;
        }
    }
}
