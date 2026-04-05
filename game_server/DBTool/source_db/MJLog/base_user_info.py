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

tableComment = "梦加日志-用户信息表"
field = [
    ["string(32)", "sole_id", "项目唯一角色id"],
    ["string(64)", "uid", "用户id"],
    ["long", "cid", "角色id"],
    ["string(50)", "name", "角色名"],
    ["int", "create_time", "创建-时间戳（10位）"],
    ["int", "ar_time", "帐号注册-时间戳（10位）"],
    ["string(50)", "ar_ip", "账号注册ip"],
    ["string(300)", "afid", "广告afid"],
    ["string(50)", "adid", "设备id"],
    ["string(64)", "version", "玩家登录时的客户端版本号"],
    ["string(100)", "nation", "国家(使用国际通用缩写英文比如：中国CN、加拿大CD)"],
    ["string(50)", "adfrom", "一级渠道：无渠道时，苹果默认ios，安卓默认android,小程序：applet"],
    ["string(50)", "adfrom2", "二级渠道名称：无渠道时，默认使用default"],
    ["int", "server_id", "创角所在的服务器id"],
    ["int", "platform", "创角所在的平台id"],
    ["string(50)", "region", "创角所在的区域id"],
    ["text", "ext", "扩展字段：json格式"],
    ["int", "date_time", "日期"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

