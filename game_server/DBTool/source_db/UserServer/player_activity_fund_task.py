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

__author__ = "claude"
__date__ = "$2026-1-14$"

tableComment = "玩家活动基金任务数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "fund_record_id", "基金记录ID（player_activity_fund表的主键id）"],
    ["long", "task_id", "任务ID"],
    ["long", "current_count", "当前计数"],
    ["int", "finished_times", "已完成次数"],
]
key = ["cid"]
ukey = []
dbTag = "main"
