using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerBattleFail : _ATALBasicUIWnd<GGUIMonoTowerBattleFail>
    {
        private static GGUIWndTowerBattleFail _g_instance = new GGUIWndTowerBattleFail();

        public static GGUIWndTowerBattleFail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattleFail();
                return _g_instance;
            }
        }

        public GGUIWndTowerBattleFail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerBattleFail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerBattleFail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        private TowerChallengeResult _m_result;
        private Action _m_onClose;

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onBtnCloseClick);
        }

        private void _onBtnCloseClick(GameObject _obj)
        {
            _m_onClose?.Invoke();
        }

        public void setInfo(TowerChallengeResult _result, Action _onClose)
        {
            _m_result = _result;
            _m_onClose = _onClose;
            _refreshWnd();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
        }
    }
}