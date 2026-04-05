using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIRuleItemBarController : _AALUGUIGridBarController
    {
        private NPGGUIWndRuleItemBar _m_wBarWnd;//显示的窗体
        private NPRuleRefObj _m_ruleRefObj;

        public NPGGUIRuleItemBarController(Transform _parent) : base()
        {
            _m_wBarWnd = new NPGGUIWndRuleItemBar(_parent);
            _m_wBarWnd.load();
            
            _m_wBarWnd.regLoadDoneDelegate(() =>
            {
                _m_wBarWnd.showWnd();
           });
        }

        public NPRuleRefObj ruleRefObj { get { return _m_ruleRefObj; } }
        public override bool isAfterLineBar { get { return true; } }
        public override int barHeight { get { return _m_wBarWnd != null && _m_wBarWnd.rectTransform != null ? (int)_m_wBarWnd.rectTransform.rect.height : 0; } }

        public override void show()
        {
            _m_wBarWnd?.showWnd();
            ALUnityCommon.moveTransformToFirst(_m_wBarWnd?.wnd);
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
        /// <param name="_questInfo"></param>
        public void setInfo(NPRuleRefObj _ruleRefObj)
        {
            _m_ruleRefObj = _ruleRefObj;
            _m_wBarWnd?.setInfo(_ruleRefObj);
        }
    }
}
