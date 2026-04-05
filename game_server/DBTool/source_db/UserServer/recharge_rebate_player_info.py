# coding=utf-8

tableComment = "玩家充值返利数据"
field = [
    ["long", "activity_instance_id", "活动实例ID"],
    ["long", "cid", "玩家CID"],
    ["long", "group_id", "返利组ID"],
    ["long", "count", "当前计数（VIP点数或充值天数）"],
    ["int", "last_recharge_date", "最后充值日期（YYYYMMDD格式）"],
    ["text", "had_draw_step_list", "已领取档位ID列表（逗号分隔）"],
]
key = ["activity_instance_id"]
ukey = []
dbTag = "main"
