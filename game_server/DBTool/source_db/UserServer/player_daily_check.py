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
__date__ = "$2022-04-25$"

tableComment = "玩家每日签到"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "last_check_round_start_time_ms", "上一次签到轮的开始时间ms"],

    ["long", "cur_round_start_time_ms", "本轮签到的开始时间ms"],
    ["int", "not_check_days", "没签到的天数"],
    ["long", "consort_id", "情人id"],
    ["string(256)", "dessert_list", "甜品列表"],
    ["bool", "has_check", "今天是否已签到"],
    ["long", "choose_dessert_id", "选择的甜品id"],
    ["bytes", "reward_list", "奖励列表"],
    ["long", "next_refresh_time_ms", "下一次刷新时间"],
    ["int", "total_check_days", "累计签到天数"],
    ["int", "rewarded_check_days", "已领奖的签到天数"],
]
key = ["cid"]
ukey = []
dbTag = "main"
