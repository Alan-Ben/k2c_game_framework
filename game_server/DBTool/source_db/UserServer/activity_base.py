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

tableComment = "活动基础数据"
field = [
    ["long", "activity_id", "活动配置ID"],
    ["long", "cross_instance_id", "跨服分组实例ID"],
    ["long", "game_logic_instance_id", "游戏逻辑主体实例ID"],
    ["long", "start_ms", "开启时间戳（毫秒）"],
    ["long", "end_ms", "结束时间戳（毫秒）"],
    ["long", "settle_ms", "结算时间戳（毫秒）"],
    ["long", "close_ms", "关闭时间戳（毫秒）"],
    ["int", "cur_state", "当前状态"],
    ["bytes", "settled_guild_leaders_set", "结算时当前US联盟盟主CID集合"],
    ["bytes", "settled_activity_team_leaders_set", "结算时当前US队伍CID集合"],
]

key = []
ukey = []  # key，不能重复
