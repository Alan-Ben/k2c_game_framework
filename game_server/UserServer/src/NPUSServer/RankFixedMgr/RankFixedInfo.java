package NPUSServer.RankFixedMgr;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.RankObj.Rank_BaseItem;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPGameRes.Refs.Rank.RefRank;
import NPGameRes.Refs.Rank.RefRankFixed;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;
import NPUSServer.RankingEvent.USRankingEventRecordCallbackMgr;
import NPUSServer.RankingEvent._IRankingEventHolder;
import NPUSServer.USLog;
import USDB.Bo.RankFixedBO;

import java.util.List;

/**
 * 常驻排行榜信息
 */
public class RankFixedInfo implements _IRankingEventHolder
{
    private NPUserServer _m_server;

    private RefRankFixed _m_ref;

    //数据库ID
    private long _m_lDbId;
    //排行榜实例id
    private long _m_lRankInstanceId;
    //事件注册序列号(本地生成)
    private long _m_lEventRegSerial;
    //事件监听注销序列号(目标事件管理器生成返回)
    private long _m_lEventUnRegSerial;

    //是否完成初始化
    private boolean _m_bHasInit;

    public RankFixedInfo(NPUserServer _server, RefRankFixed _ref, RankFixedBO _bo)
    {
        _m_server = _server;

        _m_ref = _ref;

        _m_lDbId = _bo.getId();
        _m_lRankInstanceId = _bo.getRankInstanceId();
    }

    public NPUserServer getUSServer(){return _m_server;}

    public RefRankFixed getRef()
    {
        return _m_ref;
    }

    public RefRank getRankRef()
    {
        return RefRank.getMgr().get(getRankId());
    }

    /**
     * 获取常驻排行榜配置ID
     * @return
     */
    public long getRefId()
    {
        return _m_ref.Id();
    }

    public long getRankInstanceId()
    {
        return _m_lRankInstanceId;
    }

    /**
     * 确保初始化
     */
    public void makeSureInit()
    {
        ALProcess process = ALProcess.CreateProcess("rank_fixed_init");
        //1.创建排行榜实例
        process.addResDelegateProcess(action -> _createRankInstance(action::dealAction), "create_rank_instance", null, false);
        //2.注册排行榜事件
        process.addResDelegateProcess(action -> _regRankingEvent(action::dealAction), "reg_ranking_event", null, false);
        //开始处理
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(_m_server, "RankFixedInfo _makeSureInit onRootProcessStop fail. usId:{} rankFixedId:{} rankId:{}", getUSServer().getServerTypeId(), _m_ref.Id(), getRankId());
            }

