select @S=isnull(@S+'+','')+ltrim(rtrim(NAME)) from TB_Industry where ID in (select t.Id  from dbo.SplitToTable(@IndustryIds,',') as  t)
