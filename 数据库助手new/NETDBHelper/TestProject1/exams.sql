CREATE TABLE student

(

sno　DECIMAL(5)　NOT NULL,

sname　CHAR(6)　NOT NULL,

sex　CHAR(2)　NOT NULL,

birthday　DATETIME ,

dno　CHAR(3)

)
         
        SELECT  p.P_cid ,  
                p.window ,  
                COUNT(1) AS recommendCount  
        INTO    #tmpRecommendCount  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
        WHERE   c.cFlag >= 2  
                AND c.cFlag <> 3 --window同意推荐        
                AND c.RecommendDate >= @dtInputStartDate  
                AND c.RecommendDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                p.window 
                order by 
                p.ccc asc,
                p.dde desc,
                x.eee

                CREATE TABLE student

(

sno DECIMAL (5) ,

sname CHAR(6) ,

sex CHAR(2) ,

birthday DATETIME ,

dno CHAR(3)

)

ALTER TABLE table_name

DROP COLUMN column_name

INSERT INTO student(sno,sname,sex,dno)

VALUES (12,'王小二','男',5)

SELECT * FROM student

ALTER TABLE table_name
MODIFY column_name varchar(max)

CREATE DATABASE Sales

ON

( NAME = Sales_dat,

FILENAME = 'c:\program files\microsoft sql

server\mssql\data\saledat.mdf',

SIZE = 10,

MAXSIZE = 50,

FILEGROWTH = 5 )

LOG ON

( NAME = 'Sales_log',

FILENAME = 'c:\program files\microsoft sql

server\mssql\data\salelog.ldf',

SIZE = 5MB,

MAXSIZE = 25MB,

FILEGROWTH = 5MB )

ALTER TABLE　student
ALTER COLUMN　　sno CHAR(5)　NOT NULL

DROP　TABLE　Mystudent

CREATE VIEW StudentInfo_View

AS

SELECT * FROM StudentInfo

CREATE VIEW Score_View(sno ,sname, sex, address,dno)

AS

SELECT StudentInfo.*

FROM StudentInfo, RecruitInfo

WHERE StudentInfo. address = RecruitInfo. address

AND RecruitInfo. Score>630

SELECT *

FROM StudentInfo_View

CREATE VIEW BoyScore_View

AS

SELECT * FROM Score_View

WHERE sno IN

(SELECT sno FROM Boys_View)

--case when else end
SELECT　CNAME,CTIME,CREDIT=

CASE

WHEN　CTIME >= 40 THEN 5

WHEN　CTIME >= 30 THEN 4

WHEN　CTIME >= 20 THEN 3

ELSE　2

END

FROM　　COURSE

ORDER BY　　CREDIT

--
EXEC printavg_course '操作系统', @pavg output
--
CREATE VIEW [dbo].[V_KHListWithlxr]
AS
SELECT lxrxxb.LXRXM, lxrxxb.BGDH, lxrxxb.YDDH, lxrxxb.DZYX, lxjlb.LXBH, lxjlb.LXSJ, lxjlb.LXJY, KH.KHBH, KH.KHDM,
	KH.KHMC, KH.SF, KH.ZYLXR, KH.DQ, KH.DH, KH.DZ, KH.GSLXR, KH.Industry, KH.Status, KH.InputDate, KH.InputMan,
	KH.BLKH, KH.BZ, HT.Window, KH.DegreeOfIntention, HT.IsGreen, HT.HtStartDate, HT.HtEndDate, KH.Source,
	KH.ModifyDate, KH.Property, KH.EmployeeNumber, KH.GSLXRApplicant
FROM dbo.TB_KHDAB AS KH (NOLOCK)
LEFT OUTER JOIN(
	SELECT LXRXM, BGDH, YDDH, DZYX, SZQY
    FROM dbo.TB_LXRXXB AS xxb(NOLOCK)
    WHERE   LXRBH =(
		SELECT TOP (1) LXRBH
		FROM dbo.TB_LXRXXB (NOLOCK)
		WHERE SZQY = xxb.SZQY)
) AS lxrxxb
ON lxrxxb.SZQY = KH.KHBH

LEFT OUTER JOIN (
	SELECT LXBH, QYMC, LXSJ, LXJY
    FROM dbo.TB_LXJLB AS jlb(NOLOCK)
    WHERE LXBH = (
		SELECT TOP (1) LXBH
        FROM dbo.TB_LXJLB(NOLOCK)
        WHERE QYMC = jlb.QYMC
        ORDER BY LXBH DESC)
) AS lxjlb
ON lxjlb.QYMC = KH.KHBH

LEFT OUTER JOIN(
	SELECT   Window, KHMC, IsGreen, HtStartDate, HtEndDate, HTXH
    FROM      dbo.TB_HTDAB AS itemHT(NOLOCK)
    WHERE   HTXH =(
		SELECT   TOP (1) HTXH
        FROM      dbo.TB_HTDAB(NOLOCK)
        WHERE   KHMC = itemHT.KHMC
        ORDER BY HTXH desc)
) AS HT ON HT.KHMC = KH.KHBH