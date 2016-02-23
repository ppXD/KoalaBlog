BEGIN TRANSACTION

--Person
CREATE TABLE [dbo].[Person](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[RealName] [nvarchar](20) NULL,
	[NickName] [nvarchar](50) NULL,
	[Gender] [int] NULL,
	[SexualTrend] [int] NULL,
	[MaritalStatus] [int] NULL,
	[QQ] [varchar](20) NULL,
	[DOB] [date] NULL,
	[HomePage] [varchar](50) NULL,
	[BloodType] [int] NULL,
	[Introduction] [nvarchar](150) NULL,
	[RealNameAccessLevel] [int] NOT NULL,
	[SexualTrendAccessLevel] [int] NOT NULL,
	[MaritalStatusAccessLevel] [int] NOT NULL,
	[QQAccessLevel] [int] NOT NULL,
	[DOBAccessLevel] [int] NOT NULL,
	[BloodTypeAccessLevel] [int] NOT NULL,
	[HomePageAccessLevel] [int] NOT NULL,
	[AllowablePersonForComment] [int] NOT NULL,
	[AllowCommentAttachContent] [bit] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

--Address
CREATE TABLE [dbo].[Address](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[AddressLine1] [nvarchar](150) NOT NULL,
	[AddressLine2] [nvarchar](150) NULL,
	[City] [nvarchar] (20) NULL,
	[Province] [nvarchar] (20) NULL,
	[Country] [nvarchar] (20) NULL,
	[PostCode] [nvarchar] (20) NULL,
	[AddressType] [int] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Person]
GO

--SchoolCategory
CREATE TABLE [dbo].[SchoolCategory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_SchoolCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--School
CREATE TABLE [dbo].[School](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[SchoolCategoryID] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Class] [nvarchar](30) NULL,
	[Department] [nvarchar](30) NULL,
	[Location] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[School]  WITH CHECK ADD  CONSTRAINT [FK_School_SchoolCategory] FOREIGN KEY([SchoolCategoryID])
REFERENCES [dbo].[SchoolCategory] ([ID])
GO

ALTER TABLE [dbo].[School] CHECK CONSTRAINT [FK_School_SchoolCategory]
GO

--Education
CREATE TABLE [dbo].[Education](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[SchoolID] [bigint] NOT NULL,
	[SchoolYear] [int] NOT NULL,
	[AccessLevel] [int] NOT NULL,
 CONSTRAINT [PK_Education] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Education]  WITH CHECK ADD  CONSTRAINT [FK_Education_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Education] CHECK CONSTRAINT [FK_Education_Person]
GO

ALTER TABLE [dbo].[Education]  WITH CHECK ADD  CONSTRAINT [FK_Education_School] FOREIGN KEY([SchoolID])
REFERENCES [dbo].[School] ([ID])
GO

ALTER TABLE [dbo].[Education] CHECK CONSTRAINT [FK_Education_School]
GO

--Job
CREATE TABLE [dbo].[Job](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[DepartmentOrPosition] [nvarchar](50) NULL,
	[WorkStartDate] [datetime] NULL,
	[WorkEndDate] [datetime] NOT NULL,
	[WorkCity] [nvarchar](50) NOT NULL,
	[WorkProvince] [nvarchar](50) NOT NULL,
	[AccessLevel] [int] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Person]
GO

--Tag
CREATE TABLE [dbo].[Tag](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Tag]  WITH CHECK ADD  CONSTRAINT [FK_Tag_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Tag] CHECK CONSTRAINT [FK_Tag_Person]
GO

--Avatar
CREATE TABLE [dbo].[Avatar](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[AvatarPath] [nvarchar](150) NULL,
	[AvatarBinary] [varbinary](max) NULL,
	[MimeType] [nvarchar](30) NOT NULL,
	[AltAttribute] [nvarchar](30) NULL,
	[TitleAttribute] [nvarchar](50) NULL,
	[SeoFilename] [nvarchar](300) NULL, 
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Avatar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Avatar]  WITH CHECK ADD  CONSTRAINT [FK_Avatar_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Avatar] CHECK CONSTRAINT [FK_Avatar_Person]
GO

--UserAccountXPerson
CREATE TABLE [dbo].[UserAccountXPerson](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[UserAccountID] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
 CONSTRAINT [PK_UserAccountXPerson] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserAccountXPerson]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountXPerson_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
GO

ALTER TABLE [dbo].[UserAccountXPerson] CHECK CONSTRAINT [FK_UserAccountXPerson_UserAccount]
GO

ALTER TABLE [dbo].[UserAccountXPerson]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountXPerson_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[UserAccountXPerson] CHECK CONSTRAINT [FK_UserAccountXPerson_Person]
GO

--Blog
CREATE TABLE [dbo].[Blog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[Content] [nvarchar](200) NOT NULL,
	[IsDeleted] [bit] NOT NULL
 CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_Blog_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Blog] CHECK CONSTRAINT [FK_Blog_Person]
GO

--Favorite
CREATE TABLE [dbo].[Favorite](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[BlogID] [bigint] NOT NULL,
 CONSTRAINT [PK_Favorite] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Favorite]  WITH CHECK ADD  CONSTRAINT [FK_Favorite_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Favorite] CHECK CONSTRAINT [FK_Favorite_Person]
GO

ALTER TABLE [dbo].[Favorite]  WITH CHECK ADD  CONSTRAINT [FK_Favorite_Blog] FOREIGN KEY([BlogID])
REFERENCES [dbo].[Blog] ([ID])
GO

ALTER TABLE [dbo].[Favorite] CHECK CONSTRAINT [FK_Favorite_Blog]
GO

--Content
CREATE TABLE [dbo].[Content](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[ContentPath] [nvarchar](150) NULL,
	[ContentBinary] [varbinary](max) NULL,
	[MimeType] [nvarchar](30) NOT NULL,
	[AltAttribute] [nvarchar](30) NULL,
	[TitleAttribute] [nvarchar](50) NULL,
	[SeoFilename] [nvarchar](300) NULL, 
	[Type] [int] NOT NULL
 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--BlogXContent
CREATE TABLE [dbo].[BlogXContent](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[BlogID] [bigint] NOT NULL,
	[ContentID] [bigint] NOT NULL,
 CONSTRAINT [PK_BlogXContent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BlogXContent]  WITH CHECK ADD  CONSTRAINT [FK_BlogXContent_Blog] FOREIGN KEY([BlogID])
REFERENCES [dbo].[Blog] ([ID])
GO

ALTER TABLE [dbo].[BlogXContent] CHECK CONSTRAINT [FK_BlogXContent_Blog]
GO

ALTER TABLE [dbo].[BlogXContent]  WITH CHECK ADD  CONSTRAINT [FK_BlogXContent_Content] FOREIGN KEY([ContentID])
REFERENCES [dbo].[Content] ([ID])
GO

ALTER TABLE [dbo].[BlogXContent] CHECK CONSTRAINT [FK_BlogXContent_Content]
GO

--BlogXBlog
CREATE TABLE [dbo].[BlogXBlog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[BaseBlogID] [bigint] NOT NULL,
	[NewBlogID] [bigint] NOT NULL,
	[IsBase] [bit] NOT NULL,
 CONSTRAINT [PK_BlogXBlog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BlogXBlog]  WITH CHECK ADD  CONSTRAINT [FK_BlogXBlog_BaseBlog] FOREIGN KEY([BaseBlogID])
REFERENCES [dbo].[Blog] ([ID])
GO

ALTER TABLE [dbo].[BlogXBlog] CHECK CONSTRAINT [FK_BlogXBlog_BaseBlog]
GO

ALTER TABLE [dbo].[BlogXBlog]  WITH CHECK ADD  CONSTRAINT [FK_BlogXBlog_NewBlog] FOREIGN KEY([NewBlogID])
REFERENCES [dbo].[Blog] ([ID])
GO

ALTER TABLE [dbo].[BlogXBlog] CHECK CONSTRAINT [FK_BlogXBlog_NewBlog]
GO

--Comment
CREATE TABLE [dbo].[Comment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[BlogID] [bigint] NOT NULL,
	[Content] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Person]
GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Blog] FOREIGN KEY([BlogID])
REFERENCES [dbo].[Blog] ([ID])
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Blog]
GO

--CommentXComment
CREATE TABLE [dbo].[CommentXComment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[BaseCommentID] [bigint] NOT NULL,
	[NewCommentID] [bigint] NOT NULL,
 CONSTRAINT [PK_CommentXComment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CommentXComment]  WITH CHECK ADD  CONSTRAINT [FK_CommentXComment_BaseComment] FOREIGN KEY([BaseCommentID])
REFERENCES [dbo].[Comment] ([ID])
GO

ALTER TABLE [dbo].[CommentXComment] CHECK CONSTRAINT [FK_CommentXComment_BaseComment]
GO

ALTER TABLE [dbo].[CommentXComment]  WITH CHECK ADD  CONSTRAINT [FK_CommentXComment_NewComment] FOREIGN KEY([NewCommentID])
REFERENCES [dbo].[Comment] ([ID])
GO

ALTER TABLE [dbo].[CommentXComment] CHECK CONSTRAINT [FK_CommentXComment_NewComment]
GO

--CommentXContent
CREATE TABLE [dbo].[CommentXContent](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[CommentID] [bigint] NOT NULL,
	[ContentID] [bigint] NOT NULL,
 CONSTRAINT [PK_CommentXContent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CommentXContent]  WITH CHECK ADD  CONSTRAINT [FK_CommentXContent_Comment] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comment] ([ID])
GO

ALTER TABLE [dbo].[CommentXContent] CHECK CONSTRAINT [FK_CommentXContent_Comment]
GO

ALTER TABLE [dbo].[CommentXContent]  WITH CHECK ADD  CONSTRAINT [FK_CommentXContent_Content] FOREIGN KEY([ContentID])
REFERENCES [dbo].[Content] ([ID])
GO

ALTER TABLE [dbo].[CommentXContent] CHECK CONSTRAINT [FK_CommentXContent_Content]
GO

--EntityLike
CREATE TABLE [dbo].[EntityLike](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[EntityID] [bigint] NOT NULL,
	[EntityTableName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EntityLike] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EntityLike]  WITH CHECK ADD  CONSTRAINT [FK_EntityLike_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[EntityLike] CHECK CONSTRAINT [FK_EntityLike_Person]
GO

--EntityConcurrentLock
CREATE TABLE [dbo].[EntityConcurrentLock](
	[PersonID] [bigint] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_EntityConcurrentLock] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--EntityComplain
CREATE TABLE [dbo].[EntityComplain](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[EntityID] [bigint] NOT NULL,
	[EntityTableName] [nvarchar](100) NOT NULL,
	[Type] [int] NOT NULL,
	[Reason] [nvarchar] (200) NULL,
 CONSTRAINT [PK_EntityComplain] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EntityComplain]  WITH CHECK ADD  CONSTRAINT [FK_EntityComplain_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[EntityComplain] CHECK CONSTRAINT [FK_EntityComplain_Person]
GO

--PersonXPerson
CREATE TABLE [dbo].[PersonXPerson](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[FollowerID] [bigint] NOT NULL,
	[FollowingID] [bigint] NOT NULL,
 CONSTRAINT [PK_PersonXPerson] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PersonXPerson]  WITH CHECK ADD  CONSTRAINT [FK_PersonXPerson_Follower] FOREIGN KEY([FollowerID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[PersonXPerson] CHECK CONSTRAINT [FK_PersonXPerson_Follower]
GO

ALTER TABLE [dbo].[PersonXPerson]  WITH CHECK ADD  CONSTRAINT [FK_PersonXPerson_Following] FOREIGN KEY([FollowingID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[PersonXPerson] CHECK CONSTRAINT [FK_PersonXPerson_Following]
GO

--Group
CREATE TABLE [dbo].[Group](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Person]
GO

--GroupMember
CREATE TABLE [dbo].[GroupMember](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[GroupID] [bigint] NOT NULL,
	[PersonID] [bigint] NOT NULL,
 CONSTRAINT [PK_GroupMember] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GroupMember]  WITH CHECK ADD  CONSTRAINT [FK_GroupMember_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO

ALTER TABLE [dbo].[GroupMember] CHECK CONSTRAINT [FK_GroupMember_Group]
GO

ALTER TABLE [dbo].[GroupMember]  WITH CHECK ADD  CONSTRAINT [FK_GroupMember_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[GroupMember] CHECK CONSTRAINT [FK_GroupMember_Person]
GO

--AccessControl
CREATE TABLE [dbo].[BlogAccessControl](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[BlogID] [bigint] NOT NULL,
	[AccessLevel] [int] NOT NULL,
 CONSTRAINT [PK_BlogAccessControl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BlogAccessControl]  WITH CHECK ADD  CONSTRAINT [FK_BlogAccessControl_Blog] FOREIGN KEY([BlogID])
REFERENCES [dbo].[Blog] ([ID])
GO

ALTER TABLE [dbo].[BlogAccessControl] CHECK CONSTRAINT [FK_BlogAccessControl_Blog]
GO

--AccessControlXGroup
CREATE TABLE [dbo].[BlogAccessControlXGroup](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[BlogAccessControlID] [bigint] NOT NULL,
	[GroupID] [bigint] NOT NULL,
 CONSTRAINT [PK_BlogAccessControlXGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BlogAccessControlXGroup]  WITH CHECK ADD  CONSTRAINT [FK_BlogAccessControlXGroup_BlogAccessControl] FOREIGN KEY([BlogAccessControlID])
REFERENCES [dbo].[BlogAccessControl] ([ID])
GO

ALTER TABLE [dbo].[BlogAccessControlXGroup] CHECK CONSTRAINT [FK_BlogAccessControlXGroup_BlogAccessControl]
GO

ALTER TABLE [dbo].[BlogAccessControlXGroup]  WITH CHECK ADD  CONSTRAINT [FK_BlogAccessControlXGroup_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO

ALTER TABLE [dbo].[BlogAccessControlXGroup] CHECK CONSTRAINT [FK_BlogAccessControlXGroup_Group]
GO

COMMIT TRANSACTION