package NPUSServer.CrossDataBasicPack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;

/**
 * 跨服数据存储的数据接口类
 */
public abstract class _ATCrossDataInfo<T extends  _IALProtocolStructure> {
    //管理本数据的管理器对象
    private _ATCrossDataMgr _m_dataMgr;

    /**
     * 提供系统内部调用的设置管理器对象的接口
     * @param _dataMgr
     */
    protected void _setDataMgr(_ATCrossDataMgr _dataMgr)
    {
        _m_dataMgr = _dataMgr;
    }

    /**
     * 同步数据的消息请求
     */
    public void syncData()
    {
        if(null == _m_dataMgr)
        {
            ALServerLog.Error("sync data when data mgr is null!");
            return ;
        }

        //发送消息同步数据
        _m_dataMgr._syncData(makeCrossDataProtocolObj());
    }

    /**
     * 移除本信息对象
     */
    public void rmvData()
    {
        if(null == _m_dataMgr)
        {
            ALServerLog.Error("sync data when data mgr is null!");
            return ;
        }

        //发送消息同步数据
        _m_dataMgr.rmvData(getDataId());
    }

    /**
     * 获取对应数据Id，需要与CrossData服务器上逻辑一致
     * 这里要注意Id要保证不同服务器数据Id的唯一性，建议将服务器Id作为基数，将实例Id*10000 + 服务器Id
     * @return
     */
    public abstract long getDataId();

    /**
     * 构造传递到CrossData服务器的数据结构体
     * @return
     */
    public abstract T makeCrossDataProtocolObj();
}
