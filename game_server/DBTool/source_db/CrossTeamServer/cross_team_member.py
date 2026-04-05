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

__author__ = "mark"
__date__ = "$2023-11-21 10:17:54$"

tableComment = "跨服队伍成员数据"
field = [
    ["long", "team_id", "队伍实例ID"],
    ["long", "member_cid", "队伍成员CID"],
    ["int", "member_pos", "队伍成员职位"],
    ["long", "join_ms", "加入队伍时间（毫秒）"],
]

key = ["team_id"]
ukey = []  # key，不能重复
dbTag = "crossteam_main"