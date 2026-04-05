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

__author__="mark"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "User Info 玩家头像框数据"
field = [
         ["long", "cid", "玩家CID"],
         ["long", "iconBgkId", "头像框ID"],
         ["int","lvl","头像框等级"],
         ["int","expireTimeS","超时时间戳，0一下表示永久"],
         ["bool","viewed","是否已查看"],
         ["bool","need_check_send_mail","用于检查是否需要发送过期提醒邮件"],
        ]
key = ["cid"]
ukey = [] # key，不能重复
tasktag = "main"