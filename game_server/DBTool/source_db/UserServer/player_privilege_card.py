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

tableComment = "玩家权益卡数据"
field = [
    ["long", "cid", "玩家CID"],
    ["int", "cardType", "权益卡类型"],
    ["long", "startS", "生效开始时间戳（秒）"],
    ["long", "endS", "生效结束时间戳（秒）"],
    ["long", "lastGainDailyRewardS", "最后一次领取每日奖励的时间戳（秒）"],
]
key = ["cid"]
ukey = []
dbTag = "main"
