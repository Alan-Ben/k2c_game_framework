package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ChildEnum.EAdultStatus;
import Common.ChildObj.Adult_UnmarriedInfo;
import Common.ServerObj.ServerObj_AdultMarriedInfo;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackInt;
import NPCommon.Util.CommonFunc;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_ADULT_MARRY;
import NPUSServer.GeneralV.UsID;
import NPUSServer.MatchAdultMgr.MatchAdultItem;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.ChildSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.LazyDealer.AdultBonusCalTask;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr.ToMeMarryApplyInfo;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer.OfflineDealer_AddultMarryReward;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerAdultBO;
import USDB.Bo.PlayerAdultMarriedBO;
import USDB.Bo.PlayerAdultMarryApplyBO;
import USLOGDB.Bo.LogChildToAdultBO;

import java.util.ArrayList;
import java.util.List;

public class AdultMgr 
{
	//子嗣组件对象
	private ChildComponent _m_comp;
	
	//未婚子嗣数据列表
	private ArrayList<UnmarryAdultInfo> _m_alUnmarryAdultList;
	//已婚子嗣列表
	private ArrayList<MarriedAdultInfo> _m_alMarriedAdultList;
	
	//成年子嗣的收益总和
	private long _m_lBonusSum;
	//子嗣收益计算任务
    private LazyTaskDealer _m_lazyCalBonusDealer;
	
	public AdultMgr(ChildComponent _comp)
	{
		_m_comp = _comp;
		
		_m_alUnmarryAdultList = new ArrayList<>();
		_m_alMarriedAdultList = new ArrayList<>();
		
		_m_lazyCalBonusDealer = new LazyTaskDealer(new AdultBonusCalTask(this), 100);
	}

	public ChildComponent getComp() {return _m_comp;}
	public NPUSUserData getUserData() {return _m_comp.getUserData();}
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	public long getBonusSum() {return _m_lBonusSum;}
	public void setBonusSum(long _bonus) {_m_lBonusSum = _bonus;}

	/**
	 * 子嗣收益计算任务
	 */
	public void recalBonus()
    {
		_m_lazyCalBonusDealer.setNeedDeal();
    }
	
