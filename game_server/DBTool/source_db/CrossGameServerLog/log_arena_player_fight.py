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

tableComment = "擂台-挑战日志"
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    
    ["long", "instance_id", "实例ID"],
    ["long", "ref_id", "配置ID"],
    ["long", "cid", "玩家CID"],
    ["int", "ori_rank", "源名次"],
    ["string(512)", "section_data", "攻击方截面数据"],
    ["long", "target_cid", "目标玩家CID"],
    ["int", "target_rank", "目标名次"],
    ["string(512)", "target_section_data", "防守方截面数据"],
    ["int", "result", "挑战结果"],
]
key = []
ukey = []  # key，不能重复
dbTag = "crossgame_log"
