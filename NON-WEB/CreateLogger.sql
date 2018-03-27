/****** Object:  Table [dbo].[logger]    Script Date: 3/27/2018 6:24:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[logger](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
	[Result] [nvarchar](255) NULL,
	[Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_logger] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


