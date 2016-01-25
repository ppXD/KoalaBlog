using KoalaBlog.Entity.Models.Enums;
using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Person : EntityBase
    {
        public Person()
        {
            this.Addresses = new List<Address>();
            this.Avatars = new List<Avatar>();
            this.Blogs = new List<Blog>();
            this.Comments = new List<Comment>();
            this.EntityComplains = new List<EntityComplain>();
            this.EntityLikes = new List<EntityLike>();
            this.Favorites = new List<Favorite>();
            this.Groups = new List<Group>();
            this.GroupMembers = new List<GroupMember>();
            this.Jobs = new List<Job>();
            this.Educations = new List<Education>();
            this.MyFollowingPersons = new List<PersonXPerson>();
            this.MyFans = new List<PersonXPerson>();
            this.Tags = new List<Tag>();
            this.UserAccountXPersons = new List<UserAccountXPerson>();
        }

        public long ID { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public Nullable<Gender> Gender { get; set; }
        public Nullable<SexualTrend> SexualTrend { get; set; }
        public Nullable<MaritalStatus> MaritalStatus { get; set; }
        public string QQ { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string HomePage { get; set; }
        public Nullable<BloodType> BloodType { get; set; }
        public string Introduction { get; set; }
        public PersonInfoAccessInfo RealNameAccessLevel { get; set; }
        public PersonInfoAccessInfo SexualTrendAccessLevel { get; set; }
        public PersonInfoAccessInfo MaritalStatusAccessLevel { get; set; }
        public PersonInfoAccessInfo QQAccessLevel { get; set; }
        public PersonInfoAccessInfo DOBAccessLevel { get; set; }
        public PersonInfoAccessInfo BloodTypeAccessLevel { get; set; }
        public PersonInfoAccessInfo HomePageAccessLevel { get; set; }
        public AllowablePersonForComment AllowablePersonForComment { get; set; }
        public bool AllowCommentAttachContent { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Avatar> Avatars { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<EntityComplain> EntityComplains { get; set; }
        public virtual ICollection<EntityLike> EntityLikes { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        /// <summary>
        /// 我正在关注的人
        /// </summary>
        public virtual ICollection<PersonXPerson> MyFollowingPersons { get; set; }
        /// <summary>
        /// 我的粉丝
        /// </summary>
        public virtual ICollection<PersonXPerson> MyFans { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<UserAccountXPerson> UserAccountXPersons { get; set; }
    }
}
