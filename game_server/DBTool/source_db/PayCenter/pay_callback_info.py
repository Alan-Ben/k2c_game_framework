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

__author__ = "PayCenter"
__date__ = "$2025-08-26$"

tableComment = "支付回调信息记录表"
field = [
    # 基础订单信息
    ["string(64)", "order_id", "订单id"],
    ["string(32)", "sdk_order_id", "SDK订单id"],

    # 用户信息
    ["string(32)", "uid", "用户id"],
    ["long", "cid", "角色id"],
    
    # 支付信息
    ["string(32)", "app_id", "应用ID"],
    ["long", "product_id", "产品ID"],
    ["string(32)", "amount", "商品定价"],
    ["string(32)", "amount_type", "商品定价货币"],
    ["string(32)", "pay_type", "支付方式（钱包聚合平台）"],
    ["long", "create_time", "创建时间（10位时间戳）"],
    ["long", "pay_time", "支付时间（10位时间戳）"],
    ["string(255)", "extension", "扩展参数"],
    ["int", "sdk_type", "订单来源:1正常，2补单，3虚拟充值,4测试订单"],
    ["string(64)", "trade_id", "第三方订单号"],
    ["string(64)", "sku_id", "第三方内购产品id"],
    ["string(32)", "sdk_pay_id", "sdk档位ID"],
    ["int", "purchase_type", "购买类型：0普通，1测试，2促销，3奖励"],
    ["string(32)", "payment", "实际支付金额"],
    ["string(32)", "payment_code", "实际支付货币"],
    ["string(64)", "channel_code", "渠道标识(网页支付时玩家选择的站点标识)"],
    ["string(64)", "pay_id", "支付ID（钱包ID）"],
    ["int", "order_type", "订单类型：1 内购，2 网页充值，3 福利（虚拟充值）"],

    # 推送信息
    ["bool", "had_push", "是否已推送"],
    ["bool", "delivery_fail", "是否发货失败"],
]

key = ["order_id"]
ukey = []  # key，不能重复
dbTag = "pc_db"