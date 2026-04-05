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

__author__ = "yoey"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "User Info 角色冻结信息"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "freeze_time_ms", "冻结结束时间戳"],

]

key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
