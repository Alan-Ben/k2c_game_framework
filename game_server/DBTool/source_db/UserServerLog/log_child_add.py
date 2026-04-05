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

__author__ = "mark"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家获取未成年子嗣日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["long", "cid", "玩家CID"],
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "childId", "子嗣实例ID"],
    ["long", "consortId", "关联妃子ID"],
    ["long", "initRes", "初始形象"],
    ["long", "quality", "子嗣品质"],
    ["int", "attr", "子嗣相性"],
    ["long", "career", "子嗣职业"],
    ["long", "seatId", "训练席位"],
    ["bool", "isGiftde", "是否卷王"],
    ["long", "baseBonus", "基础收益"],
    ["long", "initStudyBonus", "教学经验加成（万分比）"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
