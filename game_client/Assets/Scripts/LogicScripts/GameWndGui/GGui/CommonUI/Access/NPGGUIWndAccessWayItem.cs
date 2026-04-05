using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取途径item
    /// </summary>
    public class NPGGUIWndAccessWayItem : _ATALBasicUISubWnd<NPGGUIMonoAccessWayItem>
    {
        private NPAccessInfo _m_accessInfo;
        private NPGGuiWndTexture _m_wIconWnd;

        public NPGGUIWndAccessWayItem(NPGGUIMonoAccessWayItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickBtnGoTo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //图标
            if (wnd.imgIcon != null)
            {
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            }

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickBtnGoTo);
        }


        #region 点击事件

        /// <summary>
        /// 点击前往
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnGoTo(GameObject _go)
        {
            if (_m_accessInfo == null)
                return;

            //未解锁
            if (_m_accessInfo.isLock)
            {
                string lockDesc = _m_accessInfo.getLockDesc();
                if (!string.IsNullOrEmpty(lockDesc))
                    NPGUIAddSceneCenterTip.instance.showTextInfo(_m_accessInfo.getLockDesc());
                return;
            }

            if (_m_accessInfo.refObj == null || _m_accessInfo.refObj.go_to == null)
                return;

            //执行跳转效果
            _m_accessInfo.refObj.go_to.dealEffect();
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (_m_accessInfo == null || _m_accessInfo.refObj == null || wnd == null)
                return;

            //图标
            _m_wIconWnd?.setTexture(_m_accessInfo.refObj.tex_icon);

            //获取途径标题（名称）
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_accessInfo.refObj.name));

            //获取途径描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_accessInfo.refObj.desc, _m_accessInfo.refObj.desc_args));

            //无跳转效果时隐藏的物体
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnNoGoToEffect, _m_accessInfo.refObj.go_to != null && !_m_accessInfo.refObj.go_to.isEmpty);

            //获取途径未开启描述
            string lockDesc = _m_accessInfo.getLockDesc();
            if (!string.IsNullOrEmpty(lockDesc))
                ALUGUICommon.setLabelTxt(wnd.txtLockDesc, TextTranslate.instance.getLanguage(TransKeyConst.common_parentheses_str, lockDesc));
            else
                ALUGUICommon.setLabelTxt(wnd.txtLockDesc, string.Empty);

            //判断是否必得途径
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSure, !_m_accessInfo.isLock && _m_accessInfo.isSure);

            //判断获取途径是否开启
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnLock, _m_accessInfo.isLock);
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnLock, !_m_accessInfo.isLock);
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_accessInfo"></param>
        public void setInfo(NPAccessInfo _accessInfo)
        {
            _m_accessInfo = _accessInfo;
            _refreshWnd();
        }

        #endregion
    }
}
