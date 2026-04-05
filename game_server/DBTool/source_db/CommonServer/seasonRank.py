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

tableComment = "赛季排行榜"
field = [
         ["long", "uid", "玩家uid"],
		 ["int", "serverId", "服务器ID"],
         ["string", "playername", "玩家名字"],
		 ["long", "icon", "玩家头像"],
		 ["long", "iconBgk", "玩家头像框"],
         ["int", "grades", "玩家段位"],
		 ["int", "starhoner", "段位星耀值"],
         ["long", "legendscore", "传说积分"],
		 ["int", "lastRankTime", "最近一次排名变更时间"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "comm_main"