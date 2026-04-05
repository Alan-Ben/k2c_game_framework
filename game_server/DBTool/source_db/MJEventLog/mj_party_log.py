# 宴会参与日志
tableComment = "梦加日志-宴会参与记录"
field = [
    # 玩家基础信息（必需）
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 参与记录字段
    ["long", "party_type", "舞会类型"],
    ["int", "action", "参与类别：1=参与；2=举办"],

    # 事件和时间戳（必需）
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
