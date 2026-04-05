tableComment = "梦加日志-爬塔采掘进度记录"
field = [
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],
    ["int", "triggerid_before", "采掘进度_挑战前"],
    ["int", "is_win", "是否成功"],
    ["int", "triggerid_final", "采掘进度_挑战后"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
