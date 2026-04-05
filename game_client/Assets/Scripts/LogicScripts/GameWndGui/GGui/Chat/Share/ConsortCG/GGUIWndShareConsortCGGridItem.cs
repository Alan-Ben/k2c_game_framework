using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分享情人CG的item
    /// </summary>
    public class GGUIWndShareConsortCGGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoShareConsortCGGridItem>
    {
        private ConsortCgInfo _m_showData;
        public Action<ConsortCgInfo> clickItem;
        private NPGGuiWndTexture _m_texIcon;

        public GGUIWndShareConsortCGGridItem(GGUIMonoShareConsortCGGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public ConsortCgInfo showData { get => _m_showData; }

        protected override void _onShowWnd()
        {
            _m_texIcon?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_texIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_texIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_texIcon?.discard();
            _m_texIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnSelected, _clickSelected);

            if (null != wnd.texIcon)
                _m_texIcon = new NPGGuiWndTexture(wnd.texIcon);
        }

        private void _clickSelected(GameObject obj)
        {
            clickItem?.Invoke(_m_showData);
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_showData"></param>
        public void setInfo(ConsortCgInfo _showData)
        {
            _m_showData = _showData;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_showData == null || _m_showData.consortCgRefObj == null)
                return;

            _m_texIcon?.setTexture(_m_showData.consortCgRefObj.cg_icon);

            ALUGUICommon.setLabelTxt(wnd.txtCGName, TextTranslate.instance.getLanguage(_m_showData.consortCgRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtConsortName, GCommon.getItemName(ENPItemType.CONSORT, _m_showData.consortCgRefObj.consort_id));
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.selectedList, _isSelected);
        }
    }
}
