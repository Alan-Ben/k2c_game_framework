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

tableComment = "大臣-大臣升级天赋技能（使用物品）"
field = [
         ["long", "cid", "玩家CID"],
         ["int", "level","玩家等级"],
         ["int", "vip_lvl","玩家vip等级"],
         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
         
         ["long", "hero_id", "大臣ID"],
         ["long", "talent_skill_id", "大臣天赋技能ID"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"