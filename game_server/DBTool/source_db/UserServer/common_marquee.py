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

tableComment = "跑马灯信息"
field = [
    ["long", "ref_id", "跑马灯配置id"],
    ["long", "php_id", "跑马灯后台id"],
    ["long", "create_time_ms", "生成时间"],
    ["long", "expired_time_ms", "过期时间"],
    ["bytes", "param_list", "参数列表"],
    ["int", "show_pos_id", "窗口展示队列"],
    ["int", "priority_id", "优先级"],
    ["int", "duration_sec", "循环播放时长秒"],
    ["int", "duration_count", "循环播放次数"],
    ["long", "ui_res_id", "预制体ID"],
    ["string(2048)", "content", "内容"],
    ["int", "can_del_type", "是否可删除类型"],
    ["bool", "offline_need_show", "玩家离线期间是否需要展示"],
    ["string", "defaultLang", "默认语言"],
    ["bytes", "channelList", "渠道列表"],
    ["string(2048)", "showCondition", "展示条件"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"
