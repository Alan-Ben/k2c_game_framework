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

tableComment = "分组实例数据"
field = [
    ["long", "group_id", "分组ID"],
    ["long", "activity_id", "活动ID"],
    ["bytes", "us_ids", "参与分组的US服务器ID集合"],
    ["bool", "need_discard", "是否需要删除"],
]

key = []
ukey = []  # key，不能重复
dbTag = "gamelogic_main"