select wh.* from TB_Work_History wh
join TB_Houxuan h on h.CandidateId=wh.CandidateId 
join TB_RecommReport rr on rr.id=h.RecommReportID
cross apply(
   select top 1 * from TB_RRportWorkList rw where rw.RReportID=rr.id order by case when rw.EndDate is null then getdate() else rw.EndDate end desc,rw.StartDate desc
)rw
where h.HxrName='陈大宝366' and h.cFlag>=16 and wh.Company_New=rw.Company and (wh.Company_Old<>rw.Company or wh.Company_Old is null)
order by wh.ModifyDate
select top 10 * from TB_Work_History order by id desc  