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

tableComment = "梦加日志-变现广告表"
field = [
    ["string(32)", "sole_id", "项目唯一角色id"],
    ["string(64)", "uid", "用户id"],
    ["long", "cid", "角色id"],
    ["string(64)", "adfrom2", "二级渠道名称：无渠道时，默认使用default"],
    ["string(64)", "nation", "国家"],
    ["string(16)", "ads_type", "广告类型"],
    ["string(16)", "game_postion", "广告位置,项目组自定义"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
    ["int", "server_id", "所在的服务器id"],
    ["int", "platform", "所在的平台id"],
    ["string(50)", "region", "所在的区域id"],
    ["text", "ext", "扩展字段：json格式"],
    ["int", "date_time", "日期"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

