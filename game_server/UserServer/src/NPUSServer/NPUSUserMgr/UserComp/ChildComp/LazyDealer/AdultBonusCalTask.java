package NPUSServer.NPUSUserMgr.UserComp.ChildComp.LazyDealer;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.AdultMgr;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;

/***************
 * 家人重新计算加护点加成的计算处理任务
 */
public class AdultBonusCalTask implements _IALSynTask
{
    //子嗣组件对象
    private AdultMgr _m_mgrAdult;

    public AdultBonusCalTask(AdultMgr _adultMgr)
    {
    	_m_mgrAdult = _adultMgr;
    }

    public void run()
    {
        _m_mgrAdult.getUserData().lockUser();
        try{
            //记录原先数值
            long preValue = _m_mgrAdult.getBonusSum();

            //重新计算实力
            long newValue = _m_mgrAdult.calBonusSum();

            //判断数据是否有变动，如无变动则不做后续处理
            if(preValue == newValue)
                return;

            //设置新数据
            _m_mgrAdult.setBonusSum(newValue);
    		//推送数据
            _m_mgrAdult.getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_063_OnAdultBonusSumChg(newValue));

            //触发计算子嗣总收益数值
            _m_mgrAdult.getComp().recalBonus();
        }finally
        {
            _m_mgrAdult.getUserData().unlockUser();
        }
    }
}
