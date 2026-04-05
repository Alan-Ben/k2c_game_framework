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

tableComment = "活动排期US组数据"
field = [
         ["long", "schedule_db_id", "排期实例ID"],
         ["string", "res_file_name", "资源文件名"],
         ["string", "res_file_md5", "资源文件MD5"],
         ["string", "res_file_dir", "资源文件目录"],
         ["long", "cross_instance_id", "跨服实例ID"],
         ["long", "game_logic_instance_id", "游戏逻辑主体实例ID"],
         ["bool", "had_discarded", "是否已完成废弃处理"],
        ]
key = ["schedule_db_id"]
ukey = []
dbTag = "ss_db"