package NPUSServer.NPUSUserMgr.UserComp.EveningDungeonComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerEveningDungeonBO;

public class EveningDungeonComponent extends _ANPUserComponent
{
    private EveningDungeonInfo _m_dungeonInfo;

    public EveningDungeonComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MIDDAY_DUNGEON);
    }

    public EveningDungeonInfo getDungeonInfo()
    {
        return _m_dungeonInfo;
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerEveningDungeonBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerEveningDungeonBO>()
        {
            @Override
            public void dealSuc(PlayerEveningDungeonBO _bo)
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

                PlayerEveningDungeonBO bo = new PlayerEveningDungeonBO();
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
    private void _setBo(PlayerEveningDungeonBO _bo)
    {
        _m_dungeonInfo = new EveningDungeonInfo(this, _bo);
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
