using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.NotifiMngDto
{
    public class NoticeReadStatusDto
    {
        public int NoticeId { get; set; }
        public int DisId { get; set; }
        public int DepartId { get; set; }
        public int InUserId { get; set; }
    }
}
