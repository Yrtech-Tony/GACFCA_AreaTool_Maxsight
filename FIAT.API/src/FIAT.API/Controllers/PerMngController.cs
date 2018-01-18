using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIAT.API.Service;
using FIAT.API.Models;
using FIAT.API.Models.PerMngDto;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.API.Controllers
{
    [Authorize]
    [Route("fiat/api/v1/[controller]/[action]")]
    public class PerMngController : Controller
    {
        IPerMngService _perMngService;
        public PerMngController(IPerMngService perMngService)
        {
            _perMngService = perMngService;
        }

        [HttpGet]
        [ActionName("GetPlansByOrganization")]
        public Task<APIResult> GetPlansByOrganization(int busId, int repId, int areaId, int zoneId, string vType, string status)
        {
            return _perMngService.GetPlansByOrganization(busId, repId, areaId, zoneId, vType, status);

        }
        [HttpGet]
        [ActionName("GetDistributorByBatch")]
        public Task<APIResult> GetDistributorByBatch(int batchId)
        {
            return _perMngService.GetDistributorByBatch(batchId);
        }

        [HttpPost]
        [ActionName("StaffInfoReg")]
        public async Task<APIResult> StaffInfoReg([FromBody]StuffInfoParams param)
        {
            return await _perMngService.StaffInfoReg(param);
        }

        [HttpGet]
        [ActionName("GetStaffAndPostInfo")]
        public Task<APIResult> GetStaffAndPostInfo(int busType, int disId, string yearMonth, string postCode)
        {
            return _perMngService.GetStaffAndPostInfo(busType, disId, yearMonth, postCode);
        }
        //区域报告导出
        [HttpGet]
        [ActionName("GetStaffZoneReport")]
        public Task<APIResult> GetStaffZoneReport(string yearMonth, string zoneId, int busType)
        {
            return _perMngService.GetStaffZoneReport(yearMonth, zoneId, busType);
        }
        //人员登记进度查询
        [HttpGet]
        [ActionName("GetStaffRegProgress")]
        public Task<APIResult> GetStaffRegProgress(int busId, int repId, int areaId, int zoneId, string yearMonth)
        {
            return _perMngService.GetStaffRegProgress(busId, repId, areaId, zoneId, yearMonth);
        }
        //人员区域报告
        [HttpGet]
        [ActionName("GetStaffRegProgressForZone")]
        public Task<APIResult> GetStaffRegProgressForZone(int busId, int repId, int areaId, int zoneId, string yearMonth)
        {
            return _perMngService.GetStaffRegProgressForZone(busId, repId, areaId, zoneId, yearMonth);
        }
        //开始检核
        [HttpGet]
        [ActionName("StartCheckStaff")]
        public Task<APIResult> StartCheckStaff(int busType, int disId,string yearMonth)
        {
            return _perMngService.StartCheckStaff(busType, disId,yearMonth);
        }
        //完成提交
        [HttpPost]
        [ActionName("UpdateStaffInfo")]
        public async Task<APIResult> UpdateStaffInfo([FromBody]StuffInfoParams param)
        {
            return await _perMngService.UpdateStaffInfo(param);
        }
        [HttpGet]
        [ActionName("DisForStaff")]
        public async Task<APIResult> GetDisForStaff(int bId, int rId, int zId, int aId, string m, string dName)
        {
            return await _perMngService.GetDisForStaff(bId,rId,zId,aId,m, dName);
        }
        [HttpGet]
        [ActionName("MonthForStaff")]
        public async Task<APIResult> GetMonthForStaff(int bId, int rId, int zId, int aId)
        {
            return await _perMngService.GetMonthForStaff(bId, rId, zId, aId);
        }
        [HttpPost]
        [ActionName("NoDisForStaff")]
        public async Task<APIResult> SaveDisForStaff([FromBody]StaffDisParamDto param)
        {
            return await _perMngService.SaveDisForStaff(param);
        }
    }
}
