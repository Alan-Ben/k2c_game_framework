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
__date__ ="$2017-08-04$"

tableComment = "玩家延时CD"
field = [
	["long", "cid", "玩家CID"],
	["int", "cd_id", "CD类型ID"],
	["long", "last_calc_time", "最近结算时间，ms"],
	["long", "full_get_next_cd_remain_time_ms", "CD已满时，获得下一点CD所需时间，ms"],
	["int", "count", "cd计数"],
    ["int", "overflow_count", "溢出cd计数"],
    ["long", "relative_activity_instance_id", "关联的活动实例ID"],
]

key = ["cid", "cd_id"]
ukey = []
dbTag = "main"