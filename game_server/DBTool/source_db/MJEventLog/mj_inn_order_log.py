# -*- coding: utf-8 -*-

tableComment = "梦加日志-太空运输订单结算记录"
field = [
    # 玩家基础信息
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 订单收益记录
    ["int", "num", "订单数量"],
    ["long", "rating_num", "完成该订单获得的人气值"],
    ["long", "blueprint_num", "完成该订单获得的图纸"],
    ["long", "gratitude_num", "完成该订单获得的心意值"],

    # 事件和时间戳
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
