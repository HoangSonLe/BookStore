USE [master]
GO
/****** Object:  Database [MyDB]    Script Date: 11/11/2019 2:29:49 AM ******/
CREATE DATABASE [MyDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\MyDB.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MyDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\MyDB_log.ldf' , SIZE = 1088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MyDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MyDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MyDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MyDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyDB] SET RECOVERY FULL 
GO
ALTER DATABASE [MyDB] SET  MULTI_USER 
GO
ALTER DATABASE [MyDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [MyDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'MyDB', N'ON'
GO
USE [MyDB]
GO
/****** Object:  Table [dbo].[About]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[About](
	[AboutID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[AboutImage] [nvarchar](250) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AboutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Comment]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[CustomerID] [int] NULL,
	[EmployeeID] [int] NULL,
	[Context] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contact]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](250) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](250) NULL,
	[Password] [varchar](250) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Sex] [bit] NULL,
	[Image] [nvarchar](50) NULL,
	[Address] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Phone] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] NOT NULL,
	[DepartmentName] [nvarchar](50) NOT NULL,
	[Information] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Division]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Division](
	[DivisionID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NULL,
	[DepartmentID] [int] NULL,
	[Division] [datetime] NULL,
	[Validity] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[DivisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employee]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](250) NULL,
	[Password] [varchar](250) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Address] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Phone] [nvarchar](250) NULL,
	[BirthDate] [datetime] NULL,
	[Role] [int] NULL,
	[ManagerID] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Feedback](
	[FeedbackID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Email] [varchar](255) NULL,
	[ContextSubject] [nvarchar](255) NULL,
	[ContextMessage] [nvarchar](255) NULL,
	[CreatedDate] [datetime] NULL,
	[Phone] [nvarchar](50) NULL,
	[Reply] [bit] NOT NULL,
	[EmployeeID] [int] NULL,
	[ReplyContext] [nvarchar](50) NULL,
	[ReplyDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[FeedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuID] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](250) NULL,
	[MenuContent] [nvarchar](250) NULL,
	[ParentID] [int] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Price] [int] NULL,
	[Discount] [int] NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[EmployeeID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[PayMethod] [nvarchar](50) NULL,
	[ShipMethod] [nvarchar](50) NULL,
	[ShipCost] [float] NULL,
	[Comment] [nvarchar](250) NULL,
	[OrderStatus] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ShipDate] [datetime] NULL,
	[Total] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permission]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentID] [int] NULL,
	[WebPageID] [int] NULL,
	[AddPermission] [bit] NOT NULL,
	[UpdatePermission] [bit] NOT NULL,
	[DeletePermission] [bit] NOT NULL,
	[SeePermission] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) NULL,
	[Unit] [nvarchar](50) NULL,
	[UrlFriendly] [varchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [int] NULL,
	[PromotionPrice] [int] NULL,
	[IncludeVAT] [bit] NULL,
	[Quantity] [int] NULL,
	[CategoryID] [int] NULL,
	[PublisherID] [int] NULL,
	[Discount] [int] NULL,
	[ViewCounts] [int] NULL,
	[Status] [bit] NULL DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[UrlFriendly] [nvarchar](250) NULL,
	[ParentID] [int] NULL,
	[Status] [bit] NULL DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductImages](
	[ProductImagesID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductImage] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductImagesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductLike]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductLike](
	[ProductLikeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[CustomerID] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductLikeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Publishers]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Publishers](
	[PublisherID] [int] IDENTITY(1,1) NOT NULL,
	[PublisherName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Logo] [nvarchar](150) NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[PublisherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] NOT NULL,
	[RoleName] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Slider]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slider](
	[SlideID] [int] IDENTITY(1,1) NOT NULL,
	[SliderImage] [nvarchar](250) NULL,
	[Display] [int] NULL,
	[Description] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[SlideID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WebPage]    Script Date: 11/11/2019 2:29:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebPage](
	[WebPageID] [int] IDENTITY(1,1) NOT NULL,
	[WebPageName] [nvarchar](50) NOT NULL,
	[WebPageURL] [nvarchar](250) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WebPageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([EmployeeID], [UserName], [Password], [FirstName], [LastName], [Address], [Email], [Phone], [BirthDate], [Role], [ManagerID], [CreatedDate]) VALUES (1, N'sonlh', N'b3003135720325e541923035f440ea96', N'Lê Hoàng', N'Sơn', N'Trương Đinh Hội', N'hoangsonle@gmail.com', N'0369006910', CAST(N'1998-02-11 00:00:00.000' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[Employee] ([EmployeeID], [UserName], [Password], [FirstName], [LastName], [Address], [Email], [Phone], [BirthDate], [Role], [ManagerID], [CreatedDate]) VALUES (2, N'minhlc', N'a672a1f9abc161d1acbdeeab9b91eff7', N'Lý Cẩm', N'Minh', N'An Dương Vương', N'minhlc@gmail.com', N'0123456789', CAST(N'1998-04-03 00:00:00.000' AS DateTime), 2, 1, NULL)
SET IDENTITY_INSERT [dbo].[Employee] OFF
SET IDENTITY_INSERT [dbo].[Feedback] ON 

INSERT [dbo].[Feedback] ([FeedbackID], [Name], [Email], [ContextSubject], [ContextMessage], [CreatedDate], [Phone], [Reply], [EmployeeID], [ReplyContext], [ReplyDate]) VALUES (1, N'cdscd', N'phubui2702@gmail.com', N'dscsdcdsdcs', N'scdscsd', CAST(N'2019-11-10 23:46:22.653' AS DateTime), N'0369006910', 1, NULL, NULL, NULL)
INSERT [dbo].[Feedback] ([FeedbackID], [Name], [Email], [ContextSubject], [ContextMessage], [CreatedDate], [Phone], [Reply], [EmployeeID], [ReplyContext], [ReplyDate]) VALUES (2, N'sonle211', N'hoangsonle211@gmail.com', N'vfvf', N'vfvfdv', CAST(N'2019-11-10 23:48:31.337' AS DateTime), N'0369006910', 0, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Feedback] OFF
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (1, N'NHÀ GIẢ KIM', N'Quyển', N'nha-gia-kim', N'NHÀ GIẢ KIM là cuốn sách được in nhiều nhất chỉ sau Kinh Thánh. Cuốn sách của Paulo Coelho có sự hấp dẫn ra sao khiến độc giả không chỉ các xứ dùng ngôn ngữ Bồ Đào Nha mà các ngôn ngữ khác say mê như vậy?

Tiểu thuyết NHÀ GIẢ KIM của Paulo Coelho như một câu chuyện cổ tích giản dị, nhân ái, giàu chất thơ, thấm đẫm những minh triết huyền bí của phương Đông. Trong lần xuất bản đầu tiên tại Brazil vào năm 1988, sách chỉ bán được 900 bản. Nhưng, với số phận đặc biệt của cuốn sách dành cho toàn nhân loại, vượt ra ngoài biên giới quốc gia, Nhà giả kim đã làm rung động hàng triệu tâm hồn, trở thành một trong những cuốn sách bán chạy nhất mọi thời đại, và có thể làm thay đổi cuộc đời người đọc.

“Nhưng nhà luyện kim đan không quan tâm mấy đến những điều ấy. Ông đã từng thấy nhiều người đến rồi đi, trong khi ốc đảo và sa mạc vẫn là ốc đảo và sa mạc. Ông đã thấy vua chúa và kẻ ăn xin đi qua biển cát này, cái biển cát thường xuyên thay hình đổi dạng vì gió thổi nhưng vẫn mãi mãi là biển cát mà ông đã biết từ thuở nhỏ. Tuy vậy, tự đáy lòng mình, ông không thể không cảm thấy vui trước hạnh phúc của mỗi người lữ khách, sau bao ngày chỉ có cát vàng với trời xanh nay được thấy chà là xanh tươi hiện ra trước mắt. ‘Có thể Thượng đế tạo ra sa mạc chỉ để cho con người biết quý trọng cây chà là,’ ông nghĩ.” - Trích NHÀ GIẢ KIM', 69000, 62100, 1, 100, 9, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (2, N'HAI SỐ PHẬN', N'Quyển', N'hai-so-phan', N'“Hai số phận” không chỉ đơn thuần là một cuốn tiểu thuyết, đây có thể xem là "thánh kinh" cho những người đọc và suy ngẫm, những ai không dễ dãi, không chấp nhận lối mòn.
“Hai số phận” làm rung động mọi trái tim quả cảm, nó có thể làm thay đổi cả cuộc đời bạn. Đọc cuốn sách này, bạn sẽ bị chi phối bởi cá tính của hai nhân vật chính, hoặc bạn là Kane, hoặc sẽ là Abel, không thể nào nhầm lẫn. Và điều đó sẽ khiến bạn thấy được chính mình.
Khi bạn yêu thích tác phẩm này, người ta sẽ nhìn ra cá tính và tâm hồn thú vị của bạn!
“Nếu có giải thưởng Nobel về khả năng kể chuyện, giải thưởng đó chắc chắn sẽ thuộc về Archer.”
 - Daily Telegraph-
(“Hai số phận” (Kane & Abel) là câu chuyện về hai người đàn ông đi tìm vinh quang. William Kane là con một triệu phú nổi tiếng trên đất Mỹ, lớn lên trong nhung lụa của thế giới thượng lưu. Wladek Koskiewicz là đứa trẻ không rõ xuất thân, được gia đình người bẫy thú nhặt về nuôi. Một người được ấn định để trở thành chủ ngân hàng khi lớn lên, người kia nhập cư vào Mỹ cùng đoàn người nghèo khổ. 
Trải qua hơn sáu mươi năm với biết bao biến động, hai con người giàu hoài bão miệt mài xây dựng vận mệnh của mình . “Hai số phận” nói về nỗi khát khao cháy bỏng, những nghĩa cử, những mối thâm thù, từng bước đường phiêu lưu, hiện thực thế giới và những góc khuất... mê hoặc người đọc bằng ngôn từ cô đọng, hai mạch truyện song song được xây dựng tinh tế từ những chi tiết nhỏ nhất.)', 175000, 157500, 1, 100, 9, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (3, N'KÍNH SỢ VÀ RUN RẨY', N'Quyển', N'kinh-so-va-run-ray', N'Từ câu chuyện trong Kinh Thánh Cựu Ước mô tả việc Tổ phụ Abraham tuân lệnh Thiên Chúa, sẵn sàng sát tế con trai mình là Isaac (thường được ngợi ca như một điển tích về sự trung thành vô điều kiện), triết gia Soren Kierkegaard triển khai ba luận đề truy vấn: liệu có tồn tại một sự đình bỏ có tính mục đích luận đối với luân lý hay không?; liệu có tồn tại một bổn phận tuyệt đối với Thượng đế hay không?; Liệu Abraham có thể biện hộ được về mặt luân lý khi che giấu mục đích của mình không cho Sarah, Eleazar và Isaac biết hay không?

Qua đó, ông đưa ra một phép biện chứng hiện sinh, một cái nhìn giải thiêng xuyên qua chân lý, lòng kính sợ, sự xao xuyến, bổn phận và hành động đức tin…

Với Kính sợ và Run rẩy, Soren Kierkegaard bắc một cây cầu suy tưởng nối văn chương, thần học và triết học. Đây cũng là tác phẩm độc đáo, hay nhất và gây tranh cãi nhất của ông; gây kinh ngạc đối với độc giả phổ thông và cả giới nghiên cứu chuyên sâu.

“Kính sợ và Run rẩy là một trong những tác phẩm nổi tiếng và có nhiều ảnh hưởng nhất trong thần học triết lý, văn chương của thế kỷ 19 và thế kỷ 20” – George Steiner

“Bản dịch này – có lẽ là lần đầu tiên của một tác phẩm quan trọng của Kierkegaard sang tiếng Việt – nhuần nhị, say mê không chỉ trong văn phong mà cả trong sự thâm cảm của dịch giả với tác phẩm, bất chấp bao ngăn cách.” – Bùi Văn Nam Sơn.', 169000, 152100, 1, 100, 9, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (4, N'DẠO BƯỚC PHỐ ĐÊM', N'Quyển', N'dao-buoc-pho-dem', N'Dạo Bước Phố Đêm

Một cậu con trai đặc biệt để ý một cô gái có mái tóc đen. Cậu đã lặng lẽ đeo đuổi cô từ một đêm ở Ponto-cho, đến hội chợ sách cũ tại đền Shimogamo, rồi đến cả lễ hội trường.

Dù họ học cùng một trường đại học, là bạn bè khóa trên khóa dưới, nhưng cô gái không bao giờ nhận ra cảm xúc của cậu.

Dù hai người họ “ngẫu nhiên gặp gỡ” bao nhiêu lần, cô vẫn chỉ nói, “Tình cờ quá anh nhỉ!”

Hàng loạt sự kiện hiếm khi xảy ra đã xảy ra, bao nhiêu con người kì lạ đầy cá tính đã tụ tập lại, giúp cảm xúc hai phía tìm được đến điểm hẹn chung.

Đây là một tác phẩm văn học đáng yêu thuộc thể loại huyền ảo và pop romance. Cũng là một tác phẩm xuất sắc đã đoạt giải Yamamoto Shuguro và xếp vị thứ 2 trong giải Honya.', 98000, 88200, 1, 100, 9, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (5, N'CHÚNG TA LÀ NHỮNG ĐỨA TRẺ CÔ ĐƠN', N'Quyển', N'chung-ta-la-nhung-dua-tre', N'Bạn đã bao giờ trở về nhà sau một ngày làm việc dài, mệt mỏi, áp lực, nằm dài trên ghế với bộ đồ công sở, bật thật lớn một bản nhạc deep house, át hết tất cả những âm thanh xung quanh và tự huyễn hoặc với chính mình rằng “Mình sẽ ổn thôi”?
Bạn đã có một ngày như thế chưa?

Khi còn bé, chúng ta mong đến ngày làm người lớn, để rồi những ngày đã khôn lớn, chúng ta lại ước ao có một giây phút được trở về như những ngày thơ bé. Chính những người trẻ như chúng ta lại cứ ao ước, khát khao được tự do trong những tháng ngày tuổi trẻ của mình. Chúng ta cứ bắt bản thân sống o ép, phải xoay theo quy luật của xã hội, phải chiều theo miệng người đời.', 88000, 79200, 1, 100, 10, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (6, N'ĐÀN BÀ, CỨ YÊU ĐI, ĐỪNG NGẠI!', N'Quyển', N'dan-ba-cu-yeu-di-dung-ngai', N'Đàn Bà, Cứ Yêu Đi, Đừng Ngại!

Tình vỡ rồi thì đi tìm tình mới. Người lạc lối rồi thì dứt áo chào người thôi.

Đàn bà kiên cường thì đừng mong cuộc đời không gặp phải sóng gió. Khi gặp sóng gió, hãy mong đủ bản lĩnh sức lực vượt qua.

Đàn bà kiên cường không phải cứ dốc hết lòng yêu một người, để rồi cho rằng không bao giờ yêu thêm được một ai như vậy nữa.

Mà đàn bà kiên cường, chính là đàn bà hiểu một điều. Trong cuộc đời chúng ta được phép thử, nếu như thử mà sai, thì được phép thử lại. Và mọi thứ vĩnh viễn chỉ là phép thử, là sự chọn lựa cho đến khi gặp được đúng người. Đau lòng hay tổn thương cũng chỉ là một loại cảm xúc nhất thời trong một vài phép thử.

Vậy nên, đàn bà cứ yêu đi, đừng ngại!', 99000, 89100, 1, 100, 10, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (7, N'SẼ CÓ CÁCH ĐỪNG LO', N'Quyển', N'se-co-cach-dung-lo', N'Cuộc sống là muôn vạn những chữ "Ngờ", chúng ta không học được chữ "Ngờ", càng không thể đoán biết trước được nó sẽ đến lúc nào. Nhưng chúng ta hoàn toàn có thể học cách đón nhận nó, một cách tích cực và thanh thản nhất có thể...

Có người sẽ vì những đắng cay ngang trái ở đời mà gục ngã, mất hết niềm tin sống lẫn nhuệ khí sinh tồn. Nhưng cũng có người càng bất hạnh, càng nghịch cảnh thì động lực vươn lên trong họ lại càng lớn. Họ nén hết cay đắng xuống, tạo thành một lực đẩy để bật ra khỏi vũng lầy dưới chân mình. 

Mọi thứ đều sẽ có cách giải quyết, nút thắt nào cũng sẽ có cách để mở, người cần đến sẽ đến và người cần đi sẽ buộc phải ra đi. Sẽ có cách, đừng lo!', 69000, 62100, 1, 100, 10, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (8, N'CÀ PHÊ CÙNG TONY', N'Quyển', N'ca-phe-cung-tony', N'Có đôi khi vào những tháng năm bắt đầu vào đời, giữa vô vàn ngả rẽ và lời khuyên, khi rất nhiều dự định mà thiếu đôi phần định hướng, thì CẢM HỨNG là điều quan trọng để bạn trẻ bắt đầu bước chân đầu tiên trên con đường theo đuổi giấc mơ của mình. Cà Phê Cùng Tony là tập hợp những bài viết của tác giả Tony Buổi Sáng. Đúng như tên gọi, mỗi bài nhẹ nhàng như một tách cà phê, mà bạn trẻ có thể nhận ra một chút gì của chính mình hay bạn bè mình trong đó: Từ chuyện lớn như định vị bản thân giữa bạn bè quốc tế, cho đến chuyện nhỏ như nên chú ý những phép tắc xã giao thông thường.

Chúng tôi tin rằng những người trẻ tuổi luôn mang trong mình khát khao vươn lên và tấm lòng hướng thiện, và có nhiều cách để động viên họ biến điều đó thành hành động. Nếu như tập sách nhỏ này có thể khơi gợi trong lòng bạn đọc trẻ một cảm hứng tốt đẹp, như tách cà phê thơm vào đầu ngày nắng mới, thì đó là niềm vui lớn của tác giả Tony Buổi Sáng.', 99000, 89100, 1, 100, 10, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (9, N'WORLD TEACHER', N'Quyển', N'world-teacher', N'Một ông già từng được xưng tụng là điệp viên tài giỏi nhất thế giới, sau khi về hưu đã quyết định trở thành một thầy giáo, đào tạo cho những cô cậu trẻ tuổi để nối nghiệp mình. Tuy nhiên, chỉ vài năm sau đó, ông đã bị ám sát ở tuổi sáu mươi trong một nhiệm vụ chống lại thế giới ngầm.

Những tưởng rằng đã chết, nhưng khi tỉnh dậy thì lại thấy bản thân được đầu thai ở một thế giới khác, nơi mà phép thuật và những sinh vật huyền bí cùng tồn tại. Bởi kí ức của kiếp trước vẫn được giữ lại, lão già – hiện giờ là một đứa trẻ sơ sinh nhanh chóng nắm bắt tình hình, và bắt đầu một sự khổ luyện ngay từ khi còn bé.
Sau những chăm chỉ rèn luyện, cậu bé đã đạt được loại phép thuật đặc biệt, cùng với một nguồn sức mạnh to lớn. Cậu quyết định sẽ tiếp tục sự nghiệp giáo viên còn dang dở ở kiếp trước.', 129000, 116100, 1, 100, 11, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (10, N'SWORD ART ONLINE', N'Quyển', N'sworld-art-online v ', N' “Nở ra, hoa hồng!”

Kirito và Eugeo cùng lên thẳng tầng cao nhất của tòa tháp trắng Central Cathedral, biểu tượng của Giáo hội Công lý, cũng là nơi ở của tư tế tối cao Administrator.

Một lần nữa, họ đối mặt với Hiệp sĩ Chỉnh hợp Alice (Kim Mộc Tê). Thuật chi phối vũ trang toàn diện của Kirito và Alice cùng bùng nổ, đục vỡ tường ngoài tòa tháp, làm hai người văng ra khỏi Cathedral.

Dù bị chia tách với Kirito, Eugeo vẫn tin tưởng rằng bạn mình sẽ xoay xở sống được. Và tiếp tục lên các tầng trên một mình.

Kẻ địch xuất hiện trước mặt cậu là Hiệp sĩ Chỉnh hợp lớn tuổi nhất, hùng mạnh nhất: Bercouli Synthesis One.

Đối diện người anh hùng trong câu chuyện cổ tích ngày thơ bé, Eugeo tuốt mạnh thanh kiếm Aobara.

Trận chiến hạ màn bằng sự ra đời của một kiếm sĩ mới.

Không còn Kirito ở bên, Eugeo khoác vào người bộ giáp của Hiệp sĩ Chỉnh hợp.

Trong đôi mắt cậu, lóe lên ánh sáng lạnh băng.', 125000, 112500, 1, 100, 11, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (11, N'NGƯỜI THẮP SAO TRỜI', N'Quyển', N'nguoi-thap-sao-troi', N'Bất kể gặp phải ai, đó đều là người chắc chắn phải gặp.
Dù xảy ra chuyện gì, đó đều là chuyện chắc chắn xảy ra.
Tất cả phát sinh vào thời điểm nào, đó đều là thời điểm không thể thay đổi.
Bởi vậy đừng ao ước được làm lại, đừng mất công tiếc nuối: Giá như…', 60000, 54000, 1, 100, 12, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (12, N'NGHĨ GIÀU & LÀM GIÀU', N'Quyển', N'nghi-giau-lam-giau', N'Think and Grow Rich - Nghĩ giàu và Làm giàu là một trong những cuốn sách bán chạy nhất mọi thời đại. Đã hơn 60 triệu bản được phát hành với gần trăm ngôn ngữ trên toàn thế giới và được công nhận là cuốn sách tạo ra nhiều triệu phú hơn, một cuốn sách truyền cảm hứng thành công nhiều hơn bất cứ cuốn sách kinh doanh nào trong lịch sử. Tác phẩm này đã giúp tác giả của nó, Napoleon Hill, được tôn vinh bằng danh hiệu “người tạo ra những nhà triệu phú”. Đây cũng là cuốn sách hiếm hoi được đứng trong top của rất nhiều bình chọn theo nhiều tiêu chí khác nhau - bình chọn của độc giả, của giới chuyên môn, của báo chí. Lý do để Think and Grow Rich - Nghĩ giàu và Làm giàu có được vinh quang này thật hiển nhiên và dễ hiểu: Bằng việc đọc và áp dụng những phương pháp đơn giản, cô đọng này vào đời sống của mỗi cá nhân mà đã có hàng ngàn người trên thế giới trở thành triệu phú và thành công bền vững.', 110000, 99000, 1, 100, 13, 5, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (13, N'BÍ MẬT TƯ DUY TRIỆU PHÚ', N'Quyển', N'bi-mat-tu-duy-trieu-phu', N'Trong cuốn sách này T. Harv Eker sẽ tiết lộ những bí mật tại sao một số người lại đạt được những thành công vượt bậc, được số phận ban cho cuộc sống sung túc, giàu có, trong khi một số người khác phải chật vật, vất vả mới có một cuộc sống qua ngày. Bạn sẽ hiểu được nguồn gốc sự thật và những yếu tố quyết định thành công, thất bại để rồi áp dụng, thay đổi cách suy nghĩ, lên kế hoạch rồi tìm ra cách làm việc, đầu tư, sử dụng nguồn tài chính của bạn theo hướng hiệu quả nhất.', 110000, 99000, 1, 100, 14, 5, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (14, N'CHỐT SALE (CLOSING)', N'Quyển', N'chot-sale', N'“Chốt Sales” chia sẻ những hiểu biết sâu sắc về nghề bán hàng với những kỹ năng & các bước quan trọng giúp mọi cá nhân có thể tiến hành việc chốt hợp đồng một cách hiệu quả nhất. Đơn giản mà nói, quyển sách này sẽ hướng dẫn chúng ta cách hoàn thành hợp đồng, tăng doanh số bán hàng đột phá bằng cách “giúp khách hàng thành công”.', 105000, 94500, 1, 100, 15, 5, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (15, N'DOANH NGHIỆP CỦA THẾ KỶ 21', N'Quyển', N'doanh-nghiep-the-ky-21', N'Một quyển sách khác của tác giả bộ sách nổi tiếng Dạy con làm giàu. Trong cuốn sách này, Robert T. Kiyosaki sẽ chỉ ra cho bạn đọc thấy lý do tại sao mình cần phải gây dựng doanh nghiệp riêng của mình và chính xác đó là doanh nghiệp gì. Nhưng đây không phải là việc thay đổi loại hình doanh nghiệp mình đang triển khai mà đó là việc thay đổi chính bản thân. Tác giả còn cho bạn đọc biết cách thức tìm kiếm những gì mình cần để phát triển doanh nghiệp hoàn hảo, nhưng để doanh nghiệp của mình phát triển thì mình cũng sẽ phải phát triển theo.', 85000, 76500, 1, 100, 16, 4, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (16, N'SỐNG ĐƠN GIẢN CHO MÌNH THANH THẢN', N'Quyển', N'song-dong-gian-cho-doi-thanh-than', N'Một quyển sách khác của tác giả bộ sách nổi tiếng Dạy con làm giàu. Trong cuốn sách này, Robert T. Kiyosaki sẽ chỉ ra cho bạn đọc thấy lý do tại sao mình cần phải gây dựng doanh nghiệp riêng của mình và chính xác đó là doanh nghiệp gì. Nhưng đây không phải là việc thay đổi loại hình doanh nghiệp mình đang triển khai mà đó là việc thay đổi chính bản thân. Tác giả còn cho bạn đọc biết cách thức tìm kiếm những gì mình cần để phát triển doanh nghiệp hoàn hảo, nhưng để doanh nghiệp của mình phát triển thì mình cũng sẽ phải phát triển theo.', 59000, 53000, 1, 100, 17, 6, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (17, N'ĐẮC NHÂN TÂM', N'Quyển', N'dac-nhan-tam', N'Đắc Nhân Tâm - Được lòng người, là cuốn sách đưa ra các lời khuyên về cách thức cư xử, ứng xử và giao tiếp với mọi người để đạt được thành công trong cuộc sống. Gần 80 năm kể từ khi ra đời, Đắc Nhân Tâm là cuốn sách gối đầu giường của nhiều thế hệ luôn muốn hoàn thiện chính mình để vươn tới một cuộc sống tốt đẹp và thành công.', 76000, 68000, 1, 100, 18, 5, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (18, N'TÔI TỰ HỌC', N'Quyển', N'toi-tu-hoc', N'Sách “Tôi tự học” của tác giả Nguyễn Duy Cần đề cập đến khái niệm, mục đích của học vấn đối với con người đồng thời nêu lên một số phương pháp học tập đúng đắn và hiệu quả. Tác giả cho rằng giá trị của học vấn nằm ở sự lĩnh hội và mở mang tri thức của con người chứ không đơn thuần thể hiện trên bằng cấp. Trong xã hội ngày nay, không ít người quên đi ý nghĩa đích thực của học vấn, biến việc học của mình thành công cụ để kiếm tiền nhưng thực ra nó chỉ là phương tiện để  đưa con người đến thành công mà thôi. Bởi vì học không phải để lấy bằng mà học còn để “biết mình” và để “đối nhân xử thế”.', 60000, 54000, 1, 100, 19, 4, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (19, N'GIẢI NGỐ CHO CON TRAI', N'Quyển', N'giai-ngo-cho-con-trai', N'Dậy thì là giai đoạn cơ thể thay đổi cả về thể chất và tâm lý để chuyển từ một đứa trẻ thành người trưởng thành. Quá trình này thường bắt đầu xuất hiện ở lứa tuổi 8-13 đối với bé gái và 9-14 đối với bé trai, và thường kết thúc khi cơ thể đã đạt đến chiều cao và dáng vẻ của một người lớn thực sự. Bạn bè cùng tuổi không có nghĩa là sẽ bắt đầu phát triển cùng lúc. Điều đó đôi khi gây khó khăn và lúng túng cho cả người dậy thì trước và người dậy thì sau khi các bạn cùng
học cùng chơi với nhau, nhưng rồi cuối cùng ai cũng sẽ đến lúc dậy thì thôi.', 59000, 53000, 1, 100, 20, 5, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (20, N'CHỜ ĐẾN MẪU GIÁO THÌ ĐÃ MUỘN', N'Quyển', N'cho-den-mau-giao-thi-da-muon', N'Chờ đến mẫu giáo thì đã muộn là cuốn sách bàn về phương pháp giáo dục trẻ trong giai đoạn từ 0 đến 3 tuổi của tác giả Ibuka Masaru, người sáng lập tập đoàn Sony đồng thời là một nhà nghiên cứu giáo dục. Dựa trên những nghiên cứu về sinh lý học của não bộ và di truyền học, ông đã khẳng định sự phát triển về trí tuệ và năng lực của trẻ được quyết định trong giai đoạn từ 0 đến 3 tuổi, giai đoạn này là “thời kỳ thích hợp” để “nuôi dạy một đứa trẻ trở nên ngoan ngoãn, vui vẻ, có trí tuệ thông minh và khỏe mạnh”. Từ đó, ông đưa ra những giải pháp để giúp các bậc cha mẹ tạo ra môi trường để trẻ có thể phát huy hết khả năng của mình.', 69000, 62000, 1, 100, 21, 1, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (21, N'DẠY CON KIỂU NHẬT', N'Quyển', N'day-con-kieu-nhat', N'Mọi đứa trẻ sinh ra đều có thể trở thành thiên tài nếu chúng nhận được sự giáo dục đúng cách từ sớm. Và cha mẹ - những bậc sinh thành, tiếp xúc thường xuyên nhất với trẻ - là người có thể thực hiện việc này tốt hơn cả. Việc giáo dục trẻ cần được bắt đầu ngay từ khi bé chưa được 1 tuổi chứ không phải đợi đến lúc lớn lên. Đó không phải là một nhiệm vụ quá khó khăn bởi vào giai đoạn này, bạn có thể dạy con mình ngay từ chính những hoạt động tiếp xúc khi bạn cưng chiều hay vui đùa với bé.', 79000, 71000, 1, 100, 22, 6, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (22, N'BÍ ẨN CỦA NÃO PHẢI', N'Quyển', N'bi-an-cua-nao-phai', N'Đối với trẻ nhỏ việc giúp bé phát triển khả năng tư duy sáng tạo là điều rất quan trọng, vì vậy cha mẹ cần quan tâm giúp trẻ rèn luyện kích thích hoạt động của não phải để bồi dưỡng cho trẻ khả năng tư duy sáng tạo. Bởi lẽ việc phát triển não phải cho trẻ không phải chỉ phát triển về cảm xúc, về sự sáng tạo mà những kết quả đó sẽ giúp ích rất nhiều cho sự phát triển toàn não bộ. Khi được giáo dục đúng cách, não phải sẽ phát triển và tạo sự liên kết với não trái một cách tốt nhất.', 82000, 73000, 1, 100, 23, 5, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (23, N'PHƯƠNG PHÁP ĂN DẶM BÉ CHỈ HUY', N'Quyển', N'phuong-phap-an-dam-be-chi-huy', N'Nếu bạn đã quen với hình ảnh các bé được mẹ dùng muỗng đút thức ăn nghiền nhuyễn vào miệng, bé nhè ra và mẹ lại vét vào cho đến khi nào bé nuốt thì thôi, thì BLW sẽ là một hình ảnh hoàn toàn khác. Với phương pháp này, sẽ không có chuyện đút muỗng hay nghiền nhuyễn, mà ba mẹ sẽ cung cấp cho bé những thức ăn có hình dạng và kích cỡ phù hợp để bé có thể cầm lấy và tự đút cho mình bằng các ngón tay, tự chọn thức ăn, tự quyết định ăn bao nhiêu và ăn với tốc độ như thế nào. Rất có thể nhiều mẹ sẽ lo lắng bé ăn như thế nhỡ bị hóc thì sao.', 69000, 62000, 1, 100, 24, 6, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (24, N'NARUTO', N'Quyển', N'naruto', N'Naruto là một trong những series truyện tranh nổi tiếng nhất Nhật Bản. Cùng với Dragon Ball và One Piece, Bleach…, tác phẩm đã sớm vươn ra ngoài thế giới và trở thành một trong những “tượng đài” trong lòng độc giả.', 22000, 19000, 1, 100, 25, 2, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (25, N'MƯỜI VẠN CÂU HỎI VÌ SAO?', N'Quyển', N'muoi-van-vi-sao', N'Tuổi thơ là khoảng thời gian đẹp nhất trong cuộc đời mỗi con người. Ở lứa tuổi này, trẻ luôn tràn trề hy vọng, cùng những ngây thơ trong sáng buổi ban đầu. Đứng trước thế giới với bao điều kỳ diệu mang trong mình sự tò mò, khát vọng tìm hiểu, câu nói thường thấy nhất ở trẻ là "Vì sao?" Để có thể trả lời chính xác câu hỏi của trẻ, không phải là việc đơn giản. Các nghiên cứu cho thấy, sự phát triển ở bộ não trẻ diễn ra nhanh nhất vào tuổi 13 trở về trước, là một phụ huynh khi không mang lại cho trẻ cơ hội suy nghĩ, tìm hiểu, có thể bạn sẽ phải ân hận! Thế giới ngày nay phát triển nhanh chóng, kho tàng kiến thức là vô hạn, luôn được đổi mới với tốc độ chóng mặt. Cũng xuất phát từ những suy nghĩ trên, chúng tôi trên cơ sở thu thập rộng rãi gần một nghìn câu hỏi mà các em nhỏ cảm thấy hứng thú, để đưa ra bộ sách Mười vạn câu hỏi vì sao?, mang lại cho các em những câu trả lời theo từng chủ đề. Cuốn sách với sự đóng góp của các chuyên gia khoa học thường thức giàu kinh nghiệm, sử dụng ngôn ngữ dễ hiểu, kết hợp những hình ảnh minh họa sinh động sẽ mang đến cho các em những kiến thức cơ bản, chứa đựng nội dung phong phú.', 230000, 210000, 1, 100, 26, 2, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (26, N'ĐỂ TỰ TIN HƠN: TỚ YÊU CHÍNH MÌNH', N'Quyển', N'de-tu-tin-hon-toi-yeu-chinh-minh', N'Bé yêu ơi, mỗi ngày bé hãy tập nói “Tớ yêu chính mình!” thật nhiều lần nhé!

Đây là câu thần chú kì diệu giúp bé trở nên tự tin và thấy cuộc sống tươi đẹp hơn đấy!', 30000, 270000, 1, 100, 27, 2, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (27, N'VỪA HỌC VỪA CHƠI 1-4 TUỔI: EQ - RÈN NẾP SỐNG HAY', N'Quyển', N'vua-hoc-vua-choi-1-4-tuoi-eq-ren-luyen-nep-song', N'Các chuyên gia và nền giáo dục hiện đại chú trọng phát triển đồng đều trí thông minh và khả năng hoà đồng của trẻ, hướng đi bằng hai "chân" IQ và EQ lớn lên vững vàng trên đường đời. Đây là bộ truyện được xây dựng dựa trên những tiêu chí phát triển IQ và EQ cho trẻ ngay từ lúc còn nhỏ. Mượn hình ảnh các loài động vật ngộ nghĩnh, dễ thương, nội dung truyện được chọn lọc, phù hợp với tâm sinh lí trẻ trong lứa tuổi 2-6 tuổi, kèm theo những hướng dẫn gợi ý cho các bậc cha mẹ làm sao để kích thích những nhân tố tốt trong trẻ ngày một phát triển hơn.', 8000, 7000, 1, 100, 28, 2, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (28, N'KHI HƠI THỞ HÓA THINH KHÔNG', N'Quyển', N'khi-hoi-tho-hoa-thinh-khong', N'Khi hơi thở hóa thinh không” là cuốn hồi kí được viết bởi Paul Kalanithi – một bác sĩ phẫu thuật não và cũng là một bệnh nhân ung thư phổi giai đoạn cuối. Paul viết cuốn sách này trong những tháng cuối cùng của cuộc đời anh – khi mà anh đang đối mặt trực tiếp với cái chết.', 109000, 98000, 1, 100, 29, 6, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (29, N'TRUMP - ĐỪNG BAO GIỜ BỎ CUỘC', N'Quyển', N'trump-dung-bao-gio-bo-cuoc', N'Trump - Đừng bao giờ bỏ cuộc, doanh nhân nổi tiếng nhất thế giới này thẳng thắn giãi bày những thách thức to lớn nhất, những giây phút tệ hại nhất, và những cuộc chiến khốc liệt nhất - và cách ông biến những nghịch cảnh đó thành các thành công mới.', 65000, 59000, 1, 100, 30, 4, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (30, N'SÓI GIÀ PHỐ WALL', N'Quyển', N'soi-gia-pho-wall', N'Sói Già Phố Wall là cuốn tự truyện của Jordan Belfort - một huyền thoại trong ngành môi giới chứng khoán trên sàn phố Wall. Tác phẩm kể về quá trình phất lên của Jordan nói riêng, cũng như nội tình cuộc đại khủng hoảng tín dụng thứ cấp ở Mỹ nói chung.', 165000, 148000, 1, 100, 31, 6, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (31, N'RONG CHƠI', N'Quyển', N'rong-choi', N'Rong chơi ra đời, không phải để trở thành một cuốn tiểu sử hàng trăm trang kể về cuộc đời của Trần Lập. Nó chỉ là một cuốn sách nhỏ ghi lại chuyến đi rong của Yo Le, của mỗi chúng ta trong thế giới của Trần Lập - một thế giới ngập tràn những thú chơi: chơi với âm nhạc, chơi với mô tô, chơi với những cung đường và cả với những biến cố chẳng thể ngờ tới!', 99000, 89000, 1, 100, 32, 6, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (32, N'TIẾNG VIỆT 1', N'Quyển', N'tieng-viet-1', N'Sách tiếng Việt lớp 1', 14000, 12000, 1, 100, 33, 3, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (33, N'SINH HỌC 6', N'Quyển', N'sinh-hoc-6', N'Sách sinh học lớp 6', 15000, 13000, 1, 100, 34, 3, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (34, N'SINH HỌC 10', N'Quyển', N'sinh-hoc-10', N'Sách sinh học lớp 10', 11000, 10000, 1, 100, 35, 3, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (35, N'CÔNG PHÁ TOÁN 3', N'Quyển', N'cong-pha-toan-3', N'Công Phá Toán 3 giúp các em học sinh nắm chắc tư duy giải nhanh các dạng toán 12 và thâu tóm toàn bộ bài tập chọn lọc bài tập từ 200 đề thi thử mới nhất. Tác giả là sinh viên có những thành tích cao trong học tập và cũng chính nhiệt huyết của tuổi trẻ đã mang đến cho bạn đọc những kinh nghiệm của bản thân, chia sẻ tri thức, phần nào giúp các em học sinh hiện nay tự tin hơn khi luyện thi môn Toán.', 250000, 225000, 1, 100, 36, 3, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (36, N'NGỮ PHÁP TIẾNG ANH', N'Quyển', N'ngu-phap-tieng-anh', N'Cuốn NGỮ PHÁP TIẾNG ANH tổng hợp các chủ điểm ngữ pháp trọng yếu mà các em học sinh cần nắm vững. Các chủ điểm ngữ pháp được trình bày rõ ràng, chi tiết. Sau mỗi chủ điểm ngữ pháp là phần bài tập nhằm giúp các em củng cố kiến thức đã học.', 70000, 63000, 1, 100, 37, 7, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (37, N'4 TUẦN THI ĐẬU HSK 4', N'Quyển', N'4-tuan-thi-dau-hsk-4', N'4 Tuần Thi Đậu HSK4 (Cấp Độ 4) giúp học viên làm quen và luyện tập với cấu trúc đề và các điểm ngữ pháp cơ bản của kì thi HSK. Nội dung sách bám sát với chương trình thi, giúp cho quá trình học của các bạn sẽ trở nên dễ dàng và hiệu quả hơn.', 139000, 125000, 1, 100, 38, 7, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (38, N'TẬP VIẾT CHỮ HÁN', N'Quyển', N'tap-viet-chu-han', N'Dù bạn học bất kỳ một môn ngoại ngữ nào cũng đều trải qua một quá trình khó khăn và vất vả đòi hỏi người học phải có sự quyết tâm cao trong quá trình học tập, chỉ có sự cố gắng của bản thân mới có hiệu quả và đạt được thành công mong muốn. Tuy nhiên khá nhiều bạn khá băn khoăn và đặt khá nhiều câu hỏi: làm thế nào để học tiếng Trung hiệu quả nhất, cần phải học ở đâu, học tài liệu nào, phải học bao nhiêu thời gian mới có thể giao tiếp được, phương pháp học như thế nào để có hiệu quả…. Tóm lại là ai cũng mong được thành công trong thời gian ngắn nhất với môn ngoại ngữ mình chọn học!', 50000, 45000, 1, 100, 39, 7, 10, 0, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Unit], [UrlFriendly], [Description], [Price], [PromotionPrice], [IncludeVAT], [Quantity], [CategoryID], [PublisherID], [Discount], [ViewCounts], [Status]) VALUES (39, N'TỰ HOC TIẾNG HÀN CẤP TỐC', N'Quyển', N'tu-hoc-tieng-han-cap-toc', N'Học bất kì ngoại ngữ nào người học cũng đều hướng tới mong muốn có thể giao tiếp được thành thạo. Học tiếng Hàn Quốc cũng vậy, người học cũng đều mong muốn có thể giao tiếp tốt, nhất là trong giao tiếp hằng ngày. Nhằm giúp các bạn có tài liệu phục vụ việc học giao tiếp chúng tôi xin giới thiệu tới các bạn cuốn sách “Tự học tiếng Hàn cấp tốc”.', 75000, 67000, 1, 100, 40, 7, 10, 0, 1)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[ProductCategory] ON 

INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (1, N'Văn học', N'van-hoc', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (2, N'Kinh tếc', N'kinh-te', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (3, N'Tâm lý - Kỹ năng sống', N'tam-ly-ky-nang-song', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (4, N'Nuôi dạy con', N'nuoi-day-con', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (5, N'Sách thiếu nhi', N'sach-thieu-nhi', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (6, N'Tiểu sử - Hồi ký', N'tieu-su-hoi-ky', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (7, N'Giáo khoa - Tham khảo', N'giao-khoa-tham-khao', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (8, N'Sách học ngoại ngữ', N'sach-hoc-ngoai-ngu', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (9, N'Tiểu thuyết', N'tieu-thuyet', NULL, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (10, N'Truyen ngắn - Tản văn', N'truyen-ngan-tan-van', 1, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (11, N'Light novel', N'light-novel', 1, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (12, N'Ngôn tình', N'ngon-tinh', 1, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (13, N'Nhân vật - Bài học kinh doanh', N'nhan-vat-bai-hoc-kinh-doanh', 2, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (14, N'Quản trị - Lãnh đạo', N'quan-tri-lanh-dao', 2, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (15, N'Marketing - Bán hàng', N'marketing-ban-hang', 2, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (16, N'Phân tích kinh tế', N'phan-tich-kinh-te', 2, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (17, N'Kỹ năng sống', N'ky-nang-song', 3, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (18, N'Rèn luyện nhân cách', N'ren-luyen-nhan-cach', 3, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (19, N'Tâm lý', N'tam-ly', 3, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (20, N'Sách cho tuổi mới lớn', N'sach-cho-tuoi-moi-lon', 3, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (21, N'Cẩm nang làm cha mẹ', N'cam-nang-lam-cha-me', 4, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (22, N'Phương pháp giáo dục trẻ các nước', N'phuong-phap-giao-duc-tre-cac-nuoc', 4, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (23, N'Phát triển trí tuệ cho trẻ', N'phat-trien-tri-tue-cho-tre', 4, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (24, N'Phát triển kỹ năng cho trẻ', N'phat-trien-ky-nang-cho-tre', 4, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (25, N'Manga - Comic', N'manga-comic', 5, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (26, N'Kiến thức bách khoa', N'kien-thuc-bach-khoa', 5, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (27, N'Sách tran kỹ năng sống cho trẻ', N'sach-tranh-ky-nang-song-cho-tre', 5, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (28, N'Vừa học - Vừa chơi với trẻ', N'vua-hoc-vua-choi-voi-tre', 5, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (29, N'Câu chuyện cuộc đời', N'cau-chuyen-cuoc-doi', 6, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (30, N'Chính trị', N'chinh-tri', 6, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (31, N'Kinh tế', N'kinh-te', 6, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (32, N'Nghệ thuật - Giải trí', N'nghe-thuat-giai-tri', 6, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (33, N'Cấp 1', N'cap-1', 7, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (34, N'Cấp 2', N'cap-2', 7, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (35, N'Cấp 3', N'cap-3', 7, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (36, N'Luyện thi ĐH - CĐ', N'luyen-thi-dh-cd', 7, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (37, N'Tiếng Anh', N'tieng-anh', 8, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (38, N'Tiếng Nhật', N'tieng-nhat', 8, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (39, N'Tiếng Hoa', N'tieng-hoa', 8, 1)
INSERT [dbo].[ProductCategory] ([CategoryID], [Name], [UrlFriendly], [ParentID], [Status]) VALUES (40, N'Tiếng Hàn', N'tieng-han', 8, 1)
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
SET IDENTITY_INSERT [dbo].[Publishers] ON 

INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (1, N'NXB Văn Học', N'18 Nguyễn Trường Tộ, P.Trúc Bạch, Ba Đình, Hà Nội', N'0437161518', N'tonghopvanhoc@vnn.vn', N'nxb-van-hoc.jpg', N'Ra đời trong những ngày tháng khói lửa của cuộc kháng chiến chống Pháp, trưởng thành qua các thời kỳ đấu tranh giải phóng dân tộc và công cuộc xây dựng Tổ quốc XHCN, gần 70 năm qua, NXB Văn học luôn đồng hành cùng những biến động của đất nước, hoà chung nhịp thở của đời sống nhân dân và phong trào văn nghệ cả nước.')
INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (2, N'NXB Kim Đồng', N'55 Quang Trung, Nguyễn Du, Hai Bà Trưng, Hà Nội', N'02439434490', N'info@nxbkimdong.com.vn', N'nxb-kim-dong.jpg', N'Nhà xuất bản Kim Đồng trực thuộc Trung ương Đoàn TNCS Hồ Chí Minh là Nhà xuất bản tổng hợp có chức năng xuất bản sách và văn hóa phẩm phục vụ thiếu nhi và các bậc phụ huynh trong cả nước, quảng bá và giới thiệu văn hóa Việt Nam ra thế giới.
Nhà xuất bản có nhiệm vụ tổ chức bản thảo, biên soạn, biên dịch, xuất bản và phát hành các xuất bản phẩm có nội dung: giáo dục truyền thống dân tộc, giáo dục về tri thức, kiến thức… trên các lĩnh vực văn học, nghệ thuật, khoa học kỹ thuật nhằm cung cấp cho các em thiếu nhi cũng như các bậc phụ huynh các kiến thức cần thiết trong cuộc sống, những tinh hoa của tri thức nhân loại nhằm góp phần giáo dục và hình thành nhân cách thế hệ trẻ.
Đối tượng phục vụ của Nhà xuất bản là các em từ tuổi nhà trẻ mẫu giáo (1 đến 5 tuổi), nhi đồng (6 đến 9 tuổi), thiếu niên (10 đến 15 tuổi) đến các em tuổi mới lớn (16 đến 18 tuổi) và các bậc phụ huynh.')
INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (3, N'NXB Giáo Dục', N'81 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', N'02438220801', N'nxbgiaoduc@gmail.com', N'nxb-giao-duc.jpg', N'Nhà xuất bản Giáo dục (nay là Nhà xuất bản Giáo dục Việt Nam) được thành lập ngày 1/6/1957.  Là một doanh nghiệp Nhà nước trực thuộc Bộ Giáo dục & Đào tạo,  Nhà xuất bản Giáo dục có nhiệm vụ tổ chức biên soạn, biên tập, in và phát hành các loại sách giáo khoa và các sản phẩm giáo dục phục vụ nghiên cứu, giảng dạy, học tập của các ngành học, bậc học trong toàn quốc; đồng thời giúp Bộ Giáo dục & Đào tạo chỉ đạo công tác thiết bị giáo dục và thư viện trường học trên toàn quốc')
INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (4, N'NXB Trẻ', N'161B Lý Chính Thắng, Phường 7, Quận 3 , TP. Hồ Chí Minh', N'02839316289', N'hopthubandoc@nxbtre.com.vn', N'nxb-tre.jpg', N'Năm năm sau ngày đất nước thống nhất, phong trào thanh thiếu nhi thành phố đã có những bước phát triển vượt bậc cả về số lượng và chất lượng, công việc giáo dục thanh thiếu nhi cần thêm nhiều tài liệu thiết thực, bổ ích, phù hợp với yêu cầu phát triển của địa phương trong tình hình mới, được sự quan tâm lãnh đạo và chỉ đạo của Thành ủy, một số cán bộ Đoàn tâm huyết với việc giáo dục thanh thiếu nhi qua các xuất bản phẩm của Thành đoàn được phân công chuẩn bị lực lượng để thành lập một NXB, trước mắt là in sách phục vụ cho phong trào thiếu nhi thành phố. Trên tinh thần đó, ngày 24-3-1981 UBND TP HCM đã ký quyết định thành lập Nhà xuất bản Măng Non trực thuộc Thành đoàn TP.HCM.')
INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (5, N'NXB Tổng hợp TP.HCM', N'62 Nguyễn Thị Minh Khai, phường Đa Kao, quận 1, TPHCM', N'02838256713', N'tonghop@nxbhcm.com.vn', N'nxb-tong-hop.jpg', N'Ra đời trong những ngày tháng khói lửa của cuộc kháng chiến chống Pháp, trưởng thành qua các thời kỳ đấu tranh giải phóng dân tộc và công cuộc xây dựng Tổ quốc XHCN, gần 70 năm qua, NXB Văn học luôn đồng hành cùng những biến động của đất nước, hoà chung nhịp thở của đời sống nhân dân và phong trào văn nghệ cả nước.')
INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (6, N'NXB Lao Động', N'175 Giảng Võ, Q. Đống Đa, Hà Nội', N'0438515380', N'nxblaodong@gmail.com', N'nxb-lao-dong.jpg', N'Tổ chức bản thảo, biên tập và phát hành sách về đường lối của Đảng, chính sách của Nhà nước và chủ trương công tác của Tổng Liên đoàn Lao động Việt Nam; sách phổ biến, giáo dục kiến thức toàn diện cho công nhân, lao động; sách giáo dục truyền thống cho công nhân, lao động và toàn xã hội.')
INSERT [dbo].[Publishers] ([PublisherID], [PublisherName], [Address], [Phone], [Email], [Logo], [Description]) VALUES (7, N'NXB Đại Học Sư Phạm', N'136 Đường Xuân Thuỷ, Quận Cầu Giấy, Thành phố Hà Nội', N'02437547735', N'online@nxbdhsp.edu.vn', N'nxb-dh-su-pham.jpg', N' Nhà xuất bản Đại học sư phạm là tổ chức xuất bản hoạt động trên lĩnh vực tư tưởng, văn hoá thuộc Trường Đại học Sư phạm Hà Nội, hoạt động trong khuôn khổ pháp luật của Nhà nước, theo Luật Xuất bản đã được Chủ tịch nước Cộng hoà Xã hội chủ nghĩa Việt Nam công bố ngày 03/12/2012 theo Nghị quyết số 19/2012/QH13 của Quốc hội khoá XIII.')
SET IDENTITY_INSERT [dbo].[Publishers] OFF
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Super Admin')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Admin')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'Employee')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (4, N'Sale')
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (N'Cash') FOR [PayMethod]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (N'Bike') FOR [ShipMethod]
GO
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_PhanQuyen_Them]  DEFAULT ((0)) FOR [AddPermission]
GO
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_PhanQuyen_Sua]  DEFAULT ((0)) FOR [UpdatePermission]
GO
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_PhanQuyen_Xoa]  DEFAULT ((0)) FOR [DeletePermission]
GO
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_PhanQuyen_Xem]  DEFAULT ((0)) FOR [SeePermission]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Division]  WITH CHECK ADD FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Division]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([Role])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD FOREIGN KEY([ParentID])
REFERENCES [dbo].[Menu] ([MenuID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([WebPageID])
REFERENCES [dbo].[WebPage] ([WebPageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ProductCategory] ([CategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([PublisherID])
REFERENCES [dbo].[Publishers] ([PublisherID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductCategory]  WITH CHECK ADD FOREIGN KEY([ParentID])
REFERENCES [dbo].[ProductCategory] ([CategoryID])
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ProductLike]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductLike]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
USE [master]
GO
ALTER DATABASE [MyDB] SET  READ_WRITE 
GO
