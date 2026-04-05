package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.LazyDealer;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCalculator;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;

/***************
 * 家人重新计算加护点加成的计算处理任务
 */
public class ConsortRecalCharmTask implements _IALSynTask
{
    //家人数据
    private ConsortInfo _m_ciConsort;

    public ConsortRecalCharmTask(ConsortInfo _consort)
    {
    	_m_ciConsort = _consort;
    }

    public void run()
    {
        _m_ciConsort.getUserData().lockUser();
        try{
            //记录原先数值
            long preValue = _m_ciConsort.getCharm();

            //重新计算实力
            long newValue = ConsortCalculator.calCharm(_m_ciConsort);

            //判断数据是否有变动，如无变动则不做后续处理
            if(preValue == newValue)
                return;

            //设置新数据
            _m_ciConsort.setCharm(newValue);

            //推送数据
            _m_ciConsort.getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_052_OnConsortCharmChg(_m_ciConsort));

            //触发后续的刷新处理
            //重新计算玩家加护值
            _m_ciConsort.getUserData().getConsortComponent().calCharmSum();
            
            //日志（暂不记录）
            //ConsortInfo.logResChg(_m_ciConsort, ELogConsortResEnum.CHARM, preValue, newValue);
        }finally
        {
            _m_ciConsort.getUserData().unlockUser();
        }

        //TODO : 当数据变动的时候，需要调用其他LazyDealer处理
    }
}
