package NPUSServer.UserIndexMgr;

/***************
 * 获取用户Id对应Cid结果的回调类
 * @author mj
 *
 */
public abstract class _AUserIndexCallback
{
    /*****************
     * 获取对应Uid下的Cid数据回调处理
     * @param _uid
     * @param _cid
     */
    public abstract void getCidForUid(String _uid, long _cid);
}
