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

tableComment = "玩家游历数据"
field = [
    ["long", "cid", "玩家CID"],
    ["bytes", "finishedOnceEvents", "已完成的一次性事件列表"],
    ["bytes", "finishedEarlyEvents", "已完成的前置事件列表"],
    ["bool", "isFinishedAllEarlyEvents", "已完成的所有前置事件列表"],
    ["long", "lastRandPosId", "上次随机到的游历位置ID"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
