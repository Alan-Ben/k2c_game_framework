using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class TowerBattleHPTipCacheMgr : _AALUICacheController<GGUIWndTowerBattleHPTip, GGUIMonoTowerBattleHPTip>
    {
        private Transform _m_cacheRoot;
        public TowerBattleHPTipCacheMgr(Transform _root, int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _root;
        }

        public TowerBattleHPTipCacheMgr(int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
        {
        }

        protected override string _warningTxt => "TowerBattleHPTipCacheMgr";

        protected override void _onInit(GGUIMonoTowerBattleHPTip _template)
        {
            
        }

        protected override GGUIWndTowerBattleHPTip _createItem(GGUIMonoTowerBattleHPTip _template)
        {
            GGUIMonoTowerBattleHPTip mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false);
            if (mono == null)
                return null;
            mono.transform.SetParent(_m_cacheRoot);
            mono.transform.localPosition = Vector3.zero;
            mono.transform.localScale = Vector3.one;
            GGUIWndTowerBattleHPTip wnd = new GGUIWndTowerBattleHPTip(mono);

            return wnd;
        }

        protected override void _discardItem(GGUIWndTowerBattleHPTip _item)
        {
            GGUIMonoTowerBattleHPTip mono = null;
            if(null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }
        protected override void _resetItem(GGUIWndTowerBattleHPTip _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }
    }
}