	/**
	 * 初始化子嗣数据，先全部放入 未婚子嗣队列（_m_alUnmarryAdultList）中
	 * @param _handler
	 */
	public void _initAdultFromDB(_ICallBackBool _handler)
	{
        getComp().getUSServer().getBM().getBM(PlayerAdultBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerAdultBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "player:{} load adult bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerAdultBO> _list)
            {
            	_initAdultBo(_list);
                
                _handler.onRunOver(true);
            }
        });
	}
	private void _initAdultBo(List<PlayerAdultBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerAdultBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			UnmarryAdultInfo adult = new UnmarryAdultInfo(_m_comp, bo);
			_m_alUnmarryAdultList.add(adult);
		}
	}
	
	/**
	 * 初始化已婚子嗣数据
	 * @param _handler
	 */
	public void _initMarriedAdultFromDB(_ICallBackBool _handler)
	{
		getComp().getUSServer().getBM().getBM(PlayerAdultMarriedBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerAdultMarriedBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "player:{} load adult married bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerAdultMarriedBO> _list)
            {
            	_initMarriedAdultBo(_list);
                
                _handler.onRunOver(true);
            }
        });
	}
	private void _initMarriedAdultBo(List<PlayerAdultMarriedBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerAdultMarriedBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			//从未婚子嗣数据中获取并移除数据
			UnmarryAdultInfo adult = _takeOutUnmarryAdult(bo.getAdultId());
			if(null == adult)
			{
				USLog.error(getUSServer(), "player:{} adult:{} init married adult fail, not find unmarry-adult.", getUserData().getCid(), bo.getAdultId());
				continue;
			}
			
			//构造已婚子嗣
			MarriedAdultInfo marriedAdult = new MarriedAdultInfo(_m_comp, adult.getBo(), bo);
			_m_alMarriedAdultList.add(marriedAdult);
		}
	}
	
	/**
	 * 初始化子嗣联姻请求数据
	 * @param _handler
	 */
	public void _initMarriyApplyFromDB(_ICallBackBool _handler)
	{
		getComp().getUSServer().getBM().getBM(PlayerAdultMarryApplyBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerAdultMarryApplyBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "player:{} load adult marry-apply bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerAdultMarryApplyBO> _list)
            {
            	_initMarryApplyBo(_list);
                
                _handler.onRunOver(true);
            }
        });
	}
	private void _initMarryApplyBo(List<PlayerAdultMarryApplyBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerAdultMarryApplyBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			UnmarryAdultInfo adult = lookupUnmarryAdult(bo.getAdultId());
			if(null == adult)
			{
				USLog.error(getUSServer(), "player:{} adult:{} init marry apply fail, not find unmarry-adult.", getUserData().getCid(), bo.getAdultId());
				continue;
			}
			
			MarryApplyInfo apply = new MarryApplyInfo(adult, bo);
			adult.setMarryApply(true, apply, getUserData().getPlayerInitContext());
		}
	}
	/**
	 * 检查联姻服务池的子嗣数据
	 */
	public void _initCheckServerPool(_ICallBackBool _handler)
	{
		//未婚子嗣数据检查
		for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
		{
			UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
			if(null == adult)
				continue;
			
			if(!adult.isIdle()) //非空闲的未婚成年子嗣=已发起私人请求，不能再联姻池中
			{
				getUSServer().getMatchAdultPool().removeItem(adult.getAdultId(), getUserData().getPlayerInitContext());
			}
			else
			{
				MatchAdultItem matchItem = getUSServer().getMatchAdultPool().lookupItem(adult.getAdultId());
				if(null != matchItem)
				{
					adult.setMatchItem(true, matchItem, getUserData().getPlayerInitContext());
				}
			}
		}
		
		_handler.onRunOver(true);
	}
	
	/**
	 * 对未婚子嗣据进行刷新，包括：移除过期申请数据
	 * @param _bInited
	 * @param _context
	 */
	private void _refreshUnmarryAdult(boolean _bInited, NPPlayerContext _context)
	{
		for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
		{
			UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
			if(null == adult)
				continue;
		
			if(adult._isExpired())
			{
				adult.setIdle(_bInited, _context);
			}
		}
	}
	
	/**
	 * 获取指定未婚子嗣
	 * @param _adultId
	 * @return
	 */
	private UnmarryAdultInfo _takeOutUnmarryAdult(long _adultId)
	{
		for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
		{
			UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
			if(null == adult)
				continue;
			
			if(adult.getAdultId() == _adultId)
			{
				_m_alUnmarryAdultList.remove(i);
				return adult;
			}
		}
		
		return null;
	}
	
	/**
	 * 初始化完成调用
	 */
	public void _onInited()
	{
		_m_lBonusSum = calBonusSum();
	}

	/**
	 * 计算所有子嗣的上课收益总和
	 * @return
	 */
	public long calBonusSum()
	{
		getUserData().lockUser();
		
		try
		{
			long sum = 0;
			
			//未婚子嗣收益总和
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				sum += adult.getBonus();
			}
			
			//已婚子嗣收益总和
			for(int i = 0; i < _m_alMarriedAdultList.size(); i++)
			{
				MarriedAdultInfo adult = _m_alMarriedAdultList.get(i);
				if(null == adult)
					continue;
				
				sum += adult.getBonus() + adult.getMarriedBonus();
			}

			//已记录子嗣收益
			sum += getUserData().getParam(ENPPlayerParam.ADULT_RECORD_BONUS);
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造未婚子嗣协议
	 * @param _list
	 */
	public void makeUnmarryAdultList(ArrayList<Adult_UnmarriedInfo> _list)
	{
		getUserData().lockUser();
		
		try
		{
			//进行一次刷新，移除过期数据
			_refreshUnmarryAdult(true, getUserData().getPlayerInitContext());
			
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				_list.add(adult.toUnmarriedProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造已婚子嗣协议
	 * @param _list
	 */
	public void makeMarriedAdultList(ArrayList<Long> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alMarriedAdultList.size(); i++)
			{
				_AAdultInfo adult = _m_alMarriedAdultList.get(i);
				if(null == adult)
					continue;
				
				_list.add(adult.getAdultId());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 是否拥有指定成年子嗣（未婚+已婚）
	 * @param _adultId
	 * @return
	 */
	public _AAdultInfo lookupAdult(long _adultId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				if(adult.getAdultId() == _adultId)
					return adult;
			}
			
			for(int i = 0; i < _m_alMarriedAdultList.size(); i++)
			{
				MarriedAdultInfo adult = _m_alMarriedAdultList.get(i);
				if(null == adult)
					continue;
				
				if(adult.getAdultId() == _adultId)
					return adult;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 成年未婚子嗣数量
	 * @return
	 */
	public int getUnmarryAdultCount()
	{
		getUserData().lockUser();
		
		try
		{
			return _m_alUnmarryAdultList.size();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 成年已婚子嗣数量
	 * @return
	 */
	public int getMarriedAdultCount()
	{
		getUserData().lockUser();
		
		try
		{
			return _m_alMarriedAdultList.size();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 从未婚子嗣队列中取出指定子嗣数据
	 * @param _adultId
	 * @return
	 */
	public UnmarryAdultInfo checkIdleAndTakeOutUnmarryAdult(long _adultId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				if(adult.getAdultId() == _adultId)
				{
					if(!adult.isIdle())
						return null;
					
					_m_alUnmarryAdultList.remove(i);
					return adult;
				}
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查指定联姻请求并移除未婚子嗣
	 * @param _adultId
	 * @param _targetApplyCid
	 * @param _context
	 * @return
	 */
	public UnmarryAdultInfo checkApplyAndTakeOutUnmarryAdult(long _adultId, long _targetApplyCid, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				if(adult.getAdultId() == _adultId)
				{
					if(adult.getMarryApplyTargetCid() != _targetApplyCid)
						return null;

					//移除未婚子嗣队列
					_m_alUnmarryAdultList.remove(i);
					//设置子嗣空闲
					adult.setIdle(false, _context);
					
					return adult;
				}
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定未婚子嗣的数据
	 * @param _adultId
	 * @return
	 */
	public UnmarryAdultInfo lookupUnmarryAdult(long _adultId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				if(adult.getAdultId() == _adultId)
					return adult;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找已婚子嗣
	 * @param _adultId
	 * @return
	 */
	public MarriedAdultInfo lookupMarriedAdult(long _adultId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alMarriedAdultList.size(); i++)
			{
				MarriedAdultInfo adult = _m_alMarriedAdultList.get(i);
				if(null == adult)
					continue;
				
				if(adult.getAdultId() == _adultId)
					return adult;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建成年子嗣
	 * @param _consortId
	 * @param _initIntimacy
	 * @param _initResId
	 * @param _quality
	 * @param _careerId
	 * @param _isGiftde
	 * @param _initStudyBonus
	 * @param _name
	 * @param _trainBonus
	 * @param _graduateBonus
	 * @param _graduateItem
	 * @param _context
	 * @return
	 */
	public UnmarryAdultInfo createAdult(long _consortId
			, long _initIntimacy
			, long _initResId
			, long _quality
			, int _attrTypeV
			, long _careerId
			, boolean _isGiftde
			, int _initStudyBonus
			, String _name
			, long _trainBonus
			, long _graduateBonus
			, NPCommon_ItemInfo _graduateItem
			, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getComp().getUSServer().getBM();

			PlayerAdultBO bo = new PlayerAdultBO();
			bo.setId(UsID.makeAdultId(getComp().getUSServer()));
			bo.setCid(bmObj, getUserData().getCid());
			bo.setConsortId(bmObj, _consortId);
			bo.setInitIntimacy(bmObj, _initIntimacy);
			bo.setInitResId(bmObj, _initResId);
			bo.setQuality(bmObj, _quality);
			bo.setAttrType(bmObj, _attrTypeV);
			bo.setCareer(bmObj, _careerId);
			bo.setIsGiftde(bmObj, _isGiftde);
			bo.setInitStudyBonus(bmObj, _initStudyBonus);
			bo.setGraduateTs(bmObj, CommonFunc.getNowTimeSec());
			bo.setName(bmObj, _name);
			bo.setBonus(bmObj, (_trainBonus + _graduateBonus));
			bo.setGraduateItem(bmObj, CommonFunc.ByteBfferToBytes(_graduateItem.makePackage()));
			bo.insert(bmObj);
			
			UnmarryAdultInfo adult = new UnmarryAdultInfo(_m_comp, bo);
			_m_alUnmarryAdultList.add(adult);
			
			//推送未婚子嗣
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_054_OnUnmarriedAdultAdd(adult));

			//计算产出速度
			recalBonus();

			//单个子嗣收益最高记录
			getUserData().getRecordComponent().ensureRecord(ENPPlayerRecordParam.MAX_EARNINGS_CHILD, adult.getBonus(), ENCounterDealType.SET_GT, _context);
	        //获得成年子嗣计数
	        getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.ADULT_GAIN_NUM, 1, _context);
	        
	        //mj日志
	        adult.mjLog(4);
	        
			return adult;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建成年子嗣（只允许ChildComponent调用）
	 * @param _child
	 * @param _graduateItem
	 * @param _context
	 * @return
	 */
	public UnmarryAdultInfo _createAdult(ChildInfo _child, NPCommon_ItemInfo _graduateItem, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//子嗣毕业收益
			long graduateBonus = ChildSystem.calAdultBonus(_child);
			
			//创建成年子嗣
			UnmarryAdultInfo adult = createAdult(_child.getConsortId()
					, _child.getInitIntimacy()
					, _child.getInitResId()
					, _child.getQuality()
					, _child.getAttrV()
					, _child.getCareer()
					, _child.isGiftde()
					, _child.getInitStudyBonus()
					, _child.getName()
					, _child.getTrainBonus()
					, graduateBonus
					, _graduateItem
					, _context);

			//日志
			if(null != adult)
			{
				LogChildToAdultBO logBo = new LogChildToAdultBO();
				logBo.setCid(getUSServer().getBM(), getUserData().getCid());
				logBo.setAdultId(getUSServer().getBM(), adult.getAdultId());
				logBo.setChildId(getUSServer().getBM(), _child.getChildId());
				logBo.setInitRes(getUSServer().getBM(), adult.getInitResId());
				logBo.setTrainBonus(getUSServer().getBM(), _child.getTrainBonus());
				logBo.setGraduateBonus(getUSServer().getBM(), graduateBonus);
				CommLogDB.log(getUSServer().getBM(), logBo, _context);
			}
			
			return adult;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 取消子嗣请求
	 * @param _adultId
	 * @param _context
	 * @return
	 */
	public Result cancelApplyToPlayer(long _adultId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//检查未婚子嗣数据
			UnmarryAdultInfo adult = lookupUnmarryAdult(_adultId);
	        if(null == adult)
	        {
	        	return ChildErr.CHILD_NOT_EXISTS;
	        }
	        
	        //检查子嗣状态
	        if(EAdultStatus.APPLY_PLAYER != adult.getStatus())
	        {
	        	return ChildErr.ADULT_NOT_APPLY_PLAYER;
	        }
	        
	        //移除请求数据，更新子嗣状态
	        adult.checkAndDelMarryApply(0, _context);
	        
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 指定联姻申请被同意的处理
	 * @param _marriedInfo
	 * @param _context
	 * @return
	 */
	public MarriedAdultInfo beAgreedMarriedApplyAdult(ServerObj_AdultMarriedInfo _marriedInfo, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//查找指定成年子嗣数据
			UnmarryAdultInfo adult = checkApplyAndTakeOutUnmarryAdult(_marriedInfo.getAdultId(), _marriedInfo.getMarriedCid(), _context);
			if(null == adult)
				return null; 
			
			//计算联姻子嗣的收益
			long marriedBonusLimit = (long) Math.ceil(1.0f * adult.getBonus() * RefGeneral.Ref().child_married_bonus_add_per / 10000f);
			long realMarriedBonus = Math.min(_marriedInfo.getMarriedAdult().getBonus(), marriedBonusLimit);
			
			//构造已婚子嗣数据
			BM bmObj = getUSServer().getBM();
			
			PlayerAdultMarriedBO marriedBo = new PlayerAdultMarriedBO();
			marriedBo.setCid(bmObj, getUserData().getCid());
			marriedBo.setAdultId(bmObj, adult.getAdultId());
			marriedBo.setMarriedCid(bmObj, _marriedInfo.getMarriedCid());
			marriedBo.setMarriedAdultId(bmObj, _marriedInfo.getMarriedAdult().getId());
			marriedBo.setInitResId(bmObj, _marriedInfo.getMarriedAdult().getInitResId());
			marriedBo.setQuality(bmObj, _marriedInfo.getMarriedAdult().getQuality());
			marriedBo.setAttrType(bmObj, _marriedInfo.getMarriedAdult().getAttrType().ordinal());
			marriedBo.setCareer(bmObj, _marriedInfo.getMarriedAdult().getCareerId());
			marriedBo.setIsGiftde(bmObj, _marriedInfo.getMarriedAdult().getIsGiftde());
			marriedBo.setName(bmObj, _marriedInfo.getMarriedAdult().getName());
			marriedBo.setBonus(bmObj, realMarriedBonus);
			marriedBo.setMarriedTs(bmObj, CommonFunc.getNowTimeSec());
			marriedBo.insert(bmObj);
			
			//构造已婚子嗣数据
			MarriedAdultInfo marriedAdult = new MarriedAdultInfo(_m_comp, adult.getBo(), marriedBo);
			_m_alMarriedAdultList.add(marriedAdult);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_055_OnMarriedAdultAdd(marriedAdult));

			//发送联姻奖励邮件
			AdultMarrySystem.SendAdultMarriedMail(marriedAdult, _marriedInfo.getMarriedCname(), _marriedInfo.getMarriedItem(), _context);

			//增加联姻计数
			getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARRIED_COUNT, 1, _context);

	        //触发事件
			Event_P_ADULT_MARRY event = new Event_P_ADULT_MARRY(_context);
	        getUserData().onLogicEvent(event);
	        
	        //日志数据
	        _AAdultInfo.logMarriedAdult(marriedAdult, _context);
			
			//只保留N条数据
			while(_m_alMarriedAdultList.size() > RefGeneral.Ref().married_adult_limit)
			{
				MarriedAdultInfo removeAdult = _m_alMarriedAdultList.remove(0);
				if(null == removeAdult)
					continue;
				
				removeAdult.discard();
			}

			//计算产出速度
			recalBonus();

	        //mj日志
	        adult.mjLog(5);
			
			return marriedAdult;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 通知指定联姻申请
	 * @param _adultId
	 * @param _applyAdultId
	 * @param _context
	 * @return
	 */
	public void agreeApplyAdult(long _adultId, long _applyAdultId, NPPlayerContext _context, _ICallBackInt _callback)
	{
		getUserData().lockUser();
		
		try
		{
	        //检查请求数据
	        ToMeMarryApplyInfo toMeApply = getComp().getToMeMarryApplyMgr().getAndDel(_applyAdultId, _context);
	        if(null == toMeApply)
	        {
	        	_callback.onRunOver(ChildErr.TO_ME_APPLY_NOT_EXISTS.getCode());
	        	return;
	        }
	        
			//检查对应子嗣
	        UnmarryAdultInfo adult = checkIdleAndTakeOutUnmarryAdult(_adultId);
	        if(null == adult)
	        {
	        	_callback.onRunOver(ChildErr.CHILD_NOT_EXISTS.getCode());
	        	return;
	        }

			//计算联姻子嗣的收益
			long marriedBonusLimit = (long) Math.ceil(1.0f * adult.getBonus() * RefGeneral.Ref().child_married_bonus_add_per / 10000f);
			long realMarriedBonus = Math.min(toMeApply.getBonus(), marriedBonusLimit);
			
	        //预先设置该子嗣结婚数据
	        PlayerAdultMarriedBO marriedBo = new PlayerAdultMarriedBO();
	        marriedBo.setCid(getUSServer().getBM(), getUserData().getCid());
	        marriedBo.setAdultId(getUSServer().getBM(), _adultId);
	        marriedBo.setMarriedCid(getUSServer().getBM(), toMeApply.getApplyCid());
	        marriedBo.setMarriedAdultId(getUSServer().getBM(), toMeApply.getApplyAdultId());
	        marriedBo.setInitResId(getUSServer().getBM(), toMeApply.getApplyAdultInitResId());
	        marriedBo.setQuality(getUSServer().getBM(), toMeApply.getApplyQuality());
	        marriedBo.setAttrType(getUSServer().getBM(), toMeApply.getApplyAttrType().ordinal());
	        marriedBo.setCareer(getUSServer().getBM(), toMeApply.getApplyCareer());
	        marriedBo.setName(getUSServer().getBM(), toMeApply.getApplyName());
			marriedBo.setBonus(getUSServer().getBM(), realMarriedBonus);
	        marriedBo.setMarriedTs(getUSServer().getBM(), CommonFunc.getNowTimeSec());
	        marriedBo.insert(getUSServer().getBM());
	        
	        MarriedAdultInfo marriedInfo = new MarriedAdultInfo(getComp(), adult.getBo(), marriedBo);
	        
	        //发起联姻数据
        	AdultMarrySystem.SendAgreePlayerApply(marriedInfo, _context, _errCode -> 
        	{
        		ALSynTaskManager.getInstance().regTask(()->
        		{
        			_callbackAgreePlayerApply(_errCode, toMeApply, marriedInfo, _context, _callback);
        		});
        	});
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 发起同意联姻后的回调处理
	 * @param _errCode
	 * @param _toMeApply
	 * @param _marriedAdult
	 * @param _context
	 * @param _callback
	 */
	private void _callbackAgreePlayerApply(int _errCode, ToMeMarryApplyInfo _toMeApply, MarriedAdultInfo  _marriedAdult, NPPlayerContext _context, _ICallBackInt _callback)
	{
		getUserData().lockUser();
		
		try
		{
			if(_errCode > 0) //联姻失败，子嗣重回未婚状态
    		{
				_marriedAdult.getMarriedBo().del(getUSServer().getBM());

				UnmarryAdultInfo adult = new UnmarryAdultInfo(_m_comp, _marriedAdult.getBo());
				_m_alUnmarryAdultList.add(adult);
    		}
    		else //联姻成功，子嗣进入已婚子嗣
    		{
    			_m_alMarriedAdultList.add(_marriedAdult);
    			//推送数据
    			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_055_OnMarriedAdultAdd(_marriedAdult));

				//发送联姻奖励邮件
    			AdultMarrySystem.SendAdultMarriedMail(_marriedAdult, _toMeApply.getApplyCname(), _toMeApply.getMarriedItem(), _context);
    			//发送消息弹框
    			OfflineDealer_AddultMarryReward.addAdultMarryReward(_marriedAdult, _toMeApply.getMarriedItem(), _context);
    			
    			//增加联姻计数
    			getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARRIED_COUNT, 1, _context);
    			
    	        //触发事件
    			Event_P_ADULT_MARRY event = new Event_P_ADULT_MARRY(_context);
    	        getUserData().onLogicEvent(event);

    	        //日志数据
    	        _AAdultInfo.logMarriedAdult(_marriedAdult, _context);
    			
    			//只保留N条数据
    			while(_m_alMarriedAdultList.size() > RefGeneral.Ref().married_adult_limit)
    			{
    				MarriedAdultInfo removeAdult = _m_alMarriedAdultList.remove(0);
    				if(null == removeAdult)
    					continue;
    				
    				removeAdult.discard();
    			}

    	        //mj日志
    			_marriedAdult.mjLog(5);
    		}

			//计算产出速度
			recalBonus();
			
			//返回结果
			_callback.onRunOver(_errCode);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 同意联姻池的子嗣
	 * @param _marriedInfo
	 * @param _context
	 * @return
	 */
	public MarriedAdultInfo beAgreedMatchMarriedAdult(ServerObj_AdultMarriedInfo _marriedInfo, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//取出未婚子嗣队列
			UnmarryAdultInfo adult = checkIdleAndTakeOutUnmarryAdult(_marriedInfo.getAdultId());
			if(null == adult)
				return null;

			//计算联姻子嗣的收益
			long marriedBonusLimit = (long) Math.ceil(1.0f * adult.getBonus() * RefGeneral.Ref().child_married_bonus_add_per / 10000f);
			long realMarriedBonus = Math.min(_marriedInfo.getMarriedAdult().getBonus(), marriedBonusLimit);
			
			//构造已婚子嗣数据
			BM bmObj = getUSServer().getBM();

			PlayerAdultMarriedBO marriedBo = new PlayerAdultMarriedBO();
			marriedBo.setCid(bmObj, getUserData().getCid());
			marriedBo.setAdultId(bmObj, adult.getAdultId());
			marriedBo.setMarriedCid(bmObj, _marriedInfo.getMarriedCid());
			marriedBo.setMarriedAdultId(bmObj, _marriedInfo.getMarriedAdult().getId());
			marriedBo.setInitResId(bmObj, _marriedInfo.getMarriedAdult().getInitResId());
			marriedBo.setQuality(bmObj, _marriedInfo.getMarriedAdult().getQuality());
			marriedBo.setAttrType(bmObj, _marriedInfo.getMarriedAdult().getAttrType().ordinal());
			marriedBo.setCareer(bmObj, _marriedInfo.getMarriedAdult().getCareerId());
			marriedBo.setIsGiftde(bmObj, _marriedInfo.getMarriedAdult().getIsGiftde());
			marriedBo.setName(bmObj, _marriedInfo.getMarriedAdult().getName());
			marriedBo.setBonus(bmObj, realMarriedBonus);
			marriedBo.setMarriedTs(bmObj, CommonFunc.getNowTimeSec());
			marriedBo.insert(bmObj);
			
			//构造已婚子嗣数据
			MarriedAdultInfo marriedAdult = new MarriedAdultInfo(_m_comp, adult.getBo(), marriedBo);
			_m_alMarriedAdultList.add(marriedAdult);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_055_OnMarriedAdultAdd(marriedAdult));

			//发送联姻奖励邮件
			AdultMarrySystem.SendAdultMarriedMail(marriedAdult, _marriedInfo.getMarriedCname(), _marriedInfo.getMarriedItem(), _context);

			//增加联姻计数
			getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARRIED_COUNT, 1, _context);

	        //触发事件
			Event_P_ADULT_MARRY event = new Event_P_ADULT_MARRY(_context);
	        getUserData().onLogicEvent(event);
	        
	        //日志数据
	        _AAdultInfo.logMarriedAdult(marriedAdult, _context);
			
			//只保留N条数据
			while(_m_alMarriedAdultList.size() > RefGeneral.Ref().married_adult_limit)
			{
				MarriedAdultInfo removeAdult = _m_alMarriedAdultList.remove(0);
				if(null == removeAdult)
					continue;
				
				removeAdult.discard();
			}

			//计算产出速度
			recalBonus();

	        //mj日志
			marriedAdult.mjLog(5);
			
			return marriedAdult;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 发起同意联姻池里的子嗣
	 * @param _adultId
	 * @param _applyAdultId
	 * @param _context
	 * @param _callback
	 */
	public void agreeMatchAdult(long _adultId, long _applyCid, long _applyAdultId, NPPlayerContext _context, _ICallBackInt _callback)
	{
		getUserData().lockUser();
		
		try
		{
			//检查对应子嗣
	        UnmarryAdultInfo adult = checkIdleAndTakeOutUnmarryAdult(_adultId);
	        if(null == adult)
	        {
	        	_callback.onRunOver(ChildErr.CHILD_NOT_EXISTS.getCode());
	        	return;
	        }
	        
	        //发起联姻数据
        	AdultMarrySystem.SendAgreeGroupApply(adult, _applyCid, _applyAdultId, _context, (_errCode, _marriedAdult) ->
        	{
        		ALSynTaskManager.getInstance().regTask(()->
        		{
        			_callbackAgreeServerApply(_errCode, adult, _marriedAdult, _context, _callback);
        		});
        	});
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 发起同意联姻池里的子嗣
	 * @param _errCode
	 * @param _adult
	 * @param _context
	 * @param _callback
	 */
	private void _callbackAgreeServerApply(int _errCode, UnmarryAdultInfo  _adult, ServerObj_AdultMarriedInfo _beMarriedAdult, NPPlayerContext _context, _ICallBackInt _callback)
	{
		getUserData().lockUser();
		
		try
		{
			if(_errCode > 0) //联姻失败，子嗣重回未婚状态
    		{
				_m_alUnmarryAdultList.add(_adult);
				
				//返回结果
				_callback.onRunOver(_errCode);
    		}
    		else //联姻成功，子嗣进入已婚子嗣
    		{
    			//计算联姻子嗣的收益
    			long marriedBonusLimit = (long) Math.ceil(1.0f * _adult.getBonus() * RefGeneral.Ref().child_married_bonus_add_per / 10000f);
    			long realMarriedBonus = Math.min(_beMarriedAdult.getMarriedAdult().getBonus(), marriedBonusLimit);
    			
    			//预先设置该子嗣结婚数据
    	        PlayerAdultMarriedBO marriedBo = new PlayerAdultMarriedBO();
    	        marriedBo.setCid(getUSServer().getBM(), getUserData().getCid());
    	        marriedBo.setAdultId(getUSServer().getBM(), _adult.getAdultId());
    	        marriedBo.setMarriedCid(getUSServer().getBM(), _beMarriedAdult.getMarriedCid());
    	        marriedBo.setMarriedAdultId(getUSServer().getBM(), _beMarriedAdult.getMarriedAdult().getId());
    	        marriedBo.setInitResId(getUSServer().getBM(), _beMarriedAdult.getMarriedAdult().getInitResId());
    	        marriedBo.setQuality(getUSServer().getBM(), _beMarriedAdult.getMarriedAdult().getQuality());
    	        marriedBo.setAttrType(getUSServer().getBM(), _beMarriedAdult.getMarriedAdult().getAttrType().ordinal());
    	        marriedBo.setCareer(getUSServer().getBM(), _beMarriedAdult.getMarriedAdult().getCareerId());
    	        marriedBo.setName(getUSServer().getBM(), _beMarriedAdult.getMarriedAdult().getName());
    			marriedBo.setBonus(getUSServer().getBM(), realMarriedBonus);
    	        marriedBo.setMarriedTs(getUSServer().getBM(), CommonFunc.getNowTimeSec());
    	        marriedBo.insert(getUSServer().getBM());
    	        
    	        MarriedAdultInfo marriedAdult = new MarriedAdultInfo(getComp(), _adult.getBo(), marriedBo);
    	        _m_alMarriedAdultList.add(marriedAdult);
    	        
    	        //推送数据
    			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_055_OnMarriedAdultAdd(marriedAdult));

				//发送联姻奖励邮件
    			AdultMarrySystem.SendAdultMarriedMail(marriedAdult, _beMarriedAdult.getMarriedCname(), _beMarriedAdult.getMarriedItem(), _context);
    			//发送消息弹框
    			OfflineDealer_AddultMarryReward.addAdultMarryReward(marriedAdult, _beMarriedAdult.getMarriedItem(), _context);

    			//增加联姻计数
    			getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARRIED_COUNT, 1, _context);

    	        //触发事件
    			Event_P_ADULT_MARRY event = new Event_P_ADULT_MARRY(_context);
    	        getUserData().onLogicEvent(event);
    	        
    	        //日志数据
    	        _AAdultInfo.logMarriedAdult(marriedAdult, _context);
    			
    			//只保留N条数据
    			while(_m_alMarriedAdultList.size() > RefGeneral.Ref().married_adult_limit)
    			{
    				MarriedAdultInfo removeAdult = _m_alMarriedAdultList.remove(0);
    				if(null == removeAdult)
    					continue;
    				
    				removeAdult.discard();
    			}

    			//计算产出速度
    			recalBonus();

    	        //mj日志
    			marriedAdult.mjLog(5);
    			
				//返回结果
				_callback.onRunOver(0);
    		}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("\n unMarry adult size:").append(_m_alUnmarryAdultList.size());
			sb.append("\n marred adult size:").append(_m_alMarriedAdultList.size());
			
			for(int i = 0; i < _m_alUnmarryAdultList.size(); i++)
			{
				UnmarryAdultInfo adult = _m_alUnmarryAdultList.get(i);
				if(null == adult)
					continue;
				
				sb.append("\nadult:").append(adult.getAdultId())
					.append(", isGiftde:").append(adult.getIsGiftde())
					.append(", bonus:").append(adult.getBonus());
			}

			for(int i = 0; i < _m_alMarriedAdultList.size(); i++)
			{
				MarriedAdultInfo adult = _m_alMarriedAdultList.get(i);
				if(null == adult)
					continue;
				
				sb.append("\nadult:").append(adult.getAdultId())
					.append(", isGiftde:").append(adult.getIsGiftde())
					.append(", bonus:").append(adult.getBonus())
					.append("| married adult:").append(adult.getMarriedAdultId())
					.append(", bonus:").append(adult.getMarriedBonus());
			}
			
			sb.append("\n remain adult bonus:").append(getUserData().getParam(ENPPlayerParam.ADULT_RECORD_BONUS));
			
			return sb.toString();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
