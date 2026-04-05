package NPUSServer.GraveMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.GraveObj.GraveObj_NewInfo;
import Common.GraveObj.GraveObj_Record;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GraveRecordBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;

/**
 * 杰出者 - 记录数据管理
 * 此处仅记录历史列表，新晋（在名人堂外围展示）的由另外的管理器
 * @author mj
 *
 */
public class GraveRecordMgr 
{
	//归属US服务器对象
	private NPUserServer _m_usUSServer;
	//所有记录，按称号ID进行分类
	private HashMap<Long, ArrayList<GraveObj_Record>> _m_hmRecordListMap;
	//锁对象
	private MutexObject _m_mutex;
	
	public GraveRecordMgr(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;

		_m_hmRecordListMap = new HashMap<>();
		
		_m_mutex = new MutexObject();
	}
	
	public NPUserServer getUSServer() {return _m_usUSServer;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	/**
	 * 构造记录数据
	 * @param _bo
	 * @return
	 */
	private GraveObj_Record toRecord(GraveRecordBO _bo)
	{
		GraveObj_Record info = new GraveObj_Record();
		info.setCid(_bo.getCid());
		info.setAchieveTs(_bo.getAchieveTs());
		
		return info;
	}
	
	public boolean s_init()
	{
		//记录数据
		List<GraveRecordBO> boList = getUSServer().getBM().getBM(GraveRecordBO.class).s_findAll();
		if(null == boList)
		{
			USLog.error(_m_usUSServer, "GraveRecordMgr.s_init GraveRecordBO fail.");
			return false;
		}
		for(int i = 0; i < boList.size(); i++)
		{
			GraveRecordBO bo = boList.get(i);
			if(null == bo)
				continue;
			
			_m_hmRecordListMap.computeIfAbsent(bo.getTitleId(), k -> new ArrayList<>()).add(toRecord(bo));
		}
		
		return true;
	}
	
	/**
	 * 是否有记录
	 * @return
	 */
	public boolean hasRecords()
	{
		_lock();
		
		try
		{
			return _m_hmRecordListMap.size() > 0;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造记录数据列表
	 * @param _titleIdList
	 * @param _curPage
	 * @param _pageCount
	 * @param _list
	 * @return
	 */
	public int makeRecordList(ArrayList<Long> _titleIdList, int _curPage, int _pageCount, ArrayList<GraveObj_Record> _list)
	{
		_lock();
		
		try
		{
			//数据集合总数
			int count = 0;
			//总数据集合
			ArrayList<GraveObj_Record> allRecordList = new ArrayList<>();
			//整理数据集合
			for(int i = 0; i < _titleIdList.size(); i++)
			{
				ArrayList<GraveObj_Record> recordList = _m_hmRecordListMap.get(_titleIdList.get(i));
				if(null == recordList)
					continue;
				
				allRecordList.addAll(recordList);
				count += recordList.size();
			}
			
			if(allRecordList.isEmpty())
				return 0;
			
			//数据集合进行排序，按照达成时间倒叙排列
			CommonFunc.sortAscList(allRecordList, new Comparator<GraveObj_Record>() 
			{
				@Override
				public int compare(GraveObj_Record o1, GraveObj_Record o2) 
				{
					return Integer.compare(o2.getAchieveTs(), o1.getAchieveTs());
				}
			});
			
			//获取数据集合中的指定页数
			int startIdx = (_curPage - 1) * _pageCount;
			if(startIdx < allRecordList.size())
			{
				for(int i = startIdx; i < allRecordList.size(); i++)
				{
					_list.add(allRecordList.get(i));
					
					if(_list.size() >= _pageCount)
						break;
				}
			}
			
			return count;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取随机记录数据
	 * @return
	 */
	public GraveObj_Record lookupRnd()
	{
		_lock();
		
		try
		{
			if(_m_hmRecordListMap.isEmpty())
				return null;
			
			ArrayList<Long> titleIdList = new ArrayList<>(_m_hmRecordListMap.keySet());
			int titleIdx = CommonFunc.randomInt(titleIdList.size() - 1);
			long titleId = titleIdList.get(titleIdx);
			
			ArrayList<GraveObj_Record> recordList = _m_hmRecordListMap.get(titleId);
			if(null == recordList)
				return null;
			int recordIdx = CommonFunc.randomInt(recordList.size() - 1);
			return recordList.get(recordIdx);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加记录
	 * @param _newInfo
	 */
	public void addRecord(GraveObj_NewInfo _newInfo)
	{
		_lock();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			GraveRecordBO bo = new GraveRecordBO();
			bo.setTitleId(bmObj, _newInfo.getTitleId());
			bo.setCid(bmObj, _newInfo.getCid());
			bo.setAchieveTs(bmObj, CommonFunc.getNowTimeSec());
			bo.insert(bmObj);
			
			_m_hmRecordListMap.computeIfAbsent(bo.getTitleId(), k -> new ArrayList<>()).add(toRecord(bo));
		}
		finally
		{
			_unlock();
		}
	}
}
