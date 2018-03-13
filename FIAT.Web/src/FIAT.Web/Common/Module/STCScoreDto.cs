using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.Web.Common.Module
{
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
