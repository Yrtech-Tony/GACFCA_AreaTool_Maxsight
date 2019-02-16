﻿using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FIAT.Web.Common.Module;
using FIAT.Web.Context;
using FIAT.Web.Common;

namespace FIAT.Web.Service
{
    public interface IUsersService
    {
        Task<UserInfo> Login(string UserName, string Password);
        Task<UserInfo> LoginForBs(string UserName, string Password);
    }
    public class UsersService : IUsersService
    {
        public async Task<UserInfo> Login(string UserName, string Password)
        {
            string spName = @"up_sant_Login_Login_R_01";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserName", UserName);
            dp.Add("@Password", Password);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                try
                {
                    var userManys = await conn.QueryMultipleAsync(spName, param: dp, commandType: System.Data.CommandType.StoredProcedure);
                    UserInfo userDto = userManys.ReadFirst<UserInfo>();
                    var bizList = userManys.Read<BizDto>();
                    var repList = userManys.Read<RepDto>();
                    var zionList = userManys.Read<ZionDto>();
                    var areaList = userManys.Read<AreaDto>();
                    var serverList = userManys.Read<ServerDto>();
                    var departmentList = userManys.Read<DepartmentDto>();
                    //var impPlanStatusList = userManys.Read<ImpPlanStatusDto>();
                    //var impResultStatusList = userManys.Read<ImpResultStatusDto>();
                    var impStatusList = userManys.Read<ImpStatusDto>();
                    foreach (var item in areaList)
                    {
                        item.ServerList.AddRange(serverList.Where(s => s.AId == item.AId));
                    }
                    foreach (var item in zionList)
                    {
                        item.AreaList.AddRange(areaList.Where(a => a.QId == item.QId));
                    }
                    foreach (var item in repList)
                    {
                        item.ZionList.AddRange(zionList.Where(q => q.EId == item.EId));
                    }
                    foreach (var item in bizList)
                    {
                        item.RepList.AddRange(repList.Where(r => r.BId == item.BId));
                    }

                    userDto.DepartmentList.AddRange(departmentList);

                    
                    return userDto;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<UserInfo> LoginForBs(string UserName, string Password)
        {
            string spName = @"up_sant_Login_LoginForBs_R_01";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserName", UserName);
            dp.Add("@Password", Password);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                try
                {
                    var userManys = await conn.QueryMultipleAsync(spName, param: dp, commandType: System.Data.CommandType.StoredProcedure);
                    UserInfo userRoleDto = userManys.ReadFirst<UserInfo>();
                    var roleDto = userManys.Read<RoleInfo>();
                    //var zionList = userManys.Read<ZionDto>();
                    //var areaList = userManys.Read<AreaDto>();
                    //var serverList = userManys.Read<ServerDto>();
                    var departmentList = userManys.Read<DepartmentDto>();
                    //foreach (var item in areaList)
                    //{
                    //    item.ServerList.AddRange(serverList.Where(s => s.AId == item.AId));
                    //}
                    //foreach (var item in zionList)
                    //{
                    //    item.AreaList.AddRange(areaList.Where(a => a.QId == item.QId));
                    //}
                    userRoleDto.RoleList.AddRange(roleDto);
                    //userRoleDto.ZionList.AddRange(zionList);
                    userRoleDto.DepartmentList.AddRange(departmentList);

                    return userRoleDto;
                }
                catch (Exception ex)
                {
                    return null ;
                }
            }
        }
    
    }
}