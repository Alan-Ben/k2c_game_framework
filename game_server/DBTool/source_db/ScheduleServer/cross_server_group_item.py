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

tableComment = "跨服分组对象"
field = [
		 ["long", "group_id", "分组id"],
		 ["string(2048)", "us_id_list", "服务器列表"],
		 ["long", "crs_group_instance_id", "跨服分组排行实例"],
		 ["bool", "has_push", "是否推送"],
        ]
key = []
ukey = []
dbTag = "ss_db"