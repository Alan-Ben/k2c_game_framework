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

tableComment = "玩家功能解锁数据"
field = [
    ["long", "cid", "玩家CID"],
    ["int", "func_type", "已领取功能类型"],
    ["bool", "is_client_notified", "客户端是否已通知"],
    ["bool", "not_draw_reward", "未领取奖励"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
