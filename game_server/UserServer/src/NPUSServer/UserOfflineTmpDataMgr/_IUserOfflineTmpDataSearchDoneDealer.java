package NPUSServer.UserOfflineTmpDataMgr;

/**
 * 当时无数据时，临时存储请求的相关请求信息对象
 * 方便查询到之后对结果进行返回
 */
public interface _IUserOfflineTmpDataSearchDoneDealer {

    /**
     * 查询到数据后的处理函数
     */
    public void onSearchDone_InLock(_IUserOfflineTmpDataInfo _dataInfo);
}
