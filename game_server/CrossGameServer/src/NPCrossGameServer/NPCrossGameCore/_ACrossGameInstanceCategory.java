package NPCrossGameServer.NPCrossGameCore;

import ALBasicServer.ALBasicMutex.MutexInstance;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_002_RetCreateCrossGameInstance;
import NPCommon.ErrMain.CGSErr;
import NPCommon.Log.CommLog;
import NPCrossGameServer.NPCrossGameCore.GCMsgDispather.CrossGameGCMsgDispatcher;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter.CrossGameInstanceCommiter;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.CrossGameCategoryMsgMgr;
import NPCrossGameServer.NPCrossGameCore.RequestDispather.CrossGameRequestDispatcher;
import NPCrossGameServer.SynTask_CalCrossGameWeight;
import NPEnum.ENPCrossGameCategoryEnum;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._ATWCGBasicRequestSubOrderDealer_CustomCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;


/**
 * 跨服游戏分类下的具体游戏实例管理
 * @param <T>
 * @author mj
 */
public abstract class _ACrossGameInstanceCategory<T extends _ACrossGameInstance>
{
    //游戏实例映射表
    private HashMap<Long, T> _m_hmGameInstanceMap;
    //游戏实例列表
    protected ArrayList<T> _m_alGameInstanceList;
    //消息处理对象
    private CrossGameCategoryMsgMgr _m_mmMsgMgr;
    //初始化完成标志
    private boolean _m_bInited;
    //锁对象
    private MutexInstance _m_mutex;

