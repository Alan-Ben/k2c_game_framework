package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent;

import CommonEnum.ESpecialItemType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerSpecialItemBO;

public abstract class _ASpecialItemDealer
{
    private SpecialItemComponent _m_comp;

    public _ASpecialItemDealer(SpecialItemComponent _comp)
    {
        _m_comp = _comp;
    }

    public SpecialItemComponent getComp()
    {
        return _m_comp;
    }
    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    public abstract ESpecialItemType getSpecialItemType();

    protected void _lock()
    {
        getComp().getUserData().lockUser();
    }

    protected void _unlock()
    {
        getComp().getUserData().unlockUser();
    }

    /**
     * 初始化bo
     * @param _bo
     */
    public void initBo(PlayerSpecialItemBO _bo)
    {
        USLog.error(getComp().getUSServer(), "class:{} not support bo please check", getClass().getSimpleName());
    }

    public abstract void onInited();
    
    public abstract void dispose();
}
