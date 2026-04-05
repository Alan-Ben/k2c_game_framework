package NPUSServer.NPUSUserMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Enum.NPServerEnum;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

/****
 * 统一管理玩家数据加载后回调
 */
public class UserSafeCall
{
    @FunctionalInterface
    public interface _IUserLoadOverHandler
    {
        void onLoadOver();
    }

    public UserSafeCall(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    private final List<_IUserLoadOverHandler> _m_lockedCalls = new ArrayList<>();//加锁回调列表
    private final List<_IUserLoadOverHandler> _m_unLockedCalls = new ArrayList<>();//不加锁回调列表
    private NPUSUserData _m_userData; //玩家数据

    public NPUSUserData getUserData()
    {
        return _m_userData;
    }

    /***
     * 不加锁回调
     * @param _handler
     */
    public void unlockedCall(final _IUserLoadOverHandler _handler)
    {
        if (getUserData().getUserDataState() != NPServerEnum.ENPUserDataState.LOADED)
        {
            synchronized (_m_unLockedCalls)
            {
                _m_unLockedCalls.add(_handler);
            }
            return;
        } else
        {
            ALSynTaskManager.getInstance().regTask(() -> _handler.onLoadOver());
        }

    }

    /****
     * 加锁进行回调
     * @param _handler
     */
    public void safeCall(final _IUserLoadOverHandler _handler)
    {

        if (getUserData().getUserDataState() != NPServerEnum.ENPUserDataState.LOADED)
        {
            synchronized (_m_lockedCalls)
            {
                _m_lockedCalls.add(_handler);
            }
            return;
        } else
        {
            ALSynTaskManager.getInstance().regTask(() ->
            {
                getUserData().lockUser();
                try
                {
                    _handler.onLoadOver();
                } finally
                {
                    getUserData().unlockUser();
                }
            });
        }

    }

    /*****
     * 用户数据加载成功后统一执行回调
     */
    public void dealPendingCalls()
    {
        List<_IUserLoadOverHandler> lockedCalls = null;
        List<_IUserLoadOverHandler> unlockedCalls = null;
        synchronized (_m_lockedCalls)
        {
            if (!_m_lockedCalls.isEmpty())
            {
                lockedCalls = new ArrayList<>(_m_lockedCalls);
                _m_lockedCalls.clear();
            }

        }
        synchronized (_m_unLockedCalls)
        {
            if (!_m_unLockedCalls.isEmpty())
            {
                unlockedCalls = new ArrayList<>(_m_unLockedCalls);
                _m_unLockedCalls.clear();
            }
        }
        if (lockedCalls != null)
        {
            __dealCall(lockedCalls, true);
        }
        if (unlockedCalls != null)
        {
            __dealCall(unlockedCalls, false);
        }
    }

    /****
     * 执行回调
     * @param _calls
     * @param _bNeedLock
     */
    private void __dealCall(List<_IUserLoadOverHandler> _calls, boolean _bNeedLock)
    {
        ALSynTaskManager.getInstance().regTask(() ->
        {
            if (_bNeedLock)
            {
                getUserData().lockUser();
            }

            try
            {
                for (_IUserLoadOverHandler _handler : _calls)
                {
                    try
                    {
                        _handler.onLoadOver();
                    } catch (Exception e)
                    {
                        USLog.error(_m_userData.getUSServer(), "player:{} deal load over handler failed", getUserData().getCid(), e);
                    }
                }
            } finally
            {
                if (_bNeedLock)
                {
                    getUserData().unlockUser();
                }
            }
        });

    }
}
