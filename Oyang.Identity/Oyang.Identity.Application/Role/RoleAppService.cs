using Oyang.Identity.Domain;
using System;
using System.Linq;
using Oyang.Identity.Domain.Entities;
using System.Collections.Generic;
using Oyang.Identity.IApplication.Role;
using Oyang.Identity.IApplication.Role.Dtos;
using Oyang.Identity.Infrastructure.Common;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using AutoMapper;
using Oyang.Identity.Infrastructure.Utility;
using Oyang.Identity.Infrastructure.Expansion;
using Microsoft.EntityFrameworkCore;

namespace Oyang.Identity.Application.Role
{
    public class RoleAppService : BaseAppService, IRoleAppService
    {
        private readonly CurrentUser _currentUser;
        private readonly IdentityDbContext _dbContext;
        private readonly IMapper _mapper;
        public RoleAppService(
            CurrentUser currentUser,
            IdentityDbContext dbContext,
            IMapper mapper
            )
        {
            _currentUser = currentUser;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public RoleDto Get(Guid id)
        {
            var entity = _dbContext.Set<RoleEntity>().Find(id);
            var dto = _mapper.Map<RoleEntity, RoleDto>(entity);
            return dto;
        }

        public Pagination<RoleDto> GetList(GetListInputDto input)
        {
            var query = _dbContext.Set<RoleEntity>().AsNoTracking();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Name), t => t.Name.Contains(input.Name));
            var (listEntity, totalCount) = query.ToPagination(input);
            var listDto = _mapper.Map<List<RoleEntity>, List<RoleDto>>(listEntity);
            var pagination = new Pagination<RoleDto>(input, totalCount, listDto);
            return pagination;
        }
        public void Add(AddInputDto input)
        {
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Name), "名称不能为空");
            var exist = _dbContext.Set<RoleEntity>().AsNoTracking().Any(t => t.Name == input.Name);
            ValidationObject.Validate(exist, "名称已存在");
            var entity = _mapper.Map<AddInputDto, RoleEntity>(input);
            _dbContext.AddWithAudit(entity);
        }

        public void Remove(Guid id)
        {
            _dbContext.RemoveWithAudit<RoleEntity>(id);
        }

        public void SetPermission(SetPermissionInputDto input)
        {
            var list = _dbContext.Set<RolePermissionEntity>().Where(t => t.RoleId == input.RoleId).ToList();
            var listRemove = list.Where(t => !input.PermissionIds.Contains(t.PermissionId)).ToArray();
            _dbContext.RemoveWithAudit(listRemove);

            var listExistPermissionId = list.Select(t => t.PermissionId).ToList();
            var listAdd = input.PermissionIds.Where(t => !listExistPermissionId.Contains(t))
                .Select(t => new RolePermissionEntity()
                {
                    RoleId = input.RoleId,
                    PermissionId = t,
                });
            _dbContext.AddWithAudit(listAdd);
        }

        public void SetUser(SetUserInputDto input)
        {
            var list = _dbContext.Set<UserRoleEntity>().Where(t => t.RoleId == input.RoleId).ToList();
            var listRemove = list.Where(t => !input.UserIds.Contains(t.UserId)).ToArray();
            _dbContext.RemoveWithAudit(listRemove);

            var listExistUserId = list.Select(t => t.UserId).ToList();
            var listAdd = input.UserIds.Where(t => !listExistUserId.Contains(t))
                .Select(t => new UserRoleEntity()
                {
                    UserId = t,
                    RoleId = input.RoleId,
                });
            _dbContext.AddWithAudit(listAdd);
        }

        public void Update(UpdateInputDto input)
        {
            var entity = _dbContext.Find<RoleEntity>(input.Id);
            _mapper.Map(input, entity);
            _dbContext.SetUpdateAudit(entity);
        }
    }
}
