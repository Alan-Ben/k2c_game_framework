# -*- coding: utf-8 -*-

tableComment = "梦加日志-藏品重塑记录"

field = [
    # 角色基础信息
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 重塑信息
    ["int", "recycle_num", "本次重塑的藏品数量"],
    ["text", "recycle_dict", "重塑明细JSON"],

    # 时间戳
    ["int", "timestamp", "事件发生时间戳(10位)"],
]

key = []
ukey = []
dbTag = "us_log"
