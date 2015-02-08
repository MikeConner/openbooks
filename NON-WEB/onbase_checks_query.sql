DECLARE @doc_type INT;
SET @doc_type = 164;
SET NOCOUNT ON;

SELECT

k138.keyvaluechar AS 'contract_no'

FROM hsi.itemdata i
 
LEFT JOIN hsi.keyitem138 k138 ON k138.itemnum = i.itemnum

WHERE i.itemtypenum = @doc_type AND k138.keyvaluechar IS NOT NULL
