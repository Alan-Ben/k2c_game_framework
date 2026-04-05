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

tableComment = "跑马灯信息-后台多语言内容"
field = [
    ["long", "marquee_dbid", "跑马灯实例id"],
    ["string", "lang", "语言ID"],
    ["bytes", "param_list", "参数列表"],
    ["string(2048)", "content", "内容"],
]
key = ["marquee_dbid"]
ukey = []  # key,不能重复
dbTag = "main"
