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
__date__ = "$2014-8-28 10:17:54$"

tableComment = "US火星矿战斗日志表"
recordExpiredSec = 3600 * 24 * 30 * 3
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "mineInstanceId", "火星矿实例ID"],
    
    ["bool", "isWin", "true-攻击方胜利"],
    ["long", "attackLoseValue", "进攻方损失精兵"],
    ["long", "attackTroopNum", "防守方当前精兵"],
    
    ["long", "attackPlayerCid", "攻击方玩家CID"],
    ["long", "attackCostValue", "攻击方损耗数量"],
    ["long", "attackTeamId", "攻击方队伍ID"],
    ["long", "attackTeamPower", "攻击方队伍实力"],
    ["long", "attackTeamSoldierPower", "攻击方队伍单兵实力"],
    ["long", "attackTeamTroopNum", "攻击方队伍带兵量"],
    ["long", "attackTeamLossValue", "攻击方队伍耗数量"],
    
    ["long", "defencePlayerCid", "防守方玩家CID"],
    ["long", "defenceCostValue", "防守方损耗数量"],
    ["long", "defenceTeamId", "防守方队伍CID"],
    ["long", "defenceTeamPower", "防守方队伍实力"],
    ["long", "defenceTeamTroopNum", "防守方队伍带兵量"],
    ["long", "defenceTeamLossValue", "防守方队伍耗数量"],
]
key = []
ukey = []  # key，不能重复
dbTag = "us_log"
