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

tableComment = "玩家居民建筑功能数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "buildingId", "建筑ID"],
    ["int", "dispatchedNum", "已派遣居民数量"],
    ["long", "energy", "累积的能量数值"],
    ["long", "energyLastCalMs", "能量数值最后一次计算时间（毫秒）"],
]
key = ["cid"]
ukey = []
dbTag = "main"
