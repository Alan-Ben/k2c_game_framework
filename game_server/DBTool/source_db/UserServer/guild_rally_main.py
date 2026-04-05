# coding=utf-8

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

# 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段

classAnnotation = "isIdAuto = true"
tableComment = "联盟集结主数据"
field = [
    ["long", "guild_id", "联盟ID"],
    ["int", "rally_type", "集结类型"],
    ["long", "leader_cid", "发起者CID"],
    ["long", "leader_team_id", "发起者队伍ID"],
    ["long", "create_time_ms", "创建时间(毫秒)"],
    ["long", "expire_time_ms", "过期时间(毫秒)"],
    ["long", "min_power_limit", "最低战力限制"],
    ["int", "max_member_limit", "最大参与人数"],
    ["bytes", "team_snapshot", "发起者队伍快照"],
    ["bytes", "ext_data", "扩展数据"],
]
key = ["guild_id"]
ukey = []
dbTag = "main"

