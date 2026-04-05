# coding=utf-8
# 寻宝记录日志

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
__date__ = "$2025-11-08$"

tableComment = "梦加日志-寻宝记录"
field = [
    # 角色信息
    ["long", "cid", "角色id"],
    ["string(64)", "uid", "平台用户id"],
    ["int", "vip_lv", "玩家VIP等级"],
    ["int", "server_id", "服务器id"],
    ["int", "platform", "平台id"],
    ["int", "region", "区域id"],
    ["int", "create_time", "玩家创角时间"],

    # 寻宝详情
    ["int", "is_advanced", "是否高级寻宝,1=是,2=否"],
    ["long", "area_id", "本次飞行所在的飞行点"],
    ["long", "fly_distance", "飞行距离"],
    ["string(2000)", "result_list", "寻宝结果JSON字符串"],
    ["long", "total_num", "累计获得该矿石数量"],

    # 时间戳
    ["int", "timestamp", "事件发生时间戳(10位)"],
]
key = []
ukey = []
dbTag = "us_log"
