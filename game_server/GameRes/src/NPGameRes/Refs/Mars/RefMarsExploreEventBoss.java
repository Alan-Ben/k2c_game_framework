package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.Mars.UsMarsBattle._IUsMarsBattleObj;

import java.util.ArrayList;

@RefTable(tableName = "mars_explore_event_boss")
public class RefMarsExploreEventBoss extends RefBase implements _IUsMarsBattleObj
{
    private static RefMarsExploreEventBattleMgr _g_mgr = new RefMarsExploreEventBattleMgr();

    public static RefMarsExploreEventBattleMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreEventBattleMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreEventBattleMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreEventBoss newRef = (RefMarsExploreEventBoss) _newRef;
        event_id = newRef.event_id;
        next_event_id = newRef.next_event_id;
        reward_item_list = newRef.reward_item_list;
        solider_power = newRef.solider_power;
        solider_num = newRef.solider_num;
    }

    @Override
    public long Id()
    {
        return event_id;
    }

    public static class RefMarsExploreEventBattleMgr extends RefTableContainer<RefMarsExploreEventBoss>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long event_id;//事件id
    public long next_event_id;//下一个事件ID
    public ArrayList<NPCommonCostItem> reward_item_list = new ArrayList<>();//奖励列表
    public long solider_power;//单兵战力
    public long solider_num;//兵力

    /************************* 战斗处理对象的防守方重载函数 ******************/
    /**
     * 获取当前可参战的战斗人员数量
     * @return
     */
    public long getTeamSoldierNum()
    {
        return solider_num;
    }

    /**
     * 获取参战的单兵实力
     * @return
     */
    public long getTeamSoldierPower()
    {
        return solider_power;
    }

    /**
     * 增加伤兵数量，注意这里是增量不是全量
     * @param _hurtNum
     */
    public void addHurtSoldierNum(long _hurtNum)
    {
    }

    /**
     * 记录战斗日志的接口
     * @param _isAttacker
     * @param _isAttWin
     * @param _attackPower
     * @param _attckHurtNum
     * @param _attackTroopNum
     * @param _defencePower
     * @param _defenceHurtNum
     * @param _defenceTroopNum
     */
    public void logBattle(boolean _isAttacker, boolean _isAttWin, _IUsMarsBattleObj _enemy, long _attackPower, long _attckHurtNum
            , long _attackTroopNum, long _defencePower, long _defenceHurtNum, long _defenceTroopNum)
    {
    }
    /************************* 战斗处理对象的防守方重载函数 end ******************/
}