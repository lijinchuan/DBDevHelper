select id from tb_houxuan where hxrname='杜秀云' and cflag>=16

--查推荐报告
select id from tb_recommreport where hxid=(select id from tb_houxuan where hxrname='陶正君'  and cflag>=16) 