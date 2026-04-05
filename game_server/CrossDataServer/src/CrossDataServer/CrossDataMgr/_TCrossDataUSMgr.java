package CrossDataServer.CrossDataMgr;

/**
 * 单个类型的数据管理对象基本实现
 * 基于接口进行实现
 */

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;

import java.util.ArrayList;
import java.util.Hashtable;

public class _TCrossDataUSMgr<T extends _IALProtocolStructure, DT extends _ITCrossDataInfo<T>>
{
	//对应US服务器的Id
	private int _m_iUSId;
	private _ATCrossDataMgr<T, DT> _m_dmDataMgr;
	/**
	 * 根据Id存储数据的数据集
	 */
	private Hashtable<Long, DT> _m_htDataTable;
	//数据管理器内锁
	private MutexAtom _m_mutex;

	protected _TCrossDataUSMgr(int _usId, _ATCrossDataMgr<T, DT> _dataMgr)
	{
		_m_iUSId = _usId;
		_m_dmDataMgr = _dataMgr;
		_m_htDataTable = new Hashtable<Long, DT>();

		_m_mutex = new MutexAtom();
		//降低优先级
		_m_mutex.reducePriority(100);
	}

	protected void _lock(){_m_mutex.lock();}
	protected void _unlock(){_m_mutex.unlock();}

    /**
     * 管理的数据类型
     * @return
     */
	public int getUISId() {return _m_iUSId;}


	/**
	 * 同步数据信息
	 * 同步的时候如果不存在数据则需要加入，如果已经存在则需要修改
	 * 实际修改方式需要具体业务类内部进行处理
	 * @param _data
	 * @return 返回创建的新数据，如果只是旧数据同步则不返回
	 *
	 */
	@SuppressWarnings("unchecked")
	protected DT _syncData(long _dataId, T _data)
	{
		if(null == _data)
			return null;

		_lock();

		try {
			DT preData = _m_htDataTable.get(_dataId);
			//如果原数据不存在则添加数据，否则修改已有数据
			if(null != preData) {
				//调用同步数据处理
				preData.syncData(_data);
				return null;
			}

			//添加新数据
			preData = _m_dmDataMgr._createNewDataInfo(_data);
			if(null == preData)
			{
				ALServerLog.Error(this.getClass() + " create data err. errDataId:" + _dataId);
				return null;
			}

			//添加到数据集
			_m_htDataTable.put(preData.getDataId(), preData);

			return preData;
		}
		finally {
			_unlock();
		}
	}

	/**
	 * 同步删除相关数据
	 * @param _dataId
	 * @return 返回删除的数据对象
	 */
	protected DT _rmvData(long _dataId)
	{
		_lock();

		try
		{
			//如无数据删除则不处理
			return _m_htDataTable.remove(_dataId);
		}
		finally {
			_unlock();
		}
	}

	/**
	 * 清理本US数据中的所有数据，返回清理的数据队列
	 */
	protected ArrayList<DT> clearData()
	{
		_lock();

		try
		{
            //从数据集中添加
            ArrayList<DT> clearList = new ArrayList<DT>(_m_htDataTable.values());
			//清理数据集
			_m_htDataTable.clear();

			return clearList;
		}
		finally {
			_unlock();
		}
	}
}
