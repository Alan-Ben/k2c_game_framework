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

tableComment = "活动排期数据"
field = [
		 ["long", "php_schedule_id", "后台排期ID"],
		 ["int", "submit_count", "提交次数"],
		 ["bool", "is_active", "是否生效"],
		 ["long", "pre_push_time_ms", "预下发时间"],
		 ["long", "activity_id", "活动id"],
		 ["long", "start_time_ms", "开始时间"],
		 ["long", "end_time_ms", "领奖时间"],
		 ["long", "close_time_ms", "结束时间"],
         ["bytes", "group_data", "分组数据 生效后按此数据构造分组"],
         ["bool", "is_inited", "是否完成初始化"],
        ]
key = []
ukey = []
dbTag = "ss_db"