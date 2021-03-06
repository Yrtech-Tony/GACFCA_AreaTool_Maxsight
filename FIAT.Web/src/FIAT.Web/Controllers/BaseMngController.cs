﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using FIAT.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.Web.Controllers
{

    public class BaseMngController : Controller
    {
        private IHostingEnvironment _environment;
        public BaseMngController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        // GET: /<controller>/
        //[Authorize]
        [PermissionRequired]
        public IActionResult BAS010()
        {
            return View();
        }
        public IActionResult BAS010P()
        {
            return View();
        }
        //[Authorize]
        [PermissionRequired]
        public IActionResult BAS020()
        {
            return View();
        }
        //[Authorize]
        [PermissionRequired]
        public IActionResult BAS030()
        {
            return View();
        }
        [PermissionRequired]
        public IActionResult BAS040()
        {
            return View();
        }
        [PermissionRequired]
        public IActionResult BAS050()
        {
            return View();
        }
        [PermissionRequired]
        public IActionResult BAS060()
        {
            return View();
        }
        [PermissionRequired]
        public IActionResult BAS070()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UploadPostTarget()
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
                    paramList.Add(new KeyValuePair<string, string>("SheetNameList", JsonConvert.SerializeObject(new List<string> { "PostTargetSetting" })));
                    HttpResponseMessage res = await CommonHelper.GetHttpClient1().PostAsync(CommonHelper.Current.GetExcelAPIBaseUrl, new FormUrlEncodedContent(paramList));
                    result = res.Content.ReadAsStringAsync().Result;
                }
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> UploadDealers()
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

                    //string url = CommonHelper.Current.GetExcelAPIBaseUrl + "excel?filepath=" + @"C:\inetpub\FIAT.Web\wwwroot\uploads\Template.xlsx&sheetName=Plans";
                    // string url = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetExcelAPIBaseUrl + "excel?filepath=" + @"C:\inetpub\FIAT.Web\wwwroot\uploads\Template.xlsx&sheetName=Plans");
                    List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                    //paramList.Add(new KeyValuePair<string, string>("FullFileName", @"C:\\Projects\\PangPang\\dev.fiat.web\\wwwroot\\uploads\\" + file.FileName));
                    paramList.Add(new KeyValuePair<string, string>("FullFileName", Path.Combine(uploads, file.FileName)));
                    paramList.Add(new KeyValuePair<string, string>("SheetNameList", JsonConvert.SerializeObject(new List<string> { "Dealers" })));
                    HttpResponseMessage res = await CommonHelper.GetHttpClient1().PostAsync(CommonHelper.Current.GetExcelAPIBaseUrl, new FormUrlEncodedContent(paramList));
                    result = res.Content.ReadAsStringAsync().Result;
                    //Url = Path.Combine(uploads, file.FileName);
                }
                //List<Info> list = new List<Info>();
                //list= ExcelHelpe 
                //string message = $"{files.Count} file(s) / { size}bytes uploaded successfully!";
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
