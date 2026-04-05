package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp;

import ALBasicServer.ALProcess.ALProcess;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CallBack._ICallBackBool;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent.MarsExploreEventMgr;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeamMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;

/**
 * 火星阶段-4-探索
 * @author mj
 *
 */
public class MarsExploreComponent extends _ANPUserComponent
{
	//探索数据
	private MarsExploreInfo _m_eiExploreInfo;
	
	//事件数据管理
	private MarsExploreEventMgr _m_mgrEventMgr;
	
	//队伍数据管理
	private MarsExploreTeamMgr _m_mgrTeamMgr;
	
    //PVP日志数据
    private MarsExplorePVPLogList _m_plPVPLogObj;
    
    public MarsExploreComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MARS_EXPLORE);
        
		_m_eiExploreInfo = new MarsExploreInfo(getUserData());
        _m_mgrEventMgr = new MarsExploreEventMgr(getUserData());
        _m_mgrTeamMgr = new MarsExploreTeamMgr(getUserData());
        
        _m_plPVPLogObj = new MarsExplorePVPLogList(this);
    }
    
    public MarsExploreInfo getExploreInfo() {return _m_eiExploreInfo;}
    public MarsExploreEventMgr getEventMgr() {return _m_mgrEventMgr;}
    public MarsExploreTeamMgr getTeamMgr() {return _m_mgrTeamMgr;}
    
    public MarsExplorePVPLogList getPVPLogObj() {return _m_plPVPLogObj;}
    
    @Override
    protected void _init()
    {
    	ALProcess process = ALProcess.CreateProcess("mars_explore_comp_init");
    	
    	//步骤1：基础数据加载
        process.addResDelegateProcess(action -> _m_eiExploreInfo._initFromDB(action::dealAction), "mars_explore_init",
                () -> USLog.error(getUSServer(), "load mars-explore comp fail[mars_explore_init], cid:{}", getCid()), false);
        //步骤2：探险事件加载
        process.addResDelegateProcess(action -> getEventMgr()._initFromDB(action::dealAction), "mars_explore_init",
                () -> USLog.error(getUSServer(), "load mars-explore-event comp fail[mars_explore_init], cid:{}", getCid()), false);
    	//步骤3：探险队伍加载
        process.addResDelegateProcess(action -> getTeamMgr()._initFromDB(action::dealAction), "mars_explore_init",
                () -> USLog.error(getUSServer(), "load mars-explore-team comp fail[mars_explore_init], cid:{}", getCid()), false);
        //步骤4：探险队伍采集结果加载
        process.addResDelegateProcess(action -> getTeamMgr()._initTeamCollectResultFromDB(action::dealAction), "mars_explore_init",
                () -> USLog.error(getUSServer(), "load mars-explore-team-collect-result comp fail[mars_explore_init], cid:{}", getCid()), false);
        //步骤5：探险数据加载完成后处理
        process.addResDelegateProcess(action -> _dealOnLoaded(action::dealAction), "mars_explore_init",
                () -> USLog.error(getUSServer(), "load mars-explore-deal-inited comp fail[mars_explore_init], cid:{}", getCid()), false);
        
        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load mars-explore comp fail[onRootProecssStop], cid:{}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    //步骤5：探险数据加载完成后处理
    private void _dealOnLoaded(_ICallBackBool _handler)
    {
    	_handler.onRunOver(true);
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    	//检查探索等级，与主基地等级保持一致
    	getExploreInfo()._checkExploreLvl();
    	
    	//探索数据加载完成后处理
    	getExploreInfo()._onLoaded();
    	//探索事件数据加载完成后处理
    	getEventMgr()._onLoaded();
		//队伍状态启动一致性校验
		getTeamMgr().onSInitedCheck();
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    	
    }
}
