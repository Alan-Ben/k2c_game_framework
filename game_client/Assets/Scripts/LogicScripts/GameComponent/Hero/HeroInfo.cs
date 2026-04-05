using System;
using System.Collections.Generic;
using System.Text;
using Common.HeroObj;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴信息
    /// </summary>
    public class HeroInfo : _IHeroCardShow
    {
        //伙伴id
        private long _m_lId;
        //当前皮肤Id
        private long _m_lCurSkinId;
        //觉醒星级
        private long _m_lStar;
        //伙伴静态数据
        private HeroRefObj _m_heroRefObj;
        //当前阶段
        private HeroStepRefObj _m_curHeroStepRef;
        //下一阶段
        private HeroStepRefObj _m_nextHeroStepRef;
        //当前等级信息
        private HeroLevelRefObj _m_curHeroLvlRef;
        //下一等级信息
        private HeroLevelRefObj _m_nextHeroLvlRef;
        //光环信息
        private HeroHaloInfo _m_haloInfo;
        //皮肤列表管理
        [NotNull] private HeroSkinInfoMgr _m_skinInfoMgr = new HeroSkinInfoMgr();
        //资质技能列表管理
        [NotNull] private HeroTalentSkillInfoMgr _m_talentSkillInfoMgr = new HeroTalentSkillInfoMgr();
        //经营技能列表管理
        [NotNull] private HeroBusinessSkillInfoMgr _m_businessSkillInfoMgr = new HeroBusinessSkillInfoMgr();
        //觉醒技能列表管理
        [NotNull] private HeroStarSkillInfoMgr _m_starSkillInfoMgr = new HeroStarSkillInfoMgr();
        //属性容器
        private PlayerAttrPropertyContainer _m_attrPropertyContainer;
        //lazyTask处理对象
        private LazyNextFrameTaskDealer _m_lazyPowerCalcTask;
        //实力值
        private long _m_lPower;
        //驻扎建筑的数据
        private HeroPlaceBuildingData _m_placeData;
        //道具额外加成实力
        private long _m_lItemAddPower;
        //竞技场额外加成实力
        private long _m_lArenaAddPower;
        //游历额外加成实力
        private long _m_lTravelAddPower;

        /// <summary>
        /// 伙伴id
        /// </summary>
        public long id { get { return _m_lId; } }
        /// <summary>
        /// 当前皮肤Id
        /// </summary>
        public long curSkinId { get { return _m_lCurSkinId; } }
        /// <summary>
        /// 觉醒星级
        /// </summary>
        public long star { get { return _m_lStar; } }
        /// <summary>
        /// 当前等级
        /// </summary>
        public long level { get { return _m_curHeroLvlRef == null ? 0 : _m_curHeroLvlRef.level; } }
        /// <summary>
        /// 当前阶段
        /// </summary>
        public long curStep { get { return _m_curHeroStepRef == null ? 0 : _m_curHeroStepRef.step; } }
        /// <summary>
        /// 该伙伴所属的套系id
        /// </summary>
        public long belongSuitId { get { return _m_heroRefObj == null ? 0 : _m_heroRefObj.suit_id; } }
        /// <summary>
        /// 相性枚举
        /// </summary>
        public ESpecAttrType specAttrType { get { return _m_heroRefObj != null ? _m_heroRefObj.spec_attr_type : ESpecAttrType.NONE; } }
        /// <summary>
        /// 伙伴静态数据
        /// </summary>
        public HeroRefObj heroRefObj { get { return _m_heroRefObj; } }
        /// <summary>
        /// 当前阶段
        /// </summary>
        public HeroStepRefObj curHeroStepRef { get { return _m_curHeroStepRef; } }
        /// <summary>
        /// 下一阶段
        /// </summary>
        public HeroStepRefObj nextHeroStepRef { get { return _m_nextHeroStepRef; } }
        /// <summary>
        /// 当前等级信息
        /// </summary>
        public HeroLevelRefObj curHeroLvlRef { get { return _m_curHeroLvlRef; } }
        /// <summary>
        /// 下一等级信息
        /// </summary>
        public HeroLevelRefObj nextHeroLvlRef { get { return _m_nextHeroLvlRef; } }
        /// <summary>
        /// 当前皮肤数据
        /// </summary>
        public HeroSkinRefObj curSkinRefObj { get { return GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_lCurSkinId); } }
        /// <summary>
        /// 光环信息
        /// </summary>
        public HeroHaloInfo heroHaloInfo { get { return _m_haloInfo; } }
        /// <summary>
        /// 套系信息
        /// </summary>
        public HeroSuitInfo suitInfo { get { return _m_heroRefObj != null && _m_heroRefObj.suit_id > 0 ? NPPlayer.instance.heroComponent.getHeroSuitInfo(_m_heroRefObj.suit_id) : null; } }
        /// <summary>
        /// 皮肤列表管理器
        /// </summary>
        [NotNull]
        public HeroSkinInfoMgr heroSkinInfoMgr { get { return _m_skinInfoMgr; } }
        /// <summary>
        /// 资质技能列表管理器
        /// </summary>
        [NotNull]
        public HeroTalentSkillInfoMgr heroTalentSkillInfoMgr { get { return _m_talentSkillInfoMgr; } }
        /// <summary>
        /// 经营技能列表管理器
        /// </summary>
        [NotNull]
        public HeroBusinessSkillInfoMgr heroBusinessSkillInfoMgr { get { return _m_businessSkillInfoMgr; } }
        /// <summary>
        /// 觉醒技能列表管理器
        /// </summary>
        [NotNull]
        public HeroStarSkillInfoMgr starSkillInfoMgr { get { return _m_starSkillInfoMgr; } }
        /// <summary>
        /// 基础属性容器
        /// </summary>
        public PlayerAttrPropertyContainer attrPropertyContainer { get { return _m_attrPropertyContainer; } }
        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool isUnlock { get { return true; } }
        /// <summary>
        /// 是否有星辉
        /// </summary>
        public bool haveHalo { get { return _m_heroRefObj != null && _m_heroRefObj.halo_id > 0; } }
        /// <summary>
        /// 是否有觉醒技能
        /// </summary>
        public bool haveStarSkill { get { return _m_heroRefObj != null && _m_heroRefObj.star_skill_id_list != null && _m_heroRefObj.star_skill_id_list.Count > 0; } }
        /// <summary>
        /// 实力值
        /// </summary>
        public long power { get { return _m_lPower; } }
        /// <summary>
        /// 所在建筑 id
        /// </summary>
        public HeroPlaceBuildingData placeData { get { return _m_placeData; } }
        /// <summary>
        /// 道具额外加成实力
        /// </summary>
        public long itemAddPower { get { return _m_lItemAddPower; } }
        /// <summary>
        /// 竞技场额外加成实力
        /// </summary>
        public long arenaAddPower { get { return _m_lArenaAddPower; } }
        /// <summary>
        /// 游历额外加成实力
        /// </summary>
        public long travelAddPower { get { return _m_lTravelAddPower; } }


        public HeroInfo(Hero_Info _info)
        {
            if(null == _info)
                return;

            //基础属性容器
            _m_attrPropertyContainer = new PlayerAttrPropertyContainer();

            _m_lazyPowerCalcTask = new LazyNextFrameTaskDealer(new HeroReCalcPowerTask(this));

            //初始化信息
            _initInfo(_info);
            
            //计算属性
            reCalcHero();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="_info"></param>
        private void _initInfo(Hero_Info _info)
        {
            if (null == _info)
                return;

            //基础信息
            _m_lId = _info.getHeroId();
            _m_lStar = _info.getStar();
            _m_heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lId);
            if(_m_heroRefObj == null)
                Debug.LogError($"【伙伴】未获取到伙伴数据，id{_m_lId}");

            //皮肤
            updateCurSkinId(_info.getSkinId());
            _m_skinInfoMgr.initSkinList(_info.getSkinList());
            //资质技能
            _m_talentSkillInfoMgr.initTalentSKill(this, _info.getTalentSkillList());
            //经营技能
            _m_businessSkillInfoMgr.initBusinessSkillList(this, _info.getBusinessSkillList());
            //觉醒技能
            _m_starSkillInfoMgr.initStarSkillList(_m_lId,_info.getStar());
            //光环
            _m_haloInfo = new HeroHaloInfo(this, _info.getHaloInfo());
            //阶段
            updateStep(_info.getStep());
            //等级
            updateLvl(_info.getLevel());
            //所在建筑
            updatePlaceData(_info.getPlaceInfo());
            //道具额外加成实力
            updateItemAddPower(_info.getItemAddPower());
            //竞技场额外加成实力
            updateArenaAddPower(_info.getArenaAddPower());
            //游历额外加成实力
            updateTravelAddPower(_info.getTravelAddPower());

            //添加星级自身加成
            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_lId, _m_lStar);
            if(starRef != null)
                addSelfAttrModifier(starRef.self_attr_prop_modifier);
        }

        /// <summary>
        /// 更新当前皮肤id
        /// </summary>
        /// <param name="_id"></param>
        public void updateCurSkinId(long _id)
        {
            _m_lCurSkinId = _id;
        }

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="_skinInfo"></param>
        public void updateSkin(Hero_SkinInfo _skinInfo)
        {
            if (_skinInfo == null)
                return;

            _m_skinInfoMgr.updateSkinInfo(_skinInfo);
        }
        
        /// <summary>
        /// 更新资质技能
        /// </summary>
        /// <param name="_talentSkillInfo"></param>
        public void updateTalentSkillInfo(Hero_TalentSkillInfo _talentSkillInfo)
        {
            if (null == _talentSkillInfo)
                return;

            _m_talentSkillInfoMgr.updateTalentSkillInfo(this, _talentSkillInfo);
        }

        /// <summary>
        /// 更新光环
        /// </summary>
        /// <param name="_haloInfo"></param>
        public void updateHaloInfo(Hero_HaloInfo _haloInfo)
        {
            if (null == _haloInfo)
                return;

            _m_haloInfo.updateInfo(this, _haloInfo);
        }

        /// <summary>
        /// 更新阶段
        /// </summary>
        /// <param name="_step"></param>
        public void updateStep(int _step)
        {
            _m_curHeroStepRef = GRefdataCoreMgr.instance.heroStepRefCore.getRef(_step);
            _m_nextHeroStepRef = GRefdataCoreMgr.instance.heroStepRefCore.getRef(_step + 1);
        }

        /// <summary>
        /// 更新等级
        /// </summary>
        /// <param name="_lvl"></param>
        public void updateLvl(long _lvl)
        {
            _m_curHeroLvlRef = GRefdataCoreMgr.instance.heroLevelRefCore.getRef(_lvl);
            _m_nextHeroLvlRef = GRefdataCoreMgr.instance.heroLevelRefCore.getRef(_lvl + 1);

            //重新计算属性
            reCalcHero();
        }
        
        /// <summary>
        /// 更新驻扎建筑数据
        /// </summary>
        public void updatePlaceData(Hero_PlaceInfo _placeInfo)
        {
            long lastBuildingId = _m_placeData.buildingId;
            _m_placeData = new HeroPlaceBuildingData(_placeInfo);
        }

        /// <summary>
        /// 更新道具额外加成实力
        /// </summary>
        /// <param name="_itemAddPower"></param>
        public void updateItemAddPower(long _itemAddPower)
        {
            _m_lItemAddPower = _itemAddPower;
            //重新计算属性
            reCalcHero();
        }

        /// <summary>
        /// 更新竞技场加成实力
        /// </summary>
        /// <param name="_arenaAddPower"></param>
        public void updateArenaAddPower(long _arenaAddPower)
        {
            _m_lArenaAddPower = _arenaAddPower;
            //重新计算属性
            reCalcHero();
        }

        /// <summary>
        /// 游历额外加成实力
        /// </summary>
        /// <param name="_travelAddPower"></param>
        public void updateTravelAddPower(long _travelAddPower)
        {
            _m_lTravelAddPower = _travelAddPower;
            //重新计算属性
            reCalcHero();
        }

        /// <summary>
        /// 更新实力
        /// </summary>
        /// <param name="_power"></param>
        public void updatePower(long _power)
        {
            long oldPower = _m_lPower;
            _m_lPower = _power;
            WinMsg.SendMsg(WinMsgType.ON_HERO_POWER_CHG, this, oldPower, _m_lPower);
        }

        /// <summary>
        /// 更新星级
        /// </summary>
        /// <param name="_star"></param>
        public void updateStar(int _star)
        {
            //记录旧数据
            long oldStar = _m_lStar;

            //更新数据
            _m_lStar = _star;
            starSkillInfoMgr.updateStar(_star);

            //更新星级加成
            HeroStarRefObj oldStarRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_lId, oldStar);
            HeroStarRefObj newStarRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_lId, _m_lStar);
            replaceSelfAttrModifier(oldStarRef == null ? null : oldStarRef.self_attr_prop_modifier,
                newStarRef == null ? null : newStarRef.self_attr_prop_modifier);
        }

        /// <summary>
        /// 获取展示卡牌半身像
        /// </summary>
        /// <returns></returns>
        public NPGTextureIndex getCardImage()
        {
            if (null != curSkinRefObj)
                return curSkinRefObj.card_image;

            return null;
        }

        /// <summary>
        /// 获取全身形象
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTdShow()
        {
            if (null != curSkinRefObj)
                return curSkinRefObj.td_show;

            return null;
        }

        /// <summary>
        /// 获取形象背景
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTdBg()
        {
            if (null != curSkinRefObj)
                return curSkinRefObj.td_bg_index;

            return null;
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <returns></returns>
        public NPGTextureIndex getIcon()
        {
            return GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, _m_lCurSkinId);
        }

        /// <summary>
        /// 显示卡牌背景图
        /// </summary>
        /// <returns></returns>
        public NPGTextureIndex getCardBg()
        {
            if (_m_heroRefObj != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroRefObj.id);
                return qualityExtRef?.hero_card_bg;
            }
            return null;
        }

        /// <summary>
        /// 是否到达阶段等级上线,需要升阶了
        /// </summary>
        /// <returns></returns>
        public bool isStepLvlLimit()
        {
            if (null == _m_curHeroStepRef || null == _m_curHeroLvlRef)
                return false;

            return _m_curHeroStepRef.level_limit == _m_curHeroLvlRef.level;
        }

        /// <summary>
        /// 是否可升级
        /// </summary>
        /// <returns></returns>
        public bool getCanLvlUp(bool _isTen)
        {
            if (_m_curHeroLvlRef == null)
                return false;

            //消耗经验数量
            long heroExp = 0;
            if (_isTen)
            {
                GRefdataCoreMgr.instance.heroLevelRefCore.dealAllRef(_refObj =>
                {
                    if (_refObj != null &&
                        _refObj.level >= _m_curHeroLvlRef.level &&
                        _refObj.level < (_m_curHeroLvlRef.level + 10) &&
                        _m_curHeroStepRef.level_limit > _refObj.level)
                        heroExp += _refObj.need_exp;
                });
            }
            else
            {
                heroExp = _m_curHeroLvlRef.need_exp;
            }

            //是否被等阶限制
            bool stepLvlLimit = isStepLvlLimit();
            //是否可以升级
            bool canLevelUp = _m_nextHeroLvlRef != null && GCommon.isItemEnough(ENPItemType.CURRENCY, (long)ECurrency.HERO_EXP, heroExp, false);

            //是否可以升阶
            if (stepLvlLimit)
                return false;
            else
                return canLevelUp;
        }
        
        /// <summary>
        /// 获取升级消耗
        /// </summary>
        /// <param name="_isTen"></param>
        /// <returns></returns>
        public NPCommonCostItem getLevelUpCostItem(bool _isTen)
        {
            long expCount = 0;
            NPCommonCostItem costItem = null;
            if (_m_curHeroLvlRef != null)
            {
                if (_isTen)
                {
                    //如果是十连升级，遇到升阶限制的上限等级时只计算到该上限等级
                    GRefdataCoreMgr.instance.heroLevelRefCore.dealAllRef(_refObj =>
                    {
                        if (_refObj != null &&
                            _refObj.level >= _m_curHeroLvlRef.level &&
                            _refObj.level < (_m_curHeroLvlRef.level + 10) &&
                            _m_curHeroStepRef != null &&
                            _refObj.level < _m_curHeroStepRef.level_limit)
                            expCount += _refObj.need_exp;
                    });
                }
                else
                {
                    expCount = _m_curHeroLvlRef.need_exp;
                }

                costItem = new NPCommonCostItem(ENPItemType.CURRENCY, (long) ECurrency.HERO_EXP, expCount);
            }

            return costItem;
        }

        /// <summary>
        /// 获取资质升级消耗
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_isTen"></param>
        /// <returns></returns>
        public NPCommonCostItem getTalentSkillLevelUpCostItem(long _id, bool _isTen)
        {
            return _m_talentSkillInfoMgr.getTalentSkillLevelUpCostItem(_id, _isTen);
        }

        /// <summary>
        /// 获取经营技能属性加成值
        /// </summary>
        /// <param name="_businessBuildingRef"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getBusinessSkillAddPropValue(BusinessBuildingRefObj _businessBuildingRef, EBonusPropertyType _type, NPVarInfo _varInfo = null)
        {
            return _m_businessSkillInfoMgr.getBusinessSkillAddPropValue(_businessBuildingRef, _type, _varInfo);
        }

        /// <summary>
        /// 获取总资质
        /// </summary>
        /// <returns></returns>
        public long getTotalTalent()
        {
            return HeroCommon.calTalent(this);
        }

        /// <summary>
        /// 获取伙伴竞技场基础实力
        /// </summary>
        /// <returns></returns>
        public long getArenaBasePower()
        {
            return HeroCommon.calArenaBasePower(this);
        }

        #region 属性计算

        /// <summary>
        /// 获取JudgeUnionBonus
        /// </summary>
        /// <returns></returns>
        public JudgeUnionBonus toJudgeUnionBonus()
        {
            if (_m_heroRefObj == null)
                return null;

            JudgeUnionBonusPart[] judgePart = new JudgeUnionBonusPart[]
            {
                new JudgeUnionBonusPart() { filterType = EBonusFilterType.HERO_ATTR, id =  (long)_m_heroRefObj.spec_attr_type },
                new JudgeUnionBonusPart() { filterType = EBonusFilterType.HERO_ID, id =  _m_heroRefObj.id },
                new JudgeUnionBonusPart() { filterType = EBonusFilterType.QUALITY, id =  (long)GCommon.getItemQuality(ENPItemType.HERO,_m_heroRefObj.id )},
            };
            JudgeUnionBonus judgeUnionBonus = new JudgeUnionBonus(judgePart);
            return judgeUnionBonus;
        }

        /// <summary>
        /// 重新计算的处理函数，会调用对应的处理类延期计算
        /// </summary>
        public void reCalcHero(bool _isDealNextFrame = true)
        {
            //是否需要在下一帧处理
            if (_isDealNextFrame)
                _m_lazyPowerCalcTask?.setNeedDeal();
            else
                HeroCommon.dealRecalculatePower(this, false);
        }

        /// <summary>
        /// 获取玩家身上加成
        /// </summary>
        /// <param name="_propertyType"></param>
        /// <returns></returns>
        public long getPlayerBonusAddValue(EBonusPropertyType _propertyType)
        {
            long playerPowerAddition = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(_propertyType, this.toJudgeUnionBonus());
            return playerPowerAddition;
        }

        /// <summary>
        /// 添加自身属性
        /// </summary>
        /// <param name="_attModifier"></param>
        public void addSelfAttrModifier(PlayerAttrPropertyModifier _attModifier)
        {
            _m_attrPropertyContainer?.addModifier(_attModifier);
            reCalcHero();
        }

        /// <summary>
        /// 添加自身属性
        /// </summary>
        /// <param name="_attModifier"></param>
        /// <param name="_stack"></param>
        public void addSelfAttrModifier(PlayerAttrPropertyModifier _attModifier, long _stack)
        {
            _m_attrPropertyContainer?.addModifier(_attModifier, _stack);
            reCalcHero();
        }

        /// <summary>
        /// 替换自身属性
        /// </summary>
        /// <param name="_oldAttModifier"></param>
        /// <param name="_newAttrModifier"></param>
        public void replaceSelfAttrModifier(PlayerAttrPropertyModifier _oldAttModifier, PlayerAttrPropertyModifier _newAttrModifier)
        {
            _m_attrPropertyContainer?.replaceModifier(_oldAttModifier, _newAttrModifier);
            reCalcHero();
        }

        /// <summary>
        /// 替换自身属性
        /// </summary>
        /// <param name="_oldAttModifier"></param>
        /// <param name="_oldStack"></param>
        /// <param name="_newAttrModifier"></param>
        /// <param name="_newStack"></param>
        public void replaceSelfAttrModifier(PlayerAttrPropertyModifier _oldAttModifier, long _oldStack, PlayerAttrPropertyModifier _newAttrModifier, long _newStack)
        {
            _m_attrPropertyContainer?.replaceModifier(_oldAttModifier, _oldStack, _newAttrModifier, _newStack);
            reCalcHero();
        }

        #endregion


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("大臣id:").AppendLine(_m_lId.ToString());
            sb.Append("等级:").AppendLine(level.ToString());
            sb.Append("阶段:").AppendLine(curStep.ToString());
            sb.AppendLine("资质技能:").AppendLine(_m_talentSkillInfoMgr.ToString());
            sb.AppendLine("皮肤:").AppendLine(_m_skinInfoMgr.ToString());
            return sb.ToString();
        }
    }
}