package CrossDataServer.CrossDataMgr;

/**
 * 单个类型的数据管理对象基本实现
 * 基于接口进行实现
 */

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Hashtable;

public abstract class _ATCrossDataMgr<T extends _IALProtocolStructure, DT extends _ITCrossDataInfo<T>> implements  _ICrossDataMgr
{
	private long _m_lGroupId;
	/**
	 * 根据Id存储数据的数据集
	 */
	private Hashtable<Integer, _TCrossDataUSMgr<T, DT>> _m_htUSDataTable;
	//数据管理器内锁
	private MutexAtom _m_mutex;

	public _ATCrossDataMgr(long _groupId)
	{
		_m_lGroupId = _groupId;
		_m_htUSDataTable = new Hashtable<Integer, _TCrossDataUSMgr<T, DT>>();
		_m_mutex = new MutexAtom();
	}

	public long getGroupId() {return _m_lGroupId;}

	protected void _lock(){_m_mutex.lock();}
	protected void _unlock(){_m_mutex.unlock();}


	/**
	 * 同步数据信息
	 * 同步的时候如果不存在数据则需要加入，如果已经存在则需要修改
	 * 实际修改方式需要具体业务类内部进行处理
	 * @param _data
	 *
	 */
	@SuppressWarnings("unchecked")
	public void syncData(int _usId, byte[] _data)
	{
		if(null == _data)
			return ;

		T realData = _readData(ByteBuffer.wrap(_data));
		if (null == realData) {
			ALServerLog.Error(this.getClass() + " sync data for err data.");
			return;
		}

		//获取数据Id
		long dataId = _getDataId(realData);

		_lock();

		try {
			_TCrossDataUSMgr<T, DT> usDataMgr = _ensureUSDataMgr(_usId);
			//尝试同步数据，返回是否创建新的
			DT newData = usDataMgr._syncData(dataId, realData);
			if(null != newData)
			{
				//调用子类的添加数据处理，方便子类增加额外的数据管理
				_onAddData_InLock(newData);
			}
		}
		finally {
			_unlock();
		}
	}
	/**
	 * 同步某个服务器的数据队列
	 * @param _dataList
	 */
	public void syncDataList(int _usId, ArrayList<byte[]> _dataList)
	{
		_lock();

		try {
			//先获取US处理对象，避免每次都检索
			_TCrossDataUSMgr<T, DT> usDataMgr = _ensureUSDataMgr(_usId);

			//每个数据进行同步
			for (int i = 0; i < _dataList.size(); i++) {
				byte[] data = _dataList.get(i);
				if (null == data)
					continue;

				T realData = _readData(ByteBuffer.wrap(data));
				if (null == realData) {
					ALServerLog.Error(this.getClass() + " sync data for err data.");
					return;
				}

				//获取数据Id
				long dataId = _getDataId(realData);

				//尝试同步数据，返回是否创建新的
				DT newData = usDataMgr._syncData(dataId, realData);
				if (null != newData) {
					//调用子类的添加数据处理，方便子类增加额外的数据管理
					_onAddData_InLock(newData);
				}
			}
		}
		finally {
			_unlock();
		}
	}

	/**
	 * 同步删除相关数据
	 * @param _dataId
	 */
	public void rmvData(int _usId, long _dataId)
	{
		_lock();

		try {
			_TCrossDataUSMgr<T, DT> usDataMgr = _ensureUSDataMgr(_usId);
			//尝试同步数据，返回是否创建新的
			DT newData = usDataMgr._rmvData(_dataId);
			if(null != newData)
			{
				//调用子类的添加数据处理，方便子类增加额外的数据管理
				_onRmvData_InLock(newData);
			}
		}
		finally {
			_unlock();
		}
	}
	public void rmvData(int _usId, ArrayList<Long> _dataId)
	{
		_lock();

		try {
			_TCrossDataUSMgr<T, DT> usDataMgr = _ensureUSDataMgr(_usId);
			for(Long dataId : _dataId)
			{
				//尝试同步数据，返回是否创建新的
				DT newData = usDataMgr._rmvData(dataId);
				if(null != newData)
				{
					//调用子类的添加数据处理，方便子类增加额外的数据管理
					_onRmvData_InLock(newData);
				}
			}
		}
		finally {
			_unlock();
		}
	}

	/**
	 * 清除某个US数据的处理
	 * 一般在切换分组，或者数据重新同步的时候处理
	 * @param _usId
	 */
	public void rmvUSData(int _usId)
	{
		_lock();

		try
		{
			//获取数据集对象
			_TCrossDataUSMgr<T, DT> mgr = _m_htUSDataTable.remove(_usId);
			if(null == mgr)
				return ;

			//清理数据
			ArrayList<DT> rmvList = mgr.clearData();
			//逐个调用子类处理
			for (DT rmvObj :rmvList) {
				if(null == rmvObj)
					continue;

				//调用移除数据处理，确保外围管理正常
				_onRmvData_InLock(rmvObj);
			}
		}
		finally {
			_unlock();
		}
	}

	/**
	 * 确认获取一个US的数据管理对象
	 * @param _usId
	 * @return
	 */
	private _TCrossDataUSMgr<T, DT> _ensureUSDataMgr(int _usId)
	{
		_lock();

		try
		{
			_TCrossDataUSMgr<T, DT> mgr = _m_htUSDataTable.get(_usId);
			if(null == mgr)
			{
				mgr = new _TCrossDataUSMgr<T, DT>(_usId, this);
				_m_htUSDataTable.put(_usId, mgr);
			}

			return mgr;
		}
		finally {
			_unlock();
		}
	}

	/**
	 * 从协议数据中获取对应数据Id
	 * 用于检查是否已存在对应数据
	 * @param _dataProtocol
	 * @return
	 */
	protected abstract long _getDataId(T _dataProtocol);

	/**
	 * 从字节数据读取对应的结构体
	 * @param _data
	 * @return
	 */
	protected abstract  T _readData(ByteBuffer _data);

	/**
	 * 根据数据内容创建一个数据结构体
	 * @param _dataProtocol
	 * @return
	 */
	protected abstract  DT _createNewDataInfo(T _dataProtocol);

	/**
	 * 在添加数据信息对象时调用的函数，可以在子类增加额外管理
	 * 注意处理操作在管理器的锁内
	 * @param _data
	 */
	protected abstract  void _onAddData_InLock(DT _data);

	/**
	 * 在移除数据的时候调用的子类事件函数
	 * 子类可以同步自己的数据管理
	 * @param _data
	 */
	protected abstract  void _onRmvData_InLock(DT _data);
}
