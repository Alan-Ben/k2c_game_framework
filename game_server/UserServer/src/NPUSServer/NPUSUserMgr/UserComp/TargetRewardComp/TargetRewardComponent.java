package NPUSServer.NPUSUserMgr.UserComp.TargetRewardComp;

import Common.ActivityEnum.EActivityState;
import Common.CommonFuncObj.CommonFunc_TargetReward;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.Refs.Common.RefCommonTargetReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerTargetRewardBO;

import java.util.ArrayList;
import java.util.List;

public class TargetRewardComponent extends _ANPUserComponent implements _IHandlerHolder
{
    private List<TargetRewardInfo> _m_targetRewardList;

    public TargetRewardComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.TARGET_REWARD);
        _m_targetRewardList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerTargetRewardBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerTargetRewardBO>>()
        {
            @Override
            public void dealSuc(List<PlayerTargetRewardBO> _boList)
            {
                for (PlayerTargetRewardBO bo : _boList)
                {
                    RefCommonTargetReward ref = RefCommonTargetReward.getMgr().get(bo.getRefId());
                    if (ref == null)
                    {
                        USLog.error(getUSServer(), "TargetRewardComponent init error, ref is null, cid:{} refId:{}",
                                getUserData().getCid(), bo.getRefId());
                        continue;
                    }
                    _m_targetRewardList.add(new TargetRewardInfo(TargetRewardComponent.this,bo, ref));
                }

                List<RefCommonTargetReward> list = RefCommonTargetReward.getMgr().getList();
                for (RefCommonTargetReward ref : list)
                {
                    if (null == ref)
                        continue;

                    TargetRewardInfo targetRewardInfo = lookupInfo(ref.Id());
                    if (null != targetRewardInfo)
                        continue;

                    _m_targetRewardList.add(new TargetRewardInfo(TargetRewardComponent.this, ref));
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
        getUserData().lockUser();
        try
        {
            getUSServer().getCommActivityMgr().activityStateChg.addHandler(this, new HandlerTwo<_AActivityBase, EActivityState>()
            {
                @Override
                public void handle(_AActivityBase _activity, EActivityState _state)
                {
                    getUserData().lockUser();
                    try
                    {
                        for (TargetRewardInfo targetRewardInfo : _m_targetRewardList)
                        {
                            if (null == targetRewardInfo)
                                continue;

                            targetRewardInfo.checkActivityState(_activity.getInstanceId(), _activity.getActivityId(),_state);
                        }
                    } finally
                    {
                        getUserData().unlockUser();
                    }
                }
            });

            //注册全部任务监听
            for (TargetRewardInfo targetRewardInfo : _m_targetRewardList)
            {
                if (null == targetRewardInfo)
                    continue;

                targetRewardInfo.initCheck();
            }
        } finally
        {
            getUserData().unlockUser();
        }

    }

    @Override
    public void dispose()
    {
        getUserData().lockUser();
        try
        {
            getUSServer().getCommActivityMgr().activityStateChg.clear(this);

            //注销全部任务监听
            for (TargetRewardInfo targetRewardInfo : _m_targetRewardList)
            {
                if (null == targetRewardInfo)
                    continue;

                targetRewardInfo.unRegEvtEntry();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找
     * @param _id
     * @return
     */
    public TargetRewardInfo lookupInfo(long _id)
    {
        getUserData().lockUser();
        try{
            for (TargetRewardInfo targetRewardInfo : _m_targetRewardList)
            {
                if (null == targetRewardInfo)
                    continue;

                if (targetRewardInfo.getRefId() == _id)
                    return targetRewardInfo;
            }

            return null;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取奖励
     * @param _id
     * @param _context
     * @return
     */
    public Result drawReward(long _id, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try{
            TargetRewardInfo targetRewardInfo = lookupInfo(_id);
            if (targetRewardInfo == null)
                return CommErr.PARAM_ERROR;

            return targetRewardInfo.drawReward(_context);
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 生成proto列表
     * @return
     */
    public List<CommonFunc_TargetReward> makeProtoList()
    {
        getUserData().lockUser();
        try{
            List<CommonFunc_TargetReward> list = new ArrayList<>();
            for (TargetRewardInfo targetRewardInfo : _m_targetRewardList)
            {
                if (null == targetRewardInfo)
                    continue;

                list.add(targetRewardInfo.toProto());
            }
            return list;
        }finally
        {
            getUserData().unlockUser();
        }
    }
}
