using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.StatisticDto
{
    public class ExcelAfterSalesDataParams
    {
        public string InUserId { get; set; }
        public List<ExcelAfterSalesData> AfterSalesData { get; set; }
    }
    public class ExcelAfterSalesData
    {
        public string AreaCode { get; set; }
        public string DisCode { get; set; }
        public string LTypeName { get; set; }
        public string STypeName { get; set; }
        public string YearMonth { get; set; }
        public string Data { get; set; }
    }
}
