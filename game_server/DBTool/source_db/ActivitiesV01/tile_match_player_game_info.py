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

tableComment = "三消游戏玩家游戏数据"
field = [
    ["long", "activity_instance_id", "活动实例id"],
    ["long", "cid", "玩家ID"],
    ["int", "mode_type", "游戏类型 ETileMatch_ModeType"],
    ["bytes", "block_list", "三消方格列表"],
    ["long", "task_id", "任务ID"],
    ["int", "had_go_step", "已经走的步数"],
    ["string(1024)", "task_block_list", "任务方块计数列表"],
]
key = ["activity_instance_id"]
ukey = []  # key，不能重复
dbTag = "main"
