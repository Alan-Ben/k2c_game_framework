package NPGateServer;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.CommonFunc;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 客户端断线重连会话管理器
 *
 * 负责维护每个用户的重连凭证（ReloginSession），使客户端在断线后
 * 能通过持有的 reloginKey 快速恢复会话，无需重新走完整登录流程。
 *
 * Session 生命周期：
 *
 * 注册（写入 map）：
 *   - 登录成功后：login() -> regReloginSession()，同账号新登录会覆盖旧会话
 *
 * 状态变更（不移除，只修改）：
 *   - 用户断线时：disconnect() -> onUserDisconnect()，刷新 _mAddTime
 *
 * 注销（从 map 移除）：
 *   - US 主动踢出（OPERATE/SYSTEM）：onGCUSKickout() -> removeReloginSession()
 *   - 设备顶号：onDeviceKickout() -> removeReloginSession()
 *
 * 查询（只读）：
 *   - 重连鉴权时：checkReloginSession()，验证 uid + reloginKey
 *
 * Session 无自动过期机制，依赖上述注销入口显式清理。
 */
public class NPClientReloginMgr
{
    private static NPClientReloginMgr _instance = new NPClientReloginMgr();

    public static NPClientReloginMgr getInstance()
    {
        return _instance;
    }

    public NPClientReloginMgr()
    {
        _m_mutex = new MutexAtom();
    }

    public static class ReloginSession
    {
        public String _mUid;
        public long _mSessionId;
        public String _mKey;
        public long _mAddTime;
    }

    private Map<String, ReloginSession> _mSessionMap = new ConcurrentHashMap<>();
    private MutexAtom _m_mutex;

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 注册重连会话（登录或重连成功后调用，同账号会覆盖旧会话）
     *
     * @param _uid        用户 uid
     * @param _seesionId  当前连接的 sessionId
     * @param _reloginKey 重连 key
     */
    public void regReloginSession(String _uid, long _seesionId, String _reloginKey)
    {
        _lock();
        try
        {
            ReloginSession session = new ReloginSession();
            session._mUid = _uid;
            session._mSessionId = _seesionId;
            session._mKey = _reloginKey;
            session._mAddTime = CommonFunc.getNowTimeMS();

            _mSessionMap.put(_uid, session);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重连鉴权：验证 uid + reloginKey（只读，不移除 session）
     *
     * @param _uid        用户 uid
     * @param _reloginKey 客户端持有的重连 key
     * @return 验证通过返回 session，否则返回 null
     */
    public ReloginSession checkReloginSession(String _uid, String _reloginKey)
    {
        _lock();
        try
        {
            ReloginSession session = _mSessionMap.get(_uid);
            if (null == session)
                return null;

            if (_reloginKey.compareTo(session._mKey) != 0)
                return null;

            return session;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 按 sessionId 匹配后移除重连会话（踢出/顶号场景使用）
     * sessionId 不一致时说明该账号已有新连接接管，不移除并返回 null
     *
     * @param _uid       用户 uid
     * @param _sessionId 发起移除的连接的 sessionId
     * @return 成功移除返回 session，sessionId 不匹配或不存在返回 null
     */
    public ReloginSession removeReloginSession(String _uid, long _sessionId)
    {
        _lock();
        try
        {
            ReloginSession session = _mSessionMap.remove(_uid);
            if (null != session && session._mSessionId != _sessionId)
            {
                _mSessionMap.put(_uid, session);
                return null;
            }
            return session;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 用户断线时刷新离线起始时间（不移除 session，保留重连能力）
     * key 不一致时忽略，说明该账号已在本服务器建立了新连接
     *
     * @param _uid 用户 uid
     * @param _key 当前连接持有的 reloginKey
     */
    public void onUserDisconnect(String _uid, String _key)
    {
        _lock();
        try
        {
            ReloginSession keyObj = _mSessionMap.get(_uid);
            if (null == keyObj)
                return;

            // 如果 key 不同则代表本用户还在本服务器承载中
            if (keyObj._mKey.compareTo(_key) != 0)
                return;

            // 刷新离线起始时间
            keyObj._mAddTime = CommonFunc.getNowTimeMS();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清空所有重连会话（运维应急使用，用于处理 session 内存驻留异常）
     */
    public void clear()
    {
        _lock();
        try
        {
            _mSessionMap.clear();
        } finally
        {
            _unlock();
        }
    }
}
