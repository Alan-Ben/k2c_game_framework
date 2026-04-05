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

__author__ = "yoey"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家登录到 US 记录"
field = [
    ["string(50)", "account_id", "玩家用户名"],
    ["long", "cid", "玩家角色cid"],
    ["int", "last_login_server_id", "上一次的登录服务器id"],
    ["long", "last_login_time_ms", "上一次登录时间戳"],
]
key = ["account_id"]
ukey = []
dbTag = "rcs_db"
