using System;
using Common.ChildObj;
using CommonEnum;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;
using NPCommon;

namespace GOE
{
    public class AdultInfo : _IChildInfo
    {
        private readonly long _m_id;
        private readonly GConsortRefObj _m_guardianRef;
        private readonly ChildInitResRefObj _m_initResRef;
        private readonly ChildAttrRefObj _m_attrRef;
        private readonly ChildQualityRefObj _m_qualityRef;
        private readonly ChildCareerRefObj _m_careerRef;
        private readonly BasicAttrRefObj _m_basicAttrRef;
        private readonly bool _m_isSuper; // 是否是卷王
        private readonly string _m_name;
        private readonly ChildResRefObj _m_resRef;
        private readonly long _m_initIntimacy;
        private readonly long _m_playerCid;
        private JudgeUnionBonusPart[] _m_bonusJudgeParts;
        private readonly long _m_earnings;
        private readonly long _m_graduateTs;
        private readonly int _m_initStudyBonus; // 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus

        private PlayerInfo_IconShow _m_playerInfo;
        private bool _m_isStartPlayerInfoInit;
        private Action _m_playerInfoInitCompleteDelegate;



        public AdultInfo(Adult_Info _serverChildInfo, long _playerCid)
        {
            _m_id = _serverChildInfo.getId();
            _m_isSuper = _serverChildInfo.getIsGiftde();
            _m_guardianRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_serverChildInfo.getConsortId());
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
            _m_resRef = GRefdataCoreMgr.instance.childResCore.getRef(_m_initResRef.getResIdByStep(_m_qualityRef.getMaxStep()));
            _m_name = _serverChildInfo.getName();
            _m_initIntimacy = _serverChildInfo.getInitIntimacy();
            _m_playerCid = _playerCid;
            _m_earnings = _serverChildInfo.getBonus();
            _m_graduateTs = _serverChildInfo.getGraduateTs();
            _m_initStudyBonus = _serverChildInfo.getInitStudyBonus();
        }


        public long id { get { return _m_id; } }
        public GConsortRefObj guardianRef { get { return _m_guardianRef; } }
        public ChildInitResRefObj initResRef { get { return _m_initResRef; } }
        public ChildQualityRefObj qualityRef { get { return _m_qualityRef; } }
        public ChildCareerRefObj careerRef { get { return _m_careerRef; } }
        public ChildAttrRefObj attrRef { get { return _m_attrRef; } }
        public BasicAttrRefObj basicAttrRef { get { return _m_basicAttrRef; } }
        public long initIntimacy { get { return _m_initIntimacy; } }
        /// <summary>
        /// 是否是卷王
        /// </summary>
        public bool isSuper { get { return _m_isSuper; } }
        public ChildResRefObj resRef { get { return _m_resRef; } }
        public string name { get { return _m_name; } }
        public int maxLevel { get { return _m_qualityRef.getMaxLvl(); } }
        public EChildSexType sex { get { return _m_initResRef.sex; } }
        public int initStudyBonus { get { return _m_initStudyBonus; } }
        /// <summary>
        /// 所属的玩家的 cid
        /// </summary>
        public long playerCid { get { return _m_playerCid; } }
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
        public long earnings { get { return _m_earnings; } }
        /// <summary>
        /// 毕业时间戳（秒）
        /// </summary>
        public long graduateTs { get { return _m_graduateTs; } }


        [Pure]
        public NPGGoIndex getResIndex()
        {
            return _m_resRef?.td_show;
        }
        [Pure]
        public string getClassroomVideoName()
        {
            return _m_attrRef.getRoomVideoNameByStep(_m_qualityRef.getMaxStep());
        }
        public void getPlayerName(Action<string> _complete)
        {
            if (_complete == null)
                return;

            _initPlayerInfo(() => { _complete.Invoke(_m_playerInfo?.getPlayerName()); });
        }
        [Pure]
        public long getEducationExpValue()
        {
            long gainValue = NPPlayer.instance?.playerInfo?.curLevelRef?.child_educate_get_hero_exp ?? 0;
            long percentage = 0;
            percentage += NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN_PER, bonusJudgeParts);
            percentage += _m_initStudyBonus;
            long finalValue = (long)Math.Ceiling(gainValue * (1 + percentage / 10000d));
            if (_m_isSuper)
                finalValue *= 2;

            finalValue += NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN, bonusJudgeParts);
            return finalValue;
        }


        private void _initPlayerInfo(Action _complete)
        {
            if (_m_playerInfo != null)
            {
                _complete?.Invoke();
                return;
            }
            
            _m_playerInfoInitCompleteDelegate += _complete;
            if (_m_isStartPlayerInfoInit)
                return;
            
            _m_isStartPlayerInfoInit = true;
            if (_m_playerCid == NPPlayer.instance.playerInfo.CID)
            {
                _m_playerInfo = new PlayerInfo_IconShow();
                _m_playerInfo.setCid(NPPlayer.instance.playerInfo.CID);
                _m_playerInfo.setPlayerName(NPPlayer.instance.playerInfo.PlayerName);
                
                Action complete = _m_playerInfoInitCompleteDelegate;
                _m_playerInfoInitCompleteDelegate = null;
                _m_isStartPlayerInfoInit = false;
                            
                complete?.Invoke();
            }
            else
            {
                NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_m_playerCid),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>(
                        (_isSuc, _msg) =>
                        {
                            if (_isSuc)
                                _m_playerInfo = _msg.getPlayerBrief();

                            Action complete = _m_playerInfoInitCompleteDelegate;
                            _m_playerInfoInitCompleteDelegate = null;
                            _m_isStartPlayerInfoInit = false;
                            
                            complete?.Invoke();
                        }));
            }
        }

        public bool canMarryWith(AdultInfo _adultInfo)
        {
            // if (_adultInfo == null)
            //     return false;
            //
            // return sex != _adultInfo.sex;
            return true;
        }
    }
}