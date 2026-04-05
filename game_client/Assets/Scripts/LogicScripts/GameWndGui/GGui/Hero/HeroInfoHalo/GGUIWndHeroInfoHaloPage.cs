using ALPackage;
using Common.HeroObj;
using CommonEnum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴信息光环页签
    /// </summary>
    public class GGUIWndHeroInfoHaloPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroInfoHaloPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_lHeroInfo;
        //点击关闭按钮
        private Action _m_aOnClickClose;
        //升级完成回调
        private Action _m_aOnReqUpgradeDone;
        //当前选中的等级数据
        private HeroHaloLevelRefObj _m_curSelectHaloLevelRef;
        //等级列表
        private GGUIWndHeroHaloLevelContainer _m_wHaloLevelContainer;
        //套系技能列表
        private GGUIWndHeroHaloSuitSkillContainer _m_wSuitSkillContainer;
        //升级消耗
        private NPGGUIWndCommonItem _m_wCostItem;
        //显示序列
        private long _m_lShowSerialzie;

        public GGUIWndHeroInfoHaloPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
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
            // WinMsg.RegisterMsg(WinMsgType.ON_HERO_HALO_CHG, _onHeroHaloChg);
            _m_lShowSerialzie = ALSerializeOpMgr.next();
        }
        
        protected override void _onHideWnd()
        {
            // WinMsg.UnregisterMsg(WinMsgType.ON_HERO_HALO_CHG, _onHeroHaloChg);
            _m_lShowSerialzie = ALSerializeOpMgr.next();
            _m_wHaloLevelContainer?.hideWnd();
            _m_wSuitSkillContainer?.hideWnd();
            _m_wCostItem?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wHaloLevelContainer?.resetWnd();
            _m_wSuitSkillContainer?.resetWnd();
            _m_wCostItem?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wHaloLevelContainer?.discard();
            _m_wHaloLevelContainer = null;

            _m_wSuitSkillContainer?.discard();
            _m_wSuitSkillContainer = null;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
            ALUGUICommon.uncombineBtnClick(wnd.btnActivate, _onClickUnlock);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHaloLevelContainer != null)
            {
                _m_wHaloLevelContainer = new GGUIWndHeroHaloLevelContainer(wnd.monoHaloLevelContainer);
                _m_wHaloLevelContainer.onItemIndexChanged += _onClickLevelItem;
            }

            if (wnd.monoSuitSkillContainer != null)
                _m_wSuitSkillContainer = new GGUIWndHeroHaloSuitSkillContainer(wnd.monoSuitSkillContainer);

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
            ALUGUICommon.combineBtnClick(wnd.btnActivate, _onClickUnlock);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_onClickClose"></param>
        /// <param name="_onReqUpgradeDone"></param>
        public void setInfo(HeroInfo _heroInfo, Action _onClickClose, Action _onReqUpgradeDone)
        {
            _m_lHeroInfo = _heroInfo;
            _m_aOnClickClose = _onClickClose;
            _m_aOnReqUpgradeDone = _onReqUpgradeDone;

            _refreshLevelContainer();
        }

        //刷新等级列表
        private void _refreshLevelContainer()
        {
            if (_m_lHeroInfo == null || !_m_lHeroInfo.haveHalo)
                return;

            List<HeroHaloLevelRefObj> refList = GRefdataCoreMgr.instance.getHeroHaloLevelRefList(_m_lHeroInfo.heroRefObj.halo_id);
            if (_m_wHaloLevelContainer != null)
            {
                _m_wHaloLevelContainer.showWnd();
                _m_wHaloLevelContainer.setInfo(refList, _m_lHeroInfo.id);
            }

            //是否已激活，未激活不可拖动列表
            if (_m_wHaloLevelContainer != null && _m_wHaloLevelContainer.wnd != null && _m_wHaloLevelContainer.wnd.scrollRect != null)
            {
                bool isUnlock = _m_lHeroInfo.heroHaloInfo != null && _m_lHeroInfo.heroHaloInfo.isUnlock;
                _m_wHaloLevelContainer.wnd.scrollRect.enabled = isUnlock;
            }
        }

        //刷新等级相关技能加成详情
        private void _refreshLevelDetailInfo()
        {
            if (wnd == null || _m_curSelectHaloLevelRef == null)
                return;

            //筛选需要展示的列表，只有等级变化才需要展示
            HeroHaloLevelRefObj lastLevelRef = GRefdataCoreMgr.instance.getHeroHaloLevelRef(_m_curSelectHaloLevelRef.halo_id, _m_curSelectHaloLevelRef.level - 1);
            List<WCGPairInt> targetList = new List<WCGPairInt>();
            List<WCGPairInt> lastSkillLevelList = lastLevelRef != null ? lastLevelRef.halo_suit_skill_level_list : null;
            List<WCGPairInt> curSkillLevelList = _m_curSelectHaloLevelRef != null ? _m_curSelectHaloLevelRef.halo_suit_skill_level_list : null;
            if (lastSkillLevelList == null && curSkillLevelList != null)
            {
                for (int i = 0; i < curSkillLevelList.Count; i++)
                {
                    //剔除0级
                    if (curSkillLevelList[i].second() > 0)
                        targetList.Add(curSkillLevelList[i]);
                }
            }
            else if(curSkillLevelList != null)
            {
                for (int i = 0; i < curSkillLevelList.Count; i++)
                {
                    bool isFind = false;
                    for (int j = 0; j < lastSkillLevelList.Count; j++)
                    {
                        if (curSkillLevelList[i].first() == lastSkillLevelList[j].first())
                        {
                            isFind = true;
                            //等级不一样，添加到展示列表
                            if(curSkillLevelList[i].second() != lastSkillLevelList[j].second())
                                targetList.Add(curSkillLevelList[i]);
                        }
                    }

                    //上一级没有,直接添加
                    if(!isFind)
                        targetList.Add(curSkillLevelList[i]);
                }
            }

            //设置套系技能列表
            if (_m_wSuitSkillContainer != null)
            {
                if (targetList.Count > 0)
                {
                    _m_wSuitSkillContainer.showWnd();
                    _m_wSuitSkillContainer.showItemList(targetList);
                }
                else
                    _m_wSuitSkillContainer.hideWnd();
            }
            ALUGUICommon.setGameObjEnable(wnd.goNotHaveSuitSkillHideList, targetList.Count > 0);
            ALUGUICommon.setGameObjEnable(wnd.goNotHaveSuitSkillShowList, targetList.Count <= 0);

            //当前等级
            ALUGUICommon.setLabelTxt(wnd.txtCurLevel, TextTranslate.instance.getLanguage(TransKeyConst.hero_upgradeHaloLevel_num, _m_lHeroInfo.heroHaloInfo?.level));
            //实力变化
            ALUGUICommon.setLabelTxt(wnd.txtLastPower, lastLevelRef != null ? lastLevelRef.self_attr_prop_modifier?.getPropValue(EBasicAttrType.POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT):"0");
            ALUGUICommon.setLabelTxt(wnd.txtCurPower, _m_curSelectHaloLevelRef.self_attr_prop_modifier?.getPropValue(EBasicAttrType.POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtLastPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, lastLevelRef != null ? lastLevelRef.self_attr_prop_modifier?.getPropValue(EBasicAttrType.POWER_PER) / 100f : 0));
            ALUGUICommon.setLabelTxt(wnd.txtCurPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_curSelectHaloLevelRef.self_attr_prop_modifier?.getPropValue(EBasicAttrType.POWER_PER) / 100f));
        }

        //刷新升级按钮状态
        private void _refreshButtonState()
        {
            if (wnd == null || _m_curSelectHaloLevelRef == null || _m_lHeroInfo == null || _m_lHeroInfo.heroHaloInfo == null)
                return;

            //升级消耗
            HeroHaloLevelRefObj lastLevelRef = GRefdataCoreMgr.instance.getHeroHaloLevelRef(_m_curSelectHaloLevelRef.halo_id, _m_curSelectHaloLevelRef.level - 1);
            HeroHaloLevelRefObj nextLevelRef = GRefdataCoreMgr.instance.getHeroHaloLevelRef(_m_curSelectHaloLevelRef.halo_id, _m_curSelectHaloLevelRef.level + 1);
            if (_m_wCostItem != null && lastLevelRef != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(lastLevelRef.upgrade_cost);
            }

            //是否激活
            ALUGUICommon.setGameObjEnable(wnd.goNotActivateShowList, !_m_lHeroInfo.heroHaloInfo.isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goNotActivateHideList, _m_lHeroInfo.heroHaloInfo.isUnlock);

            //升级按钮状态
            if (_m_lHeroInfo.heroHaloInfo.level >= _m_curSelectHaloLevelRef.level)
            {
                ALUGUICommon.setGameObjEnable(wnd.goAlreadyUpgradeShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.goAlreadyUpgradeHideList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goAlreadyUpgradeShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goAlreadyUpgradeHideList, true);
                if (_m_lHeroInfo.heroHaloInfo.level + 1 == _m_curSelectHaloLevelRef.level)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeHideList, true);
                    ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeShowList, false);
                    GGameCommonInfo.disgrayImage(wnd.canNotUpgradeGrayList);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeShowList, true);
                    GGameCommonInfo.grayImage(wnd.canNotUpgradeGrayList);
                }
            }

            //是否满级
            bool isMaxLevel = nextLevelRef == null && _m_lHeroInfo.heroHaloInfo.isUnlock;
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, !isMaxLevel);
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, isMaxLevel);

            //刷新按钮红点
            _refreshBtnRedTip();
        }

        //刷新按钮红点
        private void _refreshBtnRedTip()
        {
            if (wnd == null || _m_curSelectHaloLevelRef == null || _m_lHeroInfo == null || _m_lHeroInfo.heroHaloInfo == null)
                return;

            //是否激活
            ALUGUICommon.setGameObjEnable(wnd.goActivateRedTip, !_m_lHeroInfo.heroHaloInfo.isUnlock && _m_curSelectHaloLevelRef.level == 0);

            //是否可升级
            bool canUpgrade = false;
            if (_m_lHeroInfo.heroHaloInfo.isUnlock && 
                _m_lHeroInfo.heroHaloInfo.nextHaloLevelRef != null && 
                GCommon.isItemEnough(_m_lHeroInfo.heroHaloInfo.curHaloLevelRef.upgrade_cost, false) &&
                _m_curSelectHaloLevelRef.level == _m_lHeroInfo.heroHaloInfo.nextHaloLevelRef.level)
                canUpgrade = true;
            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip, canUpgrade);
        }

        //激活升级的表现
        private void _dealActiveOrUpgradeShow()
        {
            if (_m_curSelectHaloLevelRef == null)
                return;

            long curLevel = _m_curSelectHaloLevelRef.level;

            //直接刷新下一级界面
            HeroHaloLevelRefObj nextHaloLevelRef = GRefdataCoreMgr.instance.getHeroHaloLevelRef(_m_curSelectHaloLevelRef.halo_id, _m_curSelectHaloLevelRef.level + 1);
            if (nextHaloLevelRef != null)
                _m_curSelectHaloLevelRef = nextHaloLevelRef;
            _refreshLevelDetailInfo();
            _refreshButtonState();

            //播放升级动画
            wnd?.aniUpgrade?.forcePlay();
            _m_wSuitSkillContainer?.playUpgradeAni();
            _m_wHaloLevelContainer?.playUpgradeAni(curLevel);

            //移动等级列表
            if (nextHaloLevelRef != null)
            {
                if (_m_wHaloLevelContainer != null && _m_wHaloLevelContainer.wnd != null && _m_wHaloLevelContainer.wnd.scrollRect != null)
                {
                    //先屏蔽拖动
                    _m_wHaloLevelContainer.wnd.scrollRect.enabled = false;
                    long curSerialize = _m_lShowSerialzie;
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        if (curSerialize != _m_lShowSerialzie)
                            return;

                        _m_wHaloLevelContainer.wnd.scrollRect.enabled = true;
                    },_m_wHaloLevelContainer.wnd.fixPosDuration);
                }

                _m_wHaloLevelContainer?.fixToItemIndex(_m_wHaloLevelContainer.currentItemIndex + 1, true);
            }
            else
            {
                //刷新一次当前选中item
                _m_wHaloLevelContainer?.refreshCurItem();
            }
        }

        //伙伴光环变化
        private void _onHeroHaloChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_lHeroInfo == null)
                return;

            long heroId = (long) _objects[0];
            if (heroId == _m_lHeroInfo.id)
                _refreshLevelContainer();
        }

        #region 点击事件

        //点击等级item
        private void _onClickLevelItem()
        {
            if (_m_wHaloLevelContainer == null || 
                _m_wHaloLevelContainer.curSelectedItem == null || 
                _m_wHaloLevelContainer.curSelectedItem.haloLevelRef  == null || 
                (_m_curSelectHaloLevelRef != null && _m_curSelectHaloLevelRef.level == _m_wHaloLevelContainer.curSelectedItem.haloLevelRef.level))
                return;

            _m_curSelectHaloLevelRef = _m_wHaloLevelContainer.curSelectedItem.haloLevelRef;
            _refreshLevelDetailInfo();
            _refreshButtonState();
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            _m_aOnClickClose?.Invoke();
        }

        //点击激活
        private void _onClickUnlock(GameObject _go)
        {
            if (_m_lHeroInfo == null || !_m_lHeroInfo.haveHalo || _m_lHeroInfo.heroHaloInfo.isUnlock)
                return;

            NPPlayer.instance.heroComponent.reqHeroHaloUnlock(_m_lHeroInfo.id, () =>
            {
                _dealActiveOrUpgradeShow();
            });
        }

        //点击升级
        private void _onClickUpgrade(GameObject _go)
        {
            if (_m_lHeroInfo == null || !_m_lHeroInfo.haveHalo || _m_curSelectHaloLevelRef == null || _m_lHeroInfo.heroHaloInfo == null)
                return;

            if (_m_curSelectHaloLevelRef.level != _m_lHeroInfo.heroHaloInfo.level + 1)
            {
                //请先激活前置技能
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.hero_needActivePreHaloLevelTip_none);
                return;
            }

            if (!GCommon.isItemEnough(_m_lHeroInfo.heroHaloInfo.curHaloLevelRef.upgrade_cost, true))
                return;

            NPPlayer.instance.heroComponent.reqHeroHaloUpgrade(_m_lHeroInfo.id, (_isSuc)=>
            {
                if (!_isSuc)
                    return;

                _m_aOnReqUpgradeDone?.Invoke();
                _dealActiveOrUpgradeShow();
            });
        }

        //点击效果总览按钮
        private void _onClickDetail(GameObject _go)
        {
            if (_m_lHeroInfo == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroHaloOwnInfo.instance, () =>
            {
                GGUIWndHeroHaloOwnInfo.instance.showWnd();
                GGUIWndHeroHaloOwnInfo.instance.setInfo(_m_lHeroInfo);
            }, UINodeTagConst.C_HERO_HALO_OWN_INFO);
        }

        #endregion
    }
}