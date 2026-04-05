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

tableComment = "玩家妃子聊天数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "last_refresh_count_time_ms", "上次刷新次数时间ms"],
    ["int", "day_had_send_pay_ai_times", "今日已发送付费AI次数"],
    ["int", "day_had_send_circle_ai_times", "今日已发送圈子AI次数"],
    ["int", "day_had_send_circle_ai_reply_times", "今日已发送圈子AI回复次数"],
    ["int", "day_had_send_consort_initiative_times", "今日妃子主动发送消息次数"],
    ["int", "day_had_evaluate_reply_times", "今日已评价回复次数"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
