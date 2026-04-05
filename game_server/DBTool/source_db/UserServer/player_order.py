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

tableComment = "玩家订单数据"
field = [
    ["long", "cid", "玩家CID"],
    ["string", "order_id", "订单号"],
    ["long", "pay_id", "支付id"],
    ["long", "goods_id", "商品id"],
    ["int", "order_status", "订单状态"],
    ["int", "pay_type", "支付方式类型"],
    ["long", "create_time_ms", "创建时间 ms"],
    ["string", "sdk_order_id", "sdk订单号"],
    ["long", "pay_time_ms", "支付时间 ms"],
    ["long", "arrive_time_ms", "收到支付回调时间 ms"],
    ["bool", "is_offline_pay", "是否离线时支付"],
    ["text", "item_list", "对应物品列表"],
    ["bool", "had_client_notify_pay", "是否客户端已通知支付"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
