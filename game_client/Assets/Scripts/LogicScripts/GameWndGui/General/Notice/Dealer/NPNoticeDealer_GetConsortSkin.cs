using System;
using ALPackage;
using System.Collections.Generic;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    ///获得妃子皮肤弹窗
    /// </summary>
    public class NPNoticeDealer_GetConsortSkin : NPUINoticeMgr._ANPUINoticeDealer
    {
        private GConsortSkinInfo _m_iConsortSkinInfo;//妃子皮肤信息

        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_GetConsortSkin(GConsortSkinInfo _skinInfo)
        {
            _m_iConsortSkinInfo = _skinInfo;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }


        public override void dealShowNotice()
        {
            if (_m_iConsortSkinInfo == null)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndConsortSkinGet.instance.load(() =>
                {
                    GGUIWndConsortSkinGet.instance.showWnd();
                    GGUIWndConsortSkinGet.instance.setData(_m_iConsortSkinInfo);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndConsortSkinGet.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndConsortSkinGet.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndConsortSkinGet.instance.discard();
                _m_bWndLoaded = false;
                setDealerDone();
            }
        }

        protected override void _onDealerDone()
        {

        }
    }
}
