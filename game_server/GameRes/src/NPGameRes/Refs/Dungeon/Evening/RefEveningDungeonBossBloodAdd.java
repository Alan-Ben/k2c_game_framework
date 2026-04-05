package NPGameRes.Refs.Dungeon.Evening;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "evening_dungeon_boss_blood_add")
public class RefEveningDungeonBossBloodAdd extends RefBase
{
    private static RefEveningDungeonBossBloodAddMgr _g_mgr = new RefEveningDungeonBossBloodAddMgr();

    public static RefEveningDungeonBossBloodAddMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefEveningDungeonBossBloodAddMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEveningDungeonBossBloodAddMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEveningDungeonBossBloodAdd newRef = (RefEveningDungeonBossBloodAdd) _newRef;
        respawn_time = newRef.respawn_time;
        boss_respawn_blood_add = newRef.boss_respawn_blood_add;
        box_id = newRef.box_id;
    }

    public static class RefEveningDungeonBossBloodAddMgr extends RefTableContainer<RefEveningDungeonBossBloodAdd>
    {
        /**
         * 正序查找复活配置
         * @param _rebornTimes
         * @return
         */
        public RefEveningDungeonBossBloodAdd getBloodAddRef(int _rebornTimes)
        {
            RefEveningDungeonBossBloodAdd lastRef = null;
            for (RefEveningDungeonBossBloodAdd data : getList())
            {
                if (data.respawn_time == _rebornTimes)
                    return data;

                if (data.respawn_time > _rebornTimes)
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
        return respawn_time;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long respawn_time;//复活次数
    public int boss_respawn_blood_add;//对基础血量的增长万分比
    public long box_id;//宝箱id
}