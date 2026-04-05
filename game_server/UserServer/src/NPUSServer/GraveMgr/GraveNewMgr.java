package NPUSServer.GraveMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GraveObj.GraveObj_NewInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GraveNewBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 新晋杰出者数据管理
 * 新晋杰出者会在名人堂进行展示
 * @author mj
 *
 */
public class GraveNewMgr 
{
	//归属US服务器对象
	private NPUserServer _m_usUSServer;
	//新晋数据列表
	private ArrayList<GraveNewInfo> _m_alNewInfoList;
	//锁对象
	private MutexObject _m_mutex;
	
	public GraveNewMgr(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;
		
		_m_alNewInfoList = new ArrayList<>();
		
		_m_mutex = new MutexObject();
	}
	
	public NPUserServer getUSServer() {return _m_usUSServer;}

	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public boolean s_init()
	{
		//记录数据
		List<GraveNewBO> boList = getUSServer().getBM().getBM(GraveNewBO.class).s_findAll();
		if(null == boList)
		{
			USLog.error(_m_usUSServer, "GraveRecordMgr.s_init GraveRecordBO fail.");
			return false;
		}
		for(int i = 0; i < boList.size(); i++)
		{
			GraveNewBO bo = boList.get(i);
			if(null == bo)
				continue;
			
			GraveNewInfo info = new GraveNewInfo(getUSServer(), bo);
			_m_alNewInfoList.add(info);
		}
		
		CommonFunc.sortAscList(_m_alNewInfoList, new Comparator<GraveNewInfo>() 
		{
			@Override
			public int compare(GraveNewInfo o1, GraveNewInfo o2) 
			{
				return Long.compare(o1.getEndMs(), o2.getEndMs());
			}
		});
		
		//开启检查过期任务
		ALSynTaskManager.getInstance().regTask(new GraveCheckNewInfoEndTask(getUSServer()));
		
		return true;
	}
	
	/**
	 * 检查所有新晋数据是否结束
	 */
	public void checkAllNewInfoEnd()
	{
		_lock();
		
		try
		{
			ArrayList<GraveNewInfo> removeList = null;
			for(int i = 0; i < _m_alNewInfoList.size(); i++)
			{
				GraveNewInfo info = _m_alNewInfoList.get(i);
				if(null == info)
					continue;
				
				if(!info.isEnd())
					break;
				
				if(null == removeList)
				{
					removeList = new ArrayList<>();
				}
				removeList.add(info);
			}
			
			if(null != removeList)
			{
				for(int i = 0; i < removeList.size(); i++)
				{
					GraveNewInfo info = removeList.get(i);
					
					_m_alNewInfoList.remove(info);
					info._del();
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加新晋杰出者数据
	 * @param _newInfo
	 * @param _endMs
	 */
	public void addGraveNewInfo(GraveObj_NewInfo _newInfo, long _endMs)
	{
		_lock();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			GraveNewBO bo = new GraveNewBO();
			bo.setTitleId(bmObj, _newInfo.getTitleId());
			bo.setCid(bmObj, _newInfo.getCid());
			bo.setEndMs(bmObj, _endMs);
			bo.insert(bmObj);
			
			GraveNewInfo info = new GraveNewInfo(getUSServer(), bo);
			_m_alNewInfoList.add(info);
			
			//推送所有在线玩家
			ALSynTaskManager.getInstance().regTask(()->
			{
				getUSServer().getUsUserMgr().broadCastMessage(US2GCWriter_004_PlayerOp.make_062_OnGraveNewReward());
			});
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查指定玩家是否可以领取新晋玩家庆祝奖励
	 * @param _cid
	 * @return
	 */
	public boolean hasGraveNewReward(long _cid)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alNewInfoList.size(); i++)
			{
				GraveNewInfo info = _m_alNewInfoList.get(i);
				if(null == info)
					continue;
				
				if(!info.hasCongCid(_cid))
					return true;
			}
			
			return false;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取指定称号列表的最新新晋杰出者的数据
	 * @param _titleIdList
	 * @param _list
	 */
	public void makeGraveTitleNewInfoList(ArrayList<Long> _titleIdList, ArrayList<GraveObj_NewInfo> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _titleIdList.size(); i++)
			{
				GraveNewInfo info = getGraveNewInfo(_titleIdList.get(i));
				if(null == info)
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
	 * 获取指定称号最新的杰出者数据
	 * @param _titleId
	 * @return
	 */
	public GraveNewInfo getGraveNewInfo(long _titleId)
	{
		_lock();
		
		try
		{
			for(int i = _m_alNewInfoList.size() - 1; i >= 0; i--)
			{
				GraveNewInfo info = _m_alNewInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.getTitleId() == _titleId)
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
	 * 构造指定玩家新晋杰出者奖励列表
	 * @param _cid
	 * @param _list
	 */
	public void makeGraveNewRewardList(long _cid, ArrayList<GraveObj_NewInfo> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alNewInfoList.size(); i++)
			{
				GraveNewInfo info = _m_alNewInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.hasCongCid(_cid))
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
	 * 祝贺新晋杰出者
	 * @param _cid
	 * @return
	 */
	public ArrayList<Long> congGraveNewRewardList(long _cid)
	{
		_lock();
		
		try
		{
			ArrayList<Long> congNewCidList = new ArrayList<>();
			
			for(int i = 0; i < _m_alNewInfoList.size(); i++)
			{
				GraveNewInfo info = _m_alNewInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.addCid(_cid))
				{
					congNewCidList.add(info.getCid());
				}
			}
			
			return congNewCidList;
		}
		finally
		{
			_unlock();
		}
	}
}
