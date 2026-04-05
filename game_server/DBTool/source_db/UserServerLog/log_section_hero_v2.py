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

tableComment = "伙伴截面数据"
field = [
    #### 截面日志类型
    ["int", "sectionType", "截面日志类型，枚举ELogSectionType"],
    
    #### 日志表通用字段
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "cid", "玩家CID"],
    ["long", "heroId", "骑士ID"],
    ["int", "heroLvl", "等级"],
    ["long", "heroTalent", "资质"],
    ["long", "heroPower", "战力"],
    ["text", "heroBusinessSkillLvl", "经营技能等级"],
    ["long", "equipID", "佩戴的藏品id"],
    ["int", "equipLvl", "佩戴的藏品等级"],
]
key = ["sectionType"]
ukey = []  # key，不能重复
dbTag = "us_log"
