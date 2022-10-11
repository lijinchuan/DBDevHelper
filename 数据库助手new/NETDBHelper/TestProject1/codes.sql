CREATE function [dbo].[fn_GetLast5DayPositionCount](@Last5WorkDay nvarchar(50),@Cs nvarchar(50),@Industry int ,@Function int,@City nvarchar(50))
Returns real
As
Begin
    declare @Last5DayPositionCount int=0
    Select @Last5DayPositionCount=sum(1) from TB_PPosition a
    join  TB_HTDAB b on a.P_cid=b.HTXH
    join TB_KHDAB c on b.KHMC=c.KHBH
    where P_OpenTime <= (CONVERT(varchar(12) ,@Last5WorkDay, 111 )+' 07:00:00') and datepart(hour,P_OpenTime)<9
     and (ISNULL(@City,'')='' or  a.PosCitys=@City)
     and a.PosTypes=cast( @Function as nvarchar(50))
     and c.Industry=@Industry
Return @Last5DayPositionCount
End
