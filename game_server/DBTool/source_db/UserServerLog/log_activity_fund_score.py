# coding=utf-8
# 活动基金分数变动日志（公式分数和任务分数共用）

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
__date__ ="$2026-01-21$"

tableComment = "活动基金分数变动日志"
field = [
         ["long", "cid", "玩家CID"],
         ["long", "fund_id", "基金ID"],
         ["long", "activity_instance_id", "活动实例ID"],

         ["int", "score_type", "分数类型(1=公式分数,2=任务分数)"],
         ["long", "score_before", "变动前分数"],
         ["long", "score_after", "变动后分数"],
         ["long", "change_value", "变动值"],

         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"
recordExpiredSec = 3600*24*90  # 90天过期