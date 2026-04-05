using ALPackage;
using Common.HeroObj;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴信息详情页签
    /// </summary>
    public class GGUIWndHeroInfoDetailPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroInfoDetailPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //属性信息附加窗口
        private GGUIWndHeroPowerLevelInfo _m_wPowerLevelInfo;
        //连升十级勾选
        private NPGGUIWndCommonToggleEx _m_wTenLevelUpToggle;
        //升级消耗
        private NPGGUIWndCommonItem _m_wLevelUpCostItem;
        //皮肤按钮item
        private GGUIWndHeroSkinBtnItem _m_wSkinBtn;
        //藏品入口item
        private GGUIWndHeroEquipBtnItem _m_wEquipBtn;

        public GGUIWndHeroInfoDetailPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
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
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);//伙伴经验变化
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_LEVEL_CHG, _onHeroLevelChg);//伙伴等级变化
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_STEP_CHG, _onHeroStepChg);//伙伴阶段变化
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_HALO_CHG, _onHeroHaloChg);//伙伴光环变化
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_LEVEL_UPGRADE, _simulateClickHeroLevelUpgrade);//模拟点击升级
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_EQUIP_ENTRY_BTN, _simulateClickHeroEquipEntryBtn);//模拟点击伙伴藏品入口按钮
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_TEN_UPGRADE_TOGGLE, _simulateClickTenUpgradeToggle);//模拟点击伙伴十连升级开关
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipInfoChg);//藏品信息变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagChg);//背包变更

            _m_wTenLevelUpToggle?.showWnd();
            _m_wTenLevelUpToggle?.setSelected(AccountSettingMgr.instance.accountSetting.isHeroTenUpgrade, true, false);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);//伙伴经验变化
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_LEVEL_CHG, _onHeroLevelChg);//伙伴等级变化
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_STEP_CHG, _onHeroStepChg);//伙伴阶段变化
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_HALO_CHG, _onHeroHaloChg);//伙伴光环变化
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_LEVEL_UPGRADE, _simulateClickHeroLevelUpgrade);//模拟点击升级
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_EQUIP_ENTRY_BTN, _simulateClickHeroEquipEntryBtn);//模拟点击伙伴藏品入口按钮
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_TEN_UPGRADE_TOGGLE, _simulateClickTenUpgradeToggle);//模拟点击伙伴十连升级开关
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipInfoChg);//藏品信息变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagChg);//背包变更

            _m_wLevelUpCostItem?.hideWnd();
            _m_wPowerLevelInfo?.hideWnd();
            _m_wSkinBtn?.hideWnd();
            _m_wEquipBtn?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wLevelUpCostItem?.resetWnd();
            _m_wPowerLevelInfo?.resetWnd();
            _m_wSkinBtn?.resetWnd();
            _m_wEquipBtn?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wTenLevelUpToggle?.discard();
            _m_wTenLevelUpToggle = null;

            _m_wLevelUpCostItem?.discard();
            _m_wLevelUpCostItem = null;

            _m_wPowerLevelInfo?.discard();
            _m_wPowerLevelInfo = null;

            _m_wSkinBtn?.discard();
            _m_wSkinBtn = null;

            _m_wEquipBtn?.discard();
            _m_wEquipBtn = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnLvlUpgrade, _onClickLvlUpgrade);//点击升级按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnStepUpgrade, _onClickStepUpgrade);//点击升阶按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnBless, _onClickBless);//点击加护按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnSkin, _onClickSkin);//点击皮肤按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnEquip, _onClickEquip);//点击藏品按钮
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.toggleTenLevelUp != null)
            {
                _m_wTenLevelUpToggle = new NPGGUIWndCommonToggleEx(wnd.toggleTenLevelUp);
                _m_wTenLevelUpToggle.clickDelegate += _onClickToggle;
            }

            if (wnd.monoLevelUpCostItem != null)
                _m_wLevelUpCostItem = new NPGGUIWndCommonItem(wnd.monoLevelUpCostItem);

            if (wnd.monoPowerLevelInfo != null)
                _m_wPowerLevelInfo = new GGUIWndHeroPowerLevelInfo(wnd.monoPowerLevelInfo);

            if (wnd.monoSkinBtn != null)
                _m_wSkinBtn = new GGUIWndHeroSkinBtnItem(wnd.monoSkinBtn);

            if (wnd.monoEquipBtnItem != null)
                _m_wEquipBtn = new GGUIWndHeroEquipBtnItem(wnd.monoEquipBtnItem);

            ALUGUICommon.combineBtnClick(wnd.btnLvlUpgrade, _onClickLvlUpgrade);//点击升级按钮
            ALUGUICommon.combineBtnClick(wnd.btnStepUpgrade, _onClickStepUpgrade);//点击升阶按钮
            ALUGUICommon.combineBtnClick(wnd.btnBless, _onClickBless);//点击加护按钮
            ALUGUICommon.combineBtnClick(wnd.btnSkin, _onClickSkin);//点击皮肤按钮
            ALUGUICommon.combineBtnClick(wnd.btnEquip, _onClickEquip);//点击藏品按钮
        }

        /// <summary>
        /// 设置信息
        /// </summary>
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
            _refreshPowerInfo();
            _refreshButtonState();
            _refreshEquipInfo();
        }

        //刷新实力属性信息
        private void _refreshPowerInfo()
        {
            if (_m_heroInfo == null)
                return;

            if (_m_wPowerLevelInfo != null)
            {
                _m_wPowerLevelInfo.showWnd();
                _m_wPowerLevelInfo.setInfo(_m_heroInfo);
            }
        }

        //刷新按钮状态
        private void _refreshButtonState()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            //设置消耗的道具
            bool isTen = _m_wTenLevelUpToggle != null && _m_wTenLevelUpToggle.isOn;
            if (_m_heroInfo.nextHeroLvlRef != null)
            {
                NPCommonCostItem levelUpCostItem = _m_heroInfo.getLevelUpCostItem(isTen);
                if (_m_wLevelUpCostItem != null)
                {
                    _m_wLevelUpCostItem.showWnd();
                    _m_wLevelUpCostItem.setItem(levelUpCostItem);
                }
            }

            //显示升级或升阶按钮
            bool canStepUp = _m_heroInfo.isStepLvlLimit();
            ALUGUICommon.setGameObjEnable(wnd.goStepUpgradeShow, canStepUp);
            ALUGUICommon.setGameObjEnable(wnd.goStepUpgradeHide, !canStepUp);

            //是否满级
            ALUGUICommon.setGameObjEnable(wnd.goLvLimitShow, _m_heroInfo.nextHeroLvlRef == null);
            ALUGUICommon.setGameObjEnable(wnd.goLvLimitHide, _m_heroInfo.nextHeroLvlRef != null);

            //是否可以十连升级
            bool canTenLevelUp = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.hero_level_upgrade_ten_times_simple_unlock_id);
            ALUGUICommon.setGameObjEnable(wnd.goTenLevelUpShowList, canTenLevelUp);
            ALUGUICommon.setGameObjEnable(wnd.goTenLevelUpHideList, !canTenLevelUp);

            //皮肤按钮显示设置
            if (_m_wSkinBtn != null)
            {
                _m_wSkinBtn.showWnd();
                _m_wSkinBtn.setInfo(_m_heroInfo);
            }

            //是否有其他皮肤
            ALUGUICommon.setGameObjEnable(wnd.goNoSkinHideList, _m_heroInfo != null && 
                                                                _m_heroInfo.heroRefObj != null && 
                                                                _m_heroInfo.heroRefObj.heroSkinRefList != null && 
                                                                _m_heroInfo.heroRefObj.heroSkinRefList.Count > 1);

            //是否有加护
            ALUGUICommon.setGameObjEnable(wnd.btnBless, _m_heroInfo.heroRefObj != null && _m_heroInfo.heroRefObj.relationConsortIdList.Count > 0);

            //刷新红点提示
            _refreshRedTip();
        }

        //刷新藏品信息
        private void _refreshEquipInfo()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_m_heroInfo.id);
            _m_wEquipBtn?.showWnd();
            _m_wEquipBtn?.setInfo(equipInfo, _m_heroInfo);
        }

        //刷新红点提示
        private void _refreshRedTip()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip, NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_LEVEL_UP, _m_heroInfo.id));
            ALUGUICommon.setGameObjEnable(wnd.goStepUpRedTip, NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_STEP_UP, _m_heroInfo.id));
        }

        #region 点击事件

        //点击连升十级开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _commonToggleEx)
        {
            if (_m_wTenLevelUpToggle == null)
                return;

            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.hero_level_upgrade_ten_times_simple_unlock_id,true))
                return;

            bool isOn = !_m_wTenLevelUpToggle.isOn;
            _m_wTenLevelUpToggle.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting.setIsHeroTenUpgrade(isOn);
            _refreshButtonState();
        }

        //点击升阶
        private void _onClickStepUpgrade(GameObject _gameObject)
        {
            if (null == _m_heroInfo)
                return;

            //没有下一阶段说明满阶了
            if (null == _m_heroInfo.nextHeroStepRef)
                return;

            //打开升阶确认弹窗
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroStepUpgradeCheck.instance, () =>
            {
                GGUIWndHeroStepUpgradeCheck.instance.showWnd();
                GGUIWndHeroStepUpgradeCheck.instance.setInfo(_m_heroInfo);
            }, UINodeTagConst.C_HERO_STEP_UPGRADE);
        }

        //点击升级
        private void _onClickLvlUpgrade(GameObject _gameObject)
        {
            if (null == _m_heroInfo)
                return;

            //当前已经满级
            if (_m_heroInfo.isStepLvlLimit())
                return;

            bool isTen = _m_wTenLevelUpToggle != null && _m_wTenLevelUpToggle.isOn;

            //判断消耗，只要能升一级就可以升级
            if (GCommon.isItemEnough(ENPItemType.CURRENCY,(long)ECurrency.HERO_EXP, _m_heroInfo.curHeroLvlRef != null ? _m_heroInfo.curHeroLvlRef.need_exp : 0, true))
            {
                //请求升级
                NPPlayer.instance.heroComponent.reqHeroUpgrade(_m_heroInfo.id, isTen, () =>
                {
                    //如果还可以升级，尝试触发引导
                    if(isShow && _m_heroInfo != null && !_m_heroInfo.isStepLvlLimit() && _m_heroInfo.nextHeroLvlRef != null)
                        GCommon.triggerTutorial();
                });
            }
        }

        //点击加护按钮
        private void _onClickBless(GameObject _go)
        {
            GGUIWndHeroInfo.instance.setSelectBlessPage();
        }

        //点击皮肤按钮
        private void _onClickSkin(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndHeroSkinMain.instance,UINodeTagConst.C_HERO_SKIN,null, ()=>
            {
                GGUIWndHeroSkinMain.instance.setInfo(_m_heroInfo?.heroRefObj);
            });
        }

        //点击藏品按钮
        private void _onClickEquip(GameObject _go)
        {
            EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_m_heroInfo.id);
            if (equipInfo == null)
            {
                //进入替换界面
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroEquipChange.instance, () =>
                {
                    GGUIWndHeroEquipChange.instance.showWnd();
                    GGUIWndHeroEquipChange.instance.setInfo(null, _m_heroInfo);
                }, UINodeTagConst.C_HERO_EQUIP_CHANGE);
            }
            else
            {
                //进入藏品信息界面
                QueueMgr.instance.AddNode(new GNodeHeroEquipInfo(_m_heroInfo));
            }
        }

        //模拟点击升级
        private void _simulateClickHeroLevelUpgrade()
        {
            _onClickLvlUpgrade(null);
        }

        //模拟点击藏品按钮
        private void _simulateClickHeroEquipEntryBtn()
        {
            _onClickEquip(null);
        }

        //模拟点击连升十级开关
        private void _simulateClickTenUpgradeToggle()
        {
            _onClickToggle(_m_wTenLevelUpToggle);
        }

        #endregion


        #region 消息事件

        //伙伴经验变化
        private void _onCurrencyChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0)
                return;

            ECurrency type = (ECurrency)_objs[0];

            if (type == ECurrency.HERO_EXP)
                _refreshButtonState();
        }

        //伙伴等级变化
        private void _onHeroLevelChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0 || _m_heroInfo == null)
                return;

            HeroInfo heroInfo = (HeroInfo)_objs[0];

            if (heroInfo != null && _m_heroInfo.id == heroInfo.id)
            {
                _m_heroInfo = heroInfo;
                _refreshButtonState();
            }
        }

        //伙伴阶段变化
        private void _onHeroStepChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0 || _m_heroInfo == null)
                return;

            HeroInfo heroInfo = (HeroInfo)_objs[0];

            if (heroInfo != null && _m_heroInfo.id == heroInfo.id)
            {
                _m_heroInfo = heroInfo;
                _refreshWnd();

                //如果还可以升级，尝试触发引导
                if (isShow && _m_heroInfo != null && !_m_heroInfo.isStepLvlLimit() && _m_heroInfo.nextHeroLvlRef != null)
                    GCommon.triggerTutorial();
            }
        }

        //伙伴光环变化
        private void _onHeroHaloChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0 || _m_heroInfo == null)
                return;

            HeroInfo heroInfo = (HeroInfo)_objs[0];

            if (heroInfo != null && _m_heroInfo.id == heroInfo.id)
            {
                _m_heroInfo = heroInfo;
            }
        }

        //藏品信息变更
        private void _onEquipInfoChg(params object[] _objs)
        {
            _refreshEquipInfo();
        }

        //背包变更
        private void _onBagChg(params object[] _objects)
        {
            _refreshRedTip();
        }

        #endregion
    }

}