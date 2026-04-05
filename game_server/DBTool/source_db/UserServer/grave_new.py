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

tableComment = "新晋杰出者数据"
field = [
    ["long", "titleId", "称号ID"],
    ["long", "cid", "玩家CID"],
    ["long", "endMs", "结束时间，时间戳：毫秒"],
    ["bytes", "congCids", "注册玩家CID集合"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"
