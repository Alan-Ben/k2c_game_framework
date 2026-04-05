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

tableComment = "联姻池请求数据"
field = [
    ["long", "applyAdultId", "请求子嗣实例ID"],
    ["long", "applyCid", "请求玩家CID"],
    ["string", "applyCname", "请求玩家昵称"],
    ["long", "initResId", "请求子嗣初始形象配置，用于确认子嗣形象列表"],
    ["long", "quality", "请求子嗣品质"],
    ["int", "attrType", "请求子嗣相性"],
    ["long", "career", "请求子嗣职业"],
    ["bool", "isGiftde", "是否卷王"],
    ["string", "name", "请求子嗣名称"],
    ["long", "bonus", "子嗣收益"],
    ["bytes", "marriedItem", "联姻奖励"],
    ["int", "applyExpiredTs", "发起请求截至时间戳（秒）"],
    ["long", "minBonus", "对方子嗣允许的最小收益"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"
