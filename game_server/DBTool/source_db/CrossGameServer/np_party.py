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

tableComment = "Party 聚会数据表"
field = [
		 ["long", "instanceId", "聚会实例ID"],
		 ["int", "serverType", "发起服务器类型"],
		 ["int", "serverTypeId", "发起服务器类型ID"],
		 ["long", "refId", "配置ID"],
		 ["long", "ownerCid", "举办玩家CID"],
		 ["string(50)", "ownerName", "举办玩家名称"],
		 ["string(200)", "declar", "宣言"],
		 ["int", "startTs", "开启时间戳（秒）"],
		 ["int", "endTs", "结束时间戳（秒）"],
		 ["int", "profitEndTs", "收益结束时间戳（秒）"],
		 ["int", "extraSeatCount", "额外扩展席位数"],
		 ["int", "sceneType", "场景类型"],
		 ["bytes(1024)", "sceneData", "场景数据"],
        ]
key = []
ukey = ["instanceId"]
dbTag = "crossgame_main"