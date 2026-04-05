using ALPackage;
using CommonEnum;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒升星确认界面
    /// </summary>
    public class GGUIWndHeroStarUpgradeCheck : _ANPGGUIBasicWnd<GGUIMonoHeroStarUpgradeCheck>
    {
        private static GGUIWndHeroStarUpgradeCheck _g_instance;
        public static GGUIWndHeroStarUpgradeCheck instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroStarUpgradeCheck();
                return _g_instance;
            }
        }

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //资质技能图标
        private NPGGuiWndTexture _m_wTalentSkillIcon;
        //当前星级
        private GGUIWndHeroCommonStar _m_wCurStar;
        //下个星级
        private GGUIWndHeroCommonStar _m_wNextStar;
        //觉醒技能列表
        private GGUIWndHeroStarSkillContainer _m_wStarSkillContainer;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;

        public GGUIWndHeroStarUpgradeCheck() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroStarUpgradeCheck.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroStarUpgradeCheck.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if(_m_wTalentSkillIcon != null)
                _m_wTalentSkillIcon.hideWnd();

            if(_m_wCurStar != null)
                _m_wCurStar.hideWnd();

            if(_m_wNextStar != null)
                _m_wNextStar.hideWnd();

            if(_m_wStarSkillContainer != null)
                _m_wStarSkillContainer.hideWnd();

            if(_m_wCostItem != null)
                _m_wCostItem.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wTalentSkillIcon != null)
                _m_wTalentSkillIcon.discardTexture();

            if (_m_wCurStar != null)
                _m_wCurStar.resetWnd();

            if (_m_wNextStar != null)
                _m_wNextStar.resetWnd();

            if (_m_wStarSkillContainer != null)
                _m_wStarSkillContainer.resetWnd();

            if (_m_wCostItem != null)
                _m_wCostItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wTalentSkillIcon != null)
                _m_wTalentSkillIcon.discard();
            _m_wTalentSkillIcon = null;

            if (_m_wCurStar != null)
                _m_wCurStar.discard();
            _m_wCurStar = null;

            if (_m_wNextStar != null)
                _m_wNextStar.resetWnd();
            _m_wNextStar = null;

            if (_m_wStarSkillContainer != null)
                _m_wStarSkillContainer.resetWnd();
            _m_wStarSkillContainer = null;

            if (_m_wCostItem != null)
                _m_wCostItem.resetWnd();
            _m_wCostItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickStepUpgrade);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.monoCurStar != null)
                _m_wCurStar = new GGUIWndHeroCommonStar(wnd.monoCurStar);

            if (wnd.monoNextStar != null)
                _m_wNextStar = new GGUIWndHeroCommonStar(wnd.monoNextStar);

            if (wnd.monoStarSkillContainer != null)
                _m_wStarSkillContainer = new GGUIWndHeroStarSkillContainer(wnd.monoStarSkillContainer);

            if (wnd.imgTalentSkillIcon != null)
                _m_wTalentSkillIcon = new NPGGuiWndTexture(wnd.imgTalentSkillIcon);

            if (wnd.monoUpgradeCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoUpgradeCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickStepUpgrade);
        }
        
        //设置信息
        public void setInfo(HeroInfo _heroInfo)
        {
            if(null == _heroInfo)
                return;

            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshValueChg();
            _refreshStarSkillContainer();
            _refreshTalentSkill();
            _refreshCost();
        }

        //刷新值变化
        private void _refreshValueChg()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            long curStar = _m_heroInfo.star;
            long nextStar = _m_heroInfo.star + 1;
            HeroStarRefObj curStarRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, curStar);
            HeroStarRefObj nextStarRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, nextStar);

            if (_m_wCurStar != null)
            {
                _m_wCurStar.showWnd();
                _m_wCurStar.setInfo(curStar);
            }

            if (_m_wNextStar != null)
            {
                _m_wNextStar.showWnd();
                _m_wNextStar.setInfo(nextStar);
            }

            long curValue = 0;
            long curPer = 0;
            long curMarsPowerAddPer = 0;
            long nextValue = 0;
            long nextPer = 0;
            long nextMarsPowerAddPer = 0;
            if (curStarRef != null && curStarRef.self_attr_prop_modifier != null)
            {
                curValue = curStarRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER);
                curPer = curStarRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER_PER);
            }

            if (curStarRef != null && curStarRef.mars_team_player_property != null)
            {
                curMarsPowerAddPer = curStarRef.mars_team_player_property.getPropertyValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
            }

            if (nextStarRef != null && nextStarRef.self_attr_prop_modifier != null)
            {
                nextValue = nextStarRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER);
                nextPer = nextStarRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER_PER);
            }

            if( nextStarRef != null && nextStarRef.mars_team_player_property != null)
            {
                nextMarsPowerAddPer = nextStarRef.mars_team_player_property.getPropertyValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
            }

            ALUGUICommon.setLabelTxt(wnd.txtCurBaseValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, curValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtNextBaseValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, nextValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtCurPerValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, curPer/100f));
            ALUGUICommon.setLabelTxt(wnd.txtNextPerValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, nextPer/100f));
            ALUGUICommon.setLabelTxt(wnd.txtCurMarsTeamPowerAddPerValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, curMarsPowerAddPer / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtNextMarsTeamPowerAddPerValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, nextMarsPowerAddPer / 100f));
        }

        //刷新觉醒技能
        private void _refreshStarSkillContainer()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null || _m_heroInfo.heroRefObj.star_skill_id_list == null)
                return;

            //觉醒技能列表
            List<long> starSkillIdList = new List<long>();
            for (int i = 0; i < _m_heroInfo.heroRefObj.star_skill_id_list.Count; i++)
            {
                long starSkillId = _m_heroInfo.heroRefObj.star_skill_id_list[i];
                HeroStarSkillInfo starSkillInfo = _m_heroInfo.starSkillInfoMgr.getStarSkillInfo(starSkillId);
                HeroStarSkillRefObj starSkillRef = GRefdataCoreMgr.instance.heroStarSkillRefCore.getRef(starSkillId);
                if(starSkillInfo == null || (starSkillRef != null && starSkillInfo.level < starSkillRef.maxLevel))
                    starSkillIdList.Add(starSkillId);
            }

            if (_m_wStarSkillContainer != null)
            {
                _m_wStarSkillContainer.showWnd();
                _m_wStarSkillContainer.showItemList(_m_heroInfo, starSkillIdList, true);
            }
        }

        //刷新资质技能
        private void _refreshTalentSkill()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null || _m_heroInfo.heroRefObj.extra_talent_skill_id_list == null)
                return;

            long talentSkillId = 0;
            long nextStar = _m_heroInfo.star + 1;
            for (int i = 0; i < _m_heroInfo.heroRefObj.extra_talent_skill_id_list.Count; i++)
            {
                if (_m_heroInfo.heroRefObj.extra_talent_skill_id_list[i].first() == nextStar)
                    talentSkillId = _m_heroInfo.heroRefObj.extra_talent_skill_id_list[i].second();
            }

            HeroTalentSkillRefObj talentSkillRef = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(talentSkillId);
            if (talentSkillRef == null)
            {
                Debug.LogError($"未获取到下一星级解锁的资质配置，id:{talentSkillId}");
                return;
            }

            HeroTalentSkillLevelRefObj talentSkillLevelRef = talentSkillRef.getBaseTalentSkillLevelRefByLevel(1);

            //设置图标
            if (_m_wTalentSkillIcon != null)
            {
                _m_wTalentSkillIcon.showWnd();
                _m_wTalentSkillIcon.setTexture(talentSkillRef.icon);
            }

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtTalentSkillLevelName, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentSkillLevelName_level_name, 1, talentSkillRef.name));

            //设置描述
            long talentValue = talentSkillLevelRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
            ALUGUICommon.setLabelTxt(wnd.txtTalentSkillDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, talentValue));
        }

        //刷新消耗
        private void _refreshCost()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            if (starRef == null)
                return;

            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(starRef.upgrade_cost);
            }
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _gameObject)
        {
            if(null == _m_heroInfo)
                return;
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_STAR_UPGRADE_CHECK);
        }
        
        //点击升星
        private void _onClickStepUpgrade(GameObject _gameObject)
        {
            if (_m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            if (starRef == null)
                return;

            //道具不足不能升星
            if (!GCommon.isItemEnough(starRef.upgrade_cost, true))
                return;

            NPPlayer.instance.heroComponent.reqHeroStarUpgrade(_m_heroInfo.id);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_STAR_UPGRADE_CHECK);
        }

        #endregion
    }
}