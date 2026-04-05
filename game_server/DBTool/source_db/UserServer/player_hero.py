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

tableComment = "玩家大臣数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "hero_id", "大臣id"],
    ["long", "skin_id", "皮肤id"],
    ["int", "level", "等级"],
    ["int", "step", "进阶的阶级"],
    ["int", "star", "觉醒星级"],
    ["long", "building_id", "大臣所在建筑Id"],
    ["long", "serial", "放置序列号"],
    ["long", "ext_add_power", "道具额外加成实力"],
    ["long", "arena_add_power", "竞技场额外加成实力"],
    ["long", "travel_add_power", "游历额外加成实力"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
