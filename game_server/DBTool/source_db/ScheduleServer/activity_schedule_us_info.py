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

tableComment = "活动排期US数据"
field = [
         ["long", "schedule_db_id", "排期实例ID"],
         ["long", "us_group_db_id", "us分组实例ID"],
         ["int", "us_id", "usId"],
         ["bool", "had_push", "是否已推送" ],
         ["bool", "had_enter_settle", "是否进入结算状态" ],
         ["bool", "had_done", "是否完成排期" ],
         ["bool", "had_enter_playing", "是否进入开启状态" ],
        ]
key = ["schedule_db_id"]
ukey = []
dbTag = "ss_db"