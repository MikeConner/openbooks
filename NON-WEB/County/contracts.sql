USE [AlleghenyCounty]
GO

/****** Object:  Table [dbo].[contracts]    Script Date: 12/29/2015 4:17:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[contracts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContractTypeID] [int] NOT NULL,
	[ContractID] [int] NOT NULL,
	[VendorNo] [int] NOT NULL,
	[VendorName] [nvarchar](100) NULL,
	[DepartmentID] [int] NOT NULL,
	[Amount] [decimal](14, 2) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[CancelDate] [datetime] NOT NULL,
	[ExecutiveAction] [nvarchar](50) NULL,
 CONSTRAINT [PK_contracts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


