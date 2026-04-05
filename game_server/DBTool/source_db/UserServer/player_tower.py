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

__author__="scott"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "玩家爬塔数据"
field = [
         ["long", "cid", "玩家CID"],
         ["long", "chapter_id", "章节ID"],
         ["int", "chapter_level", "关卡内层数 从1开始"],
         ["long", "last_draw_tower_coin_time_ms", "上次领取每日迷宫币的时间戳（毫秒）"],

         ["long", "had_active_research_chapter_id", "已激活研究章节ID"],
         ["int", "had_active_research_chapter_level", "已激活研究章节内层数"],

         ["long", "highest_had_reach_chapter_id", "到达最高章节ID"],
         ["int", "highest_had_reach_chapter_level", "到达最高章节内层数"],
        ]
key = ["cid"]
ukey = [] # key，不能重复
dbTag = "main"