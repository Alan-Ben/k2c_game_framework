using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class InnStationInfo
    {
        [NotNull] private readonly InnStationRefObj _m_refObj;
        [NotNull] private readonly List<InnDishInfo> _m_relatedDishInfos;
        private readonly ReadOnlyList<InnDishInfo> _m_uiDishInfos;

        private InnStationLevelRefObj _m_levelRefObj;
        private InnStationLevelRefObj _m_nextLevelRefObj;
        private bool _m_isBuilt;


        public InnStationInfo([NotNull] InnStationRefObj _stationRef, [NotNull] List<InnDishInfo> _relatedDishInfos, [NotNull] List<InnDishInfo> _uiDishInfos)
        {
            _m_refObj = _stationRef;
            _m_relatedDishInfos = _relatedDishInfos;
            _m_uiDishInfos = _uiDishInfos;
            _m_isBuilt = false;
            _updateLevel(1);
        }
        
        
        public event Action onLevelChg;
        public event Action onBuilt;
        public long stationId { get { return _m_refObj.id; } }
        [NotNull] public InnStationRefObj refObj { get { return _m_refObj; } }
        public InnStationLevelRefObj levelRefObj { get { return _m_levelRefObj; } }
        public InnStationLevelRefObj nextLevelRefObj { get { return _m_nextLevelRefObj; } }
        public bool isLevelMax { get { return _m_nextLevelRefObj == null; } }
        public bool isBuilt { get { return _m_isBuilt; } }
        public int level { get { return _m_levelRefObj?.level ?? 1; } }
        public string nameTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.name); } }
        public string descTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.desc); } }
        public ReadOnlyList<InnDishInfo> uiDishInfos { get { return _m_uiDishInfos; } }


        [Pure]
        public NPGGoIndex getCurrentResIndex()
        {
            return _m_isBuilt ? _m_refObj.td_res_index : _m_refObj.unbuilt_td_res_index;
        }
        public string getCurrentTipTranslated()
        {
            return _m_isBuilt ? _m_levelRefObj.levelUpTipTranslated : _m_refObj.unlockTipTranslated;
        }
        public bool isUnlockable()
        {
            if (_m_isBuilt)
                return false;
            
            long curHadSettleGuestsCount = NPPlayer.instance.getValue(ENPPlayerValueType.INN_HAD_RECEIVE_GUEST_COUNT);
            long needReceiveGuestNum = _m_refObj.need_receive_guest_num;
            InnStationInfo nextBuildableStation = NPPlayer.instance.innComp.nextBuildableStationInfo;
            return curHadSettleGuestsCount >= needReceiveGuestNum && this == nextBuildableStation;
        }
        public bool isUpgradable()
        {
            if (!NPPlayer.instance.innComp.canUpgradeStation())
                return false;
            
            if (_m_isBuilt == false || _m_levelRefObj == null || _m_nextLevelRefObj == null)
                return false;

            return GCommon.isItemEnough(_m_levelRefObj.upgrade_cost, false);
        }


        internal void _updateLevel(int _level)
        {
            if (_m_levelRefObj != null && _level == _m_levelRefObj.level)
                return;

            _removeDishBonus();
            _m_levelRefObj = GRefdataCoreMgr.instance.getInnStationLevelRef(_m_refObj.id, _level);
            _m_nextLevelRefObj = GRefdataCoreMgr.instance.getInnStationLevelRef(_m_refObj.id, _level + 1);
            _addDishBonus();
            onLevelChg?.Invoke();
        }
        internal void _setBuilt()
        {
            if (_m_isBuilt)
                return;

            _m_isBuilt = true;
            onBuilt?.Invoke();
        }
        
        
        private void _addDishBonus()
        {
            if (_m_levelRefObj == null)
                return;

            foreach (InnDishInfo dishInfo in _m_relatedDishInfos)
            {
                if (dishInfo == null)
                    continue;
                
                dishInfo._addPopularity(_m_levelRefObj.popularity_add);
                dishInfo._addAffection(_m_levelRefObj.affection_add);
                dishInfo._addFinesse(_m_levelRefObj.finesse_add);
            }
        }
        private void _removeDishBonus()
        {
            if (_m_levelRefObj == null)
                return;

            foreach (InnDishInfo dishInfo in _m_relatedDishInfos)
            {
                if (dishInfo == null)
                    continue;
                
                dishInfo._removePopularity(_m_levelRefObj.popularity_add);
                dishInfo._removeAffection(_m_levelRefObj.affection_add);
                dishInfo._removeFinesse(_m_levelRefObj.finesse_add);
            }
        }
    }
}