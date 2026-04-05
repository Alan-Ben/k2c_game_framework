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
__date__ = "$2022-04-25$"

tableComment = "玩家竞技场战报数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "opponent_cid", "对手cid"],
    ["int", "defeat_hero_num", "击败我方大臣数量"],
    ["int", "deduct_influence", "扣除影响力"],
    ["long", "timestamp", "时间戳"],
    ["bool", "from_celebrity_rand", "是否来自名人榜"],
]
key = ["cid"]
ukey = []
dbTag = "main"
