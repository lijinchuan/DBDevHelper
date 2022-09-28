USE [TopucHunter_Beta]
GO

/****** Object:  StoredProcedure [dbo].[WindowUsedSourcingList_StatisticsTest]    Script Date: 2022/9/28 21:30:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

----------存储过程:WindowUsedSourcingList_StatisticsTest------------
-- =============================================          
-- Author:  <Author,,Name>          
-- Create date: <Create Date, ,>          
-- Description: <Description, ,>          
-- 2013-06-14 Bond DB Transfer        
-- =============================================            
--exec WindowUsedSourcingList_Statistics '2012-09-01','2012-10-29'          
          
CREATE PROCEDURE [dbo].[WindowUsedSourcingList_StatisticsTest]  
    (  
      @inputStartDate VARCHAR(50) ,  
      @inputEndDate VARCHAR(50)  
    )  
AS   
    BEGIN        
        DECLARE @dtInputStartDate DATETIME ,  
            @dtInputEndDate DATETIME        
         
        SET @dtInputStartDate = CONVERT(DATETIME, @inputStartDate)        
        SET @dtInputEndDate = CONVERT(DATETIME, @inputEndDate)        
         
        SELECT  P_cid ,  
                Window ,    
                --COUNT(CASE WHEN ( pStatus IS NULL    
                --                  OR pStatus = 0    
                --                ) THEN 1    
                --           ELSE 0    
                --      END) AS posCount    
                COUNT(1) AS posCount  
        INTO    #tmpPosCount  
        FROM    TB_PPosition  
        WHERE   window IS NOT NULL  
                AND Window <> ' 11'  
                AND ( pStatus IS NULL  
                      OR pStatus = 0  
                    )  
        GROUP BY P_cid ,  
                Window        
         
        SELECT  b.SZQY ,  
                COUNT(1) AS hrCount  
        INTO    #tmpHRCount  
        FROM    TB_LXRXXB b  
        GROUP BY b.SZQY        
         
       
         
        SELECT  p.P_cid ,  
                p.window ,  
                COUNT(1) AS sourcingCount  
        INTO    #tmpHouXuan  
        FROM    TB_Houxuan c  
                INNER JOIN V_Resume_Statistics r ON CONVERT(DATETIME, [cDate]) >= @dtInputStartDate  
                                                    AND CONVERT(DATETIME, [cDate]) <= @dtInputEndDate  
                                                    AND ( ( c.PersonID > 0  
                                                            AND c.PersonID = r.id  
                                                          )  
                                                          OR ( c.PersonID IS NULL  
                                                              AND c.SourcingListID > 0  
                                                              AND c.SourcingListID = r.id  
                                                             )  
                                                        )  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
        GROUP BY p.P_cid ,  
                p.window        
         
        SELECT  p.P_cid ,  
                p.window ,  
                COUNT(1) AS recommendCount  
        INTO    #tmpRecommendCount  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
        WHERE   c.cFlag >= 2  
                AND c.cFlag <> 3 --window同意推荐        
                AND c.RecommendDate >= @dtInputStartDate  
                AND c.RecommendDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                p.window        
         
        SELECT  p.P_cid ,  
                p.window ,  
                COUNT(1) AS interviewCount  
        INTO    #tmpInterview  
        FROM    TB_Interview iv  
                INNER JOIN TB_Houxuan c ON iv.HouxuanID = c.id  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
        WHERE   ( c.cFlag >= 8  
                  OR c.cFlag = 2  
                )  
                AND iv.KehuInterviewDate >= @dtInputStartDate  
                AND iv.KehuInterviewDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                p.window        
         
        SELECT  p.P_cid ,  
                c.Window ,  
        COUNT(1) AS offerCount  
        INTO    #tmpOfferCount  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
        WHERE   c.cFlag >= 12  
                AND cFlag NOT IN ( 15, 17 )  
                AND c.OfferDate >= @dtInputStartDate  
                AND c.OfferDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                c.Window        
                
        SELECT  p.P_cid ,  
                c.Window ,  
                SUM(CAST(o.Performance AS INT)) AS offerPerformance  
        INTO    #tmpOfferPerformance  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
                INNER JOIN TB_HxrOffer o ON c.id = o.hxid  
        WHERE   c.cFlag >= 12  
                AND cFlag NOT IN ( 15, 17 )  
                AND c.OfferDate >= @dtInputStartDate  
                AND c.OfferDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                c.Window                    
         
        SELECT  p.P_cid ,  
                c.window ,  
                COUNT(1) AS onboardCount  
        INTO    #tmpOnboardCount  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
        WHERE   c.cFlag >= 16  
                AND c.cFlag NOT IN ( 17, 20 )  
                AND c.OnboardDate >= @dtInputStartDate  
                AND c.OnboardDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                c.window        
                
        SELECT  p.P_cid ,  
                c.Window ,  
                SUM(CAST(o.Performance AS INT)) AS onboardPerformance  
        INTO    #tmpOnboardPerformance  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
                INNER JOIN TB_HxrOffer o ON c.id = o.hxid  
        WHERE   c.cFlag >= 16  
                AND c.cFlag NOT IN ( 17, 20 )  
                AND c.OnboardDate >= @dtInputStartDate  
                AND c.OnboardDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                c.Window        
                    
        SELECT  p.P_cid ,  
                c.Window ,  
                SUM(CAST(o.Performance AS INT)) AS onboardPerformance  
        INTO    #tmpImplementPerformance  
        FROM    TB_Houxuan c  
                INNER JOIN TB_PPosition p ON c.JobID = p.p_id  
                INNER JOIN TB_HxrOffer o ON c.id = o.hxid  
        WHERE   c.cFlag >= 16  
                AND c.cFlag NOT IN ( 17, 19, 20 )  
                AND c.OnboardDate >= @dtInputStartDate  
                AND c.OnboardDate <= @dtInputEndDate  
        GROUP BY p.P_cid ,  
                c.Window         
         
        SELECT  a.KHBH ,  ht.HTXH,
                a.KHMC ,  ISNULL(a.ZYLXR,ht.KHQSR) AS HR,
                pos.Window AS Window ,  
                ht.Window AS hWindow ,  
                lxr.hrCount ,  
                pos.posCount ,  
                hx.sourcingCount ,  
                recomm.recommendCount ,  
                interview.interviewCount ,  
                offer.offerCount ,  
                ISNULL(#tmpOfferPerformance.offerPerformance, 0) AS offerPerformance ,  
                onboard.onboardCount ,  
                ISNULL(#tmpOnboardPerformance.onboardPerformance, 0) AS onboardPerformance ,  
                ISNULL(#tmpImplementPerformance.onboardPerformance, 0) AS implementPerformance  
        FROM    TB_HTDAB ht  
                INNER JOIN TB_KHDAB a ON a.KHBH = ht.KHMC  
                FULL JOIN #tmpPosCount pos ON pos.P_cid = ht.HTXH  
                LEFT JOIN #tmpHRCount AS lxr ON lxr.SZQY = a.KHBH  
                LEFT JOIN #tmpHouXuan AS hx ON ht.HTXH = hx.P_cid  
                                               AND hx.window = pos.window  
                LEFT JOIN #tmpRecommendCount AS recomm ON ht.HTXH = recomm.P_cid  
                                   AND recomm.window = pos.window  
                LEFT JOIN #tmpInterview AS interview ON ht.HTXH = interview.P_cid  
                                                        AND interview.window = pos.window  
                LEFT JOIN #tmpOfferCount AS offer ON ht.HTXH = offer.P_cid  
                                                     AND offer.window = pos.window  
                LEFT JOIN #tmpOfferPerformance ON ht.HTXH = #tmpOfferPerformance.P_cid  
                                                  AND #tmpOfferPerformance.window = pos.window  
                LEFT JOIN #tmpOnboardCount AS onboard ON ht.HTXH = onboard.P_cid  
                                                         AND onboard.window = pos.window  
                LEFT JOIN #tmpOnboardPerformance ON ht.HTXH = #tmpOnboardPerformance.P_cid  
                                                    AND #tmpOnboardPerformance.window = pos.window  
                LEFT JOIN #tmpImplementPerformance ON ht.HTXH = #tmpImplementPerformance.P_cid  
                                                      AND #tmpImplementPerformance.Window = pos.window  
        WHERE   ( pos.Window IS NOT NULL  
                  AND pos.Window <> ''  
                )  
                AND ( ht.HistoryFlag IS NULL  
                      OR ht.HistoryFlag <> 1  
                    )  
        ORDER BY ht.HTXH ,  
                pos.Window        
         
        DROP TABLE #tmpPosCount        
        DROP TABLE #tmpHRCount        
        DROP TABLE #tmpHouXuan        
        DROP TABLE #tmpRecommendCount        
        DROP TABLE #tmpInterview        
        DROP TABLE #tmpOfferCount        
        DROP TABLE #tmpOfferPerformance    
        DROP TABLE #tmpOnboardCount        
        DROP TABLE #tmpOnboardPerformance        
        DROP TABLE #tmpImplementPerformance     
    END        
        
--SELECT * FROM TB_KHDAB tk WHERE tk.KHMC LIKE '%金宝%'        
--SELECT * FROM TB_HTDAB th WHERE th.KHMC = 2884        
--SELECT * FROM TB_PPosition tp WHERE tp.P_cid = 392


GO


