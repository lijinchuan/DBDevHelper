
Create proc [dbo].[usp_Report_SyncNcaReportDailyData_RefreshOldData]
@date datetime
as
BEGIN      

SET NOCOUNT ON;
              
select 姓名 as LoginId,EnglishName,人员编号 as ChineseName,Office,PosTypes,办公电话 as ExtNo into #Nca
from TB_Tresume 
join 
(select distinct LoginId from tb_Nca_Daily_Report) a
on 姓名 = a.LoginId 


Declare @today Date
Declare @enddate Date
Set @today = cast(@date as DATE)
set @enddate = DATEADD(DAY,1,@today)
select  InputMan,can.Id as CandidateId,CurrentJob
into #NewCandidate
from 
TB_Candidate can
join
#Nca nca
on can.InputMan = nca.LoginId
where can.InputDate between @today and @enddate
--Group by can.InputMan

--当天新增候选人
select InputMan,@today as RecordDate,COUNT(1) as NewCandidateCount
into #NewCandidateCount
from #NewCandidate
group by InputMan


--新增被面试

 
select can.InputMan,@today as RecordDate,COUNT(distinct can.id) as InterViewCount into #NewInterview from  TB_Candidate can  join
#Nca nca
on can.InputMan = nca.LoginId inner join  tb_houxuan hou  on can.Id=hou.CandidateId inner join  TB_Interview it
on hou.id=it.HouxuanID where it.CreateTime between @today and @enddate
Group by can.InputMan



--当天推荐成功的
select can.InputMan,@today as RecordDate,COUNT(distinct can.id) as NewRecommendCount
into #NewCandidateRecommendCount
from tb_houxuan hou
join
TB_Candidate can
on hou.CandidateId = can.Id
JOIN #Nca nca
on can.InputMan = nca.LoginId
where cFlag >=2  and 
      cFlag <> 3 AND
      RecommendDate between @today and @enddate
Group by can.InputMan

--当天新增职能匹配
select ncd.InputMan, canhis.CandidateId,MIN(canhis.Id) as HistoryId into #CandidateCreateHistory
from #NewCandidate ncd
join
TB_Candidate_History canhis
on ncd.CandidateId = canhis.CandidateId
group by ncd.InputMan,canhis.CandidateId

select his.InputMan,COUNT(1) as FunctionMatchCandidateCount into #FunctionMatchCandidate
from TB_Candidate_History ncd
join
#CandidateCreateHistory his
on ncd.id = his.HistoryId
join
#Nca nca
on his.InputMan = nca.LoginId and PATINDEX( '%,'+cast(ncd.CurrentJob_New as nvarchar)+',%',','+nca.PosTypes+',') >0
group by his.InputMan

--当天推荐成功的职能匹配的，新增智能被推荐
select ncd.InputMan, canhis.CandidateId,MIN(canhis.Id) as HistoryId into #CandidateHistoryMIN
from TB_Candidate ncd 
join
#Nca nca
on ncd.InputMan = nca.LoginId 
join
TB_Candidate_History canhis
on ncd.Id = canhis.CandidateId
group by ncd.InputMan,canhis.CandidateId

select can.InputMan,@today as RecordDate,COUNT(distinct hou.CandidateId) as NewFunctionRecommendCount into #FunctionMatchRecommendCandidate
from tb_houxuan hou
join
#CandidateHistoryMIN can
on hou.CandidateId = can.CandidateId
join
TB_Candidate_History his
on his.Id = can.HistoryId
JOIN #Nca nca
on can.InputMan = nca.LoginId  AND
   PATINDEX( '%,'+cast(his.CurrentJob_New as nvarchar)+',%',','+nca.PosTypes+',') >0
where cFlag >=2  and 
      cFlag <> 3 AND
      RecommendDate between @today and @enddate
Group by can.InputMan

--当天有效沟通
select InputMan,count(1) as EffectiveCommunicationCount
into #EffectiveComm
from
( 
select rcomm.MemName as InputMan,rcomm.CandidateId as hxid
from TB_CommunicateCommon rcomm
join
#Nca nca
on rcomm.MemName = nca.LoginId and
rcomm.CommunicateTime between @today and @enddate
where len(rcomm.CommunicateContent)>30 and rcomm.CommunicateStatus = 1
)comm
group by InputMan

--呼叫中心的沟通时长，根据分机号查
declare @todayTime datetime,@enddatetime datetime
set @todayTime = @today
set @enddatetime =@enddate
select AG_Code,sum(CL_Length) Length--,chlog.AG_Code StayByPhone 
into #CallCenter
from CallCenter.fastv3.dbo.CHLog 
where  CL_PreTime>=@todayTime  and CL_PreTime<@enddatetime and isnull( AG_Code,'')<>''
group by chlog.AG_Code

select nca.LoginId,cc.Length 
into #CallCenterRecords
from 
#CallCenter cc
join
#Nca nca
on cc.AG_Code = nca.ExtNo


select 
nca.*,ncc.NewCandidateCount,ncrc.NewRecommendCount,fmc.FunctionMatchCandidateCount,itv.InterViewCount,
fmrc.NewFunctionRecommendCount,ec.EffectiveCommunicationCount,ccr.Length as CommunicateDuration
into #ncaCounts
from 
#Nca nca
left join
#NewCandidateCount ncc
on nca.LoginId = ncc.InputMan
left join
#NewInterview itv
on nca.LoginId = itv.InputMan
left join
#NewCandidateRecommendCount ncrc
on nca.LoginId = ncrc.InputMan
left join
#FunctionMatchCandidate fmc
on nca.LoginId = fmc.InputMan
left join
#FunctionMatchRecommendCandidate fmrc
on nca.LoginId = fmrc.InputMan
left join 
#EffectiveComm ec
on nca.LoginId = ec.InputMan
left join 
#CallCenterRecords ccr
on nca.LoginId = ccr.LoginId

  

insert into tb_Nca_Daily_Report(LoginId,EnglishName,ChineseName,Office,PosFunction,NewInteview,NewCandidate,NewCandidateRecommended,NewFunction,NewFunctionRecommended,
PositionHandling,CommunictionDuration,EffectiveCommunication,RecordDate,CreatedDate,UpdatedDate)
select nca.LoginId,nca.EnglishName,nca.ChineseName,nca.Office,nca.PosTypes,InterViewCount,nca.NewCandidateCount,nca.NewRecommendCount,
nca.FunctionMatchCandidateCount,nca.NewFunctionRecommendCount,0,nca.CommunicateDuration,nca.EffectiveCommunicationCount,@today,GETDATE(),GETDATE()
from #ncaCounts nca
where not exists(select 1 from tb_Nca_Daily_Report where LoginId = nca.LoginId and RecordDate = @today)


update dr 
set dr.UpdatedDate = GETDATE(),
    dr.NewCandidate = nca.NewCandidateCount,
    dr.NewCandidateRecommended = nca.NewRecommendCount,
    dr.NewFunction = nca.FunctionMatchCandidateCount,
    dr.NewFunctionRecommended = nca.NewFunctionRecommendCount,
    dr.NewInteview=     nca.InterViewCount,
    dr.EffectiveCommunication = nca.EffectiveCommunicationCount,
    dr.CommunictionDuration = nca.CommunicateDuration
from tb_Nca_Daily_Report dr
join #ncaCounts nca
on dr.LoginId = nca.LoginId and dr.RecordDate = @today

END
