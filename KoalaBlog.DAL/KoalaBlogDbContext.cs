using System;
using System.Linq;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using KoalaBlog.Entity;
using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Mapping;

namespace KoalaBlog.DAL
{
    public partial class KoalaBlogDbContext : DbContext
    {
        static KoalaBlogDbContext()
        {
            Database.SetInitializer<KoalaBlogDbContext>(null);
        }

        public KoalaBlogDbContext()
            : base("Name=KoalaBlogDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
        }
        public DbSet<BlogAccessControl> BlogAccessControls { get; set; }
        public DbSet<BlogAccessControlXGroup> BlogAccessControlXGroups { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogXBlog> BlogXBlogs { get; set; }
        public DbSet<BlogXContent> BlogXContents { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentXComment> CommentXComments { get; set; }
        public DbSet<CommentXContent> CommentXContents { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<EntityComplain> EntityComplains { get; set; }
        public DbSet<EntityLike> EntityLikes { get; set; }
        public DbSet<EntityConcurrentLock> EntityLikeLocks { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonXPerson> PersonXPersons { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolCategory> SchoolCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleXPermission> RoleXPermissions { get; set; }
        public DbSet<RoleXUserAccount> RoleXUserAccounts { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserAccountXPermission> UserAccountXPermissions { get; set; }
        public DbSet<UserAccountXPerson> UserAccountXPersons { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region manually load all configuration.
            //modelBuilder.Configurations.Add(new AccessControlMap());
            //modelBuilder.Configurations.Add(new AccessControlXGroupMap());
            //modelBuilder.Configurations.Add(new AddressMap());
            //modelBuilder.Configurations.Add(new AvatarMap());
            //modelBuilder.Configurations.Add(new BlogMap());
            //modelBuilder.Configurations.Add(new BlogXBlogMap());
            //modelBuilder.Configurations.Add(new BlogXContentMap());
            //modelBuilder.Configurations.Add(new CommentMap());
            //modelBuilder.Configurations.Add(new CommentXCommentMap());
            //modelBuilder.Configurations.Add(new CommentXContentMap());
            //modelBuilder.Configurations.Add(new ContentMap());
            //modelBuilder.Configurations.Add(new EntityComplainMap());
            //modelBuilder.Configurations.Add(new EntityLikeMap());
            //modelBuilder.Configurations.Add(new FavoriteMap());
            //modelBuilder.Configurations.Add(new GroupMap());
            //modelBuilder.Configurations.Add(new GroupMemberMap());
            //modelBuilder.Configurations.Add(new EducationMap());
            //modelBuilder.Configurations.Add(new JobMap());
            //modelBuilder.Configurations.Add(new PersonMap());
            //modelBuilder.Configurations.Add(new PersonXPersonMap());
            //modelBuilder.Configurations.Add(new SchoolMap());
            //modelBuilder.Configurations.Add(new SchoolCategoryMap());
            //modelBuilder.Configurations.Add(new TagMap());
            //modelBuilder.Configurations.Add(new PermissionMap());
            //modelBuilder.Configurations.Add(new RoleMap());
            //modelBuilder.Configurations.Add(new RoleXPermissionMap());
            //modelBuilder.Configurations.Add(new RoleXUserAccountMap());
            //modelBuilder.Configurations.Add(new UserAccountMap());
            //modelBuilder.Configurations.Add(new UserAccountXPermissionMap());
            //modelBuilder.Configurations.Add(new UserAccountXPersonMap());
            //modelBuilder.Configurations.Add(new LogMap());
            //modelBuilder.Configurations.Add(new EmailConfirmationMap());
            #endregion

            //dynamically load all configuration.
            var typesToRegister = Assembly.GetAssembly(typeof(EntityTypeConfigurationBase<>))
                                          .GetTypes()
                                          .Where(
                                                 type => !String.IsNullOrEmpty(type.Namespace) &&
                                                 type.BaseType != null &&
                                                 type.BaseType.IsGenericType &&
                                                 type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfigurationBase<>)
                                                );

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
    }
}
