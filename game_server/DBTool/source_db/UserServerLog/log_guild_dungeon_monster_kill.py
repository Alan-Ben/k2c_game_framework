# -*- coding: utf-8 -*-

tableComment = "公会副本击杀日志"

field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一ID"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    ["long", "guild_id", "公会ID"],
    ["int", "guild_level", "公会等级"],
    ["long", "dungeon_id", "副本ID"],
    ["int", "dungeon_level", "副本等级"],
    ["long", "monster_id", "怪物ID"],
    ["int", "type", "类型"],
]

# 索引配置
key = []
ukey = []

# 数据库标签
dbTag = "us_log"