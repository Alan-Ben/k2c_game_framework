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
__date__ =""

tableComment = "排行榜子数据"
field = [
    ["long", "instanceId", "排行榜实例ID"],
	["long", "rankId", "排行榜配置ID"],
	["long", "objId", "排行榜对象ID"],
	["long", "subObjId", "排行榜对象的子对象ID"],
    ["long", "score_source_id", "排行榜分数来源id"],
	["long", "score", "排行榜分数"],
	["long", "updatedMs", "排行榜变更时间戳"],
]
		
key = ["instanceId"]
ukey = [] #key,不能重复
dbTag = "main"