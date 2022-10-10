-- =============================================
-- Author:        <Sven,>
-- Create date: <2017/5/2>
-- Description:    <获取DataName的存储过程,>
-- =============================================
Create Function  GetDataName(
   @hxid int
   )
   RETURNS nvarchar(500)
AS
BEGIN
  
  --TB_Houxuan 用到的字段
  Declare @candidateId int
  Declare @ConfirmDate Datetime 
  --推荐报告中的真实姓名
  Declare @trueName nvarchar(500)
  --dataName
  Declare @dataName nvarchar(500)
  
  --历史数据用的
  Declare @mobile1 nvarchar(500)
  Declare @mobile2 nvarchar(500)
  Declare @mobileModifyDate nvarchar(500)
  
  --获取候选人的信息
  select @candidateId=CandidateId,@ConfirmDate=ConfirmDate from TB_Houxuan where id=@hxid
  
  --获取基本数据
  select @trueName=TrueName from TB_RecommReport where Hxid=@hxid
  
  --判断有无历史数据，没有直接拿结果
  if not exists( select top 1 id from TB_Candidate_History where candidateId=@candidateId and ModifyDate<=@ConfirmDate  order by ModifyDate desc)
  begin  
   select @dataName=DataName from TB_Candidate where id=@candidateId
	
	return @dataName
  end
  
  --拿历史数据
  select top 1 @mobile1=Mobile_New,@mobile2=Mobile2_New,@mobileModifyDate=ModifyDate from TB_Candidate_History where candidateId=@candidateId and ModifyDate<=@ConfirmDate
  order by ModifyDate desc
  
  --根据拿到的历史数据判断
  --这里要加个判断,历史中的记录由于可能存在多条同一时间的，要判断同一时间段内的表
  
  select @dataName=ModifyMan from TB_Candidate_History where candidateId=@candidateId and TrueName_New=@trueName
  and (Mobile_New=@mobile1 or    Mobile2_New=@mobile2 or ModifyDate=@mobileModifyDate)
   
  return @dataName 
END
