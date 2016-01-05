USE [Allegheny_County_PROD]
GO

/****** Object:  Table [dbo].[tlk_department]    Script Date: 1/5/2016 2:51:57 PM ******/
DROP TABLE [dbo].[tlk_department]
GO

/****** Object:  Table [dbo].[tlk_department]    Script Date: 1/5/2016 2:51:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tlk_department](
	[DeptCode] [int] IDENTITY(1,1) NOT NULL,
	[DeptName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tlk_department] PRIMARY KEY CLUSTERED 
(
	[DeptCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


