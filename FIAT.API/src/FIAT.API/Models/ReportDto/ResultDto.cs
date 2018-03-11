using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.ReportDto
{
    public class ResultDto
    {
        public ResultDto()
        {
            RAPList = new List<RAProgressDto>();
            RATCList = new List<RATCScoreDto>();
            STCList = new List<STCScoreDto>();
        }
        public string YN { get; set; }
        public List<RAProgressDto> RAPList { get; set; }
        public List<RATCScoreDto> RATCList { get; set; }
        public List<STCScoreDto> STCList { get; set; }
    }
    public class RAProgressDto
    {
        public string DisId { get; set; }
        public string DisName { get; set; }
        public string DisTCount { get; set; }
        public string DisCCount { get; set; }
        public string DisNCount { get; set; }
        public string DisProgress { get; set; }
        public string DisTScore { get; set; }
        public string DType { get; set; }
    }
    public class RATCScoreDto
    {
        public string TCScore { get; set; }
        public string TCId { get; set; }
        public string TCTitle { get; set; }
        public string TCWeight { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public string ATScore { get; set; }
        public string DType { get; set; }
    }
    public class STCScoreDto
    {
        public string ItemScore { get; set; }
        public string TCId { get; set; }
        public string SDisId { get; set; }
        public string SDisName { get; set; }
        public string SDisCode { get; set; }
        public string SmallAreaId { get; set; }

        public string SmallAreaCode { get; set; }

        public string SmallAreaName { get; set; }

        public string MidAreaId { get; set; }

        public string MidAreaCode { get; set; }

        public string MidAreaName { get; set; }

        public string BigAreaId { get; set; }

        public string BigAreaCode { get; set; }
        public string BigAreaName { get; set; }
        public string AreaId { get; set; }
        public string STScore { get; set; }
    }
}
