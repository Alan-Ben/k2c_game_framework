tableComment = "公会协作据点信息表"
field = [
    ["long", "guild_id", "公会ID"],
    ["long", "area_id", "区域ID"],
    ["int", "reward_point_index", "奖励据点索引"],
    ["bool", "is_unlock", "是否解锁"],
    ["long", "leader_cid", "盟主CID"],
    ["string(1024)", "damage_list", "已造成伤害列表（分号分隔：1100;1200;5000）"],
    ["bool", "had_draw_guild_reward", "公会奖励是否已领取"],
]
key = ["guild_id"]
ukey = []
dbTag = "main"