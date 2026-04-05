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

tableComment = "玩家联盟数据"
field = [
    ["long", "cid", "玩家id"],
    ["long", "guild_id", "玩家所属公会Id"],
    ["long", "dispatch_hero_id", "玩家向公会派遣的大臣Id"],
    ["int", "free_cd_join_guild_num", "免cd加入联盟次数"],
    ["long", "join_guild_cd_end_time_ms", "加入联盟cd结束时间戳"],

    ["int", "day_tag", "上次重置每日数据的日期"],
    ["bytes", "day_had_draw_construct_reward_list", "今天已领取建设奖励列表"],
]

key = ["cid"]
ukey = []  # key，不能重复
  