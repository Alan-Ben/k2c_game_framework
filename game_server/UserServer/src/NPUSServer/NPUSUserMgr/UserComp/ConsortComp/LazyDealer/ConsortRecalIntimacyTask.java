package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.LazyDealer;

import ALBasicServer.ALTask._IALSynTask;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSORT_INTIMACY_CHG;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCalculator;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;

/***************
 * 家人重新计算加护点加成的计算处理任务
 */
public class ConsortRecalIntimacyTask implements _IALSynTask
{
    //家人数据
    private ConsortInfo _m_ciConsort;

    public ConsortRecalIntimacyTask(ConsortInfo _consort)
    {
    	_m_ciConsort = _consort;
    }

    public void run()
    {
        _m_ciConsort.getUserData().lockUser();
        try{
            //记录原先数值
            long preValue = _m_ciConsort.getIntimacy();

            //重新计算实力
            long newValue = ConsortCalculator.calIntimacy(_m_ciConsort);

            //判断数据是否有变动，如无变动则不做后续处理
            if(preValue == newValue)
                return;

            //设置新数据
            _m_ciConsort.setIntimacy(newValue);

            //检查解锁家人的经营技能
            _m_ciConsort.getBusinessSkillMgr().checkUnlockSkill(false);

            //推送数据
            _m_ciConsort.getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_051_OnConsortIntimacyChg(_m_ciConsort));

            //触发后续的刷新处理
            //重新计算玩家亲密度
            _m_ciConsort.getUserData().getConsortComponent().calIntimacySum();

            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_INTIMACY_CHG);
            //触发亲密度变更事件
            Event_P_CONSORT_INTIMACY_CHG event = new Event_P_CONSORT_INTIMACY_CHG(context,_m_ciConsort.getConsortId(), newValue);
            _m_ciConsort.getUserData().onLogicEvent(event);
            
            //日志（暂不记录）
            //ConsortInfo.logResChg(_m_ciConsort, ELogConsortResEnum.INTIMACY, preValue, newValue);
        }finally
        {
            _m_ciConsort.getUserData().unlockUser();
        }

        //TODO : 当数据变动的时候，需要调用其他LazyDealer处理
    }
}
