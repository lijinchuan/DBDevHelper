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