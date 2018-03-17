USE [CityController]
GO

/****** Object:  Table [dbo].[elected_officials]    Script Date: 3/17/2018 12:53:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[elected_officials](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Office] [nvarchar](50) NULL,
	[Committee] [nvarchar](128) NULL,
	[Salary] [decimal](14, 2) NULL,
	[PersonalPage] [nvarchar](128) NULL,
	[DisclosureLink] [nvarchar](128) NULL,
	[ImageUrl][nvarchar](50) NULL
 CONSTRAINT [PK_elected_officials] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

