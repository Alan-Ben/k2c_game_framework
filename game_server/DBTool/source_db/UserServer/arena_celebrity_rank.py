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

tableComment = "竞技场名人榜数据"
field = [
    ["long", "attacker_cid", "挑战者角色ID"],
    ["string(128)", "attacker_name", "挑战者角色名"],
    ["string(128)", "defender_name", "被挑战者角色名"],
    ["int", "defeat_hero_num", "击败对方英雄数量"],
    ["bool", "is_select_attack", "是否选择挑战"],
    ["long", "timestamp", "时间戳"],
]

key = []
ukey = []  # key，不能重复
