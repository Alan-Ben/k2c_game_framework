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

tableComment = "联盟成员数据"
field = [
    ["long", "guild_id", "联盟id"],
    ["long", "cid", "玩家id"],
    ["int", "position", "职位"],
    ["long", "total_contribute", "总贡献度"],
    ["int", "total_deal_entrust_num", "总处理委托数"],
    ["bool", "is_online", "上次在线时间"],
    ["long", "last_report_online_timestamp", "上次上报在线时间"],

    ["long", "last_reset_data_timestamp", "上次重置数据时间"],
    ["bytes", "past_day_contribute", "历史贡献度记录"],
    ["int", "day_deal_entrust_num", "当日处理委托数"],
    
    ["long", "last_active_guild_box", "最后一个被标记的宝箱实例ID"],
    ["long", "last_free_guild_box", "最后一个被标记的宝箱实例ID"],
    ["long", "last_gift_guild_box", "最后一个被标记的宝箱实例ID"],
    ["bool", "is_guild_box_share_anonymous", "分享宝箱匿名"],
]

key = ["guild_id"]
ukey = []  # key，不能重复
