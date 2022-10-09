CREATE VIEW [dbo].[V_KHListWithlxr]
AS
SELECT lxrxxb.LXRXM, lxrxxb.BGDH, lxrxxb.YDDH, lxrxxb.DZYX, lxjlb.LXBH, lxjlb.LXSJ, lxjlb.LXJY, KH.KHBH, KH.KHDM,
	KH.KHMC, KH.SF, KH.ZYLXR, KH.DQ, KH.DH, KH.DZ, KH.GSLXR, KH.Industry, KH.Status, KH.InputDate, KH.InputMan,
	KH.BLKH, KH.BZ, HT.Window, KH.DegreeOfIntention, HT.IsGreen, HT.HtStartDate, HT.HtEndDate, KH.Source,
	KH.ModifyDate, KH.Property, KH.EmployeeNumber, KH.GSLXRApplicant
FROM dbo.TB_KHDAB AS KH (NOLOCK)
LEFT OUTER JOIN(
	SELECT LXRXM, BGDH, YDDH, DZYX, SZQY
    FROM dbo.TB_LXRXXB AS xxb(NOLOCK)
    WHERE   LXRBH =(
		SELECT TOP (1) LXRBH
		FROM dbo.TB_LXRXXB (NOLOCK)
		WHERE SZQY = xxb.SZQY)
) AS lxrxxb
ON lxrxxb.SZQY = KH.KHBH

LEFT OUTER JOIN (
	SELECT LXBH, QYMC, LXSJ, LXJY
    FROM dbo.TB_LXJLB AS jlb(NOLOCK)
    WHERE LXBH = (
		SELECT TOP (1) LXBH
        FROM dbo.TB_LXJLB(NOLOCK)
        WHERE QYMC = jlb.QYMC
        ORDER BY LXBH DESC)
) AS lxjlb
ON lxjlb.QYMC = KH.KHBH

LEFT OUTER JOIN(
	SELECT   Window, KHMC, IsGreen, HtStartDate, HtEndDate, HTXH
    FROM      dbo.TB_HTDAB AS itemHT(NOLOCK)
    WHERE   HTXH =(
		SELECT   TOP (1) HTXH
        FROM      dbo.TB_HTDAB(NOLOCK)
        WHERE   KHMC = itemHT.KHMC
        ORDER BY HTXH desc)
) AS HT ON HT.KHMC = KH.KHBH