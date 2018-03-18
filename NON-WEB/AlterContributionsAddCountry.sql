USE [CityController]
GO

/****** Object:  Table [dbo].[contributions]    Script Date: 6/3/2015 9:53:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[contributions]
ADD Country nvarchar(2) DEFAULT 'US'
ADD StreetAddress nvarchar(50)
ADD Province nvarchar(50)
GO
