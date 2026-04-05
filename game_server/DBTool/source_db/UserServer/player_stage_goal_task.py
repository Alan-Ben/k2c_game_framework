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

tableComment = "玩家阶段目标任务数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "step", "阶段ID"],
    ["long", "task_id", "任务ID"],
    ["long", "counter", "当前任务计数"],
    ["bool", "reward_drawed", "任务奖励是否已领取"],
]
key = ["cid"]
ukey = []
dbTag = "main"
