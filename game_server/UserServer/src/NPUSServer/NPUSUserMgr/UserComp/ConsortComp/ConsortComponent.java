package NPUSServer.NPUSUserMgr.UserComp.ConsortComp;

import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.ConsortEnum.EConsortSourceType;
import Common.ConsortEnum.EConsortStoryType;
import Common.ConsortObj.Consort_Info;
import Common.DinnerEnum.EDinnerPermitType;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.*;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.Refs.Consort.RefConsort;
import NPGameRes.Refs.Consort.RefConsortStory;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_CONSORT;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCGMgr.ConsortCGMgr;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTravel.ConsortTravelMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.USLog;
import USDB.Bo.*;
import USLOGDB.Bo.LogConsortAddBO;
import USLOGDB.Bo.LogSectionConsortV2BO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class ConsortComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
	//数据实例ID
	private long _m_lId;
	//随机邀约的指定妃子ID列表
	private ArrayList<Long> _m_alRandCallConsortIdList;
	
	//家人列表
	private ArrayList<ConsortInfo> _m_alConsortList;
	
	//家人已解锁CG数据管理
	private ConsortCGMgr _m_mgrCGMgr;
	//家人出游数据管理
	private ConsortTravelMgr _m_mgrTravelMgr;
    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
    
    //玩家所有妃子的亲密度数值
    private long _m_lIntimacySum;
    //玩家所有加护值数值
    private long _m_lCharmSum;

    //玩家所有妃子的初始亲密度数值
    private long _m_lInitIntimacySum;
    //玩家所有初始加护值数值
    private long _m_lInitCharmSum;
    
	public ConsortComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.CONSORT);
		
		_m_alRandCallConsortIdList = new ArrayList<>();
		
		_m_alConsortList = new ArrayList<>();
		
		_m_mgrCGMgr = new ConsortCGMgr(getUserData());
		_m_mgrTravelMgr = new ConsortTravelMgr(getUserData());
		_m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();
	}
	
	public ConsortCGMgr getCGMgr() {return _m_mgrCGMgr;}
	public ConsortTravelMgr getTravelMgr() {return _m_mgrTravelMgr;}
    public NPPlayerPropertyContainer getPlayerPropertyContainer() {return _m_pcPlayerPropertyContainer;}

    public long getIntimacySum() {return _m_lIntimacySum;}
    public long getCharm() {return _m_lCharmSum;}
    
    public long getInitIntimacySum() {return _m_lInitIntimacySum;}
    public void addInitIntimacy(long _initIntimacy) {_m_lInitIntimacySum += _initIntimacy;}
    
    public long getInitCharmSum() {return _m_lInitCharmSum;}
    public void addInitCharm(long _initCharm) {_m_lInitCharmSum += _initCharm;}
    
	@Override
	protected void _init() 
	{
		final ALProcess process = ALProcess.CreateProcess("consort_comp_init");
		//初始化加载，加载过程如果出现异常，则直接加载失败
		//步骤1: 家人基础数据加载
        //步骤2: 家人数据加载
		//步骤3: 经营技能数据加载
		//步骤4: 皮肤数据加载
		//步骤5: 加护技能数据加载
		//步骤6: 加载出游相关数据
		//步骤7: 加载已解锁CG（独立数据，不从属ConsortInfo）
		//步骤8: 家人数据整理
		
		//步骤1: 家人基础数据加载
		process.addResDelegateProcess(_doneAction -> _initOtherFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_other_init"
		        , () -> { USLog.error(getUSServer(), "player:{} load consort other fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤2: 家人数据加载
		process.addResDelegateProcess(_doneAction -> _initConsortFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_init"
		        , () -> { USLog.error(getUSServer(), "player:{} load consort fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤3: 经营技能数据加载
		process.addResDelegateProcess(_doneAction -> _initBusinessSkillFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_business_init"
		        , () -> { USLog.error(getUSServer(), "player:{} load consort business skill fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤4: 皮肤数据加载
		process.addResDelegateProcess(_doneAction -> _initSkinFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_skin_init"
		        , () ->  { USLog.error(getUSServer(), "player:{} load consort skin fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤5: 加护技能数据加载
		process.addResDelegateProcess(_doneAction -> _initBlessSkillFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_bless_init"
		        , () -> { USLog.error(getUSServer(), "player:{} load consort bless fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤6: 加载出游相关数据
		process.addResDelegateProcess(_doneAction -> _initTravelFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_travel_init"
		        , () -> { USLog.error(getUSServer(), "player:{} load consort travel fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤7: 加载已解锁CG（独立数据，不从属ConsortInfo）
		process.addResDelegateProcess(_doneAction -> _m_mgrCGMgr._initFromDB(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_cg_init"
		        , () -> { USLog.error(getUSServer(), "player:{} load consort cg fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		//步骤8: 家人数据整理
		process.addResDelegateProcess(_doneAction -> _dealConsort(_isSucc -> { _doneAction.dealAction(_isSucc); }) //处理流程
		        , "consort_deal"
		        , () ->  { USLog.error(getUSServer(), "player:{} deal consort fail.", getUserData().getCid()); } //出现异常时的处理
		        , false);
		
		//开启执行
		process.dealProcess(new _IEZProcessMonitorNoTimeOut()
		{
		    //异常终止的事件函数
		    @Override
		    public void onRootProecssStop()
		    {
		    	USLog.error(getUSServer(), "player:{} init consort fail.", getUserData().getCid());
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
		    	USLog.error(getUSServer(), "player:{} init consort error:{}.", getUserData().getCid(), _ex.getMessage());
		    	USLog.error(getUSServer(), "", _ex);
		    }
		});
	}
	//步骤1: 家人基础数据加载
	private void _initOtherFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortOtherBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerConsortOtherBO>() {

			@Override
			public void dealSuc(PlayerConsortOtherBO _bo) 
			{
				//随机邀约的指定妃子ID列表
				if(null != _bo.getRandCallConsortIds())
				{
					ByteBuffer buff = ByteBuffer.wrap(_bo.getRandCallConsortIds());
					Common_LongList obj = new Common_LongList();
					obj.readPackage(buff);
					
					_m_alRandCallConsortIdList.addAll(obj.getValueList());
				}
				
				_m_lId = _bo.getId();
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				_handler.onRunOver(true);
			}
		});
    
	}
	//步骤2:家人数据加载
	private void _initConsortFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerConsortBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load consort bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerConsortBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerConsortBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		RefConsort ref = RefConsort.getMgr().get(bo.getConsortId());
            		if(null == ref)
            		{
            			USLog.error(getUSServer(), "player:{} init consort:{} bo fail, not find ref."
            					, getUserData().getCid(), bo.getConsortId());
            			continue;
            		}
            		
            		ConsortInfo consort = new ConsortInfo(ConsortComponent.this, bo, ref);
            		_m_alConsortList.add(consort);
            	}
            	
                _handler.onRunOver(true);
            }
        });
	}
	//步骤3: 经营技能数据加载
	private void _initBusinessSkillFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortBusinessSkillBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerConsortBusinessSkillBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load consort business bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerConsortBusinessSkillBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerConsortBusinessSkillBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		ConsortInfo consort = lookup(bo.getConsortId());
            		if(null == consort)
            		{
            			USLog.error(getUSServer(), "player:{} init consort:{} business skill:{} fail, not find consort."
            					, getUserData().getCid(), bo.getConsortId(), bo.getSkillId());
            			continue;
            		}
            		
            		consort.getBusinessSkillMgr()._initBo(bo);
            	}
            	
                _handler.onRunOver(true);
            }
        });
	}
	//步骤4: 皮肤数据加载
	private void _initSkinFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortSkinBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerConsortSkinBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load consort skin bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerConsortSkinBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerConsortSkinBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		ConsortInfo consort = lookup(bo.getConsortId());
            		if(null == consort)
            		{
            			USLog.error(getUSServer(), "player:{} init consort:{} skin:{} fail, not find consort."
            					, getUserData().getCid(), bo.getConsortId(), bo.getSkinId());
            			continue;
            		}
            		
            		consort.getSkinMgr()._initBo(bo);
            	}
            	
                _handler.onRunOver(true);
            }
        });
	}
	//步骤5: 加护技能数据加载
	private void _initBlessSkillFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortBlessSkillBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerConsortBlessSkillBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load consort bless skill bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerConsortBlessSkillBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerConsortBlessSkillBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		ConsortInfo consort = lookup(bo.getConsortId());
            		if(null == consort)
            		{
            			USLog.error(getUSServer(), "player:{} init consort:{} bless skill:{} fail, not find consort."
            					, getUserData().getCid(), bo.getConsortId(), bo.getSkillId());
            			continue;
            		}
            		
            		consort.getBlessSkillInfoMgr()._initBo(bo);
            	}
            	
                _handler.onRunOver(true);
            }
        });
	}
	//步骤6: 加载出游相关数据
	private void _initTravelFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortTravelBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerConsortTravelBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load consort travel bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerConsortTravelBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerConsortTravelBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		getTravelMgr()._initBo(bo);
            	}
            	
                _handler.onRunOver(true);
            }
        });
	}
	//步骤8: 家人数据整理
	private void _dealConsort(_ICallBackBool _handler)
	{
		for(int i = 0; i < _m_alConsortList.size(); i++)
		{
			ConsortInfo info = _m_alConsortList.get(i);
			if(null == info)
				continue;
			
			_m_lInitIntimacySum += info.getInitIntimacy();
			_m_lInitCharmSum += info.getInitCharm();
		}
		
		_handler.onRunOver(true);
	}
	
	public void _initCalAllConsort()
	{
		for(int i = 0; i < _m_alConsortList.size(); i++)
		{
			ConsortInfo consort = _m_alConsortList.get(i);
			if(null == consort)
				continue;
			
			//初始化计算家人属性
			consort._initCalConsort();
			//检查初始化家人的经营技能
			consort.getBusinessSkillMgr().checkUnlockSkill(true);
			
			//挂载属性变更处理Dealer
			consort._initPropertyChgDealer();
		}

		calIntimacySum();
		calCharmSum();
	}
	
	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
	}

	@Override
	public void dispose() 
	{
	}

	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.CONSORT;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		getUserData().lockUser();
		
		try
		{
			return lookup(_itemId) != null ? 1 : 0;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		return null != lookup(_itemId);
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		//数量不为1，需要报错
		if(_count != 1)
		{
			USLog.error(getUSServer(), "player:{} init gain consort:{} count:{} error.", getUserData().getCid(), _itemId, _count);
			return;
		}
		
		_gainConsort(_itemId, _context);
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
	{
		//数量不为1，需要报错
		if(_count != 1)
		{
			USLog.error(getUSServer(), "player:{} gain consort:{} count:{} error.", getUserData().getCid(), _itemId, _count);
			return;
		}
		
		_gainConsort(_itemId, _context);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
	{
		return false;
	}
	
	/**
	 * 计算玩家亲密度
	 */
	public void calIntimacySum()
	{
		getUserData().lockUser();
		
		try
		{
			_m_lIntimacySum = 0;
            for (ConsortInfo info : _m_alConsortList)
            {
                if (null == info)
                    continue;

                _m_lIntimacySum += info.getIntimacy();
            }
			
			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RECORD_UPDATE);
			//更新总亲密度
			getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.PLAYER_CONSORT_INTIMACY, _m_lIntimacySum, context);
			//更新总亲密度增加值
			getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.PLAYER_CONSORT_INTIMACY_ADD, (_m_lIntimacySum - _m_lInitIntimacySum), context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 计算玩家加护值
	 */
	public void calCharmSum()
	{
		getUserData().lockUser();
		
		try
		{
			_m_lCharmSum = 0;
			for(int i = 0; i < _m_alConsortList.size(); i++)
			{
				ConsortInfo info = _m_alConsortList.get(i);
				if(null == info)
					continue;
				
				_m_lCharmSum += info.getCharm();
			}
			
			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RECORD_UPDATE);
			//更新总加护值
			getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.PLAYER_CONSORT_CHARM, _m_lCharmSum, context);
			//更新总加护值增加值
			getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.PLAYER_CONSORT_CHARM_ADD, (_m_lCharmSum - _m_lInitCharmSum), context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/*******************
	 * 构造家人列表数据
	 * @param _list
	 */
	public void makeProto(ArrayList<Consort_Info> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alConsortList.size(); i++)
			{
				ConsortInfo consort = _m_alConsortList.get(i);
				if(null == consort)
					continue;
				
				_list.add(consort.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*********
	 * 检查家人是否为空
	 * @return
	 */
	public boolean isConsortEmpty()
	{
		getUserData().lockUser();
		
		try
		{
			return _m_alConsortList.isEmpty();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*******
	 * 获取全部家人列表数据
	 * @return
	 */
	public ArrayList<ConsortInfo> getConsortList()
	{
		getUserData().lockUser();
		
		try
		{
			return new ArrayList<>(_m_alConsortList);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	public int getConsortNum()
	{
		getUserData().lockUser();

		try
		{
			return _m_alConsortList.size();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*****
	 * 获取全部家人ID列表
	 * @return
	 */
	public ArrayList<Long> getConsortIdList()
	{
		getUserData().lockUser();
		
		try
		{
			return new ArrayList<>();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*************
	 * 查找指定家人数据
	 * @param _consortId
	 * @return
	 */
	public ConsortInfo lookup(long _consortId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alConsortList.size(); i++)
			{
				ConsortInfo info = _m_alConsortList.get(i);
				if(null == info)
					continue;
				
				if(info.getConsortId() == _consortId)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*************
	 * 是否拥有指定家人
	 * @param _consortId
	 * @return
	 */
	public boolean hasConsort(long _consortId)
	{
		return null != lookup(_consortId);
	}
	
	/*******
	 * 随机获取家人数据
	 * @return
	 */
	public ConsortInfo lookupRnd()
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_alConsortList.isEmpty())
				return null;
			
			int idx = CommonFunc.randomInt(_m_alConsortList.size() - 1);
			return _m_alConsortList.get(idx);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取家人
	 * @param _consortId
	 * @param _context
	 * @return
	 */
	protected ConsortInfo _gainConsort(long _consortId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//通过事件类型来定义获取途径
			EConsortSourceType sourceType = EConsortSourceType.NONE;
			if (_context.getContextId() == ENPGameEvent.TRAVEL_GAIN_CONSORT.ordinal())
			{
				sourceType = EConsortSourceType.TRAVEL;
			} else if (_context.getContextId() == ENPGameEvent.CHAPTER_GAIN_CONSORT.ordinal())
			{
				sourceType = EConsortSourceType.CHAPTER;
			}

			//已经获得家人
			if(hasConsort(_consortId))
				return null;
			
			//获取家人配表
			RefConsort ref = RefConsort.getMgr().get(_consortId);
			if(null == ref)
			{
				USLog.error(getUSServer(), "player:{} gain consort:{} fail, not find ref.", getUserData().getCid(), _consortId);
				return null;
			}
			
			//构造家人数据
			BM bmObj = getUSServer().getBM();
			
			PlayerConsortBO bo = new PlayerConsortBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setConsortId(bmObj, _consortId);
			bo.setFettersLvl(bmObj, ref.init_fetters_lvl);
			bo.setIntimacy(bmObj, ref.init_intimacy);
			bo.setCharm(bmObj, ref.init_charm);
			bo.insert(bmObj);
			
			ConsortInfo consort = new ConsortInfo(this, bo, ref);
			_m_alConsortList.add(consort);
			
			//穿戴默认皮肤
			consort.setCurSkin(ref.default_skin_id, _context);

			//计算属性
			consort._initCalConsort();
			//检查初始化家人的经营技能
			consort.getBusinessSkillMgr().checkUnlockSkill(true);
			//挂载属性变更处理对象
			consort._initPropertyChgDealer();
			
			//更新初始数据
			consort._saveInitValue();
			//更新全部初始化总数值
			addInitIntimacy(consort.getInitIntimacy());
			addInitCharm(consort.getInitCharm());

			calIntimacySum();
			calCharmSum();

			//获取额外奖励，不做表现
			NPPlayerContext newContext = NPPlayerContext.createNew(_context);
			getUserData().gainItemList(ref.unlock_gain_item_list, newContext);
			
			//获取宴会凭证
			getUserData().getDinnerComponent().addPermit(EDinnerPermitType.FAMILY, _consortId, _context);
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_050_OnConsortAdd(consort, sourceType));
			
			//关联大臣的战力重新计算
			consort.relationHeroReCal();

			//放入数据
			_context.collectItem(getItemType(), _consortId, 1, false);
			
			//解锁结婚事件的CG
			ArrayList<RefConsortStory> storyRefList = consort.getRef().callStoryRefListMap.get(EConsortStoryType.MARRY.ordinal());
			if(null != storyRefList)
			{
				for(int i = 0; i < storyRefList.size(); i++)
				{
					RefConsortStory story = storyRefList.get(i);
					if(null == story)
						continue;
					
					if(story.unlock_cg > 0)
					{
						getUserData().getConsortComponent().getCGMgr().gainCG(story.unlock_cg, _context);
					}
				}
			}

			//触发事件
			Event_P_GAIN_CONSORT event = new Event_P_GAIN_CONSORT(_context, _consortId);
			getUserData().onLogicEvent(event);

			if (ref.quality == EQuality.RED && _context.getContextId() != ENPGameEvent.GM_CMD.ordinal())
			{
				ArrayList<String> paramList = new ArrayList<>();
				paramList.add(getUserData().getPlayerComponent().getName());
				paramList.add(ref.name);

				getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().marquee_gain_ur_consort_marquee_id, paramList);
			}

            if (_context.getContextId() == ENPGameEvent.RECRUIT.ordinal())
            {
                ArrayList<String> paramList = new ArrayList<>();
                paramList.add(getUserData().getPlayerComponent().getName());
                paramList.add(ref.name);
                getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().recruit_gain_hero_or_consort_marquee_id, paramList);
            }

			//日志数据
			LogConsortAddBO logBo = new LogConsortAddBO();
			logBo.setCid(bmObj, getUserData().getCid());
			logBo.setConsortId(bmObj, consort.getConsortId());
			logBo.setSourceType(bmObj, sourceType.ordinal());
			logBo.setAllIntimacy(bmObj, consort.getIntimacy());
			logBo.setAllCharm(bmObj, consort.getCharm());
			logBo.setAllCharmPointPer(bmObj, consort.getCharmPointPer());
			CommLogDB.log(bmObj, logBo, _context);
			
			return consort;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造指定约随机妃子数据协议
	 * @param _list
	 */
	public void makeRandCallConsortProto(ArrayList<Long> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alRandCallConsortIdList.size(); i++)
			{
				_list.add(_m_alRandCallConsortIdList.get(i));
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加指定邀约随机妃子数据
	 * @param _consortId
	 * @param _context
	 */
	public void addRandCallConsortId(long _consortId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//更新数据
			_m_alRandCallConsortIdList.add(_consortId);
			_updateBo();

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_062_OnRandCallConsortChg(getUserData()));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 弹出首个指定随机邀约妃子数据
	 * @return
	 */
	public ConsortInfo popRandCallConsortId()
	{
		getUserData().lockUser();
		
		try
		{
			while(!_m_alRandCallConsortIdList.isEmpty())
			{
				long consortId = _m_alRandCallConsortIdList.remove(0);
				ConsortInfo consort = lookup(consortId);
				if(null == consort)
					continue;
				
				//保存数据
				_updateBo();
				
				//推送数据
				getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_062_OnRandCallConsortChg(getUserData()));
				
				return consort;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 保存数据
	 */
	private void _updateBo()
	{
		Common_LongList randCallConsortIdListObj = new Common_LongList();
		randCallConsortIdListObj.getValueList().addAll(_m_alRandCallConsortIdList);
		
		if(0 == _m_lId)
		{
			PlayerConsortOtherBO bo = new PlayerConsortOtherBO();
			bo.setCid(getUSServer().getBM(), getUserData().getCid());
			bo.setRandCallConsortIds(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(randCallConsortIdListObj.makePackage()));
			bo.insert(getUSServer().getBM());
			
			_m_lId = bo.getId();
		}
		else
		{
    		ALMySqlUpdateValue updateV = new ALMySqlUpdateValue();
    		updateV.addValueObj("randCallConsortIds", randCallConsortIdListObj.makePackage());

			getUSServer().getBM().getBM(PlayerConsortOtherBO.class).update("id", _m_lId, updateV);
		}
	}

	/**
	 * 获取总亲密度
	 * @return
	 */
	public long getTotalConsortIntimacy()
	{
		getUserData().lockUser();
		try
		{
			long totalIntimacy = 0;
			for (ConsortInfo consort : _m_alConsortList)
			{
				totalIntimacy += consort.getIntimacy();
			}
			return totalIntimacy;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取总加护点数
	 * @return
	 */
	public long getTotalConsortCharm()
	{
		getUserData().lockUser();
		try
		{
			long totalCharm = 0;
			for (ConsortInfo consort : _m_alConsortList)
			{
				totalCharm += consort.getCharm();
			}
			return totalCharm;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 妃子截面数据
	 * 
	 * @param _sectionLogType
	 */
	public void logSection(ELogSectionType _sectionLogType)
    {
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			for(int i = 0; i < _m_alConsortList.size(); i++)
			{
				ConsortInfo info = _m_alConsortList.get(i);
				if(null == info)
					continue;
				
				LogSectionConsortV2BO logBo = new LogSectionConsortV2BO();
		        //截面日志类型
		        logBo.setSectionType(getUSServer().getBM(), _sectionLogType.ordinal());
		        //玩家
		        logBo.setCid(bmObj, getUserData().getCid());
		        //知己id
		        logBo.setConsortId(bmObj, info.getConsortId());
		        logBo.setConsortIntimacy(bmObj, info.getIntimacy());
		        logBo.setConsortCharm(bmObj, info.getCharm());
		        logBo.setConsortSkillPointGain(bmObj, info.getCharmPointRecord());
		        logBo.setConsortFettersLvl(bmObj, info.getFettersInfo().getLvl());
		        logBo.setConsortBusinessSkillLvl(bmObj, info.getBusinessSkillMgr()._sectionLog());
		        logBo.setConsortBlessSkillLvl(bmObj, info.getBlessSkillInfoMgr()._sectionLog());
		        
		        CommLogDB.log(bmObj, logBo);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
    }

	/**
	 * 重新计算所有妃子亲密度
	 */
	public void recalAllConsortIntimacy()
	{
		getUserData().lockUser();
		try
		{
			for (ConsortInfo consortInfo : _m_alConsortList)
			{
				consortInfo.recalIntimacy();
			}
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 重新计算所有妃子魅力值
	 */
	public void recalAllConsortCharm()
	{
		getUserData().lockUser();
		try
		{
			for (ConsortInfo consortInfo : _m_alConsortList)
			{
				consortInfo.recalCharm();
			}
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 删除指定家人及其所有数据库数据（GM命令）
	 * 清理主表、加护技能、经营技能、皮肤等所有关联记录
	 */
	public String cmdDelConsort(long _consortId)
	{
		getUserData().lockUser();
		try
		{
			// 从内存列表中查找并移除
			ConsortInfo info = null;
			for (int i = 0; i < _m_alConsortList.size(); i++)
			{
				ConsortInfo item = _m_alConsortList.get(i);
				if (item != null && item.getConsortId() == _consortId)
				{
					info = item;
					_m_alConsortList.remove(i);
					break;
				}
			}
			if (null == info)
				return "fail, not find consort.";

			// 清理全局属性加成（星辉/羁绊/经营技能）
			info.cmdDiscard();
			// 更新亲密度/加护力初始基线
			_m_lInitIntimacySum -= info.getInitIntimacy();
			_m_lInitCharmSum -= info.getInitCharm();
			// 重新计算总亲密度和总加护值
			calIntimacySum();
			calCharmSum();

			BM bm = getUSServer().getBM();
			// 删除主表记录
			info.getBo().del(bm);

			// 按cid+consortId批量删除所有关联子表记录
			HashMap<String, Object> conditions = new HashMap<>();
			conditions.put("cid", getUserData().getCid());
			conditions.put("consortId", _consortId);
			bm.getBM(PlayerConsortBlessSkillBO.class).delAll(conditions);
			bm.getBM(PlayerConsortBusinessSkillBO.class).delAll(conditions);
			bm.getBM(PlayerConsortSkinBO.class).delAll(conditions);

			return "ok";
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 删除所有家人及其数据库数据（GM命令）
	 */
	public int cmdDelAllConsort()
	{
		getUserData().lockUser();
		try
		{
			int count = 0;
			BM bm = getUSServer().getBM();
			for (ConsortInfo info : _m_alConsortList)
			{
				if (null == info) continue;
				info.cmdDiscard();
				info.getBo().del(bm);

				HashMap<String, Object> conditions = new HashMap<>();
				conditions.put("cid", getUserData().getCid());
				conditions.put("consortId", info.getConsortId());
				bm.getBM(PlayerConsortBlessSkillBO.class).delAll(conditions);
				bm.getBM(PlayerConsortBusinessSkillBO.class).delAll(conditions);
				bm.getBM(PlayerConsortSkinBO.class).delAll(conditions);
				count++;
			}
			_m_alConsortList.clear();
			_m_lInitIntimacySum = 0;
			_m_lInitCharmSum = 0;
			_m_lIntimacySum = 0;
			_m_lCharmSum = 0;
			return count;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 随机妃子
	 */
	public ConsortInfo randGetConsort()
	{
		getUserData().lockUser();
		try
		{
			return CommonFunc.randSelect(_m_alConsortList);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
