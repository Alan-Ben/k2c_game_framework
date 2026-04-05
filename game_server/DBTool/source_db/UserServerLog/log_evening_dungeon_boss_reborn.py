tableComment = "晚间副本boss复活日志"
field = [
    ["int", "event_id", "事件类型"],
    ["long", "guid", "事件唯一id"],
    ["int", "date_time", "日期"],
    ["int", "timestamp", "时间戳"],
    ["long", "base_hp", "Boss基础血量"],
    ["long", "boss_hp", "Boss当前血量"],
    ["int", "reborn_times", "复活次数 0就是初始化"],
    ["int", "server_start_day", "服务器开服天数"],
]
key = []
ukey = []
dbTag = "us_log"
