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

tableComment = "玩家商店商品数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "shop_db_id", "商店数据id"],
    ["long", "shop_item_ref_id", "商品配置id"],
    ["long", "discount_refId", "折扣配置id"],
    ["long", "shop_item_group_id", "商品组id"],
    ["long", "can_buy_num", "限购数量"],
]
key = ["cid","shop_db_id"]
ukey = []  # key,不能重复
dbTag = "main"
