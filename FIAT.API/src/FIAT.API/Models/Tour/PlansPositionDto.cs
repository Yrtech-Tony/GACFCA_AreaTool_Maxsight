using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.Tour
{
    public class PlansPositionDto
    {
        public int Id { get; set; }
        public int Batch { get; set; }
        public int DisId { get; set; }
        public string SalesConsultant { get; set; }
        public string SalesManager { get; set; }
        public string SalesInside { get; set; }
        public string InUserid { get; set; }
        public DateTime InDateTime { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime ModifyDatetime { get; set; }
    }
    

}
