# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "claude"
__date__ = "$2026-01-20$"

tableComment = "冲榜礼包服务器数据（缓存配表字段+激活状态）"
field = [
    ["long", "ui_res_path_id", "礼包页面加载资源id"],
    ["int", "sale", "折扣(万分比)"],
    ["int", "buy_limit", "购买次数限制"],
    ["string(200)", "name", "礼包名称"],
    ["string(200)", "cost", "消耗道具"],
    ["string(200)", "ori_cost", "原价消耗道具"],
    ["string(2000)", "reward_item_list", "奖励列表"],
    ["long", "end_time_ms", "截止时间（时间戳毫秒）"],
    ["long", "activate_time_ms", "激活时间（时间戳毫秒，>0表示已激活）"],
]

key = []
ukey = []  # key，不能重复
dbTag = "main"
