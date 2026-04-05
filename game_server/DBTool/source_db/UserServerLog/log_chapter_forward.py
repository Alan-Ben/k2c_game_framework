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

tableComment = "关卡前进日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["long", "cid", "玩家CID"],
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["int", "level", "等级"],
    ["long", "chapter_id", "章节ID"],
    ["int", "point", "点位"],
    ["long", "power", "当前战力"],
    ["long", "earnings", "当前赚速"],
    ["bool", "is_critical_hit", "是否暴击"],
    ["long", "gold_cost", "消耗金币"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
