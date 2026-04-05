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

tableComment = "防守玩家战报数据"
field = [
    ["long", "cid", "玩家cid"],
    ["long", "attackerCid", "攻击玩家CID"],
    ["long", "chapterId", "楼层ID"],
    ["int", "chapterLevel", "楼层等级"],
    ["bool", "isSucc", "攻击是否成功"],
    ["int", "curChapterLevel", "当前楼层等级"],
    ["long", "timestamp", "时间戳 毫秒"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
