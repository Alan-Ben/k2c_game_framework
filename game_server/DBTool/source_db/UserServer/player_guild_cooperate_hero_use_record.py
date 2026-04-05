tableComment = "玩家联盟协作大臣使用记录数据表"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "hero_id", "大臣id"],
    ["int", "use_count", "使用次数"],
    ["int", "recover_count", "恢复次数"],
    ["long", "last_refresh_time_ms", "上次刷新时间"],
]
key = ["cid"]
ukey = []
dbTag = "main"