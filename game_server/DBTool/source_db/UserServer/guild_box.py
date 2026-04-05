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

tableComment = "联盟宝箱数据"
field = [
    ["long", "guild_id", "联盟id"],
    ["long", "box_id", "宝箱配置ID"],
    ["long", "share_cid", "分享玩家CID"],
    ["long", "end_time", "截至时间（毫秒）"],
    ["bool", "is_anonymous", "是否匿名"],
]

key = ["guild_id"]
ukey = []  # key，不能重复
