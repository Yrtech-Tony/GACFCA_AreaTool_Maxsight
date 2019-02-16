using System.Collections.Generic;

namespace FIAT.Web.Common.Module
{
    public class UserDto
    {
        public UserDto()
        {
            BizList = new List<BizDto>();
            DepartmentList = new List<DepartmentDto>();
            //ImpPlanStatusList = new List<ImpPlanStatusDto>();
            //ImpResultStatusList = new List<ImpResultStatusDto>();
            ImpStatusList = new List<ImpStatusDto>();
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public string UserTypeName { get; set; }
        public string DisName { get; set; }
        public string OrgBizName { get; set; }
        public string OrgBizId { get; set; }
        public string OrgRepName { get; set; }
        public string OrgRepId { get; set; }
        public string OrgZionName { get; set; }
        public string OrgZionId { get; set; }
        public string OrgAreaName { get; set; }
        public string OrgAreaId { get; set; }
        public string OrgServerName { get; set; }
        public string OrgServerId { get; set; }
        public string OrgDepartmentName { get; set; }
        public string OrgDepartmentId { get; set; }

        public List<BizDto> BizList { get; set; }
        public List<DepartmentDto> DepartmentList { get; set; }
        //public List<ImpPlanStatusDto> ImpPlanStatusList { get; set; }
        //public List<ImpResultStatusDto> ImpResultStatusList { get; set; }
        public List<ImpStatusDto> ImpStatusList { get; set; }
    }

    public class BizDto
    {
        public BizDto()
        {
            RepList = new List<RepDto>();
        }
        public string BId { get; set; }
        public string BCode { get; set; }
        public string BName { get; set; }
        public List<RepDto> RepList { get; set; }
    }

    public class RepDto
    {
        public RepDto()
        {
            ZionList = new List<ZionDto>();
        }
        public string EId { get; set; }
        public string ECode { get; set; }
        public string EName { get; set; }
        public string BId { get; set; }
        public List<ZionDto> ZionList { get; set; }
    }
    
    public class ImpStatusDto
    {
        public string ImpStatusCode { get; set; }
        public string ImpStatusName { get; set; }
        public int StatusKind { get; set; }
    }
}
