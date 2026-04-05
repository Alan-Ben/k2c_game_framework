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

tableComment = "玩家家人数据"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "consortId", "家人ID"],
    ["int", "fettersLvl", "羁绊等级"],
    ["bool", "isHaloUnlock", "星辉是否解锁"],
    ["int", "haloLvl", "星辉等级"],
    ["long", "intimacy", "亲密度"],
    ["long", "initIntimacy", "初始亲密度"],
    ["long", "charm", "加护力"],
    ["long", "initCharm", "初始加护力"],
    ["long", "charmPoint", "加护力点数"],
    ["long", "curSkinId", "当前皮肤ID"],
	["bytes", "triggeredCallStoryIdList", "已触发的邀约事件ID列表"],
    ["long", "charmPointRecord", "加护力点数记录"],
    ["bool", "has_add_chat_friend", "是否已添加聊天好友"],
]
key = ["cid"]
ukey = []  # key,不能重复
dbTag = "main"