    public _ACrossGameInstanceCategory()
    {
        _m_hmGameInstanceMap = new HashMap<>();
        _m_alGameInstanceList = new ArrayList<>();
        _m_mmMsgMgr = new CrossGameCategoryMsgMgr(this);

        _m_mutex = new MutexInstance();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public CrossGameCategoryMsgMgr getMsgMgr()
    {
        return _m_mmMsgMgr;
    }

    public boolean isInited()
    {
        return _m_bInited;
    }

    protected void setInited()
    {
        _m_bInited = true;
    }

    /**
     * 初始化子项
     * @return
     */
    protected boolean initSub()
    {
        for (int i = 0; i < _m_alGameInstanceList.size(); i++)
        {
            T instance = _m_alGameInstanceList.get(i);
            if (null == instance)
                continue;

            if (!instance.initSub())
                return false;
        }

        return true;
    }

    /**
     * 初始化注册协议处理对象
     */
    @SuppressWarnings("rawtypes")
    protected void initDealer()
    {
        //GCMsg处理对象列表
        ArrayList<_ATWCGBasicRequestSubOrderDealer_CustomCommiter> tmpGCMsgDealerList = new ArrayList<>();
        //获取GCMsg协议处理对象列表准备注册
        initGCMsgSubDealer(tmpGCMsgDealerList);
        //注册协议对象
        for (int i = 0; i < tmpGCMsgDealerList.size(); i++)
        {
            _ATWCGBasicRequestSubOrderDealer_CustomCommiter dealer = tmpGCMsgDealerList.get(i);
            if (null == dealer)
                continue;

            CrossGameGCMsgDispatcher.getInstance().regSubDealer(dealer);
        }

        //Request处理对象列表
        ArrayList<_ATWCGBasicRequestSubOrderDealer_CustomCommiter> tmpRequestDealerList = new ArrayList<>();
        //获取Request协议处理对象列表准备注册
        initRequestSubDealer(tmpRequestDealerList);
        //注册协议对象
        for (int i = 0; i < tmpRequestDealerList.size(); i++)
        {
            _ATWCGBasicRequestSubOrderDealer_CustomCommiter dealer = tmpRequestDealerList.get(i);
            if (null == dealer)
                continue;

            CrossGameRequestDispatcher.getInstance().regSubDealer(dealer);
        }
    }

    /**
     * 初始化完成调用
     */
    protected void initDone()
    {
        for (int i = 0; i < _m_alGameInstanceList.size(); i++)
        {
            T instance = _m_alGameInstanceList.get(i);
            if (null == instance)
                continue;

            instance.onInitDone();
        }
    }

    /***************
     * 发送到空间的消息
     * 这个使用Space对象作为处理对象即可
     * @param _msgBuffer
     */
    public void dealMsg(long _instanceId, long _cid, ByteBuffer _msgBuffer)
    {
        T instance = getGameInstance(_instanceId);
        //获取对应的的category对象
        if (null == instance)
        {
            return;
        }
    }

    /***************
     * 发送到空间的处理行为
     * 一般是系统发送的处理行为
     * 需要通知空间进行事件触发或者阶段调整
     *
     * 可以使用_TWCGBasicRequestDispather_CustomCommiter<NPSpaceCommiter>作为Dispather使用
     * @param _msgBuffer
     */
    public void dealRequest(_IWCGBasicRequestCommiter _commiter, long _instanceId, long _cid, ByteBuffer _msgBuffer)
    {
        T instance = getGameInstance(_instanceId);
        //获取对应的的category对象
        if (null == instance)
        {
            _commiter.commitFailRes(CGSErr.NO_CROSSGAME_INSTANCE_ERROR.getCode());
            return;
        }

        //创建对应游戏实例的Instance的Commiter对象
        CrossGameRequestDispatcher.getInstance().DealProtocol(new CrossGameInstanceCommiter(instance, _cid, _commiter), _msgBuffer);
    }

    /**
     * 注册协议处理对象，用于后续GM命令
     * @param _dealer
     */
    @SuppressWarnings("rawtypes")
    public void regGCMsgSubDealer(_ATWCGBasicRequestSubOrderDealer_CustomCommiter _dealer)
    {
        CrossGameGCMsgDispatcher.getInstance().regSubDealer(_dealer);
    }

    /**
     * 注册协议处理对象，用于后续GM命令
     * @param _dealer
     */
    @SuppressWarnings("rawtypes")
    public void regRequestSubDealer(_ATWCGBasicRequestSubOrderDealer_CustomCommiter _dealer)
    {
        CrossGameRequestDispatcher.getInstance().regSubDealer(_dealer);
    }

    /**
     * tick操作处理消息
     * @return
     */
    public boolean tick()
    {
        //优先处理消息
        boolean needDealMsg = true;
        int dealMsgCount = 0;

        do
        {
            //处理空间消息
            _lock();

            //处理Tick操作
            try
            {
                needDealMsg = _m_mmMsgMgr.dealMessage();
                dealMsgCount++;
            } catch (Exception _e)
            {
                _e.printStackTrace();
            } catch (Throwable _th)
            {
                _th.printStackTrace();
            } finally
            {
                _unlock();
            }

            //处理消息超过1000预警
            if (dealMsgCount > 1000 && dealMsgCount % 1000 == 0)
            {
                ALServerLog.Error("Cross-Game: " + getCategory() + " deal msg count: " + dealMsgCount);
            }

            //处理消息超过10000个可能死循环，此时直接终止
            if (dealMsgCount > 10000)
            {
                ALServerLog.Error("Cross-Game: " + getCategory() + " break deal msg count: " + dealMsgCount);

                needDealMsg = false;
            }

        } while (needDealMsg);

        //对所有instance进行tick
        _lock();

        try
        {
            for (int i = 0; i < _m_alGameInstanceList.size(); i++)
            {
                T instance = _m_alGameInstanceList.get(i);
                if (null == instance)
                    continue;

                instance.tick();
            }
        } finally
        {
            _unlock();
        }

        return true;
    }

    /**
     * 查找对应的游戏
     * @param _instanceId
     * @return
     */
    public T getGameInstance(long _instanceId)
    {
        _lock();

        try
        {
            return _m_hmGameInstanceMap.get(_instanceId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册游戏实例
     * @param _instance
     * @return
     */
    public boolean regGameInstance(T _instance)
    {
        _lock();

        try
        {
            //不允许重复注册
            if (_m_hmGameInstanceMap.containsKey(_instance.getInstanceId()))
            {
                CommLog.error("NPPartyCategory.regGameInstance containsKey error, instance:{}", _instance.getInstanceId());
                return false;
            }

            //注册游戏实例，key=instanceId
            _m_hmGameInstanceMap.put(_instance.getInstanceId(), _instance);
            _m_alGameInstanceList.add(_instance);

            //实例对象初始化
            _instance.initComm(getCategory());

            //同步权重到CS
            ALSynTaskManager.getInstance().regTask(SynTask_CalCrossGameWeight.getInstance());

            //输出日志
            //CommLog.info("Cross-Game regGameInstance, {}-{}", getCategory(), _instance.getInstanceId());

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销游戏实例
     * @param _instanceId
     */
    public void unregGameInstance(long _instanceId)
    {
        _lock();

        try
        {
            //移除实例
            T removeInstance = _m_hmGameInstanceMap.remove(_instanceId);
            //需要检查当前是否是同一个对象，如果不是，需要重新注册
            if (null == removeInstance)
            {
                return;
            }

            //销毁队列里的对象
            _m_alGameInstanceList.remove(removeInstance);

            //执行销毁
            removeInstance.discard();

            //同步权重到CS
            ALSynTaskManager.getInstance().regTask(SynTask_CalCrossGameWeight.getInstance());

            //输出日志
            CommLog.info("Cross-Game unregGameInstance, {}-{}", getCategory(), _instanceId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 计算权重
     * @return
     */
    public int calWeight()
    {
        _lock();

        try
        {
            int weight = 0;

            for (int i = 0; i < _m_alGameInstanceList.size(); i++)
            {
                T instance = _m_alGameInstanceList.get(i);
                if (null == instance)
                    continue;

                weight += instance.getInitWeight() + instance.calExtraHandleWeight();
            }

            return weight;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        _lock();
        try
        {
            for (T t : _m_alGameInstanceList)
            {
                sb.append("=============").append("\n");
                sb.append(t.toString());
            }
            return sb.toString();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 实例游戏的类别
     * @return
     */
    public abstract ENPCrossGameCategoryEnum getCategory();

    /**
     * 从数据库加载数据
     * @return
     */
    protected abstract boolean initFromDB();

    /**
     * 初始化协议处理对象
     * @param _dealerList
     */
    @SuppressWarnings("rawtypes")
    protected abstract void initGCMsgSubDealer(ArrayList<_ATWCGBasicRequestSubOrderDealer_CustomCommiter> _dealerList);


    /**
     * 初始化协议处理对象
     * @param _dealerList
     */
    @SuppressWarnings("rawtypes")
    protected abstract void initRequestSubDealer(ArrayList<_ATWCGBasicRequestSubOrderDealer_CustomCommiter> _dealerList);

    /**
     * 创建实例对象
     * @param _instanceId
     * @param _extData
     * @param _ret
     * @return
     */
    public abstract boolean createNewInstance(long _instanceId, byte[] _extData, NP2CGS_RB_001_002_RetCreateCrossGameInstance _ret);
}
