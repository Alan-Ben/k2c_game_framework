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

__author__ = "copilot"
__date__ = "$2026-03-21$"

tableComment = "远程效果使用日志"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "remote_effect_id", "远程效果id"],
    ["long", "client_serialize", "客户端流水号"],

    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

