----------存储过程:usp_DeleteNcaFollowSetting------------
create proc usp_DeleteNcaFollowSetting
@id int
as
update TB_SystemRecommendPool
set Owner = ''
where Owner = ( SELECT Nca 
FROM TB_NCAFollowSetting WHERE ID = @id
)

Delete FROM TB_NCAFollowSetting WHERE Id=@Id