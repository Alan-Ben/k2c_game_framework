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

__author__="scott"
__date__ ="$2020-7-14 10:17:54$"

tableComment = "全局参数"
field = [
			["int", "type", "索引"],
			["long", "v", "值"],
        ]
key = []
ukey = []
dbTag = "comm_main"