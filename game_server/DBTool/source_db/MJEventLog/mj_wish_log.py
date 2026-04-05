# -*- coding: utf-8 -*-

tableComment = "梦加日志-幸运转盘抽卡记录"
field = [
    # 玩家基础信息（必需）
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 抽卡记录
    ["int", "amount", "抽卡数量：单抽=1，十连抽=10"],
    ["string(2048)", "product", "抽卡产出物"],

    # 事件和时间戳（必需）
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
