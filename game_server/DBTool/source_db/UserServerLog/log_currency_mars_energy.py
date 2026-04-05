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

tableComment = "货币（火星能量）数据日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["long", "cid", "玩家CID"],
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    ["int", "player_level", "玩家等级"],
    
    ["long", "chgValue", "金币变化数量"],
    ["long", "value", "金币数量"],
    ["long", "costSpeed", "消耗速度（单位：分钟）"],
    ["long", "lastSettleTimeMs", "上次结算时间（毫秒）"],
    ["long", "totalGainCount", "总获得数量"],
    ["long", "totalConsumeCount", "总消耗数量"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
