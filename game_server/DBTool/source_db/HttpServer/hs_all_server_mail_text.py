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

tableComment = "http 全服邮件-文本数据"
field = [
    ["long", "mail_db_id", "邮件唯一id"],
    ["string(32)", "lang", "语言编号(查看公共参数中的语言ID对应表)"],
    ["string(128)", "title", "对应语种的邮件标题"],
    ["string(1024)", "content", "对应语种的邮件内容"],
]
key = ["mail_db_id"]
ukey = []
dbTag = "hs_db"
tasktag = "hs_db"
