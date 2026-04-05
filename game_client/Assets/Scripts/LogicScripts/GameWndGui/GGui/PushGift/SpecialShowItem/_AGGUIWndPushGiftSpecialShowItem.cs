using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包特殊展示物品
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AGGUIWndPushGiftSpecialShowItem<T> : _ANPGGUIBasicLoadPrefabSubWnd<T> , _IGGUIWndPushGiftSpecialShowItem where T : _AGGUIMonoPushGiftSpecialShowItem
    {
        private NPCommonAssetPathInfo _m_iCommonAssetPath;
        protected NPCommonCostItem _m_iCommonCostItem;
        
        private NPGGUIWndCommonItem _m_wCommonItem;

        protected _AGGUIWndPushGiftSpecialShowItem(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iCommonAssetPath = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_iCommonAssetPath?.asset_path; } }
        protected override string _monoObjName { get { return _m_iCommonAssetPath?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public NPCommonAssetPathInfo assetPathInfo { get { return _m_iCommonAssetPath; } }
        public _AALBasicLoadUIWndBasicClass wndInstance { get { return this; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoCommonItem != null)
                _m_wCommonItem = new NPGGUIWndCommonItem(wnd.monoCommonItem);
        }

        protected override void _onDiscard()
        {
            _m_wCommonItem?.discard();
            _m_wCommonItem = null;

            _m_iCommonCostItem = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wCommonItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonItem?.resetWnd();
        }

        public void setData(NPCommonCostItem _commonCostItem)
        {
            _m_iCommonCostItem = _commonCostItem;

            _onSetData();

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_wCommonItem != null)
            {
                _m_wCommonItem.showWnd();
                _m_wCommonItem.setItem(_m_iCommonCostItem);
            }
            
            _onRefreshWnd();
        }
        
        protected abstract void _onSetData();
        
        protected abstract void _onRefreshWnd();
    }
}