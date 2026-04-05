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

tableComment = "玩家截面数据"
field = [
    #### 截面日志类型
    ["int", "sectionType", "截面日志类型，枚举ELogSectionType"],
    
    #### 日志表通用字段
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    #### 玩家
    ["long", "cid", "玩家CID"],
    ["long", "villageEarning", "村庄总收益"],
    ["long", "childEarning", "子嗣总收益"],
    ["long", "chapterStageId", "最新停留关卡"],
    ["int", "consortNum", "知己数量"],
    ["int", "heroNum", "骑士数量"],
    ["int", "level", "玩家等级"],
    ["long", "totalTalent", "总资质（大臣）"],
    ["long", "totalFightPower", "总战力"],
    ["long", "totalDiamondGain", "玩家累计获得的钻石"],
    ["long", "totalGoldGain", "玩家累计获得的金币"],
    ["long", "totalDiamondCost", "玩家累计消耗的钻石"],
    ["long", "totalGoldCost", "玩家累计消耗的金币"],
    ["long", "farmGoldGain", "农田累计征收获得的金币"],
    ["long", "totalPlayerExp", "声望值（累计）"],
    ["long", "questId", "当前正在进行的主线任务ID"],
    #### 主线
    ["int", "loginDayCount", "登录天数"],
    ["long", "highestEarning", "历史最高赚速"],
    ["int", "farmCollect", "农田收取金币次数"],
    ["int", "farmLvl", "农田等级"],
    ["int", "totalHeroLvl", "伙伴总等级"],
    ["int", "totalBuildingLvl", "建筑总等级"],
    ["int", "gainEquipTimes", "累计获得藏品数量"],
    ["int", "consortRandCallCount", "问候知己次数"],
    ["int", "consortCallCount", "指定问候知己次数"],
    ["int", "childNum", "子嗣总数量"],
    ["int", "travelCount", "游历次数"],
    ["int", "starDinnerCount", "举办宴会次数"],
    ["int", "joinDinnerCount", "参与宴会次数"],
    ["int", "arenaAttackTimes", "谈判次数"],
    ["int", "ArenaStationCollectTimes", "竞技场领取收益次数"],
    ["long", "towerPassedChapter", "迷宫层数"],
    ["int", "rankLikeCount", "排行榜点赞次数"],
    #### 子嗣
    ["int", "youngChildNum", "当前未成年子嗣数量"],
    ["int", "adultChildNum", "成年子嗣数量"],
    ["int", "childMarryCount", "子嗣联姻次数"],
    ["long", "marryChildEarning", "已婚子嗣收益(己方子嗣+对方子嗣)"],
    ["long", "unmarriedChildEarning", "未婚子嗣收益"],
    ["int", "childCultureCount", "子嗣培养次数"],
]
key = ["sectionType"]
ukey = []  # key，不能重复
dbTag = "us_log"
