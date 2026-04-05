package NPUSServer.NPUSUserMgr.UserComp.ChildComp.LazyDealer;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;

/***************
 * 家人重新计算加护点加成的计算处理任务
 */
public class AllChildBonusCalTask implements _IALSynTask
{
    //子嗣组件对象
    private ChildComponent _m_compChild;

    public AllChildBonusCalTask(ChildComponent _childComp)
    {
    	_m_compChild = _childComp;
    }

    public void run()
    {
        _m_compChild.getUserData().lockUser();
        try{
            //记录原先数值
            long preValue = _m_compChild.getBonusSum();

            //重新计算实力
            long newValue = _m_compChild.getChildMgr().getBonusSum() + _m_compChild.getAdultMgr().getBonusSum();

            //判断数据是否有变动，如无变动则不做后续处理
            if(preValue == newValue)
                return;

            //设置新数据
            _m_compChild.setBonusSum(newValue);
    		//推送数据
            _m_compChild.getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_061_OnChildBonusSumChg(newValue));

            //触发后续的刷新处理
            _m_compChild.replaceEarnings(preValue, newValue, false);
        }finally
        {
            _m_compChild.getUserData().unlockUser();
        }
    }
}
