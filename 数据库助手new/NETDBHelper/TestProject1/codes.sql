----------存储过程:usp_SchoolLevel_FullRefresh------------
Create Proc usp_SchoolLevel_FullRefresh
AS
--更新教育经历的学校级别
update edu
SET edu.SchoolLevel = sl.SchoolLevels
 from TB_Education edu
join [SchoollLevels] sl 
on edu.School = sl.Name or (edu.School = sl.NameEN and len(RTRIM(LTRIM(sl.NameEN)))>1)

--更新候选人的学校级别

select Name,MIN(Level)as hxrLevel 
INTO #level
from (
select  Name,NameEN,Level
from (select NAme,NameEN,L985,L211,L1B,L2B,L3B from [SchoollLevels]) p
unpivot
(
   Level for Levels in (L985,L211,L1B,L2B,L3B)
)as unpvot
) a
where a.Level <> 0
group by Name

truncate table TB_HxrSchoolLevel

insert into TB_HxrSchoolLevel(sourceId,sourceType,SchoolLevel)
select SourceId,SourceType,min(#level.hxrLevel) 
from TB_Education edu
join
#level
on 
edu.School = #level.Name
group by SourceId,SourceType

