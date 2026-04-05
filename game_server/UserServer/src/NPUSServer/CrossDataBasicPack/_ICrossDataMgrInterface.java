package NPUSServer.CrossDataBasicPack;

/**
 * 作为跨服数据管理器需要实现的接口对象
 */
public interface _ICrossDataMgrInterface {
    /**
     * 获取数据类型
     * @return
     */
    int getDataType();
    /**
     * 在跨服数据管理服务器上线的时候触发的广播处理函数，一般需要各管理器进行信息同步
     */
    void onCrossDataServerOnline();
}
