using Dapper;
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
using log4net;

namespace FIAT.Web.Service
{
    public interface IUsersService
    {
        Task<APIResult> Login(string UserName, string Password);
        Task<APIResult> LoginForBs(string UserName, string Password);
    }
    public class UsersService : IUsersService
    {
        private ILog log;
        public UsersService()
        {
            //log4Net
            this.log = LogManager.GetLogger(Startup.repository.Name, typeof(UsersService));
        }
        public async Task<APIResult> Login(string UserName, string Password)
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
                    UserDto userDto = userManys.ReadFirst<UserDto>();
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
                    userDto.BizList.AddRange(bizList);
                    userDto.DepartmentList.AddRange(departmentList);
                    //userDto.ImpPlanStatusList.AddRange(impPlanStatusList);
                    //userDto.ImpResultStatusList.AddRange(impResultStatusList);
                    userDto.ImpStatusList.AddRange(impStatusList);

                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<UserDto>(userDto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {
                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }

        public async Task<APIResult> LoginForBs(string UserName, string Password)
        {
            string spName = @"up_sant_Login_LoginForBs_R_01";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserName", UserName);
            dp.Add("@Password", Password);
            //log.Info("初始化连接....");
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                //log.Info("初始化连接成功....");
                conn.Open();
                //log.Info("打开连接成功....");
                try
                {
                    var userManys = await conn.QueryMultipleAsync(spName, param: dp, commandType: System.Data.CommandType.StoredProcedure);
                    //log.Info("查询成功....");
                    UserRoleDto userRoleDto = userManys.ReadFirst<UserRoleDto>();
                    //log.Info("查询成功，获取到UserInfo=" + userRoleDto.Name);
                   
                    var roleDto = userManys.Read<RoleDto>();
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
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<UserRoleDto>(userRoleDto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {
                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message }; ;
                }
            }
        }        
    }
}
