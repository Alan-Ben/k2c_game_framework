package NPUSServer.GraveMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.GraveObj.GraveObj_NewInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsGravePlayerTitleRecordBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 称号记录数据管理（只记录本服玩家数据）
 * 根据玩家Cid记录此玩家历史曾经获得的所有称号有哪些，如果重复拿到称号会重复显示
 * @author mj
 *
 */
public class GravePlayerTitleRecordMgr 
{
	//归属US服务器对象
	private NPUserServer _m_usUSServer;
	//数据记录集合
	private HashMap<Long, ArrayList<Long>> _m_hmPlayerTitleRecordListMap;
	//锁对象
	private MutexObject _m_mutex;
	
	public GravePlayerTitleRecordMgr(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;
		
		_m_hmPlayerTitleRecordListMap = new HashMap<>();
		
		_m_mutex = new MutexObject();
	}
	
	public NPUserServer getUSServer() {return _m_usUSServer;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}

	public boolean s_init()
	{
		//记录数据
		List<UsGravePlayerTitleRecordBO> boList = getUSServer().getBM().getBM(UsGravePlayerTitleRecordBO.class).s_findAll();
		if(null == boList)
		{
			USLog.error(_m_usUSServer, "GravePlayerTitleRecordMgr.s_init UsGravePlayerTitleRecordBO fail.");
			return false;
		}
		for(int i = 0; i < boList.size(); i++)
		{
			UsGravePlayerTitleRecordBO bo = boList.get(i);
			if(null == bo)
				continue;
			
			//移除无效数据
			if(null == bo.getTitleIdList())
			{
				bo.del(getUSServer().getBM());
				continue;
			}
			
			ByteBuffer buff = ByteBuffer.wrap(bo.getTitleIdList());
			Common_LongList listObj = new Common_LongList();
			listObj.readPackage(buff);
			
			_m_hmPlayerTitleRecordListMap.put(bo.getCid(), listObj.getValueList());
		}
		
		return true;
	}
	
	/**
	 * 构造数据列表
	 * @param _cid
	 */
	public ArrayList<Long> getRecordList(long _cid)
	{
		_lock();
		
		try
		{
			ArrayList<Long> list = new ArrayList<>();
			
			if(!_m_hmPlayerTitleRecordListMap.containsKey(_cid))
				return list;
			
			ArrayList<Long> titleIdList = _m_hmPlayerTitleRecordListMap.get(_cid);
			list.addAll(titleIdList);
			
			return list;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加数据记录
	 * @param _newInfo
	 */
	public void addRecord(GraveObj_NewInfo _newInfo)
	{
		_lock();
		
		try
		{
			if(!_m_hmPlayerTitleRecordListMap.containsKey(_newInfo.getCid())) //新增数据
			{
				ArrayList<Long> titleIdList = _m_hmPlayerTitleRecordListMap.computeIfAbsent(_newInfo.getCid(), k -> new ArrayList<>());
				titleIdList.add(_newInfo.getTitleId());
				
				Common_LongList listObj = new Common_LongList();
				listObj.getValueList().addAll(titleIdList);
				
				BM bmObj = getUSServer().getBM();
				
				UsGravePlayerTitleRecordBO bo = new UsGravePlayerTitleRecordBO();
				bo.setCid(bmObj, _newInfo.getCid());
				bo.setTitleIdList(bmObj, CommonFunc.ByteBfferToBytes(listObj.makePackage()));
				bo.insert(bmObj);
			}
			else //修改已有数据
			{
				ArrayList<Long> titleIdList = _m_hmPlayerTitleRecordListMap.get(_newInfo.getCid());
				titleIdList.add(_newInfo.getTitleId());
				
				while(titleIdList.size() > RefGeneral.Ref().grave_title_record_limit)
				{
					titleIdList.remove(0);
				}
				
				Common_LongList listObj = new Common_LongList();
				listObj.getValueList().addAll(titleIdList);
				
				ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
	            updateValue.addValueObj("titleIdList", CommonFunc.ByteBfferToBytes(listObj.makePackage()));
	            getUSServer().getBM().getBM(UsGravePlayerTitleRecordBO.class).update("cid", _newInfo.getCid(), updateValue);
			}
		}
		finally
		{
			_unlock();
		}
	}
}
