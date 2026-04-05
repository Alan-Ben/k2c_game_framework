package NPUSServer.MatchAdultMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.MarryMatch_Service.MarryMatch.MmsDiscardGroup;
import AllRpcData.MarryMatch_Service.MarryMatch.MmsUploadMatchItemList;
import Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo;
import NPCommon.Enum.EUsParam;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.UsAdultGroupApplyBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;

/**
 * 全服联姻池，用于查找指定子嗣的详细数据
 * @author mj
 *
 */
public class MatchAdultPool implements _IHandlerHolder
{
	private NPUserServer _m_usUSServer;

	//分组ID
	private long _m_lGroupId;
	
	//联姻池子嗣Map表，key-子嗣ID
	private HashMap<Long, MatchAdultItem> _m_hmMatchAdultMap;
	//联姻池子嗣List列表，用于遍历检查
	private ArrayList<MatchAdultItem> _m_alMatchAdultList;
	//根据匹配规则分List，用于获取可以匹配的列表
	private HashMap<Integer, ArrayList<MatchAdultItem>> _m_hmMatchItemListMap;
	
	//联姻池序列号
	private long _m_lSerial;
	
	private MutexObject _m_mutex;
	
	public MatchAdultPool(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;

		_m_hmMatchAdultMap = new HashMap<>();
		_m_alMatchAdultList = new ArrayList<>();
		_m_hmMatchItemListMap = new HashMap<>();
		
		_m_mutex = new MutexObject();
	}

