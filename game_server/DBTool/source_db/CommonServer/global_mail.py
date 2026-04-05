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

__author__="yoey"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "User Info 全局邮件"
field = [
         ["string", "senderName", "发送者名称"],
         ["string(200)", "title", "邮件标题"],
         ["string(500)", "content", "邮件描述"],
         ["int", "createTime", "邮件发送时间(s)"],
		 ["int", "existTime", "邮件生存时间(s)"],
         ["string", "reward", "奖励信息"],
		 ["int", "sendType", "发送玩家类型0全部玩家1旧玩家2新玩家"],
		 ["string(2048)", "channel_list", "指定渠道列表，空位全渠道"],
		 ["bool", "isSend", "列表发送或不发送"],
        ]
key = []
ukey = [] # key，不能重复

dbTag = "comm_main"