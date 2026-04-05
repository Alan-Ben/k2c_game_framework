# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

#支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__="yoey"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "精彩比赛录像"
field = [ 
		["text", "data", "战斗数据"], 
		["long", "dungeonId", "地图ID"],
		["long", "battleResVersion", "战斗资源版本号"],
		["int", "endTimeSec", "战斗结束时间"],
		["int", "battleTimeMs", "战斗持续时间"],
		["int", "playerNum", "玩家数量"],
		["int", "avgDps", "综合Dps"],
		["int", "maxKillNum", "最大击杀人口数量"],
		["int", "totoalKillNum", "总击杀人口数量"],
		["int", "viewCount", "查看次数"],
		["int", "voteCount", "点赞次数"],
		["varbinary(1024)", "campA1", "阵营A"], 
		["varbinary(1024)", "campB1", "阵营B"], 
		["int", "roomType", "比赛类型"],
	]
	
key = []
ukey = []
dbTag = "comm_main"