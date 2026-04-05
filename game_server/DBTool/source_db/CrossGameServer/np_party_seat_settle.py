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

tableComment = "Party 聚会座位结算数据表"
field = [
		 ["long", "instanceId", "聚会实例ID"],
		 ["int", "seatIdx", "座位下标"],
		 ["int", "continuedSecs", "持续时长（秒）"],
        ]
key = ["instanceId"]
ukey = []
dbTag = "crossgame_main"