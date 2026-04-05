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

tableComment = "宴会-宴会玩家日志"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "instance_id", "宴会实例ID"],
    ["long", "cid", "玩家CID"],
    ["bool", "is_owner", "是否开宴 true-开宴"],
    ["bool", "is_add", "是否赴宴 true-赴宴"],
    ["int", "seat_idx", "座位下标"],
    ["long", "join_ts", "入座时间"],
    ["long", "protect_end_ts", "保护截至时间"],
    ["long", "profit_end_ts", "收益截至时间"],
]
key = []
ukey = []  # key，不能重复
dbTag = "crossgame_log"
