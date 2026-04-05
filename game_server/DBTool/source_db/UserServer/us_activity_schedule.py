# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "mark"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "服务器排期数据"
field = [
    ["long", "schedule_id", "排期ID"],
    ["long", "us_group_id", "US分组ID"],
    ["long", "cross_instance_id", "跨服实例ID"],
    ["long", "game_logic_instance_id", "游戏逻辑主体实例ID"],
    ["long", "activity_id", "活动id"],
    ["long", "start_time_ms", "开始时间"],
    ["long", "end_time_ms", "领奖时间"],
    ["long", "close_time_ms", "结束时间"],
    ["bool", "is_registered", "已注册"],
    ["bool", "is_done", "活动已完成"],
    ["bytes", "us_id_list", "usId列表"],
    ["string(1024)", "res_file_name", "资源文件名"],
    ["string(1024)", "res_file_md5", "资源文件md5"],
    ["string(1024)", "res_file_dir", "资源文件目录"],
    ["long", "activity_instance_id", "活动实例id"],
    ["int", "submit_count", "提交次数"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"
