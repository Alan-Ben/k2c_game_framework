package NPUSServer.NPUSUserMgr.UserComp.ChildComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.DinnerEnum.EDinnerPermitType;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Common.RefBasicAttr;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.AdultMgr;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildMgr;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.LazyDealer.AllChildBonusCalTask;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr.ToMeMarryApplyMgr;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;

public class ChildComponent extends _ANPUserComponent implements _IHandlerHolder
{
	//未成年子嗣数据管理
	private ChildMgr _m_mgrChildMgr;
	//成年子嗣数据管理
	private AdultMgr _m_mgrAdultMgr;
	//针对玩家的指定联姻请求数据管理
	private ToMeMarryApplyMgr _m_mgrToMeMarryApplyMgr;
	//子嗣收益数值
	private long _m_lBonusSum;
	//子嗣收益计算任务
    private LazyTaskDealer _m_lazyCalBonusDealer;
	//总赚速
	private long _m_earnings;

	public ChildComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.CHILD);
		
		_m_mgrChildMgr = new ChildMgr(this);
		_m_mgrAdultMgr = new AdultMgr(this);
		_m_mgrToMeMarryApplyMgr = new ToMeMarryApplyMgr(this);
		
		_m_lazyCalBonusDealer = new LazyTaskDealer(new AllChildBonusCalTask(this), 100);
	}
	
	public ChildMgr getChildMgr() {return _m_mgrChildMgr;}
	public AdultMgr getAdultMgr() {return _m_mgrAdultMgr;}
	public ToMeMarryApplyMgr getToMeMarryApplyMgr() {return _m_mgrToMeMarryApplyMgr;}
	
	public long getBonusSum() {return _m_lBonusSum;}
	public void setBonusSum(long _bonus) {_m_lBonusSum = _bonus;}
	
	/**
	 * 子嗣收益计算任务
	 */
	public void recalBonus()
    {
		_m_lazyCalBonusDealer.setNeedDeal();
    }

	@Override
	protected void _init() 
	{
		//----------- 初始化流程 -----------//
		//步骤1：对玩家的请求数据加载
		//步骤2：子嗣（未成年）训练位数据加载
		//步骤3：子嗣（未成年）数据加载
		//步骤4：子嗣（成年）数据加载
		//步骤5：子嗣（成年已婚）数据加载
		//步骤6：子嗣（成年未婚申请）数据加载
		//步骤7：子嗣（成年）联姻池数据检查
		final ALProcess process = ALProcess.CreateProcess("child_comp_init");
		//步骤1: 对玩家的请求数据加载
		process.addResDelegateProcess(_doneAction -> getToMeMarryApplyMgr()._initFromDB(
	              _isSucc ->
	              {
	                  _doneAction.dealAction(_isSucc);
	              })
			        , "child_to_me_apply_init"
			        , () -> //出现异常时的处理
			        {
			            USLog.error(getUSServer(), "player:{} load child to-me-apply bo fail.", getUserData().getCid());
			        }
			        , false);
		//步骤2: 子嗣（未成年）训练位数据加载
		process.addResDelegateProcess(_doneAction -> getChildMgr()._initChildSeatFromDB(
	              _isSucc ->
	              {
	                  _doneAction.dealAction(_isSucc);
	              })
			        , "child_seat_init"
			        , () -> //出现异常时的处理
			        {
			            USLog.error(getUSServer(), "player:{} load child seat bo fail.", getUserData().getCid());
			        }
			        , false);
		//步骤3: 子嗣（未成年）数据加载
		process.addResDelegateProcess(_doneAction -> getChildMgr()._initChildFromDB(
              _isSucc ->
              {
                  _doneAction.dealAction(_isSucc);
              })
		        , "child_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load child bo fail.", getUserData().getCid());
		        }
		        , false);
		//步骤4: 子嗣（成年）数据加载
		process.addResDelegateProcess(_doneAction -> getAdultMgr()._initAdultFromDB(
              _isSucc ->
              {
                  _doneAction.dealAction(_isSucc);
              })
		        , "adult_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load adult bo fail.", getUserData().getCid());
		        }
		        , false);
		//步骤5: 子嗣（成年已婚）数据加载
		process.addResDelegateProcess(_doneAction -> getAdultMgr()._initMarriedAdultFromDB(
              _isSucc ->
              {
                  _doneAction.dealAction(_isSucc);
              })
		        , "adult_married_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load adult married bo fail.", getUserData().getCid());
		        }
		        , false);
		//步骤6: 子嗣（成年未婚申请）数据加载
		process.addResDelegateProcess(_doneAction -> getAdultMgr()._initMarriyApplyFromDB(
              _isSucc ->
              {
                  _doneAction.dealAction(_isSucc);
              })
		        , "adult_marry_apply_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load adult marry player apply bo fail.", getUserData().getCid());
		        }
		        , false);
		//步骤7：子嗣（成年）联姻池数据检查
		process.addResDelegateProcess(_doneAction -> getAdultMgr()._initCheckServerPool(
              _isSucc ->
              {
                  _doneAction.dealAction(_isSucc);
              })
		        , "adult_marry_pool_check"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} check adult pool apply fail.", getUserData().getCid());
		        }
		        , false);
		
		//开启执行
		process.dealProcess(new _IEZProcessMonitorNoTimeOut()
		{
		    //异常终止的事件函数
		    @Override
		    public void onRootProecssStop()
		    {
		    	USLog.error(getUSServer(), "player:{} init child fail.", getUserData().getCid());
		        getUserData().setDataLoadFail();
		    }
		
		    //正常结束的事件函数
		    @Override
		    public void onRootProecssSuc()
		    {
		        setInited();
		    }
		
		    @Override
		    public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
		    {
		    	USLog.error(getUSServer(), "player:{} init child error:{}.", getUserData().getCid(), _ex.getMessage());
		    	USLog.error(getUSServer(), "", _ex);
		    }
		});
	}
	
	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
		//未成年子嗣初始化
		getChildMgr()._onInited();
		//成年子嗣初始化
		getAdultMgr()._onInited();
		
		//子嗣收益数值
		_m_lBonusSum = getChildMgr().getBonusSum() + getAdultMgr().getBonusSum();
		//替换产出速度
		replaceEarnings(0, _m_lBonusSum, true);
		
		//监听属性变化
		getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().addHandler(this, new HandlerThree<ENPPlayerPropertyType, Long, Long>()
        {
            @Override
            public void handle(ENPPlayerPropertyType _type, Long _preValue, Long _curVal)
            {
            	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ON_PLAYER_PROPERTY_CHG);
            	
            	//修改训练位脑力值上限
                if(ENPPlayerPropertyType.CHILD_SEAT_ENERGY_NUM == _type)
                {
                	getChildMgr().onMaxEnergyChg(_preValue, _curVal, context);
                }
            }
        });
	}

	@Override
	public void dispose() 
	{
		getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
	}
	
	/***************
	 * 检查子嗣名称是否重复使用
	 * @param _name
	 * @return
	 */
	public boolean hasName(String _name)
	{
		getUserData().lockUser();
		
		try
		{
			//GOD-5386 【优化-1】学徒-取名允许重名 https://www.teambition.com/task/6687a15b5796a5d16cbc6d53
			return false;
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 未成年子嗣转为已成年子嗣
	 * @param _childId
	 * @param _context
	 * @return
	 */
	public UnmarryAdultInfo tranChildToAdult(long _childId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//移除未成年子嗣
			ChildInfo child = getChildMgr()._delChild(_childId, _context);
			if(null == child)
				return null;

			//领取毕业奖励（毕业奖励需要保存，用于当作对方的联姻奖励）
			NPCommon_ItemInfo graduateItem = new NPCommon_ItemInfo();
			//计算毕业奖励
	        RefBasicAttr basicAttrRef = RefBasicAttr.getMgr().get(child.getAttr().ordinal());
	        ConsortInfo consort = getUserData().getConsortComponent().lookup(child.getConsortId());
	        if(null != consort && null != consort.getFettersInfo().getLvlRef() && null != basicAttrRef)
	        {
	        	graduateItem.setItemType(basicAttrRef.graduate_item.getItemType().ordinal());
	        	graduateItem.setSubId(basicAttrRef.graduate_item.getItemId());
	        	graduateItem.setCount(consort.getFettersInfo().getLvlRef().graduate_get_item_num);
	        
	        	//领取毕业奖励
	        	getUserData().gainItemP(graduateItem, _context);
	        }
			
			//创建成年未婚子嗣
			UnmarryAdultInfo adult = getAdultMgr()._createAdult(child, graduateItem, _context);

			//卷王子嗣毕业：第一个无视概率直接发放宴会凭证，后续按概率发放
			if (adult != null && child.isGiftde())
			{
				boolean isFirst = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.GIFTDE_CHILD_GRADUATE_COUNT) == 0;
				int per = RefGeneral.Ref().giftde_child_dinner_permit_per;
				// 第一个卷王子嗣无视概率，直接发放；否则按概率发放
				if (isFirst || (per > 0 && CommonFunc.randomInt_noInclude(10000) < per))
				{
					getUserData().getDinnerComponent().addPermit(EDinnerPermitType.GIFTDE_CHILD_CELE, adult.getAdultId(), _context);
				}
				// 累加卷王子嗣毕业历史数量
				getUserData().getPlayerComponent().incParam(ENPPlayerParam.GIFTDE_CHILD_GRADUATE_COUNT);
			}

			return adult;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 替换产出速度
	 */
	public void replaceEarnings(long _preSpeed, long _newSpeed, boolean _isInit)
	{
		getUserData().lockUser();
		try
		{
			_m_earnings -= _preSpeed;
			_m_earnings += _newSpeed;

			//初始化的时候不进行处理，在最后统一调用结算
			if (!_isInit)
			{
				checkExceedMaxEarningsRecord();

				getUserData().getPlayerComponent().setNeedCalEarnings();
			}
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 检查是否超过历史最大赚速
	 */
	public void checkExceedMaxEarningsRecord()
	{
		getUserData().lockUser();
		try{
		    //记录最大赚速
    		if (_m_earnings > getUserData().getPlayerComponent().getParamV(ENPPlayerParam.CHILD_EARNINGS_MAX_RECORD))
    			getUserData().getPlayerComponent().setParam(ENPPlayerParam.CHILD_EARNINGS_MAX_RECORD, _m_earnings);
		}finally
		{
		    getUserData().unlockUser();
		}
	}

	public long getTotalEarning()
	{
		getUserData().lockUser();
		try{
			return _m_earnings;
		}finally
		{
		    getUserData().unlockUser();
		}
	}
}
