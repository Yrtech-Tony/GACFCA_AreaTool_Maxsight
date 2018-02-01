using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIAT.Web.Common;
using FIAT.Web.Common.Module;
using Npoi.Core.SS.UserModel;
using Npoi.Core.SS.Util;
using System.IO;
using Npoi.Core.HSSF.Util;
using Microsoft.AspNetCore.Hosting;
using Npoi.Core.XSSF.UserModel;
using Npoi.Core.HSSF.UserModel;
using System.Net.Http;
using System.Drawing;
using System.IO.Compression;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.Web.Controllers
{
    public class ReportController : Controller
    {

        private IHostingEnvironment _environment;
        public ReportController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        //[Authorize]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult REP010()
        {
            DateTime now = DateTime.Now;
            ViewBag.CurrentDate = now.ToString("yyyy-MM-dd");
            ViewBag.FirstDay = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            return View();
        }
        public async Task<ActionResult> DownLoadForRename(string fileName = "", string sourcepath = "")
        {
            string contentType = "application/octet-stream";
            byte[] bytes = await CommonHelper.Current.HttpGetFileBytes(sourcepath);
            return this.File(bytes, contentType, fileName);
        }
        [HttpPost]
        public async Task<ActionResult> UploadScoreAjaxDown(string disCode, string startTime, string endTime, string status, string Pid, string disname, string name, string visitTypeName, string visitDateTime)
        {
            try
            {
                string result = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetAPIBaseUrl + "Tour/GetTaskListByDisIdForExcel/" + disCode + "/" + startTime + "/" + endTime + "/" + status + "/" + Pid);
                var apiResult = CommonHelper.DecodeString<APIResult>(result);
                
                if (apiResult.ResultCode == ResultType.Success)
                {
                    ExcelResult er = CommonHelper.DecodeString<ExcelResult>(apiResult.Body);
                    List<ResultExcelDto> list = er.ResultList;
                    string discode = list.FirstOrDefault().DisCode;
                    string pDisInfo = list.FirstOrDefault().PId;
                    string aDisName = string.Empty;
                    string rDisName = string.Empty;
                    string eDisName = string.Empty;
                    if (!string.IsNullOrWhiteSpace(pDisInfo) && pDisInfo.Split(',').Length == 3)
                    {
                        aDisName = pDisInfo.Split(',')[0];
                        rDisName = pDisInfo.Split(',')[1];
                        eDisName = pDisInfo.Split(',')[2];
                    }

                    List<LosePic> lpList = er.LPicList;
                    if (lpList.Count % 2 == 1)
                    {
                        lpList.Add(new LosePic());
                    }
                    //未通过体系
                    List<ResultExcelDto> unlist2 = (from a in list where a.PassYN == "0" select a).ToList();

                    List<string> tiId = (from a in unlist2 select a.TITitle).Distinct().ToList();

                    //任务卡得分
                    var ts = (from a in list select new { TCId = a.TCId, TCTitle = a.TCTitle, TCWeight = a.TCWeight, ItemScore = a.ItemScore, ItemQty = a.ItemQty }).Distinct().ToList();
                    string totalScore = list.FirstOrDefault().STScore;

                    var uploads = Path.Combine(_environment.WebRootPath, "Template");
                    //var newFile = Path.Combine(uploads, list[0].PTitle + "_" + discode + "_" + disname + "_" + DateTime.Now.ToString("yyyyMMdd hhmmss") + ".xlsx");
                    var reportname = list[0].PTitle + "_" + visitTypeName + "_" + discode + "_" + disname;
                    var newFile = Path.Combine(uploads, reportname + ".xlsx");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
                    {

                        IWorkbook workbook = new XSSFWorkbook();

                        //ISheet sheet1 = workbook.CreateSheet("检核结果");
                        ISheet sheet1 = workbook.CreateSheet(reportname);

                        sheet1.FitToPage = true;
                        sheet1.PrintSetup.FitWidth = 1;
                        sheet1.PrintSetup.FitHeight = 0;
                        sheet1.PrintSetup.PaperSize = 9;
                        sheet1.SetMargin(MarginType.RightMargin, 0);
                        //sheet1.PrintSetup.NoOrientation = true;
                        
                        IFont font_b = workbook.CreateFont();
                        font_b.FontName = "微软雅黑";
                        font_b.FontHeightInPoints = 10;

                        IFont font_c = workbook.CreateFont();
                        font_c.FontName = "微软雅黑";
                        font_c.FontHeightInPoints = 10;
                        font_c.Color = HSSFColor.White.Index;
                        font_c.IsBold = true;

                        byte[] rgb5 = new byte[3] { 217, 217, 217 };
                        byte[] rgb6 = new byte[3] { 221, 217, 196 };
                        byte[] rgb7 = new byte[3] { 32, 55, 100 };

                        var style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                        //style1.FillForegroundColor = HSSFColor.Grey25Percent.Index;

                        byte[] rgb = new byte[3] { 116, 163, 210 };
                        style1.SetFillForegroundColor(new XSSFColor(rgb));

                        style1.FillPattern = FillPattern.SolidForeground;
                        style1.Alignment = HorizontalAlignment.Center;
                        style1.BorderLeft = BorderStyle.Thin;
                        style1.BorderBottom = BorderStyle.Thin;
                        style1.BorderRight = BorderStyle.Thin;
                        style1.BorderTop = BorderStyle.Thin;
                        style1.SetFont(font_b);

                        var style_Stand = workbook.CreateCellStyle();
                        style_Stand.Alignment = HorizontalAlignment.Left;
                        style_Stand.WrapText = true;
                        style_Stand.SetFont(font_b);
                        style_Stand.BorderLeft = BorderStyle.Thin;
                        style_Stand.BorderBottom = BorderStyle.Thin;
                        style_Stand.BorderRight = BorderStyle.Thin;
                        style_Stand.BorderTop = BorderStyle.Thin;
                        
                        var style_b2 = workbook.CreateCellStyle();
                        style_b2.WrapText = true;
                        style_b2.VerticalAlignment = VerticalAlignment.Center;
                        style_b2.SetFont(font_b);
                        style_b2.BorderLeft = BorderStyle.Thin;
                        style_b2.BorderBottom = BorderStyle.Thin;
                        style_b2.BorderRight = BorderStyle.Thin;
                        style_b2.BorderTop = BorderStyle.Thin;

                        var style_b3 = workbook.CreateCellStyle();
                        style_b3.WrapText = true;
                        style_b3.VerticalAlignment = VerticalAlignment.Center;
                        style_b3.SetFont(font_b);

                        var style2 = workbook.CreateCellStyle();
                        style2.Alignment = HorizontalAlignment.Left;
                        style2.SetFont(font_b);
                        style2.BorderLeft = BorderStyle.Thin;
                        style2.BorderBottom = BorderStyle.Thin;
                        style2.BorderRight = BorderStyle.Thin;
                        style2.BorderTop = BorderStyle.Thin;

                        var style3 = workbook.CreateCellStyle();
                        style3.Alignment = HorizontalAlignment.Right;
                        style3.SetFont(font_b);

                        var style4 = workbook.CreateCellStyle();
                        style4.Alignment = HorizontalAlignment.Center;
                        style4.VerticalAlignment = VerticalAlignment.Center;
                        style4.SetFont(font_b);
                        style4.BorderLeft = BorderStyle.Thin;
                        style4.BorderBottom = BorderStyle.Thin;
                        style4.BorderRight = BorderStyle.Thin;
                        style4.BorderTop = BorderStyle.Thin;

                        var style5 = workbook.CreateCellStyle();
                        style5.Alignment = HorizontalAlignment.Center;
                        style5.VerticalAlignment = VerticalAlignment.Center;
                        style5.SetFont(font_b);

                        var style6 = workbook.CreateCellStyle();
                        style6.Alignment = HorizontalAlignment.Center;
                        style6.VerticalAlignment = VerticalAlignment.Center;
                        style6.SetFont(font_b);

                        var style_t = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_t.Alignment = HorizontalAlignment.Center;
                        style_t.SetFillForegroundColor(new XSSFColor(rgb));
                        //style_t.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                        style_t.FillPattern = FillPattern.SolidForeground;

                        IFont font_t = workbook.CreateFont();
                        font_t.FontHeightInPoints = 18;
                        font_t.FontName = "微软雅黑";
                        style_t.SetFont(font_t);

                        //标题
                        IRow row_t = sheet1.CreateRow(0);
                        ICell cell_t = row_t.CreateCell(0);
                        cell_t.SetCellValue("经销商报告");
                        CellRangeAddress region_t = new CellRangeAddress(0, 0, 0, 6);
                        cell_t.CellStyle = style_t;
                        sheet1.AddMergedRegion(region_t);

                        IRow row = sheet1.CreateRow(1);
                        ICell cell = row.CreateCell(0);
                        row.CreateCell(1).CellStyle = style2;
                        cell.SetCellValue("经销商代码:" + discode);
                        CellRangeAddress region = new CellRangeAddress(1, 1, 0, 1);
                        cell.CellStyle = style2;
                        sheet1.AddMergedRegion(region);
                        ICell cell_11 = row.CreateCell(2);
                        row.CreateCell(3).CellStyle=style2;
                        row.CreateCell(4).CellStyle = style2;
                        row.CreateCell(5).CellStyle = style2;
                        row.CreateCell(6).CellStyle = style2;
                        cell_11.SetCellValue("经销商名称：" + disname);
                        CellRangeAddress region_1 = new CellRangeAddress(1, 1, 2, 6);
                        cell_11.CellStyle = style2;
                        sheet1.AddMergedRegion(region_1);

                        IRow row1 = sheet1.CreateRow(2);
                        ICell cel2 = row1.CreateCell(0);
                        row1.CreateCell(1).CellStyle = style2;
                        cel2.SetCellValue("所属小区：" + aDisName);
                        cel2.CellStyle = style2;
                        CellRangeAddress region2 = new CellRangeAddress(2, 2, 0, 1);
                        sheet1.AddMergedRegion(region2);
                        ICell cel2_1 = row1.CreateCell(2);
                        row1.CreateCell(3).CellStyle = style2;
                        row1.CreateCell(4).CellStyle = style2;
                        cel2_1.SetCellValue(rDisName == "-" ? "所属大区：" + eDisName : "所属区域：" + rDisName);
                        cel2_1.CellStyle = style2;
                        CellRangeAddress region2_1 = new CellRangeAddress(2, 2, 2, 4);
                        sheet1.AddMergedRegion(region2_1);
                        ICell cel2_2 = row1.CreateCell(5);
                        row1.CreateCell(6).CellStyle = style2;
                        cel2_2.SetCellValue(rDisName == "-" ? "" : (eDisName == "-" ? "" : "所属大区：" + eDisName));
                        cel2_2.CellStyle = style2;
                        CellRangeAddress region2_2 = new CellRangeAddress(2, 2, 5, 6);
                        sheet1.AddMergedRegion(region2_2);

                        IRow row0 = sheet1.CreateRow(3);
                        ICell cel0 = row0.CreateCell(0);
                        row0.CreateCell(1).CellStyle = style2;
                        cel0.SetCellValue("检核类型：" + visitTypeName);
                        cel0.CellStyle = style2;
                        CellRangeAddress region3 = new CellRangeAddress(3, 3, 0, 1);
                        sheet1.AddMergedRegion(region3);
                        ICell cel0_1 = row0.CreateCell(2);
                        row0.CreateCell(3).CellStyle = style2;
                        row0.CreateCell(4).CellStyle = style2;
                        cel0_1.SetCellValue("巡检月份：" + (list.Count == 0 ? "" : list[0].PTitle));
                        cel0_1.CellStyle = style2;
                        CellRangeAddress region3_1 = new CellRangeAddress(3, 3, 2, 4);
                        sheet1.AddMergedRegion(region3_1);
                        ICell cel0_2 = row0.CreateCell(5);
                        row0.CreateCell(6).CellStyle = style2;
                        cel0_2.SetCellValue("检核时间：" + visitDateTime);
                        cel0_2.CellStyle = style2;
                        CellRangeAddress region3_2 = new CellRangeAddress(3, 3, 5, 6);
                        sheet1.AddMergedRegion(region3_2);

                        if (visitTypeName == "D" || visitTypeName == "E")
                        {
                            //DMS 和 卖车宝
                            string plansPosition = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetAPIBaseUrl + "Tour/GetPlansPosition/" + disCode + "/" + Pid);
                            APIResult planAPIResult = CommonHelper.DecodeString<APIResult>(plansPosition);
                            Dictionary<string, object> dic = CommonHelper.DecodeString<Dictionary<string,object>>(planAPIResult.Body); 
                            IRow row4 = sheet1.CreateRow(4);
                            ICell cel4_0 = row4.CreateCell(0);
                            row4.CreateCell(1).CellStyle = style2;
                            cel4_0.SetCellValue("销售经理：" + dic["SalesManager"]);
                            cel4_0.CellStyle = style2;
                            CellRangeAddress region4_0 = new CellRangeAddress(4, 4, 0, 1);
                            sheet1.AddMergedRegion(region4_0);

                            ICell cel4_1 = row4.CreateCell(2);
                            row4.CreateCell(3).CellStyle = style2;
                            row4.CreateCell(4).CellStyle = style2;
                            cel4_1.SetCellValue("销售顾问：" + dic["SalesConsultant"]);
                            cel4_1.CellStyle = style2;
                            CellRangeAddress region4_1 = new CellRangeAddress(4, 4, 2, 4);
                            sheet1.AddMergedRegion(region4_1);
                            
                            if(visitTypeName == "D")
                            {
                                ICell cel4_2 = row4.CreateCell(5);
                                row4.CreateCell(6).CellStyle = style2;
                                cel4_2.SetCellValue("销售内勤：" + dic["SalsInside"]);
                                cel4_2.CellStyle = style2;
                                CellRangeAddress region4_2 = new CellRangeAddress(4, 4, 5, 6);
                                sheet1.AddMergedRegion(region4_2);
                            }                            
                        }                        

                        var style_sub = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_sub.Alignment = HorizontalAlignment.Center;
                        style_sub.SetFillForegroundColor(new XSSFColor(rgb));
                        //style_sub.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                        style_sub.FillPattern = FillPattern.SolidForeground;
                        style_sub.SetFont(font_b);
                        var style_5 = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_5.Alignment = HorizontalAlignment.Left;
                        style_5.SetFillForegroundColor(new XSSFColor(rgb5));
                        style_5.FillPattern = FillPattern.SolidForeground;
                        style_5.BorderLeft = BorderStyle.Thin;
                        style_5.BorderBottom = BorderStyle.Thin;
                        style_5.BorderRight = BorderStyle.Thin;
                        style_5.BorderTop = BorderStyle.Thin;
                        style_5.SetFont(font_b);
                        var style_6 = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_6.Alignment = HorizontalAlignment.Center;
                        style_6.SetFillForegroundColor(new XSSFColor(rgb6));
                        style_6.FillPattern = FillPattern.SolidForeground;
                        style_6.BorderLeft = BorderStyle.Thin;
                        style_6.BorderBottom = BorderStyle.Thin;
                        style_6.BorderRight = BorderStyle.Thin;
                        style_6.BorderTop = BorderStyle.Thin;
                        style_6.SetFont(font_b);
                        var style_7 = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_7.Alignment = HorizontalAlignment.Center;
                        style_7.SetFillForegroundColor(new XSSFColor(rgb7));
                        style_7.FillPattern = FillPattern.SolidForeground;
                        style_7.BorderLeft = BorderStyle.Thin;
                        style_7.BorderBottom = BorderStyle.Thin;
                        style_7.BorderRight = BorderStyle.Thin;
                        style_7.BorderTop = BorderStyle.Thin;
                        style_7.SetFont(font_c);
                        sheet1.CreateRow(5);
                        IRow row_score = sheet1.CreateRow(6);
                        ICell cell_score = row_score.CreateCell(0);
                        cell_score.SetCellValue("检核结果汇总");
                        CellRangeAddress region_score = new CellRangeAddress(6, 6, 0, 4);
                        cell_score.CellStyle = style_sub;
                        sheet1.AddMergedRegion(region_score);

                        int tCount = 7;
                        int sCount = tCount;

                        IRow rows = sheet1.CreateRow(sCount);

                        //任务卡名称
                        ICell cell_s1 = rows.CreateCell(0);
                        cell_s1.SetCellValue("模块");
                        cell_s1.CellStyle = style1;
                        ICell cell_s2 = rows.CreateCell(1);
                        cell_s2.SetCellValue("");
                        cell_s2.CellStyle = style1;
                        CellRangeAddress region_s1 = new CellRangeAddress(sCount, sCount, 0, 1);
                        sheet1.AddMergedRegion(region_s1);

                        //指标数量
                        ICell cell_s3 = rows.CreateCell(2);
                        cell_s3.SetCellValue("指标数量");
                        cell_s3.CellStyle = style1;
                        CellRangeAddress region_s2 = new CellRangeAddress(sCount, sCount, 2, 2);
                        sheet1.AddMergedRegion(region_s2);

                        //权重
                        ICell cell_s4 = rows.CreateCell(3);
                        cell_s4.SetCellValue("权重");
                        cell_s4.CellStyle = style1;
                        CellRangeAddress region_s3 = new CellRangeAddress(sCount, sCount, 3, 3);
                        sheet1.AddMergedRegion(region_s3);

                        //得分
                        ICell cell_s5 = rows.CreateCell(4);
                        cell_s5.SetCellValue("得分");
                        cell_s5.CellStyle = style1;
                        CellRangeAddress region_s4 = new CellRangeAddress(sCount, sCount, 4, 4);
                        sheet1.AddMergedRegion(region_s4);

                        sCount++;
                        if (ts != null && ts.Count > 0)
                        {
                            for (int i = 0; i < ts.Count; i++)
                            {
                                IRow row_un = sheet1.CreateRow(sCount + i);
                                ICell cell_0 = row_un.CreateCell(0);
                                cell_0.SetCellValue(ts[i].TCTitle);
                                cell_0.CellStyle = style_5;
                                ICell cell_1 = row_un.CreateCell(1);
                                cell_1.SetCellValue(ts[i].TCTitle);
                                cell_1.CellStyle = style_5;
                                CellRangeAddress r_s1 = new CellRangeAddress(sCount + i, sCount + i, 0, 1);
                                sheet1.AddMergedRegion(r_s1);
                                ICell cell_4 = row_un.CreateCell(2);
                                cell_4.SetCellValue(ts[i].ItemQty);
                                cell_4.CellStyle = style_6;
                                CellRangeAddress r_s2 = new CellRangeAddress(sCount + i, sCount + i, 2,2);
                                sheet1.AddMergedRegion(r_s2);
                                ICell cell_5 = row_un.CreateCell(3);
                                decimal rWeight = 0;
                                decimal.TryParse(ts[i].TCWeight, out rWeight);
                                cell_5.SetCellValue(Math.Round(rWeight, 2) * 100 + "%");
                                cell_5.CellStyle = style_6;
                                CellRangeAddress r_s3 = new CellRangeAddress(sCount + i, sCount + i, 3, 3);
                                sheet1.AddMergedRegion(r_s3);
                                ICell cell_6 = row_un.CreateCell(4);
                                cell_6.SetCellValue(ts[i].ItemScore);
                                cell_6.CellStyle = style_6;
                                CellRangeAddress r_s4 = new CellRangeAddress(sCount + i, sCount + i, 4, 4);
                                sheet1.AddMergedRegion(r_s4);
                            }
                        }

                        if (ts != null && ts.Count >= 0)
                        {
                            sCount = ts.Count + tCount + 1;
                        }
                        IRow rowst = sheet1.CreateRow(sCount);

                        //任务卡名称
                        ICell cell_st1 = rowst.CreateCell(0);
                        cell_st1.SetCellValue("总分");
                        cell_st1.CellStyle = style_7;
                        ICell cell_st2 = rowst.CreateCell(1);
                        cell_st2.SetCellValue("");
                        cell_st2.CellStyle = style_7;
                        ICell cell_st3 = rowst.CreateCell(2);
                        cell_st3.SetCellValue("");
                        cell_st3.CellStyle = style_7;
                        ICell cell_st4 = rowst.CreateCell(3);
                        cell_st4.SetCellValue("");
                        cell_st4.CellStyle = style_7;
                        CellRangeAddress region_st1 = new CellRangeAddress(sCount, sCount, 0, 3);
                        sheet1.AddMergedRegion(region_st1);

                        //指标数量
                        ICell cell_st5 = rowst.CreateCell(4);
                        cell_st5.SetCellValue(totalScore);
                        cell_st5.CellStyle = style_7;
                        CellRangeAddress region_st2 = new CellRangeAddress(sCount, sCount, 4,4);
                        sheet1.AddMergedRegion(region_st2);

                        /***
                        IRow row_sub = sheet1.CreateRow(5);
                        ICell cell_sub = row_sub.CreateCell(0);
                        cell_sub.SetCellValue("检核结果汇总");
                        CellRangeAddress region_sub = new CellRangeAddress(5, 5, 0, 6);
                        cell_sub.CellStyle = style_sub;
                        sheet1.AddMergedRegion(region_sub);

                        IRow row_6 = sheet1.CreateRow(6);
                        ICell cell_6 = row_6.CreateCell(0);
                        cell_6.SetCellValue("未达标汇总:");
                        cell_6.CellStyle = style2;
                        CellRangeAddress region_6 = new CellRangeAddress(6, 6, 0, 0);
                        sheet1.AddMergedRegion(region_6);

                        if (tiId != null && tiId.Count > 0)
                        {
                            for (int i = 0; i < tiId.Count; i++)
                            {
                                IRow row_un = sheet1.CreateRow(7 + i);
                                ICell cell_un = row_un.CreateCell(0);
                                cell_un.SetCellValue((i + 1) + "、" + tiId[i]);
                                cell_un.CellStyle = style2;
                            }
                        }
                        *****/

                        int pw = 1;
                        int tcw = 1;
                        int tiw = 1;
                        int csw = 1;
                        int rw = 1;
                        if (list != null && list.Count > 0)
                        {
                            for (int k = 0; k < list.Count; k++)
                            {
                                if ((list[k].PTitle == null ? 0 : list[k].PTitle.Length) > pw)
                                {
                                    pw = list[k].PTitle.Length;
                                }
                                if ((list[k].TCTitle == null ? 0 : list[k].TCTitle.Length) > tcw)
                                {
                                    tcw = list[k].TCTitle.Length;
                                }
                                if ((list[k].TITitle == null ? 0 : list[k].TITitle.Length) > tiw)
                                {
                                    tiw = list[k].TITitle.Length;
                                }
                                if ((list[k].CSTitle == null ? 0 : list[k].CSTitle.Length) > csw)
                                {
                                    csw = list[k].CSTitle.Length;
                                }
                                if ((list[k].Remarks == null ? 0 : list[k].Remarks.Length) > rw)
                                {
                                    rw = list[k].Remarks.Length;
                                }
                            }
                        }
                        //int dataCount = 10;
                        //if (tiId != null && tiId.Count >= 0)
                        //{
                        //    dataCount = tiId.Count + 10;
                        //}
                        int dataCount = sCount + 2;

                        IRow row3 = sheet1.CreateRow(dataCount);

                        //任务卡名称
                        ICell cell6 = row3.CreateCell(0);
                        cell6.SetCellValue("模块");
                        cell6.CellStyle = style1;
                        CellRangeAddress region6 = new CellRangeAddress(dataCount, dataCount, 0, 0);
                        sheet1.AddMergedRegion(region6);
                        sheet1.SetColumnWidth(0, 7 * 600);
                        //体系名称
                        ICell cell7 = row3.CreateCell(1);
                        cell7.SetCellValue("审计项目");
                        cell7.CellStyle = style1;
                        CellRangeAddress region7 = new CellRangeAddress(dataCount, dataCount, 1, 1);
                        sheet1.AddMergedRegion(region7);
                        sheet1.SetColumnWidth(1, 7 * 600);
                        //体系权重
                        ICell cell8 = row3.CreateCell(2);
                        cell8.SetCellValue("权重");
                        cell8.CellStyle = style1;
                        CellRangeAddress region8 = new CellRangeAddress(dataCount, dataCount, 2, 2);
                        sheet1.AddMergedRegion(region8);
                        sheet1.SetColumnWidth(2, 13 * 300);
                        //检查标准
                        ICell cell9 = row3.CreateCell(3);
                        cell9.SetCellValue("检查标准");
                        cell9.CellStyle = style1;
                        //ICell cell10 = row3.CreateCell(4);
                        //cell10.SetCellValue("");
                        //cell10.CellStyle = style1;
                        CellRangeAddress region9 = new CellRangeAddress(dataCount, dataCount, 3, 3);
                        sheet1.AddMergedRegion(region9);
                        sheet1.SetColumnWidth(3, (13 * 600));
                        //sheet1.SetColumnWidth(4, (15 * 300));
                        //sheet1.SetColumnWidth(4, (15 * 300));
                        //结果
                        ICell cell_10 = row3.CreateCell(4);
                        cell_10.SetCellValue("是否达成");
                        cell_10.CellStyle = style1;
                        CellRangeAddress region10 = new CellRangeAddress(dataCount, dataCount, 4, 4);
                        sheet1.AddMergedRegion(region10);
                        sheet1.SetColumnWidth(4, 13 * 300);

                        //得分
                        ICell cell11 = row3.CreateCell(5);
                        cell11.SetCellValue("得分");
                        cell11.CellStyle = style1;
                        CellRangeAddress region11 = new CellRangeAddress(dataCount, dataCount, 5, 5);
                        sheet1.AddMergedRegion(region11);
                        sheet1.SetColumnWidth(5, 9*300);

                        //备注
                        ICell cell12 = row3.CreateCell(6);
                        cell12.SetCellValue("备注");
                        cell12.CellStyle = style1;
                        CellRangeAddress region12 = new CellRangeAddress(dataCount, dataCount, 6, 6);
                        sheet1.AddMergedRegion(region12);
                        sheet1.SetColumnWidth(6, 15 * 300);

                        int firstRow1 = dataCount + 1;
                        int lastRow1 = 0;
                        int sumCnt1 = 0;
                        int firstRow2 = dataCount + 1;
                        int lastRow2 = 0;
                        int sumCnt2 = 0;
                        int firstRow3 = dataCount + 1;
                        int lastRow3 = 0;
                        int sumCnt3 = 0;
                        int firstRow4 = dataCount + 1;
                        int lastRow4 = 0;
                        int sumCnt4 = 0;
                        int firstRow5 = dataCount + 1;
                        int lastRow5 = 0;
                        int sumCnt5 = 0;
                        //数据的动态加载
                        for (int i = 0; i < list.Count; i++)
                        {

                            object r = "r_" + i;
                            r = sheet1.CreateRow(dataCount + 1 + i);
                            int cnum = 0;
                            //计划任务名称

                            //任务卡名称
                            object c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(list[i].TCTitle);
                            (c as ICell).CellStyle = style_b2;
                            if (i == sumCnt1)
                            {
                                int cnt = (from l1 in list where l1.TCId == list[i].TCId select l1).Count();
                                sumCnt1 += cnt;
                                lastRow1 = firstRow1 + cnt - 1;
                                if (list[i].CSId != null)
                                {
                                    sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 0, 0));
                                }
                                firstRow1 = lastRow1 + 1;
                            }
                            cnum++;


                            //体系名称
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(list[i].TITitle);
                            (c as ICell).CellStyle = style_b2;
                            //sheet1.AddMergedRegion(new CellRangeAddress(5 + i, 5 + i, 7 + cnum, 7 + cnum));
                            if (i == sumCnt2)
                            {
                                int cnt = (from l1 in list where l1.TCId == list[i].TCId && l1.TIId == list[i].TIId select l1).Count();
                                sumCnt2 += cnt;
                                lastRow2 = firstRow2 + cnt - 1;
                                if (list[i].CSId != null)
                                {
                                    sheet1.AddMergedRegion(new CellRangeAddress(firstRow2, lastRow2, 1, 1));
                                }
                                firstRow2 = lastRow2 + 1;
                            }
                            cnum++;

                            //体系权重
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(list[i].TIWeight);
                            (c as ICell).CellStyle = style4;
                            //sheet1.AddMergedRegion(new CellRangeAddress(5 + i, 5 + i, 7 + cnum, 7 + cnum));
                            if (i == sumCnt5)
                            {
                                int cnt = (from l1 in list where l1.TCId == list[i].TCId && l1.TIId == list[i].TIId select l1).Count();
                                sumCnt5 += cnt;
                                lastRow5 = firstRow5 + cnt - 1;
                                if (list[i].CSId != null)
                                {
                                    sheet1.AddMergedRegion(new CellRangeAddress(firstRow5, lastRow5, 2, 2));
                                }
                                firstRow5 = lastRow5 + 1;
                            }
                            cnum++;

                            //检查标准名称
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(list[i].CSTitle);
                            (c as ICell).CellStyle = style_Stand;
                            cnum++;

                            ////检查标准名称
                            //c = "c_" + cnum;
                            //c = (r as IRow).CreateCell(cnum);
                            //(c as ICell).SetCellValue("");
                            //(c as ICell).CellStyle = style_Stand;
                            //cnum++;
                            
                            //sheet1.AddMergedRegion(new CellRangeAddress(dataCount + 1 + i, dataCount + 1 + i, 2, 3));

                            //结果
                            string val = "";
                            if (list[i].Result != null)
                            {
                                val = list[i].Result.ToUpper() == "TRUE" ? "是" : (list[i].Result == null ? "" : "否");
                            }
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(val);
                            (c as ICell).CellStyle = style4;
                            sheet1.AddMergedRegion(new CellRangeAddress(dataCount + 1 + i, dataCount + 1 + i, 4, 4));
                            cnum++;

                            //得分
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(list[i].Score);
                            (c as ICell).CellStyle = style4;
                            if (i == sumCnt4)
                            {
                                int cnt = (from l1 in list where l1.TCId == list[i].TCId && l1.TIId == list[i].TIId select l1).Count();
                                sumCnt4 += cnt;
                                lastRow4 = firstRow4 + cnt - 1;
                                if (list[i].CSId != null)
                                {
                                    sheet1.AddMergedRegion(new CellRangeAddress(firstRow4, lastRow4, 5, 5));
                                }
                                firstRow4 = lastRow4 + 1;
                            }
                            cnum++;

                            //备注
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(list[i].Remarks);
                            (c as ICell).CellStyle = style_b2;
                            if (i == sumCnt3)
                            {
                                int cnt = (from l1 in list where l1.TCId == list[i].TCId && l1.TIId == list[i].TIId select l1).Count();
                                sumCnt3 += cnt;
                                lastRow3 = firstRow3 + cnt - 1;
                                if (list[i].CSId != null)
                                {
                                    sheet1.AddMergedRegion(new CellRangeAddress(firstRow3, lastRow3, 6, 6));
                                }
                                firstRow3 = lastRow3 + 1;
                            }
                            cnum++;

                        }
                        sheet1.CreateRow(dataCount + 1 + list.Count);
                        IRow row_sub2 = sheet1.CreateRow(dataCount + 2 + list.Count);
                        ICell cell_sub2 = row_sub2.CreateCell(0);
                        cell_sub2.SetCellValue("巡检照片");
                        CellRangeAddress region_sub2 = new CellRangeAddress(dataCount + 2 + list.Count, dataCount + 2 + list.Count, 0, 6);
                        cell_sub2.CellStyle = style_sub;
                        sheet1.AddMergedRegion(region_sub2);

                        for (int i = 0; i < lpList.Count; i++)
                        {
                            XSSFClientAnchor anchor;
                            IRow row_pic = sheet1.CreateRow(dataCount + 2 + list.Count + i + 1);
                            if (i % 2 == 1)
                            {
                                row_pic.Height = 4000;
                                anchor = new XSSFClientAnchor(50, 50, 50, 50, 4, dataCount + 2 + list.Count + i + 1, 7, dataCount + 2 + list.Count + i + 2);
                            }
                            else
                            {
                                ICell cell_pic = row_pic.CreateCell(0);
                                cell_pic.CellStyle = style_b3;
                                ICell cell_pic2 = row_pic.CreateCell(4);
                                cell_pic2.CellStyle = style_b3;

                                CellRangeAddress region_tp_1 = new CellRangeAddress(dataCount + 2 + list.Count + i + 1, dataCount + 2 + list.Count + i + 1, 0, 2);
                                sheet1.AddMergedRegion(region_tp_1);
                                CellRangeAddress region_tp_2 = new CellRangeAddress(dataCount + 2 + list.Count + i + 1, dataCount + 2 + list.Count + i + 1, 4, 6);
                                sheet1.AddMergedRegion(region_tp_2);

                                if (lpList[i].PicName != null)
                                {
                                    cell_pic.SetCellValue((i + 1) + "、第" + (i + 1) + "个拍照点的名称" + lpList[i].PicName);
                                }
                                if (lpList[i + 1].PicName != null)
                                {
                                    cell_pic2.SetCellValue((i + 2) + "、第" + (i + 2) + "个拍照点的名称" + lpList[i + 1].PicName);
                                }
                                anchor = new XSSFClientAnchor(50, 50, 50, 50, 0, dataCount + 2 + list.Count + i + 2, 3, dataCount + 2 + list.Count + i + 3);
                            }
                            if (lpList[i].PicUrl != null)
                            {
                                string imagesPath = lpList[i].PicUrl;
                                HttpClient webClient = new HttpClient();
                                Stream stream = await webClient.GetStreamAsync(imagesPath);
                                Image img = Image.FromStream(stream).GetThumbnailImage(500, 500, null, IntPtr.Zero);
                                MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                var patriarch = sheet1.CreateDrawingPatriarch();
                                anchor.AnchorType = AnchorType.MoveAndResize;
                                int index = workbook.AddPicture(ms.ToArray(), PictureType.PNG);
                                var signaturePicture = patriarch.CreatePicture(anchor, index);
                            }

                        }
                        workbook.Write(fs);
                        return Json(newFile);
                    }
                }
                else
                {
                    return Json("没有查询结果.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [PermissionRequired]
        public IActionResult REP020()
        {
            return View();
        }
    }
}
