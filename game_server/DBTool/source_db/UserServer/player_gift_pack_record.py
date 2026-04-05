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

__author__ = "scott"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家礼包购买记录"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "gift_pack_id", "礼包id"],
    ["int", "client_buy_count", "客户端购买次数"],
    ["int", "buy_count", "购买次数"],
    ["long", "next_refresh_time_ms", "下次刷新时间"],
    ["long", "relative_life_cycle_instance_id", "活动实例id"],
]
key = ["cid"]  # key
ukey = []  # key,不能重复
dbTag = "main"
