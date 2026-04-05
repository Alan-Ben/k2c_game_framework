package NPUSServer.RankFixedMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.NpServerObj.NPServerObj_CrossServerGroupInfo;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.Refs.Rank.RefRankFixed;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.RankFixedBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 常驻排行榜管理器
 */
public class RankFixedMgr implements _IHandlerHolder
{
    private NPUserServer _m_usUSServer;
    private List<RankFixedInfo> _m_rankList = new ArrayList<>();
    private MutexAtom _m_mutex = new MutexAtom();


    public RankFixedMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }
    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /**
     * 从数据库加载常驻排行榜
     */
    public boolean initFromDB()
    {
        List<RankFixedBO> rankFixedBoList = getUSServer().getBM().getBM(RankFixedBO.class).s_findAll();
        if (null == rankFixedBoList)
            return false;

        for (RankFixedBO bo : rankFixedBoList)
        {
            RefRankFixed ref = RefRankFixed.getMgr().get(bo.getRefId());
            if (null == ref)
            {
                USLog.error(_m_usUSServer, "RankFixedMgr initFromDB RefRankFixed is null, refId:{}", bo.getRefId());
                continue;
            }

            RankFixedInfo rank = new RankFixedInfo(getUSServer(), ref, bo);
            _m_rankList.add(rank);
        }

        return true;
    }

    /**
     * 初始化完成
     */
    public void onInited()
    {
        //检查是否有没有开启的常驻排行榜
        checkStartRank();

        //确保初始化
        List<RankFixedInfo> rankList;
        _lock();
        try{
            rankList = new ArrayList<>(_m_rankList);
        }finally
        {
            _unlock();
        }
        for (RankFixedInfo rankInfo : rankList)
        {
            rankInfo.makeSureInit();
        }

        //监听跨服实例变更
        getUSServer().getLocalCrossServerGroupMgr().OnCrossServerGroupChg.addHandler(this, new HandlerOne<NPServerObj_CrossServerGroupInfo>()
        {
            @Override
            public void handle(NPServerObj_CrossServerGroupInfo _crossServerGroup)
            {
                onCrossInstanceIdChg(_crossServerGroup.getCrsInstanceId());
            }
        });
    }

    /**
     * 检查开启常驻排行榜
     */
    public void checkStartRank()
    {
        List<RefRankFixed> needOpenFixedRankList = new ArrayList<>();

        _lock();
        try
        {
            for (RefRankFixed refRankFixed : RefRankFixed.getMgr().getList())
            {
                if (null != lookupRank(refRankFixed.Id()))
                    continue;

                needOpenFixedRankList.add(refRankFixed);
            }
        } finally
        {
            _unlock();
        }

        //开启尚未开启的排行榜
        for (RefRankFixed ref : needOpenFixedRankList)
        {
            //创建排行榜信息
            _createRankInfo(ref);
        }
    }

    /**
     * 获取所有排行榜对象
     * @return
     */
    public List<RankFixedInfo> getAllRankList()
    {
        _lock();
        try{
            return new ArrayList<>(_m_rankList);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有排行榜对象
     * @return
     */
    public List<RankFixedInfo> getRankList(List<Long> _rankFixedIdList)
    {
        _lock();
        try{
            List<RankFixedInfo> rankList = new ArrayList<>();
            for (RankFixedInfo rankInfo : _m_rankList)
            {
                if (_rankFixedIdList.contains(rankInfo.getRefId()))
                {
                    rankList.add(rankInfo);
                }
            }
            return rankList;
        }finally
        {
            _unlock();
        }
    }


    /**
     * 获取排行榜信息
     * @param _refId
     * @return
     */
    public RankFixedInfo lookupRank(long _refId)
    {
        _lock();
        try{
            for (RankFixedInfo rank : _m_rankList)
            {
                if (rank.getRefId() == _refId)
                {
                    return rank;
                }
            }
            return null;
        }finally
        {
            _unlock();
        }
    }

    /**************
     * 创建排行榜信息
     * @param _ref
     * @return
     */
    private RankFixedInfo _createRankInfo(RefRankFixed _ref)
    {
        _lock();
        try
        {
            RankFixedBO bo = new RankFixedBO();
            bo.setRefId(getUSServer().getBM(), _ref.Id());
            bo.insert(getUSServer().getBM());

            RankFixedInfo rank = new RankFixedInfo(getUSServer(), _ref, bo);
            _m_rankList.add(rank);

            return rank;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 跨服实例变更处理
     * @param _crossInstanceId 跨服实例id
     */
    public void onCrossInstanceIdChg(long _crossInstanceId)
    {
        //确保初始化
        List<RankFixedInfo> rankList;
        _lock();
        try{
            rankList = new ArrayList<>(_m_rankList);
        }finally
        {
            _unlock();
        }
        for (RankFixedInfo rankInfo : rankList)
        {
            rankInfo.onCrossInstanceIdChg(_crossInstanceId);
        }
    }
}
