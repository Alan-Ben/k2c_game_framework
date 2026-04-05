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

tableComment = "排行榜结算奖励日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "cid", "玩家CID"],
    ["int", "version", "0-旧流程 1-新流程1"],
    ["bool", "is_mail", "是否邮件奖励"],
    ["long", "rank_instance", "排行榜实例ID"],
    ["long", "rank_id", "排行榜ID"],
    ["int", "rank_type", "排行榜类型"],
    ["long", "rank", "排行"],
    ["bool", "is_leader", "是否盟主"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
