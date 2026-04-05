package NPUSServer.CrossGameSys.CallbackDealer;

public interface _ICallBack_TryCgsServerHandle
{
    /**
     * 成功返回
     * @param _instanceId
     */
    void dealSuc(long _instanceId);

    /**
     * 失败返回
     * @param _errCode
     */
    void dealFail(int _errCode);
}
