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

__author__ = "alzq"
__date__ = "$2023-11-21 10:17:54$"

tableComment = "活动排行榜数据"
field = [
    ["long", "rank_id", "排行榜配置ID"],
    ["long", "cross_instance_id", "跨服分组实例ID"],
    ["bool", "is_discard", "是否需要释放，涉及跨服同步"],
]

key = []
ukey = []  # key，不能重复
dbTag = "crossrank_main"