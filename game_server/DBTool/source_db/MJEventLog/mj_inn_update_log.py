# -*- coding: utf-8 -*-

tableComment = "梦加日志-旅店货舱/清单/礼物/勋章升级记录"
field = [
    # 玩家基础信息
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 等级变更记录
    ["long", "device_id", "货舱/清单/礼物/勋章id"],
    ["int", "before_lv", "升级前等级"],
    ["int", "after_lv", "升级后等级"],

    # 事件和时间戳
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"