package NPUSServer.RankingInstance.Dealer;

import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import NPCommon.CommonRank.RankObj;
import NPCommon.ErrMain.RankErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ERankingInstanceServerType;
import NPUSServer.NPUserServer;
import NPUSServer.RankingInstance._IToServerRankingInstanceDealer;
import NPUSServer.USRank.USRankList;

import java.util.List;

public class ToUsRankingInstanceDealer implements _IToServerRankingInstanceDealer
{
    private NPUserServer _m_server;

    public ToUsRankingInstanceDealer(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public ERankingInstanceServerType getServerType()
    {
        return ERankingInstanceServerType.USER;
    }

    @Override
    public void dealCreateOp(long _rankId, long _crossInstanceId, _ICallBackResultT<Long> _callback)
    {
        long rankInstanceId = getUSServer().getRankListMgr().createUsRank(_rankId, _crossInstanceId);
        _callback.onRunOver(Result.SUCC, rankInstanceId);
    }

    @Override
    public void dealDiscardOp(long _rankInstanceId, _ICallBackResult _callback)
    {
        getUSServer().getRankListMgr().closeRankList(_rankInstanceId);
        _callback.onRunOver(Result.SUCC);
    }

    @Override
    public void onScoreChg(long _rankInstanceId, long _cid, long _scoreSourceId, long _chgValue, _ICallBackBool _callback)
    {
        getUSServer().getRankListMgr().onScoreChg(_rankInstanceId, _cid, _scoreSourceId, _chgValue);
        _callback.onRunOver(true);
    }

    @Override
    public void makeRankBaseList(long _rankInstanceId, boolean _needCross, int _limit, _ICallBackResultT<List<Rank_BaseItem>> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.makeCrossRankBaseList(_limit, _callback);
        } else
        {
            List<Rank_BaseItem> rankBaseList;
            if (_limit <= 0)
            {
                rankBaseList = rankList.makeRankObjBaseList();
            } else
            {
                rankBaseList = rankList.makeRankObjBaseList(_limit);
            }

            _callback.onRunOver(Result.SUCC, rankBaseList);
        }
    }

    @Override
    public void dumpRankListByUs(long _rankInstanceId, boolean _needCross, int _usId, int _limitRank, _ICallBackResultT<List<Rank_ItemDump>> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.dumpCrossRankBaseListByUs(_usId, _limitRank, _callback);
        } else
        {
            _callback.onRunOver(Result.SUCC, rankList.dumpRankObjListWithLimitRankByUs(belongUs -> belongUs == _usId, _limitRank));
        }
    }

    @Override
    public void makeRankBaseByRank(long _rankInstanceId, int _rank, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.makeCrossRankBaseByRank(_rank, _callback);
        } else
        {
            Rank_BaseItem rankBaseItem = rankList.makeRankObjBaseByRank(_rank);

            _callback.onRunOver(Result.SUCC, rankBaseItem == null ? new Rank_BaseItem() : rankBaseItem);
        }
    }

    @Override
    public void makeRankBaseByKey(long _rankInstanceId, long _key, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.makeCrossRankBaseByKey(_key, _callback);
        } else
        {
            Rank_BaseItem rankBaseItem = rankList.makeRankObjBaseByKey(_key);

            _callback.onRunOver(Result.SUCC, rankBaseItem == null ? new Rank_BaseItem() : rankBaseItem);
        }
    }

    @Override
    public void makeRankBaseByKey2(long _rankInstanceId, long _key, long _subKey, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.makeCrossRankBaseByKey2(_key, _subKey, _callback);
        } else
        {
            Rank_BaseItem rankBaseItem = rankList.makeRankObjBaseByKey2(_key, _subKey);

            _callback.onRunOver(Result.SUCC, rankBaseItem == null ? new Rank_BaseItem() : rankBaseItem);
        }
    }

    @Override
    public void makeRankBaseSubListByKey(long _rankInstanceId, boolean _needCross, long _key, _ICallBackResultT<List<Rank_BaseSubItem>> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.makeRankBaseSubListByKey(_key, _callback);
        } 
        else
        {
            RankObj rankObj = rankList.lookup(_key);
            if(null == rankObj)
            {
            	_callback.onRunOver(Result.SUCC, null);
            	return;
            }

            _callback.onRunOver(Result.SUCC, rankObj.makeRankObjBaseSubList());
        }
    }

    @Override
    public void getRankListSize(long _rankInstanceId, boolean _needCross, _ICallBackResultT<Integer> _callback)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            _callback.onRunOver(RankErr.RANK_NO_EXIST, null);
            return;
        }

        //区分请求跨服情况
        if (_needCross)
        {
            rankList.getCrossRankListSize(_callback);
        } else
        {
            _callback.onRunOver(Result.SUCC, rankList.getRankSize());
        }
    }

    @Override
    public void chgCrossInstanceId(long _rankInstanceId, long _crossInstanceId)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            return;
        }

        rankList.chgCrossInstance(_crossInstanceId);
    }

    @Override
    public void removeObj(long _rankInstanceId, long _objId)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            return;
        }

        rankList.remove(_objId);
    }

    @Override
    public void removeSubObj(long _rankInstanceId, long _objId, long _subObjId)
    {
        USRankList rankList = getUSServer().getRankListMgr().lookup(_rankInstanceId);
        if (null == rankList)
        {
            return;
        }

        rankList.removeSubObj(_objId, _subObjId, null);
    }
}
