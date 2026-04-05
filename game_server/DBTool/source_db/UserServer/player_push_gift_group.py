tableComment = "玩家推送礼包组状态"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "group_id", "礼包组id"],
    ["long", "trigger_time_ms", "触发时间毫秒"],
    
    ["long", "push_gift_id", "推送礼包id"],
    ["long", "active_time_ms", "激活时间毫秒"],
    ["bool", "has_read", "是否已读"],
]
key = ["cid"]
ukey = []
dbTag = "main"
