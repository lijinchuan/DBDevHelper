
    BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
        SET NOCOUNT ON ;  
   
 --SELECT 1          AS KHBH,  
 --       '' AS         KHMC,  
 --       '' AS         GSLXR,  
 --       0.01       AS Commission,  
 --       GETDATE()     OnboardDate  
   
 --获得参数  
        DECLARE @Limit1 DECIMAL(18, 2)  
        DECLARE @Limit2 DECIMAL(18, 2)  
   
        SELECT TOP 1
                @Limit1 = tscfv.ConfigDecimal
        FROM    TB_Sys_ConfigForVersion tscfv
        WHERE   tscfv.ConfigVersion = 1
                AND tscfv.ConfigStrategyId = 4
                AND tscfv.ConfigId = 1  
   
        SELECT TOP 1
                @Limit2 = tscfv.ConfigDecimal
        FROM    TB_Sys_ConfigForVersion tscfv
        WHERE   tscfv.ConfigVersion = 1
                AND tscfv.ConfigStrategyId = 4
                AND tscfv.ConfigId = 2  
   
        SELECT  innerHT.KHMC ,
                MIN(innerHouxuan.OnboardDate) AS firstOnboardDate
        INTO    #tmpFirstOnboardDate
        FROM    TB_HTDAB innerHT
                INNER JOIN TB_PPosition innerPosition ON innerPosition.P_cid = innerHT.HTXH
                INNER JOIN TB_Houxuan innerHouxuan ON innerPosition.P_id = innerHouxuan.JobID
                INNER JOIN TB_HxrOffer tho ON tho.hxid = innerHouxuan.id
        WHERE   innerHouxuan.cFlag > 15
                AND innerHouxuan.cFlag NOT IN ( 17, 19, 20 )
                AND innerHouxuan.OnboardDate IS NOT NULL
                AND tho.OfferHxrOnboradTime IS NOT NULL
        GROUP BY innerHT.KHMC  
 --select * from #tmpFirstOnboardDate  
 --select * from TB_Sys_ConfigForVersion  

        select KHBH,MIN(InputTime) as firstSettleWindow into #tmpFirstSettleWindow
        from TB_KHDAB_Action_History
        where Action =5
        group by KHBH
   
        SELECT  KH.KHBH ,
                KH.KHMC ,
                KH.GSLXR ,
                SUM(CASE WHEN ( DATEADD(yy, 1, tmp.firstOnboardDate) < houxuan.OnboardDate )
                         THEN CAST(CAST(tho.Performance AS DECIMAL(18, 0)) AS INT)
                              * @Limit2
                         ELSE CAST(CAST(tho.Performance AS DECIMAL(18, 0)) AS INT)
                              * @Limit1
                    END) AS Commission ,
                tmp.firstOnboardDate OnboardDate ,
                fsw.firstSettleWindow
        --INTO #tmpCommission  
        FROM    TB_KHDAB KH
                INNER JOIN TB_HTDAB HT ON HT.KHMC = KH.KHBH
                INNER JOIN #tmpFirstOnboardDate tmp ON KH.KHBH = tmp.KHMC
                INNER JOIN TB_PPosition position ON position.P_cid = HT.HTXH
                INNER JOIN TB_Houxuan houxuan ON houxuan.JobID = position.P_id
                INNER JOIN TB_HxrOffer tho ON tho.hxid = houxuan.id  
                Inner Join #tmpFirstSettleWindow fsw on kh.KHBH = fsw.KHBH 
        --LEFT JOIN TB_Payment payment  
        --     ON  payment.hxid = houxuan.id  
        WHERE   houxuan.cFlag > 15
                AND houxuan.cFlag NOT IN ( 17, 19, 20 )
                AND ( @BD IS NULL
                      OR @BD = ''
                      OR KH.GSLXR = @BD
                    )
                AND ( ( @OfferDateStartDate IS NULL
                        AND @OfferDateEndDate IS NULL
                      )
                      OR houxuan.OfferDate BETWEEN @OfferDateStartDate
                                           AND     @OfferDateEndDate
                    )
                AND ( ( @OnboardStartDate IS NULL
                        AND @OnboardStartDate IS NULL
                      )
                      OR houxuan.OnboardDate BETWEEN @OnboardStartDate
                                             AND     @OnboardEndDate
                    )  
        --AND (  
        --        (@PayMentStartDate IS NULL AND @PayMentEndDate IS NULL)  
        --        OR payment.PaymentDate BETWEEN @PayMentStartDate AND @PayMentEndDate  
        --    )  
        GROUP BY KH.KHBH ,
                KH.KHMC ,
                KH.GSLXR ,
                tmp.firstOnboardDate  ,
                fsw.firstSettleWindow
 --SELECT * FROM #tmpCommission  
 --SELECT GSLXR,SUM(Commission) FROM #tmpCommission GROUP BY GSLXR  
   
   DROP TABLE #tmpFirstSettleWindow
        DROP TABLE #tmpFirstOnboardDate  
 --DROP TABLE #tmpCommission  
    END
