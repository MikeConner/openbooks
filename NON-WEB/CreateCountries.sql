USE [CityController]
GO

/****** Object:  Table [dbo].[tlk_countries]    Script Date: 3/17/2018 3:24:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tlk_countries](
	[country_name] [nvarchar](50) NOT NULL,
	[country_code] [nvarchar](2) NOT NULL
) ON [PRIMARY]
GO


