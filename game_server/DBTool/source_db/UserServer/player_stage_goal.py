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

tableComment = "玩家阶段目标数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "step", "阶段ID"],
    ["bool", "is_done", "是否已完成"],
    ["bytes", "had_draw_big_step_list", "已领取大阶段ID列表"],
    ["bytes", "had_draw_big_step_first_reach_list", "已领取大阶段首达ID列表"],
]
key = ["cid"]
ukey = []
dbTag = "main"
