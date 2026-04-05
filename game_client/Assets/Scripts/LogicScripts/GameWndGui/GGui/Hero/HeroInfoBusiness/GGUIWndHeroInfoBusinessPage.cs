using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴信息经营页签
    /// </summary>
    public class GGUIWndHeroInfoBusinessPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroInfoBusinessPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //技能图标
        private NPGGuiWndTexture _m_wIcon;
        //升级消耗
        private NPGGUIWndCommonItem _m_wCostItem;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;
        //显示序列号
        private long _m_lShowSerialize;
        //点击关闭按钮
        private Action _m_aOnClickClose;

        public GGUIWndHeroInfoBusinessPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
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
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_BUSINESS_SKILL_CHG, _onBusinessSkillChg);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_HERO_BUSINESS_SKILL_UPGRADE, _onSimulateClickBusinessSkillUpgrade);
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_BUSINESS_SKILL_CHG, _onBusinessSkillChg);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_HERO_BUSINESS_SKILL_UPGRADE, _onSimulateClickBusinessSkillUpgrade);
            _m_wIcon?.hideWnd();
            _m_wCostItem?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }
        
        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wCostItem?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wIcon?.discard();
            _m_wIcon = null;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoUpgradeCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoUpgradeCostItem);

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
            _refreshSkill();
            _refreshAdditionSkill();
        }

        //刷新技能
        private void _refreshSkill()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            long skillId = _m_heroInfo.heroRefObj.business_skill_id;
            HeroBusinessSkillInfo businessSkillInfo = _m_heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(skillId);
            if (businessSkillInfo == null || businessSkillInfo.businessSkillRefObj == null)
                return;

            //技能图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(businessSkillInfo.businessSkillRefObj.icon);
            }

            //名称等级
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(businessSkillInfo.businessSkillRefObj.name, businessSkillInfo.businessSkillRefObj.name_args));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num,
                TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, 
                    businessSkillInfo.level,
                businessSkillInfo.businessSkillRefObj.business_skill_lvl_max)));

            //描述
            PlayerBonusPropertyContainer bonusPropertyContainer = new PlayerBonusPropertyContainer();
            bonusPropertyContainer.addModifier(businessSkillInfo.businessSkillRefObj.bonus_prop_modifier);
            bonusPropertyContainer.addModifier(businessSkillInfo.businessSkillRefObj.bonus_prop_modifier_per_level, businessSkillInfo.level - 1);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(businessSkillInfo.businessSkillRefObj.desc, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, bonusPropertyContainer.getValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER)/100f)));

            //下级加成
            bonusPropertyContainer.addModifier(businessSkillInfo.businessSkillRefObj.bonus_prop_modifier_per_level);
            ALUGUICommon.setLabelTxt(wnd.txtNextLevelAdd, TextTranslate.instance.getLanguage(TransKeyConst.hero_nextLevelAdd_num, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, bonusPropertyContainer.getValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f)));

            //升级消耗
            if (_m_wCostItem != null)
            {
                HeroBusinessSkillUpgradeRefObj skillUpgradeRef = GRefdataCoreMgr.instance.getHeroBusinessSkillUpgradeRef(businessSkillInfo.businessSkillRefObj.upgrade_cost_group, businessSkillInfo.level);
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(skillUpgradeRef?.upgrade_cost_item);
            }

            //满级显隐
            bool isMaxLevel = businessSkillInfo.level == businessSkillInfo.businessSkillRefObj.business_skill_lvl_max;
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, isMaxLevel);
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, !isMaxLevel);

            //刷新升级红点
            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip,NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_BUSINESS_UPGRADE, _m_heroInfo.id));
        }

        //刷新额外技能
        private void _refreshAdditionSkill()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            List<WCGPairInt> addSkillList = _m_heroInfo.heroRefObj.extra_business_skill_id_list;
            if (addSkillList == null)
                return;
            addSkillList.Sort((_a, _b) => _a.first().CompareTo(_b.first()));

            if (wnd.monoAdditionItemList != null)
            {
                for (int i = 0; i < addSkillList.Count; i++)
                {
                    //ui是否有配置
                    if (wnd.monoAdditionItemList.Count <= i)
                        break;

                    WCGPairInt tempSkillPair = addSkillList[i];
                    if (tempSkillPair == null)
                        continue;

                    ALUGUICommon.setGameObjEnable(wnd.monoAdditionItemList[i]?.goItem, true);

                    //是已解锁
                    bool isUnlock = _m_heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(tempSkillPair.second()) != null;
                    ALUGUICommon.setGameObjEnable(wnd.monoAdditionItemList[i]?.goUnlockShowList, isUnlock);
                    ALUGUICommon.setGameObjEnable(wnd.monoAdditionItemList[i]?.goUnlockHideList, !isUnlock);
                    //描述
                    HeroBusinessSkillRefObj skillRef = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(tempSkillPair.second());
                    if (skillRef != null)
                        ALUGUICommon.setLabelTxt(wnd.monoAdditionItemList[i]?.txtDesc, TextTranslate.instance.getLanguage(skillRef.desc, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, skillRef.bonus_prop_modifier?.getPropValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f)));
                    //解锁描述
                    if (!isUnlock)
                        ALUGUICommon.setLabelTxt(wnd.monoAdditionItemList[i]?.txtUnlockDesc,
                            TextTranslate.instance.getLanguage(TransKeyConst.hero_businessSkillUnlockCondDesc_name_level,
                                _m_heroInfo.heroRefObj.transName, tempSkillPair.first()));
                }
                //隐藏多余的item
                long addCount = wnd.monoAdditionItemList.Count - addSkillList.Count;
                if (addCount > 0)
                {
                    for (int j = wnd.monoAdditionItemList.Count - 1; j >= wnd.monoAdditionItemList.Count - addCount; j--)
                    {
                        ALUGUICommon.setGameObjEnable(wnd.monoAdditionItemList[j]?.goItem, false);
                    }
                }
            }
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

            long skillId = _m_heroInfo.heroRefObj.business_skill_id;
            HeroBusinessSkillInfo businessSkillInfo = _m_heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(skillId);
            if (businessSkillInfo == null || businessSkillInfo.businessSkillRefObj == null || !businessSkillInfo.businessSkillRefObj.can_upgrade)
                return;

            HeroBusinessSkillUpgradeRefObj skillUpgradeRef = GRefdataCoreMgr.instance.getHeroBusinessSkillUpgradeRef(businessSkillInfo.businessSkillRefObj.upgrade_cost_group, businessSkillInfo.level);
            if (skillUpgradeRef == null || !GCommon.isItemEnough(skillUpgradeRef.upgrade_cost_item, true))
                return;

            long serialize = _m_lShowSerialize;
            NPPlayer.instance.heroComponent.reqHeroBusinessSkillUpgrade(_m_heroInfo.id, businessSkillInfo.businessSkillId,
                (_isSuc) =>
                {
                    if (wnd == null || !isShow || serialize != _m_lShowSerialize)
                        return;

                    if (!_isSuc)
                        return;

                    //播放特效
                    if (_m_lSfxObjList == null)
                        _m_lSfxObjList = new List<CommonUISfxObj>();

                    if (wnd.upgradeSfxId > 0 && wnd.upgradeSfxParent != null)
                    {
                        CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxId, wnd.upgradeSfxParent);
                        _m_lSfxObjList.Add(sfxObj);
                    }
                });
        }

        #endregion


        #region 消息事件

        //经营技能变更
        private void _onBusinessSkillChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long heroId = (long)_objects[0];
            long businessSkillId = (long) _objects[1];
            if(_m_heroInfo != null && _m_heroInfo.id == heroId && _m_heroInfo.heroRefObj != null && _m_heroInfo.heroRefObj.business_skill_id == businessSkillId)
                _refreshSkill();
        }

        //模拟点击经营技能升级
        private void _onSimulateClickBusinessSkillUpgrade(params object[] _objects)
        {
            _onClickUpgrade(null);
        }

        #endregion
    }
}