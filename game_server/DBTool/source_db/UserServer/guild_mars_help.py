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

tableComment = "联盟成员在火星系统的互助"
field = [
    ["long", "guild_id", "联盟id"],
    ["long", "sender_cid", "发送者cid"],
    ["int", "obj_type", "互助目标实体类型"],
    ["long", "obj_id", "互助目标实体ID"],
    ["int", "deal_limit", "求助允许处理的次数上限"],
    ["int", "deal_secs", "求助扣除的时长（秒）"],
    ["bytes", "helper_cid_list", "帮助者玩家CID列表"],
    ["bytes", "ext", "目标实体额外数据"],
]

key = ["guild_id", "sender_cid"]
ukey = []  # key，不能重复
