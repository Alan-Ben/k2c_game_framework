using System;
using ALPackage;
using Common.ConsortEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortStoryBarController : _AALUGUIGridBarController
    {
        private GGUIWndConsortStoryBar _m_wBarWnd;//显示的窗体
        
        private EConsortStoryType _m_eStoryType;

        public GGUIWndConsortStoryBarController(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base()
        {
            _m_wBarWnd = new GGUIWndConsortStoryBar(_assetPathInfo, _parent);
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

        /// <summary>
        /// 设置故事类型
        /// </summary>
        /// <param name="_storyType"></param>
        public void setStoryType(EConsortStoryType _storyType)
        {
            _m_eStoryType = _storyType;
            
            _m_wBarWnd?.setData(_m_eStoryType);
        }
    }
}