# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "alzq"
__date__ = "$2023-11-21 10:17:54$"

tableComment = "跨服队伍数据"
field = [
    ["long", "team_id", "队伍实例ID"],
    ["int", "member_limit", "队伍成员上限"],
    ["int", "join_type", "加入方式（ENPCrossTeamJoinType枚举序号）"],
    ["int", "join_cond_type", "加入条件类型（ENPCrossTeamApplyCond枚举序号）"],
    ["long", "join_cond_value", "加入条件数值"],
    ["string", "team_name", "队伍名称"],
    ["string(1024)", "team_dec", "队伍宣言"],
]

key = ["team_id"]
ukey = []  # key，不能重复
dbTag = "crossteam_main"
