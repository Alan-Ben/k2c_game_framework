# -*- coding: utf-8 -*-

tableComment = "玩家冲榜礼包购买记录"

field = [
    ["long", "cid", "玩家CID"],
    ["long", "rank_gift_pack_instance_id", "关联的冲榜礼包实例ID(us_rank_gift_pack表的id)"],
    ["int", "buy_count", "已购买次数"],
]

key = ["cid"]
ukey = []
dbTag = "main"
