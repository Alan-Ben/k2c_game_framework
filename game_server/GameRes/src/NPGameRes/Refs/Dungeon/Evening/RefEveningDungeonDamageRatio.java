package NPGameRes.Refs.Dungeon.Evening;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "evening_dungeon_damage_ratio")
public class RefEveningDungeonDamageRatio extends RefBase
{
    private static RefEveningDungeonDamageRatioMgr _g_mgr = new RefEveningDungeonDamageRatioMgr();

    public static RefEveningDungeonDamageRatioMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefEveningDungeonDamageRatioMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEveningDungeonDamageRatioMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEveningDungeonDamageRatio newRef = (RefEveningDungeonDamageRatio) _newRef;
        decrease_blood_ratio = newRef.decrease_blood_ratio;
        ratio = newRef.ratio;
    }

    public static class RefEveningDungeonDamageRatioMgr extends RefTableContainer<RefEveningDungeonDamageRatio>
    {
        /**
         * 正序查找比率配置，如果输入的伤害比率大于配置的比率则赋值，如果小于则跳出，返回数据
         * @param _damageRatio
         * @return
         */
        public RefEveningDungeonDamageRatio getRatioByDamageRatio(int _damageRatio)
        {
            RefEveningDungeonDamageRatio lastRef = null;
            for (RefEveningDungeonDamageRatio data : getList())
            {
                if (data.decrease_blood_ratio == _damageRatio)
                    return data;

                if (data.decrease_blood_ratio > _damageRatio)
                    return lastRef;

                lastRef = data;
            }
            return lastRef;
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return decrease_blood_ratio;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long decrease_blood_ratio;//造成boss血量的占比(万分比)
    public int ratio;//伤害倍数
}