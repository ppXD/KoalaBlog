BEGIN TRANSACTION

CREATE TABLE [dbo].[UserAccount](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [varchar](150) NOT NULL,
	[PasswordSalt] [varchar] (10) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[LastLogon] [datetime] NOT NULL,
	[IsOnline] [bit] NOT NULL,
 CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Permission](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[UserAccountXPermission](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[UserAccountID] [bigint] NOT NULL,
	[PermissionID] [bigint] NOT NULL,
 CONSTRAINT [PK_UserAccountXPermission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserAccountXPermission]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountXPermission_Permission] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permission] ([ID])
GO

ALTER TABLE [dbo].[UserAccountXPermission] CHECK CONSTRAINT [FK_UserAccountXPermission_Permission]
GO

ALTER TABLE [dbo].[UserAccountXPermission]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountXPermission_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
GO

ALTER TABLE [dbo].[UserAccountXPermission] CHECK CONSTRAINT [FK_UserAccountXPermission_UserAccount]
GO

CREATE TABLE [dbo].[Role](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[RoleXPermission](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[RoleID] [bigint] NOT NULL,
	[PermissionID] [bigint] NOT NULL,
 CONSTRAINT [PK_RoleXPermission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoleXPermission]  WITH CHECK ADD  CONSTRAINT [FK_RoleXPermission_Permission] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permission] ([ID])
GO

ALTER TABLE [dbo].[RoleXPermission] CHECK CONSTRAINT [FK_RoleXPermission_Permission]
GO

ALTER TABLE [dbo].[RoleXPermission]  WITH CHECK ADD  CONSTRAINT [FK_RoleXPermission_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO

ALTER TABLE [dbo].[RoleXPermission] CHECK CONSTRAINT [FK_RoleXPermission_Role]
GO

CREATE TABLE [dbo].[RoleXUserAccount](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[UserAccountID] [bigint] NOT NULL,
	[RoleID] [bigint] NOT NULL,
 CONSTRAINT [PK_RoleXUserAccount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoleXUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_RoleXUserAccount_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO

ALTER TABLE [dbo].[RoleXUserAccount] CHECK CONSTRAINT [FK_RoleXUserAccount_Role]
GO

ALTER TABLE [dbo].[RoleXUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_RoleXUserAccount_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
GO

ALTER TABLE [dbo].[RoleXUserAccount] CHECK CONSTRAINT [FK_RoleXUserAccount_UserAccount]
GO

CREATE TABLE [dbo].[Log](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[LogSource] [nvarchar](350) NOT NULL,
	[LogMessage] [ntext] NOT NULL,
	[LogLevel] [int] NOT NULL,
	CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[EmailConfirmation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[UserAccountID] [bigint] NOT NULL,
	[Code] [nvarchar](250) NOT NULL,
	[Type] [int] NOT NULL,
	CONSTRAINT [PK_EmailConfirmation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EmailConfirmation]  WITH CHECK ADD  CONSTRAINT [FK_EmailConfirmation_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
GO

ALTER TABLE [dbo].[EmailConfirmation] CHECK CONSTRAINT [FK_EmailConfirmation_UserAccount]
GO

CREATE TABLE [dbo].[Token](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NOT NULL,
	[UserAccountID] [bigint] NOT NULL,
	[AccessToken] [nvarchar](150) NOT NULL,
	[TokenType] [int] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[IsSlidingExpiration] [bit] NOT NULL,
	[IsRevoked] [bit] NOT NULL,
 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Token]  WITH CHECK ADD  CONSTRAINT [FK_Token_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
GO

COMMIT TRANSACTION