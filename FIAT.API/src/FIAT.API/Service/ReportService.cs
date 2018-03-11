using Dapper;
using FIAT.API.Common;
using FIAT.API.Context;
using FIAT.API.Models;
using FIAT.API.Models.ImprovementDto;
using FIAT.API.Models.PlanTaskMngDto;
using FIAT.API.Models.ReportDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Service
{
    public interface IReportService
    {
        Task<APIResult> GetAttachmentByUserId(int userId, string sourceType, string sDate, string eDate);
        Task<APIResult> SaveReportAttachment(SaveReportAttachdto param);
        Task<APIResult> UpdateAttachmentDownloadCnt(int id);
        Task<APIResult> GetPlansListForExcelDownload(string UserId, string DisId, int Batch);
        Task<APIResult> SearchTCScoreOFAIdByEPlan(string AreaId, string BatchId);
        Task<APIResult> GetTouProgressForZone(int busId, int repId, int areaId, int zoneId, int batchId);
        Task<APIResult> SearchTCScoreOFAIdsByEPlan(string RegionId, string BatchId, string AreaId);
        Task<APIResult> GetTourBaseScoreByDisId(string UserId, string DisId, int Batch);
        Task<APIResult> GetAllDataByDisIdForExcel(string batchId);
    }
    public class ReportService : IReportService
    {
        public async Task<APIResult> GetAttachmentByUserId(int userId, string sourceType, string sDate, string eDate)
        {
            try
            {
                string spName = @"up_RMMT_REP_GetAttachmentByUserId_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", userId, DbType.Int64);
                dp.Add("@SourceType", sourceType, DbType.String);
                dp.Add("@SDate", sDate, DbType.String);
                dp.Add("@Edate", eDate, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<ReportAttachmentDto> list = await conn.QueryAsync<ReportAttachmentDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<ReportAttachmentDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> SaveReportAttachment(SaveReportAttachdto param)
        {
            try
            {
                string AttachXml = CommonHelper.Serializer(typeof(List<ReportAttachmentDto>), param.AttachList);
                string spName = @"up_RMMT_REP_Attachment_C";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@AttachXml", AttachXml, DbType.Xml);
                dp.Add("@UserId", param.UserId, DbType.Int64);

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

        public async Task<APIResult> UpdateAttachmentDownloadCnt(int id)
        {
            try
            {
                string spName = @"up_RMMT_REP_AttachmentDownloadCnt_C";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@Id", id, DbType.Int64);

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

        public async Task<APIResult> GetPlansListForExcelDownload(string UserId, string DisId, int Batch)
        {
            try
            {
                string spName = @"up_RMMT_TAS_GetPlansListFroExcelDownLoad_R";

                DynamicParameters dp = new DynamicParameters();
                //dp.Add("@SDate", SDate, DbType.String);
                //dp.Add("@Edate", EDate, DbType.String);
                dp.Add("@UserId", UserId, DbType.Int64);
                dp.Add("@DisId", DisId, DbType.Int64);
                dp.Add("@Batch", Batch, DbType.Int64);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    IEnumerable<PlansListDto> list = await conn.QueryAsync<PlansListDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PlansListDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> SearchTCScoreOFAIdByEPlan(string AreaId, string BatchId)
        {
            try
            {
                string spName = @"up_RMMT_REP_GetTCScoreOFAIdByEPlan_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@AreaId", AreaId, DbType.Int64);
                dp.Add("@BatchId", BatchId, DbType.Int64);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    var sManys = await conn.QueryMultipleAsync(spName, param: dp, commandType: System.Data.CommandType.StoredProcedure);

                    AreaReportDto sDto = new AreaReportDto();

                    var bStatisList = sManys.Read<AreaStatisDto>();
                    var eStatisList = sManys.Read<AreaStatisDto>();
                    var rStatisList = sManys.Read<AreaStatisDto>();
                    var zStatisList = sManys.Read<AreaStatisDto>();
                    var asList = sManys.Read<AreaScoreByEPlanDto>();
                    var dsList = sManys.Read<DisSCoreByEPlanDto>();
                    foreach (var item in asList)
                    {
                        item.SDisList.AddRange(dsList.Where(s => s.TCId == item.TCId));
                    }
                    sDto.BStatislist.AddRange(bStatisList);
                    sDto.EStatislist.AddRange(eStatisList);
                    sDto.RStatislist.AddRange(rStatisList);
                    sDto.ZStatislist.AddRange(zStatisList);
                    sDto.ASBEPlist.AddRange(asList);
                    string message = "";
                    if (asList.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto(sDto), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> GetTouProgressForZone(int busId, int repId, int areaId, int zoneId, int batchId)
        {
            try
            {
                string spName = @"up_RMMT_STA_TouProgressForZone_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BusId", busId, DbType.Int64);
                dp.Add("@RepId", repId, DbType.Int64);
                dp.Add("@AreaId", areaId, DbType.Int64);
                dp.Add("@ZoneId", zoneId, DbType.Int64);
                dp.Add("@BatchId", batchId, DbType.Int64);

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

        public async Task<APIResult> SearchTCScoreOFAIdsByEPlan(string RegionId, string BatchId,string AreaId)
        {
            try
            {
                string spName = @"up_RMMT_REP_GetTCScoreOFAIdsByEPlan_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@RegionId", RegionId, DbType.Int64);
                dp.Add("@BatchId", BatchId, DbType.Int64);
                dp.Add("@AreaId", AreaId, DbType.Int64);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();

                    var sManys = await conn.QueryMultipleAsync(spName, param: dp, commandTimeout:300,commandType: System.Data.CommandType.StoredProcedure);

                    ResultDto sDto = sManys.ReadFirstOrDefault<ResultDto>();
                    if (sDto == null) sDto = new ResultDto();
                    var rsList = sManys.Read<RAProgressDto>();
                    var asList = sManys.Read<RATCScoreDto>();
                    var dsList = sManys.Read<STCScoreDto>();
                    sDto.RAPList.AddRange(rsList);
                    sDto.RATCList.AddRange(asList);
                    sDto.STCList.AddRange(dsList);
                    string message = "";
                    if (rsList.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<ResultDto>(sDto), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }

        }

        public async Task<APIResult> GetTourBaseScoreByDisId(string UserId, string DisId, int Batch)
        {
            try
            {
                string spName = @"up_RMMT_REP_GetTourBaseScoreByDisId_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", UserId, DbType.Int64);
                dp.Add("@DisId", DisId, DbType.Int64);
                dp.Add("@Batch", Batch, DbType.Int64);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    var rManys = await conn.QueryMultipleAsync(spName, dp, commandType: System.Data.CommandType.StoredProcedure);
                    RDto rDto = rManys.ReadFirstOrDefault<RDto>();
                    if (rDto == null) rDto = new RDto();
                    var colList = rManys.Read<ColDto>();
                    dynamic tbsDto = new TourBaseScoreDto();
                    tbsDto.Code = "经销商代码";
                    tbsDto.Name = "经销商简称";
                    tbsDto.ItemScore = "得分";
                    foreach (var item in colList)
                    {
                        var coln = item.ColName;
                        tbsDto[coln] = item.ColValue;
                    }
                    //rDto.touList.Add(tbsDto);

                    string colStr = CommonHelper.EncodeDto<TourBaseScoreDto>(tbsDto);
                    rDto.colList = colStr;

                    var wList= rManys.Read<dynamic>();
                    var wStr = CommonHelper.EncodeDto<TourBaseScoreDto>(wList);
                    rDto.wList = wStr;

                    var tbsList = rManys.Read<dynamic>();
                   
                    //foreach (var row in ((System.Collections.Generic.List<object>)tbsList))
                    //{

                    //    int len = ((System.Collections.Generic.IDictionary<string, object>)row).Keys.Count;
                    //    dynamic item = new TourBaseScoreDto();
                    //    for (int i = 0; i < len; i++)
                    //    {
                    //        item[((string[])(((System.Collections.Generic.IDictionary<string, object>)row).Keys))[i]] = ((object[])(((System.Collections.Generic.IDictionary<string, object>)row).Values))[i]==null?"": ((object[])(((System.Collections.Generic.IDictionary<string, object>)row).Values))[i].ToString();
                    //    }
                    //    rDto.touList.Add(item);
                    //}
                    var touStr = CommonHelper.EncodeDto<TourBaseScoreDto>(tbsList);
                    rDto.touList = touStr;

                    var bhList= rManys.Read<ColDto>();
                    dynamic bDto = new TourBaseScoreDto();
                    dynamic hDto = new TourBaseScoreDto();
                    bDto.Code = "经销商代码";
                    bDto.Name = "经销商简称";
                    bDto.ItemScore = "审计项目";
                    hDto.Code = "经销商代码";
                    hDto.Name = "经销商简称";
                    hDto.ItemScore = "检查标准";
                    foreach (var item in bhList)
                    {
                        var coln = item.ColName;
                        bDto[coln] = item.TITitle;
                        hDto[coln] = item.ColValue;
                    }
                    string bStr = CommonHelper.EncodeDto<TourBaseScoreDto>(bDto);
                    string hStr= CommonHelper.EncodeDto<TourBaseScoreDto>(hDto);
                    rDto.bList = bStr;
                    rDto.hList = hStr;

                    var rList = rManys.Read<dynamic>();
                    var rStr = CommonHelper.EncodeDto<TourBaseScoreDto>(rList);
                    rDto.rList = rStr;

                    string message = "";
                    if (rDto.touList =="")
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<RDto>(rDto), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }

        }

        public async Task<APIResult> GetAllDataByDisIdForExcel(string batchId)
        {
            try
            {
                string spName = @"up_RMMT_REP_GetTCScoreOFAIdsByEPlan_AllDis_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@BatchId", batchId, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    IEnumerable<STCScoreDto> list = await conn.QueryAsync<STCScoreDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    string message = "";
                    if (list.Count() == 0)
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<STCScoreDto>(list), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }
    }
}
