USE [CityController]
GO

/****** Object:  Table [dbo].[contributions]    Script Date: 6/3/2015 9:53:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[contributions]
    ADD [Province] NVARCHAR (50) NULL,
        [Country]  NVARCHAR (2)  NULL;
        [StreetAddress] NVARCHAR(50) NULL
		GO
        

        ALTER TABLE [dbo].[contributions]
    ADD DEFAULT 'US' FOR [Country];


GO
 