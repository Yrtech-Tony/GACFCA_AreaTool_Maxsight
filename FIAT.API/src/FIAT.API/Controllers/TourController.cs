using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIAT.API.Service;
using FIAT.API.Models;
using FIAT.API.Models.Tour;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.API.Controllers
{
    [Authorize]
    [Route("fiat/api/v1/[controller]/[action]")]
    public class TourController : Controller
    {
        ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        //查询有计划任务的经销商
        [HttpGet]
        [ActionName("GetTourDistributorList")]
        public Task<APIResult> GetTourDistributorList(int batch, int userId, string searchType, string disCode = "%", string disName = "%")
        {
            return _tourService.GetTourDistributorList(batch, userId, searchType, disCode, disName);
        }
        //查询一个经销商下的所有任务
        [HttpGet]
        [ActionName("GetTaskListByDisId")]
        public Task<APIResult> GetTaskListByDisId(int batch, int disId, string startTime, string endTime, string status, string sourceType, int inUserId = 0)
        {
            return _tourService.GetTaskListByDisId(batch, disId, startTime, endTime, status, sourceType, inUserId);
        }
        //查询一个任务下的体系列表
        [HttpGet]
        [ActionName("GetItemListByTaskId")]
        public Task<APIResult> GetItemListByTaskId(int taskId, string operation, string collAddr)
        {
            return _tourService.GetItemListByTaskId(taskId, operation, collAddr);
        }
        [HttpPost]
        [ActionName("SaveCheckResult")]
        public Task<APIResult> SaveCheckResult([FromBody]ScoreCheckResultParam param)
        {
            return _tourService.SaveCheckResult(param);
        }
        [HttpPost]
        [ActionName("SaveItemApproalYN")]
        public Task<APIResult> SaveItemApproalYN([FromBody]ItemApproalParams param)
        {
            return _tourService.SaveItemApproalYN(param);
        }
        //自定义任务
        [HttpGet]
        [ActionName("GetCustomizedTaskByTaskId")]
        public Task<APIResult> GetCustomizedTaskByTaskId(int taskId, string operation, string collAddr)
        {
            return _tourService.GetCustomizedTaskByTaskId(taskId, operation, collAddr);
        }
        [HttpPost]
        [ActionName("CustomizedTaskCheck")]
        public Task<APIResult> CustomizedTaskCheck([FromBody]CustomizedTaskDto param)
        {
            return _tourService.CustomizedTaskCheck(param);
        }

        [HttpPost]
        [ActionName("UpLoadAllScoreInfo")]
        public Task<APIResult> UpLoadAllScoreInfo([FromBody]AllTaskInfoRegLstDto param)
        {
            return _tourService.UpLoadAllScoreInfo(param);
        }

        //将保存在本地数据上传到数据库
        [HttpPost]
        [ActionName("UploadLocalDB")]
        public Task<APIResult> UploadLocalDB([FromBody]LocalDBUploadParams param)
        {
            return _tourService.UploadLocalDB(param);
        }

        //查询一个经销商下的所有任务
        [HttpGet("{disCode}/{startTime}/{endTime}/{status}/{Pid}")]
        [ActionName("GetTaskListByDisIdForExcel")]
        public Task<APIResult> GetTaskListByDisIdForExcel(string disCode, string startTime, string endTime, string status, string pid)
        {
            return _tourService.GetTaskListByDisIdForExcel(disCode, startTime, endTime, status, pid);
        }
        [HttpPost]
        [ActionName("RegCustomizedImpItem")]
        public Task<APIResult> InsertCustomizedImpItem([FromBody]CustomizedImpItemDto param)
        {
            return _tourService.InsertCustomizedImpItem(param);
        }
        [HttpGet]
        [ActionName("GetAllPlansByDisId")]
        public Task<APIResult> SearchAllPlansByDisId(string inUserId, string userType, string disId)
        {
            return _tourService.SearchAllPlansByDisId(inUserId, userType, disId);
        }
        #region Tony 
        /// <summary>
        /// 检核时如果是卖车包或者DMS的时候输入销售顾问的信息进行保存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SavePlansPosition")]
        public Task<APIResult> SavePlansPosition([FromBody]PlansPositionDto param)
        {
            return _tourService.SavePlansPosition(param);
        }
        /// <summary>
        /// 检核时如果是卖车包或者DMS的时候输入销售顾问的信息进行查询
        /// </summary>
        /// <param name="inUserId"></param>
        /// <param name="userType"></param>
        /// <param name="disId"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetPlansPosition")]
        public Task<APIResult> GetPlansPosition(string batch, string disId)
        {
            return _tourService.GetPlansPosition(batch, disId);
        }
        #endregion

    }
}
