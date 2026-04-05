using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortCGGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoConsortCGGridItem>
    {
        private ConsortCGRefObj _m_consortCGRefObj;//妃子CG配表数据
        private int _m_iIndex;//当前item在列表中的索引
        private EGameCommonUnlockRewardType _m_unlockState;//解锁状态

        private NPGGuiWndTexture _m_cgIcon;//CG图标
        
        public GGUIWndConsortCGGridItem(GGUIMonoConsortCGGridItem _wnd) : base(_wnd)
        {
        }

        public ConsortCGRefObj consortCGRefObj { get { return _m_consortCGRefObj; } }
        public Action<GGUIWndConsortCGGridItem> onClickDrawReward;//当点击领取奖励时的回调
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.cg_icon != null)
                _m_cgIcon = new NPGGuiWndTexture(wnd.cg_icon);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onItemClick);
            ALUGUICommon.combineBtnClick(wnd.btnShare, _onShareClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onItemClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnShare, _onShareClick);
            }
            
            if(_m_cgIcon != null)
                _m_cgIcon.discard();
            _m_cgIcon = null;

            onClickDrawReward = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_CG_ITEM_BY_INDEX, _simulateClickCgItemByIndex);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_CG_ITEM_BY_INDEX, _simulateClickCgItemByIndex);
            
            _m_cgIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_cgIcon?.discardTexture();
        }

        protected override void _resetGridItem()
        {
            _m_cgIcon?.discardTexture();
        }

        public void setData(ConsortCGRefObj _consortCgRef, int _index)
        {
            _m_consortCGRefObj = _consortCgRef;
            _m_iIndex = _index;

            refreshWnd();
        }

        /// <summary>
        /// 更新cg信息
        /// </summary>
        private void _updateCgInfo()
        {
            ConsortCgInfo consortCgInfo = NPPlayer.instance.consortComp.getConsortCgInfo(_m_consortCGRefObj?.cg_id ?? 0);

            if (consortCgInfo == null)
            {
                _m_unlockState = EGameCommonUnlockRewardType.LOCK;
            }
            else
            {
                _m_unlockState = consortCgInfo.rewarded
                    ? EGameCommonUnlockRewardType.UNLOCK_HAS_GET
                    : EGameCommonUnlockRewardType.UNLOCK_UN_GET;
            }
        }

        public void refreshWnd()
        {
            if(wnd == null || _m_consortCGRefObj == null)
                return;
            
            _updateCgInfo();

            ALUGUICommon.setLabelTxt(wnd.txtCgName, TextTranslate.instance.getLanguage(_m_consortCGRefObj.name));

            GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_consortCGRefObj.consort_id);
            ALUGUICommon.setLabelTxt(wnd.txtOwnerName, consortRefObj?.transName);

            if (_m_cgIcon != null)
            {
                _m_cgIcon.showWnd();
                _m_cgIcon.setTexture(_m_consortCGRefObj.cg_icon);
            }
            
            wnd.setState(_m_unlockState);
        }

        /// <summary>
        /// 当item被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onItemClick(GameObject _go)
        {
            if(_m_consortCGRefObj == null)
                return;
            
            switch (_m_unlockState)
            {
                case EGameCommonUnlockRewardType.LOCK://未解锁
                    return;
                
                case EGameCommonUnlockRewardType.UNLOCK_UN_GET://已解锁未领奖
                    onClickDrawReward?.Invoke(this);
                    return;
                
                case EGameCommonUnlockRewardType.UNLOCK_HAS_GET://已解锁已领奖
                    QueueMgr.instance.AddNode(new GNodeConsortCGDetail(_m_consortCGRefObj));
                    return;
            }
        }

        /// <summary>
        /// 点击分享按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onShareClick(GameObject _go)
        {
            if (_m_consortCGRefObj == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndConsortCGShareConfirm.instance, () =>
            {
                GGUIWndConsortCGShareConfirm.instance.showWnd();
                GGUIWndConsortCGShareConfirm.instance.setData(_m_consortCGRefObj);
            }, UINodeTagConst.C_CONSORT_CG_SHARE_CONFIRM);
        }

        /// <summary>
        /// 模拟点击CG item
        /// </summary>
        private void _simulateClickCgItemByIndex(params object[] _params)
        {
            if (_params == null || _params.Length < 1 || !(_params[0] is long index))
                return;

            if(_m_iIndex != index)
                return;

            _onItemClick(null);
        }
    }
}