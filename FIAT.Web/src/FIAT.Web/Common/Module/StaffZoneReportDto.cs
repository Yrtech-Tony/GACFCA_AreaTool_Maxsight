using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.Web.Common.Module
{
    public class StaffZoneReportDto
    {
        public string PostCode { get; set; }
        public int PostCnt { get; set; }
        public double DMSRate { get; set; }
        public int QuitCnt { get; set; }
        public double AuthenticateRate { get; set; }
    }
    public class StaffPostInfo
    {
        public string BName { get; set; }
        public string AreaName { get; set; }
        public string ZoneName { get; set; }
        public string DisName { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string Status { get; set; }
        public string DMSYN { get; set; }
        public string AuthenticateYN { get; set; }
        public string PartTimeJobYN { get; set; }
        public string PartTimeJob { get; set; }
        public string DMSYN2 { get; set; }
        public string AuthenticateYN2 { get; set; }
        public string IDNo { get; set; }
    }
    public class StaffInfo
    {
        public string IDNo { get; set; }
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

    }
    public class StaffZoneReport
    {
        public List<StaffZoneReportDto> ZoneReportList { get; set; } = new List<StaffZoneReportDto>();
        public List<StaffInfo> StaffPostInfoList { get; set; } = new List<StaffInfo>();
    }
}
