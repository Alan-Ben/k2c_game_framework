package NPUSServer.NPEvent.EventMgr.EventObj;

import NPUSServer.NPUSUserMgr.NPUSUserData;

/**************************
 * 服务器全局消息中的接口对象
 * 需要基本可获得cid或者userdata
 * 如果离线消息，可以获取的userdata为空
 */
public interface _INPGlobalUserEventObj
{
    /**
     * 获取cid
     * @return
     */
    long getCid();

    /**
     * 获取userdata
     * @return
     */
    NPUSUserData getUserData();
}
