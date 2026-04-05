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

__author__ = "alzq"
__date__ = "$2023-11-21 10:17:54$"

tableComment = "加入跨服排行分组的参与者，只有参与者都退出之后分组才能关闭"
field = [
    ["long", "cross_rank_instance_id", "跨服排行榜Id"],
    ["long", "joiner_id", "参与者Id标记"],
]

key = []
ukey = []  # key，不能重复
dbTag = "crossrank_main"