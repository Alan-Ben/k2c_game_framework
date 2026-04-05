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

tableComment = "玩家成年子嗣结婚目标数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "adultId", "子嗣实例ID"],
    ["long", "marriedCid", "结婚玩家CID"],
    ["long", "marriedAdultId", "结婚子嗣实例ID"],
    ["long", "initResId", "结婚子嗣初始形象配置，用于确认子嗣形象列表"],
    ["long", "quality", "结婚子嗣品质"],
    ["int", "attrType", "结婚子嗣相性"],
    ["long", "career", "结婚子嗣职业"],
    ["bool", "isGiftde", "是否卷王"],
    ["string", "name", "结婚子嗣名称"],
    ["long", "bonus", "收益总和"],
    ["int", "marriedTs", "结婚时间戳（秒）"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
