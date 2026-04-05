using System.Collections.Generic;
using System;
using ALPackage;
using CommonEnum;
using GC2GS.p013_HeroOp;
using GS2GC.p013_HeroOp;
using JetBrains.Annotations;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 伙伴管理器
    /// </summary>
    public partial class PlayerHeroComponent : _ANPBasicPlayerComponent
    {
        //伙伴列表
        [NotNull] private List<HeroInfo> _m_lHeroInfoList = new List<HeroInfo>();
        //伙伴套系列表
        [NotNull] private Dictionary<long, HeroSuitInfo> _m_lHeroSuitInfoDic = new Dictionary<long, HeroSuitInfo>();
        //伙伴皮肤属性加成管理器
        [NotNull] private CommonUnionBonusMgr _m_heroSkinUnionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.HERO_SKIN);
        //伙伴属性加成管理器
        [NotNull] private CommonUnionBonusMgr _m_commonUnionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.HERO);
        //玩家属性容器
        [NotNull] private NPPlayerPropertyContainer _m_playerPropertyContainer = new NPPlayerPropertyContainer("hero");
        //红点处理器
        [NotNull] private RedTipDealer _m_redTipDealer;
        //总实力
        private long _m_lTotalPower;
        //伙伴列表展示筛选方式
        private ESpecAttrType _m_eHeroListFilterType;

        //构造函数
        public PlayerHeroComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_redTipDealer = new RedTipDealer(this);
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.CONSORT, ENPPlayerCompType.EQIUP };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.HERO; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 伙伴列表展示筛选方式
        /// </summary>
        public ESpecAttrType heroListFilterType { get { return _m_eHeroListFilterType; } set { _m_eHeroListFilterType = value; } }
        /// <summary>
        /// 伙伴属性加成管理器
        /// </summary>
        public CommonUnionBonusMgr commonUnionBonusMgr { get { return _m_commonUnionBonusMgr; } }
        /// <summary>
        /// 伙伴皮肤属性加成管理器
        /// </summary>
        public CommonUnionBonusMgr heroSkinUnionBonusMgr { get { return _m_heroSkinUnionBonusMgr; } }
        /// <summary>
        /// 玩家属性容器
        /// </summary>
        public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_playerPropertyContainer; } }
        /// <summary>
        /// 总实力
        /// </summary>
        public long totalPower { get { return _m_lTotalPower; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqHeroInit();
        }

        protected override void _dealInit()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            reCalcAllHero(false);
            //红点初始化
            _m_redTipDealer.init();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_commonUnionBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_commonUnionBonusMgr.onPropertyChg += _onBonusChg;

            _m_heroSkinUnionBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_heroSkinUnionBonusMgr.onPropertyChg += _onBonusChg;

            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_RELATION_HERO_ATTR_PROPERTY_CHG, _onConsortHaloInfoChg);//妃子关联英雄属性加成变化
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_RELATE_HERO_POWER_RECALCULATE, _onEquipChg);//妃子关联英雄属性加成变化
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);//背包变更
            WinMsg.RegisterMsgAct(WinMsgType.ON_EQUIP_ADD, _onEquipChg);//藏品新增
            WinMsg.RegisterMsgAct(WinMsgType.ON_EQUIP_REMOVE, _onEquipChg);//藏品移除
            WinMsg.RegisterMsgAct(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipChg);//藏品信息变更
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerHeroComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_RELATION_HERO_ATTR_PROPERTY_CHG, _onConsortHaloInfoChg);//妃子关联英雄属性加成变化
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_RELATE_HERO_POWER_RECALCULATE, _onEquipChg);//妃子关联英雄属性加成变化
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);//背包变更
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EQUIP_ADD, _onEquipChg);//藏品新增
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EQUIP_REMOVE, _onEquipChg);//藏品移除
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipChg);//藏品信息变更
        }

        //析构函数
        private void _clear()
        {
            _m_redTipDealer.clear();
            _m_lHeroInfoList.Clear();
            _m_lHeroSuitInfoDic.Clear();
            _m_commonUnionBonusMgr.clear();
            _m_commonUnionBonusMgr.onPropertyChg -= _onBonusChg;
            _m_heroSkinUnionBonusMgr.clear();
            _m_heroSkinUnionBonusMgr.onPropertyChg -= _onBonusChg;
        }

        /// <summary>
        /// 获取所有伙伴列表
        /// </summary>
        /// <param name="_recList"></param>
        public void getAllList(List<HeroInfo> _recList)
        {
            if (null == _recList)
                return;

            _recList.AddRange(_m_lHeroInfoList);
        }

        /// <summary>
        /// 处理所有伙伴数据
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllHero(Action<HeroInfo> _action)
        {
            if (_action == null || _m_lHeroInfoList == null)
                return;

            foreach (HeroInfo heroInfo in _m_lHeroInfoList)
            {
                if(heroInfo == null)
                    continue;

                _action(heroInfo);
            }
        }

        /// <summary>
        /// 获取伙伴数据
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public HeroInfo getHeroInfo(long _heroId)
        {
            for (int i = 0; i < _m_lHeroInfoList.Count; ++i)
            {
                if (null != _m_lHeroInfoList[i] && _m_lHeroInfoList[i].id == _heroId)
                    return _m_lHeroInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取伙伴套系数据
        /// </summary>
        /// <param name="_heroHaloSuitId"></param>
        /// <returns></returns>
        public HeroSuitInfo getHeroSuitInfo(long _heroHaloSuitId)
        {
            if (_m_lHeroSuitInfoDic.TryGetValue(_heroHaloSuitId, out HeroSuitInfo _info))
                return _info;
            return null;
        }

        /// <summary>
        /// 获得总的伙伴数量
        /// </summary>
        /// <returns></returns>
        public int getTotalHeroCount()
        {
            return _m_lHeroInfoList.Count;
        }

        /// <summary>
        /// 获取总伙伴等级
        /// </summary>
        /// <returns></returns>
        public long getTotalHeroLevel()
        {
            long totalLevel = 0;
            for (int i = 0; i < _m_lHeroInfoList.Count; i++)
            {
                totalLevel += _m_lHeroInfoList[i].level;
            }

            return totalLevel;
        }

        /// <summary>
        /// 获取拥有的卡牌数量
        /// </summary>
        /// <returns></returns>
        public int getOwnCount(Predicate<HeroInfo> _filter = null)
        {
            if (_filter == null)
                return _m_lHeroInfoList.Count;

            int res = 0;
            for (int i = 0; i < _m_lHeroInfoList.Count; ++i)
            {
                HeroInfo temp = _m_lHeroInfoList[i];
                if (temp == null)
                    continue;

                if (_filter.Invoke(temp))
                    res++;
            }

            return res;
        }

        /// <summary>
        /// 伙伴是否解锁
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public bool isHeroUnlock(long _heroId)
        {
            for (int i = 0; i < _m_lHeroInfoList.Count; i++)
            {
                if (_m_lHeroInfoList[i].id == _heroId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取大臣在建筑中的数量
        /// </summary>
        /// <returns></returns>
        public long getHeroInBuildingNum()
        {
            long num = 0;
            for (int i = 0; i < _m_lHeroInfoList.Count; i++)
            {
                if (_m_lHeroInfoList[i] != null && _m_lHeroInfoList[i].placeData.buildingId > 0)
                    num++;
            }
            return num;
        }

        /// <summary>
        /// 获取伙伴总资质
        /// </summary>
        /// <returns></returns>
        public long getTotalHeroTalent()
        {
            long totalTalent = 0;
            for (int i = 0; i < _m_lHeroInfoList.Count; i++)
            {
                if (_m_lHeroInfoList[i] != null)
                    totalTalent+= _m_lHeroInfoList[i].getTotalTalent();
            }
            return totalTalent;
        }

        /// <summary>
        /// 获取达到指定星级的伙伴数量
        /// </summary>
        /// <param name="_star"></param>
        /// <returns></returns>
        public long getFitStarHeroCount(long _star)
        {
            long count = 0;
            for (int i = 0; i < _m_lHeroInfoList.Count; i++)
            {
                if (_m_lHeroInfoList[i] != null && _m_lHeroInfoList[i].star >= _star)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 是否需要展示红点
        /// </summary>
        /// <param name="_redTipId"></param>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public bool needShowRedTip(long _redTipId, long _heroId)
        {
            return _m_redTipDealer.needShowRedTip(_redTipId, _heroId);
        }

        /// <summary>
        /// 设置红点已读
        /// </summary>
        /// <param name="_redTipId"></param>
        /// <param name="_heroId"></param>
        public void setReadRedTip(long _redTipId, long _heroId)
        {
            _m_redTipDealer.setReadRedTip(_redTipId, _heroId);
        }

        /// <summary>
        /// 对所有大臣进行重新计算处理
        /// </summary>
        public void reCalcAllHero(bool _isDealNextFrame = true)
        {
            foreach (HeroInfo heroInfo in _m_lHeroInfoList)
            {
                if (heroInfo != null) 
                    heroInfo.reCalcHero(_isDealNextFrame);
            }
        }

        /// <summary>
        /// 更新总实力
        /// </summary>
        /// <param name="_prePower"></param>
        /// <param name="_newPower"></param>
        public void replaceHeroPower(long _prePower, long _newPower)
        {
            _m_lTotalPower -= _prePower;
            _m_lTotalPower += _newPower;
            WinMsg.SendMsg(WinMsgType.ON_ALL_HERO_TOTAL_POWER_CHG);
        }

        /// <summary>
        /// 玩家全局属性加成变更
        /// </summary>
        /// <param name="_type"></param>
        private void _onBonusChg(EBonusPropertyType _type)
        {
            reCalcAllHero();
        }

        /// <summary>
        /// 家人关联伙伴加成数据变化
        /// </summary>
        /// <param name="_objects"></param>
        private void _onConsortHaloInfoChg(params object[] _objects)
        {
            reCalcAllHero();
        }

        /// <summary>
        /// 关联藏品变更
        /// </summary>
        private void _onEquipChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long heroId = (long) _objects[0];
            if (heroId > 0)
            {
                HeroInfo heroInfo = getHeroInfo(heroId);
                heroInfo?.reCalcHero();
            }
        }

        /// <summary>
        /// 背包变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onBagChg(params object[] _objects)
        {
            _m_redTipDealer.onBagItemChg();
        }

        /// <summary>
        /// 资源变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onCurrencyChg(params object[] _objects)
        {
            _m_redTipDealer.onCurrencyChg();
        }

        /// <summary>
        /// 藏品变更
        /// </summary>
        private void _onEquipChg()
        {
            _m_redTipDealer.onEquipChg();
        }

        #region S2C

        /// <summary>
        /// 初始化伙伴信息
        /// </summary>
        /// <param name="_msg"></param>
        public void retHeroInit(GS2GC_002_012_RetHeroInit _msg)
        {
            if (_msg == null)
                return;

            _m_lHeroInfoList.Clear();
            for (int i = 0; i < _msg.getHeroList().Count; i++)
            {
                HeroInfo heroInfo = new HeroInfo(_msg.getHeroList()[i]);
                _m_lHeroInfoList.Add(heroInfo);
            }

            _m_lHeroSuitInfoDic.Clear();
            for (int i = 0; i < _msg.getSuitList().Count; i++)
            {
                HeroSuitInfo heroSuitInfo = new HeroSuitInfo(_msg.getSuitList()[i]);
                _m_lHeroSuitInfoDic[heroSuitInfo.suitId] = heroSuitInfo;
            }

            setInitDone();
        }

        /// <summary>
        /// 伙伴升级
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroLevelChg(GS2GC_013_050_OnHeroLevelChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            long oriLevel = heroInfo.level;
            heroInfo.updateLvl(_msg.getLevel());

            //刷新红点
            _m_redTipDealer.refreshLevelUpRedTip(heroInfo);
            _m_redTipDealer.refreshStepUpRedTip(heroInfo);
            _m_redTipDealer.refreshStarUpgradeRedTip(heroInfo);

            WinMsg.SendMsg(WinMsgType.ON_HERO_LEVEL_CHG, heroInfo, oriLevel, heroInfo.level);
            //刷新十连解锁条件
            GCommon.reloadCustomLoadPrefab();
        }

        /// <summary>
        /// 伙伴资质技能变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroTalentSkillChg(GS2GC_013_052_OnHeroTalentSkillChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            HeroTalentSkillInfo talentSkillInfo = heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_msg.getTalentSkillInfo().getTealentSkillId());
            heroInfo.updateTalentSkillInfo(_msg.getTalentSkillInfo());

            WinMsg.SendMsg(WinMsgType.ON_HERO_TALENT_SKILL_CHG, _msg.getHeroId(), _msg.getTalentSkillInfo().getTealentSkillId());

            //是否是新增的资质技能
            if (talentSkillInfo == null)
                WinMsg.SendMsg(WinMsgType.ON_HERO_ADD_TALENT_SKILL, _msg.getHeroId(), _msg.getTalentSkillInfo().getTealentSkillId());
        }

        /// <summary>
        /// 伙伴经营技能变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroBusinessSkillChg(GS2GC_013_054_OnHeroBusinessSkillChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            //是否是新的技能
            bool isNew = heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(_msg.getBusinessSkillInfo().getBusinessSkillId()) == null;
            //更新数据
            heroInfo.heroBusinessSkillInfoMgr.updateBusinessSkillInfo(_msg.getBusinessSkillInfo());

            //刷新红点
            _m_redTipDealer.refreshBusinessSkillUpgradeRedTip(heroInfo);

            if(isNew)
                WinMsg.SendMsg(WinMsgType.ON_HERO_GET_NEW_BUSINESS_SKILL, _msg.getHeroId(), _msg.getBusinessSkillInfo().getBusinessSkillId());
            else
                WinMsg.SendMsg(WinMsgType.ON_HERO_BUSINESS_SKILL_CHG, _msg.getHeroId(), _msg.getBusinessSkillInfo().getBusinessSkillId());
        }

        /// <summary>
        /// 获得伙伴
        /// </summary>
        /// <param name="_msg"></param>
        public void onGainHero(GS2GC_013_055_OnGainHero _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = new HeroInfo(_msg.getHeroInfo());
            _m_lHeroInfoList.Add(heroInfo);

            //刷新红点
            _m_redTipDealer.onGainHero(heroInfo);

            WinMsg.SendMsg(WinMsgType.ON_HERO_GAIN, heroInfo);
        }

        /// <summary>
        /// 大臣觉醒星级变化推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroStarChg(GS2GC_013_056_OnHeroStarChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            long oriStar = heroInfo.star;
            heroInfo.updateStar(_msg.getStar());

            //刷新红点
            _m_redTipDealer.refreshStarUpgradeRedTip(heroInfo);

            WinMsg.SendMsg(WinMsgType.ON_HERO_STAR_CHG, _msg.getHeroId(), oriStar, heroInfo.star);
        }

        /// <summary>
        /// 伙伴皮肤变化推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroSkinChg(GS2GC_013_057_OnHeroSkinChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            //是否是新皮肤
            bool isNewSkin = heroInfo.heroSkinInfoMgr.getSkinInfo(_msg.getSkinInfo().getSkinId()) == null;

            heroInfo.updateSkin(_msg.getSkinInfo());

            WinMsg.SendMsg(WinMsgType.ON_HERO_SKIN_CHG, heroInfo);
        }

        /// <summary>
        /// 伙伴阶段变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroStepChg(GS2GC_013_058_OnHeroStepChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            long oriStepId = heroInfo.curStep;
            heroInfo.updateStep(_msg.getStep());

            //刷新红点
            _m_redTipDealer.refreshLevelUpRedTip(heroInfo);
            _m_redTipDealer.refreshStepUpRedTip(heroInfo);

            WinMsg.SendMsg(WinMsgType.ON_HERO_STEP_CHG, heroInfo, oriStepId);
        }

        /// <summary>
        /// 伙伴穿戴皮肤变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroWearSkinChg(GS2GC_013_059_OnHeroWearSkinChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            heroInfo.updateCurSkinId(_msg.getSkinId());

            WinMsg.SendMsg(WinMsgType.ON_HERO_CUR_SKIN_ID_CHG, heroInfo);
        }

        /// <summary>
        /// 大臣套系变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroSuitChg(GS2GC_013_060_OnHeroSuitChg _msg)
        {
            if (_msg == null || _msg.getSuitInfo() == null)
                return;

            HeroSuitInfo heroSuitInfo = getHeroSuitInfo(_msg.getSuitInfo().getSuitId());
            if (heroSuitInfo == null)
                return;

            heroSuitInfo.updateInfo(_msg.getSuitInfo());

            WinMsg.SendMsg(WinMsgType.ON_HERO_SUIT_CHG, _msg.getSuitInfo().getSuitId());
        }

        /// <summary>
        /// 伙伴光环变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroHaloChg(GS2GC_013_061_OnHeroHaloChg _msg)
        {
            if (_msg == null || _msg.getHaloInfo() == null)
                return;
            
            HeroInfo heroInfo = getHeroInfo(_msg.getHaloInfo().getHeroId());
            if (heroInfo == null)
                return;
            
            heroInfo.updateHaloInfo(_msg.getHaloInfo());

            _m_redTipDealer.refreshHaloUpgradeRedTip(heroInfo);
            
            WinMsg.SendMsg(WinMsgType.ON_HERO_HALO_CHG, _msg.getHaloInfo().getHeroId());
        }
        
        /// <summary>
        /// 大臣额外加成实力推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroItemAddPowerChg(GS2GC_013_063_OnHeroItemAddPowerChg _msg)
        {
            if (_msg == null)
                return;
            
            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
             if (heroInfo == null)
                 return;
            
             heroInfo.updateItemAddPower(_msg.getValue());
        }
        
        /// <summary>
        /// 伙伴驻扎的建筑变化
        /// </summary>
        public void onHeroPlaceBuildingChg(GS2GC_013_062_OnHeroPlaceBuildingChg _msg)
        {
            if (_msg == null)
                return;
            
            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;
            
            long lastId = heroInfo.placeData.buildingId;
            heroInfo.updatePlaceData(_msg.getPlaceInfo());
            long newId = heroInfo.placeData.buildingId;
            
            WinMsg.SendMsg(WinMsgType.ON_HERO_PLACE_BUILDING_CHG, _msg.getHeroId(), lastId, newId);
        }

        /// <summary>
        /// 伙伴竞技场额外加成实力推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroArenaAddPowerChg(GS2GC_013_064_OnHeroArenaAddPowerChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            heroInfo.updateArenaAddPower(_msg.getValue());
        }

        /// <summary>
        /// 伙伴游历额外加成实力推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroTravelAddPowerChg(GS2GC_013_069_OnHeroTravelAddPowerChg _msg)
        {
            if (_msg == null)
                return;

            HeroInfo heroInfo = getHeroInfo(_msg.getHeroId());
            if (heroInfo == null)
                return;

            heroInfo.updateTravelAddPower(_msg.getValue());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化伙伴列表
        /// </summary>
        public void reqHeroInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_012_ReqHeroInit());
        }

        /// <summary>
        /// 请求升级伙伴
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_isTen"></param>
        public void reqHeroUpgrade(long _heroId, bool _isTen, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_001_ReqHeroUpgrade(_heroId, _isTen),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_001_RetHeroUpgrade>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求穿戴皮肤
        /// </summary>
        /// <param name="_skinId"></param>
        public void reqHeroSetSkin(long _skinId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_002_ReqHeroSetSkin(_skinId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_002_RetHeroSetSkin>(null));
        }

        /// <summary>
        /// 请求提升伙伴阶段
        /// </summary>
        /// <param name="_heroId"></param>
        public void reqHeroStepUpgrade(long _heroId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_004_ReqHeroStepUpgrade(_heroId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_004_RetHeroStepUpgrade>(_msg =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求提升伙伴皮肤等级
        /// </summary>
        /// <param name="skinId"></param>
        public void reqHeroSkinUpgrade(long _skinId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_005_ReqHeroSkinUpgrade(_skinId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_005_RetHeroSkinUpgrade>(null));
        }

        /// <summary>
        /// 请求提升大臣觉醒星级
        /// </summary>
        public void reqHeroStarUpgrade(long _heroId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_006_ReqHeroStarUpgrade(_heroId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_006_RetHeroStarUpgrade>(null));
        }

        /// <summary>
        /// 请求升级大臣资质技能
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_talentSkillId"></param>
        public void reqHeroTalentSkillUpgrade(long _heroId, long _talentSkillId, bool _isTen, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_007_ReqHeroTalentSkillUpgrade(_heroId, _talentSkillId, _isTen),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_013_007_RetHeroTalentSkillUpgrade>((_isSuc, _msg)=>{_callback?.Invoke(_isSuc);}));
        }

        /// <summary>
        /// 请求升级大臣经营技能
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_businessSkillId"></param>
        public void reqHeroBusinessSkillUpgrade(long _heroId, long _businessSkillId, Action<bool> _onDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_008_ReqHeroBusinessSkillUpgrade(_heroId, _businessSkillId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_013_008_RetHeroBusinessSkillUpgrade>((_isSuc, _msg)=>
                {
                    _onDone?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求解锁皮肤
        /// </summary>
        /// <param name="_skinId"></param>
        public void reqHeroSkinUnlock(long _skinId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_009_ReqHeroSkinUnlock(_skinId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_009_RetHeroSkinUnlock>(null));
        }

        /// <summary>
        /// 分配伙伴到某个建筑上
        /// </summary>
        public void reqBuildingPlaceHero(long _heroId, long _buildingId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_010_ReqBuildingPlaceHero(_heroId, _buildingId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_013_010_RetBuildingPlaceHero>((_isSuc, _msg) => _callback?.Invoke()));
        }

        /// <summary>
        /// 把伙伴从建筑上移除
        /// </summary>
        public void reqBuildingRemoveHero(long _heroId, Action _callback)
        { 
            NPGSClientListener.sendRequestByLog(new GC2GS_013_011_ReqBuildingRemoveHero(_heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_013_011_RetBuildingRemoveHero>((_isSuc, _msg) => _callback?.Invoke()));
        }

        /// <summary>
        /// 请求解锁光环
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_callback"></param>
        public void reqHeroHaloUnlock(long _heroId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_015_ReqHeroHaloUnlock(_heroId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_015_RetHeroHaloUnlock>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求升级光环
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_callback"></param>
        public void reqHeroHaloUpgrade(long _heroId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_016_ReqHeroHaloUpgrade(_heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_013_016_RetHeroHaloUpgrade>((_isSuc, _msg) =>
                {
                    if (_callback != null)
                        _callback(_isSuc);
                }));
        }

        #endregion
    }
}
