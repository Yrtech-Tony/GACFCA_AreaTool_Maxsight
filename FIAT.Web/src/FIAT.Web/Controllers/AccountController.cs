﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FIAT.Web.Common;
using FIAT.Web.Common.Module;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Collections.Generic;
using FIAT.Web.Service;
using log4net;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAT.Web.Controllers
{
    public class AccountController : Controller
    {
        //log4Net
        private ILog log;
        public AccountController(IHostingEnvironment hostingEnv)
        {
            //log4Net
            this.log = LogManager.GetLogger(Startup.repository.Name, typeof(AccountController));
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            //if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.SESSION_USERID)))
            //if (!string.IsNullOrWhiteSpace(HttpContext.Request.Cookies[SessionKeys.SESSION_USERID]))
            //{
            //    return Redirect("/Home/Index");
            //}
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string inputUserID, string inputPassword)
        {
            try
            {
               // log.Info("登录开始....");
                HttpContext.Session.Clear();
                if (string.IsNullOrWhiteSpace(inputUserID))
                {
                    ViewBag.ErrorLoginMessage = "请输入用户名。";
                }
                else if (string.IsNullOrWhiteSpace(inputPassword))
                {
                    ViewBag.ErrorLoginMessage = "请输入密码。";
                }
                else
                {
                    //log.Info("查询用户....");
                    //log.Info("数据库连接****" + Context.DapperContext.Current.SqlConnection);
                    UserInfo userinfo = await SetUserInfo(inputUserID, inputPassword);
                    
                    if (userinfo != null && userinfo.UserId != "0")
                    {
                        //log.Info("查询用户成功...." + userinfo.RoleList[0].Name);
                        ClaimsIdentity ci = new ClaimsIdentity("FIATCookieMiddlewareInstance");
                        ci.AddClaim(new Claim(ClaimTypes.Name, userinfo.UserId));
                        //ci.AddClaim(new Claim(ClaimTypes.UserData, Newtonsoft.Json.JsonConvert.SerializeObject(userinfo.RoleList)));

                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_USERNAME, userinfo.UserName);
                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_PHONE, userinfo.Phone);
                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_USERTYPENAME, userinfo.UserTypeName);
                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_DISNAME, userinfo.DisName);
                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_EMAIL, userinfo.Email);
                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_LOGGEDINAT, DateTime.Now.ToString("yyyy-MM-dd"));
                        string roleListStr = Newtonsoft.Json.JsonConvert.SerializeObject(userinfo.RoleList);
                        if (roleListStr.Length > 3001)
                        {
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST1", roleListStr.Substring(0, 1500));
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST2", roleListStr.Substring(1500, 1500));
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST3", roleListStr.Substring(3000, roleListStr.Length - 3000));
                        }
                        else if (roleListStr.Length > 1501)
                        {
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST1", roleListStr.Substring(0, 1500));
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST2", roleListStr.Substring(1500, roleListStr.Length - 1500));
                        }
                        else
                        {
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST1", roleListStr);
                            HttpContext.Response.Cookies.Append("fiat_ROLELIST2", "");
                        }
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_USERID, userinfo.UserId);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_NAME, userinfo.Name);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_USERTYPE, userinfo.UserType);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGBIZID, userinfo.OrgBizId);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGBIZTYPE, userinfo.OrgBizType);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGREPID, userinfo.OrgRepId);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGZIONID, userinfo.OrgZionId);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGAREAID, userinfo.OrgAreaId);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGSERVERID, userinfo.OrgServerId);
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ORGDEPARTMENTID, userinfo.OrgDepartmentId);
                        //HttpContext.Response.Cookies.Append(SessionKeys.SESSION_ZIONLIST, Newtonsoft.Json.JsonConvert.SerializeObject(userinfo.ZionList));
                        HttpContext.Response.Cookies.Append(SessionKeys.SESSION_DEPARTMENTLIST, Newtonsoft.Json.JsonConvert.SerializeObject(userinfo.DepartmentList));
                        HttpContext.Response.Cookies.Append("fiat_ACCESS_TOKEN", CommonHelper.GenerateToken());

                        await HttpContext.Authentication.SignInAsync("FIATCookieMiddlewareInstance", new ClaimsPrincipal(ci));
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        ViewBag.ErrorLoginMessage = "用户名或者密码不正确。";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorLoginMessage = "未查询到用户信息,请重试。";
                //log.Error("登录失败！！",ex);
                Login();
            }
            return View();
        }

        public ActionResult Logout()
        {
            InitViewState();
            return Redirect("/Account/Login");
        }

        public IActionResult Logins()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.SESSION_USERID)))
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        #region private
        //private async Task<UserInfo> SetUserInfo(string inputUserID, string inputPassword)
        //{
        //    UsersService _usersService = new UsersService();
        //    return await _usersService.LoginForBs(inputUserID, inputPassword);
        //}

        private async Task<UserInfo> SetUserInfo(string inputUserID, string inputPassword)
        {
            UsersService _usersService = new UsersService();
            //string result = "";
            //try
            //{
            //    //result = await _usersService.LoginForBs(inputUserID, inputPassword);
            //    //result = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetAPIBaseUrl + "/Users/GetForBs/" + inputUserID + "/" + inputPassword);
            //}
            //catch (Exception ex)
            //{
            //    SendAlertSMS(inputUserID);
            //    result = await CommonHelper.GetHttpClient().GetStringAsync(CommonHelper.Current.GetAPIBaseUrl_BAK + "/Users/GetForBs/" + inputUserID + "/" + inputPassword);
            //}

            var apiResult = await _usersService.LoginForBs(inputUserID, inputPassword); ;
            UserInfo userInfo;

            if (apiResult.ResultCode == ResultType.Success)
            {
                userInfo = CommonHelper.DecodeString<UserInfo>(apiResult.Body);
            }
            else
            {
                userInfo = null;
            }
            return userInfo;
        }

        private void SendAlertSMS(string userName)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("name", userName));
                paramList.Add(new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                string smsUrl = CommonHelper.Current.SMS_Service;
                var response = httpClient.PostAsync(smsUrl, new FormUrlEncodedContent(paramList)).Result;
            }
            catch (Exception ex)
            {
            }
        }
        private async void InitViewState()
        {
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_USERID);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_USERID);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_NAME);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_USERTYPE);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_ORGZIONID);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_ORGAREAID);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_ORGSERVERID);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_ORGDEPARTMENTID);
            //HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_ZIONLIST);
            HttpContext.Response.Cookies.Delete(SessionKeys.SESSION_DEPARTMENTLIST);
            HttpContext.Response.Cookies.Delete("fiat_ACCESS_TOKEN");
            HttpContext.Response.Cookies.Delete("fiat_ROLELIST1");
            HttpContext.Response.Cookies.Delete("fiat_ROLELIST2");
            HttpContext.Response.Cookies.Delete("fiat_ROLELIST3");
            HttpContext.Session.Clear();
            await HttpContext.Authentication.SignOutAsync("FIATCookieMiddlewareInstance");
        }

        #endregion

    }
}
