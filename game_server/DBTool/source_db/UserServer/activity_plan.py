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

tableComment = "活动排期数据"
field = [
    ["long", "plan_ref_id", "计划配表ID"],
    ["bool", "disable", "是否禁用"],
    ["long", "activity_id", "活动ID"],
    ["long", "trigger_ms", "触发时间"],
    ["long", "start_ms", "活动开始时间"],
    ["long", "end_ms", "活动结束时间"],
    ["long", "close_ms", "活动关闭时间"],
]

key = []
ukey = []  # key，不能重复
