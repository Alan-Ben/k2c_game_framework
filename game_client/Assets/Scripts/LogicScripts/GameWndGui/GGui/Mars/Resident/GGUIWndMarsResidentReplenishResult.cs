using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 居民补充结果窗口
    /// </summary>
    public class GGUIWndMarsResidentReplenishResult : _ANPGGUIBasicWnd<GGUIMonoMarsResidentReplenishResult>
    {
        private static GGUIWndMarsResidentReplenishResult _g_instance;
        [NotNull] public static GGUIWndMarsResidentReplenishResult instance { get { return _g_instance ??= new GGUIWndMarsResidentReplenishResult(); } }

        private GS2GC.p040_MarsPeopleOp.GS2GC_040_002_RetPeopleConfirmImmigrant _m_replenishResult;
        
        private GGUIWndCommonRewardContainer _m_wRewardContainer;

        public GGUIWndMarsResidentReplenishResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsResidentReplenishResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsResidentReplenishResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoRewardContainer != null)
                _m_wRewardContainer = new GGUIWndCommonRewardContainer(wnd.monoRewardContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickClose);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickClose);
            }
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_RESULT_SURE, _onSimulateClickSure);
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_RESULT_SURE, _onSimulateClickSure);
            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();
        }

        /// <summary>
        /// 设置并刷新显示的补充人数
        /// </summary>
        public void setData(GS2GC.p040_MarsPeopleOp.GS2GC_040_002_RetPeopleConfirmImmigrant _msg)
        {
            _m_replenishResult = _msg;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_replenishResult == null)
                return;

            bool residentNumReachLimit = NPPlayer.instance.marsComp.totalPeopleNum >= NPPlayer.instance.marsComp.peopleNumLimit;
            ALUGUICommon.setGameObjEnable(wnd.onResidentNumReachLimitShowList, residentNumReachLimit);
            ALUGUICommon.setGameObjEnable(wnd.onResidentNumReachLimitHideList, !residentNumReachLimit);
            
            // 显示补充人数
            ALUGUICommon.setLabelTxt(wnd.txtReplenishNum, _m_replenishResult.getNum().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            
            // 显示奖励列表
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.setRewardList(NPCommonCostItem.switchList(_m_replenishResult.getItemList()));
            }
        }

        private void _onClickClose(GameObject _go)
        {
            // 关闭窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_RESIDENT_REPLENISH_RESULT);
        }

        private void _onSimulateClickSure()
        {
            if (wnd == null) return;
            _onClickClose(wnd.btnSure);
        }

        public static void addWndNode(Action<bool> _addNodeResult = null)
        {
            NPPlayer.instance.marsComp.peopleSubComponent.reqPeopleConfirmImmigrant((_isSucc, _msg) =>
            {
                if (!_isSucc || _msg == null)
                {
                    _addNodeResult?.Invoke(false);
                    return;
                }
                                
                // 打开结果窗口
                instance.setData(_msg);
                QueueMgr.instance.addNode_InGame_SingleWnd(instance, instance.showWnd, UINodeTagConst.C_MARS_RESIDENT_REPLENISH_RESULT);
                
                _addNodeResult?.Invoke(true);
            });
        }
    }
}
