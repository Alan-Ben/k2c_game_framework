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
__date__ = "$2022-04-25$"

tableComment = "玩家火星探险队伍数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "teamId", "队伍ID"],
    ["int", "curState", "当前状态"],
    ["string", "name", "队伍名称"],
    ["bytes", "heroIdList", "入驻大臣ID列表"],
    ["long", "curStateStartMs", "当前状态起始时间（毫秒）"],
    ["long", "curStateKeepTimeMS", "当前状态持续时长（毫秒）"],
    ["long", "targetPos", "目标位置"],
    ["bytes", "extData", "当前状态额外数据"],
    ["long", "lossValue", "队伍损耗数量"],
]
key = ["cid"]
ukey = []
dbTag = "main"
