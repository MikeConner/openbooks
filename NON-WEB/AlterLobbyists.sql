USE [CityController]
GO

/****** Object:  Table [dbo].[expenditures]    Script Date: 6/3/2015 9:53:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[lobbyists]
ADD ForCity bit DEFAULT 0
GO

