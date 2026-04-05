package NPUSServer.RankingInstance;

import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ERankingInstanceServerType;
import NPGameRes.Refs.Rank.RefRank;
import NPUSServer.NPUserServer;
import NPUSServer.RankingInstance.Dealer.ToUsRankingInstanceDealer;
import NPUSServer.USLog;

import java.util.List;

public class USRankingInstanceFunc
{
    private NPUserServer _m_server;

    private _IToServerRankingInstanceDealer[] _m_dealerList;

    public USRankingInstanceFunc(NPUserServer _server)
    {
        _m_server = _server;

        _m_dealerList = new _IToServerRankingInstanceDealer[ERankingInstanceServerType.ERankingInstanceServerType_Length];

        //注册相关处理器
        _regDealer(new ToUsRankingInstanceDealer(_server));
    }

    public NPUserServer getUSServer(){return _m_server;}

    /**
     * 注册处理器对象
     * @param _dealer 处理器对象
     */
    private void _regDealer(_IToServerRankingInstanceDealer _dealer)
    {
        if (null == _dealer) return;

        _m_dealerList[_dealer.getServerType().ordinal()] = _dealer;
    }

    /**
     * 获取目标服务器的处理器对象
     * @param _serverType 服务器类型
     * @return 处理器对象
     */
    public _IToServerRankingInstanceDealer getTargetServerDealer(ERankingInstanceServerType _serverType)
    {
        return _m_dealerList[_serverType.ordinal()];
    }

