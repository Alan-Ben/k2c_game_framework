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

tableComment = "玩家旅店 客人数据"
field = [
    ["long", "cid", "玩家CID"],
	["long", "guest_id", "客人ID"], 
	["bool", "had_draw_handbook_reward", "是否已领取图鉴奖励"], 
    ["long", "start_line_up_id", "开始排队的id"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
