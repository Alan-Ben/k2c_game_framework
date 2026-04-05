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

__author__="cooper"
__date__ ="$2026-02-06$"

tableComment = "User Info 玩家房间皮肤数据"
field = [
         ["long", "cid", "玩家CID"],
         ["long","roomSkinId","房间皮肤ID"],
         ["int","expireTimeS","超时时间戳，0以下表示永久"],
         ["bool","viewed","是否已查看"],
         ["bool","need_check_send_mail","用于检查是否需要发送过期提醒邮件"],
        ]
key = ["cid"]
ukey = [] # key，不能重复
dbTag = "main"
