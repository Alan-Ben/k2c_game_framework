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

tableComment = "联盟数据"
field = [
    ["long", "guild_id", "联盟id"],
    ["long", "flag_id", "旗帜id"],
    ["string(512)", "name", "名称"],
    ["string(128)", "simple_name", "简称"],
    ["string(1024)", "declaration", "宣言"],
    ["string(1024)", "announcement", "公告"],
    ["int", "level", "等级"],
    ["long", "exp", "经验"],
    ["int", "join_type", "加入类型"],
    ["bytes", "join_limit_info", "加入限制信息"],
    ["bool", "is_dissovle", "是否解散"],
    ["long", "wealth", "联盟财富"],
    ["bytes", "construct_list", "建造信息"],
    ["long", "last_proactive_trans_leader_time_ms", "上次主动转让盟主时间"],
    ["long", "next_open_recruit_time_ms", "下一次公开招募时间"],
    
    ["long", "entrust_ref_id", "委托配表id"],
    ["long", "entrust_event_id", "委托事件id"],
    ["string(512)", "entrust_weight_base_list", "委托权重依据列表"],
    ["int", "entrust_point", "委托点数"],

    ["int", "week_tag", "周标记"],
    ["int", "weekly_great_reward_point", "周清空 大礼进度"],
]

key = ["guild_id"]
ukey = []  # key，不能重复
