package NPUSServer.NPUSUserMgr.UserComp.SevenLoginComp;

import GS2GC.p004_PlayerOp.GS2GC_004_058_OnLoginCountChg;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerSevenLoginRecordBO;

import java.util.ArrayList;
import java.util.List;

public class SevenLoginComponent extends _ANPUserComponent
{
    private List<Integer> _m_hadDrawDayList;

    public SevenLoginComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.SEVEN_LOGIN);
        _m_hadDrawDayList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerSevenLoginRecordBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerSevenLoginRecordBO>>()
        {
            @Override
            public void dealSuc(List<PlayerSevenLoginRecordBO> _list)
            {
                for (PlayerSevenLoginRecordBO bo : _list)
                {
                    _m_hadDrawDayList.add(bo.getDay());
                }

                setInited();
            }

            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "Can not load PlayerSevenLoginRecordBO[cid:" + getUserData().getCid() + "]");

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
     * 获取已领取的天数列表
     * @return
     */
    public List<Integer> getHadDrawDayList()
    {
        getUserData().lockUser();
        try{
            return _m_hadDrawDayList;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 判断是否已领取
     * @param _day
     * @return
     */
    public boolean hadDraw(int _day)
    {
        getUserData().lockUser();
        try{
            return _m_hadDrawDayList.contains(_day);
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加已领取的天数
     * @param _day
     * @return
     */
    public boolean addHadDrawDay(int _day)
    {
        getUserData().lockUser();
        try{
            if (hadDraw(_day))
                return false;

            PlayerSevenLoginRecordBO bo = new PlayerSevenLoginRecordBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setDay(getUSServer().getBM(), _day);
            bo.insert(getUSServer().getBM());

            _m_hadDrawDayList.add(_day);

            getUserData().sendMsgToGC(new GS2GC_004_058_OnLoginCountChg(new ArrayList<>(_m_hadDrawDayList)));

            return true;
        }finally
        {
            getUserData().unlockUser();
        }
    }
}
