package NPUSServer.NPUSUserMgr.UserComp.MiddayDungeonComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerMiddayDungeonBO;

public class MiddayDungeonComponent extends _ANPUserComponent
{
    private MiddayDungeonInfo _m_dungeonInfo;

    public MiddayDungeonComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MIDDAY_DUNGEON);
    }

    public MiddayDungeonInfo getDungeonInfo()
    {
        return _m_dungeonInfo;
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerMiddayDungeonBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerMiddayDungeonBO>()
        {
            @Override
            public void dealSuc(PlayerMiddayDungeonBO _bo)
            {
                _setBo(_bo);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "MiddayDungeonComponent init fail, cid:{}", getUserData().getCid());
                    getUserData().setDataLoadFail();
                    return;
                }

                PlayerMiddayDungeonBO bo = new PlayerMiddayDungeonBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.insert(getUSServer().getBM());

                _setBo(bo);
            }
        });
    }

    /**
     * 设置数据
     * @param _bo
     */
    private void _setBo(PlayerMiddayDungeonBO _bo)
    {
        _m_dungeonInfo = new MiddayDungeonInfo(this, _bo);
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

    }

    @Override
    public void dispose()
    {

    }
}
