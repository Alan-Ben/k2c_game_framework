# coding=utf-8
# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "cooper"
__date__ = "$2026-03-02 00:00:00$"

tableComment = "玩家情人收集数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "target_lover_id", "选中的情人配置ID，0=未选择"],
    ["bool", "is_claimed", "是否已领取当前目标情人"],
]

key = ["cid"]
ukey = []
dbTag = "main"
