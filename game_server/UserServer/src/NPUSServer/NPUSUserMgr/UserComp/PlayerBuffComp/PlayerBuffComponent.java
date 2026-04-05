package NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp;

import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_PlayerBuffInfo;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateOne;
import NPCommon.Util.Delegate.ADelegateThree;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.GameObjs.PlayerMarsProperty.PlayerMarsPropertyContainer;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.PlayerBuff.RefPlayerBuff;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.UserComp._ITickableComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBuffBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;


/****************************
 * 用户状态组件
 * @author Administrator
 *
 */
public class PlayerBuffComponent extends _ANPUserComponent implements _IUserItemBasicDealer, _ITickableComponent
{
	//玩家当前有效buff列表
    private ArrayList<PlayerBuffInfo> _m_alBuffInfoList;
    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
    //火星属性加成
    private PlayerMarsPropertyContainer _m_mcMarsPropertyContainer;
    
    //buff变化触发器，参数：buffId，buff层数，buff截至时间（毫秒）
    private ADelegateThree<Long, Integer, Long> _m_bdBuffChgDelegate;
    //buff移除触发器，参数：buffId
    private ADelegateOne<Long> _m_bdBuffDelDelegate;

    public PlayerBuffComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.BUFF_COMP);

        _m_alBuffInfoList = new ArrayList<>();
        
        _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();
        _m_mcMarsPropertyContainer = new PlayerMarsPropertyContainer();
        
        _m_bdBuffChgDelegate = new ADelegateThree<>(this);
        _m_bdBuffDelDelegate = new ADelegateOne<>(this);
    }
    
    public NPPlayerPropertyContainer getPlayerPropertyContainer() {return _m_pcPlayerPropertyContainer;}
    public PlayerMarsPropertyContainer getMarsPropertyContainer() {return _m_mcMarsPropertyContainer;}
    
    public ADelegateThree<Long, Integer, Long> getChgDelegate() { return _m_bdBuffChgDelegate;}
    public ADelegateOne<Long> getDelDelegate() {return _m_bdBuffDelDelegate;}
    
    private void _lock() {getUserData().lockUser();}
    private void _unlock() {getUserData().unlockUser();}

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerBuffBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerBuffBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Buff Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerBuffBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //初始化数据
    private void _initBo(List<PlayerBuffBO> _list)
    {
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerBuffBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefPlayerBuff ref = RefPlayerBuff.getMgr().get(bo.getBuffId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(), "Can not get buff ref for bo[id:" + bo.getId() + ", buffId:" + bo.getBuffId() + "]");
                continue;
            }

            PlayerBuffInfo info = new PlayerBuffInfo(getUserData(), ref, bo);
            _m_alBuffInfoList.add(info);
        }

        //根据结束时间排序
        _sortBuffByEndTime();

        setInited();
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
    	for(int i = 0; i < _m_alBuffInfoList.size(); i++)
    	{
    		PlayerBuffInfo buff = _m_alBuffInfoList.get(i);
    		if(null == buff)
    			continue;
    		
    		buff._onInited();
    	}
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alBuffInfoList.size(); i++)
    		{
    			PlayerBuffInfo info = _m_alBuffInfoList.get(i);
    			if(null == info)
    				continue;
    			
    			info._unregEvent();
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    //移除过期buff
    @Override
    public void tick1Sec()
    {
    	_lock();
    	
    	try
    	{
    		_clearExpired();
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    //buff结束时间排序（顺序：越早结束越靠前）
    private void _sortBuffByEndTime()
    {
        CommonFunc.sortAscList(_m_alBuffInfoList, new Comparator<PlayerBuffInfo>()
        {
            @Override
            public int compare(PlayerBuffInfo arg0, PlayerBuffInfo arg1)
            {
            	//永久buff放后面
            	if(-1 == arg0.getEndMs() && -1 != arg1.getEndMs())
        			return 1;
        		
        		if(-1 != arg0.getEndMs() && -1 == arg1.getEndMs())
        			return -1;
            	
                return Long.compare(arg1.getEndMs(), arg0.getEndMs());
            }
        });
    }
    
    //移除全部过期buff
    private void _clearExpired()
    {
    	ArrayList<PlayerBuffInfo> removeBuffList = null;
    	for(int i = 0; i < _m_alBuffInfoList.size(); i++)
    	{
    		PlayerBuffInfo buff = _m_alBuffInfoList.get(i);
    		if(null == buff)
    			continue;
    		
    		//根据排序，后面的buff都不会超时
    		if(!buff._checkExpired())
    			break;
    		
    		if(null == removeBuffList)
    		{
    			removeBuffList = new ArrayList<>();
    		}
    		removeBuffList.add(buff);
    	}
    	
    	if(null != removeBuffList)
    	{
    		for(int i = 0; i < removeBuffList.size(); i++)
    		{
    			_removeBuff(removeBuffList.get(i), NPPlayerContext.createNew(ENPGameEvent.BUFF_EXPIRED));
    		}
    	}
    }
    
    //移除指定buff
    protected void _removeBuff(PlayerBuffInfo _buff, NPPlayerContext _context)
    {
    	if(!_m_alBuffInfoList.remove(_buff))
    		return;
    	
    	_buff._del(_context);

		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_052_OnPlayerBuffRemove(_buff.getBuffId()));

		//火星属性计算
		getUserData().getMarsComponent().doLazyCalMarsProperty();
    }

    /////////////////////////////////////// item dealer ///////////////////////////////////////
    
	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.BUFF;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		return null != lookupBuff(_itemId) ? 1 : 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		return null != lookupBuff(_itemId);
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		ensureBuff(_itemId, 1, (int) _count, false, _context);
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
		ensureBuff(_itemId, 1, (int) _count, false, _context);
	}

	//移除buff
	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return removeBuff(_itemId, _context);
	}
    
    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    /**
     * 协议数据
     * @param _list
     */
    public void makeProtocol(ArrayList<NPCommon_PlayerBuffInfo> _list)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alBuffInfoList.size(); i++)
            {
                PlayerBuffInfo info = _m_alBuffInfoList.get(i);
                if (null == info)
                    continue;

                _list.add(info.toProto());
            }
        } 
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找对应buff
     * @param _buffId
     * @return
     */
    public PlayerBuffInfo lookupBuff(long _buffId)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alBuffInfoList.size(); i++)
            {
                PlayerBuffInfo info = _m_alBuffInfoList.get(i);
                if (null == info)
                    continue;

                if (info.getBo().getBuffId() == _buffId)
                    return info;
            }

            return null;
        } 
        finally
        {
            _unlock();
        }
    }
    
    /**
     * 确认buff数据，如果存在就修改，不存在则新增
     * @param _buffId
     * @param _layer
     * @param _secs 延长截至时间 秒数
     * @param _isReplace 是否替换
     * @param _context
     * @return
     */
    public PlayerBuffInfo ensureBuff(long _buffId, int _layer, long _secs, boolean _isReplace, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		RefPlayerBuff ref = RefPlayerBuff.getMgr().get(_buffId);
    		if(null == ref)
    		{
    			USLog.error(getUSServer(), "player:{} buff:{} ensure buff fail, not find ref.", getCid(), _buffId);
    			return null;
    		}
    		
    		//检查最大层数
    		int finalLayer = _layer;
    		if(ref.max_layer > 0 && _layer > ref.max_layer)
    			finalLayer = ref.max_layer;
    		
    		PlayerBuffInfo info = lookupBuff(_buffId);
    		if(null != info) //buff已经存在
    		{
    			if(_isReplace) //直接设置
    			{
    				info._set(finalLayer, _secs);
    			}
    			else //原有基础上调整
    			{
    				info._chg(_layer, _secs);
    			}
    		}
    		else //新增buff
    		{
        		//截至时间，针对永久情况（-1）但单独处理
    			long nowMs = CommonFunc.getNowTimeMS();
        		long endMs = nowMs;
        		if(_secs == -1)
        			endMs = -1;
        		else
        			endMs = endMs + _secs * 1000;

    			BM bmObj = getUSServer().getBM();
    			
    			PlayerBuffBO bo = new PlayerBuffBO();
    			bo.setCid(bmObj, getUserData().getCid());
    			bo.setBuffId(bmObj, _buffId);
    			bo.setLayer(bmObj, finalLayer);
    			bo.setStartTimeMs(bmObj, nowMs);
    			bo.setEndTimeMs(bmObj, endMs);
    			bo.insert(bmObj);
    			
    			info = new PlayerBuffInfo(getUserData(), ref, bo);
    			_m_alBuffInfoList.add(info);
    			
    			//buff变更事件
    			getUserData().getBuffComponent().getChgDelegate().onAsyncEvent(info.getBuffId(), info.getLayer(), info.getEndMs());
    		}

    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_053_OnPlayerBuffChg(info));
    		
    		//重新排序
    		_sortBuffByEndTime();
    		
    		//火星属性计算
    		getUserData().getMarsComponent().doLazyCalMarsProperty();
    		
    		return info;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 移除buff
     * @param _buffId
     * @param _context
     */
    public boolean removeBuff(long _buffId, NPPlayerContext _context)
    {
    	_lock();

    	try
    	{
    		for(int i = 0; i < _m_alBuffInfoList.size(); i++)
    		{
    			PlayerBuffInfo info = _m_alBuffInfoList.get(i);
        		if(null == info)
        			continue;

        		if(info.getBuffId() == _buffId)
        		{
        			_removeBuff(info, _context);
        			return true;
        		}
    		}
    		
    		return false;
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /**
     * 获取层数大于1的buff信息
     * @return 格式化的buff信息字符串
     */
    public String getMultiLayerBuffs()
    {
        _lock();

        try
        {
            StringBuilder result = new StringBuilder();
            result.append("当前层数大于1的buff列表:\n");

            boolean hasMultiLayerBuff = false;

            for (int i = 0; i < _m_alBuffInfoList.size(); i++)
            {
                PlayerBuffInfo info = _m_alBuffInfoList.get(i);
                if (null == info)
                    continue;

                int layers = info.getLayer();
                if (layers > 1)
                {
                    result.append("BuffId: ").append(info.getBuffId())
                          .append(", 层数: ").append(layers).append("\n");
                    hasMultiLayerBuff = true;
                }
            }

            if (!hasMultiLayerBuff)
            {
                result.append("当前没有层数大于1的buff");
            }

            return result.toString();
        }
        finally
        {
            _unlock();
        }
    }
    
    /**
     * 检查事件触发，只适用于外部触发
     * @param _buffId
     * @param _evt
     * @param _context
     * @return
     */
    public boolean checkLogicEvt(long _buffId, _ALogicEventBase _evt, NPPlayerContext _context)
    {
        _lock();

        try
        {
        	PlayerBuffInfo info = lookupBuff(_buffId);
        	if(null == info)
        		return false;
        	
        	return info.checkLogicEvt(_evt, _context);
        }
        finally
        {
            _unlock();
        }
    }
    
    @Override
    public String toString()
    {
    	_lock();
    	
    	try
    	{
    		StringBuilder sb = new StringBuilder();
    		sb.append("\nbuff size:").append(_m_alBuffInfoList.size());
    		for(int i = 0; i < _m_alBuffInfoList.size(); i++)
    		{
    			PlayerBuffInfo buff = _m_alBuffInfoList.get(i);
    			if(null == buff)
    				continue;
    			
    			sb.append("\n").append(buff.toString());
    		}
    		
    		return sb.toString();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
