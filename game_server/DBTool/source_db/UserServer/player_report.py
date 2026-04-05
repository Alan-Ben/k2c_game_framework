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

tableComment = "玩家举报数据"
field = [
	["long", "cid", "玩家账号ID"],
	["long", "target_cid", "目标玩家CID"], 
	["text", "content", "举报内容"], 
	["long", "report_ms", "举报时间（毫秒）"], 
]
key = ["cid"]
ukey = []
dbTag = "main"