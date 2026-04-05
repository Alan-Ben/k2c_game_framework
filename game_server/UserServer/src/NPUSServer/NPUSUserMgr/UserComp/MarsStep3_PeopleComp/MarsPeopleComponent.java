package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MailObj.Mail_Data;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Mars.RefMarsSatisfactionDegree;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsHomeBuildingFunc;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsPeopleBO;

/**
 * 火星 - 居民
 * @author mj
 *
 */
public class MarsPeopleComponent extends _ANPUserComponent implements _IHandlerHolder
{
	//数据
	private PlayerMarsPeopleBO _m_bo;
	
	//移民前置数据
	private MarsPeopleImmigrantInfo _m_iiImmigrantInfo;
	//居民人数数据
	private MarsPeopleNumInfo _m_niNumInfo;
	//决策数据管理
	private MarsPeopleIntelligentMgr _m_mgrIntelligentMgr;
	//信件数据管理
	private MarsPeopleLetterMgr _m_mgrLetterMgr;
	//帮助数据管理
	private MarsPeopleHelpMgr _m_mgrHelpMgr;
	//事件数据管理
	private MarsPeopleEventMgr _m_mgrEventMgr;
	
    public MarsPeopleComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MARS_PEOPLE);
        
		_m_niNumInfo = new MarsPeopleNumInfo(this);
		_m_iiImmigrantInfo = new MarsPeopleImmigrantInfo(this);
        _m_mgrEventMgr = new MarsPeopleEventMgr(this);
        _m_mgrIntelligentMgr = new MarsPeopleIntelligentMgr(getUserData());
        _m_mgrLetterMgr = new MarsPeopleLetterMgr(getUserData());
        _m_mgrHelpMgr = new MarsPeopleHelpMgr(getUserData());
    }
    
    protected void _lock() {getUserData().lockUser();}
    protected void _unlock() {getUserData().unlockUser();}
    
    public PlayerMarsPeopleBO getBo() {return _m_bo;}
    //满意度数值
    public int getSatisfaction() {return _m_bo.getSatisfaction();}
    //上次满意度计算时间
    public long getLastCalMs() {return _m_bo.getLastCalMs();}
    public void setLastCalMs(long _timeMs) {_m_bo.saveLastCalMs(getUSServer().getBM(), _timeMs);}
    
    public MarsPeopleNumInfo getNumInfo() {return _m_niNumInfo;}
    public MarsPeopleImmigrantInfo getImmigrantInfo() {return _m_iiImmigrantInfo;}
    public MarsPeopleIntelligentMgr getIntelligentMgr() {return _m_mgrIntelligentMgr;}
    public MarsPeopleLetterMgr getLetterMgr() {return _m_mgrLetterMgr;}
    public MarsPeopleHelpMgr getHelpMgr() {return _m_mgrHelpMgr;}
    public MarsPeopleEventMgr getEventMgr() {return _m_mgrEventMgr;}
    
    @Override
    protected void _init()
    {
    	ALProcess process = ALProcess.CreateProcess("mars_people_comp_init");
    	
    	//步骤1：基础数据加载
        process.addResDelegateProcess(action -> _initFromDB(action::dealAction), "mars_people_init",
                () -> USLog.error(getUSServer(), "load mars-people comp fail[mars_people_init], cid:{}", getCid()), false);
    	//步骤2：决策数据加载
        process.addResDelegateProcess(action -> _m_mgrIntelligentMgr._initFromDB(action::dealAction), "mars_people_intelligent_init",
                () -> USLog.error(getUSServer(), "load mars-people-intelligent comp fail[mars_people_init], cid:{}", getCid()), false);
    	//步骤3：信件数据加载
        process.addResDelegateProcess(action -> _m_mgrLetterMgr._initFromDB(action::dealAction), "mars_people_letter_init",
                () -> USLog.error(getUSServer(), "load mars-people-letter comp fail[mars_people_init], cid:{}", getCid()), false);
    	//步骤4：帮助数据加载
        process.addResDelegateProcess(action -> _m_mgrHelpMgr._initFromDB(action::dealAction), "mars_people_help_init",
                () -> USLog.error(getUSServer(), "load mars-people-help comp fail[mars_people_init], cid:{}", getCid()), false);
    	
        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load mars-people comp fail[onRootProecssStop], cid:{}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }
    //mars_people_init
    private void _initFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerMarsPeopleBO.class).findOne("cid", getCid(), 
        		new _ASelectCallback<PlayerMarsPeopleBO>()
        {
			@Override
			public void dealSuc(PlayerMarsPeopleBO _bo) 
			{
				_m_bo = _bo;
				
				_m_mgrEventMgr._loadBo(_m_bo);
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				if(getHasErr())
				{
					_handler.onRunOver(false);
					return;
				}
				
				PlayerMarsPeopleBO bo = new PlayerMarsPeopleBO();
				bo.setCid(getBM(), getCid());
				bo.setSatisfaction(getBM(), RefGeneral.Ref().mars_satisfaction_degree_init_per);
				bo.insert(getBM());
				
				_m_bo = bo;
				
				_handler.onRunOver(true);
			}
        });
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
    	//刷新每日移民次数
    	getImmigrantInfo().refreshUsedCount(true);
    	
        //监听跨天处理
        getUserData().OnCrossDay.addHandler(this, new HandlerOne<Integer>()
        {
            @Override
            public void handle(Integer _nowTag)
            {
            	//计算满意度
            	calDailySatisfaction(NPPlayerContext.createNew(ENPGameEvent.SERVER_CROSS_DAY));
            	
            	//清空每日事件记录
            	_m_mgrEventMgr.clearDailyEvent();
            	//重置每日求助计数
            	_m_mgrHelpMgr.dealOnCrossDay(_nowTag);
            }
        });
        
        //居民数据tick
        getUserData().safeCall(()->
        {
        	ALSynTaskManager.getInstance().regTask(new MarsPeopleTickTask(getUSServer(), getCid()));
        });
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    	//清除当前处理对象
    	getUserData().OnCrossDay.clear(this);
    }
    
    /**
     * 计算计算满意度
     * @param _context
     */
    public void calDailySatisfaction(NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
            //检查火星系统是否解锁（名义中心解锁条件）
            long unlockId = RefGeneral.Ref().mars_satisfaction_degree_reward_mail_simple_unlock_id;
            if(!NPPlayerConditionDealerMgr.IsEnable(unlockId, getUserData(), null))
                return;

    		//上次计算时间
    		long lastCalMs = getLastCalMs();
    		//当天0点时间
            long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);

    		//首次仅作时间记录，不进行奖励下发
            if (lastCalMs <= 0)
            {
                setLastCalMs(todayZeroClockMS);
                return;
            }

            //计算可以领取的天数
            int diffDays = CommonFunc.getDayDiff(lastCalMs, todayZeroClockMS);
            //没有跨天则不处理
            if (diffDays <= 0)
                return;

            //获取当前可以符合要求满意度配置
            RefMarsSatisfactionDegree ref = RefMarsSatisfactionDegree.getMgr().getBySatisfaction(getSatisfaction());
            if(null == ref)
	            return;

            setLastCalMs(todayZeroClockMS);

            //通过邮件补发前一天的奖励（不管间隔多少天，只发1天的量）
            if(!ref.reward_list.isEmpty())
            {
            	//创建邮件数据
            	Mail_Data mailData = new Mail_Data();
                mailData.setMailRefId(RefGeneral.Ref().mars_satisfaction_degree_reward_mail_id);
                mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(ref.reward_list));

            	//通过邮件系统发送奖励
            	MailSystem.addMail(getUSServer(), getUserData().getCid(), mailData, _context);
            }
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 设置满意度
     * @param _num
     * @param _context
     */
    public void setSatisfaction(int _num, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		//确保数值在 0 - 10000 之间
    		int num = Math.min(_num, 10000);
    		num = Math.max(num, 0);
    		
    		_m_bo.saveSatisfaction(getBM(), num);
    		
    		getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_052_OnSatisfactionChg(num));
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    /**
     * 调整满意度
     * @param _num - 减少使用负数
     * @param _context
     */
    public void chgSatisfaction(int _num, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		int newNum = _m_bo.getSatisfaction() + _num;
    		if(newNum < 0)
    			newNum = 0;
    		
    		setSatisfaction(newNum, _context);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     *  tick处理
     * @param _context
     */
    public void dealTick(NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		//GOB-8235 【优化-1】火星的政务需要在建造火星基地的时候开始刷新，要不一建成基地就会有很多事件一次刷新出来
    		//https://www.teambition.com/task/69546e6220971b7c7c5b6fdc
    		MarsHomeBuildingFunc home = getUserData().getMarsBuildingComponent().getHomeFunc();
    		if(null == home || home.getBuildingLvl() == 0)
    			return;
    		
    		long nowTimeMs = CommonFunc.getNowTimeMS();
    		//刷新信件
    		if(nowTimeMs >= _m_bo.getLastLetterBuildMs() + RefGeneral.Ref().mars_letter_refresh_time * 1000)
    		{
    			_m_bo.saveLastLetterBuildMs(getBM(), nowTimeMs);
    			
    			_m_mgrLetterMgr.buildLetterList(RefGeneral.Ref().mars_letter_refresh_num_once, _context);
    		}
    		
    		//刷新求助
    		if(nowTimeMs >= _m_bo.getLastHelpBuildMs() + RefGeneral.Ref().mars_daily_help_refresh_time * 1000)
    		{
    			_m_bo.saveLastHelpBuildMs(getBM(), nowTimeMs);
    			
    			_m_mgrHelpMgr.buildHelp(_context);
    		}
    		
    		//刷新事件
    		if(nowTimeMs >= _m_bo.getLastEventBuildMs() + RefGeneral.Ref().mars_event_trigger_time * 1000)
    		{
    			_m_bo.saveLastEventBuildMs(getBM(), nowTimeMs);
    			
    			_m_mgrEventMgr.dealEvent(_context);
    		}
    		
    		//刷新治愈居民
    		if(nowTimeMs >= _m_bo.getLastCureSickPeopleMs() + RefGeneral.Ref().mars_building_cure_time_gap_sec * 1000)
    		{
    			_m_bo.saveLastCureSickPeopleMs(getBM(), nowTimeMs);
    			
    			//处理治愈
    			int cureNum = MarsPeopleCalculate.calCureNum(getUserData());
    			if(cureNum > 0)
    			{
    				_m_niNumInfo.cureSickNum(cureNum, _context);
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
