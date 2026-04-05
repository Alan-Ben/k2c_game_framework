# coding=utf-8
# To change this license header, choose License Headers in Project Properties.
# To change this template file, choose Tools | Templates
# and open the template in the editor.

#支持类型
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

tableComment = "火星-火星科技数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "techId", "科技ID"],
    ["int", "lvl", "科技等级"],
    ["bool", "isUpgrading", "是否升级中"],
    ["long", "startUpgradeLvlMs", "建筑开始升级时间（毫秒）"],
    ["long", "endUpgradeLvlMs", "建筑结束升级时间（毫秒）"],
    ["bytes", "upgradeCost", "升级消耗（玩家取消时返还）"],
    ["int", "guildHelpType", "公会求助类型"],
    ["long", "guildHelpId", "公会求助ID"],
    ["int", "guildHelpSecs", "公会助力时间（秒）"],
    ["int", "itemHelpSecs", "加速道具助力时间（秒）"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"