            @Override
            public void onRootProecssSuc()
            {
                _m_bHasInit = true;

                //初始化完成后处理
                onInited();
            }
        });
    }

    /**
     * 初始化完成处理
     */
    public void onInited()
    {
        //检查是否有跨服实例id变更
        onCrossInstanceIdChg(getUSServer().getLocalCrossServerGroupMgr().getCrsInstanceId());
    }

    /**
     * 创建排行榜实例
     * @param _action process回调
     */
    private void _createRankInstance(_ICallBackBool _action)
    {
        //判断是否已经创建过实例
        if (_m_lRankInstanceId > 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用创建接口发起创建排行榜实例流程
        getUSServer().getRankingInstanceFunc().createRankingInstance(getRankId(), 0, (_result, _rankInstanceId) ->
        {
            //判断创建是否成功
            if (_result.isSucc())
            {
                //创建成功则更新实例id
                _m_lRankInstanceId = _rankInstanceId;

                //更新实例id到数据库
                _saveRankInstanceId(_m_lRankInstanceId);

                _action.onRunOver(true);
            } else
            {
                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_result.getCode() == CommErr.REF_NOT_FOUND.getCode() || _result.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(_m_server, "RankFixedInfo _makeSureInit _createRankInstance fail. rankFixedId:{} rankId:{} errCode:{}",
                            _m_ref.Id(), getRankId(), _result.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> _createRankInstance(_action), 3000);
            }
        });
    }

    /**
     * 注册排行榜事件
     * @param _action 回调
     */
    private void _regRankingEvent(_ICallBackBool _action)
    {
        //判断是否已经注册过事件
        if (_m_lEventRegSerial > 0)
        {
            _action.onRunOver(true);
            return;
        }

        //生成新的事件注册序列号，用于事件注册
        _m_lEventRegSerial = USRankingEventRecordCallbackMgr.getInstance().regRankInfoCallback(this);
        //调用注册接口发起注册
        getUSServer().getRankingEventFunc().registerRankingEvent(getRankRef(), _m_lEventRegSerial, (_result, _unRegSerial) ->
        {
            //区分注册回调是否成功
            if (_result.isSucc())
            {
                _m_lEventUnRegSerial = _unRegSerial;
                _action.onRunOver(true);
            } else
            {
                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_result.getCode() == CommErr.REF_NOT_FOUND.getCode() || _result.getCode() == CommErr.SYS_ERR.getCode())
                {
                    //重置事件注册序列号
                    USRankingEventRecordCallbackMgr.getInstance().unregisterRankingEvent(_m_lEventRegSerial);
                    _m_lEventRegSerial = 0;

                    USLog.error(_m_server, "RankFixedInfo _makeSureInit _regRankingEvent fail. rankFixedId:{} rankId:{} errCode:{}",
                            _m_ref.Id(), getRankId(), _result.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> _regRankingEvent(_action), 3000);
            }
        });
    }

    /**
     * 更新实例id到数据库
     */
    private void _saveRankInstanceId(long _rankInstanceId)
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("rank_instance_id", _rankInstanceId);
        getUSServer().getBM().getBM(RankFixedBO.class).update("id", _m_lDbId, updateValue);
    }

    /**
     * 分数变更处理
     */
    public void onScoreChange(long _cid, long _scoreSourceId, long _chgScore)
    {
        if (!_m_bHasInit)
        {
            USLog.error(_m_server, "RankFixedInfo onScoreChange fail. not init. rankFixedId:{} rankId:{} rankInstanceId:{} cid:{} chgScore:{}",
                    _m_ref.Id(), getRankId(), _m_lRankInstanceId, _cid, _chgScore);
            return;
        }

        //调用排行榜实例分数变更接口
        getUSServer().getRankingInstanceFunc().onScoreChg(getRankId(), _m_lRankInstanceId, _cid, _scoreSourceId, _chgScore, _isSucc ->
        {
            if (!_isSucc)
            {
                USLog.error(_m_server, "RankFixedInfo onScoreChange fail. rankFixedId:{} rankId:{} rankInstanceId:{} cid:{} chgScore:{}",
                        _m_ref.Id(), getRankId(), _m_lRankInstanceId, _cid, _chgScore);
            }

            //如果是联盟排行需要通知联盟更新国力数据
            if (RefGeneral.Ref().guild_rank_fixed_id == getRefId())
                getUSServer().getGuildMgr().onGuildRankScoreChg(_cid);
        });
    }

    public long getRankId()
    {
        return _m_ref.rank_id;
    }

    /**
     * 获取排行榜基础排名信息列表
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseList(boolean _needCross, int _limit, _ICallBackResultT<List<Rank_BaseItem>> _callback)
    {
        getUSServer().getRankingInstanceFunc().makeRankBaseList(getRankId(), _m_lRankInstanceId, _needCross, _limit, _callback);
    }

    /**
     * 获取排行榜基础排名信息 通过排名
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseByRank(int _rank, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        getUSServer().getRankingInstanceFunc().makeRankBaseByRank(getRankId(), _m_lRankInstanceId, _rank, _needCross, _callback);
    }

    /**
     * 获取排行榜基础排名信息 通过主体id
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseByKey(long _key, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        getUSServer().getRankingInstanceFunc().makeRankBaseByKey(getRankId(), _m_lRankInstanceId, _key, _needCross, _callback);
    }

    /**
     * 获取排行榜玩家数量
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void getRankListSize(boolean _needCross, _ICallBackResultT<Integer> _callback)
    {
        getUSServer().getRankingInstanceFunc().getRankListSize(getRankId(), _m_lRankInstanceId, _needCross, _callback);
    }

    /**
     * 删除成员分数
     * @param _objId 主id
     * @param _subId 成员id
     */
    public void removeSubObj(long _objId, long _subId)
    {
        getUSServer().getRankingInstanceFunc().removeSubObj(getRankId(), _m_lRankInstanceId, _objId, _subId);
    }

    /**
     * 获取排行榜玩家数量
     */
    public void onCrossInstanceIdChg(long _crossInstanceId)
    {
        getUSServer().getRankingInstanceFunc().chgCrossInstanceId(getRankId(), _m_lRankInstanceId, _crossInstanceId);
    }
}
