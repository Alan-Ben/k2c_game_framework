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

tableComment = "梦加日志-登录表"
field = [
    ["string(32)", "sole_id", "项目唯一角色id"],
    ["string(64)", "uid", "用户id"],
    ["long", "cid", "角色id"],
    ["int", "operation", "2登录,3登出"],
    ["string(50)", "login_ip", "登入时的ip"],
    ["int", "timestamp", "登入时的unix时间戳（10位）"],
    ["string(50)", "adid", "登录时的手机设备id"],
    ["string(64)", "version", "玩家登录时的客户端版本号"],
    ["int", "server_id", "玩家登录时的服务器id"],
    ["int", "platform", "账号归属的平台id"],
    ["string(50)", "region", "账号归属的区域id"],
    ["text", "ext", "扩展字段：json格式"],
    ["int", "date_time", "日期"],
    ["long", "main_quest_progress", "主线任务进度"],
    ["long", "chapter_progress", "关卡进度：关卡id*1000+格子索引"],
    ["int", "level", "等级"],
    ["long", "crystal", "钻石"],
    ["int", "create_date", "玩家创角日期"],
    ["long", "total_power", "大臣总实力"],
    ["long", "earnings", "玩家总赚速"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"

