using AutoMapper;
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
        }
    }
}
