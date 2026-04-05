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

tableComment = "火星-玩家前往火星数据"
field = [
    ["long", "cid", "玩家CID"],
    ["int", "stage", "阶段"],
    ["long", "arrivedMs", "第一阶段到达时间（毫秒）"],
    ["long", "stageStartMs", "当前阶段开始时间（毫秒）"],
    ["bool", "sendDoneMarquee", "发送登录成功后的跑马灯"],
    ["bool", "hasSentStageMsg", "当前阶段是否已发送过留言（阶段变更时清空）"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"