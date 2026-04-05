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

__author__="mark"
__date__ ="$2017-08-04$"

classAnnotation = "isIdAuto = false" # 类注解 
tableComment = "本服火星矿产数据"
field = [
    ["long", "refId", "配置ID"],
    ["long", "endShowMs", "结束展示时间（毫秒）"],
    ["long", "remainNum", "剩余资源数量"],
    ["long", "cid", "玩家CID"],
    ["long", "teamId", "玩家队伍ID"],
    ["long", "teamSoldierPower", "玩家队伍单兵实力"],
    ["long", "teamTroopNum", "玩家队伍带兵量"],
    ["long", "teamLossValue", "玩家队伍损耗数量"],
    ["long", "startCollectMs", "玩家开始采集时间（毫秒）"],
    ["long", "collectSpeed", "玩家采集速度（秒）"],
    ["long", "guild_id", "玩家公会ID"],
    ["string(64)", "cname", "玩家名称"], 
    ["string(128)", "guild_simple_name", "公会简称"],
    ["bytes", "attacked_guild_ids", "进攻过的联盟ID列表"],
]
key = []
ukey = []  # key,不能重复
dbTag = "main"