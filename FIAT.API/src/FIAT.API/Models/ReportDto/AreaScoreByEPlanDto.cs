using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.ReportDto
{
    public class AreaReportDto
    {
        public AreaReportDto()
        {
            BStatislist = new List<AreaStatisDto>();
            EStatislist = new List<AreaStatisDto>();
            RStatislist = new List<AreaStatisDto>();
            ZStatislist = new List<AreaStatisDto>();
            RSBEPlist = new List<AreaScoreByEPlanDto>();
            ASBEPlist = new List<AreaScoreByEPlanDto>();
        }
        public string ScoreProcess { get; set; }
        public string TCount { get; set; }
        public string FCount { get; set; }
        public string RCount { get; set; }
        public string ATScore { get; set; }

        public List<AreaStatisDto> BStatislist { get; set; }
        public List<AreaStatisDto> EStatislist { get; set; }
        public List<AreaStatisDto> RStatislist { get; set; }
        public List<AreaStatisDto> ZStatislist { get; set; }
        public List<AreaScoreByEPlanDto> RSBEPlist { get; set; }
        public List<AreaScoreByEPlanDto> ASBEPlist { get; set; }
    }
    public class AreaStatisDto
    {
        public string DisId { get; set; }
        public string DisName { get; set; }
        public string DisTCount { get; set; }
        public string DisCCount { get; set; }
        public string DisNCount { get; set; }
        public string DisProgress { get; set; }
        public string DisTScore { get; set; }
    }
    public class AreaScoreByEPlanDto
    {
        public AreaScoreByEPlanDto()
        {
            SDisList = new List<DisSCoreByEPlanDto>();
            ASBEPlist = new List<AreaScoreByEPlanDto>();
        }
        public string TCScore { get; set; }
        public string TCId { get; set; }
        public string TCTitle { get; set; }
        public string TCWeight { get; set; }
        public string AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string ATScore { get; set; }
        public string ScoreProcess { get; set; }

        public List<DisSCoreByEPlanDto> SDisList { get; set; }
        public List<AreaScoreByEPlanDto> ASBEPlist { get; set; }
    }
    public class DisSCoreByEPlanDto
    {
        public string ItemScore { get; set; }
        public string TCId { get; set; }
        public string SDisId { get; set; }
        public string SDisName { get; set; }
        public string SDisCode { get; set; }
        public string AreaId { get; set; }
        public string STScore { get; set; }
    }
}
