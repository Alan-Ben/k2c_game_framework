using System;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class InnDishInfo : IComparable<InnDishInfo>
    {
        [NotNull] private readonly InnDishRefObj _m_refObj;
        [NotNull] private readonly CommonUnionBonusMgr _m_bonusMgr;
        private readonly BasicAttrRefObj _m_attrRef;
        
        private InnDishLevelRefObj _m_levelRefObj;
        private InnDishLevelRefObj _m_nextLevelRefObj;
        private long _m_finesse; // 当前的熟练度
        private bool _m_isUnlock; // 是否解锁
        private long _m_unlockGuestNum; // 在第几个客人时解锁
        private bool _m_hasRecipe; // 是否获得菜谱
        
        private long _m_popularityAdd; // 人气加成
        private long _m_affectionAdd; // 心意值加成
        private long _m_finesseAdd; // 熟练度加成
        
        
        internal InnDishInfo([NotNull] InnDishRefObj _dishRef, [NotNull] CommonUnionBonusMgr _bonusMgr)
        {
            _m_refObj = _dishRef;
            _m_attrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _dishRef.basic_attr);
            _m_bonusMgr = _bonusMgr;
            _updateLevel(1);
            _updateFinesse(0);
        }


        public event Action onLevelChg;
        public event Action onFinesseChg;
        public long dishId { get { return _m_refObj.id; } }
        /// <summary>
        /// 菜品的序号
        /// </summary>
        public int num { get { return _m_refObj.num; } }
        [NotNull] public InnDishRefObj refObj { get { return _m_refObj; } }
        public int level { get { return _m_levelRefObj?.level ?? 1; } }
        public InnDishLevelRefObj levelRefObj { get { return _m_levelRefObj; } }
        public InnDishLevelRefObj nextLevelRefObj { get { return _m_nextLevelRefObj; } }
        public bool isLevelMax { get { return _m_nextLevelRefObj == null; } }
        public long finesse { get { return _m_finesse; } }
        public string nameTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.name); } }
        public bool isUnlock { get { return _m_isUnlock; } }
        public bool hasRecipe { get { return _m_hasRecipe; } }
        public bool canUpgrade { get { return _m_isUnlock && _m_nextLevelRefObj != null && _m_finesse >= (_m_levelRefObj?.up_need_finesse ?? 0); } }
        public long popularityAdd { get { return _m_popularityAdd; } }
        public long affectionAdd { get { return _m_affectionAdd; } }
        public long finesseAdd { get { return _m_finesseAdd; } }
        public BasicAttrRefObj attrRef { get { return _m_attrRef; } }
        public string attrNameTranslated { get { return TextTranslate.instance.getLanguage(_m_attrRef?.name); } }
        public string unlockTipTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.unlock_tip, _m_refObj.unlock_tip_params); } }
        public string unlockSmallTipTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.unlock_small_tip, _m_refObj.unlock_small_tip_params); } }
        public string descTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.desc); } }
        public long unlockGuestNum { get { return _m_unlockGuestNum; } }
        public GGUIMonoInnMenuGridItemState uiState
        {
            get
            {
                // 先判断是否解锁
                if (!_m_isUnlock)
                {
                    // 是否有菜谱
                    if (_m_hasRecipe)
                    {
                        // 是否满足解锁条件
                        if (_m_refObj.unlock_condition != null && !_m_refObj.unlock_condition.IsEnable(null))
                            return GGUIMonoInnMenuGridItemState.HAS_RECIPE_BUT_CONDITION_DISABLE; // 有菜谱但条件不满足

                        return GGUIMonoInnMenuGridItemState.HAS_RECIPE_AND_CONDITION_ENABLE; // 有菜谱且条件满足
                    }
                    else
                    {
                        if (_m_refObj.unlock_condition == null || _m_refObj.unlock_condition.IsEnable(null))
                            return GGUIMonoInnMenuGridItemState.NO_RECIPE_BUT_CONDITION_ENABLE; // 没有菜谱但条件满足
                        return GGUIMonoInnMenuGridItemState.NO_RECIPE; // 没有菜谱且条件不满足
                    }
                }
                else
                {
                    return canUpgrade ? GGUIMonoInnMenuGridItemState.UNLOCKED_AND_CAN_UPGRADE : GGUIMonoInnMenuGridItemState.UNLOCKED_BUT_CANNOT_UPGRADE; // 已解锁
                }
            }
        }


        internal void _updateUnlock(bool _unlock, long _unlockGuestNum)
        {
            _m_unlockGuestNum = _unlockGuestNum;
            
            if (_m_isUnlock == _unlock)
                return;
            
            _m_isUnlock = _unlock;
            if (_m_isUnlock)
                _addBonus();
            else
                _removeBonus();
        }
        internal void _updateHasRecipe(bool _hasRecipe)
        {
            _m_hasRecipe = _hasRecipe;
        }
        internal void _updateLevel(int _level)
        {
            if (_m_levelRefObj != null && _level == _m_levelRefObj.level)
                return;

            if (_m_isUnlock)
                _removeBonus();
            _m_levelRefObj = GRefdataCoreMgr.instance.getInnDishLevelRef(refObj.upgrade_group_id, _level);
            _m_nextLevelRefObj = GRefdataCoreMgr.instance.getInnDishLevelRef(refObj.upgrade_group_id, _level + 1);
            if (_m_isUnlock)
                _addBonus();
            onLevelChg?.Invoke();
        }
        internal void _updateFinesse(long _finesse)
        {
            if (_m_finesse == _finesse)
                return;
            
            _m_finesse = _finesse;
            onLevelChg?.Invoke();
        }

        internal void _addPopularity(long _popularityAdd)
        {
            _m_popularityAdd += _popularityAdd;
        }
        internal void _addAffection(long _affectionAdd)
        {
            _m_affectionAdd += _affectionAdd;
        }
        internal void _addFinesse(long _finesseAdd)
        {
            _m_finesseAdd += _finesseAdd;
        }
        internal void _removePopularity(long _popularityAdd)
        {
            _m_popularityAdd -= _popularityAdd;
        }
        internal void _removeAffection(long _affectionAdd)
        {
            _m_affectionAdd -= _affectionAdd;
        }
        internal void _removeFinesse(long _finesseAdd)
        {
            _m_finesseAdd -= _finesseAdd;
        }
        
        
        private void _addBonus()
        {
            if (_m_levelRefObj == null)
                return;

            _m_bonusMgr.addModifierToFilterTotal(EBonusFilterType.BUILDING_ATTR, (long)_m_refObj.basic_attr, _m_refObj.bonus_prop_modifier_per_level, _m_levelRefObj.level);
        }
        private void _removeBonus()
        {
            if (_m_levelRefObj == null)
                return;

            _m_bonusMgr.removeModifierToFilterTotal(EBonusFilterType.BUILDING_ATTR, (long)_m_refObj.basic_attr, _m_refObj.bonus_prop_modifier_per_level, _m_levelRefObj.level);
        }
        public bool checkCanUnlock()
        {
            return _m_hasRecipe && checkUnlockRequirementMet();
        }
        public bool checkConditionAndStationMetButNoRecipe()
        {
            return !_m_hasRecipe && checkUnlockRequirementMet();
        }
        public bool checkUnlockRequirementMet()
        {
            if (_m_isUnlock)
                return false;

            if (_m_refObj.unlock_condition != null && !_m_refObj.unlock_condition.IsEnable(null))
                return false;

            if (_m_refObj.need_station_id_list != null)
            {
                foreach (long id in _m_refObj.need_station_id_list)
                {
                    InnStationInfo stationInfo = NPPlayer.instance.innComp.getStationInfoById(id);
                    if (stationInfo is not { isBuilt: true })
                        return false;
                }
            }

            return true;
        }
        
        
        public int CompareTo(InnDishInfo _other)
        {
            if (_other == null)
                return -1;

            if (isUnlock && !_other.isUnlock)
                return -1;
            if (!isUnlock && _other.isUnlock)
                return 1;
            
            // 按照解锁顺序排列
            int result = unlockGuestNum.CompareTo(_other.unlockGuestNum);
            if (result != 0)
                return result;

            // 如果解锁顺序相同，则按照菜品ID排序
            return dishId.CompareTo(_other.dishId);
        }
    }
}