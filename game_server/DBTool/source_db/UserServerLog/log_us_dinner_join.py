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

tableComment = "宴会玩家赴宴日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "instanceId", "宴会实例ID"],
    ["long", "joinerType", "赴宴对象类型"],
    ["long", "joinerId", "赴宴对象ID"],
    ["long", "costId", "消耗类型ID"],
    ["long", "gainCoin", "获得宴会币"],
    ["long", "gainScore", "获得宴会积分"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
