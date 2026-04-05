using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 账号切换方式列表item
    /// </summary>
    public class NPGGUIWndAccountSwitchContainerItem : _ATALBasicUISubWnd<NPGGUIMonoAccountSwitchContainerItem>
    {
        private NPLoginWayRefObj _m_loginWayRef;//登录方式配置
        private NPGGuiWndTexture _m_wIcon;//图标

        public NPGGUIWndAccountSwitchContainerItem(NPGGUIMonoAccountSwitchContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_wIcon != null)
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

            if (wnd == null)
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
            if (_m_loginWayRef == null)
                return;

            //设置是否是当前登录方式
            bool isCurLogin = _m_loginWayRef.login_type == SDKLoginSetting.instance.getSDKLoginType();
            ALUGUICommon.setGameObjEnable(wnd.goLoginShowList, isCurLogin);
            ALUGUICommon.setGameObjEnable(wnd.goNoLoginShowList, !isCurLogin);

            //刷新图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_loginWayRef.icon_index);
            }

            //设置描述
            if (isCurLogin)
            {
                string desc = TextTranslate.instance.getLanguage(TransKeyConst.account_accountCurLogin_str, TextTranslate.instance.getLanguage(_m_loginWayRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtDesc,GCommon.addColorForRichText(desc, wnd.curLoginTextColor)); //{0}已登录
            }
            else
            {
                string desc = TextTranslate.instance.getLanguage(TransKeyConst.account_accountNotLogin_str, TextTranslate.instance.getLanguage(_m_loginWayRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.addColorForRichText(desc, wnd.notCurLoginTextColor)); //{0}登录
            }
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

            //设置是否是当前登录方式
            bool isCurLogin = _m_loginWayRef.login_type == SDKLoginSetting.instance.getSDKLoginType();
            if (isCurLogin)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.account_isCurLoginType_none);//已是当前登录方式
                return;
            }

            //先登出操作
            SDKMgr.instance.logout();
            //开始登录指定类型
            SDKMgr.instance.login(_m_loginWayRef.login_type, _tokenData =>
            {
                //sdk登录成功，游戏开始重登
                Game.instance.relogin();
            },((_code, _msg) =>
            {
                //上浮提示
                //切换账号失败（{0}）
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.account_switchFailedCode_num, _code));
            }));
        }
    }
}
