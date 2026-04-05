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

tableComment = "火星-火星居民数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "startMs", "开启时间（毫秒）"],
    ["long", "endMs", "截至时间（毫秒）"],
    ["int", "curUsedCount", "本次移民次数，用于计算本次的奖励数据"],
    ["int", "dayTag", "当日tag"],
    ["int", "usedCount", "当日移民次数"],
    ["long", "immigrantNum", "移民居民数量"],
    ["long", "idleNum", "休闲居民数量"],
    ["long", "sickNum", "生病居民数量"],
    ["int", "satisfaction", "满意度"],
    ["long", "lastCalMs", "上次计算满意度时间（毫秒）"],
    ["bytes", "dailyEventInfo", "每日事件数据"],
    ["long", "lastLetterBuildMs", "上次信件创建时间（毫秒）"],
    ["long", "lastHelpBuildMs", "上次求助创建时间（毫秒）"],
    ["long", "lastEventBuildMs", "上次事件创建时间（毫秒）"],
    ["long", "lastCureSickPeopleMs", "上次治愈居民时间（毫秒）"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"