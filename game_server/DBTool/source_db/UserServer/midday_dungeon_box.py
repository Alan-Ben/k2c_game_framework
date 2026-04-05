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

tableComment = "副本宝箱信息"
field = [
    ["long", "box_ref_id", "宝箱ID"],
    ["long", "sender_cid", "发送者ID"],
    ["string", "sender_name", "发送者名称"],
    ["long", "expired_ms", "过期时间"],
    ["bytes", "had_draw_cid_list", "已领取的玩家列表"],
]
key = []
ukey = []  # key，不能重复
tasktag = "main"
