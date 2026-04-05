package RankingCreate;

import ALServerLog.ALServerLog;
import NPGameRes.Refs.Rank.RefRank;

/******************
 * 排行榜框架中创建中间存储排行榜的管理器
 */
public class RankingCreateMgr
{
    //单例模式
    private static RankingCreateMgr _g_instance = new RankingCreateMgr();
    public static RankingCreateMgr getInstance()
    {
        return _g_instance;
    }

    //环境对象
    private _IRankingCreateEnv _m_iRankingEnv;
    //是否初始化
    private boolean _m_isInit;
    private boolean _m_isInitDone;

    public RankingCreateMgr()
    {
        _m_iRankingEnv = null;
        _m_isInit = false;
        _m_isInitDone = false;

    }

    public _IRankingCreateEnv getEnv()
    {
        return _m_iRankingEnv;
    }

    /****************
     * 检查初始化状态
     * @return
     */
    public boolean checkInit()
    {
        //判断是否有开始初始化
        if(!_m_isInit)
        {
            ALServerLog.Error("RankingCreateMgr has not start initialized!");
            return false;
        }

        //判断是否结束初始化
        if(!_m_isInitDone)
        {
            ALServerLog.Error("RankingCreateMgr has not finish initialized!");
            return false;
        }

        return true;
    }


    /**
     * 设置排行榜处理对象的环境接口对象
     */
    public synchronized boolean init(_IRankingCreateEnv _env)
    {
        if (_m_isInit)
        {
            ALServerLog.Error("RankingEventMgr has been initialized!");
            return false;
        }
        //直接设置变量，避免多次调用初始化
        _m_isInit = true;

        _m_iRankingEnv = _env;

        //此处调用环境中的初始化处理
        if(null != _m_iRankingEnv)
            _m_iRankingEnv.onCreateMgrInit(this);

        //调用处理后再设置变量，确保不会过程中判断状态错误
        _m_isInitDone = true;

        return true;
    }

    /**
     * 创建排行榜
     * @param _rankId 排行榜id
     * @return 排行榜实例id
     */
    public long createRank(long _rankId)
    {
        if (!checkInit())
        {
            return 0;
        }

        //获取排行数据
        RefRank rankRef = RefRank.getMgr().get(_rankId);
        if(null == rankRef)
        {
            ALServerLog.Error("RankingCreateMgr.createRank: can't find rankRef by id: " + _rankId);
            return 0;
        }

        return getEnv().createRankList(_rankId);
    }

    /**
     * 销毁指定排行榜
     * @param _rankInstanceId 排行榜实例id
     */
    public void discardRank(long _rankInstanceId)
    {
        if (!checkInit())
        {
            return;
        }

        getEnv().discardRankList(_rankInstanceId);
    }
}
