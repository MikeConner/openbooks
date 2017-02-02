USE [CityController]
GO

/****** Object:  Table [dbo].[payment_details]    Script Date: 2/2/2017 4:07:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[payment_details](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContractID] [nvarchar](50) NOT NULL,
	[AmountPaid] [decimal](14, 2) NOT NULL,
	[DatePaid] [datetime] NULL,
 CONSTRAINT [PK_paymentDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


