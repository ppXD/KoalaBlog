using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class UserAccount : EntityBase
    {
        public UserAccount()
        {
            this.RoleXUserAccounts = new List<RoleXUserAccount>();
            this.UserAccountXPermissions = new List<UserAccountXPermission>();
            this.EmailConfirmations = new List<EmailConfirmation>();
            this.UserAccountXPersons = new List<UserAccountXPerson>();
            this.Tokens = new List<Token>();
        }

        public long ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Status { get; set; }
        public System.DateTime LastLogon { get; set; }
        public bool IsOnline { get; set; }
        public virtual ICollection<RoleXUserAccount> RoleXUserAccounts { get; set; }
        public virtual ICollection<UserAccountXPermission> UserAccountXPermissions { get; set; }
        public virtual ICollection<EmailConfirmation> EmailConfirmations { get; set; }
        public virtual ICollection<UserAccountXPerson> UserAccountXPersons { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }

        public const string STATUS_ACTIVE = "Active";
        public const string STATUS_SUSPENDED = "Suspend";
    }
}
