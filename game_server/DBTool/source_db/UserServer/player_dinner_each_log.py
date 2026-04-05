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

__author__ = "scott"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家宴会交互数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "targetCid", "对方玩家CID"],
    ["int", "joinedCount", "参加对方宴会次数"],
    ["int", "beJoinedCount", "对方参加己方宴会次数"],
    ["int", "lastTs", "最后一次更新时间"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
