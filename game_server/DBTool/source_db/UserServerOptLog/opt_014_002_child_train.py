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

tableComment = "子嗣-训练子嗣"
field = [
         ["long", "cid", "玩家CID"],
         ["int", "level","玩家等级"],
         ["int", "vip_lvl","玩家vip等级"],
         ["int", "event_id", "事件类型"],
         ["long", "guid", "事件唯一id"],
         ["int", "date_time", "日期"],
         ["int", "timestamp", "时间戳"],
         
         ["long", "childId", "子嗣实例ID"],
         ["int", "oriLvl", "子嗣原先等级"],
         ["int", "curLvl", "子嗣当前等级"],
         ["long", "costSilver", "子嗣扣除的金币"],
         ["long", "addHeroExp", "子嗣增加的大臣经验"],
         ["long", "addBonus", "子嗣增加的收益"],
        ]
key = []
ukey = [] # key，不能重复
dbTag = "us_log"