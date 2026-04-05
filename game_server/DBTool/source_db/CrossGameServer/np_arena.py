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

tableComment = "比武擂台 聚会数据表"
field = [
    ["long", "instanceId", "实例id"],
    ["int", "server_type", "服务器类型"],
    ["int", "server_type_id", "服务器id"],
    ["int", "life_state", "生命周期状态"],
    ["long", "ref_id", "比武擂台配置表id"],
    ["long", "begin_time_ms", "本期开启时间"],
    ["long", "end_time_ms", "本期结束时间"],
    ["long", "clear_time_ms", "本期清空时间"],
    ["long", "last_stack_reward_settle_time_ms", "最后累计奖励结算时间"],
    ["long", "over_msg_info_id", "最后一条战斗公告消息id"],
]
key = []
ukey = ["instanceId"]
dbTag = "crossgame_main"
