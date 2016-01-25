using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebModels
{
    public class Person
    {
        public long ID { get; set; }
        public string AvatarUrl { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string SexualTrend { get; set; }
        public string MaritalStatus { get; set; }
        public string QQ { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string HomePage { get; set; }
        public string BloodType { get; set; }
        public string Introduction { get; set; }
        public int FollowingCount { get; set; }
        public int FansCount { get; set; }
        public int BlogCount { get; set; }
    }
}
