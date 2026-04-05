package NPUSServer.NPUSUserMgr.UserComp.HeroComp.LazyDealer;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroCalculator;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

/***************
 * 英雄重新计算实力的计算处理任务
 */
public class HeroRecalPowerTask implements _IALSynTask
{
    //大臣信息对象
    private HeroInfo _m_hiHeroInfo;

    public HeroRecalPowerTask(HeroInfo _heroInfo)
    {
        _m_hiHeroInfo = _heroInfo;
    }

    public void run()
    {
        _m_hiHeroInfo.getUserdata().lockUser();
        try{
            //记录原先大臣实力值
            long prePower = _m_hiHeroInfo.getPower();

            //重新计算实力
            long newPower = HeroCalculator.calPower(_m_hiHeroInfo, null);

            //判断数据是否有变动，如无变动则不做后续处理
            if (newPower == prePower)
                return;

            //设置大臣新数据
            _m_hiHeroInfo.updatePower(newPower);

            //更新数值到玩家组件
            _m_hiHeroInfo.getComp().replaceHeroPower(prePower, newPower, false);

            //大臣战力发生改变，触发所有建筑重新计算产出速度
            _m_hiHeroInfo.getUserdata().getBuildingComponent().recalAllBuilding();
            
            //触发大臣实力变化事件
            _m_hiHeroInfo.getPowerChgDelegate().onEvent();
            
        }finally
        {
            _m_hiHeroInfo.getUserdata().unlockUser();
        }
    }
}
