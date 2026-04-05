package CrossDataServer.CrossDataMgr;

/**
 * 单个类型的数据管理对象基本实现
 * 基于接口进行实现
 */

import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public interface _ICrossDataMgr
{
    /**
     * 同步数据信息
     * 同步的时候如果不存在数据则需要加入，如果已经存在则需要修改
     * 实际修改方式需要具体业务类内部进行处理
     * @param _data
     *
     */
	void syncData(int _usId, byte[] _data);
    /**
     * 同步某个服务器的数据队列
     * @param _dataList
     */
    void syncDataList(int _usId, ArrayList<byte[]> _dataList);

    /**
     * 同步删除相关数据
     * @param _dataId
     */
    void rmvData(int _usId, long _dataId);
    void rmvData(int _usId, ArrayList<Long> _dataId);

    /**
     * 清除某个US数据的处理
     * 一般在切换分组，或者数据重新同步的时候处理
     * @param _usId
     */
    void rmvUSData(int _usId);

    /**
     * 处理发送给数据管理器的自定义处理操作请求
     * @param _commiter
     * @param _protocol
     */
    void _dealCustomOp(_IWCGBasicRequestCommiter _commiter, ByteBuffer _protocol);
}
