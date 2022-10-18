insert into TB_HxrSchoolLevel(sourceId,sourceType,SchoolLevel)
    select SourceId,SourceType,min(#level.hxrLevel) 
    from TB_Education edu
    join
    #level
    on 
    edu.School = #level.Name
    where edu.SourceId = @sourceId and edu.SourceType = @sourceType
    group by SourceId,SourceType