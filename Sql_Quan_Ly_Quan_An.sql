USE [master]
GO
/****** Object:  Database [QuanLyQuanAn]    Script Date: 14/05/2022 12:03:07 SA ******/
CREATE DATABASE [QuanLyQuanAn]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLyQuanAn', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\QuanLyQuanAn.mdf' , SIZE = 10304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QuanLyQuanAn_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\QuanLyQuanAn_log.ldf' , SIZE = 70976KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [QuanLyQuanAn] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLyQuanAn].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyQuanAn] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLyQuanAn] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLyQuanAn] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QuanLyQuanAn] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLyQuanAn] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLyQuanAn] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLyQuanAn] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [QuanLyQuanAn]
GO
/****** Object:  StoredProcedure [dbo].[acc_login]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[acc_login]
 @userName nvarchar(100),@Pass nvarchar(100)

 as

 begin
	select * from dbo.Account where UserName = @userName AND Pass = @Pass 

 end

GO
/****** Object:  StoredProcedure [dbo].[ChuyenBan]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[ChuyenBan]
@idTable1 int , @idTable2 int

AS BEGIN

	DECLARE @idFBill INT
	DECLARE @idSBill INT

	DECLARE @isFTableEmpty INT = 1
	DECLARE @isSTableEmpty INT = 1


	SELECT @idSBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND stathanhtoan =0
	SELECT @idFBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND stathanhtoan =0

	IF (@idFBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		(
		    DateCheckIn,
		    DateCheckOut,
		    idTable,
		    stathanhtoan,
		    discount
		)
		VALUES
		(   GETDATE(), -- DateCheckIn - date
		    Null, -- DateCheckOut - date
		    @idTable1,         -- idTable - int
		    0,         -- stathanhtoan - int
		    0          -- discount - int
		    )
		SELECT @idFBill = MAX(id) FROM dbo.Bill WHERE idTable =@idTable1 AND stathanhtoan = 0
		
	END
	SELECT @isFTableEmpty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFBill


	IF (@idSBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		(
		    DateCheckIn,
		    DateCheckOut,
		    idTable,
		    stathanhtoan,
		    discount
		)
		VALUES
		(   GETDATE(), -- DateCheckIn - date
		    Null, -- DateCheckOut - date
		    @idTable2,         -- idTable - int
		    0,         -- stathanhtoan - int
		    0          -- discount - int
		    )
		SELECT @idSBill = MAX(id) FROM dbo.Bill WHERE idTable =@idTable2 AND stathanhtoan = 0
		
	END
	SELECT @isSTableEmpty = COUNT(*) FROM dbo.BillInfo WHERE idBill =@idSBill

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSBill
	UPDATE dbo.BillInfo SET idBill = @idSBill WHERE idBill =@idFBill
	UPDATE dbo.BillInfo SET idBill = @idFBill WHERE id IN (SELECT * FROM IDBillInfoTable)
	DROP TABLE IDBillInfoTable

	IF(@isFTableEmpty = 0)
		UPDATE dbo.TableFood SET trangthai=N'Trống' WHERE id = @idTable2

	IF(@isSTableEmpty = 0)
		UPDATE dbo.TableFood SET trangthai=N'Trống' WHERE id = @idTable1

END

GO
/****** Object:  StoredProcedure [dbo].[DeleteBill]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[DeleteBill]
@idTable INT
AS
BEGIN
    DELETE dbo.Bill WHERE idTable =@idTable
END

GO
/****** Object:  StoredProcedure [dbo].[get_listaccount]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[get_listaccount] 
 @userName nvarchar(100)
 AS
 BEGIN
	SELECT * FROM dbo.Account WHERE UserName =@userName
	
 END

GO
/****** Object:  StoredProcedure [dbo].[Get_tablelist]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Get_tablelist]
 AS
 SELECT * FROM dbo.TableFood

GO
/****** Object:  StoredProcedure [dbo].[GetBillDate]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[GetBillDate]
@checkIn date, @checkOut date
AS
BEGIN
    SELECT t.name,b.totalPrice,b.DateCheckIn,b.DateCheckOut,b.discount
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE b.DateCheckIn >= @checkIn AND b.DateCheckOut <=@checkOut AND b.stathanhtoan =1 AND t.id =b.idTable
END

GO
/****** Object:  StoredProcedure [dbo].[GetListBillByDateAndPage]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetListBillByDateAndPage]
@checkIn date, @checkOut date, @page int
AS 
BEGIN
	DECLARE @pageRows INT = 10
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows
	
	;WITH BillShow AS( SELECT b.ID, t.name , b.totalPrice, DateCheckIn, DateCheckOut, discount
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.stathanhtoan = 1
	AND t.id = b.idTable)
	
	SELECT TOP (@selectRows) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRows) id FROM BillShow)
END

GO
/****** Object:  StoredProcedure [dbo].[GetListBillByRP]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[GetListBillByRP]
@checkIn date, @checkOut date, @page int
AS 
BEGIN
	SELECT t.name,b.totalPrice,b.DateCheckIn,b.DateCheckOut,b.discount
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.stathanhtoan = 1
	AND t.id = b.idTable
END

GO
/****** Object:  StoredProcedure [dbo].[GetNumBillByDate]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetNumBillByDate]
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT COUNT(*)
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.stathanhtoan = 1
	AND t.id = b.idTable
END

GO
/****** Object:  StoredProcedure [dbo].[InserBill]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[InserBill]
@idTable int

AS
BEGIN
 INSERT dbo.Bill
 (
     DateCheckIn,
     DateCheckOut,
     idTable,
     stathanhtoan,
	 discount
 )
 VALUES
 (   GETDATE(), -- DateCheckIn - date
     Null, -- DateCheckOut - date
     @idTable,         -- idTable - int
     0,          -- stathanhtoan - int
	 0
     )

END

GO
/****** Object:  StoredProcedure [dbo].[InsertBillInfo]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[InsertBillInfo]

@idBill INT , @idFood INT, @count INT

AS
BEGIN
	DECLARE @isBillInfo INT
    DECLARE @foodcount INT =1

	SELECT @isBillInfo = id,@foodcount = b.soluong
	FROM dbo.BillInfo AS b
	WHERE b.idBill = @idBill AND b.idFood = @idFood

	IF (@isBillInfo > 0)
	BEGIN 
		DECLARE @newcount INT = @foodcount + @count
		IF(@newcount >0)
			UPDATE dbo.BillInfo SET soluong=@foodcount +@count WHERE idFood = @idFood
		ELSE
			DELETE dbo.BillInfo WHERE idBill =@idBill AND idFood =@idFood
	END
	ELSE

	BEGIN

		INSERT dbo.BillInfo
		(
			idBill,
			idFood,
			soluong
		)
		VALUES
		(   @idBill, -- idBill - int
			@idFood, -- idFood - int
			@count  -- soluong - int
			)
	END
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateAccount]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[UpdateAccount]
@username nvarchar(100),@displayname nvarchar(100),@pass nvarchar(100),@newpass nvarchar(100)
AS
BEGIN
	DECLARE @isRpass INT =0
	SELECT @isRpass = COUNT(*) FROM dbo.Account WHERE UserName =@username AND Pass = @pass

	IF(@isRpass =1)

	BEGIN
		IF(@newpass = NULL OR @newpass = '')
		BEGIN
		    
		
	    UPDATE dbo.Account SET displayName=@displayname WHERE UserName=@username
		END
		ELSE	
			UPDATE dbo.Account SET displayName =@displayname,Pass =@newpass WHERE UserName =@username
	END
    
END

GO
/****** Object:  UserDefinedFunction [dbo].[fuConvertToUnsign1]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fuConvertToUnsign1] 
( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) 
SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) 
SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO
/****** Object:  Table [dbo].[Account]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[UserName] [nvarchar](100) NOT NULL,
	[displayName] [nvarchar](100) NOT NULL DEFAULT (N'Duy'),
	[Pass] [nvarchar](1000) NOT NULL DEFAULT ((0)),
	[kieu] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Bill]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DateCheckIn] [date] NOT NULL DEFAULT (getdate()),
	[DateCheckOut] [date] NULL,
	[idTable] [int] NOT NULL,
	[stathanhtoan] [int] NOT NULL,
	[discount] [int] NULL,
	[totalPrice] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillInfo]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idBill] [int] NOT NULL,
	[idFood] [int] NOT NULL,
	[soluong] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[idCategory] [int] NOT NULL,
	[price] [float] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FoodCategory]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TableFood]    Script Date: 14/05/2022 12:03:07 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableFood](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[trangthai] [nvarchar](100) NOT NULL DEFAULT (N'Trống'),
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Account] ([UserName], [displayName], [Pass], [kieu]) VALUES (N'K9', N'Khánh Duy', N'33354741122871651676713774147412831195', 1)
INSERT [dbo].[Account] ([UserName], [displayName], [Pass], [kieu]) VALUES (N'Khánh Duy', N'admin', N'33354741122871651676713774147412831195', 1)
INSERT [dbo].[Account] ([UserName], [displayName], [Pass], [kieu]) VALUES (N'staff', N'staff', N'1962026656160185351301320480154111117132155', 0)
SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (1, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 1, 1, 0, NULL)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (2, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 1, 1, 0, 10000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (3, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 4, 1, 0, NULL)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (4, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 2, 1, 0, NULL)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (5, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 9, 1, 0, 120000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (6, CAST(N'2022-05-12' AS Date), NULL, 1, 0, 0, NULL)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (7, CAST(N'2022-05-12' AS Date), NULL, 4, 0, 0, NULL)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (8, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 6, 1, 22, 93600)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (9, CAST(N'2022-05-12' AS Date), NULL, 7, 0, 0, NULL)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [stathanhtoan], [discount], [totalPrice]) VALUES (10, CAST(N'2022-05-12' AS Date), CAST(N'2022-05-12' AS Date), 3, 1, 50, 60000)
SET IDENTITY_INSERT [dbo].[Bill] OFF
SET IDENTITY_INSERT [dbo].[BillInfo] ON 

INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (1, 4, 3, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (2, 1, 3, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (3, 2, 6, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (4, 3, 3, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (5, 5, 1, 1)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (6, 10, 1, 1)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [soluong]) VALUES (7, 8, 1, 1)
SET IDENTITY_INSERT [dbo].[BillInfo] OFF
SET IDENTITY_INSERT [dbo].[Food] ON 

INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (1, N'Tôm thẻ', 1, 120000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (2, N'Hàu', 1, 10000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (3, N'Vú heo nướng', 2, 100000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (4, N'', 5, 0)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (5, N'Combo rau', 3, 15000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (6, N'Rau muốn', 3, 5000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (7, N'Chả ốc', 4, 15000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (8, N'Chả tôm', 4, 15000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (9, N'Sting', 5, 15000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (10, N'Pepsi', 5, 15000)
SET IDENTITY_INSERT [dbo].[Food] OFF
SET IDENTITY_INSERT [dbo].[FoodCategory] ON 

INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (1, N'Hải sản')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (2, N'Thịt')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (3, N'Rau')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (4, N'Chả')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (5, N'Nước')
SET IDENTITY_INSERT [dbo].[FoodCategory] OFF
SET IDENTITY_INSERT [dbo].[TableFood] ON 

INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (1, N'Bàn 1', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (2, N'Bàn 2', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (3, N'Bàn 3', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (4, N'Bàn 4', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (5, N'Bàn 5', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (6, N'Bàn 6', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (7, N'Bàn 7', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (8, N'Bàn 8', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (9, N'Bàn 9', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (10, N'Bàn 10', N'Trống')
INSERT [dbo].[TableFood] ([id], [name], [trangthai]) VALUES (11, N'Bàn 11', N'Trống')
SET IDENTITY_INSERT [dbo].[TableFood] OFF
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([idTable])
REFERENCES [dbo].[TableFood] ([id])
GO
ALTER TABLE [dbo].[BillInfo]  WITH CHECK ADD FOREIGN KEY([idFood])
REFERENCES [dbo].[Food] ([id])
GO
ALTER TABLE [dbo].[BillInfo]  WITH CHECK ADD FOREIGN KEY([idBill])
REFERENCES [dbo].[Bill] ([id])
GO
ALTER TABLE [dbo].[Food]  WITH CHECK ADD FOREIGN KEY([idCategory])
REFERENCES [dbo].[FoodCategory] ([id])
GO
USE [master]
GO
ALTER DATABASE [QuanLyQuanAn] SET  READ_WRITE 
GO
