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

tableComment = "联盟副本全局配置数据"
field = [
    ["long", "guildId", "联盟ID"],
    ["int", "autoHour", "自动开启时间：小时"],
    ["int", "autoMin", "自动开启时间：分钟"],
    ["long", "lastAutoStartedMs", "上次自动开启时间（毫秒）"],
    ["long", "lastAutoSettledMs", "上次自动结算时间（毫秒）"],
]

key = ["guildId"]
ukey = []  # key，不能重复
