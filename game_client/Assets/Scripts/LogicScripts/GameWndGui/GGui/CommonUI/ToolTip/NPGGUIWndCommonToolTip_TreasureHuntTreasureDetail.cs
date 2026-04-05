using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndCommonToolTip_TreasureHuntTreasureDetail : _ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonToolTip_TreasureHuntTreasureDetail>
    {
        private NPGGuiWndTexture _m_wTreasureIcon;

        public NPGGUIWndCommonToolTip_TreasureHuntTreasureDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;

            if (wnd.imgTreasureIcon != null)
                _m_wTreasureIcon = new NPGGuiWndTexture(wnd.imgTreasureIcon);
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_wTreasureIcon?.discard();
            _m_wTreasureIcon = null;
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wTreasureIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wTreasureIcon?.discardTexture();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_ITreasureHuntTreasureInfo _treasureInfo, RectTransform _targetTransRoot, Vector2 _interval)
        {
            if(null == wnd || _treasureInfo == null || _treasureInfo.treasureRefObj == null)
                return;

            TreasureHuntTreasureRefObj treasureRefObj = _treasureInfo.treasureRefObj;
            ETreasureHuntTreasureState treasureState = _treasureInfo.treasureState;

            if (_m_wTreasureIcon != null)
            {
                _m_wTreasureIcon.showWnd();
                _m_wTreasureIcon.setTexture(treasureRefObj.icon);
            }
            
            wnd.refreshTreasureState(treasureState);

            ALUGUICommon.setLabelTxt(wnd.txtTreasureName, TextTranslate.instance.getLanguage(treasureRefObj.name));
            
            ALUGUICommon.setLabelTxt(wnd.txtNotGetTreasureDesc, TextTranslate.instance.getLanguage(treasureRefObj.not_get_desc, treasureRefObj.not_get_desc_args_list));
            ALUGUICommon.setLabelTxt(wnd.txtTreasureDesc, TextTranslate.instance.getLanguage(treasureRefObj.desc));

            //设置位置
            setPos(_targetTransRoot, _interval.x, _interval.y);
        }
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_TreasureHuntTreasureDetail));
        }
    }
}