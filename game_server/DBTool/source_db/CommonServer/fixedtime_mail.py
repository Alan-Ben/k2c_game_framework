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

__author__="abe"
__date__ ="$2014-3-11 11:35:33$"

tableComment = "Method 后台推送定时邮件"
field = [
         ["long", "mailId", "php邮件ID"],
         ["string", "senderName", "发送者"],
         ["string", "title", "邮件标题"],
         ["string", "content", "邮件描述"],
         ["string", "reward", "奖励信息"],
         ["int", "beginTime", "开始时间"],
		 ["int", "hasSendNum", "已发次数"],
		 ["int", "cyclesNum", "循环次数"],
		 ["int", "exitstime", "邮件有效时长，秒"],
		 ["int", "sendType", "发送玩家类型:0全部玩家 1旧玩家 2新玩家"],
		 ["string(2048)", "channel_list", "指定渠道列表，空位全渠道"],
		 ["bool", "isSend", "列表发送或不发送"],
        ]
key = []
ukey = []

dbTag = "comm_main"