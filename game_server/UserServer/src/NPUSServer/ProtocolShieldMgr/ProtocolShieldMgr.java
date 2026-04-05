package NPUSServer.ProtocolShieldMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPCommon.DB.BM.BM;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsProtocolShieldBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

/**
 * 协议屏蔽管理器 - 全服级别
 *
 * 主要功能：
 * 1. 管理全服屏蔽的协议号列表（主协议号+副协议号）
 * 2. 支持通过GM命令动态添加/移除屏蔽协议
 * 3. 数据持久化到数据库，服务器重启后保留屏蔽配置
 * 4. 提供快速查询接口用于协议分发前的拦截检查
 *
 * 线程安全：所有操作使用互斥锁保护
 */
public class ProtocolShieldMgr {
	// 对应的US服务器实例对象
	private NPUserServer _m_usUserServer;

	// 屏蔽的协议号集合，使用 (主协议号 << 16) | 副协议号 作为唯一标识
	private HashSet<Long> _m_shieldProtocols;

	// 线程安全锁对象
	private MutexObject _m_mutex;

	/**
	 * 构造函数
	 *
	 * @param _userServer 用户服务器实例
	 */
	public ProtocolShieldMgr(NPUserServer _userServer) {
		_m_usUserServer = _userServer;
		_m_shieldProtocols = new HashSet<>();
		_m_mutex = new MutexObject();
	}

	public NPUserServer getUserServer() {
		return _m_usUserServer;
	}

	public BM getBM() {
		return getUserServer().getBM();
	}

	private void _lock() {
		_m_mutex.lock();
	}

	private void _unlock() {
		_m_mutex.unlock();
	}

	/**
	 * 将主协议号和副协议号编码为唯一的long值
	 *
	 * @param _mainProtocol 主协议号
	 * @param _subProtocol 副协议号
	 * @return 编码后的long值
	 */
	private long encodeProtocol(int _mainProtocol, int _subProtocol) {
		return ((long)_mainProtocol << 16) | (_subProtocol & 0xFFFF);
	}

	/**
	 * 从数据库加载所有屏蔽协议配置
	 *
	 * 服务器启动时调用，初始化屏蔽列表
	 *
	 * @return 是否加载成功
	 */
	public boolean initFromDB() {
		List<UsProtocolShieldBO> boList = getBM().getBM(UsProtocolShieldBO.class).s_findAll();
		if (null == boList) {
			USLog.error(getUserServer(), "Protocol Shield BO init fail.");
			return false;
		}

		_lock();
		try {
			_m_shieldProtocols.clear();
			for (UsProtocolShieldBO bo : boList) {
				if (null == bo)
					continue;

				long protocolKey = encodeProtocol(bo.getMainProtocol(), bo.getSubProtocol());
				_m_shieldProtocols.add(protocolKey);
			}

			USLog.sys(getUserServer(), "Protocol Shield init success, total count: {}", _m_shieldProtocols.size());
			return true;
		} finally {
			_unlock();
		}
	}

	/**
	 * 添加屏蔽协议
	 *
	 * 执行流程：
	 * 1. 检查协议是否已存在
	 * 2. 添加到内存集合
	 * 3. 插入数据库持久化
	 *
	 * @param _mainProtocol 主协议号
	 * @param _subProtocol 副协议号
	 * @return 是否添加成功（已存在返回false）
	 *
	 * 线程安全：使用互斥锁保护
	 */
	public boolean addShieldProtocol(int _mainProtocol, int _subProtocol) {
		long protocolKey = encodeProtocol(_mainProtocol, _subProtocol);

		_lock();
		try {
			// 检查是否已存在
			if (_m_shieldProtocols.contains(protocolKey)) {
				return false;
			}

			// 添加到内存
			_m_shieldProtocols.add(protocolKey);

			// 持久化到数据库
			UsProtocolShieldBO bo = new UsProtocolShieldBO();
			bo.setMainProtocol(getBM(), _mainProtocol);
			bo.setSubProtocol(getBM(), _subProtocol);
			bo.setCreateTimeMs(getBM(), System.currentTimeMillis());
			bo.insert(getBM());

			USLog.sys(getUserServer(), "Add shield protocol success: main={}, sub={}", _mainProtocol, _subProtocol);
			return true;
		} finally {
			_unlock();
		}
	}

	/**
	 * 移除屏蔽协议
	 *
	 * 执行流程：
	 * 1. 从内存集合中移除
	 * 2. 从数据库中删除
	 *
	 * @param _mainProtocol 主协议号
	 * @param _subProtocol 副协议号
	 * @return 是否移除成功（不存在返回false）
	 *
	 * 线程安全：使用互斥锁保护
	 */
	public boolean removeShieldProtocol(int _mainProtocol, int _subProtocol) {
		long protocolKey = encodeProtocol(_mainProtocol, _subProtocol);

		_lock();
		try {
			// 检查是否存在
			if (!_m_shieldProtocols.contains(protocolKey)) {
				return false;
			}

			// 从内存移除
			_m_shieldProtocols.remove(protocolKey);

			HashMap<String, Object> whereMap = new HashMap<>();
			whereMap.put("main_protocol", String.valueOf(_mainProtocol));
			whereMap.put("sub_protocol", String.valueOf(_subProtocol));

			getBM().getBM(UsProtocolShieldBO.class).delAll(whereMap);

			USLog.sys(getUserServer(), "Remove shield protocol success: main={}, sub={}", _mainProtocol, _subProtocol);
			return true;
		} finally {
			_unlock();
		}
	}

	/**
	 * 检查指定协议是否被屏蔽
	 *
	 * 高频调用方法，用于协议分发前的快速检查
	 *
	 * @param _mainProtocol 主协议号
	 * @param _subProtocol 副协议号
	 * @return true表示被屏蔽，false表示未屏蔽
	 *
	 * 线程安全：使用互斥锁保护
	 */
	public boolean isProtocolShielded(int _mainProtocol, int _subProtocol) {
		long protocolKey = encodeProtocol(_mainProtocol, _subProtocol);

		_lock();
		try {
			return _m_shieldProtocols.contains(protocolKey);
		} finally {
			_unlock();
		}
	}

	/**
	 * 获取所有屏蔽的协议列表
	 *
	 * 用于GM命令查询当前所有屏蔽的协议
	 *
	 * @return 协议列表，格式为 "主协议号-副协议号"
	 */
	public List<String> getAllShieldProtocols() {
		_lock();
		try {
			List<String> result = new ArrayList<>();
			for (Long protocolKey : _m_shieldProtocols) {
				int mainProtocol = (int)(protocolKey >> 16);
				int subProtocol = (int)(protocolKey & 0xFFFF);
				result.add(String.format("%d-%d", mainProtocol, subProtocol));
			}
			return result;
		} finally {
			_unlock();
		}
	}

	/**
	 * 清空所有屏蔽协议
	 *
	 * 清空内存和数据库中的所有屏蔽配置
	 *
	 * 线程安全：使用互斥锁保护
	 */
	public void clearAll() {
		_lock();
		try {
			_m_shieldProtocols.clear();
			getBM().getBM(UsProtocolShieldBO.class).delAll();
			USLog.sys(getUserServer(), "Clear all shield protocols");
		} finally {
			_unlock();
		}
	}
}
