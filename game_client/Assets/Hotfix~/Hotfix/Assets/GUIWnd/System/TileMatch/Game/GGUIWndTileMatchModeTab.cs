using System;
using GOE;
using Hotfix.TileMatchEnum;
using UnityEngine;

namespace Hotfix
{
    public class GGUIWndTileMatchModeTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        private ETileMatch_ModeType _m_eModeType;
        private TileMatchModeRefObj _m_rModeRefObj;
        
        public GGUIWndTileMatchModeTab(ETileMatch_ModeType _modeType, NPGGUIMonoCommonTab _wnd) : base(_wnd)
        {
            _m_eModeType = _modeType;
            _m_rModeRefObj = HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.getRef((long) _m_eModeType);
            initWnd();
        }

        public ETileMatch_ModeType modeType { get { return _m_eModeType; } }

        /// <summary>
        /// 点击事件
        /// </summary>
        public event Action<GGUIWndTileMatchModeTab> onClickButton;

        protected override void _onDiscard()
        {
            base._onDiscard();

            onClickButton = null;
        }

        protected override void _onClickSelectButton(GameObject _go)
        {
            if (wnd == null || _m_rModeRefObj == null)
                return;

            if (!_m_bIsEnable)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_m_rModeRefObj.unlock_condition_desc, _m_rModeRefObj.unlock_condition_desc_args));
                return;
            }
            
            onClickButton?.Invoke(this);
        }

        public void refreshTabUnlock()
        {
            setEnable(_m_rModeRefObj != null && _m_rModeRefObj.gameModeIsUnlock());
        }
    }
}