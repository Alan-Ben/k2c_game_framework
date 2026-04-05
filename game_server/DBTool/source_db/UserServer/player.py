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

tableComment = "玩家基本数据"
field = [
         ["long", "cid", "玩家CID"],
         ["string(64)", "cname", "玩家名称"], 
		 ["long", "icon", "玩家头像"],
		 ["long", "iconBgk", "玩家头像框"],
		 ["long", "title", "玩家当前使用的称号"],
		 ["long", "bubble", "玩家当前使用的气泡框"],
		 ["int", "createTime","账号创建时间(秒) 同ArTime"],
		 ["int", "create_date","账号创建日期 同ArTime"],
		 ["int", "createRoleTime", "创角时间(秒)"],
		 ["int", "create_role_date", "创角日期"],
		 ["int", "lvl", "玩家等级"],
		 ["int", "vipLvl", "VIP等级"],
		 ["int", "gmLevel", "gm权限"],
		 ["string(20)", "player_lang", "玩家语言"],
         ["int", "last_login_date", "账号最后日期"],
		 ["int", "login_day_count", "登录次数"],
		 ["long", "last_take_version", "最后领取奖励的客户端版本"],
		 ["long", "recharged_gem", "充值钻石数量"],
		 ["int", "last_left_bag_time_s", "最后一次查看背包物品的时间"],
		 ["long", "lastOfflineMs", "最后一次下线时间（毫秒）"],
		 ["long", "prefab", "玩家预制形象配置ID"],
         ["long", "is_set_default", "是否创角 1-已设置"],
         ["long", "player_stage", "玩家段位"],
         ["long", "happy_value_cur_step_value", "当前阶段数值"],
         ["long", "last_login_week_tag", "最后一次登录周标记"],
         ["long", "week_login_day_count", "本周登录天数"],
         ["long", "latest_login_time_ms", "最近一次登陆时间（毫秒）"],
         ["string(32)", "uid", "平台账号"],
		 ["string(128)", "adfrom", "渠道"],
		 ["string(128)", "adfrom2", "二级渠道"],
		 ["string(128)", "adid", "设备id"],
		 ["string(64)", "clientPackageName", "客户端包名"],
		 ["string(32)", "clientVerion", "客户端版本"],
		 ["string(32)", "nation", "国家"],
		 ["string(32)", "ar_ip", "注册IP"],
		 ["string(128)", "sdkid", "渠道账号"],
		 ["long", "recharged_money", "充值货币金额"],
		 ["long", "power_max_record", "玩家实力历史记录最高值"],
		 ["long", "earnings_max_record", "玩家赚速历史记录最高值"],
		 ["long", "market_next_refresh_ms", "集市下次刷新时间"],
		 ["int", "last_log_section_day", "上次截面日志记录日期"],
		 ["int", "last_gain_visit_reward_day", "上次领取拜访其他玩家奖励日期"],
		 ["long", "cute_actor_id", "Q版形象ID"],
		 ["long", "consort_call_next_refresh_ms", "家人指定邀约下次刷新时间（针对需要计数的数据）"],
		 ["int", "birth_giftde_coum", "下次出生卷王次数"],
		 ["long", "adult_record_bonus", "子嗣记录收益总值（因为移除记录在玩家身上）"],
		 ["int", "last_draw_daily_reward_date", "最后一次领取每日奖励的日期标记"],
		 ["long", "extra_earnings", "额外赚速"],
		 ["long", "building_earnings_max_record", "建筑赚速历史记录最高值"],
		 ["long", "child_earnings_max_record", "子嗣赚速历史记录最高值"],
		 ["int", "seven_days_login_cal_offset", "七天登录计算偏移量"],
         
		 ["bytes", "curTitle", "当前称号"],
		 ["bool", "curTitleShow", "当前称号是否对外展示"],
         
		 ["long", "curPlayerSkin", "当前玩家皮肤"],
         ["long", "is_set_prefab", "是否设置预制形象 1-已设置"],
         ["long", "pending_order_db_id", "待处理的订单数据ID"],
         
		 ["bool", "isMarsGoRouteDone", "火星-前往火星全部阶段完成"],
		 ["long", "had_draw_vip_level_reward", "VIP等级奖励领取记录"],
		 ["long", "had_draw_vip_level_recharge_reward", "VIP等级充值奖励领取记录"],
         
		 ["bool", "isStoreReviews", "是否商店评价"],
		 ["long", "room_skin", "房间皮肤ID"],
		 ["int", "giftde_child_graduate_count", "卷王子嗣毕业历史数量"],
        ]
		
key = ["cid","lastOfflineMs"]
ukey = [] #key,不能重复
dbTag = "main"