
DECLARE @doc_type INT;
SET @doc_type = 229;

SELECT
k325.keyvaluechar AS 'docketNum', 
(select itemtypename from [ONBASE].[hsi].[itemtype] where itemtypenum = i.itemtypenum) as doc_type,
(select keyvaluedate  from hsi.keyitem167 where itemnum = i.itemnum) as date_filed,
i.itemnum as docid,
i.*

FROM hsi.itemdata i

LEFT JOIN hsi.keyitem325 k325 ON k325.itemnum = i.itemnum
 
WHERE   k325.itemnum IS NOT NULL  

// GROUP BY DOC_TYPE, inlcude how many there are of each one - So what is docID

/*
table: hsi.itemdata
itemdata table: hsi.itemdata
keyword table: hsi.keyitem325
Joinfieldkey: k325.itemnum
DocTypeField: his.itemdata.itemtypenum 
DocType: 119 (or whatever the doc type is there) 
DocTypeFilter: Boolean (on/off)
Field1- "docketnumber" : k325.keyvaluechar
Field2- "Doc Type": SubQuery:  (select itemtypename from [ONBASE].[hsi].[itemtype] where itemtypenum = i.itemtypenum) as doc_type,
Field3- : date filed : keyitemnumber for date filed + match using itemnum to document (select keyvaluedate  from hsi.keyitem167 where itemnum = 1558
DocId: i. Itemnum

URL: 
*/


