package NPUSServer.CrossDataBasicPack;

import ALBasicCommon.ALSerializeMaker;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import GOM2CD_RB.gom_p001_DataOp.GOM2CD_RB_001_001_RetInitData;
import NPCommon.ErrMain.CommErr;
import NPUSServer.CrossDataBasicPack.Writer.GOM2CD_R_Writer_001_DataOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;
import java.util.Hashtable;

/**
 * 在US上，对数据对象进行管理的管理器
 */
public abstract class _ATCrossDataMgr<T extends _IALProtocolStructure, DT extends _ATCrossDataInfo<T>> implements _ICrossDataMgrInterface
{
    private NPUserServer _m_sServer;
    private int _m_iDataType;
    private long _m_lGroupId;
    //处理的操作序列号，如果变更了GroupId避免重复同步
    private long _m_lSyncSerialize;

    //是否已经同步了数据，标志位未成功的时候不会发送同步数据变更
    //有此标志位也可以避免在服务器开启顺序不同的时候，大量重复发送初始化数据
    private boolean _m_bIsSynced;

    //存储数据的数据集结构体
    private Hashtable<Long, DT> _m_htDataTable;

    private MutexAtom _m_mutex;

    public _ATCrossDataMgr(NPUserServer _usServer, int _dataType)
    {
        _m_sServer = _usServer;
        _m_iDataType = _dataType;
        _m_lGroupId = 0;
        _m_lSyncSerialize = ALSerializeMaker.makeNewSerialize();

        _m_bIsSynced = false;

        _m_htDataTable = new Hashtable<Long, DT>();

        _m_mutex = new MutexAtom();
    }

