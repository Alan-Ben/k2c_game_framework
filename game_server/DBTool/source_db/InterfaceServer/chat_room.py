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

__author__="yoey"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "IS 聊天房间数据"
field = [
		 ["int", "server_type", "聊天房间所在服务器ID"],
		 ["int", "server_type_id", "聊天房间所在服务器类型ID"],
		 ["int", "room_type", "房间类型"],
		 ["long", "room_type_id", "房间类型ID（比如联盟聊天中的联盟ID）"],
		 ["int", "sdk_room_id", "聊天服务器赋予的聊天房间唯一ID"],
        ]
key = []
ukey = []
dbTag = "is_db"
tasktag = "is_db"