package NPUSServer.NPUSUserMgr.UserComp.AnnouncementComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import USDB.Bo.PlayerAnnouncementRecordBO;

import java.util.ArrayList;
import java.util.List;

public class AnnouncementComponent extends _ANPUserComponent
{
    private List<Long> _m_hadDrawList;

    public AnnouncementComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ANNOUNCEMENT);

        _m_hadDrawList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUserData().getUSServer().getBM().getBM(PlayerAnnouncementRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerAnnouncementRecordBO>>()
        {
            @Override
            public void dealSuc(List<PlayerAnnouncementRecordBO> _boList)
            {
                for (PlayerAnnouncementRecordBO bo : _boList)
                {
                    _m_hadDrawList.add(bo.getAnnouncementId());
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
     * 返回是否领取过公告奖励
     * @param _announcementId 公告ID
     * @return
     */
    public boolean hasDraw(long _announcementId)
    {
        getUserData().lockUser();
        try{
            return _m_hadDrawList.contains(_announcementId);
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 记录领取过公告奖励
     * @param _announcementId 公告ID
     * @return
     */
    public Result recordDraw(long _announcementId)
    {
        getUserData().lockUser();
        try{
            if (hasDraw(_announcementId))
                return HttpErr.ANNOUNCEMENT_HAD_DRAW;

            PlayerAnnouncementRecordBO bo = new PlayerAnnouncementRecordBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setAnnouncementId(getUSServer().getBM(), _announcementId);
            bo.insert(getUSServer().getBM());

            _m_hadDrawList.add(_announcementId);

            return Result.SUCC;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清空领取记录
     * @return
     */
    public void cleanRecord()
    {
        getUserData().lockUser();
        try{
            _m_hadDrawList.clear();

            getUserData().getUSServer().getBM().getBM(PlayerAnnouncementRecordBO.class).delAll("cid", getUserData().getCid());
        }finally
        {
            getUserData().unlockUser();
        }
    }
}
