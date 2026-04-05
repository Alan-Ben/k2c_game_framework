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
__date__ = "$2025-08-19$"

tableComment = "礼包生命周期数据"
field = [
    ["long", "gift_pack_id", "礼包ID"],
]
key = []  # key
ukey = []  # unique key
dbTag = "main"