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

tableComment = "玩家活动基金数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "fund_id", "基金ID"],
    ["long", "activity_instance_id", "活动实例ID（常驻基金为0）"],
    ["long", "formula_score", "公式分数"],
    ["long", "task_score", "任务分数"],
    ["int", "drawn_free_steps", "已领取免费档最大阶段"],
    ["int", "drawn_paid_steps", "已领取付费档最大阶段"],
    ["long", "activity_start_time_ms", "活动开始时间（毫秒，永久基金为0）"],
    ["long", "last_refresh_round", "上次刷新轮次号"],
]
key = ["cid"]
ukey = []
dbTag = "main"
