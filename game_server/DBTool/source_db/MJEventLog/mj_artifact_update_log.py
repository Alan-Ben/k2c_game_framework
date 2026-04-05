tableComment = "梦加日志-藏品升级记录"
field = [
    # 玩家基础信息（必需）
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 升级记录
    ["long", "aid", "藏品id"],
    ["long", "collection_id", "藏品实例id"],
    ["long", "fid", "伙伴id，无伙伴则记0"],
    ["int", "initial_lv", "升级前的等级"],
    ["int", "initial_aptitude", "升级前的资质点"],
    ["int", "final_lv", "升级后的等级"],
    ["int", "final_aptitude", "升级后的资质点"],

    # 时间戳（必需）
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
