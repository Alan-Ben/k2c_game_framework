# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

#支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__="mark"
__date__ ="$2017-08-04$"

tableComment = "火星-火星科技数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "teamId", "玩家队伍ID"],
    ["long", "refId", "采集矿配置ID"],
    ["long", "resNum", "采集资源数量"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"