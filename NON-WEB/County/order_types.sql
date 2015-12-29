USE [AlleghenyCounty]
GO

/****** Object:  Table [dbo].[order_types]    Script Date: 12/29/2015 4:17:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[order_types](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderType] [nvarchar](255) NULL,
 CONSTRAINT [PK_tlk_service] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


