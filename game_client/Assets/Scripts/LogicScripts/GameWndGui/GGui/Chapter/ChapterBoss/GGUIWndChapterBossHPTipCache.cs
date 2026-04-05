using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterBossHPTipCache : _AALUICacheController<GGUIWndChapterBossHPTip, GGUIMonoChapterBossHPTip>
    {
        private Transform _m_cacheRoot;
        public GGUIWndChapterBossHPTipCache(Transform _root, int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _root;
        }

        public GGUIWndChapterBossHPTipCache(int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
        {
        }

        protected override string _warningTxt => "GGUIWndChapterBossHPTipCache";

        protected override void _onInit(GGUIMonoChapterBossHPTip _template)
        {
            
        }

        protected override GGUIWndChapterBossHPTip _createItem(GGUIMonoChapterBossHPTip _template)
        {
            GGUIMonoChapterBossHPTip mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false);
            if (mono == null)
                return null;
            mono.transform.SetParent(_m_cacheRoot);
            mono.transform.localPosition = Vector3.zero;
            mono.transform.localScale = Vector3.one;
            GGUIWndChapterBossHPTip wnd = new GGUIWndChapterBossHPTip(mono);

            return wnd;
        }

        protected override void _discardItem(GGUIWndChapterBossHPTip _item)
        {
            GGUIMonoChapterBossHPTip mono = null;
            if(null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }
        protected override void _resetItem(GGUIWndChapterBossHPTip _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }
    }
}