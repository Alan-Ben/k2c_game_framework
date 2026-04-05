package NPUSServer.NPUSUserMgr.UserComp.HeroComp;

import NPEnum.ENPItemType;
import NPGameRes.Refs.Hero.RefHeroSkin;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.USLog;

public class HeroSkinItemDealer implements _IUserItemBasicDealer
{
    private HeroComponent _m_comp;

    public HeroSkinItemDealer(HeroComponent _comp)
    {
        _m_comp = _comp;
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.HERO_SKIN;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        //查找皮肤配置
        RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(_itemId);
        if (refHeroSkin == null)
            return 0;

        //查找大臣
        HeroInfo heroInfo = _m_comp.lookupHero(refHeroSkin.hero_id);
        if (heroInfo == null)
            return 0;

        return heroInfo.getSkinMgr().lookupSkin(_itemId) != null ? 1 : 0;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        //查找皮肤配置
        RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(_itemId);
        if (refHeroSkin == null)
            return false;

        //查找大臣
        HeroInfo heroInfo = _m_comp.lookupHero(refHeroSkin.hero_id);
        if (heroInfo == null)
            return false;

        return heroInfo.getSkinMgr().lookupSkin(_itemId) != null;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        _gainSkin(_itemId, true, _context);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        _gainSkin(_itemId, false, _context);
    }

    /**
     * 获得皮肤
     * @param _itemId
     * @param _isInit
     * @param _context
     */
    private void _gainSkin(long _itemId, boolean _isInit, NPPlayerContext _context)
    {
        //查找皮肤配置
        RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(_itemId);
        if (refHeroSkin == null)
        {
            USLog.error(_m_comp.getUSServer(), "Player gain hero skin fail, refHeroSkin not found heroSkinId:{}.", _itemId);
            return;
        }

        //查找大臣
        HeroInfo heroInfo = _m_comp.lookupHero(refHeroSkin.hero_id);
        //如果没有对应大臣, 需要给玩家对应皮肤的解锁道具
        if (heroInfo == null)
        {
            _m_comp.getUserData().gainItem(refHeroSkin.unlock_item, _context);
            return;
        }

        _m_comp._lock();
        try
        {
            heroInfo.getSkinMgr().gainSkin(_itemId, _isInit, _context);
        } finally
        {
            _m_comp._unlock();
        }
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

}
