using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Principal
{
    public class KoalaBlogIdentityObject
    {
        public long UserID { get; set; }
        public long PersonID { get; set; }
        public string PersonNickName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string AvatarUrl { get; set; }
        public string Introduction { get; set; }
    }
}
