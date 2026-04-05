using System.Collections.Generic;
using ALPackage;
using GC2GS.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnDishUnlock : _ATALBasicUIWnd<GGUIMonoInnDishUnlock>
    {
        [NotNull] public static GGUIWndInnDishUnlock instance { get { return _g_instance ??= new GGUIWndInnDishUnlock(); } }
        private static GGUIWndInnDishUnlock _g_instance;
        

        // 菜品信息
        private InnDishInfo _m_dishInfo;
        private List<InnDishInfo> _m_dishList;
        private InnDishInfo _m_prevDishInfo;
        private InnDishInfo _m_nextDishInfo;
        // 解锁花费
        private NPGGUIWndCommonItem _m_costItemWnd;
        // 菜品图标
        private NPGGuiWndTexture _m_dishIconWnd;

        public GGUIWndInnDishUnlock()
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoInnDishUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnDishUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_costItemWnd?.showWnd();
            _m_dishIconWnd?.showWnd();
            refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_DISH_UNLOCK_BUTTON, _onSimulateClickUnlockButton);
        }
        protected override void _onHideWnd()
        {
            _m_costItemWnd?.hideWnd();
            _m_dishIconWnd?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_DISH_UNLOCK_BUTTON, _onSimulateClickUnlockButton);
        }
        protected override void _onReset()
        {
            _m_costItemWnd?.resetWnd();
            _m_dishIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_dishInfo = null;

            _m_costItemWnd?.discard();
            _m_costItemWnd = null;
            _m_dishIconWnd?.discard();
            _m_dishIconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onBtnGotoClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUnlock, _onBtnUnlockClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextDish, _onBtnNextDishClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevDish, _onBtnPrevDishClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCostItem != null)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            if (wnd.imgDishIcon != null)
                _m_dishIconWnd = new NPGGuiWndTexture(wnd.imgDishIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onBtnGotoClick);
            ALUGUICommon.combineBtnClick(wnd.btnUnlock, _onBtnUnlockClick);
            ALUGUICommon.combineBtnClick(wnd.btnNextDish, _onBtnNextDishClicked);
            ALUGUICommon.combineBtnClick(wnd.btnPrevDish, _onBtnPrevDishClicked);
        }
        
        /// <summary>
        /// 设置菜品信息并刷新窗口
        /// </summary>
        /// <param name="_dishInfo">菜品信息</param>
        /// <param name="_dishList">菜品列表</param>
        public void refreshWnd(InnDishInfo _dishInfo, List<InnDishInfo> _dishList = null)
        {
            _m_dishInfo = _dishInfo;
            _m_dishList = _dishList;
            _m_prevDishInfo = null;
            _m_nextDishInfo = null;
            if (_m_dishList is { Count: > 1 })
            {
                int curIndex = _m_dishList.IndexOf(_m_dishInfo);
                if (curIndex >= 0)
                {
                    int nextIndex = NPGameUtility.intRepeat(curIndex + 1, _m_dishList.Count);
                    int prevIndex = NPGameUtility.intRepeat(curIndex - 1, _m_dishList.Count);

                    _m_nextDishInfo = _m_dishList[nextIndex];
                    if (_m_nextDishInfo is { isUnlock: true })
                        _m_nextDishInfo = null;
                    _m_prevDishInfo = _m_dishList[prevIndex];
                    if (_m_prevDishInfo is { isUnlock: true })
                        _m_prevDishInfo = null;
                }
            }
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_dishInfo == null)
                return;

            // 设置菜品图标
            _m_dishIconWnd?.setTexture(_m_dishInfo.refObj.icon);
            // 设置菜品名字
            ALUGUICommon.setLabelTxt(wnd.txtDishName, _m_dishInfo.nameTranslated);
            // 设置菜品编号
            ALUGUICommon.setLabelTxt(wnd.txtDishNum, TextTranslate.instance.getLanguage(TransKeyConst.inn_dishNum_num, _m_dishInfo.num));
            // 设置菜品描述
            ALUGUICommon.setLabelTxt(wnd.txtDishDesc, _m_dishInfo.descTranslated);
            // 设置解锁提示
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, _m_dishInfo.unlockTipTranslated);

            // 设置解锁花费
            if (_m_costItemWnd != null && _m_dishInfo.refObj.unlock_cost != null && _m_dishInfo.refObj.unlock_cost.IsValid)
            {
                _m_costItemWnd.setItem(_m_dishInfo.refObj.unlock_cost);
            }

            bool requirementMet = _m_dishInfo.checkUnlockRequirementMet();
            bool canUnlock = _m_dishInfo.hasRecipe && requirementMet;
            bool conditionMetNoRecipe = !_m_dishInfo.hasRecipe && requirementMet;
            wnd.setCanUnlock(canUnlock, conditionMetNoRecipe);
            wnd.setHasNextDish(_m_nextDishInfo != null);
            wnd.setHasPrevDish(_m_prevDishInfo != null);
        }
        
        
        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_DISH_UNLOCK);
        }
        /// <summary>
        /// 前往按钮点击事件
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onBtnGotoClick(GameObject _go)
        {
            if (_m_dishInfo == null)
                return;

            if (_m_dishInfo.refObj.unlock_go_to == null)
                return;
            
            _m_dishInfo.refObj.unlock_go_to.dealEffect();
            // 关闭当前窗口
            _onBtnCloseClick(null);
        }
        /// <summary>
        /// 解锁按钮点击事件
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onBtnUnlockClick(GameObject _go)
        {
            if (_m_dishInfo == null)
                return;

            if (!GCommon.isItemEnough(_m_dishInfo.refObj.unlock_cost, true))
                return;

            InnDishInfo dishInfo = _m_dishInfo;
            NPGSClientListener.sendRequestByLog(new GC2GS_034_004_ReqInnDishUnlock(_m_dishInfo.dishId), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    _onBtnCloseClick(null);
                    GGUIWndInnDishUnlockSuccess.instance.refreshWnd(dishInfo);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnDishUnlockSuccess.instance, GGUIWndInnDishUnlockSuccess.instance.showWnd, UINodeTagConst.C_INN_DISH_UNLOCK_SUCCESS);
                }));
        }
        /// <summary>
        /// 模拟点击解锁按钮
        /// </summary>
        private void _onSimulateClickUnlockButton()
        {
            if (wnd == null) return;
            _onBtnUnlockClick(wnd.btnUnlock);
        }
        /// <summary>
        /// 下一个菜品按钮点击事件
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onBtnNextDishClicked(GameObject _go)
        {
            refreshWnd(_m_nextDishInfo, _m_dishList);
        }
        /// <summary>
        /// 上一个菜品按钮点击事件
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onBtnPrevDishClicked(GameObject _go)
        {
            refreshWnd(_m_prevDishInfo, _m_dishList);
        }
    }
}