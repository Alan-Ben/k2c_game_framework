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

tableComment = "活动阶段奖励数据"
field = [
    ["long", "instance_id", "活动实例ID"],
    ["long", "step_reward_id", "阶段奖励配置ID"],
    ["long", "step_reward_instance_id", "阶段奖励实例ID"],
]

key = []
ukey = []  # key，不能重复
