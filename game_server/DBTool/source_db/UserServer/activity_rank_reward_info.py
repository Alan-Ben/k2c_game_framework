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

tableComment = "活动排行榜奖励领取记录数据"
field = [
    ["long", "activity_instance_id", "活动实例ID"],
    ["long", "rank_id", "排行榜配置ID"],
    ["long", "cid", "cid"],
    ["long", "groupId", "团体id"],
    ["int", "rank", "排名"],
    ["bool", "had_draw", "是否已领取"],
    ["long", "score", "分数"],
]

key = []
ukey = []  # key，不能重复
