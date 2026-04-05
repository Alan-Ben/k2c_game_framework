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

tableComment = "玩家缓存数据"
field = [
         ["long", "cid", "玩家CID"],
         ["string(100)", "player_name", "玩家名"],
         ["int", "vip_lvl", "VIP等级"],
         ["int", "player_lvl", "玩家等级"],
         ["long", "memory_update_time_ms", "内存数据写入时间"],
         ["bytes", "guild_info", "玩家归属联盟信息"],
         ["bytes", "icon_info", "头像信息"],
         ["bytes", "icon_bgk_info", "头像框信息"],
         ["bytes", "bubble_info", "气泡框信息"],
         ["bytes", "cute_actor_info", "Q版形象信息"],
         ["long", "last_offline_time_ms", "最近一次离线时间"],
         ["long", "last_online_time_ms", "最近一次上线时间"],
         ["long", "freeze_time_ms", "冻结结束时间"],
         ["string(20)", "language", "玩家语言"],
         ["long", "total_power", "总实力(延迟更新)"],
         ["long", "max_power", "历史最高实力(延迟更新)"],
         ["long", "earnings", "总赚速(延迟更新)"],
         ["long", "max_earnings", "历史最大总赚速(延迟更新)"],
         ["long", "exp", "经验值"],
         ["bytes", "title_obj_v2", "称号相关"],
         ["long", "player_skin", "玩家皮肤"],
         ["long", "vipExp", "VIP经验值"],
        ]
key = ["cid"]
ukey = [] # key，不能重复
dbTag = "main"