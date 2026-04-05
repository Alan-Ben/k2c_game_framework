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
    ["long", "dialogue_id", "对话id"],
    ["long", "trigger_time_ms", "触发时间ms"],
    ["bool", "had_draw_reward", "是否领取奖励"],
    ["bytes", "detail_info", "聊天详情"],
    ["long", "draw_reward_time_ms", "领取奖励时间ms"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
