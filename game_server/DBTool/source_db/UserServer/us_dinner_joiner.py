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

tableComment = "宴会参与玩家数据"
field = [
    ["long", "instance_id", "宴会实例ID"],
    ["int", "joiner_type", "赴宴对象类型"],
    ["long", "joiner_id", "赴宴对象ID"],
    ["long", "cost_id", "赴宴消耗配置"],
    ["int", "join_ts", "赴宴时间戳（秒）"],
    ["long", "gain_coin", "结算的宴会币"],
    ["long", "gain_score", "结算的宴会人气"],
]
key = ["instance_id"]
ukey = []  # key,不能重复
dbTag = "main"
