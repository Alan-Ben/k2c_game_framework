# -*- coding: utf-8 -*-

tableComment = "梦加日志-藏品洗练记录"

field = [
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    ["long", "aid", "藏品id"],
    ["long", "collection_id", "藏品唯一id（数据库id）"],
    ["int", "a_level", "藏品等级"],
    ["int", "a_aptitude", "藏品资质点"],
    ["long", "fid", "伙伴id，无伙伴则记0"],
    ["int", "op_type", "操作类型：1=洗练"],
    ["int", "is_succeed", "本次洗练是否成功：1=成功；0=未成功"],
    ["int", "initial_attr", "变更前的属性值（加成万分比）"],
    ["int", "final_attr", "变更后的属性值（加成万分比）"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
]

key = []
ukey = []
dbTag = "us_log"
