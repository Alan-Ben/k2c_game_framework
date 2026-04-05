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

__author__="yoey"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "user服务器参数信息"
field = [
		 ["int", "tag", "参数标记"],
		 ["long", "param", "参数值"],
        ]
key = ["tag"]
ukey = []
tasktag = "main"