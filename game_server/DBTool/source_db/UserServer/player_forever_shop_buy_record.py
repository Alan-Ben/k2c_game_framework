tableComment = "玩家终身限购商品购买记录"
field = [
    ["long", "cid", "玩家CID"],
    ["long", "shop_ref_id", "商店配置ID"], 
    ["long", "shop_item_ref_id", "商品配置ID"],
    ["int", "buy_count", "终身购买次数"],
]
key = ["cid"]
ukey = []
dbTag = "main"