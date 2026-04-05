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
tableComment = "联盟集结成员数据"
field = [
    ["long", "guild_id", "联盟ID"],
    ["long", "rally_id", "集结ID"],
    ["long", "cid", "成员CID"],
    ["long", "team_id", "成员队伍ID"],
    ["long", "join_time_ms", "加入时间(毫秒)"],
    ["bool", "is_ready", "是否准备完成"],
    ["bytes", "team_snapshot", "成员队伍快照"],
]
key = ["guild_id"]
ukey = []
dbTag = "main"

