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

tableComment = "梦加日志-在线人数表"
field = [
    ["int", "online_num", "在线人数"],
    ["int", "online_android", "安卓在线用户数（暂无=0）"],
    ["int", "online_ios", "IOS在线用户数（暂无=0）"],
    ["int", "timestamp", "在线人数时时间-时间戳（10位数）"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "账号归属的平台id"],
    ["string(50)", "region", "账号归属的区域id"],
    ["text", "ext", "扩展字段：json格式"],
    ["int", "date_time", "日期"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

