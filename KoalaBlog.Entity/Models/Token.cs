using System;

namespace KoalaBlog.Entity.Models
{
    public partial class Token : EntityBase
    {
        public long ID { get; set; }
        public long UserAccountID { get; set; }
        public string AccessToken { get; set; }
        public KoalaBlog.Entity.Models.Enums.TokenType TokenType { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public bool IsSlidingExpiration { get; set; }
        public bool IsRevoked { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
