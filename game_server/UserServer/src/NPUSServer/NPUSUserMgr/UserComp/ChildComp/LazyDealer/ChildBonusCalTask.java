package NPUSServer.NPUSUserMgr.UserComp.ChildComp.LazyDealer;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildMgr;

/***************
 * 家人重新计算加护点加成的计算处理任务
 */
public class ChildBonusCalTask implements _IALSynTask
{
    //子嗣组件对象
    private ChildMgr _m_mgrChild;

    public ChildBonusCalTask(ChildMgr _childMgr)
    {
    	_m_mgrChild = _childMgr;
    }

    public void run()
    {
        _m_mgrChild.getUserData().lockUser();
        try{
            //记录原先数值
            long preValue = _m_mgrChild.getBonusSum();

            //重新计算实力
            long newValue = _m_mgrChild.calBonusSum();

            //判断数据是否有变动，如无变动则不做后续处理
            if(preValue == newValue)
                return;

            //设置新数据
            _m_mgrChild.setBonusSum(newValue);

            //触发计算子嗣总收益数值
            _m_mgrChild.getComp().recalBonus();
        }finally
        {
            _m_mgrChild.getUserData().unlockUser();
        }
    }
}
