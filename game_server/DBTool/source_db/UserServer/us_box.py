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

tableComment = "User Info 通用宝箱数据"
field = [
         ["long", "instanceId", "宝箱实例ID"],
         ["long", "refId", "宝箱配置ID"],
         ["long", "createdAt", "创建时间（毫秒）"],
         ["long", "senderCid", "发送玩家CID"],
		 ["varbinary(1024)", "gainedCidList", "领取玩家CID列表"],
        ]
key = []
ukey = ["instanceId"] # key，不能重复
tasktag = "main"