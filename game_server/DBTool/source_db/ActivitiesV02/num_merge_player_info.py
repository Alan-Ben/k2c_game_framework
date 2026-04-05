# -*- coding: utf-8 -*-

tableComment = "数字合并玩家信息表"

field = [
    ["long", "cid", "玩家CID"],
    ["long", "activity_instance_id", "活动实例ID"],
    ["long", "total_score", "累计总积分"],
    ["long", "round_max_score", "单轮最高分"],
    ["int", "total_cost_stamina", "累计消耗体力"],
    ["int", "current_cost_stamina", "当前消耗体力"],
    ["int", "current_step", "当前步数"],
    ["long", "current_score", "当前得分"],
    ["bytes", "blocks", "方块数据"],
    ["int", "generated_buff_count", "已生成buff数量"],
    ["int", "box_level", "已领取的宝箱等级"],
    ["long", "box_score", "当前宝箱积分"],
]

key = ["activity_instance_id"]
ukey = []
dbTag = "main"
