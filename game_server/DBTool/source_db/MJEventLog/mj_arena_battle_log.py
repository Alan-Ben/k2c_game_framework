# -*- coding: utf-8 -*-

tableComment = "梦加日志-碰撞测试挑战记录"
field = [
    # 玩家基础信息
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 挑战详情
    ["long", "fid", "伙伴id"],
    ["long", "rival_cid", "对手cid"],
    ["long", "rival_hp", "对手总血量"],
    ["int", "kill_num", "击败对手伙伴数量"],
    ["long", "initial_hp", "起始血量"],
    ["long", "final_hp", "剩余血量"],
    ["string(2000)", "buff_list", "增益明细"],
    ["long", "chg_score", "变更积分"],

    # 时间戳
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
