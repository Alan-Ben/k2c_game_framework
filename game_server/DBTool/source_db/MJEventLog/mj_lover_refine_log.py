# -*- coding: utf-8 -*-

tableComment = "梦加日志-情人洗练记录"

field = [
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    ["long", "lid", "情人id"],
    ["long", "intimacy", "亲密度"],
    ["int", "refine_type", "洗练类型：1=普通领悟；2=高级领悟"],
    ["long", "attr_id", "本次洗练的属性id（技能id）"],
    ["int", "is_succeed", "本次洗练是否成功：1=成功；0=未成功"],
    ["int", "before_attr", "洗练前属性值"],
    ["int", "final_attr", "洗练后属性值"],
    ["int", "event", "事件id"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
]

key = []
ukey = []
dbTag = "us_log"
