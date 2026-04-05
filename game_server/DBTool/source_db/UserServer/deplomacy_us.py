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

__author__="alzq"
__date__ ="$2026-2-4 10:17:54$"

tableComment = "建交USd信息"
field = [
			["int", "us_id", "usId"],
			["long", "build_time_ms", "建交时间"],
        ]
key = []
ukey = []
dbTag = "main"