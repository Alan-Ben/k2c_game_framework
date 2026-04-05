using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面简介page
    /// </summary>
    public class GGUIWndUnlockConsortDetailProfilePage : _AGGUIWndUnLockConsortDetailTabPage<GGUIMonoUnlockConsortDetailProfilePage>
    {
        private GGUISubWndConsortProfile _m_wndConsortProfile;//妃子简介子窗口
        
        public GGUIWndUnlockConsortDetailProfilePage(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_commonAssetPathInfo, _parent)
        {
        }

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public override EUnLockConsortDetailWndTabType tabPageType { get { return EUnLockConsortDetailWndTabType.PROFILE; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoConsortProfile != null)
                _m_wndConsortProfile = new GGUISubWndConsortProfile(wnd.monoConsortProfile);
        }

        protected override void _onDiscardSub()
        {
            if(_m_wndConsortProfile != null)
                _m_wndConsortProfile.discard();
            _m_wndConsortProfile = null;
        }

        protected override void _onShowWndSub()
        {
            
        }

        protected override void _onHideWndSub()
        {
            _m_wndConsortProfile?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wndConsortProfile?.resetWnd();
        }

        protected override void _setDataSub()
        {
        }

        protected override void _refreshWndSub()
        {
            if (_m_wndConsortProfile != null && _m_iConsortShowInfo != null)
            {
                _m_wndConsortProfile.showWnd();
                _m_wndConsortProfile.setData(_m_iConsortShowInfo.consortRefObj);
            }
        }
    }
}