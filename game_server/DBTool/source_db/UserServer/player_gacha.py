# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

#支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__="mark"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "玩家抽卡权重信息"
field = [
	["long", "cid", "玩家账号ID"],
	["long",  "pool_id", "奖池id"],
	["string(1024)", "weight_info", "权重信息"],
	["string(1024)", "guarantee_info", "保底信息"],
	["int", "step", "阶段"],
	["int", "cur_step_roll_num", "当前阶段抽取次数，用于升阶"],
	["int", "cumulative_reward_point", "累计奖励点数"],
]

key = ["cid"]
ukey = [] # key，不能重复
dbTag = "main"