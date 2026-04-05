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

tableComment = "玩家午间副本信息"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "round_start_time_ms", "副本开始时间"],
    ["int", "wave", "波次"],
    ["long", "deducted_hp", "当前波次已扣除血量"],
    ["bytes", "had_fight_hero_list", "已战斗英雄列表"],
    ["bytes", "had_borrow_hero_list", "已借用英雄列表"],
]
key = ["cid"]
ukey = []  # key，不能重复
tasktag = "main"
