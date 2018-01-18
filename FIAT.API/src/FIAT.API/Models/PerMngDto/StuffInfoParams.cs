using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.PerMngDto
{
    public class StuffInfoParams
    {
        public string YearMonth { get; set; }
        public int DisId { get; set; }
        public string BusType { get; set; }
        public int UserId { get; set; }

        public string Position { get; set; }

        public string SaveNode { get; set; }
        public List<StaffInfo> StaffInfoList { get; set; }

    }
    public class StaffInfo
    {
        public string IDNo { get; set; }
        public string OIDNo { get; set; }
        public string PostCode { get; set; }
        public string LevelM1 { get; set; }
        public string LevelM2 { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string AuthenticateYN { get; set; }
        public string DMSYN { get; set; }
        public string PartTimeJobYN { get; set; }
        public string PartTimeJob1 { get; set; }
        public string DMSYN1 { get; set; }
        public string AuthenticateYN1 { get; set; }
        public string PartTimeJob2 { get; set; }
        public string DMSYN2 { get; set; }
        public string AuthenticateYN2 { get; set; }
        public string PartTimeJob3 { get; set; }
        public string DMSYN3 { get; set; }
        public string AuthenticateYN3 { get; set; }
        public string BName { get; set; }
        public string AreaName { get; set; }
        public string ZoneName { get; set; }
        public string DisName { get; set; }
        public string RowStatus { get; set; }
        public string YearMonth { get; set; }
        public string BusType { get; set; }
        public string DisCode { get; set; }

    }
}
