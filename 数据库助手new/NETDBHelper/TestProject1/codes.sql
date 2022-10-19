----------存储过程:TranKHWindow------------
-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:    <Description,,>
-- =============================================
CREATE  PROCEDURE  TranKHWindow @UserName nvarchar(500),@NewWindow nvarchar(500),@KHXH int,  @OldWindow nvarchar(500)=null
AS
BEGIN
     Declare @HTXH int 
     Declare @HTWindow nvarchar(500)
     Declare @pWindow nvarchar(500)
     select @HTXH=HTXH,@HTWindow=Window from TB_HTDAB where KHMC=@KHXH

     if(@HTXH is not null)
     BEGIN
      IF(@OldWindow is null)
     BEGIN
     

     print('开始更新候选人');
     update TB_Houxuan
set window = @NewWindow
from TB_Houxuan hx
inner join TB_PPosition p
on hx.JobId = p.p_id
where p.p_cid = @HTXH 
and (hx.cFlag <= 12 or hx.cFlag = 13 or hx.cFlag=21) and hx.Window<>@NewWindow

     print('候选人更新成功，更新数量:'+cast(@@ROWCOUNT as nvarchar(500)));
     
     print('开始更新职位');
     update TB_PPosition set Window=@NewWindow
     where P_cid=@HTXH and Window<>@NewWindow
     
     print('职位更新成功，更新数量:'+cast(@@ROWCOUNT as nvarchar(500)));
     END
     END
     ELSE 
     BEGIN
     print('没有有效的BD客户合作记录。'+cast(@HTXH as nvarchar(500)));
     END
    
END

