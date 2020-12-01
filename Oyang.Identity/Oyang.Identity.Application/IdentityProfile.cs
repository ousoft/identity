using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Oyang.Identity.Domain.Entities;
using Oyang.Identity.IApplication.User;
using Oyang.Identity.IApplication.User.Dtos;
using Oyang.Identity.IApplication.Role;
using Oyang.Identity.IApplication.Role.Dtos;

namespace Oyang.Identity.Application
{
    public class IdentityProfile: Profile
    {
        public IdentityProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<IApplication.User.Dtos.AddInputDto, UserEntity>();
            CreateMap<IApplication.User.Dtos.UpdateInputDto, UserEntity>();

            CreateMap<RoleEntity, RoleDto>();
            CreateMap<IApplication.Role.Dtos.AddInputDto, RoleEntity>();
            CreateMap<IApplication.Role.Dtos.UpdateInputDto, RoleEntity>();
        }
    }
}
