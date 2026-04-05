using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 条件描述item
    /// </summary>
    public class GGUIWndConditionDescItem : _ATALBasicUISubWnd<GGUIMonoConditionDescItem>
    {
        private _IConditionDescShow _m_iShowInfo;//显示信息
        private bool _m_bPreConditionIsEnable;//上一次刷新时的条件是否满足

        private NPGGuiWndTexture _m_wConditionIcon;//条件图标
        private int _m_iItemIndex;//item在容器中的索引

        public GGUIWndConditionDescItem(GGUIMonoConditionDescItem _wnd) : base(_wnd)
        {
            _m_iItemIndex = -1;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.icon != null)
                _m_wConditionIcon = new NPGGuiWndTexture(wnd.icon);
            
            ALUGUICommon.combineBtnClick(wnd.btnJumpTo, _onJumpBtnClick);
        }
        
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnJumpTo, _onJumpBtnClick);
            
            if(_m_wConditionIcon != null)
                _m_wConditionIcon.discard();
            _m_wConditionIcon = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONDITION_DESC_JUMP_BY_INDEX, _onSimulateClickConditionDescJump);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONDITION_DESC_JUMP_BY_INDEX, _onSimulateClickConditionDescJump);
            _m_wConditionIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConditionIcon?.discardTexture();
        }

        public void setItemIndex(int _index)
        {
            _m_iItemIndex = _index;
        }

        public void setData(_IConditionDescShow _showInfo)
        {
            _m_iShowInfo = _showInfo;

            refreshWnd();
        }

        public void refreshWnd(bool _forceRefresh = true)
        {
            if (_m_iShowInfo == null || wnd == null)
                return;

            bool isEnable = _m_iShowInfo.conditionIsEnable;
            if(!_forceRefresh && _m_bPreConditionIsEnable == isEnable)//当不强制刷新时，条件满足情况不变时不刷新
                return;

            _m_bPreConditionIsEnable = isEnable;
            
            if (_m_wConditionIcon != null)
            {
                if (_m_iShowInfo.conditionIcon != null && _m_iShowInfo.conditionIcon.enable())
                {
                    _m_wConditionIcon.showWnd();
                    _m_wConditionIcon.setTexture(_m_iShowInfo.conditionIcon);    
                }
                else
                {
                    _m_wConditionIcon.hideWnd();
                }
            }

            string desc = GCommon.addColorForRichText(_m_iShowInfo.conditionDesc, isEnable ? wnd.enableDescColor : wnd.unableDescColor);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, desc);
            
            ALUGUICommon.setGameObjEnable(wnd.enableShow, isEnable);
            ALUGUICommon.setGameObjEnable(wnd.unableShow, !isEnable);

            if (isEnable)
            {
                GGameCommonInfo.disgrayImage(wnd.unableGrayList);
            }
            else
            {
                GGameCommonInfo.grayImage(wnd.unableGrayList);
            }

            if (isEnable)
            {
                if(wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.onConditionEnableAnimationName))
                    wnd.wndAnimation.ForcePlay(wnd.onConditionEnableAnimationName);
            }
            else
            {
                if(wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.onConditionEnableAnimationName))
                    wnd.wndAnimation.Sample(wnd.onConditionEnableAnimationName, 0f);
            }
        }


        private void _onJumpBtnClick(GameObject _)
        {
            _m_iShowInfo?.jumpFunc();
        }

        private void _onSimulateClickConditionDescJump(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] is not long targetIndex)
                return;

            if (targetIndex == _m_iItemIndex)
                _simulateClickJumpButton();
        }

        public void _simulateClickJumpButton()
        {
            _m_iShowInfo?.jumpFunc();
        }
    }
}