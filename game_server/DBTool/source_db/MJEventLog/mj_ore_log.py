# coding=utf-8
# 矿石处理及升级日志

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "cooper"
__date__ = "$2025-11-01$"

tableComment = "梦加日志-矿石处理记录"
field = [
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],
    ["long", "ore_id", "矿石id（配表id）"],
    ["int", "ore_qa", "矿石品质"],
    ["string(1000)", "research_point", "获得的养成点数"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
