CREATE proc [dbo].[usp_Report_NCAReport]
@nca nvarchar(50),
@office nvarchar(50),
@startDate datetime,
@endDate datetime
as
BEGIN

SET NOCOUNT ON;

select rpt.*,dbo.fn_GetFunctionNameByIds(trs.PosTypes) as PosFunction,trs.Office from 
(
select LoginId,EnglishName,ChineseName,sum(isnull(NewCandidate,0)) as NewCandidate,
sum(isnull(NewCandidateRecommended,0)) as NewCandidateRecommended,
sum(isnull(NewFunction,0)) as NewFunction ,
sum(isnull(NewFunctionRecommended,0)) as NewFunctionRecommended,
sum(isnull(NewInteview,0)) as NewInteview,
sum(isnull(PositionHandling,0)) as PositionHandling,
sum(isnull(CommunictionDuration,0)) as CommunictionDuration,
SUM(isnull(EffectiveCommunication,0))as EffectiveCommunication
from tb_Nca_Daily_Report
where (LoginId = @nca OR ISNULL(@nca,'')='')AND
      (Office = @office OR ISNULL(@office,'')='')AND
      RecordDate between @startDate and @endDate
group by LoginId,EnglishName,ChineseName
) rpt
join TB_Tresume trs
on rpt.LoginId = trs.姓名
where trs.DimissionDate is null OR trs.DimissionDate >= @startDate
END