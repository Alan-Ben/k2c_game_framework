using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子详情页面妃子加护信息page
    /// </summary>
    public class GGUIWndLockConsortDetailBlessPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoLockConsortDetailBlessPage>, _ILockConsortDetailTabPage
    {
        private GGUIWndConsortBlessHeroSimpleIconContainer _m_wRelationHeroContainer;
        
        private _IConsortShowInfo _m_iConsortShowInfo;//妃子信息
        
        private NPCommonAssetPathInfo _m_iCommonAssetPathInfo;//通用资源路径信息
        
        public GGUIWndLockConsortDetailBlessPage(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iCommonAssetPathInfo = _commonAssetPathInfo;
        }
        
        protected override string _monoAssetPath { get { return _m_iCommonAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iCommonAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRelationHeroContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRelationHeroContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRelationHeroContainer?.discard();
            _m_wRelationHeroContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if (wnd.monoRelationHeroContainer != null)
                _m_wRelationHeroContainer = new GGUIWndConsortBlessHeroSimpleIconContainer(wnd.monoRelationHeroContainer);
        }

        public void setData(_IConsortShowInfo _consortShowInfo)
        {
            _m_iConsortShowInfo = _consortShowInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iConsortShowInfo == null)
                return;

            if (_m_wRelationHeroContainer != null)
            {
                _m_wRelationHeroContainer.showWnd();
                _m_wRelationHeroContainer.setData(_m_iConsortShowInfo.consortId);
            }
        }
    }
}