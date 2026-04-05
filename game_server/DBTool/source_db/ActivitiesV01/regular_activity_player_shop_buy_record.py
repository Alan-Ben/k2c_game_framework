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

tableComment = "活动玩家商店购买记录数据"
field = [
    ["long", "instance_id", "活动实例ID"],
    ["long", "shop_db_id", "商店实例id"],
    ["long", "cid", "玩家id"],
    ["long", "item_id", "物品id"],
    ["long", "had_buy_count", "已经购买的次数"],
]

key = ["instance_id","shop_db_id"]
ukey = []  # key，不能重复
