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

tableComment = "联盟副本数据"
field = [
    ["long", "guildId", "联盟ID"],
    ["long", "instaceId", "联盟副本实例ID"],
    ["long", "monsterId", "怪物ID"],
    ["bool", "isReward", "是否怪物奖励"],
    ["long", "hp", "怪物当前血量"],
    ["bytes", "gainedCidList", "已领取怪物奖励的玩家列表"],
    ["bool", "isTag", "是否标记"],
]

key = ["guildId"]
ukey = []  # key，不能重复
