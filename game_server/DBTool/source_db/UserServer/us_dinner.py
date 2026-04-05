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

classAnnotation = "isIdAuto = false" # 类注解 
tableComment = "宴会数据"
field = [
    ["long", "dinner_id", "宴会配置ID"],
    ["long", "owner_cid", "开宴玩家CID"],
    ["int", "dinner_type", "宴会类型"],
    ["int", "start_ts", "开始时间戳（秒）"],
    ["int", "end_ts", "结束时间（秒）"],
    ["int", "permit_type", "凭证类型"],
    ["long", "permit_type_id", "凭证额外类型ID"],
    ["long", "score_add_per", "人气值加成"],
    ["bytes", "hero_id_list", "开宴玩家拥有的大臣ID列表"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"
