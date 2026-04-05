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

__author__ = "claude"
__date__ = "$2025-1-08 00:00:00$"

tableComment = "玩家太空寻宝保底数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "area_id", "区域ID"], 
    ["int", "treasure_guarantee_count", "奇物保底计数"], 
    ["int", "consecutive_high_quality_count", "连续获得高品质矿石次数"],
    ["int", "consecutive_no_high_quality_count", "连续未获得高品质矿石次数"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"