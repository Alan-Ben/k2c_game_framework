using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子列表bar控制器
    /// </summary>
    public class GGUIWndConsortListBarController : _AALUGUIGridBarController
    {
        private GGUIWndConsortListBar _m_wBarWnd;//显示的窗体

        public GGUIWndConsortListBarController(Transform _parent, NPCommonAssetPathInfo _assetPathInfo) : base()
        {
            _m_wBarWnd = new GGUIWndConsortListBar(_parent, _assetPathInfo);
            _m_wBarWnd.load();
        }

        public override int barHeight { get { return _m_wBarWnd != null && _m_wBarWnd.rectTransform != null ? (int)_m_wBarWnd.rectTransform.rect.height : 0; } }
        public void regLoadDoneDelegate(Action _delegate) { _m_wBarWnd?.regLoadDoneDelegate(_delegate); }

        public override void show()
        {
            _m_wBarWnd?.showWnd();
        }

        public override void hide()
        {
            _m_wBarWnd?.hideWnd();
        }

        protected override void _reset()
        {
            _m_wBarWnd?.resetWnd();
        }

        protected override void _discard()
        {
            _m_wBarWnd?.discard();
            _m_wBarWnd = null;
        }

        public override void setPos(float _x, float _y)
        {
            if (_m_wBarWnd == null)
                return;

            ALUGUICommon.setUIPos(_m_wBarWnd.rectTransform, _x, _y);
        }
    }
}