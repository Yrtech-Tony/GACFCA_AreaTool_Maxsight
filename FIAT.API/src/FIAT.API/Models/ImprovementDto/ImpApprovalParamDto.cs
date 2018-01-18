using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.ImprovementDto
{
    public class ImpApprovalParamDto
    {
        public string ImprovementContent { get; set; }
        public string ExpectedTime { get; set; }
        public int ImprovementId { get; set; }
        public int InUserId { get; set; }
        public List<AttachDto> AttachList{ get; set; }
        public string SaveStatus { get; set; }
    }
}
