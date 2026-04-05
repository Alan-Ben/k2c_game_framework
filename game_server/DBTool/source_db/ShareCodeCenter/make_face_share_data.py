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

classAnnotation = "isIdAuto = false" # 类注解 
tableComment = "捏脸分享数据"
field = [
    ["long", "serial", "实际分享id"],
    ["bytes", "data", "数据"],
    ["long", "timestamp", "生成时间戳"],
]

key = []
ukey = []  # key，不能重复
dbTag = "scc_db"