using JetBrains.Annotations;

namespace GOE
{
    public class MuseumItemInfoLevelProperty : _IMuseumItemInfoProperty
    {
        [NotNull] private readonly MuseumItemLevelRefObj _m_levelRefObj;
        [NotNull] private readonly CommonUnionBonusMgr _m_bonusMgr;
        private readonly MuseumItemUpgradeCostRefObj _m_costRefObj;
        private readonly MuseumItemLevelRefObj _m_nextLevelRefObj;
        
        
        public MuseumItemInfoLevelProperty([NotNull] MuseumItemLevelRefObj _levelRefObj, MuseumItemUpgradeCostRefObj _costRefObj, [NotNull] CommonUnionBonusMgr _bonusMgr)
        {
            _m_levelRefObj = _levelRefObj;
            _m_nextLevelRefObj = GRefdataCoreMgr.instance.getMuseumItemLevelRefObj(_levelRefObj.museum_id, _levelRefObj.level + 1);
            _m_costRefObj = _costRefObj;
            _m_bonusMgr = _bonusMgr;
        }
        
        
        public int level { get { return _m_levelRefObj.level; } }
        [NotNull] public MuseumItemLevelRefObj levelRefObj { get { return _m_levelRefObj; } }
        public MuseumItemLevelRefObj nextLevelRefObj { get { return _m_nextLevelRefObj; } }
        public MuseumItemUpgradeCostRefObj costRefObj { get { return _m_costRefObj; } }
        public bool isLevelMax { get { return _m_nextLevelRefObj == null; } }
        
        
        public bool canLevelUp()
        {
            if (isLevelMax)
                return false;

            return _m_costRefObj == null || GCommon.isItemEnough(_m_costRefObj.upgrade_cost_item, false);
        }
        
        
        public void apply()
        {
            if (_m_levelRefObj.bonus != null)
                _m_bonusMgr.addBonus(_m_levelRefObj.bonus.unionBonus);
        }
        public void remove()
        {
            if (_m_levelRefObj.bonus != null)
                _m_bonusMgr.removeBonus(_m_levelRefObj.bonus.unionBonus);
        }
    }
}