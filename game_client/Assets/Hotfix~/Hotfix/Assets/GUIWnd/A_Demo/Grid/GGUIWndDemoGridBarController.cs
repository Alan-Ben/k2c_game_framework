using ALPackage;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// grid bar 控制器范例
    /// </summary>
    public class GGUIWndDemoGridBarController : _AALUGUIGridBarController
    {
        private GGUIWndDemoGridBar _m_wBarWnd;//显示的窗体

        public GGUIWndDemoGridBarController(Transform _parent) : base()
        {
            _m_wBarWnd = new GGUIWndDemoGridBar(_parent);
            _m_wBarWnd.load();
        }

        public override int barHeight { get { return _m_wBarWnd != null && _m_wBarWnd.rectTransform != null ? (int)_m_wBarWnd.rectTransform.rect.height : 0; } }

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

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(string _str)
        {
            _m_wBarWnd?.setInfo(_str);
        }
    }
}