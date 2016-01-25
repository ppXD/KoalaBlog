using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.DTOs
{
    public class PersonDTO
    {
        public long ID { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 性取向
        /// </summary>
        public string SexualTrend { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritalStatus { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public Nullable<System.DateTime> DOB { get; set; }
        /// <summary>
        /// 主页
        /// </summary>
        public string HomePage { get; set; }
        /// <summary>
        /// 血型
        /// </summary>
        public string BloodType { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public int FollowingCount { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// Blog数量
        /// </summary>
        public int BlogCount { get; set; }
    }
}
