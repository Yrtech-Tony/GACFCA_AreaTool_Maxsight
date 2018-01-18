using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using FIAT.Web.Common;
using Newtonsoft.Json;
using FIAT.Web.Common.Module;
using Npoi.Core.XSSF.UserModel;
using Npoi.Core.SS.UserModel;
using Npoi.Core.SS.Util;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.Web.Controllers
{
    public class PerMngController : Controller
    {
        private IHostingEnvironment _environment;
        public PerMngController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PMN010()
        {
            DateTime sDate = new DateTime(2017, 4, 1);
            DateTime now = DateTime.Now;
            int diff = (now.Year - sDate.Year) * 12 + (now.Month - sDate.Month) + 1;
            ViewBag.CurrentYear = now.Year;
            ViewBag.CurentMonth = now.Month;
            ViewBag.Diff = diff;
            return View();
        }
        public IActionResult PMN011()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UploadStaffAjax()
        {
            try
            {
                long size = 0;
                var files = Request.Form.Files;
                var batch = Request.Form["Batch"];
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                string result = "";
                foreach (var file in files)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    filename = $@"\{filename}";
                    size += file.Length;
                    using (var fs = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    //string url = CommonHelper.Current.GetExcelAPIBaseUrl + "excel?filepath=" + @"C:\inetpub\FIAT.Web\wwwroot\uploads\Template.xlsx&sheetName=Plans";Path.Combine(uploads, file.FileName)
                    // string url = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetExcelAPIBaseUrl + "excel?filepath=" + @"C:\inetpub\FIAT.Web\wwwroot\uploads\Template.xlsx&sheetName=Plans");
                    List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                    paramList.Add(new KeyValuePair<string, string>("FullFileName", Path.Combine(uploads, file.FileName)));
                    paramList.Add(new KeyValuePair<string, string>("SheetNameList", JsonConvert.SerializeObject(new List<string> { "Staff" })));
                    HttpResponseMessage res = await CommonHelper.GetHttpClient1().PostAsync(CommonHelper.Current.GetExcelAPIBaseUrl, new FormUrlEncodedContent(paramList));
                    result = res.Content.ReadAsStringAsync().Result;
                }


                return Json(result);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> StaffAjaxDown(string yearMonth, string bName, string areaName, string zoneName, string batchName, string zoneId, int busType)
        {
            try
            {
                //string result = await CommonHelper.GetHttpClient().GetStringAsync("http://localhost:6502/fiat/api/v1/" +"PerMng/GetStaffZoneReport?yearMonth=" + yearMonth + "&zoneId=" + zoneId + "&busType=" + busType);
                string result = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetAPIBaseUrl + "PerMng/GetStaffZoneReport?yearMonth=" + yearMonth + "&zoneId=" + zoneId + "&busType=" + busType);
                var apiResult = CommonHelper.DecodeString<APIResult>(result);
                if (apiResult.ResultCode == ResultType.Success)
                {
                    StaffZoneReport er = CommonHelper.DecodeString<StaffZoneReport>(apiResult.Body);
                    List<StaffZoneReportDto> list = er.ZoneReportList;
                    List<StaffInfo> staffList = er.StaffPostInfoList;
                    int totalStaffCnt = (from a in list select a.PostCnt).Sum();
                    int totalquietCnt = (from a in list select a.QuitCnt).Sum();

                    var uploads = Path.Combine(_environment.WebRootPath, "Template");
                    var newFile = Path.Combine(uploads, "人员小区报告_" + zoneName + "_" + DateTime.Now.ToString("yyyyMMdd hhmmss") + ".xlsx");
                    if (!System.IO.Directory.Exists(uploads))
                    {
                        System.IO.Directory.CreateDirectory(uploads);
                    }
                    using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
                    {

                        IWorkbook workbook = new XSSFWorkbook();

                        ISheet sheet1 = workbook.CreateSheet("人员小区报告");

                        IFont font_b = workbook.CreateFont();
                        font_b.FontName = "微软雅黑";
                        font_b.FontHeightInPoints = 10;

                        var style1 = (XSSFCellStyle)workbook.CreateCellStyle();

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

                        var style_b2 = workbook.CreateCellStyle();
                        style_b2.WrapText = true;
                        style_b2.VerticalAlignment = VerticalAlignment.Center;
                        style_b2.SetFont(font_b);


                        var style2 = workbook.CreateCellStyle();
                        style2.Alignment = HorizontalAlignment.Left;
                        style2.SetFont(font_b);

                        var style3 = workbook.CreateCellStyle();
                        style3.Alignment = HorizontalAlignment.Right;
                        style3.SetFont(font_b);

                        var style4 = workbook.CreateCellStyle();
                        style4.Alignment = HorizontalAlignment.Center;
                        style4.VerticalAlignment = VerticalAlignment.Center;
                        style4.SetFont(font_b);

                        var style_t = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_t.Alignment = HorizontalAlignment.Center;
                        style_t.SetFillForegroundColor(new XSSFColor(rgb));
                        style_t.FillPattern = FillPattern.SolidForeground;

                        IFont font_t = workbook.CreateFont();
                        font_t.FontHeightInPoints = 18;
                        font_t.FontName = "微软雅黑";
                        style_t.SetFont(font_t);

                        //标题
                        IRow row_t = sheet1.CreateRow(0);
                        ICell cell_t = row_t.CreateCell(0);
                        cell_t.SetCellValue("人员小区报告");
                        CellRangeAddress region_t = new CellRangeAddress(0, 0, 0, 15);
                        cell_t.CellStyle = style_t;
                        sheet1.AddMergedRegion(region_t);

                        IRow row = sheet1.CreateRow(1);
                        ICell cell = row.CreateCell(0);
                        cell.SetCellValue("大区:" + bName + "                区域：" + areaName + "                 小区:" + zoneName + "                  巡检月份:" + batchName);
                        CellRangeAddress region = new CellRangeAddress(1, 1, 0, 15);
                        cell.CellStyle = style2;
                        sheet1.AddMergedRegion(region);


                        var style_sub = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_sub.Alignment = HorizontalAlignment.Center;
                        style_sub.SetFillForegroundColor(new XSSFColor(rgb));
                        style_sub.FillPattern = FillPattern.SolidForeground;
                        style_sub.SetFont(font_b);

                        IRow row_sub = sheet1.CreateRow(2);

                        IRow row_3 = sheet1.CreateRow(3);
                        ICell cell_3_0 = row_3.CreateCell(0);
                        cell_3_0.SetCellValue("备案概况");
                        cell_3_0.CellStyle = style1;

                        ICell cell_3_1 = row_3.CreateCell(1);
                        cell_3_1.SetCellValue("数量");
                        cell_3_1.CellStyle = style1;

                        ICell cell_3_2 = row_3.CreateCell(2);
                        cell_3_2.SetCellValue("DMS备案率");
                        cell_3_2.CellStyle = style1;

                        ICell cell_3_3 = row_3.CreateCell(3);
                        cell_3_3.SetCellValue("认证率");
                        cell_3_3.CellStyle = style1;

                        ICell cell_3_4 = row_3.CreateCell(4);
                        cell_3_4.SetCellValue("离职数量");
                        cell_3_4.CellStyle = style1;

                        //合计行
                        IRow row_4 = sheet1.CreateRow(4);
                        ICell cell_4_0 = row_4.CreateCell(0);
                        cell_4_0.SetCellValue("合计");
                        cell_4_0.CellStyle = style4;

                        ICell cell_4_1 = row_4.CreateCell(1);
                        cell_4_1.SetCellValue(totalStaffCnt);
                        cell_4_1.CellStyle = style3;

                        ICell cell_4_2 = row_4.CreateCell(2);
                        cell_4_2.SetCellValue("");
                        cell_4_2.CellStyle = style3;

                        ICell cell_4_3 = row_4.CreateCell(3);
                        cell_4_3.SetCellValue("");
                        cell_4_3.CellStyle = style3;

                        ICell cell_4_4 = row_4.CreateCell(4);
                        cell_4_4.SetCellValue(totalquietCnt);
                        cell_4_4.CellStyle = style3;


                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                IRow row_un = sheet1.CreateRow(5 + i);
                                ICell cell_un_0 = row_un.CreateCell(0);
                                cell_un_0.SetCellValue(list[i].PostCode);
                                cell_un_0.CellStyle = style2;

                                ICell cell_un_1 = row_un.CreateCell(1);
                                cell_un_1.SetCellValue(list[i].PostCnt);
                                cell_un_1.CellStyle = style3;

                                ICell cell_un_2 = row_un.CreateCell(2);
                                cell_un_2.SetCellValue(list[i].DMSRate+"%");
                                cell_un_2.CellStyle = style3;

                                ICell cell_un_3 = row_un.CreateCell(3);
                                cell_un_3.SetCellValue(list[i].AuthenticateRate+"%");
                                cell_un_3.CellStyle = style3;

                                ICell cell_un_4 = row_un.CreateCell(4);
                                cell_un_4.SetCellValue(list[i].QuitCnt);
                                cell_un_4.CellStyle = style3;
                            }
                        }

                        int dataCount = 6;
                        if (list != null && list.Count >= 0)
                        {
                            dataCount = list.Count + dataCount;
                        }
                        IRow row_n = sheet1.CreateRow(dataCount);

                        //所属大区
                        ICell cell_n_0 = row_n.CreateCell(0);
                        cell_n_0.SetCellValue("所属大区");
                        cell_n_0.CellStyle = style1;
                        sheet1.SetColumnWidth(0, 6 * 600);
                        //所属区域
                        ICell cell_n_1 = row_n.CreateCell(1);
                        cell_n_1.SetCellValue("所属区域");
                        cell_n_1.CellStyle = style1;
                        sheet1.SetColumnWidth(1, 6 * 600);
                        //所属小区
                        ICell cell_n_2 = row_n.CreateCell(2);
                        cell_n_2.SetCellValue("所属小区");
                        cell_n_2.CellStyle = style1;
                        sheet1.SetColumnWidth(2, 6 * 600);
                        //经销商
                        ICell cell_n_3 = row_n.CreateCell(3);
                        cell_n_3.SetCellValue("经销商");
                        cell_n_3.CellStyle = style1;
                        sheet1.SetColumnWidth(3, 6 * 600);
                        //岗位
                        ICell cell_n_4 = row_n.CreateCell(4);
                        cell_n_4.SetCellValue("岗位");
                        cell_n_4.CellStyle = style1;
                        sheet1.SetColumnWidth(4, 6 * 600);
                        //姓名
                        ICell cell_n_5 = row_n.CreateCell(5);
                        cell_n_5.SetCellValue("姓名");
                        cell_n_5.CellStyle = style1;
                        sheet1.SetColumnWidth(5, 4 * 600);


                        //身份证号
                        ICell cell_n_6 = row_n.CreateCell(6);
                        cell_n_6.SetCellValue("身份证号");
                        cell_n_6.CellStyle = style1;
                        sheet1.SetColumnWidth(6, 10 * 600);

                        //状态
                        ICell cell_n_7 = row_n.CreateCell(7);
                        cell_n_7.SetCellValue("状态");
                        cell_n_7.CellStyle = style1;
                        sheet1.SetColumnWidth(7, 4 * 600);

                        //是否DMS备案
                        ICell cell_n_8 = row_n.CreateCell(8);
                        cell_n_8.SetCellValue("是否DMS备案");
                        cell_n_8.CellStyle = style1;
                        sheet1.SetColumnWidth(8, 5 * 600);


                        //是否通过认证
                        ICell cell_n_9 = row_n.CreateCell(9);
                        cell_n_9.SetCellValue("是否通过认证");
                        cell_n_9.CellStyle = style1;
                        sheet1.SetColumnWidth(9, 5 * 600);

                        //机电等级
                        ICell cell_n_10 = row_n.CreateCell(10);
                        cell_n_10.SetCellValue("机电等级");
                        cell_n_10.CellStyle = style1;
                        sheet1.SetColumnWidth(10, 5 * 600);

                        //技术主管等级
                        ICell cell_n_11 = row_n.CreateCell(11);
                        cell_n_11.SetCellValue("技术主管等级");
                        cell_n_11.CellStyle = style1;
                        sheet1.SetColumnWidth(11, 6 * 600);

                        //是否兼职
                        ICell cell_n_12 = row_n.CreateCell(12);
                        cell_n_12.SetCellValue("是否兼职");
                        cell_n_12.CellStyle = style1;
                        sheet1.SetColumnWidth(12, 4 * 600);

                        int cellcnt = 0;
                        for (int i = 1; i < 4; i++)
                        {
                            //兼职岗位1
                            ICell cell_n_13 = row_n.CreateCell(13 + cellcnt);
                            cell_n_13.SetCellValue("兼职岗位" + i);
                            cell_n_13.CellStyle = style1;
                            sheet1.SetColumnWidth(13 + cellcnt, 6 * 600);
                            cellcnt++;

                            //DMS备案
                            ICell cell_n_14 = row_n.CreateCell(13 + cellcnt);
                            cell_n_14.SetCellValue("是否DMS备案");
                            cell_n_14.CellStyle = style1;
                            sheet1.SetColumnWidth(13 + cellcnt, 5 * 600);
                            cellcnt++;

                            //是否通过认证
                            ICell cell_n_15 = row_n.CreateCell(13 + cellcnt);
                            cell_n_15.SetCellValue("是否通过认证");
                            cell_n_15.CellStyle = style1;
                            sheet1.SetColumnWidth(13 + cellcnt, 5 * 600);
                            cellcnt++;
                        }
                        //数据的动态加载
                        for (int i = 0; i < staffList.Count; i++)
                        {
                            object r = "r_" + i;
                            r = sheet1.CreateRow(dataCount + 1 + i);
                            int cnum = 0;

                            //所属大区
                            object c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].BName);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //所属区域
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].AreaName);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //所属小区
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].ZoneName);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //经销商
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].DisName);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //职位
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].PostCode);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //姓名
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].Name);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //身份证号码
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].IDNo);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //状态
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].Status);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //是否DMS备案
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].DMSYN);
                            (c as ICell).CellStyle = style4;
                            cnum++;


                            //是否通过认证
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].AuthenticateYN);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //机电等级
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].LevelM2);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //技术主管等级
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].LevelM1);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //是否兼职
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].PartTimeJobYN);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //兼职岗位1
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].PartTimeJob1);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //是否DMS备案
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].DMSYN1);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //是否通过认证
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].AuthenticateYN1);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //兼职岗位2
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].PartTimeJob2);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //是否DMS备案
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].DMSYN2);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //是否通过认证
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].AuthenticateYN2);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //兼职岗位3
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].PartTimeJob3);
                            (c as ICell).CellStyle = style_b2;
                            cnum++;

                            //是否DMS备案
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].DMSYN3);
                            (c as ICell).CellStyle = style4;
                            cnum++;

                            //是否通过认证
                            c = "c_" + cnum;
                            c = (r as IRow).CreateCell(cnum);
                            (c as ICell).SetCellValue(staffList[i].AuthenticateYN3);
                            (c as ICell).CellStyle = style4;
                            cnum++;


                            /*int firstRow1 = dataCount + 1;
                            int lastRow1 = 0;
                            int sumCnt1 = 0;
                            //数据的动态加载
                            for (int i = 0; i < staffList.Count; i++)
                            {
                                object r = "r_" + i;
                                r = sheet1.CreateRow(dataCount + 1 + i);
                                int cnum = 0;

                                //所属大区
                                object c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].BName);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //所属区域
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].AreaName);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //所属小区
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].ZoneName);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //经销商
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].DisName);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //职位
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].PostCode);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //等级
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue("等级");
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //姓名
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].Name);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //状态
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].Status);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //是否DMS备案
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].DMSYN);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //是否通过认证
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].AuthenticateYN);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //兼职与否
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].PartTimeJobYN);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //兼职岗位
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].PartTimeJob);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //是否DMS备案
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].DMSYN2);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                //是否通过认证
                                c = "c_" + cnum;
                                c = (r as IRow).CreateCell(cnum);
                                (c as ICell).SetCellValue(staffList[i].AuthenticateYN);
                                (c as ICell).CellStyle = style_b2;
                                cnum++;

                                if (i == sumCnt1)
                                {
                                    int cnt = (from l1 in staffList where l1.IDNo == staffList[i].IDNo select l1).Count();
                                    sumCnt1 += cnt;
                                    lastRow1 = firstRow1 + cnt - 1;
                                    if (staffList[i].IDNo != null)
                                    {
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 0, 0));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 1, 1));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 2, 2));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 3, 3));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 4, 4));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 5, 5));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 6, 6));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 7, 7));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 8, 8));
                                        sheet1.AddMergedRegion(new CellRangeAddress(firstRow1, lastRow1, 9, 9));
                                    }
                                    firstRow1 = lastRow1 + 1;
                                }*/
                            //cnum++;
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

        public IActionResult PMN012()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFilesAjax()
        {
            try
            {
                long size = 0;
                var files = Request.Form.Files;
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                string result = "";
                foreach (var file in files)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    filename = $@"\{filename}";
                    size += file.Length;
                    using (var fs = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                    paramList.Add(new KeyValuePair<string, string>("FullFileName", Path.Combine(uploads, file.FileName)));
                    paramList.Add(new KeyValuePair<string, string>("SheetNameList", JsonConvert.SerializeObject(new List<string> { "Dealers"})));
                    HttpResponseMessage res = await CommonHelper.GetHttpClient1().PostAsync(CommonHelper.Current.GetExcelAPIBaseUrl, new FormUrlEncodedContent(paramList));
                    result = res.Content.ReadAsStringAsync().Result;
                }
                return Json(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IActionResult PMN012P()
        {
            DateTime sDate = new DateTime(2017, 4, 1);
            DateTime now = DateTime.Now;
            int diff = (now.Year - sDate.Year) * 12 + (now.Month - sDate.Month) + 1;
            ViewBag.CurrentYear = now.Year;
            ViewBag.CurentMonth = now.Month;
            ViewBag.Diff = diff;
            return View();
        }


    }
}
