# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "mark"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "Arena 比武擂台上榜玩家"
field = [
    ["long", "instanceId", "聚会实例ID"],
    ["long", "cid", "玩家CID"],
    ["int", "rank", "排名"],
    ["bytes", "fight_info", "玩家战斗数据"],
    ["long", "last_settle_time_ms", "最后一次结算奖励的时间戳"],
]
key = ["instanceId"]
ukey = []
dbTag = "crossgame_main"
