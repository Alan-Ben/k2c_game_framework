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

tableComment = "US火星矿占领日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "mineInstanceId", "火星矿实例ID"],
    ["long", "mineRefId", "火星矿配置ID"],
    ["long", "cid", "结算玩家CID"],
    ["long", "teamId", "结算队伍ID"],
    ["long", "remainNum", "剩余物品物品数量"],
    ["long", "lossValue", "队伍损耗数量"],
    ["long", "collectSpeed", "采集速度"],
    ["long", "startCollectMs", "开启采集时间（毫秒）"],
    ["long", "endCollectMs", "结束采集时间（毫秒）"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
