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
__date__ = "$2025-01-01$"

tableComment = "梦加日志-子嗣数据表"
field = [
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],
    
    ["long", "uniqe_id", "子嗣唯一ID"],
    ["long", "child_id", "子嗣ID"],
    ["long", "attribute", "子嗣属性"],
    ["long", "loversid", "子嗣对应情人"],
    ["long", "child_ep", "子嗣培养进度"],
    ["int", "stateid", "子嗣状态"],
    ["int", "sex_id", "子嗣性别"],
    ["long", "quality", "子嗣品质"],
    ["int", "time", "获得时间戳"],
    
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
