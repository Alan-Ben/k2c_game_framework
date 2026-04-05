# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

#支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__="mark"
__date__ ="$2017-08-04$"

tableComment = "火星探索-火星探索事件数据"
field = [
    ["long", "cid", "玩家CID"],
    ["int", "exploreLvl", "探索等级"],
    ["int", "eventType", "事件类型"],
    ["long", "eventId", "事件ID"],
    ["int", "quality", "事件品质"],
    ["long", "pos", "事件位置"],
    ["long", "createdMs", "事件创建时间（毫秒）"],
    ["bool", "isDone", "是否完成"],
    ["bytes", "extData", "额外数据"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"