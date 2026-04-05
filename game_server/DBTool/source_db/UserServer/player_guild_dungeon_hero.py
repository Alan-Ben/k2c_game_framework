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

classAnnotation = "isIdAuto = false" # 类注解 
tableComment = "玩家公会副本大臣出战数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "heroId", "大臣ID"],
    ["int", "fightedCount", "战斗次数"],
    ["int", "recoveredCount", "恢复次数"],
    ["long", "lastFightedMs", "最近一次战斗时间"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
