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
__date__ ="2022年3月24日"

tableComment = "Offline Reward 玩家离线奖励数据"
field = [
         ["long", "cid", "玩家CID"],
         ["int", "rewardType", "类型ID"],
         ["bytes", "offline_data", "离线数据"],
         ["bytes", "item_list", "物品列表"],
         ["bytes", "reward_show", "奖励展示"],
         ["bool", "has_pre_deal", "是否已经预处理"],
]
key = ["cid"]
ukey = []
dbTag = "main"