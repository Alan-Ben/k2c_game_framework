# coding=utf-8
# 建筑升级&员工雇佣日志

# 支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"

## 注意: 若编辑过程中删除了字段, 必须确保key和ukey中未包含不存在的字段!!!

__author__ = "cooper"
__date__ = "$2025-10-31$"

tableComment = "梦加日志-建筑升级&员工雇佣记录"
field = [
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],
    ["long", "bid", "建筑id"],
    ["int", "b_level", "建筑等级（操作时的等级，即升级前）"],
    ["int", "train_type", "培养类型：1=雇佣员工；2=提升建筑等级"],
    ["int", "before_attr", "变更前的属性值（员工数/建筑等级）"],
    ["int", "final_attr", "变更后的属性值（员工数/建筑等级）"],
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
