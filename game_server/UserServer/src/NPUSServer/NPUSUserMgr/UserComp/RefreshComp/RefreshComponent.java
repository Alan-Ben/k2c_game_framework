package NPUSServer.NPUSUserMgr.UserComp.RefreshComp;

import Common.CommonFuncObj.CommonFunc_Refresh;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Common.RefCommonRefresh;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerRefreshBO;

import java.util.ArrayList;
import java.util.List;

public class RefreshComponent extends _ANPUserComponent
{
    private List<RefreshInfo> _m_refreshInfoList;

    public RefreshComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.REFRESH);

        _m_refreshInfoList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerRefreshBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerRefreshBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerRefreshBO> _boList)
                    {
                        for (PlayerRefreshBO bo : _boList)
                        {
                            RefCommonRefresh ref = RefCommonRefresh.getMgr().get(bo.getRefId());
                            if (ref == null)
                            {
                                USLog.error(getUSServer(), "RefreshComponent init error, ref is null, cid:{} refId:{}",
                                        getUserData().getCid(), bo.getRefId());
                                continue;
                            }

                            _m_refreshInfoList.add(new RefreshInfo(ref, bo));
                        }

                        List<RefCommonRefresh> list = RefCommonRefresh.getMgr().getList();
                        for (RefCommonRefresh ref : list)
                        {
                            if (null == ref)
                                continue;

                            RefreshInfo refreshInfo = lookupInfo(ref.Id());
                            if (null != refreshInfo)
                                continue;

                            _m_refreshInfoList.add(new RefreshInfo(ref));
                        }

                        setInited();
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(getUSServer(), "RefreshComponent init error, cid:{}", getUserData().getCid());
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
     * 查找
     * @param _id
     * @return
     */
    public RefreshInfo lookupInfo(long _id)
    {
        getUserData().lockUser();
        try{
            for (RefreshInfo targetRewardInfo : _m_refreshInfoList)
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
     * 尝试刷新
     * @param _id
     * @param _context
     * @return
     */
    public Result dealRefresh(long _id, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try{
            RefreshInfo targetRewardInfo = lookupInfo(_id);
            if (targetRewardInfo == null)
                return CommErr.PARAM_ERROR;

            return targetRewardInfo.dealRefresh(getUserData(), _context);
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 生成proto列表
     * @return
     */
    public List<CommonFunc_Refresh> makeProtoList()
    {
        getUserData().lockUser();
        try{
            List<CommonFunc_Refresh> list = new ArrayList<>();
            for (RefreshInfo refreshInfo : _m_refreshInfoList)
            {
                if (null == refreshInfo)
                    continue;

                list.add(refreshInfo.toProto());
            }
            return list;
        }finally
        {
            getUserData().unlockUser();
        }
    }
}
