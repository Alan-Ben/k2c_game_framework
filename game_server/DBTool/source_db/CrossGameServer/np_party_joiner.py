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

__author__="mark"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "Party 聚会赴宴玩家数据表"
field = [
		 ["long", "instanceId", "聚会实例ID"],
		 ["long", "cid", "赴宴玩家CID"],
		 ["int", "seatIdx", "赴宴座位下标"],
		 ["int", "joinTs", "赴宴时间戳"],
		 ["int", "protectEndTs", "保护罩截至时间戳"],
		 ["int", "profitEndTs", "收益结束时间戳（秒）"],
        ]
key = ["instanceId"]
ukey = []
dbTag = "crossgame_main"