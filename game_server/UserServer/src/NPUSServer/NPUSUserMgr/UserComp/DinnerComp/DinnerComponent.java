package NPUSServer.NPUSUserMgr.UserComp.DinnerComp;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import ALBasicServer.ALProcess._IALProcessMonitor;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.DinnerEnum.EDinnerPermitType;
import Common.DinnerObj.Dinner_JoinerLogList;
import Common.DinnerObj.Dinner_Permit;
import Common.DinnerObj.Dinner_ResultInfo;
import CommonEnum.ECurrency;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Dinner.RefDinnerPermit;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerDinnerBO;
import USDB.Bo.PlayerDinnerEachLogBO;
import USDB.Bo.PlayerDinnerPermitBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class DinnerComponent extends _ANPUserComponent implements _IHandlerHolder
{
	//数据实例ID
	private long _m_lId;
	
	//开宴玩家结算数据
	private Dinner_ResultInfo _m_orOwnerResult;
	//宴会凭证数据
	private ArrayList<DinnerPermitInfo> _m_alPermitList;
	//开宴记录日志
	private DinnerStartLogList _m_slStartLogList;
	//玩家交互记录
	private ArrayList<DinnerLastEachLog> _m_alLastEachLogList;
	
    public DinnerComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.DINNER);

        _m_orOwnerResult = new Dinner_ResultInfo();
        _m_alPermitList = new ArrayList<>();
        _m_slStartLogList = new DinnerStartLogList(this);
        _m_alLastEachLogList = new ArrayList<>();
    }
    
    public DinnerStartLogList getStartLogList() {return _m_slStartLogList;}
    
    @Override
    protected void _init()
    {
    	final ALProcess process = ALProcess.CreateProcess("dinner_comp_init");	
    	//步骤1: 开宴结算数据
    	process.addResDelegateProcess(_doneAction -> _initDinnerFromDB(
                _isSucc ->
                {
                    _doneAction.dealAction(_isSucc);
                })
		        , "dinner_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load dinner fail.", getUserData().getCid());
		        }
		        , false);
    	//步骤2: 宴会凭证数据
    	process.addResDelegateProcess(_doneAction -> _initDinnerPermitFromDB(
                _isSucc ->
                {
                    _doneAction.dealAction(_isSucc);
                })
		        , "dinner_permit_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load dinner permit fail.", getUserData().getCid());
		        }
		        , false);
    	//步骤3：宴会交互记录
    	process.addResDelegateProcess(_doneAction -> _initDinnerEachLogFromDB(
                _isSucc ->
                {
                    _doneAction.dealAction(_isSucc);
                })
		        , "dinner_each_log_init"
		        , () -> //出现异常时的处理
		        {
		            USLog.error(getUSServer(), "player:{} load dinner each-log fail.", getUserData().getCid());
		        }
		        , false);
    	
		//开启执行
		process.dealProcess(new _IALProcessMonitor()
		{
		    @Override
		    public void onTimeoutDone(long _processTimeMS, String _processTag, String _exInfo)
		    {
		    }
		
		    @Override
		    public void onTimeout(long _processTimeMS, String _processTag, String _exInfo)
		    {
		    }
		
		    //异常终止的事件函数
		    @Override
		    public void onRootProecssStop()
		    {
		    	USLog.error(getUSServer(), "player:{} init dinner fail.", getUserData().getCid());
		        getUserData().setDataLoadFail();
		    }
		
		    //正常结束的事件函数
		    @Override
		    public void onRootProecssSuc()
		    {
		        setInited();
		    }
		
		    @Override
		    public void onProcessFailStop(_AALProcess _process)
		    {
		    }
		
		    @Override
		    public void onRootProecssDone()
		    {
		    }
		
		    @Override
		    public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
		    {
		    	USLog.error(getUSServer(), "player:{} init dinner error:{}.", getUserData().getCid(), _ex.getMessage());
		    	USLog.error(getUSServer(), "", _ex);
		    }
		
		    @Override
		    public long monitorTimeMS(String _processTag)
		    {
		        return 0;
		    }
		});
    }
    
    /**
     * 步骤1: 开宴结算数据
     * @param _handler
     */
    private void _initDinnerFromDB(_ICallBackBool _handler)
    {
		getUSServer().getBM().getBM(PlayerDinnerBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerDinnerBO>() {

			@Override
			public void dealSuc(PlayerDinnerBO _bo) 
			{
				_m_lId = _bo.getId();
				
				if(null != _bo.getOwnerResult())
				{
					ByteBuffer buff = ByteBuffer.wrap(_bo.getOwnerResult());
					_m_orOwnerResult.readPackage(buff);
				}
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				_handler.onRunOver(true);
			}
		});
    }
    /**
     * 步骤2: 宴会凭证数据
     * @param _handler
     */
    private void _initDinnerPermitFromDB(_ICallBackBool _handler)
    {
    	getUSServer().getBM().getBM(PlayerDinnerPermitBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerDinnerPermitBO>>()
        {
            @Override
            public void dealSuc(List<PlayerDinnerPermitBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerDinnerPermitBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		DinnerPermitInfo info = new DinnerPermitInfo(getUserData(), bo);
            		_m_alPermitList.add(info);
            	}
            	
            	_handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }
        });
    }
    /**
     * 步骤3：宴会交互记录
     * @param _handler
     */
    private void _initDinnerEachLogFromDB(_ICallBackBool _handler)
    {
    	getUSServer().getBM().getBM(PlayerDinnerEachLogBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerDinnerEachLogBO>>()
        {
            @Override
            public void dealSuc(List<PlayerDinnerEachLogBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerDinnerEachLogBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		DinnerLastEachLog log = new DinnerLastEachLog(getUserData(), bo);
            		_m_alLastEachLogList.add(log);
            	}
            	
            	//按 最后一次更新时间 顺序排列
            	CommonFunc.sortAscList(_m_alLastEachLogList, new Comparator<DinnerLastEachLog>() 
            	{
					@Override
					public int compare(DinnerLastEachLog o1, DinnerLastEachLog o2) 
					{
						return Integer.compare(o1.getLastTs(), o2.getLastTs());
					}
				});
            	
            	_handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }
        });
    }

    @Override
    public NPCommonEnum.ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    	//监听玩家属性，更新玩家开宴会加成数据
        getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().addHandler(this, new HandlerThree<ENPPlayerPropertyType, Long, Long>()
        {
            @Override
            public void handle(ENPPlayerPropertyType _type, Long _preValue, Long _curVal)
            {
            	if(ENPPlayerPropertyType.DINNER_OWNER_SCORE_PER == _type)
            	{
            		DinnerInfo dinner = getUSServer().getDinnerPool().lookupByOwnerCid(getUserData().getCid());
            		if(null != dinner)
            		{
            			dinner.setScoreAddPer(_curVal);
            		}
            	}
            }
        });
    }

    @Override
    public void dispose()
    {
    	getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
    }
    
    /**
     * 刷新宴会凭证，移除过期数据
     * @param _context
     */
    public void refreshPermit(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = _m_alPermitList.size() - 1; i >= 0; i--)
    		{
    			DinnerPermitInfo info = _m_alPermitList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.isExpired())
    			{
    				_m_alPermitList.remove(i);
    				info.discard();
    			}
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造平衡列表协议
     * @param _list
     */
    public void makePermitProto(ArrayList<Dinner_Permit> _list)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//刷新凭证数据，移除过期数据
    		refreshPermit(getUserData().getPlayerInitContext());
    		
    		//构造凭证数据
    		for(int i = 0; i < _m_alPermitList.size(); i++)
    		{
    			DinnerPermitInfo info = _m_alPermitList.get(i);
    			if(null == info)
    				continue;
    			
    			_list.add(info.toProto());
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 刷新交互记录，移除过期数据
     * @param _context
     */
    public void refreshLastEachLogProto(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		while(_m_alLastEachLogList.size() > 0)
    		{
    			DinnerLastEachLog log = _m_alLastEachLogList.get(0);
    			if(null == log)
    			{
    				_m_alLastEachLogList.remove(0);
    				continue;
    			}
    			//过期数据
    			if(log.isExpired())
    			{
    				_m_alLastEachLogList.remove(0);
    				log.discard();
    				continue;
    			}
    			
    			break;
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造交互数据列表协议
     * @param _list
     */
    public void makeLastEachLogProto(ArrayList<Dinner_JoinerLogList> _list)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//刷新交互记录，移除过期数据
    		refreshLastEachLogProto(getUserData().getPlayerInitContext());
    		
    		//按 最后一次交互时间 倒叙排列展示
    		for(int i = _m_alLastEachLogList.size() - 1; i >= 0; i--)
    		{
    			DinnerLastEachLog log = _m_alLastEachLogList.get(i);
    			if(null == log)
    				continue;
    			
    			_list.add(log.toProto());
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 设置开宴奖励
     * @param _result
     * @param _context
     */
    public void addOwnerReward(Dinner_ResultInfo _result, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		_m_orOwnerResult = new Dinner_ResultInfo();
    		_m_orOwnerResult.readPackage(_result.makePackage());
    		
    		//更新bo数据
    		_updateBo();
    	}
    	finally 
    	{
    		getUserData().unlockUser();
		}
    }
    /**
     * 更新宴会基础数据
     */
    private void _updateBo()
    {
    	if(0 == _m_lId) //数据不存在，则insert
    	{
    		PlayerDinnerBO bo = new PlayerDinnerBO();
    		bo.setCid(getUSServer().getBM(), getUserData().getCid());
    		bo.setOwnerResult(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_orOwnerResult.makePackage()));
    		bo.insert(getUSServer().getBM());
    		
    		_m_lId = bo.getId();
    	}
    	else //数据已经存在，则update
    	{
    		ALMySqlUpdateValue updateV = new ALMySqlUpdateValue();
    		updateV.addValueObj("owner_result", _m_orOwnerResult.makePackage());

			getUSServer().getBM().getBM(PlayerDinnerBO.class).update("id", _m_lId, updateV);
    	}
    }

    /**
     * 是否有开宴奖励
     * @return
     */
    public boolean hasOwnerReward()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		return _m_orOwnerResult.getInstanceId() > 0;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 增加宴会凭证（宴会凭证类型）
     * @param _permitType
     * @param _typeId
     * @param _context
     */
    public void addPermit(EDinnerPermitType _permitType, long _typeId, NPPlayerContext _context)
    {
    	RefDinnerPermit ref = RefDinnerPermit.getMgr().get(_permitType.ordinal());
    	if(null == ref)
    	{
    		USLog.error(getUSServer(), "player:{} type:{} add dinner-permit fail, not find ref.", getUserData().getCid(), _permitType);
    		return;
    	}
    	
    	addPermit(ref, _typeId, _context);
    }
    /**
     * 增加宴会凭证
     * @param _ref
     * @param _typeId
     * @param _context
     */
    public void addPermit(RefDinnerPermit _ref, long _typeId, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		PlayerDinnerPermitBO bo = new PlayerDinnerPermitBO();
    		bo.setCid(getUSServer().getBM(), getUserData().getCid());
    		bo.setPermitType(getUSServer().getBM(), _ref.permit_type.ordinal());
    		bo.setTypeId(getUSServer().getBM(), _typeId);
    		bo.setExpiredTs(getUSServer().getBM(), CommonFunc.getNowTimeSec() + _ref.lifeTs);
    		bo.insert(getUSServer().getBM());
    		
    		DinnerPermitInfo info = new DinnerPermitInfo(getUserData(), bo, _ref);
    		_m_alPermitList.add(info);
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_019_DinnerOp.make_052_OnPermitAdd(info));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 获取指定的宴会凭证
     * @param _id
     * @param _context
     * @return
     */
    public DinnerPermitInfo getAndDelPermit(long _id, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		DinnerPermitInfo permit = null;
    		
    		for(int i = 0; i < _m_alPermitList.size(); i++)
    		{
    			DinnerPermitInfo tmpPermit = _m_alPermitList.get(i);
    			if(null == tmpPermit)
    				continue;
    			
    			if(tmpPermit.getId() == _id)
    			{
    				_m_alPermitList.remove(i);
    				tmpPermit.discard();
    				
    				permit = tmpPermit;
    				break;
    			}
    		}
    		
    		//检查是否过期
    		if(null != permit && permit.isExpired())
    			permit = null;
    		
    		return permit;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 领取开宴奖励
     * @param _context
     * @return
     */
    public Dinner_ResultInfo takeOwnerReward(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(null == _m_orOwnerResult)
    			return null;
    		
    		Dinner_ResultInfo result = _m_orOwnerResult;
    		//领取宴会币
    		getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.DINNER_COIN.ordinal(), result.getGainCoin(), _context);
    		
    		//清空赴宴奖励数据
    		_m_orOwnerResult = new Dinner_ResultInfo();
    		//更新bo数据
    		_updateBo();
    		
    		return result;
    	}
    	finally 
    	{
    		getUserData().unlockUser();
		}
    }

    /**
     * 获取指定玩家的交互记录数据
     * @param _targetCid
     * @return
     */
    public DinnerLastEachLog lookupLastEachLog(long _targetCid)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alLastEachLogList.size(); i++)
    		{
    			DinnerLastEachLog log = _m_alLastEachLogList.get(i);
    			if(null == log)
    				continue;
    			
    			if(log.getTargetCid() == _targetCid)
    				return log;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 增加加护数据记录
     * @param _isJoined
     * @param _targetCid
     * @param _context
     */
    public void addLastEachLog(boolean _isJoined, long _targetCid, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		DinnerLastEachLog log = lookupLastEachLog(_targetCid);
    		if(null == log)
    		{
    			PlayerDinnerEachLogBO bo = new PlayerDinnerEachLogBO();
    			bo.setCid(getUSServer().getBM(), getUserData().getCid());
    			bo.setTargetCid(getUSServer().getBM(), _targetCid);
    			if(_isJoined)
    				bo.setJoinedCount(getUSServer().getBM(), 1);
    			else
    				bo.setBeJoinedCount(getUSServer().getBM(), 1);
    			bo.setLastTs(getUSServer().getBM(), CommonFunc.getNowTimeSec());
    			bo.insert(getUSServer().getBM());
    			
    			log = new DinnerLastEachLog(getUserData(), bo);
    			_m_alLastEachLogList.add(log);
    		}
    		else
    		{
    			if(_isJoined)
    				log.incrJoinedCount(_context);
    			else
    				log.incrBeJoinedCount(_context);
    		}
    		
        	//按 最后一次更新时间 顺序排列
        	CommonFunc.sortAscList(_m_alLastEachLogList, new Comparator<DinnerLastEachLog>() 
        	{
				@Override
				public int compare(DinnerLastEachLog o1, DinnerLastEachLog o2) 
				{
					return Integer.compare(o1.getLastTs(), o2.getLastTs());
				}
			});
        	
        	//最后保留100条数据
        	while(_m_alLastEachLogList.size() > 100)
        	{
        		DinnerLastEachLog tmpLog = _m_alLastEachLogList.remove(0);
        		if(null != tmpLog)
        		{
        			tmpLog.discard();
        		}
        	}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
}
