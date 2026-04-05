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

tableComment = "玩家主基地建筑功能数据"
field = [
    ["long", "cid", "玩家CID"],
    ["bool", "isNormalOn", "普通功率开启"],
    ["bool", "isOverdriveOn", "最高功率开启"],
    ["int", "lastCollectTimeS", "最后收集资源的时间点"]
]
key = ["cid"]
ukey = []
dbTag = "main"
