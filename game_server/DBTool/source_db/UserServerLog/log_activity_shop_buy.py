# coding=utf-8
# 活动商店购买日志

#支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__="claude"
__date__ ="$2025-01-12$"

tableComment = "活动商店购买日志"
field = [
         ["long", "cid", "玩家CID"],
         ["long", "instance_id", "活动实例ID"],
         ["long", "shop_id", "商店ID"],
         ["long", "item_id", "物品ID"],
         ["int", "num", "购买数量"],

         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"
