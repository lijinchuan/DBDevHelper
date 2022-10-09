Create Proc [dbo].[usp_GetResumesRecommendList_vtable]
(
@positionId int
)

AS    
declare  @tmp as table(
id int,
IdNo nvarchar(100),
Degree nvarchar(100),
TrueName nvarchar(100),
Industry nvarchar(200),
CurrentJob int,
NowCity varchar(200),
ObjectCity nvarchar(200),
Companys nvarchar(500),
JobType nvarchar(200)
)
insert into @tmp
SELECT  distinct
            resm.id,resm.JobID as IdNo,resm.Degree,resm.TrueName,ind.Name  as Industry,
            resm.CurrentJob,resm.NowCity,resm.ObjectCity,
            res.Company as Companys,c
            case resm.Industry when '49' then b.Name else a.TypeName end as JobType
        FROM 
        TB_Houxuan hx with(nolock)
        JOIN
        TB_Resume resm with(nolock)
        ON 
        hx.PersonID = resm.id
        LEFT JOIN 
        TB_Industry ind
        on resm.Industry = cast(ind.Id as varchar)
        LEFT JOIN
        TB_Work res
        on resm.id = res.SourceId and res.SourceType = 1 /*1 是简历 */
        LEFT JOIN
        (
            select ID ,  Case when CHARINDEX('---',child.JobName)>0 then isnull(child.Ext+' > ','') + replace(child.JobName,'---','') else child.JobName end  as TypeName 
            from TB_InduJob child
            
        ) a
        on resm.CurrentJob = a.Id and resm.Industry <> '49'
        LEFT JOIN
        (
            select kid.ID ,isnull(parent.Name + '>','') + kid.Name as Name from PositionType kid
            left join
            PositionType parent
            on kid.ParentID = parent.ID
        )b on resm.CurrentJob = b.Id and resm.Industry = '49'
        WHERE   hx.JobID = @positionId  and 
                hx.RecommendBy = 99 and 
                hx.PersonID is not null                           
                
declare @t1 as table(
id int,
IdNo nvarchar(200),
Degree nvarchar(200),
TrueName nvarchar(200),
Industry nvarchar(500),
CurrentJob int,
NowCity varchar(200),
ObjectCity nvarchar(200),
Companys nvarchar(2000),
JobType nvarchar(200)
)
insert into @t1(ID,IdNo,DEGREE,TrueName,Industry,CurrentJob,NowCity,JobType,ObjectCity,Companys)
SELECT ID,IdNo,DEGREE,TrueName,Industry,CurrentJob,NowCity,JobType,ObjectCity,
    STUFF((
    SELECT ', ' + Companys
    FROM @tmp WHERE (ID = Results.ID) rr
    FOR XML PATH(''),TYPE 
    ).value('.','VARCHAR(MAX)') 
    ,1,2,'') as Companys
    from @tmp Results
    group by ID,IdNo,DEGREE,TrueName,Industry,CurrentJob,NowCity,JobType,ObjectCity


SELECT DISTINCT a.*,
case  max(CASE WHEN intv.id is not null then 1 else 0 end) when 1 then '曾经面试过' else '未参加面试' end as InterviewStatus
FROM 
@t1 a
LEFT JOIN 
TB_HOUXUAN intvhx
on a.id = intvhx.PersonId
left join  
TB_INTERVIEW intv
on intvhx.id = intv.HouxuanID and  
   intv.InterviewTurns = '初面' and
   intv.InterviewResult = '初面完成'  
group by a.ID,IdNo,DEGREE,TrueName,Industry,CurrentJob,NowCity,JobType,Companys,ObjectCity


