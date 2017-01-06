/*
   Friday, January 06, 201710:39:50 AM
   User: 
   Server: PIT-JBENNETT\SQLEXPRESS
   Database: CityController
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.payments
	(
	ContractID nvarchar(50) NOT NULL,
	Fund int NOT NULL,
	CostCenter nvarchar(50) NOT NULL,
	ObjectAccount int NOT NULL,
	LineItem int NULL,
	AmountReceived decimal(14, 2) NOT NULL
	)  ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_payments ON dbo.payments
	(
	ContractID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.payments SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
