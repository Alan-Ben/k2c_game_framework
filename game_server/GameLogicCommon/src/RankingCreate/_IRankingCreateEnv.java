package RankingCreate;

import WCGBasicServer._AWCGBasicServer;

/************
 * 排行榜相关环境的接口对象
 * 如果要开启排行榜相关创建机制，需要实现本环境接口
 */
public interface _IRankingCreateEnv
{
    /************
     * 处理本排行事件的服务器对象
     * @return
     */
    _AWCGBasicServer getBasicServerObj();

    /*****************
     * 在管理器初始化的时候会调用的接口函数
     * @param _mgr
     * @return
     */
    boolean onCreateMgrInit(RankingCreateMgr _mgr);

    /*************
     * 在具体的服务器中请求创建排行榜
     * @param _rankId 排行榜ID
     */
    long createRankList(long _rankId);

    /*************
     * 在具体的服务器中注销排行榜对象
     * @param _rankInstanceId 排行榜实例ID
     */
    void discardRankList(long _rankInstanceId);
}
