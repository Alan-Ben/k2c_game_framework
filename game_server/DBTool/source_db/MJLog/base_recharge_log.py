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

tableComment = "梦加日志-充值表"
field = [
    ["string(32)", "sole_id", "全服唯一角色id"],
    ["string(64)", "uid", "用户id"],
    ["long", "cid", "角色id"],
    ["string(128)", "order_id", "订单号"],
    ["string(128)", "tp_order_id", "第三方订单号"],
    ["string(128)", "sdkorderId", "SDK订单号"],
    ["decimal(20,2)", "money", "充值金额"],
    ["decimal(20,2)", "cn_money", "人民币金额"],
    ["string(32)", "goods_id", "充值货物id"],
    ["int", "coin_num", "货币数量"],
    ["int", "status", "状态:充值=1"],
    ["int", "pay_time", "充值时间戳（10位）"],
    ["int", "arrive_time", "到账时间戳（10位）"],
    ["int", "timestamp", "创建时间戳（10位）"],
    ["string(50)", "adid", "设备id"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "账号归属的平台id"],
    ["string(50)", "region", "账号归属的区域id"],
    ["short", "sdk_type", "订单类型"],
    ["string(64)", "pay_id", "支付方式"],
    ["text", "ext", "扩展字段：json格式"],
    ["int", "date_time", "日期"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

