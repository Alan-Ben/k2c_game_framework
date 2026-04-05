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

__author__="yoey"
__date__ ="$2014-8-28 10:17:54$"

tableComment = "充值订单表"
field = [
        ["string(256)", "carrier", "运营商"],
        ["string(256)", "platform", "平台ios，andriod，越狱"],
		["string(256)", "adfrom", "pp，360，qq 主来源1"],
		["string(256)", "adfrom2", "来源2"],
		["string(256)", "gameid", "游戏id"],
		["string(256)", "server_id", "服务器id"],
		["string(256)", "appid", ""],
		["string(256)", "pid", "php平台id"],
		["long", "uid", "玩家id"],
		["string(128)", "cporderid", "cp定单号,php正式的，唯一"],
		["string(256)", "adfrom_orderid", "渠道支付定单号"],
		["string(256)", "item", "物品列表"],
		["string(256)", "actiid", " 活动列表"],
		["int", "crystal", "钻石数量"],
		["long", "amount", "实际货币金额"],
		["long", "usamount", "实际美元金额"],
		["string(256)", "goodname", " 商品名称"],
		["string(256)", "cpgoodid", " cp商品id"],
		["string(256)", "appgoodid", " app商品id"],
		["int", "activityExtra", "赠送发钻数量"],
		["string(20)", "status", " 状态"],
		["int", "orderTime", "到游戏服务器上时间"],
		["int", "deliverTime", "完成发送时间"],
		["int", "limitNum", "购买限制次数"],
		["int", "resetLimitSec", "重置限制时间，秒"],
		["string(256)", "limitedItems", "超出限制后给的物品列表"],
		["string(256)", "gameextinfo", "游戏额外信息"],
		["int", "err_code", "处理错误码"],
        ]

key = ["status"]
ukey = ["cporderid"] #key,不能重复
dbTag = "comm_main"