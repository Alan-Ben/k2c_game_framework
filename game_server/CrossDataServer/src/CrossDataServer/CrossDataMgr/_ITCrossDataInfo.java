package CrossDataServer.CrossDataMgr;

import ALBasicProtocolPack._IALProtocolStructure;

/**
 * 存储每个实际存储数据的数据结构体
 * 带入的模板结构体用于在多个服务器之间同步相关数据信息
 */
public interface _ITCrossDataInfo<T extends _IALProtocolStructure>
{
    /**
     * 获取索引数据的唯一Id，方便数据同步的时候进行索引
     * @return
     */
    long getDataId();

    /**
     * 同步数据调用的同步函数
     * @param _data
     */
    void syncData(T _data);
}
