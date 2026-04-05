using System;
using System.Collections.Generic;
using System.Text;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class HeroCommon
    {
        /// <summary>
        /// 计算资质值
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calTalent(HeroInfo _heroInfo, StringBuilder _sb = null)
        {
            if (null == _heroInfo)
                return 0;

            //计算资质
            long talentPoint = _heroInfo.attrPropertyContainer.getValue(EBasicAttrType.TALENT);
            if (_sb != null)
                _sb.Append("大臣资质值: ").Append(talentPoint).Append("\n");

            //获取玩家身上加成
            long playerTalentAddition = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.TALENT);
            if (_sb != null)
                _sb.Append("玩家bonus资质值: ").Append(playerTalentAddition).Append("\n");

            //获取套系加成
            long suitTalentAddition = 0;
            if (_heroInfo.suitInfo != null)
                suitTalentAddition = _heroInfo.suitInfo.attrContainer.getValue(EBasicAttrType.TALENT);
            if (_sb != null)
                _sb.Append("套系资质值: ").Append(suitTalentAddition).Append("\n");

            //获取藏品加成
            EquipInfo heroWearEquip = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_heroInfo.id);
            long equipTalentAddition = heroWearEquip != null ? heroWearEquip.talentValue : 0;
            if (_sb != null)
                _sb.Append("藏品资质值: ").Append(equipTalentAddition).Append("\n");

            long talent = talentPoint + playerTalentAddition + suitTalentAddition + equipTalentAddition;
            if (_sb != null)
                _sb.Append("总资质值: ").Append(talent).Append("\n");

            //返回累加值
            return talent;
        }

        /// <summary>
        /// 计算伙伴的实力值
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calPower(HeroInfo _heroInfo, StringBuilder _sb = null)
        {
            if (null == _heroInfo)
                return 0;

            //家人带来的加成
            PlayerAttrPropertyContainer consortAddAttrContainer = calConsortAddAttrProperty(_heroInfo);

            if (_sb != null)
                _sb.Append("大臣id: ").Append(_heroInfo.id).Append("\n");

            //1.计算基础实力
            long basicPower = calBasePower(_heroInfo, _sb);

            //2.计算万分比加成
            long powerPerAddition = _calAddPowerPer(_heroInfo, consortAddAttrContainer, _sb);

            //3.获取绝对值加成
            long powerAddition = _calAddPowerAbsolute(_heroInfo, consortAddAttrContainer, _sb);

            //4.返回累加值
            long totalPower = (long)Math.Ceiling((double)basicPower *(10000 + powerPerAddition) / 10000f + powerAddition);
            if (_sb != null)
                _sb.Append("总实力值: ").Append(totalPower).Append("\n");

            return totalPower;
        }

        /// <summary>
        /// 计算伙伴基础实力
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calBasePower(HeroInfo _heroInfo, StringBuilder _sb = null)
        {
            HeroLevelRefObj heroLevelRef = _heroInfo.curHeroLvlRef;
            long basicPower = calTalent(_heroInfo, _sb) * (heroLevelRef == null ? 0 : heroLevelRef.level_ratio);
            if (_sb != null)
                _sb.Append("实力基础值: ").Append(basicPower).Append("\n");
            return basicPower;
        }

        /// <summary>
        /// 计算伙伴总的万分比加成
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_consortAddAttrContainer"></param>
        /// <param name="_sb"></param>
        /// <returns></returns>
        private static long _calAddPowerPer(HeroInfo _heroInfo, PlayerAttrPropertyContainer _consortAddAttrContainer, StringBuilder _sb = null)
        {
            if (_heroInfo == null)
                return 0 ;

            //获取大臣自身加成
            long heroPowerPerAddition = _heroInfo.attrPropertyContainer.getValue(EBasicAttrType.POWER_PER);
            if (_sb != null)
                _sb.Append("大臣自身万分比加成: ").Append(heroPowerPerAddition).Append("\n");
            //获取套系加成
            long suitPowerPerAddition = 0;
            if (_heroInfo.suitInfo != null)
                suitPowerPerAddition = _heroInfo.suitInfo.attrContainer.getValue(EBasicAttrType.POWER_PER);
            if (_sb != null)
                _sb.Append("套系万分比加成: ").Append(suitPowerPerAddition).Append("\n");
            //获取玩家身上加成
            long playerPowerPerAddition = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.POWER_PER);
            if (_sb != null)
                _sb.Append("玩家bonus万分比加成:").Append(playerPowerPerAddition).Append("\n");
            //获取对应家人的加成
            long consortPowerPerAddition = _consortAddAttrContainer != null ? _consortAddAttrContainer.getValue(EBasicAttrType.POWER_PER) : 0;
            if (_sb != null)
                _sb.Append("家人万分比加成: ").Append(consortPowerPerAddition).Append("\n");
            //获取对应藏品的加成
            EquipInfo heroWearEquip = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_heroInfo.id);
            long equiPowerPerAddition = heroWearEquip != null ? heroWearEquip.skillAddValue : 0;
            if (_sb != null)
                _sb.Append("藏品万分比加成: ").Append(equiPowerPerAddition).Append("\n");
            //万分比加成累加值
            long powerPerAddition = heroPowerPerAddition + suitPowerPerAddition + playerPowerPerAddition + consortPowerPerAddition + equiPowerPerAddition;
            if (_sb != null)
                _sb.Append("万分比加成累加值: ").Append(powerPerAddition).Append("\n");

            return powerPerAddition;
        }

        /// <summary>
        /// 计算伙伴总的绝对值加成
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_consortAddAttrContainer"></param>
        /// <param name="_sb"></param>
        /// <returns></returns>
        private static long _calAddPowerAbsolute(HeroInfo _heroInfo, PlayerAttrPropertyContainer _consortAddAttrContainer, StringBuilder _sb = null)
        {
            //获取大臣自身加成
            long heroPowerAddition = _heroInfo.attrPropertyContainer.getValue(EBasicAttrType.POWER);
            if (_sb != null)
                _sb.Append("大臣自身绝对值加成: ").Append(heroPowerAddition).Append("\n");
            //获取套系加成
            long suitPowerAddition = 0;
            if (_heroInfo.suitInfo != null)
                suitPowerAddition = _heroInfo.suitInfo.attrContainer.getValue(EBasicAttrType.POWER);
            if (_sb != null)
                _sb.Append("套系绝对值加成: ").Append(suitPowerAddition).Append("\n");
            //获取玩家身上加成
            long playerPowerAddition = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.POWER);
            if (_sb != null)
                _sb.Append("玩家bonus绝对值加成: ").Append(playerPowerAddition).Append("\n");
            //获取对应家人的加成
            long consortPowerAddition = _consortAddAttrContainer != null ? _consortAddAttrContainer.getValue(EBasicAttrType.POWER) : 0;
            if (_sb != null)
                _sb.Append("家人绝对值加成: ").Append(consortPowerAddition).Append("\n");
            //获取道具额外的加成
            long itemAddPowerAddition = _heroInfo.itemAddPower;
            if (_sb != null)
                _sb.Append("道具绝对值加成: ").Append(itemAddPowerAddition).Append("\n");
            //获取竞技场额外的加成
            long arenaAddPowerAddition = _heroInfo.arenaAddPower;
            if (_sb != null)
                _sb.Append("竞技场绝对值加成: ").Append(arenaAddPowerAddition).Append("\n");
            //获取游历额外的加成
            long travelAddPowerAddition = _heroInfo.travelAddPower;
            if (_sb != null)
                _sb.Append("游历绝对值加成: ").Append(travelAddPowerAddition).Append("\n");
            //绝对值加成累加值
            long powerAddition = heroPowerAddition + suitPowerAddition + playerPowerAddition + consortPowerAddition + itemAddPowerAddition + arenaAddPowerAddition + travelAddPowerAddition;
            if (_sb != null)
                _sb.Append("绝对值加成累加值: ").Append(powerAddition).Append("\n");

            return powerAddition;
        }

        /// <summary>
        /// 计算家人带来的加成
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_attr"></param>
        /// <returns></returns>
        [NotNull]
        public static PlayerAttrPropertyContainer calConsortAddAttrProperty(HeroInfo _heroInfo)
        {
            if (null == _heroInfo || null == _heroInfo.heroRefObj)
                return new PlayerAttrPropertyContainer();

            PlayerAttrPropertyContainer attrPropertyContainer = new PlayerAttrPropertyContainer();
            attrPropertyContainer.addModifier(NPPlayer.instance.consortComp.getConsortRelationHeroAddPropModifier(_heroInfo.id));
            return attrPropertyContainer;
        }

        /// <summary>
        /// 计算星辉带来的属性加成，星辉(hero_halo_level)+套系(hero_halo_suit_skill_level)
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        [NotNull]
        public static PlayerAttrPropertyContainer calHaloAddAttrProperty(HeroInfo _heroInfo)
        {
            if (_heroInfo == null)
                return new PlayerAttrPropertyContainer();

            PlayerAttrPropertyContainer attrPropertyContainer = new PlayerAttrPropertyContainer();

            //添加星辉
            if (_heroInfo.heroHaloInfo != null && _heroInfo.heroHaloInfo.isUnlock && _heroInfo.heroHaloInfo.curHaloLevelRef != null)
                attrPropertyContainer.addModifier(_heroInfo.heroHaloInfo.curHaloLevelRef.self_attr_prop_modifier);

            //添加套系
            HeroSuitInfo suitInfo = _heroInfo.suitInfo;
            if(suitInfo != null)
                attrPropertyContainer.addContainer(suitInfo.attrContainer);

            return attrPropertyContainer;
        }

        /// <summary>
        /// 计算伙伴初始资质
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public static long calInitTalent(long _heroId)
        {
            //伙伴未获得时的初始资质=自动升阶的资质技能1级资质值总和（对应hero表的auto_upgrade_talent_skill_id_list字段）

            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_heroId);
            if (heroRef == null)
                return 0;

            List<long> talentSkillIdList = heroRef.auto_upgrade_talent_skill_id_list;
            if (talentSkillIdList == null || talentSkillIdList.Count == 0)
                return 0;

            long talentValue = 0;
            for (int i = 0; i < talentSkillIdList.Count; i++)
            {
                HeroTalentSkillRefObj talentSkillRef = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(talentSkillIdList[i]);
                if(talentSkillRef == null)
                    continue;

                HeroTalentSkillLevelRefObj talentSkillLevelRef = talentSkillRef.getBaseTalentSkillLevelRefByLevel(1);
                if(talentSkillLevelRef == null)
                    continue;

                talentValue += talentSkillLevelRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
            }

            return talentValue;
        }

        /// <summary>
        /// 计算伙伴所有资质技能提供的资质
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calAllSkillTalent(HeroInfo _heroInfo)
        {
            if (_heroInfo == null || _heroInfo.heroRefObj == null)
                return 0;

            long talentValue = 0;
            //资质技能列表
            List<HeroTalentSkillInfo> talentSkillInfoList = _heroInfo.heroTalentSkillInfoMgr.talentSkillInfoList;
            if (talentSkillInfoList != null)
            {
                for (int i = 0; i < talentSkillInfoList.Count; i++)
                {
                    if (talentSkillInfoList[i] == null)
                        continue;

                    if (talentSkillInfoList[i].baseTalentSkillLevelRefObj != null &&
                        talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier != null)
                    {
                        talentValue += talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
                        long levelBonusStack = talentSkillInfoList[i].levelBonusStack;
                        if (levelBonusStack > 0 && talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level != null)
                            talentValue += (talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level.getPropValue(EBasicAttrType.TALENT) * levelBonusStack);
                    }
                }
            }

            return talentValue;
        }

        /// <summary>
        /// 计算伙伴所有基础资质技能提供的资质(减去自动升阶技能和时装资质技能提供的资质)
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calBaseSkillTalent(HeroInfo _heroInfo)
        {
            if (_heroInfo == null || _heroInfo.heroRefObj == null)
                return 0;

            long talentValue = 0;

            //自动升阶技能列表
            List<long> autoUpgradeTalentSkillIdList = new List<long>();
            if(_heroInfo.heroRefObj.auto_upgrade_talent_skill_id_list != null)
                autoUpgradeTalentSkillIdList.AddRange(_heroInfo.heroRefObj.auto_upgrade_talent_skill_id_list);

            //资质技能列表
            List<HeroTalentSkillInfo> talentSkillInfoList = _heroInfo.heroTalentSkillInfoMgr.talentSkillInfoList;
            if (talentSkillInfoList != null)
            {
                for (int i = 0; i < talentSkillInfoList.Count; i++)
                {
                    if(talentSkillInfoList[i] == null)
                        continue;

                    //如果不是自动升阶技能添加到容器里，TODO 判断时装资质技能还未开发
                    if (!autoUpgradeTalentSkillIdList.Contains(talentSkillInfoList[i].talentSkillId) &&
                        talentSkillInfoList[i].baseTalentSkillLevelRefObj != null &&
                        talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier != null)
                    {
                        talentValue += talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
                        long levelBonusStack = talentSkillInfoList[i].levelBonusStack;
                        if(levelBonusStack > 0 && talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level != null)
                            talentValue += (talentSkillInfoList[i].baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level.getPropValue(EBasicAttrType.TALENT) * levelBonusStack);
                    }
                }
            }

            return talentValue;
        }

        /// <summary>
        /// 计算升阶带来的额外资质加成，当前升级资质技能资质 - 初始资质
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calStepUpAddTalent(HeroInfo _heroInfo)
        {
            if (_heroInfo == null || _heroInfo.heroRefObj == null)
                return 0;

            long talentValue = 0;

            //自动升阶技能列表
            List<long> autoUpgradeTalentSkillIdList = new List<long>();
            if (_heroInfo.heroRefObj.auto_upgrade_talent_skill_id_list != null)
                autoUpgradeTalentSkillIdList.AddRange(_heroInfo.heroRefObj.auto_upgrade_talent_skill_id_list);

            for (int i = 0; i < autoUpgradeTalentSkillIdList.Count; i++)
            {
                HeroTalentSkillInfo talentSkillInfo = _heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(autoUpgradeTalentSkillIdList[i]);
                if(talentSkillInfo == null)
                    continue;

                if (talentSkillInfo.baseTalentSkillLevelRefObj != null &&
                    talentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier != null)
                {
                    talentValue += talentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
                    long levelBonusStack = talentSkillInfo.levelBonusStack;
                    if (levelBonusStack > 0 && talentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level != null)
                        talentValue += (talentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level.getPropValue(EBasicAttrType.TALENT) * levelBonusStack);
                }
            }

            return talentValue - calInitTalent(_heroInfo.id);
        }

        /// <summary>
        /// 计算当前等级与加上等级差值的自动升阶资质技能的资质差值
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_levelDiffer">等级差值</param>
        /// <returns></returns>
        public static long calStepTalentSkillDifferentValueWithLevelDiffer(HeroInfo _heroInfo,long _levelDiffer)
        {
            if (_heroInfo == null || _heroInfo.heroRefObj == null)
                return 0;

            long differValue = 0;

            //自动升阶技能列表
            List<long> autoUpgradeTalentSkillIdList = new List<long>();
            if (_heroInfo.heroRefObj.auto_upgrade_talent_skill_id_list != null)
                autoUpgradeTalentSkillIdList.AddRange(_heroInfo.heroRefObj.auto_upgrade_talent_skill_id_list);

            //计算资质差值
            for (int i = 0; i < autoUpgradeTalentSkillIdList.Count; i++)
            {
                HeroTalentSkillInfo talentSkillInfo = _heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(autoUpgradeTalentSkillIdList[i]);
                HeroTalentSkillRefObj talentSkillRef = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(autoUpgradeTalentSkillIdList[i]);
                if (talentSkillRef == null)
                    continue;

                //当前资质
                HeroTalentSkillLevelRefObj curTalentSkillLevelRef = talentSkillRef.getBaseTalentSkillLevelRefByLevel(talentSkillInfo != null ? talentSkillInfo.level : 0);
                PlayerAttrPropertyContainer curModifier = new PlayerAttrPropertyContainer();
                if (curTalentSkillLevelRef != null)
                {
                    curModifier.addModifier(curTalentSkillLevelRef.self_attr_prop_modifier);
                    curModifier.addModifier(curTalentSkillLevelRef.self_attr_prop_modifier_per_level, talentSkillInfo.levelBonusStack);
                }
                long curTalent = curModifier.getValue(EBasicAttrType.TALENT);
                //下一级资质
                long nextTalent = 0;
                HeroTalentSkillLevelRefObj nextTalentSkillLevelRef = talentSkillRef?.getBaseTalentSkillLevelRefByLevel(talentSkillInfo != null ? talentSkillInfo.level + _levelDiffer : 1);
                if (curTalentSkillLevelRef != null && nextTalentSkillLevelRef != null)
                {
                    if (curTalentSkillLevelRef.id != nextTalentSkillLevelRef.id)
                    {
                        //如果不同的基础资质技能等级数据，重新计算资质
                        long diffValue = nextTalentSkillLevelRef.level - (talentSkillInfo != null ? talentSkillInfo.level : 0);
                        curModifier.clear();
                        curModifier.addModifier(nextTalentSkillLevelRef.self_attr_prop_modifier);
                        curModifier.addModifier(nextTalentSkillLevelRef.self_attr_prop_modifier_per_level, diffValue);
                    }
                    else
                    {
                        //如果是当前基础资质技能等级数据，直接加每级加成
                        curModifier.addModifier(curTalentSkillLevelRef.self_attr_prop_modifier_per_level, _levelDiffer);
                    }
                    nextTalent = curModifier.getValue(EBasicAttrType.TALENT);
                }
                differValue += (nextTalent - curTalent);
            }
            return differValue;
        }

        /// <summary>
        /// 计算藏品对于伙伴的实力加成值
        /// </summary>
        /// <param name="_equipInfo"></param>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calEquipAddPower(EquipInfo _equipInfo, HeroInfo _heroInfo)
        {
            //藏品实力 = 伙伴穿戴前的基础实力 * 藏品百分比加成 + 藏品带来的基础实力 *（ 1 + 伙伴穿戴后的总实力百分比 ）
            //* 伙伴穿戴前的基础实力 = 伙伴穿戴前的总资质 * 伙伴等级系数
            //* 藏品带来的基础实力 = 藏品资质 * 伙伴等级系数
        
            if (_equipInfo == null || _heroInfo == null)
                return 0;
        
            //伙伴等级系数
            HeroLevelRefObj heroLevelRef = _heroInfo.curHeroLvlRef;
        
            //当前伙伴佩戴的藏品数据
            EquipInfo heroWearEquip = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_heroInfo.id);
        
            //* 伙伴穿戴前的总资质
            long beforeEquipTotalTalent = calTalent(_heroInfo) - (heroWearEquip != null ? heroWearEquip.talentValue : 0);
        
            //* 伙伴穿戴前的基础实力
            long heroBasicPower = beforeEquipTotalTalent * (heroLevelRef == null ? 0 : heroLevelRef.level_ratio);
        
            //* 藏品带来的基础实力
            long equipBasicPower = _equipInfo.talentValue * (heroLevelRef == null ? 0 : heroLevelRef.level_ratio);
        
            //* 伙伴穿戴后的总实力百分比，总的 - 当前佩戴的 + 需要计算佩戴的
            long afterEquipPowerPer = _calAddPowerPer(_heroInfo, calConsortAddAttrProperty(_heroInfo)) - (heroWearEquip != null ? heroWearEquip.skillAddValue : 0) + _equipInfo.skillAddValue;
        
            //藏品实力 = 伙伴穿戴前的基础实力 * 藏品百分比加成 + 藏品带来的基础实力 *（ 1 + 伙伴穿戴后的总实力百分比 ）
            long equipPower = (long)Math.Ceiling(heroBasicPower * _equipInfo.skillAddValue / 10000f + equipBasicPower * (10000 + afterEquipPowerPer) / 10000f);
            return equipPower;
        }

        /// <summary>
        /// 获取伙伴竞技场增益前的实力
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <returns></returns>
        public static long calArenaBasePower(HeroInfo _heroInfo)
        {
            if (_heroInfo == null)
                return 0;

            //伙伴实力 *（1 + 伙伴觉醒加成）
            long arenaBasePower = (long)Math.Ceiling(_heroInfo.power * (1 + _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.ARENA_POWER_ADD_PER) / 10000f));
            return arenaBasePower;
        }

        /// <summary>
        /// 获取伙伴竞技场增益后的实力
        /// </summary>
        /// <param name="_arenaBasePower"></param>
        /// <param name="_buffAddPer"></param>
        /// <returns></returns>
        public static long calArenaPower(long _arenaBasePower, long _buffAddPer)
        {
            //增益前的实力 *（1 + 总临时增益万分比）
            long arenaBasePower = (long)Math.Ceiling(_arenaBasePower * (1 + _buffAddPer / 10000f));
            return arenaBasePower;
        }

        /// <summary>
        /// 处理重新计算实力
        /// </summary>
        public static void dealRecalculatePower(HeroInfo _heroInfo, bool _isDealNextFrame)
        {
            if (_heroInfo == null)
                return;

            //记录原先大臣实力值
            long prePower = _heroInfo.power;

            //重新计算实力
            long newPower = HeroCommon.calPower(_heroInfo);

            //判断数据是否有变动，如无变动则不做后续处理
            if (newPower == prePower)
                return;

            //设置大臣新数据
            _heroInfo.updatePower(newPower);

            //更新数值到伙伴组件
            NPPlayer.instance.heroComponent.replaceHeroPower(prePower, newPower);

            //通知建筑部分重新计算收益
            NPPlayer.instance.buildingComp.recalAllBusinessBuilding(_isDealNextFrame);
        }

        /// <summary>
        /// 排序伙伴，实力从大到小 > 品质从高到低 > id从小到大
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        public static int sortHeroByPower(_IHeroCardShow _a, _IHeroCardShow _b)
        {
            if (_b == null)
                return -1;
            if (_a == null)
                return 1;
            if (ReferenceEquals(_a, _b))
                return 0;
            
            long powerA = _a.power;
            long powerB = _b.power;
            if (powerA.CompareTo(powerB) != 0)
                return -powerA.CompareTo(powerB);

            EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
            if (qualityA.CompareTo(qualityB) != 0)
                return -qualityA.CompareTo(qualityB);

            return _a.id.CompareTo(_b.id);
        }
    }
}