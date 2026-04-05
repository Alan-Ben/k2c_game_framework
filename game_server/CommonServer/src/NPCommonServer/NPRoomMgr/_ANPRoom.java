package NPCommonServer.NPRoomMgr;

import NPCommon.Util.CommonFunc;

/**************
 * 房间基类，只存储序列号和创建时间
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2019年9月4日 下午11:49:57
 */
public abstract class _ANPRoom
{
    private long _m_roomSerial;
    private long _m_lStartTime;

    public abstract void onQuit();

    public _ANPRoom(long _serial)
    {
        _m_roomSerial = _serial;
        _m_lStartTime = CommonFunc.getNowTimeMS();
    }

    public long getSerial()
    {
        return _m_roomSerial;
    }

    public long getStartTimeMs()
    {
        return _m_lStartTime;
    }

    /****************
     * 房间注册成功的事件函数
     *
     * @author alzq.z
     * @time 2019年9月4日 下午11:52:36
     */
    public abstract void onRegisted();

    /****************
     * 房间注销的事件函数
     *
     * @author alzq.z
     * @time 2019年9月4日 下午11:52:36
     */
    public abstract void onUnRegisted();
}
