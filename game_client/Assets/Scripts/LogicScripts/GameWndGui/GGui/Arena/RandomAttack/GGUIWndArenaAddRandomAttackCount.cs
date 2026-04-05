using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场增加谈判次数弹窗
    /// </summary>
    public class GGUIWndArenaAddRandomAttackCount : _ANPGGUIBasicWnd<GGUIMonoArenaAddRandomAttackCount>
    {
        private static GGUIWndArenaAddRandomAttackCount _g_instance;
        public static GGUIWndArenaAddRandomAttackCount instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaAddRandomAttackCount();
                return _g_instance;
            }
        }

        //竞技场信息
        private ArenaInfo _m_arenaInfo;
        //当前选择的数量
        private int _m_lCurSelectCount;
        //最大可购买的数量
        private long _m_lLeftCanBuyMaxCount;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;

        public GGUIWndArenaAddRandomAttackCount() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaAddRandomAttackCount.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaAddRandomAttackCount.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            if(wnd != null && wnd.selectCountSlider != null)
                wnd.selectCountSlider.onValueChanged?.AddListener(_onSliderValueChg);

            _setInfo();
        }

        protected override void _onHideWnd()
        {
            if (wnd != null && wnd.selectCountSlider != null)
                wnd.selectCountSlider.onValueChanged?.RemoveListener(_onSliderValueChg);

            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnBuy, _onClickBuy);
            ALUGUICommon.uncombineBtnClick(wnd.btnMax, _onClickMax);
            ALUGUICommon.uncombineBtnClick(wnd.btnMini, _onClickMini);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnIncrease, _onClickIncrease);
            ALUGUICommon.uncombineBtnClick(wnd.btnDecrease, _onClickDecrease);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnBuy, _onClickBuy);
            ALUGUICommon.combineBtnClick(wnd.btnMax, _onClickMax);
            ALUGUICommon.combineBtnClick(wnd.btnMini, _onClickMini);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnIncrease, _onClickIncrease);
            ALUGUICommon.combineBtnClick(wnd.btnDecrease, _onClickDecrease);
        }

        //设置信息
        private void _setInfo()
        {
            _m_arenaInfo = NPPlayer.instance.arenaComp.arenaInfo;
            if (_m_arenaInfo == null)
                return;

            _m_lLeftCanBuyMaxCount = _m_arenaInfo.getLeftCanBuyRandomAttackCount();
            //设置默认选择数量
            if (_m_lLeftCanBuyMaxCount == 0)
                _m_lCurSelectCount = 0;
            else
                _m_lCurSelectCount = 1;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_arenaInfo == null)
                return;

            //设置slider
            if (wnd.selectCountSlider != null)
            {
                if(_m_lLeftCanBuyMaxCount == 0)
                    wnd.selectCountSlider.value = 0;
                else
                    wnd.selectCountSlider.value = _m_lCurSelectCount * 1.0f / _m_lLeftCanBuyMaxCount;
            }

            //当前选择数量
            ALUGUICommon.setLabelTxt(wnd.txtSelectCount, TextTranslate.instance.getLanguage(TransKeyConst.arena_restoreSelectCount_num_num, _m_lCurSelectCount, _m_lLeftCanBuyMaxCount));

            //设置最大最小按钮置灰
            GGameCommonInfo.grayImage(wnd.maxGrayImgList, _m_lCurSelectCount >= _m_lLeftCanBuyMaxCount);
            GGameCommonInfo.grayImage(wnd.minGrayImgList, _m_lCurSelectCount <= 1);

            //今日已购买次数 
            long todayAlreadyBuyCount = _m_arenaInfo.hadBuyRandomAttackNum;
            long todayMaxBuyCount = _m_arenaInfo.getCanBuyMaxRandomAttackCount();
            ALUGUICommon.setLabelTxt(wnd.txtAlreadyBuyCount, TextTranslate.instance.getLanguage(TransKeyConst.arena_restoreAlreadyBuyCount_num_num, todayAlreadyBuyCount, todayMaxBuyCount));

            //刷新消耗
            _refreshCosItem();
        }

        //刷新消耗
        private void _refreshCosItem()
        {
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(_getBuyCostItem());
            }
        }

        //获取购买消耗
        private NPCommonCostItem _getBuyCostItem()
        {
            if (wnd == null || _m_arenaInfo == null)
                return null;

            //今日已购买次数
            long todayAlreadyBuyCount = _m_arenaInfo.hadBuyRandomAttackNum;
            long timePriceTypeId = GRefdataCoreMgr.instance.npGeneral.arena_random_attack_crystal_buy_time_price_id;
            List<NPCommonCostItem> costItemList = new List<NPCommonCostItem>();
            //默认购买1次
            long selectCount = _m_lCurSelectCount != 0 ? _m_lCurSelectCount : 1;
            for (int i = 1; i <= selectCount; i++)
            {
                TimesPriceRefObj costItemPrice = GRefdataCoreMgr.instance.getTimesPriceRefObj(timePriceTypeId, todayAlreadyBuyCount + i);
                if (costItemPrice == null)
                    continue;
                costItemList.Add(new NPCommonCostItem(costItemPrice.item, costItemPrice.cost_item_formula.CalculateVariableResult(null)));
            }

            if (costItemList.Count == 0)
                return null;

            costItemList = GCommon.getCombineItemList(costItemList);
            return costItemList[0];
        }

        //slider值变化
        private void _onSliderValueChg(float _value)
        {
            if (wnd == null || wnd.selectCountSlider == null)
                return;

            //取整
            int curCount = (int)Math.Round(_m_lLeftCanBuyMaxCount * _value);
            if (curCount < 1)
                curCount = 1;

            //如果没有可购买次数，选择数量为0
            if (_m_lLeftCanBuyMaxCount == 0)
                curCount = 0;

            _m_lCurSelectCount = curCount;
            _refreshWnd();
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_ADD_RANDOM_COUNT);
        }

        //点击增加
        private void _onClickIncrease(GameObject _go)
        {
            if (_m_lCurSelectCount >= _m_lLeftCanBuyMaxCount)
                return;

            _m_lCurSelectCount++;
            _refreshWnd();
        }

        //点击减少
        private void _onClickDecrease(GameObject _go)
        {
            if (_m_lCurSelectCount <= 1)
                return;

            _m_lCurSelectCount--;
            _refreshWnd();
        }

        //点击最大值
        private void _onClickMax(GameObject _go)
        {
            if (_m_lCurSelectCount >= _m_lLeftCanBuyMaxCount)
                return;

            _m_lCurSelectCount = (int)_m_lLeftCanBuyMaxCount;
            _refreshWnd();
        }

        //点击最小值
        private void _onClickMini(GameObject _go)
        {
            if (_m_lCurSelectCount <= 1)
                return;

            _m_lCurSelectCount = 1;
            _refreshWnd();
        }

        //点击购买
        private void _onClickBuy(GameObject _go)
        {
            if (_m_lLeftCanBuyMaxCount == 0)
            {
                //今日购买次数已达上限
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_buyRandomAttackCountReachLimit_none);
                return;
            }

            //道具是否充足
            if (!GCommon.isItemEnough(_getBuyCostItem(), true))
                return;

            NPPlayer.instance.arenaComp.reqArenaBuyRandomAttack(_m_lCurSelectCount, (_isSuc) =>
            {
                if(_isSuc)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_buyRandomAttackCountSuc_none);//购买谈判次数成功
            });
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_ADD_RANDOM_COUNT);
        }

        #endregion
    }
}