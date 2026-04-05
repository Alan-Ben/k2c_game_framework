package NPUSServer.Guild.Dispatch;

import Common.GuildObj.Guild_DispatchHeroDetailInfo;
import CommonEnum.ESpecAttrType;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPGameRes.Refs.Hero.RefHero;
import USDB.Bo.GuildHeroDispatchBO;

public class GuildHeroDispatchInfo
{
    private RefHero _m_refHero;
    private GuildHeroDispatchBO _m_bo;
    private GuildHeroDispatchMgr _m_mgr;

    private int _m_level;
    private long _m_power;
    private long _m_skinId;

    private LazyTaskDealer _m_infoSaver;

    public GuildHeroDispatchInfo(RefHero _refHero, GuildHeroDispatchBO _bo, GuildHeroDispatchMgr _mgr)
    {
        _m_refHero = _refHero;
        _m_bo = _bo;
        _m_mgr = _mgr;
        _m_level = _bo.getLevel();
        _m_power = _bo.getPower();
        _m_skinId = _bo.getSkinId();
        _m_infoSaver = new LazyTaskDealer(this::_saveInfo, 3000);
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }

    public long getHeroId()
    {
        return _m_bo.getHeroId();
    }

    public ESpecAttrType getSpecAttrType()
    {
        return _m_refHero.spec_attr_type;
    }

    public int getAddValue()
    {
        return _m_bo.getAddValue();
    }

    public long getLevel()
    {
        return _m_level;
    }

    public long getPower()
    {
        return _m_power;
    }

    /**
     * 替换大臣
     * @param _refHero
     * @param _addValue
     */
    public void replaceHero(RefHero _refHero, int _addValue)
    {
        _m_refHero = _refHero;

        _m_bo.setHeroId(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _refHero.Id());
        _m_bo.setAddValue(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _addValue);
        _m_bo.saveAllMarked(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    /**
     * 更新大臣加成值
     * @param _addValue
     */
    public void updateAddValue(int _addValue)
    {
        if (_addValue == _m_bo.getAddValue())
            return;

        _m_bo.saveAddValue(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _addValue);
    }

    /**
     * 更新大臣信息
     * @param _level
     * @param _power
     */
    public void updateInfo(int _level, long _power, long _skinId)
    {
        if (_level == _m_level && _power == _m_power && _m_skinId == _skinId)
            return;

        _m_level = _level;
        _m_power = _power;
        _m_skinId = _skinId;

        _m_infoSaver.setNeedDeal();
    }

    /**
     * 保存大臣信息
     */
    private void _saveInfo()
    {
        _m_bo.setLevel(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _m_level);
        _m_bo.setPower(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _m_power);
        _m_bo.setSkinId(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _m_skinId);
        _m_bo.saveAllMarked(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    public void discard()
    {
        _m_bo.del(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    /**
     * 构造详细信息
     * @return
     */
    public Guild_DispatchHeroDetailInfo makeDetailInfo()
    {
        Guild_DispatchHeroDetailInfo info = new Guild_DispatchHeroDetailInfo();
        info.setCid(_m_bo.getCid());
        info.setHeroId(_m_refHero.Id());
        info.setLevel(_m_level);
        info.setPower(_m_power);
        info.setSkinId(_m_skinId);
        return info;
    }
}
