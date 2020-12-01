using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Common
{
    [Description]
    public class PermissionNames
    {
        #region 用户

        public const string User_Get = "用户_查看";
        public const string User_GetList = "用户_列表";
        public const string User_Add = "用户_新增";
        public const string User_Update = "用户_修改";
        public const string User_Remove = "用户_删除";
        public const string User_ResetPassword = "用户_重置密码";
        public const string User_SetRole = "用户_分配角色";

        #endregion

        #region 角色

        public const string Role_Get = "角色_查看";
        public const string Role_GetList = "角色_列表";
        public const string Role_Add = "角色_新增";
        public const string Role_Update = "角色_修改";
        public const string Role_Remove = "角色_删除";
        public const string Role_SetUser = "角色_分配用户";
        public const string Role_SetPermission = "角色_分配权限";

        #endregion


    }
}
