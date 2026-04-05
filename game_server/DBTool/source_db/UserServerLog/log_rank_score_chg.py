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

tableComment = "排行榜分数变更日志"
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "rank_id", "排行榜配表id"],
    ["long", "obj_id", "数据对象id"],
    ["long", "ori_score", "原分数"],
    ["long", "ori_rank", "原排名"],
    ["long", "new_score", "新分数"],
    ["long", "new_rank", "新排名"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
