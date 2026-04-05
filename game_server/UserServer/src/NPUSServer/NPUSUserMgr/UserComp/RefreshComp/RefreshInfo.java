package NPUSServer.NPUSUserMgr.UserComp.RefreshComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.CommonFuncObj.CommonFunc_Refresh;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Common.RefCommonRefresh;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import USDB.Bo.PlayerRefreshBO;

public class RefreshInfo
{
    private RefCommonRefresh _m_ref;
    private long _m_dbId;
    private long _m_nextRefreshTimeMs;

    public RefreshInfo(RefCommonRefresh _ref)
    {
        _m_ref = _ref;
        _m_nextRefreshTimeMs = 0;
    }

    public RefreshInfo(RefCommonRefresh _ref, PlayerRefreshBO _bo)
    {
        _m_ref = _ref;
        _m_dbId = _bo.getId();
        _m_nextRefreshTimeMs = _bo.getNextRefreshTimeMs();
    }

    public long getRefId()
    {
        return _m_ref.Id();
    }

    /**
     * 执行刷新
     * @return
     */
    public Result dealRefresh(NPUSUserData _userdata, NPPlayerContext _context)
    {
        //判断是否到了刷新时间
        if (CommonFunc.getNowTimeMS() < _m_nextRefreshTimeMs)
            return PlayerErr.REFRESH_TIME_NOT_ARRIVED;

        //计算下次刷新时间
        _m_nextRefreshTimeMs = _m_ref.refresh_clock.getNextFreshTimeTagMS(CommonFunc.getNowTimeMS());

        //处理效果
        NPPlayerEffectDealer.dealEffect(_m_ref.effect, _userdata, null, _context);

        //保存数据
        BM bmObj = _userdata.getUSServer().getBM();
        if (_m_dbId == 0)
        {
            PlayerRefreshBO bo = new PlayerRefreshBO();
            bo.setCid(bmObj, _userdata.getCid());
            bo.setRefId(bmObj, _m_ref.Id());
            bo.setNextRefreshTimeMs(bmObj, _m_nextRefreshTimeMs);
            bo.insert(bmObj);
            _m_dbId = bo.getId();
        } else
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("next_refresh_time_ms", _m_nextRefreshTimeMs);
            bmObj.getBM(PlayerRefreshBO.class).update("id", _m_dbId, updateValue);
        }

        //通知客户端
        _userdata.sendMsgToGC(US2GCWriter_021_PlayerInfo.make_073_OnCommonRefreshChg(toProto()));

        return Result.SUCC;
    }

    /**
     * 构造信息
     * @return
     */
    public CommonFunc_Refresh toProto()
    {
        CommonFunc_Refresh proto = new CommonFunc_Refresh();
        proto.setId(_m_ref.Id());
        proto.setNextRefreshTimeMs(_m_nextRefreshTimeMs);
        return proto;
    }
}
