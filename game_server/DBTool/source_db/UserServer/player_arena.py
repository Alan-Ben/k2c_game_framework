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

__author__ = "mark"
__date__ = "$2022-04-25$"

tableComment = "玩家竞技场数据"
field = [
    ["long", "cid", "玩家CID"],
    #计数相关
    ["long", "last_reset_time_ms", "上次重置计数时间"],
    ["int", "had_select_attack_num", "已指定攻击次数"],
    ["int", "had_random_attack_num", "已随机攻击次数"],
    ["int", "had_buy_random_attack_num", "已购买随机攻击次数"],
    ["bytes", "had_select_attack_hero_list", "已指定攻击大臣id列表"],
    ["bytes", "had_random_attack_hero_list", "已随机攻击大臣id列表"],
    ["bytes", "had_attack_opponent_cid_list", "已攻击过的对手CID列表(当天)"],
    ["bool", "had_unlock_arena", "是否解锁竞技场"],
]
key = ["cid"]
ukey = []
dbTag = "main"
