|-select AG_Code,sum(CL_Length) Length--,chlog.AG_Code StayByPhone 
into #CallCenter
from CallCenter.fastv3.dbo.CHLog 
where  CL_PreTime>=@todayTime  and CL_PreTime<@enddatetime and isnull( AG_Code,'')<>''
group by chlog.AG_Code
tables:callcenter.fastv3.dbo.chlog,columns:ag_code、length