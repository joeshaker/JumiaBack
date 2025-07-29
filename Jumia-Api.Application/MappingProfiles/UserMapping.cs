using AutoMapper;
using Jumia_Api.Application.Dtos.CustomerDtos;
using Jumia_Api.Application.Dtos.UserDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.MappingProfiles
{
    public class UserMapping:Profile
    {
        public UserMapping()
        {
            CreateMap<AppUser, UserProfileDto>();
            CreateMap<UpdateUserDto, AppUser>();
            CreateMap<Customer, CustomerDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender));
        }
    }
}
