using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndLockConsortDetailProfilePage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoLockConsortDetailProfilePage>, _ILockConsortDetailTabPage
    {
        private NPCommonAssetPathInfo _m_iCommonAssetPathInfo;//通用资源路径信息
        
        private _IConsortShowInfo _m_iConsortShowInfo;//妃子信息
        
        private GGUISubWndConsortProfile _m_wndConsortProfile;//妃子简介子窗口
        
        public GGUIWndLockConsortDetailProfilePage(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iCommonAssetPathInfo = _commonAssetPathInfo;
        }
        
        protected override string _monoAssetPath { get { return _m_iCommonAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iCommonAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoProfile != null)
                _m_wndConsortProfile = new GGUISubWndConsortProfile(wnd.monoProfile);
        }
        
        protected override void _onDiscard()
        {
            
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }
        
        /// <summary>
        /// 因为是未解锁妃子, 所以传入参数直接用ConsortRefShowInfo
        /// </summary>
        /// <param name="_consortShowInfo"></param>
        public void setData(_IConsortShowInfo _consortShowInfo)
        {
            _m_iConsortShowInfo = _consortShowInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_iConsortShowInfo == null || wnd == null)
                return;

            if (_m_wndConsortProfile != null)
            {
                _m_wndConsortProfile.showWnd();
                _m_wndConsortProfile.setData(_m_iConsortShowInfo.consortRefObj);
            }
        }
    }
}