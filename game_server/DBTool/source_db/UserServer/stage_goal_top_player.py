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

__author__ = "claude"
__date__ = "$2025-9-8 10:00:00$"

tableComment = "玩家大阶段目标最强记录"
field = [
    ["long", "step", "阶段ID"],
    ["long", "cid", "玩家CID"],
]

key = []
ukey = []  # key，不能重复

dbTag = "main"