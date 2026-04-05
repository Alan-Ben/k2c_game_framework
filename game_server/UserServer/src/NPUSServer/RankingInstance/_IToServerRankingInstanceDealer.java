package NPUSServer.RankingInstance;

import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ERankingInstanceServerType;

import java.util.List;

public interface _IToServerRankingInstanceDealer
{
    /**
     * 获取该功能类所针对的服务器类型
     * @return 服务器类型
     */
    ERankingInstanceServerType getServerType();

    /**
     * 开始发起创建
     */
    void dealCreateOp(long _rankId, long _crossInstanceId, _ICallBackResultT<Long> _callback);

    /**
     * 开始发起注销
     */
    void dealDiscardOp(long _rankInstanceId, _ICallBackResult _callback);

    /**
     * 处理分数变更
     */
    void onScoreChg(long _rankInstanceId, long _cid, long _scoreSourceId, long _chgValue, _ICallBackBool _callback);

    /**
     * 获取排行榜基础排名信息列表
     */
    void makeRankBaseList(long _rankInstanceId, boolean _needCross, int _limit, _ICallBackResultT<List<Rank_BaseItem>> _callback);

    /**
     * 拉取指定服务器排行榜基础排名信息列表
     */
    void dumpRankListByUs(long _rankInstanceId, boolean _needCross, int _usId, int _limitRank, _ICallBackResultT<List<Rank_ItemDump>> _callback);

    /**
     * 获取排行榜基础排名信息 通过排名
     */
    void makeRankBaseByRank(long _rankInstanceId, int _rank, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback);

    /**
     * 获取排行榜基础排名信息 通过主体id
     */
    void makeRankBaseByKey(long _rankInstanceId, long _key, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback);
    
    /**
     * 获取排行榜基础排名信息 通过（主体id+子id）
     */
    void makeRankBaseByKey2(long _rankInstanceId, long _key, long _key2, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback);
    
    /**
     * 获取排行榜基础子数据 通过（主体id）
     */
    void makeRankBaseSubListByKey(long _rankInstanceId, boolean _needCross, long _key, _ICallBackResultT<List<Rank_BaseSubItem>> _callback);

    /**
     * 获取排行榜当前玩家数量
     */
    void getRankListSize(long _rankInstanceId, boolean _needCross, _ICallBackResultT<Integer> _callback);

    /**
     * 变更跨服实例id
     */
    void chgCrossInstanceId(long _rankInstanceId, long _crossInstanceId);

    /**
     * 移除对象
     */
    void removeObj(long _rankInstanceId, long _objId);

    /**
     * 移除子对象
     */
    void removeSubObj(long _rankInstanceId, long _objId, long _subObjId);
}
