# coding=utf-8
# 活动基金奖励领取日志

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
__date__ ="$2026-01-20$"

tableComment = "活动基金奖励领取日志"
field = [
         ["long", "cid", "玩家CID"],
         ["long", "fund_id", "基金ID"],
         ["long", "activity_instance_id", "活动实例ID"],

         ["int", "had_draw_free_step_before", "领取前免费档已领取阶段"],
         ["int", "had_draw_pay_step_before", "领取前付费档已领取阶段"],
         ["int", "had_draw_free_step_after", "领取后免费档已领取阶段"],
         ["int", "had_draw_pay_step_after", "领取后付费档已领取阶段"],

         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"
recordExpiredSec = 3600*24*90  # 90天过期