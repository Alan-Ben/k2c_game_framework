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

tableComment = "玩家升级日志"
field = [
         ["long", "cid", "玩家CID"],
         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
         ["long", "ori_level", "原等级"],
         ["long", "exp", "经验"],
         ["long", "earnings", "赚速"],
         ["long", "new_level", "目标等级"],
         ["string(50)", "nation", "国家"],
         ["string(50)", "device_os", "客户端操作系统"],
         ["int", "create_role_time", "创角时间戳(10位)"],
         ["int", "create_role_date", "创角日期"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"