using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 到达火星选择登录地点item
    /// </summary>
    public class GGUIWndMarsLandingAreaItem : _ATALBasicUISubWnd<GGUIMonoMarsLandingAreaItem>
    {
        //点击事件
        private Action<GGUIWndMarsLandingAreaItem> _m_aOnClickItem;

        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GGUIWndMarsLandingAreaItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndMarsLandingAreaItem(GGUIMonoMarsLandingAreaItem _wnd)
            : base(_wnd)
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
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickSelect);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickSelect);
        }

        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
        }

        //点击选中按钮
        private void _onClickSelect(GameObject _go)
        {
            if(wnd == null)
                return;

            _m_aOnClickItem?.Invoke(this);
        }
    }
}
