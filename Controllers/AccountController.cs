#region Copyright Syncfusion Inc. 2001-2017.
// Copyright Syncfusion Inc. 2001-2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using mascis.Models;
using System.Collections.Generic;
using System.Collections;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.JavaScript;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
//using mascis.Models;

namespace mascis.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context = new ApplicationDbContext();
        mascisEntities db = new mascisEntities();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            DataTable dt = new DataTable();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            string connectionString = ConfigurationManager.ConnectionStrings["MapConnection"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=MEDACCKAMSAP03\MSSQL2017"))

            //using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=."))
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spView_Facility_GIS_CodinatesGetAll";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    SqlParameter[] sqlParameters = new SqlParameter[1];

                    sqlParameters[0] = new SqlParameter("@FacilityCode", SqlDbType.Int);
                    sqlParameters[0].IsNullable = true;

                    //if (wdd_Facility.SelectedValue != "")
                    //    sqlParameters[0].Value = wdd_Facility.SelectedValue;
                    //else
                    sqlParameters[0].Value = DBNull.Value;
                    sqlParameters[0].Direction = ParameterDirection.Input;


                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(sqlParameters);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    //return serializer.Serialize(rows);
                }
            }
            ViewBag.GISdata = rows;// Syncfusion_LocationData.GetSyncfusionLocationData();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        mascisEntities context = new mascisEntities();
                        var user = await UserManager.FindAsync(model.Email, model.Password);
                        var lockout = context.AspNetUsers.Find(user.Id);
                        if(lockout.LockoutEnabled == true)
                        {
                            return View("Lockout");
                        }
                        else
                        {
                            //var roles = await UserManager.GetRolesAsync(user.Id);
                            var roles = context.View_UsersRoles.SingleOrDefault(a => a.UserName == model.Email);
                            if (roles != null)
                            {
                                if (roles.Name.Contains("Administrator"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("MonitoringAndEvaluationOfficer"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("SCTO"))
                                    return RedirectToAction("SCTODashboard", "Dashboard");
                                if (roles.Name.Contains("MAULT"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("MAULClient"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("HSIPClient"))
                                    return RedirectToAction("FacilityDashboard", "Dashboard");
                                if (roles.Name.Contains("LMISCordinator"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("QualityAssurance"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("CCU"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("CHSS"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");
                                if (roles.Name.Contains("ComplaintHandling"))
                                    return RedirectToAction("LMISDashboard", "Dashboard");

                                else
                                    // return RedirectToLocal(returnUrl);
                                    return RedirectToAction("Login", "Account");
                            }
                            else
                            {
                                //return RedirectToLocal(returnUrl);
                                return RedirectToAction("Login", "Account");
                            }
                        }
                        
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");

                    DataTable dt = new DataTable();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    string connectionString = ConfigurationManager.ConnectionStrings["MapConnection"].ConnectionString;
                    //using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=MEDACCKAMSAP03\MSSQL2017"))

                    //using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=."))
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "spView_Facility_GIS_CodinatesGetAll";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;

                            SqlParameter[] sqlParameters = new SqlParameter[1];

                            sqlParameters[0] = new SqlParameter("@FacilityCode", SqlDbType.Int);
                            sqlParameters[0].IsNullable = true;

                            //if (wdd_Facility.SelectedValue != "")
                            //    sqlParameters[0].Value = wdd_Facility.SelectedValue;
                            //else
                            sqlParameters[0].Value = DBNull.Value;
                            sqlParameters[0].Direction = ParameterDirection.Input;


                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(sqlParameters);
                            con.Open();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            foreach (DataRow dr in dt.Rows)
                            {
                                row = new Dictionary<string, object>();
                                foreach (DataColumn col in dt.Columns)
                                {
                                    row.Add(col.ColumnName, dr[col]);
                                }
                                rows.Add(row);
                            }
                            //return serializer.Serialize(rows);
                        }
                    }
                    ViewBag.GISdata = rows;// Syncfusion_LocationData.GetSyncfusionLocationData();

                    //ViewBag.ReturnUrl = returnUrl;
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        public ActionResult GetContact(string name)
        {
            var result = db.View_fo_Contacts.FirstOrDefault(o => o.cp_name.Trim() == name.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            context.Configuration.ProxyCreationEnabled = false;
            ViewBag.Roles = context.Roles.ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.UserType = db.A_UserType.ToList();
            context.Configuration.ProxyCreationEnabled = false;
            //ViewBag.Facility = db.A_Facilities.ToList();
            db.Configuration.ProxyCreationEnabled = false;
            var contact = db.View_fo_Contacts.ToList();
            ViewBag.Contacts = contact;
            List<DropdownList> obj = new List<DropdownList>();
            obj.Add(new DropdownList { ID = "", DESC = "--Please Select--", FK_ID = "" });

            db.Configuration.ProxyCreationEnabled = false;
            var data = db.A_Facilities.AsNoTracking().OrderBy(a => a.Facility);

            foreach (var x in data)
            {
                obj.Add(new DropdownList { ID = x.FacilityCode.ToString(), DESC = x.Facility });
            }
            ViewBag.Facility = obj;

            return View();
        }

        public class DropdownList
        {
            public string ID { get; set; }
            public string FK_ID { get; set; }
            public string DESC { get; set; }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["FacilityList"] != null && Request["FacilityList"] != string.Empty)
                {
                    string facility = Request["FacilityList"].ToString();
                    model.FacilityId = System.Convert.ToInt32(facility);
                }
                if (Request["UserTypeList"] != null && Request["UserTypeList"] != string.Empty)
                {
                    string user_type = Request["UserTypeList"].ToString();
                    model.UserTypeId = System.Convert.ToInt32(user_type);
                }
                var hased = MD5Hash(model.Password);
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email,PhoneNumber = model.PhoneNumber,Name=model.Name,UserTypeId=System.Convert.ToInt32(model.UserTypeId), FacilityId = model.FacilityId, MD5Hash = hased,LockoutEnabled=false };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    if (Request["RolesList"] != null && Request["RolesList"] != string.Empty)
                    {
                        string user_role = Request["RolesList"].ToString();
                        model.UserRole = user_role;
                    }
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRole);
                    var contact = db.fo_contact_person.FirstOrDefault(o => o.cp_name.Trim() == model.Name.Trim());
                    if(contact != null)
                    {
                        contact.User_ID = user.Id;
                        db.Entry(contact).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Register", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            //return code == null ? View("Error") : View();

            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = UserManager.ChangePassword(user.Id, model.oldPassword, model.Password);
            if (result.Succeeded)
            {
                var hased = MD5Hash(model.Password);
                var person = db.AspNetUsers.FirstOrDefault(o => o.Id == user.Id);
                if (person != null)
                {
                    person.MD5Hash = hased;
                    db.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        public ActionResult Usermanagement()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.UserType = db.A_UserType.AsNoTracking().ToList();

            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Facility = db.A_Facilities.AsNoTracking().OrderBy(d => d.Facility).ToList();

            //db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Roles = db.View_AspNetRoles.OrderBy(s => s.Name);
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Contact = db.fo_contact_person.AsNoTracking().ToList();

            return View();
        }

        public ActionResult DataSourceUserManagement(DataManager dm)
        {
            IEnumerable data = db.View_UserManagement.OrderBy(a => a.Email).ToList();
            int count = db.View_UserManagement.OrderBy(a => a.Email).ToList().Count;

            DataOperations operation = new DataOperations();
            //Performing filtering operation
            if (dm.Where != null)
            {
                data = operation.PerformWhereFilter(data, dm.Where, "and");
                var filtered = (IEnumerable<object>)data;
                count = filtered.Count();
            }
            //Performing search operation
            if (dm.Search != null)
            {
                data = operation.PerformSearching(data, dm.Search);
                var searched = (IEnumerable<object>)data;
                count = searched.Count();
            }
            //Performing sorting operation
            if (dm.Sorted != null)
                data = operation.PerformSorting(data, dm.Sorted);

            //Performing paging operations
            if (dm.Skip > 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take > 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateUserInformation(string key, List<View_UserManagement> changed, List<View_UserManagement> added, List<View_UserManagement> deleted)
        {
            Models.ApplicationDbContext contextX = new Models.ApplicationDbContext();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var x = db.A_Facilities.FirstOrDefault(a => a.FacilityCode == temp.FacilityId);
                    var user = UserManager.FindById(temp.Id);
                    user.UserName = temp.UserName;
                    user.UserTypeId = temp.UserTypeId;
                    user.Email = temp.Email;
                    user.FacilityId = temp.FacilityId;
                    user.Name = temp.Name;
                    user.PhoneNumber = temp.PhoneNumber;
                    user.LockoutEndDateUtc = temp.LockoutEndDateUtc;
                    user.LockoutEnabled = temp.LockoutEnabled;
                    UserManager.Update(user);

                    db.Configuration.ProxyCreationEnabled = false;
                    var userStore = new UserStore<ApplicationUser>(contextX);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    //IdentityResult IdUserResult;
                    foreach (string r in userManager.GetRoles(temp.Id))
                    {
                        userManager.RemoveFromRole(temp.Id, r);
                    }
                    userManager.AddToRole(temp.Id, temp.Role);

                    var _contact = db.fo_contact_person.FirstOrDefault(o => o.User_ID == temp.Id);
                    if(_contact != null)
                    {
                        _contact.User_ID = null;
                        db.Entry(_contact).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    var contact = db.fo_contact_person.FirstOrDefault(o => o.cp_name.Trim() == temp.Name.Trim());
                    if (contact != null)
                    {
                        contact.User_ID = temp.Id;
                        db.Entry(contact).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            //return null;
            var data = 0;// new CouncilCountryModels().GetAll(1).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("GridEdit");
        }

        #region "Reset User Password ---- 06-Mar-2019

        public ActionResult UsersWithRoles()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new Users_in_Role_ViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }


        public ActionResult ResetUserPassword(string userId, string UserName)
        {
            ViewBag.Username = UserName.ToString();
            ViewBag.UserId = userId.Trim().ToString();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetUserPassword(ResetUserPasswordViewModel model)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
            String userId = model.UserId;// User.Identity.GetUserId();//"<YourLogicAssignsRequestedUserId>";
            String newPassword = model.ConfirmPassword;// "test@123"; //"<PasswordAsTypedByUser>";
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            ApplicationUser cUser = await store.FindByIdAsync(userId);
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);
            var hased = MD5Hash(model.ConfirmPassword);
            var person = db.AspNetUsers.FirstOrDefault(o => o.Id == userId);
            if(person != null)
            {
                 person.MD5Hash = hased;
                db.Entry(person).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("UsersWithRoles", "Account", new { area = "", });
            //return View(model);
        }


        #endregion

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}