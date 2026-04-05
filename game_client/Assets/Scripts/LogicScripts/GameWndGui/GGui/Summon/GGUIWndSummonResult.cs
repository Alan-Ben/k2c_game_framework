using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 召唤结果弹窗
    /// </summary>
    public class GGUIWndSummonResult : _ANPGGUIBasicWnd<GGUIMonoSummonResult>
    {
        private static GGUIWndSummonResult _g_instance;
        public static GGUIWndSummonResult instance { get { return _g_instance ??= new GGUIWndSummonResult(); } }

        private GachaPoolRefObj _m_rGachaPoolRefObj;
        private GS2GC.p007_CommOp.GS2GC_007_026_RetGachaRoll _m_ResultMsg; 
        
        private NPGGUIWndGetItemContainer _m_wCommonRewardContainer;
        private NPGGUIWndCommonItem _m_wDrawAgainCostItem;
        private GGUIWndCommonFixedCd _m_wFreeDrawFixedCd;//免费抽卡固定CD展示
        
        public GGUIWndSummonResult() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoSummonResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSummonResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
            {
                _m_wCommonRewardContainer = new NPGGUIWndGetItemContainer(wnd.monoRewardContainer);
                _m_wCommonRewardContainer.onAllItemShowed += _onAllRewardItemShowed;
            }

            if (wnd.monoDrawAgainCostItem != null)
                _m_wDrawAgainCostItem = new NPGGUIWndCommonItem(wnd.monoDrawAgainCostItem);

            // 构建免费抽卡固定CD展示
            if (wnd.freeDrawFixedCd != null)
                _m_wFreeDrawFixedCd = new GGUIWndCommonFixedCd(wnd.freeDrawFixedCd);
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnDrawAgain, _onDrawAgainBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnFreeDraw, _onFreeDrawBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnDrawAgain, _onDrawAgainBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnFreeDraw, _onFreeDrawBtnClick);
            }

            if (_m_wCommonRewardContainer != null)
            {
                _m_wCommonRewardContainer.onAllItemShowed -= _onAllRewardItemShowed;
                _m_wCommonRewardContainer?.discard();
                _m_wCommonRewardContainer = null;    
            }
            
            _m_wDrawAgainCostItem?.discard();
            _m_wDrawAgainCostItem = null;

            _m_wFreeDrawFixedCd?.discard();
            _m_wFreeDrawFixedCd = null;

            _m_rGachaPoolRefObj = null;
            _m_ResultMsg = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCommonRewardContainer?.hideWnd();
            _m_wDrawAgainCostItem?.hideWnd();
            _m_wFreeDrawFixedCd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonRewardContainer?.resetWnd();
            _m_wDrawAgainCostItem?.resetWnd();
            _m_wFreeDrawFixedCd?.resetWnd();
        }

        public void setData(GachaPoolRefObj _gachaPoolRefObj, GS2GC.p007_CommOp.GS2GC_007_026_RetGachaRoll _msg)
        {
            _m_rGachaPoolRefObj = _gachaPoolRefObj;
            _m_ResultMsg = _msg;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_rGachaPoolRefObj == null || _m_ResultMsg == null)
                return;

            bool isTenDraw = _m_ResultMsg.getIsTen();
            
            if (_m_wCommonRewardContainer != null)
            {
                List<NPCommon.NPCommon_ItemInfo> rewardItemList = _m_ResultMsg.getItemList();
                _m_wCommonRewardContainer.showWnd();
                if (rewardItemList != null && rewardItemList.Count > 0)
                {
                    _refreshAllRewardItemShowed(false);
                }
                else
                {
                    _refreshAllRewardItemShowed(true);
                }
                _m_wCommonRewardContainer.showItemList(rewardItemList);
            }
            
            ALUGUICommon.setGameObjEnable(wnd.oneDrawShowGoList, !isTenDraw);
            ALUGUICommon.setGameObjEnable(wnd.tenDrawShowGoList, isTenDraw);
            
            NPCommonCostItem drawCostItem = isTenDraw ? _m_rGachaPoolRefObj.ten_roll_cost : _m_rGachaPoolRefObj.roll_cost;
            if (_m_wDrawAgainCostItem != null)
            {
                _m_wDrawAgainCostItem.showWnd();
                _m_wDrawAgainCostItem.setItem(drawCostItem);
            }

            if (_m_wFreeDrawFixedCd != null)
            {
                if(_m_rGachaPoolRefObj != null && _m_rGachaPoolRefObj.fixed_cd_id > 0)
                {
                    _m_wFreeDrawFixedCd.showWnd();
                    _m_wFreeDrawFixedCd.setFixedCdId(_m_rGachaPoolRefObj.fixed_cd_id);
                }
                else
                {
                    _m_wFreeDrawFixedCd.hideWnd();
                }
            }
        }
        
        private void _refreshAllRewardItemShowed(bool _allItemShowed)
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.beforeAllItemShowShowGoList, !_allItemShowed);
            ALUGUICommon.setGameObjEnable(wnd.afterAllItemShowShowGoList, _allItemShowed);
        }
        
        /// <summary>
        /// 确认按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            _closeWnd();
        }
        
        /// <summary>
        /// 再抽一次按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onDrawAgainBtnClick(GameObject _go)
        {
            if(_m_ResultMsg ==  null)
                return;

            bool isTenDraw = _m_ResultMsg.getIsTen();
            
            _closeWnd();
            
            if (!isTenDraw)
                WinMsg.SendMsg(WinMsgType.SIMULATE_SUMMON_ONE_DRAW);
            else
                WinMsg.SendMsg(WinMsgType.SIMULATE_SUMMON_TEN_DRAW);
        }

        /// <summary>
        /// 免费抽卡按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onFreeDrawBtnClick(GameObject _go)
        {
            _closeWnd();
            WinMsg.SendMsg(WinMsgType.SIMULATE_SUMMON_FREE_DRAW);
        }
        
        private void _closeWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SUMMON_RESULT);
        }

        /// <summary>
        /// 点击遮罩
        /// </summary>
        public void onClickMask()
        {
            if(!isShow)
                return;
            
            if (_m_wCommonRewardContainer == null || _m_wCommonRewardContainer.allItemShowed)
            {
                _closeWnd();
            }
            else
            {
                _m_wCommonRewardContainer.showAllItem();
                
            }
        }
        
        /// <summary>
        /// 全部奖励物品展示完毕
        /// </summary>
        private void _onAllRewardItemShowed()
        {
            if(wnd == null)
                return;

            _refreshAllRewardItemShowed(true);
        }
    }
}