	public NPUserServer getUSServer() {return _m_usUSServer;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public long getGroupId() {return _m_lGroupId;}
	/**
	 * 是否本地池，如果是，则不发起跨服请求
	 * @return
	 */
	public boolean isLocal() {return _m_lGroupId == 0;}
	public boolean isCross() {return !isLocal();}
	
	public boolean init()
	{
		//初始化分组ID
		_m_lGroupId = getUSServer().getUSParams().getParam(EUsParam.ADULT_POOL_GROUP_ID);
		
		//加载数据
		List<UsAdultGroupApplyBO> boList = getUSServer().getBM().getBM(UsAdultGroupApplyBO.class).s_findAll();
		if(null == boList)
		{
			USLog.error(_m_usUSServer, "MatchAdultPool UsAdultGroupApplyBO initBo fail.");
			return false;
		}
		for(int i = 0; i < boList.size(); i++)
		{
			UsAdultGroupApplyBO bo = boList.get(i);
			if(null == bo)
				continue;
			
			MatchAdultItem item = new MatchAdultItem(getUSServer(), bo);
			_m_hmMatchAdultMap.put(item.getApplyAdultId(), item);
			_m_alMatchAdultList.add(item);
			_m_hmMatchItemListMap.computeIfAbsent(item.getMatchId(), k -> new ArrayList<>()).add(item);
		}
		
		//对所有请求进行排序
		_sortByApplyExpiredTs();
		//移除过期请求数据
		_checkAndDelApplyExpired();

		//跨服数据需要进行一次上传
		if(isCross())
		{
			_m_lSerial = ALSerializeMaker.makeNewSerialize();
			uploadAllItem(_m_lSerial, _m_lGroupId);
		}
		
		//开启每秒检查tick
		NPPlayerContext tickCheckContext = NPPlayerContext.createNew(ENPGameEvent.ADULT_CHECK_POOL_APPLY);
		ALSynTaskManager.getInstance().regTask(new CheckMatchAdultItemTask(getUSServer(), tickCheckContext));

		//监听跨服实例变更
		
		return true;
	}
	
	/**
	 * 针对请求截至时间戳进行排序，用于避免全遍历
	 */
	private void _sortByApplyExpiredTs()
	{
		//针对applyExpiredTs做排序，用于避免全部遍历
		CommonFunc.sortAscList(_m_alMatchAdultList, new Comparator<MatchAdultItem>() 
		{
			@Override
			public int compare(MatchAdultItem o1, MatchAdultItem o2) 
			{
				return Long.compare(o1.getApplyExpiredTs(), o2.getApplyExpiredTs());
			}
		});
	}
	
	/**
	 * 检查并移除过期数据
	 */
	private void _checkAndDelApplyExpired()
	{
		ArrayList<MatchAdultItem> expiredItemList = null;
		for(int i = 0; i < _m_alMatchAdultList.size(); i++)
		{
			MatchAdultItem item = _m_alMatchAdultList.get(i);
			if(null == item)
				continue;
			
			if(!item.isExpired())
				break;
			
			if(null == expiredItemList)
				expiredItemList = new ArrayList<>();
			
			expiredItemList.add(item);
		}
		
		if(null != expiredItemList)
		{
			for(int i = 0; i < expiredItemList.size(); i++)
			{
				MatchAdultItem item = expiredItemList.get(i);
				if(null == item)
					continue;
				
				_m_alMatchAdultList.remove(item);
				_m_hmMatchAdultMap.remove(item.getApplyAdultId());
				_m_hmMatchItemListMap.get(item.getMatchId()).remove(item);
				
				item._discard();
				
				if(isCross())
				{
					item._quitPool(item.buildNewSerial(), _m_lGroupId);
				}
			}
		}
	}
	
	/**
	 * 更新分组ID
	 * @param _groupId
	 */
	public void chgGroupId(long _groupId)
	{
		_lock();
		
		try
		{
			if(_groupId == _m_lGroupId)
				return;
			
			//跨服联姻池需要通知公共服务器移除数据
			long oriGroupId = _m_lGroupId;
			if(oriGroupId > 0)
			{
				MmsDiscardGroup discardRpc = new MmsDiscardGroup();
				discardRpc.req().setGroupId(oriGroupId);
				discardRpc.req().setUsId(getUSServer().getServerTypeId());
				
				getUSServer().rpc2marryMatch().request(discardRpc, new _ARpcCallBack<MmsDiscardGroup>() 
				{
					@Override
					public void call_back(int _errCode, MmsDiscardGroup _rpc) 
					{
						if(_errCode > 0)
						{
							USLog.error(getUSServer(), "DiscardMarryMatchGroup Group:{} to MMS fail, err:{}", _groupId, _errCode);
						}
					}
				});
			}

			//更新序列号，用于上传操作检查
			_m_lSerial = ALSerializeMaker.makeNewSerialize();
			//更新跨服分组ID
			long oldGroupId = _m_lGroupId;
			_m_lGroupId = _groupId;
			//更新分组ID
			getUSServer().getUSParams().setParam(EUsParam.ADULT_POOL_GROUP_ID, _m_lGroupId);
			
			//检查当前分组是否跨服联姻池
			if(isCross())
			{
				//上传所有申请数据
				uploadAllItem(_m_lSerial, _groupId);
			}
			
			USLog.info(getUSServer(), "adult marry apply group chg, oriGroup:{} curGroup:{}.", oldGroupId, _m_lGroupId);
		}
		finally 
		{
			_unlock();
		}
	}
	
	/**
	 * 上传全部联姻池申请数据
	 * @param _serial
	 * @param _groupId
	 */
	public void uploadAllItem(final long _serial, long _groupId)
	{
		_lock();
		
		try
		{
			//序列号发生改变，说明有新的上传操作发生，本次操作废弃
			if(_serial != _m_lSerial)
				return;
			
			if(_m_alMatchAdultList.isEmpty())
				return;
			
			ArrayList<ServerObj_AdultMarryGroupApplyInfo> itemList = new ArrayList<>();
			for(int i = 0; i < _m_alMatchAdultList.size(); i++)
			{
				MatchAdultItem item = _m_alMatchAdultList.get(i);
				if(null == item)
					continue;

				//更新序列号
				item.buildNewSerial();
				//获取匹配数据
				itemList.add(item.toApplyProto());
			}
			
			MmsUploadMatchItemList rpc = new MmsUploadMatchItemList();
			rpc.req().getItemList().addAll(itemList);
			
			getUSServer().rpc2marryMatch().request(rpc, new _ARpcCallBack<MmsUploadMatchItemList>() 
			{
				@Override
				public void call_back(int _errCode, MmsUploadMatchItemList _rpc) 
				{
					if(_errCode > 0)
					{
						USLog.error(getUSServer(), "MarryMatchPool Upload All Item fail, serial:{} err:{}", _serial, _errCode);
						//3秒后进行重试
						ALSynTaskManager.getInstance().regTask(()->
						{
							uploadAllItem(_serial, _groupId);
						}, 3000);
					}
				}	
			});
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加申请数据
	 * @param _adult
     * @param _minValue
	 * @param _context
	 * @return
	 */
	public MatchAdultItem addItem(UnmarryAdultInfo _adult, long _minValue, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//先移除旧有数据
			removeItem(_adult.getAdultId(), _context);
			
			//增加新数据
			UsAdultGroupApplyBO bo = new UsAdultGroupApplyBO();
			bo.setApplyAdultId(getUSServer().getBM(), _adult.getAdultId());
			bo.setApplyCid(getUSServer().getBM(), _adult.getUserData().getCid());
			bo.setApplyCname(getUSServer().getBM(), _adult.getUserData().getPlayerComponent().getName());
			bo.setInitResId(getUSServer().getBM(), _adult.getInitResId());
			bo.setQuality(getUSServer().getBM(), _adult.getQuality());
			bo.setAttrType(getUSServer().getBM(), _adult.getAttrType().ordinal());
			bo.setCareer(getUSServer().getBM(), _adult.getCareer());
			bo.setIsGiftde(getUSServer().getBM(), _adult.getIsGiftde());
			bo.setName(getUSServer().getBM(), _adult.getName());
			bo.setBonus(getUSServer().getBM(), _adult.getBonus());
			bo.setMarriedItem(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_adult.getGraduateItem().makePackage()));
			bo.setApplyExpiredTs(getUSServer().getBM(), CommonFunc.getNowTimeSec() + RefGeneral.Ref().marry_apply_expired_S);
			bo.setMinBonus(getUSServer().getBM(), _minValue);
            bo.insert(getUSServer().getBM());
			
			MatchAdultItem item = new MatchAdultItem(getUSServer(), bo);
			_m_hmMatchAdultMap.put(_adult.getAdultId(), item);
			_m_alMatchAdultList.add(item);
			_m_hmMatchItemListMap.computeIfAbsent(item.getMatchId(), k -> new ArrayList<>()).add(item);
			
			//需要跨服同步
			if(isCross())
			{
				item._enterPool(item.buildNewSerial(), _m_lGroupId);
			}
			
			return item;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除指定申请数据
	 * @param _adultId
	 * @param _context
	 */
	public MatchAdultItem removeItem(long _adultId, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			MatchAdultItem item = _m_hmMatchAdultMap.remove(_adultId);
			if(null == item)
				return null;
			
			_m_alMatchAdultList.remove(item);
			_m_hmMatchItemListMap.get(item.getMatchId()).remove(item);
			
			item._discard();
			
			if(isCross())
			{
				item._quitPool(item.buildNewSerial(), _m_lGroupId);
			}
			
			return item;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查子嗣，如果符合匹配条件，则移除子嗣
	 * @param _matchId
	 * @param _adult
	 * @param _context
	 * @return
	 */
	public MatchAdultItem checkAndRemoveItem(long _matchId, long _adultId, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			MatchAdultItem item = _m_hmMatchAdultMap.get(_adultId);
			if(null == item)
				return null;
			
			if(item.getMatchId() != _matchId)
				return null;
			
			return removeItem(_adultId, _context);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 查找指定申请数据
	 * @param _adultId
	 * @return
	 */
	public MatchAdultItem lookupItem(long _adultId)
	{
		_lock();
		
		try
		{
			return _m_hmMatchAdultMap.get(_adultId);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查联姻池匹配数据
	 * @param _context
	 */
	public void checkMatchItem(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			_checkAndDelApplyExpired();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取指定匹配数量的子嗣数据列表
	 * @param _adult
	 * @param _count
	 * @return
	 */
	public ArrayList<MatchAdultItem> getMatchItemList(UnmarryAdultInfo _adult, int _count)
	{
		_lock();
		
		try
		{
			ArrayList<MatchAdultItem> matchItemList = _m_hmMatchItemListMap.get(_adult.getMatchId());
			if(null == matchItemList || matchItemList.isEmpty())
				return null;
			
			ArrayList<MatchAdultItem> realMatchItemList = null;
			for(int i = 0; i < matchItemList.size(); i++)
			{
				MatchAdultItem item = matchItemList.get(i);
				if(null == item)
					continue;

				//过滤需要排除的子嗣
				if(item.getApplyAdultId() == _adult.getAdultId())
					continue;
				
				//过滤玩家自身子嗣
				if(item.getApplyCid() == _adult.getUserData().getCid())
					continue;
				
				//构造实际匹配池
				if(null == realMatchItemList)
					realMatchItemList = new ArrayList<>();
				
				realMatchItemList.add(item);
			}
			
			//无对应匹配子嗣
			if(null == realMatchItemList)
				return null;
			
			if(_count >= realMatchItemList.size())
			{
				return realMatchItemList;
			}
			else //从实际随机池中筛选首个用户数据
			{
				ArrayList<MatchAdultItem> resultItemList = new ArrayList<>();
				//当前随机出的是首个数据
				int idx = CommonFunc.randomInt_noInclude(realMatchItemList.size());
				for(int i = 0; i < _count; i++)
				{
					int realIdx = idx % realMatchItemList.size();
					
					MatchAdultItem item = realMatchItemList.get(realIdx);
					resultItemList.add(item);
					
					idx++;
				}
				
				return resultItemList;
			}
		}
		finally
		{
			_unlock();
		}
	}
}
