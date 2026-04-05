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

classAnnotation = "isIdAuto = false" # 类注解 
tableComment = "玩家子嗣数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "consortId", "关联家人ID"],
    ["long", "initIntimacy", "初始亲密度"],
    ["long", "initResId", "子嗣初始形象配置"],
    ["long", "quality", "子嗣品质"],
    ["int", "attrType", "子嗣相性"],
    ["long", "career", "子嗣职业"],
    ["long", "seatId", "训练房ID"],
    ["bool", "isGiftde", "是否卷王"],
    ["int", "initStudyBonus", "教学经验加成（万分比）"],
    ["string", "name", "子嗣名称"],
    ["int", "lvl", "子嗣等级"],
    ["long", "baseBonus", "子嗣基础收益"],
    ["long", "trainBonus", "子嗣上课收益"],
    ["int", "createdAt", "创建时间（秒）"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
