using Dapper;
using FIAT.API.Common;
using FIAT.API.Context;
using FIAT.API.Models;
using FIAT.API.Models.AttachmentMngDto;
using FIAT.API.Models.NotifiMngDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FIAT.API.Service
{
    public interface IAttachmentMngService
    {
        Task<APIResult> AddOrDeleteAttachment(AttachmentMngDto attachmentMngDto);
    }
    public class AttachmentMngService : IAttachmentMngService
    {
        string connStr = DapperContext.Current.Configuration["Data:DefaultConnection:ConnectionString"];
        public async Task<APIResult> AddOrDeleteAttachment(AttachmentMngDto attachmentMngDto)
        {

            string spName = @"up_RMMT_NOT_AddOrDeleteAttachment_U";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@IdAttachment", attachmentMngDto.Id, DbType.Int64);
            dp.Add("@IdNotice", attachmentMngDto.RefId, DbType.Int64);
            dp.Add("@Type", attachmentMngDto.Type, DbType.String);
            dp.Add("@AttachName", attachmentMngDto.AttachName, DbType.String);
            dp.Add("@Url", attachmentMngDto.Url, DbType.String);

            using (var conn = new SqlConnection(DapperContext.Current.SqlConnection))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        IEnumerable<string> idResult = await conn.QueryAsync<string>(spName, dp, tran, null, CommandType.StoredProcedure);
                        tran.Commit();
                        APIResult result = new APIResult { Body = CommonHelper.EncodeDto<string>(idResult), ResultCode = ResultType.Success, Msg = "" };
                        return result;
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
            }

        }

    }
}
