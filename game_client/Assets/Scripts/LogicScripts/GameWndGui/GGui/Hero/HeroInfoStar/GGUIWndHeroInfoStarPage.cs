using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴信息觉醒页签
    /// </summary>
    public class GGUIWndHeroInfoStarPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroInfoStarPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //升级消耗
        private NPGGUIWndCommonItem _m_wCostItem;
        //点击关闭按钮
        private Action _m_aOnClickClose;
        //星级附加窗口
        private GGUIWndHeroCommonStar _m_wStar;
        //觉醒技能列表
        private GGUIWndHeroStarSkillContainer _m_wStarSkillContainer;
        //资质技能列表
        private GGUIWndHeroTalentSkillContainer _m_wTalentSkillContainer;

        public GGUIWndHeroInfoStarPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_STAR_CHG, _onHeroStarChg);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_TALENT_SKILL_CHG, _onTalentSkillChg);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_ADD_TALENT_SKILL, _onTalentSkillAdd);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_STAR_CHG, _onHeroStarChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_TALENT_SKILL_CHG, _onTalentSkillChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_ADD_TALENT_SKILL, _onTalentSkillAdd);
            _m_wCostItem?.hideWnd();
            _m_wStar?.hideWnd();
            _m_wStarSkillContainer?.hideWnd();
            _m_wTalentSkillContainer?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
            _m_wStar?.resetWnd();
            _m_wStarSkillContainer?.resetWnd();
            _m_wTalentSkillContainer?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            _m_wStar?.discard();
            _m_wStar = null;

            _m_wStarSkillContainer?.discard();
            _m_wStarSkillContainer = null;

            _m_wTalentSkillContainer?.discard();
            _m_wTalentSkillContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoUpgradeCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoUpgradeCostItem);

            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);

            if (wnd.monoStarSkillContainer != null)
                _m_wStarSkillContainer = new GGUIWndHeroStarSkillContainer(wnd.monoStarSkillContainer);

            if (wnd.monoTalentSkillContainer != null)
                _m_wTalentSkillContainer = new GGUIWndHeroTalentSkillContainer(wnd.monoTalentSkillContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroInfo _heroInfo, Action _onClickClose)
        {
            if(null == _heroInfo)
                return;

            _m_heroInfo = _heroInfo;
            _m_aOnClickClose = _onClickClose;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshStarInfo();
            _refreshStarSkillList();
            _refreshTalentSkillList();
            _refreshUpgradeRedTip();
        }

        //刷新星级信息
        private void _refreshStarInfo()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            //火星实力加成百分比显示
            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            long marsPowerAddPer = starRef != null && starRef.mars_team_player_property != null ? starRef.mars_team_player_property.getPropertyValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER) : 0;
            ALUGUICommon.setLabelTxt(wnd.txtMarsTeamPowerAddPer, TextTranslate.instance.getLanguage(TransKeyConst.heroMars_teamPowerAdd_str, marsPowerAddPer/100));

            //默认是否可以升级星级
            bool defaultCanUpgradeStar = _m_heroInfo.heroRefObj.can_star_upgrade;
            
            if (defaultCanUpgradeStar)
            {
                //是可升星的伙伴
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeShowList,false);
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeHideList,true);

                if (_m_wStar != null)
                {
                    _m_wStar.showWnd();
                    _m_wStar.setInfo(_m_heroInfo.star);
                }

                bool canStarUpgrade = starRef != null && 
                                      (starRef.upgrade_player_condition == null || starRef.upgrade_player_condition.isEmpty || starRef.upgrade_player_condition.IsEnable(null)) && 
                                       (starRef.upgrade_condition == null || starRef.upgrade_condition.isEmpty || starRef.upgrade_condition.IsEnable(_m_heroInfo.heroRefObj, null));

                List<string> curDescArgs = new List<string>();
                if(starRef != null && starRef.desc_args != null)
                    curDescArgs.AddRange(starRef.desc_args);
                //如果升星条件不满足，需要设置参数为红色
                if (!canStarUpgrade)
                {
                    for (int i = 0; i < curDescArgs.Count; i++)
                    {
                        curDescArgs[i] = GCommon.addColorForRichText(curDescArgs[i], wnd.conditionNotPassDescColor);
                    }
                }
                //升星条件描述
                ALUGUICommon.setLabelTxt(wnd.txtUpgradeConditionDesc, TextTranslate.instance.getLanguage(starRef.upgrade_condition_desc, curDescArgs));

                //显示加成
                long addValue = 0;
                long addPer = 0;
                if (starRef.self_attr_prop_modifier != null)
                {
                    addValue = starRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER);
                    addPer = starRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER_PER);
                }
                ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.hero_powerAddValue_num, addValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                ALUGUICommon.setLabelTxt(wnd.txtAddValuePer, TextTranslate.instance.getLanguage(TransKeyConst.hero_powerAddValuePer_num, addPer/100));

                //升星消耗
                if (_m_wCostItem != null)
                {
                    _m_wCostItem.showWnd();
                    _m_wCostItem.setItem(starRef.upgrade_cost);
                }

                //按钮颜色设置
                if(wnd.monoButtonColorState != null)
                    wnd.monoButtonColorState.setState(canStarUpgrade);
                if(wnd.monoButtonTextColorState != null)
                    wnd.monoButtonTextColorState.setState(canStarUpgrade);

                //满星的显隐设置
                HeroStarRefObj nextStarRefObj = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star + 1);
                ALUGUICommon.setGameObjEnable(wnd.goMaxStarHideList, nextStarRefObj != null);
                ALUGUICommon.setGameObjEnable(wnd.goMaxStarShowList, nextStarRefObj == null);
            }
            else
            {
                //是不可升星的伙伴
                _m_wStar?.hideWnd();
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeHideList, false);
            }
        }

        //刷新觉醒技能
        private void _refreshStarSkillList()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            if (_m_wStarSkillContainer != null)
            {
                _m_wStarSkillContainer.showWnd();
                _m_wStarSkillContainer.showItemList(_m_heroInfo, _m_heroInfo.heroRefObj.star_skill_id_list);
            }
        }

        //刷新资质技能
        private void _refreshTalentSkillList()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            //获取跟随星级解锁的资质技能
            List<long> talentSkillIdList = new List<long>();
            if (_m_heroInfo.heroRefObj.extra_talent_skill_id_list != null)
            {
                for (int i = 0; i < _m_heroInfo.heroRefObj.extra_talent_skill_id_list.Count; i++)
                {
                    if(_m_heroInfo.heroRefObj.extra_talent_skill_id_list[i] != null)
                        talentSkillIdList.Add(_m_heroInfo.heroRefObj.extra_talent_skill_id_list[i].second());
                }
            }

            if (_m_wTalentSkillContainer != null)
            {
                if (talentSkillIdList.Count > 0)
                {
                    _m_wTalentSkillContainer.showWnd();
                    _m_wTalentSkillContainer.showItemList(_m_heroInfo.id, talentSkillIdList);
                }
                else
                {
                    _m_wTalentSkillContainer.hideWnd();
                }
            }
        }

        //刷新升级小红点
        private void _refreshUpgradeRedTip()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goUpgradeReTip, NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_STAR_UPGRADE, _m_heroInfo.id));
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            _m_aOnClickClose?.Invoke();
        }

        //点击升级
        private void _onClickUpgrade(GameObject _go)
        {
            if (_m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            if (starRef == null)
                return;

            //条件不通过不能升星
            if ((starRef.upgrade_player_condition != null && !starRef.upgrade_player_condition.IsEnable(null)) ||
                (starRef.upgrade_condition != null && !starRef.upgrade_condition.IsEnable(_m_heroInfo.heroRefObj, null)))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(starRef.upgrade_condition_desc, starRef.desc_args));
                return;
            }
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroStarUpgradeCheck.instance, () =>
            {
                GGUIWndHeroStarUpgradeCheck.instance.showWnd();
                GGUIWndHeroStarUpgradeCheck.instance.setInfo(_m_heroInfo);
            }, UINodeTagConst.C_HERO_STAR_UPGRADE_CHECK);
        }

        #endregion


        #region 消息事件

        //觉醒技能变更
        private void _onHeroStarChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3)
                return;

            long heroId = (long) _objects[0];
            long oriStar = (long) _objects[1];
            long curStar = (long) _objects[2];
            if (_m_heroInfo != null && _m_heroInfo.id == heroId)
            {
                _refreshWnd();

                if (oriStar != curStar)
                {
                    //播放星星显示动画
                    _m_wStar?.playStarAniByStar(curStar);
                    //播放技能升级特效
                    _m_wStarSkillContainer?.playUpgradeSfxWithAllItem();
                }
            }
        }

        //资质技能变更
        private void _onTalentSkillChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2 || _m_heroInfo == null)
                return;

            long heroId = (long)_objects[0];
            long talentSkillId = (long)_objects[1];
            if (heroId == _m_heroInfo.id)
            {
                _m_wTalentSkillContainer?.refreshAllTalenItemShow();
            }
        }

        //新增资质技能
        private void _onTalentSkillAdd(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2 || _m_heroInfo == null)
                return;

            long heroId = (long)_objects[0];
            long talentSkillId = (long)_objects[1];
            if (heroId == _m_heroInfo.id)
            {
                //播放解锁动画
                _m_wTalentSkillContainer?.playUnlockAniByTalentSkillId(talentSkillId);
            }
        }

        #endregion
    }
}