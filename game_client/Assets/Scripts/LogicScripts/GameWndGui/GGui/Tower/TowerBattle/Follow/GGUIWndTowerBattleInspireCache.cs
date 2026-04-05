using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTowerBattleInspireCache : _AALUICacheController<GGUIWndTowerBattleInspire, GGUIMonoTowerBattleInspire>
    {
        private Transform _m_cacheRoot;
        public GGUIWndTowerBattleInspireCache(Transform _root, int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _root;
        }

        public GGUIWndTowerBattleInspireCache(int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
        {
        }

        protected override string _warningTxt => "GGUIWndTowerBattleInspireCache";

        protected override void _onInit(GGUIMonoTowerBattleInspire _template)
        {
            
        }

        protected override GGUIWndTowerBattleInspire _createItem(GGUIMonoTowerBattleInspire _template)
        {
            GGUIMonoTowerBattleInspire mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false);
            if (mono == null)
                return null;
            mono.transform.SetParent(_m_cacheRoot);
            mono.transform.localPosition = Vector3.zero;
            mono.transform.localScale = Vector3.one;
            GGUIWndTowerBattleInspire wnd = new GGUIWndTowerBattleInspire(mono);

            return wnd;
        }

        protected override void _discardItem(GGUIWndTowerBattleInspire _item)
        {
            GGUIMonoTowerBattleInspire mono = null;
            if(null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }
        protected override void _resetItem(GGUIWndTowerBattleInspire _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }
    }
}