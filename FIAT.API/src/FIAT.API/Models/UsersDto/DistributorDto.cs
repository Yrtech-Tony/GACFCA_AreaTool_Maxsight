using System;

namespace FIAT.API.Models.DistributorDto
{
    public class DistributorDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ZoneType { get; set; }
        public bool UseYN { get; set; }
        public int InUserId { get; set; }
        public DateTime InDateTime { get; set; }
        public string UserName { get; set; }// 登录账号
    }
}
