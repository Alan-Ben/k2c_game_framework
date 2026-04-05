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

tableComment = "Mail Log 邮件数据日志表"
recordExpiredSec = 3600*24*30*3
field = [
         ["long", "cid", "玩家CID"],
         ### 实例ID存在内存中，不再BO中储存 ["long", "mail_uid","邮件数据ID"],
         ["int", "logType","日志类型，增删改"],
         ["long", "mail_ref_id","配置邮件ID"],
         ["long", "phpMailId","运营邮件ID"],
         ["long", "expiredTimeSec","过期时间戳（秒）"],
         ["long", "curMaxGMailId","此时邮件分界线"],
         ["string(20)", "playerLanguage","玩家语言"],
         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"