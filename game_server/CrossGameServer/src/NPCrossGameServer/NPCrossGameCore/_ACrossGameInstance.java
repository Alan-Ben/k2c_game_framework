package NPCrossGameServer.NPCrossGameCore;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCrossGameServer.NPCrossGameServer;
import NPEnum.ENPCrossGameCategoryEnum;
import NPServerProtocolWriter.NP2US.Msg.NP2US_Writer_001_BasicOp;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;

/**
 * 跨服游戏实例对象
 * @author mark
 */
public abstract class _ACrossGameInstance implements _IALProtocolReceiver
{
    //上次tick逻辑处理的时间戳
    private long _m_lLastLogicTickTimeMS;
    //逻辑处理的tick周期毫秒数
    private int _m_iLogicTickDurationMs;
    //已经移除标志位
    private boolean _m_bIsDeled;
    //跨服实例类型
    private ENPCrossGameCategoryEnum _m_eCategory = null;

    public _ACrossGameInstance()
    {
        //初始化tick周期500毫秒，子类可以调用setLogicTickMs修改逻辑处理周期时长，但是不能小于100毫秒
        _m_iLogicTickDurationMs = 500;
    }

    public boolean isDeled()
    {
        return _m_bIsDeled;
    }

    /**
     * 默认每个实例的初始权重
     * @return
     */
    public int getInitWeight()
    {
        return 10;
    }

    /**
     * 初始化完成处理
     */
    protected void initComm(ENPCrossGameCategoryEnum _category)
    {
        _m_eCategory = _category;
    }

    /**
     * 设置tick的周期毫秒数，不能小于100毫秒
     * @param _tickMs
     */
    public void setLogicTickMs(int _logicTickMs)
    {
        if (_logicTickMs < 100)
            _logicTickMs = 100;

        _m_iLogicTickDurationMs = _logicTickMs;
    }

    /**
     * tick操作
     * @return
     */
    public boolean tick()
    {
        //当前游戏实例已移除
        if (isDeled())
        {
            return false;
        }

        //当前游戏已失效
        if (!isEnable())
        {
            //失效时调用
            onEnableFalse();

            return false;
        }

        //处理游戏逻辑
        try
        {
            //如果小于处理逻辑周期最小时长，则不予处理，等待下一次tick
            if (CommonFunc.getNowTimeMS() - _m_lLastLogicTickTimeMS > _m_iLogicTickDurationMs)
            {
                //设置上次tick逻辑处理时间戳
                _m_lLastLogicTickTimeMS = CommonFunc.getNowTimeMS();

                //执行子类tick
                onTick();
            }
        } catch (Exception ex)
        {
            CommLog.error("Cross-Game Instance:tick Logic Exception, {}-{}", _m_eCategory, getInstanceId());
            CommLog.error("", ex);

            return false;
        } catch (Throwable th)
        {
            CommLog.error("Cross-Game Instance:tick Logic Throwable, {}-{}", _m_eCategory, getInstanceId());
            CommLog.error("", th);

            return false;
        }

        return true;
    }

    /**
     * 销毁处理
     */
    protected void discard()
    {
        //已经移除
        if (_m_bIsDeled)
            return;
        _m_bIsDeled = true;

        //销毁监听处理
        onDiscard();

        //输出日志
        CommLog.info("Cross-Game Instance:discard, {}-{}", _m_eCategory, getInstanceId());
    }

    /**
     * 对指定玩家发送消息
     * @param _cid
     * @param _proto
     */
    public void sendMsgToPlayer(long _cid, _IALProtocolStructure _proto)
    {
        int usTypeId = CommonFunc.parseServerTypeIdFromCid(_cid);

        NPCrossGameServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), usTypeId
                , NP2US_Writer_001_BasicOp.make_002_SendbackUserMsg(_cid, _proto));
    }

    /**
     * 对指定玩家列表发送消息
     * @param _cidList
     * @param _proto
     */
    public void sendMsgToPlayerList(ArrayList<Long> _cidList, _IALProtocolStructure _proto)
    {
        for (Long cid : _cidList)
        {
            sendMsgToPlayer(cid, _proto);
        }
    }

    //////////////////////////// 抽象方法 ////////////////////////////

    /**
     * 实例ID，用于获取到指定的实例对象
     * @return
     */
    public abstract long getInstanceId();

    /**
     * 各实例各自的初始化
     * @return
     */
    protected abstract boolean initSub();

    /**
     * 初始化失败监听
     */
    protected abstract void onInitFail();

    /**
     * 初始化完成监听
     */
    protected abstract void onInitDone();

    /**
     * 销毁监听
     */
    protected abstract void onDiscard();

    /**
     * tick时执行的处理
     */
    protected abstract void onTick();

    /**
     * 检测是否有效
     * @return
     */
    public abstract boolean isEnable();

    /**
     * 失效时调用
     */
    protected abstract void onEnableFalse();

    /**
     * 计算额外权重，每个实例默认值10，如果觉得有数量偏大，再额外增加
     * 暂无量化标准，以经验为主，后续根据运行情况进行调整
     * 备注：当前配置默认最高权重是50000，即一台US支持5000个实例对象
     * @return
     */
    public abstract int calExtraHandleWeight();
}
