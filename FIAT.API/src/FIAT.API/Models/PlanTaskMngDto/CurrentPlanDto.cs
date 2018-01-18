using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.PlanTaskMngDto
{
    public class CurrentPlanDto
    {
        public string Batch { get; set; }
        public CurrentPlanDto()
        {
            PList = new List<PDto>();
            TPList = new List<TPDto>();
            TIList = new List<TIDto>();
        }
        public List<PDto> PList { get; set; }
        public List<TPDto> TPList { get; set; }
        public List<TIDto> TIList { get; set; }
    }

    public class PDto
    {
        public string PId { get; set; }
        public string PTitle { get; set; }
        public string DisName { get; set; }
        public string LastPScore { get; set; }
    }
    public class TPDto
    {
        public string TPId { get; set; }
        public string PTitle { get; set; }
        public string DisName { get; set; }
        public string TCTitle { get; set; }
        public string LastTCScore { get; set; }
    }
    public class TIDto
    {
        public string TPId { get; set; }
        public string TIId { get; set; }
        public string PTitle { get; set; }
        public string DisName { get; set; }
        public string TCTitle { get; set; }
        public string TITitle { get; set; }
        public string LastITScore { get; set; }
    }
    public class ExcelUploadLastScorePatams
    {
        public string InUserId { get; set; }
        public List<PDto> LastPScore { get; set; }
        public List<TPDto> LastTCScore { get; set; }
        public List<TIDto> LastTIScore { get; set; }
    }
}
