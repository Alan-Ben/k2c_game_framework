using System;
using System.Collections.Generic;
using Common.HeroObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 藏品数据对象
    /// </summary>
    public class EquipInfo : _IEquipCardShow
    {
        //数据id
        private long _m_lDbId;
        //藏品id
        private long _m_lEquipId;
        //等级
        private int _m_iLevel;
        //觉醒等级
        private int _m_iAwakenLevel;
        //穿戴大臣id
        private long _m_lWearHeroId;
        //总加成值
        private long _m_lSkillAddValue;
        //藏品配置数据
        private EquipRefObj _m_equipRef;
        //是否锁定
        private bool _m_bIsLock;

        //====技能信息====
        private List<EquipSkillInfo> _m_lSkillInfoList;
        //是否正在请求数据
        private bool _m_bIsRequestingSkill;
        //是否初始化了数据
        private bool _m_bIsInitSkillDone;
        //初始化数据完成回调
        private Action<List<EquipSkillInfo>> _m_aOnInitSkillDelegate;

        /// <summary>
        /// 数据id
        /// </summary>
        public long dbId { get { return _m_lDbId; } }
        /// <summary>
        /// 藏品id
        /// </summary>
        public long equipId { get { return _m_lEquipId; } }
        /// <summary>
        /// 等级
        /// </summary>
        public long level { get { return _m_iLevel; } }
        /// <summary>
        /// 觉醒等级
        /// </summary>
        public long awakenLevel { get { return _m_iAwakenLevel; } }
        /// <summary>
        /// 穿戴大臣id
        /// </summary>
        public long wearHeroId { get { return _m_lWearHeroId; } }
        /// <summary>
        /// 总加成值
        /// </summary>
        public long skillAddValue { get { return _m_lSkillAddValue; } }
        /// <summary>
        /// 藏品配置数据
        /// </summary>
        public EquipRefObj equipRef { get { return _m_equipRef; } }
        /// <summary>
        /// 藏品信息
        /// </summary>
        public EquipInfo equipInfo { get { return this; } }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool isLock { get { return _m_bIsLock; } }
        /// <summary>
        /// 当前资质值
        /// </summary>
        public long talentValue { get { return _m_equipRef != null ? _m_equipRef.initial_talent + (_m_iLevel - 1) * _m_equipRef.upgrade_increase_talent : 0; } }
        /// <summary>
        /// 等级上限
        /// </summary>
        public long levelLimit { get { return _m_equipRef != null ? _m_equipRef.level_limit + NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.EQUIP_LEVEL_LIMIT_ADD) : 0; } }

        /// <summary>
        /// 初始化基础信息
        /// </summary>
        /// <param name="_info"></param>
        public EquipInfo(Equip_BaseInfo _info)
        {
            if (_info == null)
                return;

            updateBaseInfo(_info);
        }

        /// <summary>
        /// 初始化完整信息
        /// </summary>
        /// <param name="_info"></param>
        public EquipInfo(Equip_Info _info)
        {
            if (_info == null)
                return;

            _m_bIsInitSkillDone = true;
            updateBaseInfo(_info.getBaseInfo());
            updateSkillInfo(_info.getSkillList());
        }

        /// <summary>
        /// 更新基础信息
        /// </summary>
        /// <param name="_baseInfo"></param>
        public void updateBaseInfo(Equip_BaseInfo _baseInfo)
        {
            if (_baseInfo == null)
                return;

            _m_lDbId = _baseInfo.getDbId();
            _m_lEquipId = _baseInfo.getEquipId();
            _m_iLevel = _baseInfo.getLevel();
            _m_iAwakenLevel = _baseInfo.getAwakenLevel();
            _m_lWearHeroId = _baseInfo.getWearHeroId();
            _m_lSkillAddValue = _baseInfo.getSkillAddValue();
            _m_bIsLock = _baseInfo.getIsLock();
            _m_equipRef = GRefdataCoreMgr.instance.equipRefCore.getRef(_m_lEquipId);
        }

        /// <summary>
        /// 更新技能信息
        /// </summary>
        /// <param name="_skillList"></param>
        public void updateSkillInfo(List<Equip_SkillInfo> _skillList)
        {
            if (_skillList == null)
                return;

            for (int i = 0; i < _skillList.Count; i++)
            {
                if (_skillList[i] != null)
                    updateSkillInfo(_skillList[i]);
            }
        }

        /// <summary>
        /// 更新技能信息
        /// </summary>
        /// <param name="_skillInfo"></param>
        public void updateSkillInfo(Equip_SkillInfo _skillInfo)
        {
            if (_skillInfo == null)
                return;

            if (_m_lSkillInfoList == null)
                _m_lSkillInfoList = new List<EquipSkillInfo>();

            for (int i = 0; i < _m_lSkillInfoList.Count; i++)
            {
                if (_m_lSkillInfoList[i] != null && _m_lSkillInfoList[i].index == _skillInfo.getIndex())
                {
                    _m_lSkillInfoList[i].updateInfo(_skillInfo);
                    return;
                }
            }

            //找不到直接新增
            EquipSkillInfo skillInfo = new EquipSkillInfo(_skillInfo, _m_lDbId);
            _m_lSkillInfoList.Add(skillInfo);
        }

        #region 获取技能详情

        /// <summary>
        /// 请求藏品技能详情列表
        /// </summary>
        /// <param name="_onComplete">完成回调</param>
        public void getEquipSkillInfoList(Action<List<EquipSkillInfo>> _onComplete)
        {
            if (_onComplete == null)
                return;

            //如果已经有数据并且不请求新数据，直接返回数据
            if (_m_bIsInitSkillDone && _m_lSkillInfoList != null)
            {
                _onComplete.Invoke(_m_lSkillInfoList);
                return;
            }
            else if (_m_aOnInitSkillDelegate == null)
                _m_aOnInitSkillDelegate = _onComplete;
            else
                _m_aOnInitSkillDelegate += _onComplete;

            //是否正在请求玩家详情
            if (!_m_bIsRequestingSkill)
                _reqSkillInfo();
        }

        /// <summary>
        /// 请求详细信息
        /// </summary>
        private void _reqSkillInfo()
        {
            _m_bIsRequestingSkill = true;
            //请求技能信息
            NPPlayer.instance.equipComp.reqEquipSkillList(_m_lDbId, _info =>
            {
                //设置状态信息
                _m_bIsRequestingSkill = false;
                _m_bIsInitSkillDone = true;
                _m_lSkillInfoList = new List<EquipSkillInfo>();
                for (int i = 0; i < _info.getSkillList().Count; i++)
                {
                    if (_info.getSkillList()[i] != null)
                    {
                        EquipSkillInfo skillInfo = new EquipSkillInfo(_info.getSkillList()[i], _m_lDbId);
                        _m_lSkillInfoList.Add(skillInfo);
                    }
                }

                //执行回调
                Action<List<EquipSkillInfo>> onDone = _m_aOnInitSkillDelegate;
                _m_aOnInitSkillDelegate = null;
                onDone?.Invoke(_m_lSkillInfoList);
            });
        }

        #endregion
    }
}