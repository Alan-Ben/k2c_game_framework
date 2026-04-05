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

tableComment = "比武擂台 战斗公告表"
field = [
    ["long", "instanceId", "实例id"],
    ["long", "cid", "玩家id"],
    ["int", "type", "公告类型"],
    ["long", "timeMs", "发布时间戳"],
    ["string(128)", "paramList", "公告参数"],
    ["int", "rankBefore", "挑战前排位"],
    ["int", "rankNow", "现在排位"],
]
key = ["instanceId"]
ukey = []
dbTag = "crossgame_main"
