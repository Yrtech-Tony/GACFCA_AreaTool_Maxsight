using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.ReportDto
{
    public class SaveReportAttachdto
    {
        public int UserId { get; set; }
        public List<ReportAttachmentDto> AttachList { get; set; }

    }
}
