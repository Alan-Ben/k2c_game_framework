using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    ///  账号绑定方式列表item
    /// </summary>
    public class NPGGUIWndAccountBindContainerItem : _ATALBasicUISubWnd<NPGGUIMonoAccountBindContainerItem>
    {
        private NPLoginWayRefObj _m_loginWayRef;//登录方式配置
        private NPGGuiWndTexture _m_wIcon;//图标

        public NPGGUIWndAccountBindContainerItem(NPGGUIMonoAccountBindContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            if(_m_wIcon != null)
                _m_wIcon.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wIcon != null)
                _m_wIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (_m_wIcon != null)
                _m_wIcon.discard();
            _m_wIcon = null;

            if(wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgLoginType != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgLoginType);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_loginWayRef"></param>
        public void setInfo(NPLoginWayRefObj _loginWayRef)
        {
            _m_loginWayRef = _loginWayRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_loginWayRef == null)
                return;

            //设置是否绑定展示GO列表
            bool isBind = SDKMgr.instance.isBind(_m_loginWayRef.login_type.ToString().ToLowerInvariant());
            ALUGUICommon.setGameObjEnable(wnd.goBindShowList,isBind);
            ALUGUICommon.setGameObjEnable(wnd.goUnBindShowList,!isBind);

            //刷新图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_loginWayRef.icon_index);
            }

            //设置描述
            if(isBind)
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.account_loginTypeBind_str, TextTranslate.instance.getLanguage(_m_loginWayRef.name)));//{0}已绑定
            else
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.account_loginTypeUnBind_str, TextTranslate.instance.getLanguage(_m_loginWayRef.name)));//绑定{0}
        }

        //点击按钮
        private void _onClickBtn(GameObject _go)
        {
            if (_m_loginWayRef == null)
                return;

            if (!SDKMgr.instance.isUseSDK)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo("未使用SDK");
                return;
            }

            //是否已绑定
            bool isBind = SDKMgr.instance.isBind(_m_loginWayRef.login_type.ToString().ToLowerInvariant());
            if (isBind)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.account_alreadyBind_none);//已绑定
                return;
            }

            SDKMgr.instance.bind(true, _m_loginWayRef.login_type,()=>
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.account_bindSucceed_none);//绑定成功
                _refreshWnd();
            },((_code, _msg) =>
            {
                //上浮提示
                //绑定失败（{0}）
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.account_bindFailedCode_num, _code));
            }));
        }
    }
}
