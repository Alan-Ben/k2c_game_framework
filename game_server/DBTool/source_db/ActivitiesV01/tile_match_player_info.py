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

# 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "mark"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "三消游戏玩家数据"
field = [
    ["long", "activity_instance_id", "活动实例id"],
    ["long", "cid", "玩家ID"],
    ["long", "total_score", "总分"],
    ["int", "step_reward_step", "阶段奖励阶段"],
    ["long", "step_reward_score", "阶段奖励分数"],
    ["string(1024)", "can_draw_step_reward_list", "可领取的阶段奖励列表"],
]
key = ["activity_instance_id"]
ukey = []  # key，不能重复
dbTag = "main"
