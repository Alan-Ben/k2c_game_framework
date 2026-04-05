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

tableComment = "User Bag Item Info 玩家背包物品信息表"
field = [
         ["long", "cid", "玩家账号ID"],
		 ["long", "itemId", "物品ID"],
		 ["long", "itemCount", "物品数量"],
		 ["int", "lastGetTimeS", "最后一次获取时间"],
		 ["int", "newGetTimeS", "首次获取时间"],
		 ["int", "lastClickTimeS", "最后一次点击时间"],
         ["long", "total_gain_count", "总获得数量"],
	     ["long", "total_consume_count", "总消耗数量"],
	     ["long", "relative_activity_instance_id", "关联的活动实例ID"],
        ]
		
key = ["cid"]
ukey = [] #key,不能重复
dbTag = "main"