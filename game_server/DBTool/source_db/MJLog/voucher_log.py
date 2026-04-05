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

__author__ = "claude"
__date__ = "$2025-01-08$"

tableComment = "梦加日志-代金券日志表"
field = [
    ["string(64)", "uid", "用户在平台注册的ID"],
    ["long", "cid", "角色id"],
    ["int", "server_id", "玩家登录时的服务器id"],
    ["int", "platform", "账号归属的平台id"],
    ["string(50)", "region", "账号归属的区域id"],
    ["int", "lv", "事件发生时的角色等级"],
    ["int", "vip_lv", "事件发生时的角色VIP"],
    ["int", "ar_time", "角色创建时间戳（10位）"],
    ["string(100)", "nation", "记录订单创建时的实时国家代号（2位）（无数据时记为空，字符串）"],
    ["string(50)", "version", "记录订单创建时的客户端版本号（无数据时记为空，字符串）"],
    ["string(50)", "adid", "取事件发生时的设备id；如果取不到则取创角的设备id"],
    ["int", "event", "触发该事件的原因，等同于游戏内的mainevent"],
    ["long", "oldNumber", "总代金券的旧值 总代金券数量变更前的值"],
    ["long", "finalNumber", "总代金券的新值 总代金券数量变更后的值"],
    ["long", "exchange", "总代金券的改变值"],
    ["string(32)", "goods_id", "游戏策划配置的充值货物id，消耗代金券时，记录玩家用代币购买了什么商品。其他情况则记为null"],
    ["int", "goods_count", "消耗代金券购买商品时，记录本次购买的商品数量。其他情况则记为0"],
    ["int", "action", "1=获得,2=消耗"],
    ["int", "timestamp", "记录事件发生的时间戳（10位）"],
    ["int", "cn_money", "商品对应人民币"],
    ["decimal(20,6)", "money", "海外对应美元 国内对应人民币"],
    ["string(500)", "order_id", "订单号"],
    ["string(500)", "sdk_pay_id", "sdk档位id"],
    ["string(40)", "match_id", "匹配号"],
    ["int", "subevent", "物品获得子事件"],
    ["long", "mainExtraParam", "主事件额外参数"],
    ["long", "subExtraParam", "子事件额外参数"],
    ["int", "date_time", "事件日期"],
    ["long", "paid_oldNumber", "付费代金卷的旧值 代金券数量变更前的值"],
    ["long", "paid_finalNumber", "付费代金卷的新值 代金券数量变更后的值"],
    ["long", "paid_exchange", "付费代金卷的改变值"],
    ["text", "ext", "扩展字段：json格式 ，根据对应事件自己定义字符串的数据库类型"],
    ["string(64)", "adfrom", "一级渠道"],
    ["string(64)", "adfrom2", "二级渠道"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
