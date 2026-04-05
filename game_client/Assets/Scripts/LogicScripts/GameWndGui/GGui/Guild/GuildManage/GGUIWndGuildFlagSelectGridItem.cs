using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟旗帜列表item
    /// </summary>
    public class GGUIWndGuildFlagSelectGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoGuildFlagSelectGridItem>
    {
        //旗帜信息
        private GuildFlagRefObj _m_refObj;
        //图标
        private NPGGuiWndTexture _m_wIcon;
        //选中回调
        private Action<GGUIWndGuildFlagSelectGridItem> _m_aOnItemSelect;

        /// <summary>
        /// 选中回调
        /// </summary>
        public Action<GGUIWndGuildFlagSelectGridItem> onItemSelect { get { return _m_aOnItemSelect; } set { _m_aOnItemSelect = value; } }
        /// <summary>
        /// 旗帜数据
        /// </summary>
        public GuildFlagRefObj flagRefObj { get { return _m_refObj; } }

        public GGUIWndGuildFlagSelectGridItem(GGUIMonoGuildFlagSelectGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _resetGridItem()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wIcon?.discard();
            _m_wIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickSelect);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickSelect);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setInfo(GuildFlagRefObj _refObj)
        {
            if (_refObj == null)
                return;

            _m_refObj = _refObj;

            //刷新图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_refObj.icon);
            }
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_select"></param>
        public void setSelectState(bool _select)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _select);
        }

        /// <summary>
        /// 设置点击选中item
        /// </summary>
        public void setClickSelectItem()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, true);
            _m_aOnItemSelect?.Invoke(this);
        }

        //点击选中
        private void _onClickSelect(GameObject _go)
        {
            _m_aOnItemSelect?.Invoke(this);
        }
    }
}
