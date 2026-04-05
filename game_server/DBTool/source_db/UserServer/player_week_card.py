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
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家周卡数据"
field = [
    ["long", "cid", "玩家CID"],
	["long", "active_time_ms", "生效时间戳"], 
	["long", "expire_time_ms", "有效截止时间戳"], 
	["bool", "had_use_free_trial", "是否使用过免费试用"], 
	["int", "npc_type", "npc类型"], 
	["long", "npc_id", "npcID"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
