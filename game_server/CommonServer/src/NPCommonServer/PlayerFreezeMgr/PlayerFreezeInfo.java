package NPCommonServer.PlayerFreezeMgr;


import CSDB.Bo.PlayerFreezeInfoBO;
import NPCommon.Util.CommonFunc;
import NPCommonServer.NPCommonServer;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-29 16:38:59
 */
public class PlayerFreezeInfo
{
    /**
     * 所属管理器
     */
    private final PlayerFreezeMgr _m_playerFreezeMgr;
    /**
     * 冻结记录
     */
    private PlayerFreezeInfoBO _m_freezeBo;

    public PlayerFreezeInfo(PlayerFreezeMgr _playerFreezeMgr, PlayerFreezeInfoBO _bo)
    {
        _m_playerFreezeMgr = _playerFreezeMgr;
        _m_freezeBo = _bo;
    }

    public PlayerFreezeMgr getFreezeMgr()
    {
        return _m_playerFreezeMgr;
    }

    public PlayerFreezeInfoBO getFreezeBo()
    {
        return _m_freezeBo;
    }

    public long getFreezeTime()
    {
        return getFreezeBo().getFreezeTimeMs();
    }

    /**
     * 更新封禁时间
     * @param _freezeTime 封禁结束时间
     */
    public void freeze(long _freezeTime)
    {
        _m_freezeBo.saveFreezeTimeMs(NPCommonServer.getInstance().getBM(), _freezeTime);
    }

    @Override
    public String toString()
    {
        return "\nPlayerFreezeInfo{" +
                " uid: " + getFreezeBo().getUid() +
                " endTimeMs: " + getFreezeBo().getFreezeTimeMs() +
                " leftTime: " + (getFreezeBo().getFreezeTimeMs() - CommonFunc.getNowTimeMS()) +
                " }";
    }
}
