SET IDENTITY_INSERT contracts ON

/* Discovery*/
SELECT * FROM dbo.tlk_department WHERE DeptName LIKE '%Finance%'
SELECT * FROM dbo.tlk_service WHERE ServiceName LIKE '%state%'
SELECT * FROM dbo.tlk_vendors WHERE VendorName LIKE '%various%'

DECLARE @ContractID int
DECLARE @VendorNo nvarchar(10)
DECLARE @VendorName nvarchar(100)
DECLARE @DepartmentID int
DECLARE @SupplementalNo int
DECLARE @ResolutionNo nvarchar(40)
DECLARE @Service int
DECLARE @Amount decimal(14,2)
DECLARE @OriginalAmount decimal(14,2)
DECLARE @Description nvarchar(100)
DECLARE @DateCountersigned datetime
DECLARE @DateDuration datetime



SET @ContractID = 46005;
SET @VendorNo = '0000307655';
SET @VendorName = 'VARIOUS VENDORS - STATE CONTRACTS';
SET @DepartmentID = 107000;
SET @SupplementalNo = 3;
SET @ResolutionNo = 'RS-646-09';
SET @Service = 83; --state
SET @Amount = 0;
SET @OriginalAmount = 0;
SET @Description = '2-way radio communications equipment & accessories';
SET @DateCountersigned = '5/25/10';
SET @DateDuration = '6/30/11';

/* Insert */
INSERT INTO contracts(
	ContractID,VendorNo,VendorName,DepartmentID,
	SupplementalNo,ResolutionNo,Service,Amount,OriginalAmount,
	Description,DateCountersigned,DateDuration,DateEntered
)
VALUES
(
	@ContractID,@VendorNo,@VendorName,@DepartmentID,
	@SupplementalNo,@ResolutionNo,@Service,@Amount,@OriginalAmount,
	@Description,@DateCountersigned,@DateDuration, GETDATE() 
)

/* Verify Insert*/
SELECT * FROM contracts WHERE ContractID = @ContractID