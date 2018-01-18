using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIAT.API.Service;
using FIAT.API.Models;
using FIAT.API.Models.ReportDto;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.API.Controllers
{
    [Authorize]
    [Route("fiat/api/v1/[controller]/[action]")]
    public class ReportController : Controller
    {
        IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        [ActionName("GetAttachmentByUserId")]
        public Task<APIResult> GetAttachmentByUserId(int userId, string sourceType, string sDate, string eDate)
        {
            return _reportService.GetAttachmentByUserId(userId, sourceType, sDate, eDate);
        }
        [HttpPost]
        [ActionName("SaveReportAttachment")]
        public Task<APIResult> SaveReportAttachment([FromBody]SaveReportAttachdto param)
        {
            return _reportService.SaveReportAttachment(param);
        }
        [HttpPost]
        [ActionName("UpdateAttachmentDownloadCnt")]
        public Task<APIResult> UpdateAttachmentDownloadCnt([FromBody]int id)
        {
            return _reportService.UpdateAttachmentDownloadCnt(id);
        }

        [HttpGet]
        [ActionName("GetPlansListForExcelDownload")]
        public Task<APIResult> GetPlansListForExcelDownload(string UserId, string DisId,int Batch)
        {
            return _reportService.GetPlansListForExcelDownload(UserId, DisId, Batch);
        }
        [HttpGet]
        [ActionName("AreaScoreOnthePlan")]
        public Task<APIResult> GetTCScoreOFAIdByEPlan(string AreaId, string BatchId)
        {
            return _reportService.SearchTCScoreOFAIdByEPlan(AreaId, BatchId);
        }

        [HttpGet]
        [ActionName("GetTouProgressForZone")]
        public Task<APIResult> GetTouProgressForZone(int busId, int repId, int areaId, int zoneId, int batchId)
        {
            return _reportService.GetTouProgressForZone(busId, repId, areaId, zoneId, batchId);
        }
        [HttpGet]
        [ActionName("AreaScoresOnthePlan")]
        public Task<APIResult> GetTCScoreOFAIdsByEPlan(string RegionId, string BatchId,string AreaId)
        {
            return _reportService.SearchTCScoreOFAIdsByEPlan(RegionId, BatchId,AreaId);
        }

        [HttpGet]
        [ActionName("TourBaseScoreByDisId")]
        public Task<APIResult> GetTourBaseScoreByDisId(string UserId, string DisId, int Batch)
        {
            return _reportService.GetTourBaseScoreByDisId(UserId, DisId, Batch);
        }
    }
}
