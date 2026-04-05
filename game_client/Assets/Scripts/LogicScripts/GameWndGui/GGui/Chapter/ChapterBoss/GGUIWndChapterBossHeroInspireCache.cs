using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterBossHeroInspireCache : _AALUICacheController<GGUIWndChapterBossHeroInspire, GGUIMonoChapterBossHeroInspire>
    {
        private Transform _m_cacheRoot;
        public GGUIWndChapterBossHeroInspireCache(Transform _root, int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _root;
        }

        public GGUIWndChapterBossHeroInspireCache(int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
        {
        }

        protected override string _warningTxt => "GGUIWndChapterBossHeroInspireCache";

        protected override void _onInit(GGUIMonoChapterBossHeroInspire _template)
        {
            
        }

        protected override GGUIWndChapterBossHeroInspire _createItem(GGUIMonoChapterBossHeroInspire _template)
        {
            GGUIMonoChapterBossHeroInspire mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false);
            if (mono == null)
                return null;
            mono.transform.SetParent(_m_cacheRoot);
            mono.transform.localPosition = Vector3.zero;
            mono.transform.localScale = Vector3.one;
            GGUIWndChapterBossHeroInspire wnd = new GGUIWndChapterBossHeroInspire(mono);

            return wnd;
        }

        protected override void _discardItem(GGUIWndChapterBossHeroInspire _item)
        {
            GGUIMonoChapterBossHeroInspire mono = null;
            if(null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }
        protected override void _resetItem(GGUIWndChapterBossHeroInspire _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }
    }
}