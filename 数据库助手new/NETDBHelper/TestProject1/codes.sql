select ID ,  Case when CHARINDEX('---',child.JobName)>0 then isnull(child.Ext+' > ','') + replace(child.JobName,'---','') else child.JobName end  as TypeName
            from TB_InduJob child