package NPUSServer.NPUSUserMgr.UserComp.EveningDungeonComp;

import Common.Common_LongList;
import Common.DungeonObj.EveningDungeon_Info;
import GS2GC.p024_DungeonOp.GS2GC_024_061_OnEveningDungeonInfoChg;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerEveningDungeonBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class EveningDungeonInfo
{
    private EveningDungeonComponent _m_comp;
    private PlayerEveningDungeonBO _m_bo;
    private List<Long> _m_hadFightHeroList;

    public EveningDungeonInfo(EveningDungeonComponent _comp, PlayerEveningDungeonBO _bo)
    {
        _m_comp = _comp;
        _m_bo = _bo;

        _m_hadFightHeroList = new ArrayList<>();
        if (_bo.getHadFightHeroList() != null)
        {
            Common_LongList proto = new Common_LongList();
            proto.readPackage(ByteBuffer.wrap(_bo.getHadFightHeroList()));
            _m_hadFightHeroList = proto.getValueList();
        }
    }

    /**
     * 获取玩家数据
     * @return
     */
    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 检查是否可以使用
     * @param _roundStartTimeMs
     * @param _heroId
     * @return
     */
    public boolean checkHeroCanUse(long _roundStartTimeMs, long _heroId)
    {
        //检查是否在当前轮
        if (_roundStartTimeMs != _m_bo.getRoundStartTimeMs())
            return true;

        return !_m_hadFightHeroList.contains(_heroId);
    }

    /**
     * 检查是否可以使用
     * @param _roundStartTimeMs
     * @param _heroId
     * @return
     */
    public boolean recordHeroUse(long _roundStartTimeMs, long _heroId)
    {
        //检查是否在当前轮
        if (_roundStartTimeMs != _m_bo.getRoundStartTimeMs())
        {
            _m_bo.setRoundStartTimeMs(_m_comp.getUSServer().getBM(), _roundStartTimeMs);
            _m_hadFightHeroList.clear();
        }else
        {
            //检查是否已经使用
            if (_m_hadFightHeroList.contains(_heroId))
                return false;
        }

        _m_hadFightHeroList.add(_heroId);

        Common_LongList hadFightHeroList = new Common_LongList();
        hadFightHeroList.getValueList().addAll(_m_hadFightHeroList);
        _m_bo.setHadFightHeroList(_m_comp.getUSServer().getBM(), hadFightHeroList.makePackage().array());
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        onInfoChg();

        return true;
    }

    /**
     * 数据变动推送
     */
    public void onInfoChg()
    {
        getUserData().sendMsgToGC(new GS2GC_024_061_OnEveningDungeonInfoChg(makeInfo()));
    }

    /**
     * 清除数据
     */
    public void cleanRecord()
    {
        _m_hadFightHeroList.clear();

        Common_LongList hadFightHeroList = new Common_LongList();
        _m_bo.saveHadFightHeroList(_m_comp.getUSServer().getBM(), hadFightHeroList.makePackage().array());

        onInfoChg();
    }

    /**
     * 构造副本信息
     * @return
     */
    public EveningDungeon_Info makeInfo()
    {
        EveningDungeon_Info info = new EveningDungeon_Info();
        info.setRoundStartTimeMS(_m_bo.getRoundStartTimeMs());
        info.getHadFightHeroList().addAll(_m_hadFightHeroList);
        return info;
    }
}
