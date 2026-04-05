using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面简介page
    /// </summary>
    public class GGUIWndUnlockConsortDetailProfile : _ATALBasicUIWnd<GGUIMonoUnlockConsortDetailProfile>
    {
        private static GGUIWndUnlockConsortDetailProfile _g_instance;
        public static GGUIWndUnlockConsortDetailProfile instance { get { return _g_instance ??= new GGUIWndUnlockConsortDetailProfile(); } }
        
        private GConsortRefObj _m_iConsortRefObj;
        
        private GGUISubWndConsortProfile _m_wndConsortProfile;//妃子简介子窗口

        public GGUIWndUnlockConsortDetailProfile() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoUnlockConsortDetailProfile.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoUnlockConsortDetailProfile.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoConsortProfile != null)
                _m_wndConsortProfile = new GGUISubWndConsortProfile(wnd.monoConsortProfile);
            
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            }
            
            if(_m_wndConsortProfile != null)
                _m_wndConsortProfile.discard();
            _m_wndConsortProfile = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wndConsortProfile?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndConsortProfile?.resetWnd();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void setData(GConsortRefObj _consortRefObj)
        {
            _m_iConsortRefObj = _consortRefObj;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            if (_m_wndConsortProfile != null && _m_iConsortRefObj != null)
            {
                _m_wndConsortProfile.showWnd();
                _m_wndConsortProfile.setData(_m_iConsortRefObj);
            }
        }
        
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_PROFILE_WND);
        }
    }
}