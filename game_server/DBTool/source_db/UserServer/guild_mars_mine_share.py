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

__author__ = "cooper"
__date__ = "$2025-01-15$"

tableComment = "联盟分享矿数据"
field = [
    ["long", "guild_id", "联盟ID"],
    ["long", "finder_cid", "发现者CID"],
    ["long", "mine_instance_id", "矿实例ID"],
    ["long", "mine_end_show_ms", "矿有效期毫秒时间戳"],
    ["long", "pos_id", "矿位置ID"],
]

key = ["guild_id"]
ukey = []  # key，不能重复
