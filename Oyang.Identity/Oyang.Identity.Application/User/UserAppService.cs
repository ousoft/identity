using Oyang.Identity.Domain;
using Oyang.Identity.Domain.Entities;
using Oyang.Identity.IApplication.Database;
using Oyang.Identity.IApplication.User;
using Oyang.Identity.IApplication.User.Dtos;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using Oyang.Identity.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Oyang.Identity.Infrastructure.Expansion;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.Application.User
{
    public class UserAppService : BaseAppService, IUserAppService
    {
        private readonly CurrentUser _currentUser;
        private readonly IdentityDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserAppService(
            CurrentUser currentUser,
            IdentityDbContext dbContext,
            IMapper mapper
            )
        {
            _currentUser = currentUser;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Add(AddInputDto input)
        {
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.LoginName), "登录名不能为空");
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Password), "密码不能为空");
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Password2), "确认密码不能为空");
            ValidationObject.Validate(input.Password != input.Password2, "密码和确认密码不一致");
            var exist = _dbContext.Set<UserEntity>().AsNoTracking().Any(t => t.LoginName == input.LoginName);
            ValidationObject.Validate(exist, "登录名已存在");
            var entity =_mapper.Map<AddInputDto, UserEntity>(input);
            _dbContext.AddWithAudit(entity);
        }

        public Pagination<UserDto> GetList(GetListInputDto input)
        {
            var query = _dbContext.Set<UserEntity>().AsNoTracking();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.LoginName), t => t.LoginName.Contains(input.LoginName));
            var (listEntity, totalCount) = query.ToPagination(input);
            var listDto = _mapper.Map<List<UserEntity>, List<UserDto>>(listEntity);
            var pagination = new Pagination<UserDto>(input, totalCount, listDto);
            return pagination;
        }

        public void Remove(Guid id)
        {
            //_dbContext.Set<UserEntity>().Remove(id);
        }

        public void ResetPassword(ResetPasswordInputDto input)
        {
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Password), "密码不能为空");
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Password2), "确认密码不能为空");
            ValidationObject.Validate(input.Password != input.Password2, "密码和确认密码不一致");
            var entity = _dbContext.Find<UserEntity>(input.Id);
            entity.PasswordHash = HashAlgorithmHelper.Create(input.Password, HashMode.MD5); ;
        }

        public void SetRole(SetRoleInputDto input)
        {
            var list = _dbContext.Set<UserRoleEntity>().Where(t => t.UserId == input.UserId).ToList();
            var listRemove = list.Where(t => !input.RoleIds.Contains(t.RoleId)).ToArray();
            _dbContext.RemoveRange(listRemove);

            var listExistRoleId = list.Select(t => t.RoleId).ToList();
            var listAdd = input.RoleIds.Where(t => !listExistRoleId.Contains(t))
                .Select(t => new UserRoleEntity()
                {
                    Id = Guid.NewGuid(),
                    UserId = input.UserId,
                    RoleId = t,
                });
            _dbContext.AddRange(listAdd);
        }

        public void Update(UpdateInputDto input)
        {
            var entity = _dbContext.Find<UserEntity>(input.Id);
            _mapper.Map(input, entity);
            _dbContext.Add(entity);
        }
    }
}
