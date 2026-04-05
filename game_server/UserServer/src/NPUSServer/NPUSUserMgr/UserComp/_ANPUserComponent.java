package NPUSServer.NPUSUserMgr.UserComp;

import NPCommon.DB.BM.BM;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

abstract public class _ANPUserComponent
{
    private NPUSUserData _m_udUserData;
    private ENPPlayerCompType _m_eUserComponent;
    private boolean _m_bStartInit;
    private boolean _m_bInited;
    private long _m_lStartTimeMs;

    public _ANPUserComponent(NPUSUserData _userData, ENPPlayerCompType _userComponent)
    {
        _m_udUserData = _userData;
        _m_eUserComponent = _userComponent;
        _m_bStartInit = false;
        _m_bInited = false;

        //注册组件
        _m_udUserData.getComponentMgr().regComponent(this);
    }

    public NPUSUserData getUserData()
    {
        return _m_udUserData;
    }
    public NPUserServer getUSServer() 
    {
    	return _m_udUserData.getUSServer();
    }
    public long getCid()
    {
    	return _m_udUserData.getCid();
    }
    public BM getBM()
    {
    	return _m_udUserData.getUSServer().getBM();
    }

    public ENPPlayerCompType getCompType()
    {
        return _m_eUserComponent;
    }

    public boolean isStartInit()
    {
        return _m_bStartInit;
    }

    public boolean isInited()
    {
        return _m_bInited;
    }

    public long getStartTimeMs()
    {
        return _m_lStartTimeMs;
    }

    public void init()
    {
        if (_m_bStartInit)
            return;

        _m_bStartInit = true;
        _m_lStartTimeMs = CommonFunc.getNowTimeMS();

        //调用加载处理
        _init();
    }

    public void setInited()
    {
        if (_m_bInited)
            return;

        _m_bInited = true;
        //检测是否加载完成
        getUserData().getComponentMgr().callbackInitedComponent();
    }

    //初始化调用函数
    protected abstract void _init();

    //获取需要依赖的加载组件项，无依赖则返回null
    public abstract ENPPlayerCompType[] getDependCompList();

    //在加载完成所有组件（包括玩家身上的其他组件）数据后才调用的函数
    public abstract void onInited();

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    public abstract void dispose();
}
