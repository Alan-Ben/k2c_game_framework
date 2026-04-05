using Common.MuseumObj;
using JetBrains.Annotations;

namespace GOE
{
    public class MuseumItemInfo
    {
        private readonly long _m_itemId;
        [NotNull] private readonly MuseumItemRefObj _m_refObj;
        [NotNull] private readonly CommonUnionBonusMgr _m_bonusMgr;
        private bool _m_isActive;
        private bool _m_isObtain;
        private long _m_obtainTimeMS;
        
        [CanBeNull] private MuseumItemInfoLevelProperty _m_levelProperty;
        
        public MuseumItemInfo([NotNull] MuseumItemRefObj _refObj, [NotNull] CommonUnionBonusMgr _bonusMgr)
        {
            _m_refObj = _refObj;
            _m_bonusMgr = _bonusMgr;
            _m_itemId = _refObj.id;
            _m_isObtain = false;
            _updateIsActive(false);
            _updateLevel(1);
        }
        
        
        public long itemId { get { return _m_itemId; } }
        [NotNull] public MuseumItemRefObj refObj { get { return _m_refObj; } }
        public bool isObtain { get { return _m_isObtain; } }
        public bool isActive { get { return _m_isActive; } }
        
        public MuseumItemInfoLevelProperty levelProperty { get { return _m_levelProperty; } }
        public long obtainTimeMS { get { return _m_obtainTimeMS; } }


        internal void _updateFromServerData([NotNull] Museum_ItemInfo _serverInfo)
        {
            _updateIsActive(_serverInfo.getIsActive());
            _updateLevel(_serverInfo.getLevel());
            _setIsObtain(_serverInfo.getGainTimeMs());
        }
        internal void _setIsObtain(long _obtainTimeMS)
        {
            _m_isObtain = true;
            _m_obtainTimeMS = _obtainTimeMS;
        }
        internal void _updateIsActive(bool _isActive)
        {
            if (_isActive == _m_isActive)
                return;

            _m_isActive = _isActive;
            if (_isActive)
                _applyTheProperties();
            else
                _removeTheProperties();
        }
        internal void _updateLevel(int _level)
        {
            MuseumItemLevelRefObj levelRef = GRefdataCoreMgr.instance.getMuseumItemLevelRefObj(_m_itemId, _level);
            MuseumItemUpgradeCostRefObj costRef = GRefdataCoreMgr.instance.getMuseumItemUpgradeCostRefObj(_m_refObj.upgrade_cost_group_id, _level);
            _tryRemoveProperty(_m_levelProperty);
            _m_levelProperty = null;
            if (levelRef != null)
            {
                _m_levelProperty = new MuseumItemInfoLevelProperty(levelRef, costRef, _m_bonusMgr);
                _tryAddProperty(_m_levelProperty);
            }
        }
        
        
        private void _applyTheProperties()
        {
             _m_levelProperty?.apply();
        }
        private void _removeTheProperties()
        {
            _m_levelProperty?.remove();
        }
        private void _tryAddProperty(_IMuseumItemInfoProperty _property)
        {
            if (!_m_isActive)
                return;
            
            _property?.apply();
        }
        private void _tryRemoveProperty(_IMuseumItemInfoProperty _property)
        {
            if (!_m_isActive)
                return;
            
            _property?.remove();
        }
    }
}