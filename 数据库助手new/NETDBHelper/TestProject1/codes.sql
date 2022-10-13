----------存储过程:usp_UpdateRecommendSourcingListIndutry_bak20180109------------

Create Proc [dbo].[usp_UpdateRecommendSourcingListIndutry_bak20180109]
/*
在NCA推送中使用，更新没有行业的SoucingList候选人的行业。
*/
AS
--Recommended Sourcing List without Industry
select sl.id into #sourcingList
from dbo.TB_Candidate  sl
join TB_Houxuan hx on sl.Id = hx.CandidateId and JobID <> 1
 where RecommendBy=99 and sl.Industry  is  null  and (sl.CompleteStatus is null  or sl.CompleteStatus<60)

select kh.Industry,a.CandidateId 
into #soucingListIndustry
from 
TB_Houxuan hx join
(
select hx.CandidateId,Max(hx.Id) as hxId 
from TB_Houxuan hx join #sourcingList sl on hx.SourcingListID = sl.id
group by CandidateId
)a on
hx.id = a.hxId
join
TB_PPosition pos 
on hx.JobID = pos.P_id
join
TB_HTDAB ht 
on pos.P_cid = ht.HTXH
join
TB_KHDAB kh
on
ht.KHMC = kh.KHBH

update TB_Candidate
set Industry = #soucingListIndustry.Industry
from #soucingListIndustry
where ID = #soucingListIndustry.CandidateId and isnull(TB_Candidate.Industry,0) = 0