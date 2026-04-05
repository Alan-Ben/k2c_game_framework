tableComment = "晚间副本攻击日志"
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    ["long", "cid", "玩家CID"],
    ["int", "player_level", "玩家等级"],
    ["long", "total_hero_power", "玩家总伙伴实力"],
    ["long", "damage", "造成的实际伤害"],
    ["long", "hero_exp_reward", "获得伙伴经验奖励数量"],
    ["bool", "is_killed", "是否击杀"],
]
key = []
ukey = []
dbTag = "us_log"
