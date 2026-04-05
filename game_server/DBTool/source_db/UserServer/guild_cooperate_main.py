tableComment = "公会协作主表"
field = [
    ["long", "guild_id", "公会ID"],
    ["long", "next_refresh_time_ms", "下次刷新时间毫秒"],
    ["int", "reset_count", "已重置次数"],
    ["long", "recommend_area_id", "推荐区域ID"],
    ["int", "recommend_index", "推荐据点索引"],
    ["bytes", "point_layout_data", "据点布局数据（二进制序列化）"],
]
key = ["guild_id"]
ukey = []
dbTag = "main"