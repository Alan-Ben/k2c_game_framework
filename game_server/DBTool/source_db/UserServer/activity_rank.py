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

tableComment = "活动排行榜数据"
field = [
    ["long", "instance_id", "活动实例ID"],
    ["long", "rank_instance_id", "排行榜实例ID"],
    ["long", "rank_id", "排行榜配置ID"],
    ["bool", "can_draw_reward", "是否可以领取奖励"],
    ["bool", "had_deal_first", "是否已处理第一名"],
    ["bytes", "rewarded_cid_set", "已领取奖励的玩家CID集合"],
]

key = []
ukey = []  # key，不能重复
