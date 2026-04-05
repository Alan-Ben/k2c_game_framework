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

tableComment = "联盟大臣派遣"
field = [
    ["long", "guild_id", "联盟id"],
    ["long", "cid", "玩家"],
    ["long", "hero_id", "大臣id"],
    ["int", "add_value", "加成值"],
    ["int", "level", "等级"],
    ["long", "power", "实力"],
    ["long", "skin_id", "皮肤id"],
]

key = ["guild_id"]
ukey = []  # key，不能重复
