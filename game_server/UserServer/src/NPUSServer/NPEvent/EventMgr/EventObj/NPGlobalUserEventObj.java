package NPUSServer.NPEvent.EventMgr.EventObj;

import NPUSServer.NPUSUserMgr.NPUSUserData;

/*********************
 * 可以通过设置Cid或者UserData构造数据
 * 并在不同的情况返回数据
 */
public class NPGlobalUserEventObj implements  _INPGlobalUserEventObj
{
    private long _m_cid;
    private NPUSUserData _m_userData;

    /*****
     * 当没有UserData的时候使用本接口创建事件对象
     * @param _cid
     */
    public NPGlobalUserEventObj(long _cid)
    {
        _m_cid = _cid;
        _m_userData = null;
    }

    /****
     * 当有UserData的时候使用本接口创建事件对象
     * @param _userData
     */
    public NPGlobalUserEventObj(NPUSUserData _userData)
    {
        _m_cid = _userData.getCid();
        _m_userData = _userData;
    }

    @Override
    public long getCid()
    {
        return _m_cid;
    }

    @Override
    public NPUSUserData getUserData()
    {
        return _m_userData;
    }
}