    public NPUserServer getUSServer() {return _m_sServer;}
    public int getDataType() {return _m_iDataType;}
    public long getGroupId() {return _m_lGroupId;}

    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /**
     * 在跨服数据管理服务器上线的时候触发的广播处理函数，一般需要各管理器进行信息同步
     */
    public void onCrossDataServerOnline()
    {
        //此时直接同步数据
        //如果groupId无效不做处理
        if(_m_lGroupId == 0) {
            return;
        }

        //直接使用当前变量重置，加锁避免多线程处理错误
        _lock();

        try
        {
            //如果此时还未触发数据同步，则不做处理
            //如未完成初始化操作，由初始化本身行为保证必须执行完成。此事件处理是在已经处理完成的基础上，保证如果服务器异常后的数据同步
            if(!_m_bIsSynced)
                return ;

            //直接调用实际处理操作
            _dealInitSyncData(_m_lGroupId);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 开始同步数据，根据数据类型和数据集合，通过US进行同步
     * @param _groupId  数据集合Id
     */
    public void initSyncData(long _groupId)
    {
        //如果服务器对象为空报错
        if (null == _m_sServer) {
            ALServerLog.Error("try init sync data when server is null!");
            return;
        }

        //注册管理器
        _m_sServer.getCrossDataCore()._addCrossDataMgr(this);

        //进行具体处理
        _dealInitSyncData(_groupId);
    }

    /**
     * 修改同步数据集Id
     * 此行为不会进行管理器注册，避免重复注册，区分初始化和日常行为
     * @param _groupId
     */
    public void chgGroupId(long _groupId)
    {
        //如果服务器对象为空报错
        if (null == _m_sServer) {
            ALServerLog.Error("try init sync data when server is null!");
            return;
        }

        //进行具体处理
        _dealInitSyncData(_groupId);
    }

    /**
     * 实际处理数据同步初始化的操作
     * @param _groupId
     */
    private void _dealInitSyncData(long _groupId)
    {
        //如果服务器对象为空报错
        if (null == _m_sServer) {
            ALServerLog.Error("try init sync data when server is null!");
            return;
        }

        ALServerLog.Sys("start sync dataType:" + _m_iDataType + " for groupId:" + _groupId + " dataCount:" + _m_htDataTable.size());

        //向服务器发送数据同步请求，在回调中将剩余数据继续同步完成
        //同步期间，相关添加和删除的数据需要记录在临时列表中，确保后续同步的完整性
        _lock();

        try {
            //如果所属集合Id变动，需要向原集合删除数据
            if(_groupId != _m_lGroupId && _m_lGroupId != 0)
            {
                //向原集合删除数据
                _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                        , GOM2CD_R_Writer_001_DataOp.make_005_ClearUSFromGroup(getDataType(), getGroupId(), _m_sServer.getServerTypeId()));
            }

            _m_lGroupId = _groupId;
            //刷新序列号，避免无效消息发送
            _m_lSyncSerialize = ALSerializeMaker.makeNewSerialize();

            //如果Group无效不做同步处理
            if(_m_lGroupId == 0) {
                //重置状态变量，确保其他操作也不会同步
                _m_bIsSynced = false;
                return;
            }

            //存储临时变量
            long tmpSerialize = _m_lSyncSerialize;

            //发送请求向数据服务器同步
            //先发送开始同步处理
            _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                    , GOM2CD_R_Writer_001_DataOp.make_001_InitData(getDataType(), getGroupId(), _m_sServer.getServerTypeId())
                    , new _IWCGCallbackDealer() {
                        @Override
                        public _IALProtocolStructure createProtocolObj() {
                            return new GOM2CD_RB_001_001_RetInitData();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure ialProtocolStructure) {
                            ALServerLog.Sys("DataType:" + _m_iDataType + " init sync cross data server start! serialize:" + tmpSerialize);

                            //序列号不一致不做处理，表示原操作已经失效
                            if(tmpSerialize != _m_lSyncSerialize)
                            {
                                return ;
                            }

                            //开始处理同步所有数据
                            __dealSyncAllData();
                        }

                        @Override
                        public void dealFail(int _err) {
                            ALServerLog.Sys("DataType:" + _m_iDataType + " init sync cross data server err! - " + _err);

                            //序列号不一致不做处理，表示原操作已经失效
                            if(tmpSerialize != _m_lSyncSerialize)
                            {
                                return ;
                            }

                            //此时延迟5秒尝试同步，此处需要确保初始化正常进行
                            ALServerLog.Sys("delay 5000ms retry sync dataType:" + _m_iDataType);
                            ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                                @Override
                                public void run() {
                                    //再次处理初始化同步
                                    _dealInitSyncData(_m_lGroupId);
                                }
                            }, 5000);
                        }
                    });
        }
        finally {
            _unlock();
        }
    }
    /**
     * 处理同步所有数据的操作，一般在初始化数据的时候才会调用
     */
    protected void __dealSyncAllData()
    {
        int dataCountPerPage = _getInitSyncDataPerPageCount();
        ALServerLog.Sys("start deal sync all data for dataType:" + _m_iDataType + " for groupId:" + _m_lGroupId + " dataCount:" + _m_htDataTable.size());

        _lock();

        try {
            //成功则设置标记位并开始进行内容同步，只有做了这个处理，后续的数据才会发送同步
            _m_bIsSynced = true;

            //统计单次发送数量
            int sendCount = 0;
            //存储发送数据的队列
            ArrayList<_IALProtocolStructure> tmpList = new ArrayList<_IALProtocolStructure>();
            //开始发送数据
            for(DT dataInfo : _m_htDataTable.values())
            {
                if(null == dataInfo)
                    continue;

                tmpList.add(dataInfo.makeCrossDataProtocolObj());
                sendCount++;

                //当达到分页数量，则发送
                if(sendCount % dataCountPerPage == 0)
                {
                    //发送数据
                    _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                            , GOM2CD_R_Writer_001_DataOp.make_002_SyncData(getDataType(), getGroupId(), _m_sServer.getServerTypeId(), tmpList));

                    //重置统计和数据
                    sendCount = 0;
                    tmpList.clear();
                }
            }

            //如遍历后还有数据，统一发送
            //发送数据
            _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                    , GOM2CD_R_Writer_001_DataOp.make_002_SyncData(getDataType(), getGroupId(), _m_sServer.getServerTypeId(), tmpList));

            //重置统计和数据
            sendCount = 0;
            tmpList.clear();
        }
        finally {
            _unlock();
        }
    }

    /**
     * 初始化加入数据的处理
     * 此处理不会发送同步消息，在初始化务必调用很函数
     * @param _data
     */
    protected void _initAddData(DT _data)
    {
        if(null == _data)
            return ;

        _lock();

        try
        {
            //重复Id报错，不替换
            if(_m_htDataTable.contains(_data.getDataId()))
            {
                ALServerLog.Error("Multi init Data Id for Data:" + _m_iDataType + " dataId:" + _data.getDataId());
                return ;
            }

            _m_htDataTable.put(_data.getDataId(), _data);
            //设置管理器
            _data._setDataMgr(this);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 根据Id查询对应数据对象
     * @param _dataId
     * @return
     */
    public DT lookupData(long _dataId)
    {
        _lock();

        try
        {
            return _m_htDataTable.get(_dataId);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 初加入数据的处理
     * 此处理会发送同步消息
     * 如果是初始化添加数据，不要使用本函数，请务必使用_initAddData
     * @param _data
     */
    public void addData(DT _data)
    {
        if(null == _data)
            return ;

        _lock();

        try
        {
            //重复Id报错，不替换
            if(_m_htDataTable.contains(_data.getDataId()))
            {
                ALServerLog.Error("Multi Data Id for Data:" + _m_iDataType + " dataId:" + _data.getDataId());
                return ;
            }

            _m_htDataTable.put(_data.getDataId(), _data);
            //设置管理器
            _data._setDataMgr(this);

            //发送消息同步数据
            _syncData(_data.makeCrossDataProtocolObj());

            //调用添加事件函数
            _onAddData_InLock(_data);
        }
        finally {
            _unlock();
        }
    }

    /***********
     * 移除数据对象，将向数据管理器发送消息同步移除
     * @param _dataId
     */
    public DT rmvData(long _dataId)
    {
        _lock();

        try
        {
            DT dataInfo = _m_htDataTable.remove(_dataId);
            if(null == dataInfo)
                return null;

            //调用事件函数
            _onRmvData_InLock(dataInfo);

            //设置管理器
            dataInfo._setDataMgr(null);

            //发送消息同步数据，只有在数据同步后才会发送
            if(_m_bIsSynced)
                _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                        , GOM2CD_R_Writer_001_DataOp.make_003_RmvData(getDataType(), getGroupId(), _m_sServer.getServerTypeId(), _dataId));

            return dataInfo;
        }
        finally {
            _unlock();
        }
    }
    public void rmvData(ArrayList<Long> _dataIdList)
    {
        _lock();

        try
        {
            //逐个移除，这里因为是队列就不做有效性判断，全部发送就是了
            for(Long dataId : _dataIdList) {
                DT dataInfo = _m_htDataTable.remove(dataId);
                if (null == dataInfo)
                    return;

                //调用事件函数
                _onRmvData_InLock(dataInfo);

                //设置管理器
                dataInfo._setDataMgr(null);
            }

            //发送消息同步数据，只有在数据同步后才会发送
            if(_m_bIsSynced)
                _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                        , GOM2CD_R_Writer_001_DataOp.make_003_RmvData(getDataType(), getGroupId(), _m_sServer.getServerTypeId(), _dataIdList));
        }
        finally {
            _unlock();
        }
    }

    /**
     * 向CrossData数据发送自定义处理的内容
     * CD服务器回报时会直接返回这里
     * @param _customOpData
     * @param _callback
     */
    public void sendCustomOpToCrossData(_IALProtocolStructure _customOpData, _IWCGCallbackDealer _callback)
    {
        //发送消息同步数据，只有在数据同步后才会发送
        if(_m_bIsSynced)
        {
            ALServerLog.Error("send CustomOp when data is still not synced!");
            //直接处理回调
            if(null != _callback)
                _callback.dealFail(CommErr.SYS_ERR.getCode());
            return ;
        }

        //发送自定义数据请求
        _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                    , GOM2CD_R_Writer_001_DataOp.make_010_CustomDataOp(getDataType(), getGroupId(), _customOpData), _callback);
    }



    /**
     * 供内部调用的数据同步处理函数
     * 将向数据管理器发送数据，同步最新数据内容
     * @param _data
     */
    protected void _syncData(_IALProtocolStructure _data)
    {
        //如果服务器对象为空报错
        if (null == _m_sServer) {
            ALServerLog.Error("try sync data when server is null!");
            return;
        }

        //发送消息同步数据，只有在数据同步后才会发送
        if(_m_bIsSynced)
            _m_sServer.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_DATA.ordinal()
                    , GOM2CD_R_Writer_001_DataOp.make_002_SyncData(getDataType(), getGroupId(), _m_sServer.getServerTypeId(), _data));
    }

    /**
     * 进行数据初始化同步的时候，每次发送的数据队列长度上限是多少
     * 避免消息过大。分页发送是为了避免消息数量过多
     * @return
     */
    protected abstract int _getInitSyncDataPerPageCount();

    /**
     * 在非初始化行为时调用了添加数据的事件处理函数
     * @param _data
     */
    protected abstract void _onAddData_InLock(DT _data);

    /**
     * 在移除数据时触发的事件处理函数
     * @param _data
     */
    protected abstract void _onRmvData_InLock(DT _data);
}
