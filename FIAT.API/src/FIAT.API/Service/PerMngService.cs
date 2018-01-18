using Dapper;
using FIAT.API.Common;
using FIAT.API.Context;
using FIAT.API.Models;
using FIAT.API.Models.ImprovementDto;
using FIAT.API.Models.PerMngDto;
using FIAT.API.Models.ReportDto;
using FIAT.API.Models.StatisticDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Service
{
    public interface IPerMngService
    {
        Task<APIResult> GetPlansByOrganization(int busId, int repId, int areaId, int zoneId, string vType, string status);
        Task<APIResult> GetDistributorByBatch(int batchId);
        Task<APIResult> StaffInfoReg(StuffInfoParams param);
        Task<APIResult> GetStaffAndPostInfo(int busType, int disId, string yearMonth, string postCode);
        Task<APIResult> GetStaffZoneReport(string yearMonth, string zoneId, int busType);
        Task<APIResult> GetStaffRegProgress(int busId, int repId, int areaId, int zoneId, string yearMonth);
        Task<APIResult> GetStaffRegProgressForZone(int busId, int repId, int areaId, int zoneId, string yearMonth);
        Task<APIResult> StartCheckStaff(int busType, int disId, string yearMonth);
        Task<APIResult> UpdateStaffInfo(StuffInfoParams param);
        Task<APIResult> GetDisForStaff(int bId, int rId, int zId, int aId, string m, string dName);
        Task<APIResult> GetMonthForStaff(int bId, int rId, int zId, int aId);
        Task<APIResult> SaveDisForStaff(StaffDisParamDto param);
    }
    public class PerMngService : IPerMngService
    {
        public async Task<APIResult> StaffInfoReg(StuffInfoParams param)
        {
            try
            {
                string StaffInfoXML = CommonHelper.Serializer(typeof(List<StaffInfo>), param.StaffInfoList);
                string spName = @"up_RMMT_PMN_StaffInfoReg_C";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@StaffInfoXML", StaffInfoXML, DbType.Xml);
                dp.Add("@UserId", param.UserId, DbType.Int32);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
                }

            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetDistributorByBatch(int batchId)
        {
            try
            {
                string spName = @"up_RMMT_PMN_GetDistributorByBatch_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BatchId", batchId, DbType.Int64);
                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    IEnumerable<NameValueDto> list = await conn.QueryAsync<NameValueDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<NameValueDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetPlansByOrganization(int busId, int repId, int areaId, int zoneId, string vType, string status)
        {
            try
            {
                string spName = @"up_RMMT_PMN_Plans_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BusId", busId, DbType.Int64);
                dp.Add("@RepId", repId, DbType.Int64);
                dp.Add("@AreaId", areaId, DbType.Int64);
                dp.Add("@ZoneId", zoneId, DbType.Int64);
                dp.Add("@VType", vType, DbType.String);
                dp.Add("@Status", status, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<NameValueDto> list = await conn.QueryAsync<NameValueDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<NameValueDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetStaffAndPostInfo(int busType, int disId, string yearMonth, string postCode)
        {
            try
            {
                string spName = @"up_RMMT_PMN_GetStaffAndPostInfo_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BusType", busType, DbType.Int32);
                dp.Add("@DisId", disId, DbType.Int32);
                dp.Add("@YearMonth", yearMonth, DbType.String);
                dp.Add("@PostCode", postCode, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<StaffInfo> list = await conn.QueryAsync<StaffInfo>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<NameValueDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetStaffZoneReport(string yearMonth, string zoneId, int busType)
        {
            try
            {
                string spName = @"up_RMMT_PMN_StaffZoneReport_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@YearMonth", yearMonth, DbType.String);
                dp.Add("@ZoneId", zoneId, DbType.Int32);
                dp.Add("@BusType", busType, DbType.Int32);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    var list = await conn.QueryMultipleAsync(spName, dp, null, null, CommandType.StoredProcedure);
                    var r1 = list.ReadAsync().Result;
                    var r2 = list.ReadAsync().Result;

                    List<StaffZoneReportDto> lst = JsonConvert.DeserializeObject<List<StaffZoneReportDto>>(JsonConvert.SerializeObject(r1));
                    List<StaffInfo> ls2 = JsonConvert.DeserializeObject<List<StaffInfo>>(JsonConvert.SerializeObject(r2));
                    StaffZoneReport report = new StaffZoneReport();
                    report.ZoneReportList.AddRange(lst);
                    report.StaffPostInfoList.AddRange(ls2);
                    string message = "";
                    if (lst.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<StaffZoneReport>(report), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetStaffRegProgress(int busId, int repId, int areaId, int zoneId, string yearMonth)
        {
            try
            {
                string spName = @"up_RMMT_PMN_StaffRegProgress_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BusId", busId, DbType.Int64);
                dp.Add("@RepId", repId, DbType.Int64);
                dp.Add("@AreaId", areaId, DbType.Int64);
                dp.Add("@ZoneId", zoneId, DbType.Int64);
                dp.Add("@YearMonth", yearMonth, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<TourProgressDto> list = await conn.QueryAsync<TourProgressDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<TourProgressDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetStaffRegProgressForZone(int busId, int repId, int areaId, int zoneId, string yearMonth)
        {
            try
            {
                string spName = @"up_RMMT_PMN_StaffImportProgress_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BusId", busId, DbType.Int64);
                dp.Add("@RepId", repId, DbType.Int64);
                dp.Add("@AreaId", areaId, DbType.Int64);
                dp.Add("@ZoneId", zoneId, DbType.Int64);
                dp.Add("@YearMonth", yearMonth, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    var sManys = await conn.QueryMultipleAsync(spName, param: dp, commandType: System.Data.CommandType.StoredProcedure);
                    var bList = sManys.ReadAsync().Result;
                    var rList = sManys.ReadAsync().Result;
                    var aList = sManys.ReadAsync().Result;
                    var zList = sManys.ReadAsync().Result;
                    var dList = sManys.ReadAsync().Result;

                    ZoneTourProgressDto dto = new ZoneTourProgressDto();
                    List<BTourProgressDto> lstb = JsonConvert.DeserializeObject<List<BTourProgressDto>>(JsonConvert.SerializeObject(bList));
                    dto.BList.AddRange(lstb);
                    List<RTourProgressDto> lstr = JsonConvert.DeserializeObject<List<RTourProgressDto>>(JsonConvert.SerializeObject(rList));
                    dto.RList.AddRange(lstr);
                    List<ATourProgressDto> lsta = JsonConvert.DeserializeObject<List<ATourProgressDto>>(JsonConvert.SerializeObject(aList));
                    dto.AList.AddRange(lsta);
                    List<ZTourProgressDto> lst2 = JsonConvert.DeserializeObject<List<ZTourProgressDto>>(JsonConvert.SerializeObject(zList));
                    dto.ZList.AddRange(lst2);
                    List<DTourProgressDto> lst3 = JsonConvert.DeserializeObject<List<DTourProgressDto>>(JsonConvert.SerializeObject(dList));
                    dto.DList.AddRange(lst3);

                    string message = "";
                    if (rList.Count() == 0 && zList.Count() == 0 && dList.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<ZoneTourProgressDto>(dto), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> StartCheckStaff(int busType, int disId,string yearMonth)
        {
            try
            {
                string spName = @"up_RMMT_PMN_StartCheckStaff_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BusType", busType, DbType.Int32);
                dp.Add("@DisId", disId, DbType.Int32);
                dp.Add("@CurrentYM", yearMonth, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<StaffInfo> list = await conn.QueryAsync<StaffInfo>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<NameValueDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> UpdateStaffInfo(StuffInfoParams param)
        {
            try
            {
                string StaffInfoXML = CommonHelper.Serializer(typeof(List<StaffInfo>), param.StaffInfoList);
                string spName = @"up_RMMT_PMN_StaffInfoReg_U";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@StaffInfoXML", StaffInfoXML, DbType.Xml);
                dp.Add("@YearMonth", param.YearMonth, DbType.String);
                dp.Add("@BusType", param.BusType, DbType.Int32);
                dp.Add("@DisId", param.DisId, DbType.Int32);
                dp.Add("@UserId", param.UserId, DbType.Int32);
                dp.Add("@Position", param.Position, DbType.String);
                dp.Add("@SaveNode", param.SaveNode, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
                }

            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetDisForStaff(int bId, int rId, int zId, int aId, string m,string dName)
        {
            try
            {
                string spName = @"up_RMMT_STA_GetDisForStaff_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BId", bId, DbType.Int64);
                dp.Add("@RId", rId, DbType.Int64);
                dp.Add("@ZId", zId, DbType.Int64);
                dp.Add("@AId", aId, DbType.Int64);
                dp.Add("@M", m, DbType.String);
                dp.Add("@DName", dName==null?"":dName, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<StaffDisDto> list = await conn.QueryAsync<StaffDisDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<StaffDisDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

       public async Task<APIResult> GetMonthForStaff(int bId, int rId, int zId, int aId)
        {
            try
            {
                string spName = @"up_RMMT_STA_GetMonthForStaff_R";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BId", bId, DbType.Int64);
                dp.Add("@RId", rId, DbType.Int64);
                dp.Add("@ZId", zId, DbType.Int64);
                dp.Add("@AId", aId, DbType.Int64);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<NameValueDto> list = await conn.QueryAsync<NameValueDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<NameValueDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> SaveDisForStaff(StaffDisParamDto param)
        {
            try
            {
                string disStaffXml = CommonHelper.Serializer(typeof(List<StaffDisListDto>), param.DisList);
                string spName = @"up_RMMT_STA_SaveDisForStaff_C";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", param.InUserId, DbType.Int32);
                dp.Add("@DisStaffXML", disStaffXml, DbType.Xml);
                dp.Add("@DisStaffParam", "/ArrayOfStaffDisListDto/StaffDisListDto", DbType.String);
                dp.Add("@YearMonth", param.YearMonth, DbType.String);
                dp.Add("@BusType", param.BusType, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
                }

            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }
    }
}
