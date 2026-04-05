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

__author__="scott"
__date__ ="2022年3月24日"

tableComment = "Mail 玩家Mail数据"
field = [
         ["long", "cid", "玩家CID"],
         ### 实例ID存在内存中，不再BO中储存 ["long","mail_uid","邮件实例唯一id"],
         ["long","mail_ref_id","邮件配表ID"],
         ["long","php_mail_id","后台邮件ID"],
         ["int","sender_id","发送者id，0-客户端读取系统配置"],
         ["bool","is_locked","是否已锁定"],
         ["string(100)","title","邮件标题（非配置邮件）"],
         ["text","content","邮件内容（非配置邮件）"],
         ["text","contentReplace","邮件内容（替换部分）"],
         ["varbinary(10240)", "attachList", "邮件附件"],
         ["int","createdTs","创建时间（秒）"],
         ["int","readedTs","读取时间（秒）"],
         ["int","takedTs","领取礼包（秒）"],
         ["int","expiredTs","过期时间（秒）"],
         ["int","effectSecs","有效时长（秒）"],
         ["bool","is_must_read","是否必读"],
         ["int","language","当前邮件语言"],
         ["int","exDataType","额外数据类型"],
         ["bytes","exData","额外数据"],
         ["bool","is_read_over","是否已读完"],
         ["bytes","exTitleData","额外标题数据"],
]
key = ["cid"]
ukey = []
dbTag = "main"