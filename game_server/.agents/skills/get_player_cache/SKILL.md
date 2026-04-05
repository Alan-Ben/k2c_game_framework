---
name: get_player_cache
description: 查询玩家缓存信息（PlayerCacheGetter）。用户说"查询玩家信息"、"获取玩家缓存"、"查玩家名字/头像/等级"、"批量查玩家信息"时触发。
---

# 玩家缓存查询 Skill

日常业务用 `PlayerInfo_IconShow`（含 cid、playerName、iconId、vipLvl、playerLvl、guildId、guildName 等）。
访问入口：`getUSServer().getPlayerCacheGetter()`

## 单个查询

```java
getUSServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _targetCid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
{
    @Override
    public void handle(Boolean _isSuc, PlayerInfo_IconShow _info)
    {
        if (!_isSuc)
        {
            _commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
            return;
        }
        // 使用 _info.getPlayerName()、_info.getCid() 等
        _commiter.commitSucRes(US2GCWriter_XXX.make_XXX(_info));
    }
});
```

## 批量查询

```java
getUSServer().getPlayerCacheGetter().getInfoListA(PlayerInfo_IconShow.class, cidList, new HandlerOne<Map<Long, PlayerInfo_IconShow>>()
{
    @Override
    public void handle(Map<Long, PlayerInfo_IconShow> _map)
    {
        if (null == _map)
        {
            _commiter.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }
        // _map.get(cid) 获取单个；不存在的 cid 在 map 中无条目
        _commiter.commitSucRes(US2GCWriter_XXX.make_XXX(new ArrayList<>(_map.values())));
    }
});
```