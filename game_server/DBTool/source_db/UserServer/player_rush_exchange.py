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

__author__ = "cooper"
__date__ = "$2026-01-21 18:00:00$"

tableComment = "玩家急速兑换数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "group_id", "礼包组ID"],
    ["long", "ref_id", "当前礼包配置ID"],
    ["long", "active_time_ms", "当前兑换开始时间 如果没兑换影响刷新礼包时间"],
    ["long", "exchange_time_ms", "兑换时间 影响什么时候可以领奖"],
    ["bool", "is_rewarded", "是否已领奖"],
    ["int", "today_exchange_count", "当天兑换次数"],
    ["long", "next_reset_count_time_ms", "下次刷新兑换次数时间"],
    ["string(2048)", "used_ref_ids", "已使用的礼包ID列表（分号分隔，用于不放回抽取）"],
]

key = ["cid"]
ukey = []  # key，不能重复
dbTag = "main"
