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

tableComment = "玩家竞技场战斗数据"
field = [
    ["long", "cid", "玩家CID"],
    ["int", "round", "当前回合数"],
    ["bool", "had_buy_buff", "本轮是否已购买buff"],
    ["bytes", "buff_list", "临时增益列表"],
    ["long", "hero_id", "我方大臣id"],
    ["long", "base_power", "我方基础实力"],
    ["long", "deducted_hp", "我方被扣除血量"],
    ["long", "select_attack_item_id", "选择攻击道具id"],
    ["int", "attack_type", "攻击类型"],
    
    #对手相关
    ["long", "opponent_cid", "对手CID"],
    ["bytes", "had_defeat_hero_list", "已经击败的大臣列表"],
    ["bytes", "opponent_hero_list", "对手大臣列表"],
    ["bytes", "can_attack_hero_list", "本回合可攻击英雄列表"],
    ["long", "opponent_power", "对手战力"],
    ["string(256)", "bot_name", "机器人名字"],
    ["bool", "is_bot", "是否是机器人"],
]
key = ["cid"]
ukey = []
dbTag = "main"
