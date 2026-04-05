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

classAnnotation = "isIdAuto = true" # 类注解 
tableComment = "本服火星矿产占领玩家数据"
field = [
    ["long", "mineInstaceId", "矿实例ID"],
    ["long", "cid", "玩家CID"],
    ["long", "startCollectMs", "玩家开始采集时间（毫秒）"],
    ["long", "collectSpeed", "玩家采集速度"],
    ["varbinary(1024)", "occupyPlayer", "占领玩家"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"