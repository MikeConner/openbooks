/*
   Thursday, January 5, 2017 11:39:51 PM
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
ALTER TABLE dbo.contracts ADD
    AmountReceived decimal(14,2) NULL
GO
ALTER TABLE dbo.contracts SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
