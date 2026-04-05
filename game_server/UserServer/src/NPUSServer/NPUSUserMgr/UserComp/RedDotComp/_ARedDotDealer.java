package NPUSServer.NPUSUserMgr.UserComp.RedDotComp;

import CommonEnum.ERedDotType;
import NPCommon.Util.Pair.WCGPair;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

/**
 * 红点处理器抽象基类
 *
 * 主要功能：
 * 1. 定义红点检查的扩展接口
 * 2. 提供统一的组件访问方法
 * 3. 支持初始化回调
 *
 * 扩展方式：
 * 继承此类并实现 getRedDotType() 和 checkRedDot() 方法
 *
 * 设计模式：
 * 策略模式 - 不同红点类型对应不同的检查策略
 */
public abstract class _ARedDotDealer {

	// 持有红点组件引用
	protected RedDotComponent _m_comp;

	/**
	 * 构造函数
	 *
	 * @param _comp 红点组件实例
	 */
	public _ARedDotDealer(RedDotComponent _comp) {
		_m_comp = _comp;
	}

	/**
	 * 返回当前处理器处理的红点类型
	 *
	 * @return 红点类型枚举
	 */
	public abstract ERedDotType getRedDotType();

	/**
	 * 检查当前是否应该显示红点（核心扩展方法）
	 * <p>
	 * 执行流程：
	 * 1. 获取相关组件数据
	 * 2. 执行业务逻辑判断
	 * 3. 返回是否应该显示红点
	 * @return true-应该显示红点，false-不应该显示红点
	 */
	public abstract WCGPair<Boolean, Long> checkRedDot();

	/**
	 * 获取玩家数据对象
	 *
	 * @return 玩家数据对象
	 */
	protected NPUSUserData getUserData() {
		return _m_comp.getUserData();
	}

	/**
	 * 获取UserServer对象
	 *
	 * @return UserServer对象
	 */
	protected NPUserServer getUSServer() {
		return _m_comp.getUSServer();
	}
}
