package NPUSServer.Dungeon.Evening;

import NPCommon.NPCommon_ItemInfo;

import java.util.ArrayList;
import java.util.List;

public class EveningDungeonAttackResult
{
    public long harmHp;
    public long totalHp;
    public boolean isKill;
    public int rebornTimes;
    public long attackHeroId;  // 攻击的英雄ID
    public long heroExp;  // 获得的伙伴经验
    public List<NPCommon_ItemInfo> attackRewardList = new ArrayList<>();
    public List<NPCommon_ItemInfo> defeatRewardList = new ArrayList<>();
}
