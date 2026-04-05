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

__author__ = "scott"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家钻石充值记录"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "ref_id", "配表id"],
    ["int", "buy_count", "购买次数"],
]
key = ["cid"]  # key
ukey = []  # key,不能重复
dbTag = "main"
