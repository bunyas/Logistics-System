using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mascis.Models
{
    public class UserManagement
    {
        ApplicationDbContext context = new ApplicationDbContext();
        IdentityResult IdRoleResult;

        public object IdentityModels { get; private set; }

        public Boolean createRole(string role)
        {
            try
            {

                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                if (!roleManager.RoleExists(role))
                {
                    IdRoleResult = roleManager.Create(new IdentityRole(role));
                    if (!IdRoleResult.Succeeded)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //public Boolean createUser(string user_name, string district, string password, string email, string role)
        //{
        //    try
        //    {
        //        var userStore = new UserStore<ApplicationUser>(context);
        //        var userManager = new UserManager<ApplicationUser>(userStore);
        //        var appUser = new ApplicationUser()
        //        {
        //            UserName = user_name,
        //            District_Id = district,
        //            Email = email,
        //        };
        //        IdUserResult = userManager.Create(appUser, password);
        //        if (IdUserResult.Succeeded)
        //        {
        //            IdUserResult = userManager.AddToRole(appUser.Id, role);
        //        }
        //        if (!IdUserResult.Succeeded)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        throw (x);
        //    }
        //}

        public string getCurrentuser()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }

        public int? getCurrentuserFacility()
        {
            string UserName = HttpContext.Current.User.Identity.GetUserName();
            var userStore = new UserStore<ApplicationUser>(context);
            var x = new UserManager<ApplicationUser>(userStore).Users.SingleOrDefault(a => a.UserName == UserName);
            if (x != null)
                return new UserManager<ApplicationUser>(userStore).Users.SingleOrDefault(a => a.UserName == UserName).FacilityId;
            else
                return null;
        }

        public List<int> getUserFacilityList()
        {
            string UserName = HttpContext.Current.User.Identity.GetUserName();
            var userStore = new UserStore<ApplicationUser>(context);
            mascisEntities masciscontext = new mascisEntities();
            var facilities = masciscontext.A_Facilities.Where(f => f.RFSO_UserName == UserName).ToList();
            //var districts = new UserManager<ApplicationUser>(userStore).Users.SingleOrDefault(a => a.UserName == UserName).DistrictSupervor;

            List<int> facilitiesarray = new List<int>();
            if (facilities != null)
            {
                foreach(A_Facilities f in facilities )
                {
                    facilitiesarray.Add(f.FacilityCode);
                }
               
            }
            return (facilitiesarray);

        }

        public List<string> getCurrentFacilityEmail(int? facilityCode)
        {
            mascisEntities dbcontext = new mascisEntities();
            List<string> m = new List<string>();
           var a= dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilityCode && (e.DisableAll == false || e.DisableAll == null) && e.IsActive==true).ToList();
           if(a.Count > 0)
            {
                foreach(var n in a)
                {
                    m.Add(n.ce_email);
                }
            }
            return m;
            
            
            //string UserName = HttpContext.Current.User.Identity.GetUserName();
            //var userStore = new UserStore<ApplicationUser>(context);
            //var x = new UserManager<ApplicationUser>(userStore).Users.SingleOrDefault(a => a.UserName == UserName);
            //if (x != null)
            //    return  dbcontext.View_WebTemplate_RecipientEmail.(a => a.UserName == UserName).FacilityId;
            //else
            //    return null;
        }

        public List<string> getCurrentLMISCOdinatorEmail()
        {
            mascisEntities dbcontext = new mascisEntities();
            List<string> m = new List<string>();
            var a = dbcontext.View_fo_Contacts_Details.Where(e => e.Role == "LMISCordinator" && e.LockoutEnabled==false &&(e.DisableAll== false || e.DisableAll == null) && e.IsActive== true ).ToList();
            if (a.Count > 0)
            {
                foreach (var n in a)
                {
                    m.Add(n.ce_email);
                }
            }
            return m;
 
        }

    }
}