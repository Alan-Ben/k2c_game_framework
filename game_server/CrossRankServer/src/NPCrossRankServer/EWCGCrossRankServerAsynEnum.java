package NPCrossRankServer;

/**************
 * 异步任务枚举
 * @author Administrator
 *
 */
public enum EWCGCrossRankServerAsynEnum
{
    NONE,
    MAIN_DB,//数据库线程
    REF_RELOAD,//配表重新加载
    LOG_DB,//Log数据库线程
}
