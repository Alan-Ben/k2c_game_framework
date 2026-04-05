# coding=utf-8

__author__ = "cooper"
__date__ = "$2026-03-13$"

tableComment = "联盟火星矿战报数据"
field = [
    ["long", "guild_id", "联盟ID"],
    ["long", "cid", "发起战斗的玩家CID"],
    ["int", "log_type", "战报类型（EMarsExplorePVPLogType）"],
    ["long", "created_at", "创建时间（毫秒）"],
    ["bytes", "log_data", "战报数据"],
]

key = ["guild_id"]
ukey = []
dbTag = "main"
