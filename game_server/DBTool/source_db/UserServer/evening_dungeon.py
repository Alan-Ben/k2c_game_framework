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

tableComment = "晚间副本"
field = [
    ["long", "round_preview_time_ms", "本轮预告时间"],
    ["long", "round_start_time_ms", "本轮开始时间"],
    ["long", "round_end_time_ms", "本轮结束时间"],
    ["long", "pre_round_close_time_ms", "上轮结束时间"],
    ["bool", "had_settle", "本轮是否结算"],
    ["int", "reborn_times", "复活次数"],
    ["long", "base_hp", "基准血量"],
    ["long", "total_hp", "总血量"],
    ["long", "deducted_hp", "扣除血量"],
    ["long", "defeat_cid", "击杀玩家ID"],
    ["long", "defeat_time_ms", "击杀时间"],
    ["long", "rank_instance_id", "排行榜实例id"],
    ["bool", "had_reset_rank", "是否重置 排行榜"],
    ["int", "server_start_day", "服务器开服天数"],
]
key = []
ukey = []  # key，不能重复
tasktag = "main"
