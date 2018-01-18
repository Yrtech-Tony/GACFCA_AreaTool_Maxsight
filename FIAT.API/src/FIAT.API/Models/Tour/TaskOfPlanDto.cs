namespace FIAT.API.Models.Tour
{
    public class TaskOfPlanDto
    {
        public int TPId { get; set; }
        public string TCCode { get; set; }
        public string TCTitle { get; set; }
        public string TPStatus { get; set; }
        public string TPType { get; set; }
        public string SourceType { get; set; }
        public string PTitle { get; set; }
        public string UserName { get; set; }
        public string TCWeight { get; set; }
        public string ItemScore { get; set; }
        public string STScore { get; set; }
        public string LastTCScore { get; set; }
        public string LastPScore { get; set; }
    }
}
