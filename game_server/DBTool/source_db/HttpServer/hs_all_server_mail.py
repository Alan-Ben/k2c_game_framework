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

tableComment = "http 全服邮件"
field = [
    ["long", "mail_ref_id", "邮件配表配置id"],
    ["long", "php_mail_id", "邮件配表配置id"],
    ["string(32)", "send_time", "玩家看到的邮件接收时间"],
    ["string(32)", "expired_time", "失效时间"],
    ["string(32)", "default_lang", "默认语言"],
    ["long", "passed_time_ms", "邮件审核时间，用于邮件组标识位处理"],
    ["bytes", "content_replace", "内容替换数据"],
]
key = []
ukey = []
dbTag = "hs_db"
tasktag = "hs_db"
