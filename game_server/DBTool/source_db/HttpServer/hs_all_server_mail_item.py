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

tableComment = "http 全服邮件-附件"
field = [
    ["long", "mail_db_id", "邮件唯一id"],
    ["int", "item_type", "物品类型"],
    ["long", "sub_id", "物品子ID"],
    ["long", "item_count", "物品数量"],
]
key = ["mail_db_id"]
ukey = []
dbTag = "hs_db"
tasktag = "hs_db"
