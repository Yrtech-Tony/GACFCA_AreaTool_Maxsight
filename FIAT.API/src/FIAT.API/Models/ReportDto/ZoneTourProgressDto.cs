using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.ReportDto
{
    public class ZoneTourProgressDto
    {
        public List<BTourProgressDto> BList { get; set; } = new List<BTourProgressDto>();
        public List<RTourProgressDto> RList { get; set; } = new List<RTourProgressDto>();
        public List<ATourProgressDto> AList { get; set; } = new List<ATourProgressDto>();
        public List<ZTourProgressDto> ZList { get; set; } = new List<ZTourProgressDto>();
        public List<DTourProgressDto> DList { get; set; } = new List<DTourProgressDto>();

    }
    //业务类型
    public class BTourProgressDto
    {
        public string BName { get; set; }
        public int FinishDisCnt { get; set; }
        public int TotalDisCnt { get; set; }
        public string Rate { get; set; }
    }
    //汇报区
    public class RTourProgressDto
    {
        public string RName { get; set; }
        public int FinishDisCnt { get; set; }
        public int TotalDisCnt { get; set; }
        public string Rate { get; set; }
    }
    //大区
    public class ATourProgressDto
    {
        public string AName { get; set; }
        public int FinishDisCnt { get; set; }
        public int TotalDisCnt { get; set; }
        public string Rate { get; set; }
    }
    //小区
    public class ZTourProgressDto
    {
        public string RName { get; set; }
        public string ZName { get; set; }
        public int FinishDisCnt { get; set; }
        public int TotalDisCnt { get; set; }
        public string Rate { get; set; }
    }
    //经销商
    public class DTourProgressDto
    {
        public string DisName { get; set; }
        public string DisCode { get; set; }
        public string ZoneName { get; set; }
        public int FinishCnt { get; set; }
        public int TotalCnt { get; set; }
        public string Rate { get; set; }
        public string EdateTime { get; set; }
        public string CollAddr { get; set; }
    }

}
