package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPair;

@RefTable(tableName = "hero_level")
public class RefHeroLevel extends RefBase
{
    private static RefHeroLevelMgr _g_mgr = new RefHeroLevelMgr();

    public static RefHeroLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefHeroLevelMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefHeroLevelMgr) _mgr;
    }

    public static class RefHeroLevelMgr extends RefTableContainer<RefHeroLevel>
    {
        /**
         * 计算升级到指定等级需要的经验
         * @param _curLevel    当前等级
         * @param _targetLevel 目标等级
         * @param _silverCount 当前银币数量
         * @return
         */
        public WCGPair<RefHeroLevel, Long> calculateLevelUpNeedExp(int _curLevel, int _targetLevel, long _silverCount)
        {
            long needExp = 0;
            RefHeroLevel refTargetLevel = null;

            for (int i = _curLevel; i < _targetLevel; i++)
            {
                RefHeroLevel refLevel = get(i);
                if (refLevel == null)
                    break;

                RefHeroLevel refNextLevel = get(i + 1);
                if (refNextLevel == null)
                    break;

                //如果银币不够，则直接跳出
                if (needExp + refLevel.need_exp > _silverCount)
                    break;

                needExp += refLevel.need_exp;
                refTargetLevel = refNextLevel;
            }

            return new WCGPair<>(refTargetLevel, needExp);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroLevel newRef = (RefHeroLevel) _newRef;
        level = newRef.level;
        need_exp = newRef.need_exp;
        level_ratio = newRef.level_ratio;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return level;
    }

    public int level; // 等级
    public long need_exp;//升到下一等级需要经验
    public int level_ratio;//等级系数
}
