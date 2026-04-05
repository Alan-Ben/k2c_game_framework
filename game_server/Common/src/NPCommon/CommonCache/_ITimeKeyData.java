package NPCommon.CommonCache;

/*****
 * 实现该接口的对象，可以应用于TimeKeyLinkList
 * @param <K>
 */
public interface _ITimeKeyData<K>
{
    K getkey();//返回key

    int getTimeStamp();//返回秒计数的时间戳

    void setTimeStamp(int _timeStamp);//设置时间戳，秒

    void callbackOnRemoved();//被移除的时候回调
}
