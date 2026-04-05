package CrossDataServer.CrossDataMgr;

import ALServerLog.ALServerLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * 跨服数据相关调用接口类
 */
public class CrossDataController {

    /**
     * 注册一个数据类型的管理器
     * @param _dataTypeMgr
     */
    public static void regDataMgr(_ACrossDataTypeMgr _dataTypeMgr)
    {
        CrossDataCore.getInstance().regCrossDataMgr(_dataTypeMgr);
    }


    /**
     * 同步某个服务器的数据队列
     * @param _dataType
     * @param _usId
     * @param _data
     */
    public static void syncData(int _dataType, long _groupId, int _usId, byte[] _data)
    {
        //获取对应的数据管理器
        _ACrossDataTypeMgr dataMgr = CrossDataCore.getInstance().getCrossDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("Didn't reg dataMgr for dataType:" + _dataType);
            return ;
        }

        //获取集合管理对象
        _ICrossDataMgr groupMgr = dataMgr.ensureCrossDataMgr(_groupId);
        if(null == groupMgr)
        {
            ALServerLog.Error("Cross Data Group Mgr is null for type:" + _dataType + " group:" + _groupId);
            return ;
        }

        //同步数据
        groupMgr.syncData(_usId, _data);
    }
    public static void syncDataList(int _dataType, long _groupId, int _usId, ArrayList<byte[]> _dataList)
    {
        //获取对应的数据管理器
        _ACrossDataTypeMgr dataMgr = CrossDataCore.getInstance().getCrossDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("Didn't reg dataMgr for dataType:" + _dataType);
            return ;
        }

        //获取集合管理对象
        _ICrossDataMgr groupMgr = dataMgr.ensureCrossDataMgr(_groupId);
        if(null == groupMgr)
        {
            ALServerLog.Error("Cross Data Group Mgr is null for type:" + _dataType + " group:" + _groupId);
            return ;
        }

        //同步数据
        groupMgr.syncDataList(_usId, _dataList);
    }

    /**
     * 同步某个服务器的数据队列
     * @param _dataType
     * @param _usId
     * @param _dataId
     */
    public static void rmvData(int _dataType, long _groupId, int _usId, long _dataId)
    {
        //获取对应的数据管理器
        _ACrossDataTypeMgr dataMgr = CrossDataCore.getInstance().getCrossDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("Didn't reg dataMgr for dataType:" + _dataType);
            return ;
        }

        //获取集合管理对象
        _ICrossDataMgr groupMgr = dataMgr.ensureCrossDataMgr(_groupId);
        if(null == groupMgr)
        {
            ALServerLog.Error("Cross Data Group Mgr is null for type:" + _dataType + " group:" + _groupId);
            return ;
        }

        //同步数据
        groupMgr.rmvData(_usId, _dataId);
    }
    public static void rmvData(int _dataType, long _groupId, int _usId, ArrayList<Long> _dataId)
    {
        //获取对应的数据管理器
        _ACrossDataTypeMgr dataMgr = CrossDataCore.getInstance().getCrossDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("Didn't reg dataMgr for dataType:" + _dataType);
            return ;
        }

        //获取集合管理对象
        _ICrossDataMgr groupMgr = dataMgr.ensureCrossDataMgr(_groupId);
        if(null == groupMgr)
        {
            ALServerLog.Error("Cross Data Group Mgr is null for type:" + _dataType + " group:" + _groupId);
            return ;
        }

        //同步数据
        groupMgr.rmvData(_usId, _dataId);
    }


    /**
     * 从某个数据集中删除一个US的所有数据
     * @param _dataType
     * @param _usId
     */
    public static void rmvUSData(int _dataType, long _groupId, int _usId)
    {
        //获取对应的数据管理器
        _ACrossDataTypeMgr dataMgr = CrossDataCore.getInstance().getCrossDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("Didn't reg dataMgr for dataType:" + _dataType);
            return ;
        }

        //获取集合管理对象
        _ICrossDataMgr groupMgr = dataMgr.ensureCrossDataMgr(_groupId);
        if(null == groupMgr)
        {
            ALServerLog.Error("Cross Data Group Mgr is null for type:" + _dataType + " group:" + _groupId);
            return ;
        }

        //同步数据
        groupMgr.rmvUSData(_usId);
    }


    /**
     * 处理发送给数据管理器的自定义处理操作请求
     * @param _dataType
     */
    public static void dealCustomOp(int _dataType, long _groupId, _IWCGBasicRequestCommiter _commiter, ByteBuffer _protocol)
    {
        //获取对应的数据管理器
        _ACrossDataTypeMgr dataMgr = CrossDataCore.getInstance().getCrossDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("Didn't reg dataMgr for dataType:" + _dataType);
            return ;
        }

        //获取集合管理对象
        _ICrossDataMgr groupMgr = dataMgr.ensureCrossDataMgr(_groupId);
        if(null == groupMgr)
        {
            ALServerLog.Error("Cross Data Group Mgr is null for type:" + _dataType + " group:" + _groupId);
            return ;
        }

        //进行数据处理
        groupMgr._dealCustomOp(_commiter, _protocol);
    }
}
