package NPUSServer.NPUSUserMgr.UserComp;

import ALBasicServer.ALBasicMutex.MutexInstance;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.util.ArrayList;


public class NPUserComponentMgr
{
    private NPUSUserData _m_udUserData;
    private ArrayList<_ANPUserComponent> _m_alUserComponentList = new ArrayList<>();
    private ArrayList<_ITickableComponent> _m_lTickableCompnents = new ArrayList<>();


    //回调锁对象
    private MutexInstance _m_mutexCallbackInited = new MutexInstance();

    public NPUserComponentMgr(NPUSUserData _userData)
    {
        _m_udUserData = _userData;
    }

    public NPUSUserData getUserData()
    {
        return _m_udUserData;
    }

    public ArrayList<_ANPUserComponent> getUserComponentList()
    {
        return _m_alUserComponentList;
    }

    /**
     * 注册组件
     * @param _component
     */
    public void regComponent(_ANPUserComponent _component)
    {
        if (null == _component)
            return;

        _m_alUserComponentList.add(_component);
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    public void dispose()
    {
        for (int i = 0; i < _m_alUserComponentList.size(); i++)
        {
            _ANPUserComponent comp = _m_alUserComponentList.get(i);
            //数据无效或已开始加载则不加载
            if (null == comp)
                continue;

            //处理释放资源操作
            comp.dispose();
        }
    }

    /************
     * 尝试初始化可初始化组件，返回是否有组件开始加载
     *
     * @author alzq.z
     * @time 2018年10月31日 下午10:22:58
     */
    public boolean checkAndInitComponent()
    {
        boolean hasCompInit = false;
        for (int i = 0; i < _m_alUserComponentList.size(); i++)
        {
            _ANPUserComponent comp = _m_alUserComponentList.get(i);
            //数据无效或已开始加载则不加载
            if (null == comp || comp.isStartInit())
                continue;

            //除了PLAYER_COMP之外的所有项都需要依赖玩家组件，玩家组件不存在则需要执行创建玩家操作
            if (comp.getCompType() != ENPPlayerCompType.PLAYER_COMP
                    && !_checkCompLoaded(ENPPlayerCompType.PLAYER_COMP))
                continue;

            //判断所有依赖项是否可加载
            if (!_checkCompListLoaded(comp.getDependCompList()))
                continue;

            //调用初始化处理
            comp.init();
            //设置有对象开始
            hasCompInit = true;
        }

        return hasCompInit;
    }

    /**
     * 组件初始化完成后的回调
     */
    public void callbackInitedComponent()
    {
        _lockCallbackInited();

        try
        {
            //尝试开始加载
            boolean hasCompInit = checkAndInitComponent();

            //如果有开始加载的处理项则不需要处理后续
            if (hasCompInit)
                return;

            //是否全完成但是在错误的状态，如全部都没开始加载，但是全部都没需要开始加载
            boolean isErrDone = false;
            for (int i = 0; i < _m_alUserComponentList.size(); i++)
            {
                _ANPUserComponent comp = _m_alUserComponentList.get(i);
                if (null == comp)
                    continue;

                //判断是否初始化完成
                if (!comp.isInited())
                {
                    //有组件开始当时未结束则不进行后续处理
                    if (comp.isStartInit())
                        return;
                    else
                        isErrDone = true;
                }
            }

            //当进行到这里说明没有组件是开始但是没结束的，如果此时isErrDone为true说明数据有错误逻辑，此时需要报错
            if (isErrDone)
            {
                StringBuilder builder = new StringBuilder();
                builder.append("Has Player Component Can not start Inited:");
                for (int i = 0; i < _m_alUserComponentList.size(); i++)
                {
                    _ANPUserComponent comp = _m_alUserComponentList.get(i);
                    if (null == comp)
                        continue;

                    //判断是否初始化完成
                    if (!comp.isInited())
                    {
                        //添加说明
                        builder.append(comp.getCompType()).append("  ");
                    }
                }

                //输出Log
                USLog.error(getUserData().getUSServer(), builder.toString());
            }
        } finally
        {
            _unlockCallbackInited();
        }

        getUserData().lockUser();
        try
        {
            //全部载入完成时的调用（要在时间轴之前执行）
            for (int i = 0; i < _m_alUserComponentList.size(); i++)
            {
                _ANPUserComponent comp = _m_alUserComponentList.get(i);
                if (null == comp)
                    continue;

                try
                {
                    comp.onInited();
                } catch (Exception e)
                {
                    USLog.error(getUserData().getUSServer(),"NPUserComponentMgr checkAndInitComponent deal fail", e);
                }
            }

            try
            {
                getUserData().getPlayerComponent().onAllCompInitedDeal();
            } catch (Exception e)
            {
                USLog.error(getUserData().getUSServer(),"NPUserComponentMgr NPPlayerComponent onAllCompInitedDeal deal fail", e);
            }
        } finally
        {
            getUserData().unlockUser();
        }

        //设置玩家加载完成
        ALSynTaskManager.getInstance().regTask(new SynTask_SetDataLoadSuc(_m_udUserData));
    }

    /*************
     * 获取对应模式的组件
     *
     * @author alzq.z
     * @time 2018年10月31日 下午10:26:42
     */
    protected _ANPUserComponent _getComp(ENPPlayerCompType _type)
    {
        for (int i = 0; i < _m_alUserComponentList.size(); i++)
        {
            _ANPUserComponent comp = _m_alUserComponentList.get(i);
            if (null == comp)
                continue;

            if (comp.getCompType() == _type)
                return comp;
        }

        return null;
    }

    /*************
     * 检测加载队列是否都可加载
     *
     * @author alzq.z
     * @time 2018年10月31日 下午10:26:42
     */
    protected boolean _checkCompLoaded(ENPPlayerCompType _type)
    {
        _ANPUserComponent comp = _getComp(_type);
        if (null == comp)
            return true;

        if (!comp.isInited())
            return false;

        return true;
    }

    protected boolean _checkCompListLoaded(ENPPlayerCompType[] _list)
    {
        if (null == _list)
            return true;

        for (int i = 0; i < _list.length; i++)
        {
            _ANPUserComponent comp = _getComp(_list[i]);
            if (null == comp)
                continue;

            if (!comp.isInited())
                return false;
        }

        return true;
    }

    private void _lockCallbackInited()
    {
        _m_mutexCallbackInited.lock();
    }

    private void _unlockCallbackInited()
    {
        _m_mutexCallbackInited.unlock();
    }


    public void registTickable(_ITickableComponent _component)
    {
        _m_lTickableCompnents.add(_component);
    }

    //每个系统进行tick处理
    public void tickComponents()
    {
        for (int i = 0; i < _m_lTickableCompnents.size(); i++)
        {
            try
            {
                _m_lTickableCompnents.get(i).tick1Sec();
            } catch (Exception e)
            {
                USLog.error(getUserData().getUSServer(), "player:{} tick component:{} failed", getUserData().getCid(), _m_lTickableCompnents.get(i).getClass().getSimpleName(), e);
            }
        }
    }


    public void delayChekNotLoadComponents()
    {
        ALSynTaskManager.getInstance().regTask(() -> _m_udUserData.getComponentMgr().dumpFailedComponents(), 5 * 1000);
    }

    private void dumpFailedComponents()
    {
        _lockCallbackInited();
        try
        {
            long nowMs = CommonFunc.getNowTimeMS();
            StringBuilder notFinish = null;
            StringBuilder notStarted = null;
            for (_ANPUserComponent comp : _m_alUserComponentList)
            {
                if (!comp.isInited())
                {
                    if (comp.isStartInit())
                    {
                        if (notFinish == null)
                            notFinish = new StringBuilder();

                        long elapsedTimeMs = nowMs - comp.getStartTimeMs();
                        notFinish.append(String.format("[%s]%dms, ", comp.getCompType(), elapsedTimeMs));
                    } else
                    {
                        if (notStarted == null)
                            notStarted = new StringBuilder();
                        notStarted.append(comp.getCompType()).append(',');
                    }
                }
            }
            if (notFinish != null)
            {
                USLog.error(getUserData().getUSServer(), "player:{} has Comp:{} started but not finished!", getUserData().getCid(), notFinish.toString());
            }
            if (notStarted != null)
            {
                USLog.error(getUserData().getUSServer(), "player:{} has Comp:{} not stared", getUserData().getCid(), notStarted.toString());
            }
        } finally
        {
            _unlockCallbackInited();
        }
    }
}
