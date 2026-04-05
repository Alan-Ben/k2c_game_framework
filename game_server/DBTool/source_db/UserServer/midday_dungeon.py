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

tableComment = "午间副本"
field = [
    ["long", "round_preview_time_ms", "本轮预告时间"],
    ["long", "round_start_time_ms", "本轮开始时间"],
    ["long", "round_end_time_ms", "本轮结束时间"],
    ["int", "round_drop_box_num", "本轮掉落宝箱数量"],
    ["bool", "had_send_open_notice", "是否已发送开启通知"],
]
key = []
ukey = []  # key，不能重复
tasktag = "main"
