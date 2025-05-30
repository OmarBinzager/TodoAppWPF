USE [master]
GO
/****** Object:  Database [TodoDB2]    Script Date: 5/19/2025 2:08:51 AM ******/
CREATE DATABASE [TodoDB2]
GO
ALTER DATABASE [TodoDB2] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TodoDB2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TodoDB2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TodoDB2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TodoDB2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TodoDB2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TodoDB2] SET ARITHABORT OFF 
GO
ALTER DATABASE [TodoDB2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TodoDB2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TodoDB2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TodoDB2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TodoDB2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TodoDB2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TodoDB2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TodoDB2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TodoDB2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TodoDB2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TodoDB2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TodoDB2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TodoDB2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TodoDB2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TodoDB2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TodoDB2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TodoDB2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TodoDB2] SET RECOVERY FULL 
GO
ALTER DATABASE [TodoDB2] SET  MULTI_USER 
GO
ALTER DATABASE [TodoDB2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TodoDB2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TodoDB2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TodoDB2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TodoDB2] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TodoDB2', N'ON'
GO
USE [TodoDB2]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 5/19/2025 2:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[user_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Priority]    Script Date: 5/19/2025 2:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Priority](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[color] [varchar](50) NOT NULL,
	[user_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 5/19/2025 2:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[color] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Steps]    Script Date: 5/19/2025 2:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Steps](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[task_id] [int] NULL,
	[step_index] [int] NULL,
	[step] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 5/19/2025 2:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](250) NOT NULL,
	[description] [text] NOT NULL,
	[picture] [varchar](200) NULL,
	[category] [int] NULL,
	[priority] [int] NULL,
	[status] [int] NULL,
	[due_date] [date] NULL,
	[created_at] [datetime] NULL,
	[completed_at] [datetime] NULL,
	[user_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 5/19/2025 2:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [nvarchar](100) NULL,
	[last_name] [nvarchar](100) NULL,
	[email] [nvarchar](100) NULL,
	[password] [nvarchar](100) NULL,
	[avatar] [nvarchar](100) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--SET IDENTITY_INSERT [dbo].[Category] ON 
--
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (17, N'Privacy', 1)
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (18, N'Personel', 1)
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (19, N'Work', 1)
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (20, N'Personel', 2)
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (21, N'Privacy', 2)
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (22, N'personal', 5)
--INSERT [dbo].[Category] ([id], [name], [user_id]) VALUES (23, N'work', 5)
--SET IDENTITY_INSERT [dbo].[Category] OFF
--GO
--SET IDENTITY_INSERT [dbo].[Priority] ON 
--
--INSERT [dbo].[Priority] ([id], [name], [color], [user_id]) VALUES (3, N'Low', N'Green', 1)
--INSERT [dbo].[Priority] ([id], [name], [color], [user_id]) VALUES (10, N'Moderate', N'#FF0000FF', 1)
--INSERT [dbo].[Priority] ([id], [name], [color], [user_id]) VALUES (11, N'Extreme', N'#FFFF0000', 1)
--INSERT [dbo].[Priority] ([id], [name], [color], [user_id]) VALUES (13, N'important', N'#FF0000FF', 5)
--INSERT [dbo].[Priority] ([id], [name], [color], [user_id]) VALUES (14, N'normal', N'#FF00FFFF', 5)
--SET IDENTITY_INSERT [dbo].[Priority] OFF
--GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([id], [name], [color]) VALUES (1, N'Not Started', N'Red')
INSERT [dbo].[Status] ([id], [name], [color]) VALUES (2, N'In Progress', N'Blue')
INSERT [dbo].[Status] ([id], [name], [color]) VALUES (3, N'Completed', N'Green')
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users_Email]    Script Date: 5/19/2025 2:08:51 AM ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Priority]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Steps]  WITH CHECK ADD FOREIGN KEY([task_id])
REFERENCES [dbo].[Task] ([id])
GO
ALTER TABLE [dbo].[Steps]  WITH CHECK ADD FOREIGN KEY([task_id])
REFERENCES [dbo].[Task] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK__Task__priority__1BFD2C07] FOREIGN KEY([priority])
REFERENCES [dbo].[Priority] ([id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK__Task__priority__1BFD2C07]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([priority])
REFERENCES [dbo].[Priority] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([priority])
REFERENCES [dbo].[Priority] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([priority])
REFERENCES [dbo].[Priority] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([priority])
REFERENCES [dbo].[Priority] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([id])
GO
USE [master]
GO
ALTER DATABASE [TodoDB2] SET  READ_WRITE 
GO
