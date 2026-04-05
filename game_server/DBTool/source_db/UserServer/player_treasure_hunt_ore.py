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

tableComment = "玩家太空寻宝矿石数据"
field = [
    ["long", "cid", "玩家CID"],
	["long", "ore_id", "矿石ID"], 
	["long", "first_gain_time_ms", "首次获得时间 ms"], 
	["int", "max_record", "最大记录"], 
	["long", "reach_max_record_time_ms", "达到最大记录时间 ms"], 
	["bool", "had_reach_advanced", "是否已达到高级矿石"], 
	["string(256)", "had_draw_record_reward_list", "已领取记录档位奖励列表"], 
	["int", "total_gain_num", "总获得数量"], 
	["int", "normal_pending_num", "普通矿石待处理数量"], 
	["int", "advanced_pending_num", "高级矿石待处理数量"], 
	["int", "normal_skill_level", "普通技能等级"], 
	["int", "normal_skill_point", "普通技能点数"], 
	["int", "advanced_skill_level", "高级技能等级"], 
	["int", "advanced_skill_point", "高级技能点数"], 
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
