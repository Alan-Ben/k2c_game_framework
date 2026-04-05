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

tableComment = "梦加日志-物品日志表"
field = [
    ["string(32)", "sole_id", "项目唯一角色id"],
    ["string(64)", "uid", "用户id"],
    ["long", "cid", "角色id"],
    ["int", "itemType", "物品类型"],
    ["int", "itemId", "物品id"],
    ["int", "action", "1获得,2消耗,3无效丢失,4过期"],
    ["double", "oldNumber", "旧值"],
    ["double", "exchange", "改变值"],
    ["double", "finalNumber", "最终值"],
    ["string(100)", "item", "物品"],
    ["int", "event", "事件"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
    ["int", "server_id", "所在的服务器id"],
    ["int", "platform", "所在的平台id"],
    ["string(50)", "region", "所在的区域id"],
    ["int", "level", "玩家等级"],
    ["text", "ext", "扩展字段：json格式"],
    ["int", "date_time", "日期"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

