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

__author__ = "scott"
__date__ = "$2014-8-28 10:17:54$"

tableComment = "玩家商店数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "shop_ref_id", "商店配置id"],
    ["long", "next_fresh_time_ms", "下次刷新时间"],
    ["int", "has_refresh_num", "已付费刷新次数"],
]
key = ["cid"]  # key
ukey = []  # key,不能重复
dbTag = "main"
