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
__date__ = "$2025-01-01$"

tableComment = "梦加日志-钻石充值表"
field = [
    ["string(64)", "uid", "平台用户id"],
    ["long", "cid", "角色id"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["string(50)", "region", "区域id"],
    ["int", "lv", "角色等级"],
    ["int", "vip_lv", "VIP等级"],
    ["int", "ar_time", "角色创建时间"],
    ["string(50)", "nation", "国家"],
    ["string(50)", "version", "版本号"],
    ["string(50)", "adid", "设备id"],
    ["string(32)", "goods_id", "商品id"],
    ["int", "event", "事件ID"],
    ["long", "oldNumber", "旧值"],
    ["long", "finalNumber", "新值"],
    ["long", "exchange", "变化值"],
    ["short", "sdk_type", "订单类型"],
    ["int", "action", "操作类型"],
    ["int", "timestamp", "时间戳"],
    ["string(500)", "ext", "拓展参数"],
]
key = ["cid", "event", "timestamp"]
ukey = []  # key，不能重复
dbTag = "us_log"
