package NPUSServer.GraveMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.Common_LongList;
import Common.GraveObj.GraveObj_NewInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import USDB.Bo.GraveNewBO;

import java.nio.ByteBuffer;
import java.util.HashSet;

/**
 * 杰出者 - 新晋数据
 * @author mj
 *
 */
public class GraveNewInfo 
{
	//归属US服务器对象
	private NPUserServer _m_usUSServer;
	//bo数据
	private GraveNewBO _m_bo;
	//已经领取奖励的玩家CID列表
	private HashSet<Long> _m_hsCidSet;
	//锁对象
	private MutexAtom _m_mutex;
	
	public GraveNewInfo(NPUserServer _usServer, GraveNewBO _bo)
	{
		_m_usUSServer = _usServer;
		_m_bo = _bo;
		
		_m_hsCidSet = new HashSet<>();
		if(null != _m_bo.getCongCids())
		{
			Common_LongList listObj = new Common_LongList();
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getCongCids());
			listObj.readPackage(buff);
			
			_m_hsCidSet.addAll(listObj.getValueList());
		}
		
		_m_mutex = new MutexAtom();
	}
	
	public NPUserServer getUSServer() {return _m_usUSServer;}
	
	public GraveNewBO getBo() {return _m_bo;}
	public long getTitleId() {return getBo().getTitleId();}
	public long getCid() {return getBo().getCid();}
	public long getEndMs() {return getBo().getEndMs();}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public GraveObj_NewInfo toProto()
	{
		GraveObj_NewInfo proto = new GraveObj_NewInfo();
		proto.setTitleId(getTitleId());
		proto.setCid(getCid());
		
		return proto;
	}
	
	/**
	 * 检查是否过期
	 * @return
	 */
	public boolean isEnd()
	{
		return CommonFunc.getNowTimeMS() > getEndMs();
	}
	
	/**
	 * 检查玩家是否祝贺
	 * @param _cid
	 * @return
	 */
	public boolean hasCongCid(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hsCidSet.contains(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加玩家祝贺数据
	 * @param _cid
	 * @return
	 */
	public boolean addCid(long _cid)
	{
		_lock();
		
		try
		{
			if(!_m_hsCidSet.add(_cid))
				return false;
			
			Common_LongList listObj = new Common_LongList();
			listObj.getValueList().addAll(_m_hsCidSet);
			_m_bo.setCongCids(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(listObj.makePackage()));
			_m_bo.saveAll(getUSServer().getBM());
			
			return true;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除数据
	 */
	protected void _del() 
	{
		_lock();
		
		try
		{
			_m_bo.del(getUSServer().getBM());
		}
		finally
		{
			_unlock();
		}
	}
}
