using Dapper;
using Newtonsoft.Json;
using FIAT.API.Common;
using FIAT.API.Context;
using FIAT.API.Models;
using FIAT.API.Models.HomeDto;
using FIAT.API.Models.PlanTaskMngDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Service
{
    public interface IPlanTaskMngService
    {
        Task<APIResult> GetPlansList(string Title, string VisitType, string SDate, string EDate, string UserId,string SourceType);
        Task<APIResult> GetPlansDtlList(string Id);
        Task<APIResult> ReviewPlans(string Id, string PStatus, string RejectReason, string UserId);
        Task<APIResult> AddorUpdatePlans(string PId, string Title, string LastPScore, string DistributorId, string VisitDateTime, string VisitType, string PStatus, string InUserId, List<TCardDto> XmlData);
        Task<APIResult> GetDistributorByUserId(string UserId);
        Task<APIResult> GetStatus(string GroupCode,int BusId);
        Task<APIResult> GetAllDistributor(string Type, string Code, string Name, string DisId);
        Task<APIResult> GetAllTaskCard(string TCType, string TCRang, string SDate, string EDate, string UserId, List<StatusDto> dislist);
        Task<APIResult> DeleteTaskOfPlans(string Id);
        Task<APIResult> ClosePlanById(string Id);
        Task<APIResult> GetReviewPlansList(string Title, string VisitType, string SDate, string EDate, string PStatus, string UserId, string Area);
        Task<APIResult> GetMandatoryPlansDtlById(string UserId, List<StatusDto> list);
        Task<APIResult> GetReviewStatusByGroupCode(string GroupCode);
        Task<APIResult> AddTaskCard(string TCType, string TCRange, string TCTitle,string SourceType, string TCDescription, string InUserId, string Kind, List<AddTaskItemsDto> TaskItemXML, List<AddCheckStandardDto> XmlDataS, List<AddStandardPicDto> XmlDataP, string TCWeight);
        Task<APIResult> GetTaskCardList(string SDate, string EDate, string TCType, string TCRange, string InUserId, string UseYN,string SourceType,string Kind);
        Task<APIResult> GetTaskCardDtlList(string TCId);
        Task<APIResult> CreatePlans(string PId, string Title, string LastPScore, string DistributorId, string VisitDateTime, string VisitType, string PStatus, string InUserId, List<TCardDto> XmlData, List<StatusDto> list);
        Task<APIResult> UpdateTaskCard(string Id, string TCType, string TCRange, string TCTitle, string TCDescription, string InUserId,string UseYN, string Kind,List<AddTaskItemsDto> TaskItemXML, List<AddCheckStandardDto> XmlDataS, List<AddStandardPicDto> XmlDataP, List<TaskItemDto> DTaskItemXML, List<PictureStandardDto> XmlDataPD, List<CheckStandardDto> XmlDataSD,string tcWeight);
        Task<APIResult> ExcelUploadPlans(string InUserId,string SourceType, List<ExcelPlans> Plans, List<ExcelTaskOfPlans> TaskOfPlans, List<ExcelTaskCard> TaskCard, List<ExcelTaskItem> TaskItem, List<ExcelCheckStandard> CheckStandard, List<ExcelScore> Score, List<ExcelCheckResult> CheckResult, List<ExcelPictureStandard> PictureStandard);
        Task<APIResult> PlansPush(string Id, string UserId,string Content);
        Task<APIResult> GetReviewPlansList_Mobile( string UserId);
        Task<APIResult> GetPushInfoById(string Id);
        Task<APIResult> GetPushInfoByPlanId(string PId);
        Task<APIResult> GetLastItemScoreByPlan(string tpid, string tcid);
        Task<APIResult> GetItemsOfCardsInCurrentPlan(string pTitle);

        Task<APIResult> ExcelUploadPlansForUpdate(string InUserId, List<PDto> LastPScore, List<TPDto> LastTCScore, List<TIDto> LastTIScore);
    }

    public class PlanTaskMngService : IPlanTaskMngService
    {
        public async Task<APIResult> GetPlansList(string Title, string VisitType, string SDate, string EDate, string UserId,string SourceType)
        {
            string spName = @"up_RMMT_TAS_GetPlansList_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Title", Title == null ? "" : Title, DbType.String);
            dp.Add("@VisitType", VisitType == null ? "" : VisitType, DbType.String);
            dp.Add("@SDate", SDate, DbType.String);
            dp.Add("@EDate", EDate, DbType.String);
            dp.Add("@UserId", UserId, DbType.Int64);
            dp.Add("@SourceType", SourceType,DbType.String);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<PlansListDto> dto = await con.QueryAsync<PlansListDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PlansListDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }

        }
        public async Task<APIResult> GetPlansDtlList(string Id)
        {
            string spName = @"up_RMMT_TAS_GetPlansDtl_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", Id, DbType.Int64);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<PlanDtlDto> dto = await con.QueryAsync<PlanDtlDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PlanDtlDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> ReviewPlans(string Id, string PStatus, string RejectReason, string UserId)
        {
            string spName = @"up_RMMT_TAS_ReviewPlans_U";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", Id, DbType.Int64);
            dp.Add("@PStatus", PStatus, DbType.String);
            dp.Add("@RejectReason", RejectReason, DbType.String);
            dp.Add("@UserId", UserId, DbType.Int64);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }

        public async Task<APIResult> AddorUpdatePlans(string PId, string Title,string LastPScore, string DistributorId, string VisitDateTime, string VisitType, string PStatus, string InUserId, List<TCardDto> xmlData)
        {

            string XmlData = CommonHelper.Serializer(typeof(List<TCardDto>), xmlData);
            string spName = @"up_RMMT_TAS_AddorUpdatePlans_C";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@PId", PId, DbType.Int64);
            dp.Add("@Title", Title, DbType.String);
            dp.Add("@VLastPScore", LastPScore, DbType.String);
            dp.Add("@Code", DistributorId, DbType.Int64);
            dp.Add("@VisitDateTime", VisitDateTime, DbType.String);
            dp.Add("@VisitType", VisitType, DbType.String);
            dp.Add("@PStatus", PStatus, DbType.String);
            dp.Add("@XmlData", XmlData, DbType.Xml);
            dp.Add("@InUserId", InUserId, DbType.Int64);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }

        public async Task<APIResult> CreatePlans(string PId, string Title,string LastPScore, string DistributorId, string VisitDateTime, string VisitType, string PStatus, string InUserId, List<TCardDto> xmlDataP, List<StatusDto> list)
        {
            string XmlData = CommonHelper.Serializer(typeof(List<TCardDto>), xmlDataP);
            string List = CommonHelper.Serializer(typeof(List<StatusDto>), list);
            string spName = @"up_RMMT_TAS_CreatePlans_C";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@PId", PId, DbType.Int64);
            dp.Add("@Title", Title, DbType.String);
            dp.Add("@VLastPScore", LastPScore, DbType.String);
            dp.Add("@Code", DistributorId, DbType.String);
            dp.Add("@VisitDateTime", VisitDateTime, DbType.String);
            dp.Add("@VisitType", VisitType, DbType.String);
            dp.Add("@PStatus", PStatus, DbType.String);
            dp.Add("@XmlData", XmlData, DbType.Xml);
            dp.Add("@InUserId", InUserId, DbType.Int64);
            dp.Add("@XMLDataD", List, DbType.Xml);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }

        public async Task<APIResult> GetDistributorByUserId(string UserId)
        {
            string spName = @"up_RMMT_TAS_GetDistributorByUserId_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", UserId, DbType.Int64);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<StatusDto> dto = await con.QueryAsync<StatusDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<StatusDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetStatus(string GroupCode, int BusId)
        {
            string spName = @"up_RMMT_TAS_GetStatusByGroupCode_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@GroupCode", GroupCode, DbType.String);
            dp.Add("@BusId", BusId, DbType.String);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<StatusDto> dto = await con.QueryAsync<StatusDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<StatusDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }

        public async Task<APIResult> GetAllDistributor(string Type, string Code, string Name, string DisId)
        {
            string spName = @"up_RMMT_TAS_GetAllDistributor_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Type", Type, DbType.String);
            dp.Add("@Code", Code == null ? "0" : Code, DbType.Int64);
            dp.Add("@Name", Name == null ? "" : Name, DbType.String);
            dp.Add("@DisId", DisId == null ? "0" : DisId, DbType.Int64);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<StatusDto> dto = await con.QueryAsync<StatusDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<StatusDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }

        public async Task<APIResult> GetAllTaskCard(string TCType, string TCRang, string SDate, string EDate, string UserId, List<StatusDto> dislist)
        {
            string XmlData = CommonHelper.Serializer(typeof(List<StatusDto>), dislist);
            string spName = @"up_RMMT_TAS_GetAllTaskCard_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@TCType", TCType == null ? "" : TCType, DbType.String);
            dp.Add("@TCRang", TCRang == null ? "" : TCRang, DbType.String);
            dp.Add("@SDate", SDate, DbType.String);
            dp.Add("@EDate", EDate, DbType.String);
            dp.Add("@UserId", UserId, DbType.Int64);
            dp.Add("@XMLData", XmlData, DbType.Xml);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<TaskCardDto> dto = await con.QueryAsync<TaskCardDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<TaskCardDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }

        }

        public async Task<APIResult> DeleteTaskOfPlans(string Id)
        {
            string spName = @"up_RMMT_TAS_DeleteTaskOfPlans_D";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", Id, DbType.Int64);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }
        public async Task<APIResult> ClosePlanById(string Id)
        {
            string spName = @"up_RMMT_TAS_ClosePlans_U";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", Id, DbType.Int64);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }
        public async Task<APIResult> GetReviewPlansList(string Title, string VisitType, string SDate, string EDate, string PStatus, string UserId, string Area)
        {
            string spName = @"up_RMMT_TAS_GetReviewPlansList_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Title", Title == null ? "" : Title, DbType.String);
            dp.Add("@VisitType", VisitType == null ? "" : VisitType, DbType.String);
            dp.Add("@SDate", SDate, DbType.String);
            dp.Add("@EDate", EDate, DbType.String);
            dp.Add("@PStatus", PStatus == null ? "" : PStatus, DbType.String);
            dp.Add("@UserId", UserId, DbType.Int64);
            dp.Add("@Area", Area, DbType.Int64);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<PlansListDto> dto = await con.QueryAsync<PlansListDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PlansListDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetMandatoryPlansDtlById(string UserId, List<StatusDto> list)
        {
            string XmlData = CommonHelper.Serializer(typeof(List<StatusDto>), list);
            string spName = @"up_RMMT_TAS_GetMandatoryPlansDtlById_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", UserId, DbType.Int64);
            dp.Add("@XmlData", XmlData, DbType.Xml);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<PlanDtlDto> dto = await con.QueryAsync<PlanDtlDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PlanDtlDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetReviewStatusByGroupCode(string GroupCode)
        {
            string spName = @"up_RMMT_TAS_GetReviewStatusByGroupCode_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@GroupCode", GroupCode, DbType.String);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<StatusDto> dto = await con.QueryAsync<StatusDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<StatusDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> AddTaskCard(string TCType, string TCRange, string TCTitle, string SourceType,string TCDescription, string InUserId, string Kind, List<AddTaskItemsDto> TaskItemXML, List<AddCheckStandardDto> XmlDataS, List<AddStandardPicDto> XmlDataP,string TCWeight)
        {
            string XmlDataCS = CommonHelper.Serializer(typeof(List<AddCheckStandardDto>), XmlDataS);
            string XmlDataPS = CommonHelper.Serializer(typeof(List<AddStandardPicDto>), XmlDataP);
            string XMLDataTI = CommonHelper.Serializer(typeof(List<AddTaskItemsDto>), TaskItemXML);
            string spName = @"up_RMMT_TAS_AddTaskCard_C";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@TCType", TCType, DbType.String);
            dp.Add("@TCRange", TCRange, DbType.String);
            dp.Add("@TCTitle", TCTitle, DbType.String);
            dp.Add("@SourceType",SourceType,DbType.String);
            dp.Add("@TCDescription", TCDescription, DbType.String);
            dp.Add("@InUserId", InUserId, DbType.Int64);
            dp.Add("@Kind",Kind,DbType.String);
            dp.Add("@TaskItemXML", XMLDataTI, DbType.Xml);
            dp.Add("@XmlDataS", XmlDataCS, DbType.Xml);
            dp.Add("@XmlDataP", XmlDataPS, DbType.Xml);
            dp.Add("@TCWeight", TCWeight, DbType.String);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }
        public async Task<APIResult> GetTaskCardList(string SDate, string EDate, string TCType, string TCRange, string InUserId, string UseYN,string SourceType,string Kind)
        {
            string spName = @"up_RMMT_TAS_GetTaskCardsList_R";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@SDate", SDate, DbType.String);
            dp.Add("@EDate", EDate, DbType.String);
            dp.Add("@TCType", TCType, DbType.String);
            dp.Add("@TCRange", TCRange, DbType.String);
            dp.Add("@InUserId", InUserId, DbType.Int64);
            dp.Add("@UseYN", UseYN == null ? "" : UseYN, DbType.String);
            dp.Add("@SourceType", SourceType,DbType.String);
            dp.Add("@Kind",Kind,DbType.String);
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<TaskCardDto> dto = await con.QueryAsync<TaskCardDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<TaskCardDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetTaskCardDtlList(string TCId)
        {
            try
            {
                string spName = @"up_RMMT_TAS_GetTaskCardDtl_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@TCId", TCId, DbType.Int64);
                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    var list = await conn.QueryMultipleAsync(spName, dp, null, null, CommandType.StoredProcedure);
                    var l1 = list.ReadAsync().Result;
                    var l2 = list.ReadAsync().Result;
                    var l3 = list.ReadAsync().Result;
                    var l4 = list.ReadAsync().Result;
                    List<TaskCardDtlDto> lst = JsonConvert.DeserializeObject<List<TaskCardDtlDto>>(JsonConvert.SerializeObject(l1));
                    List<TaskItemDto> lst1 = JsonConvert.DeserializeObject<List<TaskItemDto>>(JsonConvert.SerializeObject(l2));
                    List<CheckStandardDto> lst2 = JsonConvert.DeserializeObject<List<CheckStandardDto>>(JsonConvert.SerializeObject(l3));
                    List<PictureStandardDto> lst3 = JsonConvert.DeserializeObject<List<PictureStandardDto>>(JsonConvert.SerializeObject(l4));
                    foreach (var item in lst)
                    {
                        item.TIList = new List<TaskItemDto>();
                        item.TIList.AddRange(lst1);
                        item.CSList = new List<CheckStandardDto>();
                        item.CSList.AddRange(lst2);
                        item.PSList = new List<PictureStandardDto>();
                        item.PSList.AddRange(lst3);
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<TaskCardDtlDto>(lst), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
            }
            catch (Exception ex)
            {

                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> UpdateTaskCard(string Id, string TCType, string TCRange, string TCTitle, string TCDescription, string InUserId,string UseYN, string Kind,List<AddTaskItemsDto> TaskItemXML, List<AddCheckStandardDto> XmlDataS, List<AddStandardPicDto> XmlDataP, List<TaskItemDto> DTaskItemXML, List<PictureStandardDto> XmlDataPD, List<CheckStandardDto> XmlDataSD,string tcWeight)
        {
            string XmlDataCS = CommonHelper.Serializer(typeof(List<AddCheckStandardDto>), XmlDataS);
            string XmlDataPS = CommonHelper.Serializer(typeof(List<AddStandardPicDto>), XmlDataP);
            string XMLDataTI = CommonHelper.Serializer(typeof(List<AddTaskItemsDto>), TaskItemXML);
            string XMLDataTID = CommonHelper.Serializer(typeof(List<TaskItemDto>), DTaskItemXML);
            string XmlDataCSD = CommonHelper.Serializer(typeof(List<CheckStandardDto>), XmlDataSD);
            string XmlDataPSD = CommonHelper.Serializer(typeof(List<PictureStandardDto>), XmlDataPD);

            string spName = @"up_RMMT_TAS_UpdateTaskCard_U";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", Id, DbType.Int64);
            dp.Add("@TCType", TCType, DbType.String);
            dp.Add("@TCRange", TCRange, DbType.String);
            dp.Add("@TCTitle", TCTitle, DbType.String);
            dp.Add("@TCDescription", TCDescription, DbType.String);
            dp.Add("@InUserId", InUserId, DbType.Int64);
            dp.Add("@UseYN", UseYN,DbType.Int64);
            dp.Add("@Kind",Kind,DbType.String);
            dp.Add("@TaskItemXML", XMLDataTI, DbType.Xml);
            dp.Add("@XmlDataS", XmlDataCS, DbType.Xml);
            dp.Add("@XmlDataP", XmlDataPS, DbType.Xml);
            dp.Add("@DTaskItemXML", XMLDataTID, DbType.Xml);
            dp.Add("@XmlDataSD", XmlDataCSD, DbType.Xml);
            dp.Add("@XmlDataPD", XmlDataPSD, DbType.Xml);
            dp.Add("@TCWeight", tcWeight, DbType.String);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }

        }
        public async Task<APIResult> ExcelUploadPlans(string InUserId, string SourceType, List<ExcelPlans> Plans, List<ExcelTaskOfPlans> TaskOfPlans, List<ExcelTaskCard> TaskCard, List<ExcelTaskItem> TaskItem, List<ExcelCheckStandard> CheckStandard, List<ExcelScore> Score, List<ExcelCheckResult> CheckResult, List<ExcelPictureStandard> PictureStandard)
        {
            string PlansXML = CommonHelper.Serializer(typeof(List<ExcelPlans>), Plans);
            string TaskOfPlansXML = CommonHelper.Serializer(typeof(List<ExcelTaskOfPlans>), TaskOfPlans);
            string TaskCardXML = CommonHelper.Serializer(typeof(List<ExcelTaskCard>), TaskCard);
            string TaskItemXML = CommonHelper.Serializer(typeof(List<ExcelTaskItem>), TaskItem);
            string CheckStandardXML = CommonHelper.Serializer(typeof(List<ExcelCheckStandard>), CheckStandard);
            string ScoreXML = CommonHelper.Serializer(typeof(List<ExcelScore>), Score);
            string CheckResultXML = CommonHelper.Serializer(typeof(List<ExcelCheckResult>), CheckResult);
            string PictureStandardXML = CommonHelper.Serializer(typeof(List<ExcelPictureStandard>), PictureStandard!=null&&PictureStandard.Count>0?PictureStandard.FindAll(a=>a.TIId!=null&&a.StandardPicName!=null):PictureStandard);
            string spName = @"up_RMMT_TAS_CreatePlansByExcel_C";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@InUserId", InUserId, DbType.Int64);
            dp.Add("@SourceType", SourceType,DbType.String);
            dp.Add("@PlansXML", PlansXML, DbType.Xml);
            dp.Add("@TaskOfPlansXML", TaskOfPlansXML, DbType.Xml);
            dp.Add("@TaskCardXML", TaskCardXML, DbType.Xml);
            dp.Add("@TaskItemXML", TaskItemXML, DbType.Xml);
            dp.Add("@CheckStandardXML", CheckStandardXML, DbType.Xml);
            dp.Add("@ScoreXML", ScoreXML, DbType.Xml);
            dp.Add("@CheckResultXML", CheckResultXML, DbType.Xml);
            dp.Add("@PictureStandardXML", PictureStandardXML, DbType.Xml);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }

        }
        public async Task<APIResult> PlansPush(string Id, string UserId, string Content)
        {
            string spName = @"up_RMMT_TAS_PlansPush_C";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@PId", Id, DbType.Int64);
            dp.Add("@UserId", UserId, DbType.Int64);
            dp.Add("@Content",Content,DbType.String);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }
        public async Task<APIResult> GetReviewPlansList_Mobile(string UserId)
        {
            string spName = @"up_RMMT_TAS_GetReviewPlansListMobile_R";
            DynamicParameters dp = new DynamicParameters();           
            dp.Add("@UserId", UserId, DbType.Int64);           
            using (var con = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                con.Open();
                try
                {
                    IEnumerable<PlansListDto> dto = await con.QueryAsync<PlansListDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PlansListDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {

                    return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetPushInfoById(string Id)
        {
            string spName = @"up_RMMT_Home_GetPushInfoById_R";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", Id, DbType.Int64);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                try
                {
                    IEnumerable<PushInfoDto> dto = await conn.QueryAsync<PushInfoDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PushInfoDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {
                    return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetPushInfoByPlanId(string PId)
        {
            string spName = @"up_RMMT_TAS_GetsPushInfoByPlanId_R";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@PId", PId, DbType.Int64);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                try
                {
                    IEnumerable<PushContentDto> dto = await conn.QueryAsync<PushContentDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<PushContentDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {
                    return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
                }
            }
        }
        public async Task<APIResult> GetLastItemScoreByPlan(string tpid,string tcid)
        {
            string spName = @"up_RMMT_TAS_GetLastItemScoreByPlan_R";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@TPId", tpid, DbType.Int64); 
            dp.Add("@TCId", tcid, DbType.Int64);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                try
                {
                    IEnumerable<LastScoreDto> dto = await conn.QueryAsync<LastScoreDto>(spName, dp, null, null, CommandType.StoredProcedure);
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<LastScoreDto>(dto), ResultCode = ResultType.Success, Msg = "" };
                    return result;
                }
                catch (Exception ex)
                {
                    return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
                }
            }
        }

        public async Task<APIResult> GetItemsOfCardsInCurrentPlan(string pTitle)
        {
            try
            {
                string spName = @"up_RMMT_TAS_GetItemsOfCardsInCurrentPlan_R";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@PTitle", pTitle, DbType.String);

                using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
                {
                    conn.Open();
                    var rManys = await conn.QueryMultipleAsync(spName, dp, commandType: System.Data.CommandType.StoredProcedure);
                    CurrentPlanDto rDto = rManys.ReadFirstOrDefault<CurrentPlanDto>();
                    if (rDto == null) rDto = new CurrentPlanDto();
                    var pList = rManys.Read<PDto>();
                    rDto.PList.AddRange(pList);
                    var tpList = rManys.Read<TPDto>();
                    rDto.TPList.AddRange(tpList);
                    var tiList = rManys.Read<TIDto>();
                    rDto.TIList.AddRange(tiList);
                    var message = "";
                    if (rDto.Batch =="0")
                    {
                        message = "没有数据";
                    }
                    APIResult result = new APIResult { Body = CommonHelper.EncodeDto<CurrentPlanDto>(rDto), ResultCode = ResultType.Success, Msg = message };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
            }
        }

        public async Task<APIResult> ExcelUploadPlansForUpdate(string InUserId, List<PDto> LastPScore, List<TPDto> LastTCScore, List<TIDto> LastTIScore)
        {
            string PXML = CommonHelper.Serializer(typeof(List<PDto>), LastPScore);
            string TPXML = CommonHelper.Serializer(typeof(List<TPDto>), LastTCScore);
            string TCXML = CommonHelper.Serializer(typeof(List<TIDto>), LastTIScore);
            string spName = @"up_RMMT_TAS_UpdateLastScoreByExcel_U";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@InUserId", InUserId, DbType.Int64);
            dp.Add("@PXML", PXML, DbType.Xml);
            dp.Add("@TPXML", TPXML, DbType.Xml);
            dp.Add("@TCXML", TCXML, DbType.Xml);
            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await conn.ExecuteAsync(spName, dp, tran, 180, CommandType.StoredProcedure);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = ex.Message };
                    }
                    finally
                    {
                        tran.Dispose();
                    }

                }
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
        }

    }
}
