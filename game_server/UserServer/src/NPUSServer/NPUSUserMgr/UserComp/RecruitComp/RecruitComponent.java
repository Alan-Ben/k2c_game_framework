package NPUSServer.NPUSUserMgr.UserComp.RecruitComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.RefRecruit;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import USDB.Bo.PlayerRecruitBO;

import java.util.ArrayList;
import java.util.List;

public class RecruitComponent extends _ANPUserComponent
{
    private List<Long> _m_recruitList;

    public RecruitComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.RECRUIT);

        _m_recruitList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerRecruitBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerRecruitBO>>()
        {
            @Override
            public void dealSuc(List<PlayerRecruitBO> _boList)
            {
                for (PlayerRecruitBO bo : _boList)
                {
                    _m_recruitList.add(bo.getRecruitId());
                }

                setInited();
            }

            @Override
            public void dealFail()
            {
                getUserData().setDataLoadFail();
            }
        });

    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {

    }

    @Override
    public void dispose()
    {

    }

    /**
     * 获取已兑换列表
     * @return
     */
    public List<Long> getRecruitList()
    {
        return _m_recruitList;
    }

    /**
     * 尝试兑换
     * @param _recruitId
     * @return
     */
    public Result tryRecruit(long _recruitId, NPPlayerContext _context)
    {
        RefRecruit refRecruit = RefRecruit.getMgr().get(_recruitId);
        if (refRecruit == null)
            return CommErr.REF_NOT_FOUND;

        //判断是否已兑换
        if (_m_recruitList.contains(_recruitId))
            return PlayerErr.RECRUIT_ALREADY_DONE;

        //判断是否满足条件
        if (!NPPlayerConditionDealerMgr.IsEnable(refRecruit.condition, getUserData(), null))
            return CommErr.CONDITION_NOT_ENABLE;

        //消耗道具
        if (!getUserData().spendItem(refRecruit.cost_item, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        //记录兑换
        PlayerRecruitBO bo = new PlayerRecruitBO();
        bo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
        bo.setRecruitId(getUserData().getUSServer().getBM(), _recruitId);
        bo.insert(getUserData().getUSServer().getBM());

        _m_recruitList.add(_recruitId);

        //发放奖励
        getUserData().gainItem(refRecruit.gain_item, _context);

        getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_073_OnRecuritRecordAdd(_recruitId));

        return Result.SUCC;
    }
}
