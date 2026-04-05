package NPUSServer.NPUSUserMgr.UserComp.LikeRecordComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.Common_TodayLikeCidInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Event.Events.Event_P_COLLECT_LIKE_COUNT_CHG;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerLikeRecordBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class LikeRecordComponent extends _ANPUserComponent
{
    private long _m_dbId;
    private int _m_lastRefreshDate;
    private List<Long> _m_todayHadLikeCidList;

    public LikeRecordComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.LIKE_RECORD);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerLikeRecordBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerLikeRecordBO>()
        {
            @Override
            public void dealSuc(PlayerLikeRecordBO _bo)
            {
                _initBo(_bo);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "player:{} load PlayerLikeRecordBO fail.", getUserData().getCid());
                    getUserData().setDataLoadFail();
                    return;
                }

                PlayerLikeRecordBO bo = new PlayerLikeRecordBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setLastRefreshDate(getUSServer().getBM(), CommonFunc.getNowTagYYYYMMDD());
                bo.insert(getUSServer().getBM());

                _initBo(bo);
            }
        });
    }

    /**
     * 初始化数据
     * @param _bo
     */
    private void _initBo(PlayerLikeRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_lastRefreshDate = _bo.getLastRefreshDate();
        _m_todayHadLikeCidList = new ArrayList<>();
        if (_bo.getLikeCidList() != null)
        {
            Common_LongList cidList = new Common_LongList();
            cidList.readPackage(ByteBuffer.wrap(_bo.getLikeCidList()));
            _m_todayHadLikeCidList = cidList.getValueList();
        }

        setInited();
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        Event_P_COLLECT_LIKE_COUNT_CHG event = new Event_P_COLLECT_LIKE_COUNT_CHG(
                getUserData().getPlayerInitContext(), getUSServer().getCollectLikeMgr().getLikeCount(getUserData().getCid()));
        getUserData().onLogicEvent(event);
    }

    @Override
    public void dispose()
    {

    }

    /**
     * 记录点赞
     * @param _cid
     * @return
     */
    public Result recordLike(long _cid)
    {
        //检查是否需要刷新
        _checkRefresh();

        //检查是否已经点赞
        if (_m_todayHadLikeCidList.contains(_cid))
            return PlayerErr.TODAY_HAD_LIKE;

        _m_todayHadLikeCidList.add(_cid);

        //推送变更
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_073_OnTodayLikeCidInfoChg(makeProto()));

        //更新到数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        Common_LongList cidList = new Common_LongList();
        cidList.getValueList().addAll(_m_todayHadLikeCidList);
        updateValue.addValueObj("like_cid_list", cidList.makePackage().array());
        getUSServer().getBM().getBM(PlayerLikeRecordBO.class).update("id", _m_dbId, updateValue);

        return Result.SUCC;
    }

    /**
     * 检查是否需要刷新
     */
    private void _checkRefresh()
    {
        int nowDate = CommonFunc.getNowTagYYYYMMDD();
        if (_m_lastRefreshDate == nowDate)
            return;

        _m_lastRefreshDate = nowDate;
        _m_todayHadLikeCidList.clear();
    }

    /**
     * 构造协议
     * @return
     */
    public Common_TodayLikeCidInfo makeProto()
    {
        Common_TodayLikeCidInfo obj = new Common_TodayLikeCidInfo();
        obj.setLastFreshDate(_m_lastRefreshDate);
        obj.getLikeCidList().addAll(_m_todayHadLikeCidList);
        return obj;
    }
}
