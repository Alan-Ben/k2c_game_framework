tableComment = "联盟协作攻击日志表"
field = [
    ["long", "guild_id", "公会ID"],
    ["long", "time_ms", "攻击时间毫秒"],
    ["string(256)", "player_name", "攻击者姓名"],
    ["long", "pos_id", "奖励据点ID（配表ID）"],
    ["int", "attr_type", "攻击的属性类型"],
    ["long", "attack_hp", "攻击造成的伤害"],
]
key = ["guild_id"]
ukey = []
dbTag = "main"