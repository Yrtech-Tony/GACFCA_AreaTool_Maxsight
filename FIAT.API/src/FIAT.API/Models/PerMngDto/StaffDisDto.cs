using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.PerMngDto
{
    public class StaffDisDto
    {
        public string AreaName { get; set; }
        public string DisCode { get; set; }
        public string DisName { get; set; }

        public string DisId { get; set; }
    }
    public class StaffDisParamDto
    {
        public string InUserId { get; set; }
        public string YearMonth { get; set; }
        public string BusType { get; set; }
        public List<StaffDisListDto> DisList { get; set; }
    }
    public class StaffDisListDto
    {
        public string DisCode { get; set; }
    }
}
