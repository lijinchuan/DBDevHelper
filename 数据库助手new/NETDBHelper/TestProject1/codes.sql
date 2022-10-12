CREATE PROCEDURE [dbo].[usp_UpgradePositionLevel]
AS
BEGIN
    --BEGIN TRANSACTION
    -- generate promotion
    INSERT INTO [dbo].[TB_Promotion]
               ([Time]
               ,[ResumeID]
               ,[JobName]
               ,[Remark]
               ,[InputMan]
               ,[InputTime])
    SELECT 
        [Time]        = r.BecomeAFullMemberDate,
        [ResumeId]    = r.ID,
        [JobName]    = 'CA',
        [Remark]    = '由于员工转正期至，系统自动将职级调整为CA',
        [InputMan]    = 'System',
        [InputTime]    = GETDATE()
    FROM TB_Tresume r(NOLOCK)
    WHERE r.现状 = '在职'
    AND r.BecomeAFullMemberDate IS NOT NULL
    AND r.BecomeAFullMemberDate <= CAST(GETDATE() AS DATE)
    AND r.职位 = 'NCA'

    -- update employee status from resume
    UPDATE TB_Tresume
    SET 职位 = 'CA'
    FROM TB_Tresume r(NOLOCK)
    WHERE r.现状 = '在职'
    AND r.BecomeAFullMemberDate IS NOT NULL
    AND r.BecomeAFullMemberDate <= CAST(GETDATE() AS DATE)
    AND r.职位 = 'NCA'
    
    IF @@error <> 0  --发生错误
    BEGIN
        ROLLBACK TRANSACTION
    END
    ELSE
    BEGIN
        COMMIT TRANSACTION
    END
END
BB