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
__date__ = "$2026-2-24 10:17:54$"

tableComment = "玩家大臣列表缓存数据"
field = [
    ["long", "cid", "玩家CID"],
    ["bytes", "hero_list", "大臣列表数据"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
