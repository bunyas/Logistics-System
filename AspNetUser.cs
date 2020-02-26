
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string Name { get; set; }
        public string MD5Hash { get; set; }
    
        public virtual A_UserType A_UserType { get; set; }
        public virtual A_Facilities A_Facilities { get; set; }
    }
}