    /**
     * 注册排行榜事件监听
     * @param _rankId          排行榜ID
     * @param _crossInstanceId 跨服实例id
     * @param _callback        注册结果回调
     */
    public void createRankingInstance(long _rankId, long _crossInstanceId, _ICallBackResultT<Long> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc createRankingInstance refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.REF_NOT_FOUND, 0L);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc createRankingInstance getTargetServerDealer is null, rankId:{} createServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, 0L);
            return;
        }

        //发起注册
        dealer.dealCreateOp(_rankId, _crossInstanceId, _callback);
    }

    /**
     * 注销排行榜事件监听
     * @param _rankInstanceId 排行榜ID
     * @param _callback       注册结果回调
     */
    public void discardRankingInstance(long _rankId, long _rankInstanceId, _ICallBackResult _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc discardRankingInstance refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.REF_NOT_FOUND);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc discardRankingInstance getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR);
            return;
        }

        //发起注销
        dealer.dealDiscardOp(_rankInstanceId, _callback);
    }

    /**
     * 注销排行榜事件监听
     * @param _rankInstanceId 排行榜实例id
     */
    public void onScoreChg(long _rankId, long _rankInstanceId, long _cid, long _scoreSourceId, long _score, _ICallBackBool _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc onScoreChg refRank is null, rankId:{} cid:{} score:{}", _rankId, _cid, _score);
            _callback.onRunOver(false);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc onScoreChg getTargetServerDealer is null, rankId:{} triggerServerType:{} cid:{} score:{}",
                    _rankId, refRank.instance_hold_server, _cid, _score);
            _callback.onRunOver(false);
            return;
        }

        //调用分数变更处理
        dealer.onScoreChg(_rankInstanceId, _cid, _scoreSourceId, _score, _callback);
    }

    /**
     * 获取排行榜基础排名信息列表
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     * @param _needCross      是否需要跨服
     * @param _limit          获取数量
     * @param _callback       结果回调
     */
    public void makeRankBaseList(long _rankId, long _rankInstanceId, boolean _needCross, int _limit, _ICallBackResultT<List<Rank_BaseItem>> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //调用分数变更处理
        dealer.makeRankBaseList(_rankInstanceId, _needCross, _limit, _callback);
    }
    
    /**
     * 获取排行榜基础排名信息列表通过usId过滤
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     * @param _needCross      是否需要跨服
     * @param _limitRank      最大排名
     * @param _callback       结果回调
     */
    public void dumpRankListByUs(NPUserServer _server, long _rankId, long _rankInstanceId, boolean _needCross, int _limitRank, _ICallBackResultT<List<Rank_ItemDump>> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseListByUs getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //拉取排行榜数据
        dealer.dumpRankListByUs(_rankInstanceId, _needCross, _server.getServerTypeId(), _limitRank, _callback);
    }

    /**
     * 获取排行榜基础排名信息 通过排名
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     * @param _rank           排名
     * @param _needCross      是否需要跨服
     * @param _callback       结果回调
     */
    public void makeRankBaseByRank(long _rankId, long _rankInstanceId, int _rank, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //调用查询接口
        dealer.makeRankBaseByRank(_rankInstanceId, _rank, _needCross, _callback);
    }

    /**
     * 获取排行榜基础排名信息 通过主体id
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     * @param _key            主体id
     * @param _needCross      是否需要跨服
     * @param _callback       结果回调
     */
    public void makeRankBaseByKey(long _rankId, long _rankInstanceId, long _key, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //调用查询接口
        dealer.makeRankBaseByKey(_rankInstanceId, _key, _needCross, _callback);
    }
    public void makeRankBaseByKey2(long _rankId, long _rankInstanceId, long _key, long _subKey, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //调用查询接口
        dealer.makeRankBaseByKey2(_rankInstanceId, _key, _subKey, _needCross, _callback);
    }

    /**
     * 获取排行榜当前玩家数量
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     */
    public void getRankListSize(long _rankId, long _rankInstanceId, boolean _needCross, _ICallBackResultT<Integer> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //调用查询接口
        dealer.getRankListSize(_rankInstanceId, _needCross, _callback);
    }

    /**
     * 获取指定排行榜子数据列表
     * @param _rankId
     * @param _rankInstanceId
     * @param _needCross
     * @param _key
     * @param _callback
     */
    public void makeRankBaseSubList(long _rankId, long _rankInstanceId, boolean _needCross, long _key, _ICallBackResultT<List<Rank_BaseSubItem>> _callback)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", _rankId);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc makeRankBaseList getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            _callback.onRunOver(CommErr.SYS_ERR, null);
            return;
        }

        //调用分数变更处理
        dealer.makeRankBaseSubListByKey(_rankInstanceId, _needCross, _key, _callback);
    }

    /**
     * 变更跨服实例id
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     */
    public void chgCrossInstanceId(long _rankId, long _rankInstanceId, long _crossInstanceId)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc chgCrossInstanceId refRank is null, rankId:{}", _rankId);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc chgCrossInstanceId getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            return;
        }

        //调用查询接口
        dealer.chgCrossInstanceId(_rankInstanceId, _crossInstanceId);
    }

    /**
     * 变更跨服实例id
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     */
    public void removeSubObj(long _rankId, long _rankInstanceId, long _objId, long _subObjId)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc chgCrossInstanceId refRank is null, rankId:{}", _rankId);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc chgCrossInstanceId getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            return;
        }

        //调用查询接口
        dealer.removeSubObj(_rankInstanceId, _objId, _subObjId);
    }

    /**
     * 变更跨服实例id
     * @param _rankId         排行榜ID
     * @param _rankInstanceId 排行榜实例id
     */
    public void removeObj(long _rankId, long _rankInstanceId, long _objId)
    {
        //查找对应的排行榜配置
        RefRank refRank = RefRank.getMgr().get(_rankId);
        if (refRank == null)
        {
            USLog.error(_m_server, "USRankingCreateFunc chgCrossInstanceId refRank is null, rankId:{}", _rankId);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingInstanceDealer dealer = getTargetServerDealer(refRank.instance_hold_server);
        if (null == dealer)
        {
            USLog.error(_m_server, "USRankingCreateFunc chgCrossInstanceId getTargetServerDealer is null, rankId:{} triggerServerType:{}", _rankId, refRank.instance_hold_server);
            return;
        }

        //调用查询接口
        dealer.removeObj(_rankInstanceId, _objId);
    }
}
