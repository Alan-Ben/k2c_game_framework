using System;
using System.Collections.Generic;
using Common.ChildObj;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class ChildInfo : _IChildInfo
    {
        private readonly long _m_id;
        [NotNull] private readonly GGottenConsortInfo _m_guardian;
        [NotNull] private readonly ChildInitResRefObj _m_initResRef;
        [NotNull] private readonly ChildAttrRefObj _m_attrRef;
        [NotNull] private readonly ChildQualityRefObj _m_qualityRef;
        [NotNull] private readonly ChildCareerRefObj _m_careerRef;
        [NotNull] private readonly BasicAttrRefObj _m_basicAttrRef;
        private readonly bool _m_isSuper; // 是否是卷王
        private readonly long _m_initIntimacy; // 初始亲密度
        private readonly long _m_baseBonus; // 基础加成值
        private readonly int _m_initStudyBonus; // 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus
        private JudgeUnionBonusPart[] _m_bonusJudgeParts;

        private string _m_name;
        [NotNull] private SeatInfo _m_seatInfo;
        private ChildResRefObj _m_resRef;
        private int _m_level;
        private long _m_earnings;
        
        
        public ChildInfo([NotNull] Child_Info _serverChildInfo)
        {
            _m_id = _serverChildInfo.getId();
            _m_isSuper = _serverChildInfo.getIsGiftde();
            _m_guardian = NPPlayer.instance.consortComp.getConsortInfo(_serverChildInfo.getConsortId());
            _m_initResRef = GRefdataCoreMgr.instance.childInitResCore.getRef(_serverChildInfo.getInitResId());
            if (null == _m_initResRef)
            {
                Debug.LogError($"child_init_res表中没有找到id为{_serverChildInfo.getInitResId()}的配置，请检查！");
                _m_initResRef = GRefdataCoreMgr.instance.childInitResCore.findIf((_info) => { return true;}); // 随便取一个不为 null 的，避免报错
            }
            
            _m_qualityRef = GRefdataCoreMgr.instance.childQualityCore.getRef(_serverChildInfo.getQuality());
            _m_careerRef = GRefdataCoreMgr.instance.childCareerCore.getRef(_serverChildInfo.getCareerId());
            _m_attrRef = GRefdataCoreMgr.instance.childAttrCore.getRef((long)_serverChildInfo.getAttrType());
            _m_basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_serverChildInfo.getAttrType());
            _m_initIntimacy = _serverChildInfo.getInitIntimacy();
            _m_baseBonus = _serverChildInfo.getBaseBonus();
            _m_initStudyBonus = _serverChildInfo.getInitStudyBonus();

            _updateName(_serverChildInfo.getName());
            _updateLvl(_serverChildInfo.getLvl());
            _updateEarnings(_serverChildInfo.getBonus());
            _m_seatInfo = NPPlayer.instance.childComp.getSeatInfo(_serverChildInfo.getSeatId());
            if (_m_seatInfo is { childInfo: not null })
                _m_seatInfo = null; // 座位里已经有子嗣了，这里通过设置席位为 null 让 component 把这个数据剔除
            _m_seatInfo?._setChildInfo(this);
        }
        
        
        public long id { get { return _m_id; } }
        [NotNull] public GGottenConsortInfo guardian { get { return _m_guardian; } }
        public GConsortRefObj guardianRef { get { return _m_guardian.consortRefObj; } }
        [NotNull] public ChildInitResRefObj initResRef { get { return _m_initResRef; } }
        [NotNull] public ChildQualityRefObj qualityRef { get { return _m_qualityRef; } }
        [NotNull] public ChildCareerRefObj careerRef { get { return _m_careerRef; } }
        [NotNull] public ChildAttrRefObj attrRef { get { return _m_attrRef; } }
        [NotNull] public BasicAttrRefObj basicAttrRef { get { return _m_basicAttrRef; } }
        /// <summary>
        /// 出生时妃子的亲密度
        /// </summary>
        public long initIntimacy { get { return _m_initIntimacy; } }
        /// <summary>
        /// 是否是卷王
        /// </summary>
        public bool isSuper { get { return _m_isSuper; } }
        public ChildResRefObj resRef { get { return _m_resRef; } }
        public string name { get { return _m_name; } }
        [NotNull] public SeatInfo seatInfo { get { return _m_seatInfo; } }
        public int level { get { return _m_level; } }
        public int maxLevel { get { return _m_qualityRef.getMaxLvl(); } }
        public EChildSexType sex { get { return _m_initResRef.sex; } }
        public long earnings { get { return _m_earnings; } }
        /// <summary>
        /// 当前的升学阶段
        /// </summary>
        /// <remarks>
        /// 数值从 0 开始
        /// </remarks>
        public int step { get { return _m_qualityRef.getStepByLvl(_m_level); } }
        [NotNull]
        public JudgeUnionBonusPart[] bonusJudgeParts
        {
            get
            {
                return _m_bonusJudgeParts ??= new JudgeUnionBonusPart[]
                {
                    new JudgeUnionBonusPart() { filterType = EBonusFilterType.STUDENT_ATTR, id = (long) _m_attrRef.type },
                    new JudgeUnionBonusPart() { filterType = EBonusFilterType.STUDENT_SEX, id = (long) _m_initResRef.sex }
                };
            }
        }
        public int initStudyBonus { get { return _m_initStudyBonus; } }

        
        public void getPlayerName(Action<string> _complete)
        {
            _complete?.Invoke(NPPlayer.instance.playerInfo.PlayerName);
        }
        [Pure]
        public bool canGraduate()
        {
            return _m_level >= _m_qualityRef.getMaxLvl();
        }
        [Pure]
        public bool canStepUp()
        {
            return _m_qualityRef.canStepUp(_m_level);
        }
        [Pure]
        public float getGraduationProgress()
        {
            return (float)_m_level / _m_qualityRef.getMaxLvl();
        }
        [Pure]
        public NPGGoIndex getResIndex()
        {
            return _m_resRef?.td_show;
        }
        [Pure]
        public string getClassroomVideoName()
        {
            return _m_attrRef.getRoomVideoNameByStep(_m_qualityRef.getStepByLvl(_m_level));
        }
        [Pure]
        public int getGraduatingEnergyNeed()
        {
            return Mathf.Max(0, _m_qualityRef.getMaxLvl() - _m_level - _m_seatInfo.energy);
        }
        [Pure]
        public NPCommonCostItem getEducationCost()
        {
            return new NPCommonCostItem(ENPItemType.CURRENCY, (long)ECurrency.SILVER, getEducationCostValue());
        }
        [Pure]
        public long getEducationCostValue()
        {
            long costValue = NPPlayer.instance?.playerInfo?.curLevelRef?.child_educate_cost ?? 0;
            long percentage = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_COST_PER, bonusJudgeParts);
            return (long)(costValue * (10000 - percentage) / 10000d);
        }
        [Pure]
        public long getEducationExpValue()
        {
            long gainValue = NPPlayer.instance?.playerInfo?.curLevelRef?.child_educate_get_hero_exp ?? 0;
            long percentage = 0;
            percentage += NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN_PER, bonusJudgeParts);
            percentage += _m_initStudyBonus;
            long finalValue = (long)Math.Ceiling(gainValue * (10000 + percentage) / 10000d);
            if (_m_isSuper)
                finalValue *= 2;

            finalValue += NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN, bonusJudgeParts);
            return finalValue;
        }
        [Pure]
        public long getEarningsCurrentAddValue()
        {
            // 算法抄服务端的 ========================
            
            long finalAddValue = 0;
            int nextLevel = _m_level + 1;
            //检查当前等级是否可以增加上课收益
            long addValue = (long) Math.Ceiling(_m_baseBonus * GRefdataCoreMgr.instance.npGeneral.child_add_per / 10000d);
            List<int> trainAddBonusLvlList = _m_qualityRef.addBonusStepList;
            for (int i = 0; i < trainAddBonusLvlList.Count; i++)
            {
                int curTrainAddBonusLvl = trainAddBonusLvlList[i];
                if (curTrainAddBonusLvl > nextLevel)
                    break;

                if (curTrainAddBonusLvl > _m_level)
                {
                    finalAddValue += addValue;
                }
            }

            //检查当前阶段可以增加的收益
            if (nextLevel != maxLevel && _m_qualityRef.step_lvl.Contains(nextLevel))
            {
                long addStepValue = (long)Math.Ceiling(_m_baseBonus * GRefdataCoreMgr.instance.npGeneral.child_train_add_per / 10000d);
                finalAddValue += addStepValue;
            }

            if (_m_isSuper)
                finalAddValue *= 2;

            return finalAddValue;
        }


        internal void _updateName(string _name)
        {
            _m_name = _name;
            _recalResRef();
        }
        internal void _updateLvl(int _level)
        {
            _m_level = _level;
            _recalResRef();
        }
        internal void _updateEarnings(long _earnings)
        {
            _m_earnings = _earnings;
        }

        private void _recalResRef()
        {
            if (string.IsNullOrEmpty(_m_name) && _m_initResRef.unnamed_res_id > 0)
                _m_resRef = GRefdataCoreMgr.instance.childResCore.getRef(_m_initResRef.unnamed_res_id);
            else
                _m_resRef = GRefdataCoreMgr.instance.childResCore.getRef(_m_initResRef.getResIdByStep(_m_qualityRef.getStepByLvl(_m_level)));
        }
    }
}