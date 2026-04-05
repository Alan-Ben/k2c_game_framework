
using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奖励预览弹窗
    /// </summary>
    public class GGUIWndCommonItemToolTip_RewardPreview : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_RewardPreview>
    {

        private NPGGUIWndCommonItemContainer _m_itemContainerWnd;

        public GGUIWndCommonItemToolTip_RewardPreview(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();

            if (_m_itemContainerWnd != null)
                _m_itemContainerWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            if (_m_itemContainerWnd != null)
                _m_itemContainerWnd.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            if (_m_itemContainerWnd != null)
                _m_itemContainerWnd.resetWnd();

        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (_m_itemContainerWnd != null)
                _m_itemContainerWnd.discard();
            _m_itemContainerWnd = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (null == wnd)
                return;

            if (wnd.itemContainerMono != null)
                _m_itemContainerWnd = new NPGGUIWndCommonItemContainer(wnd.itemContainerMono);
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_RewardPreview));
        }

        public void setData(string _titleStr, string _contentStr, List<NPCommonCostItem> _itemList, RectTransform _targetTransRoot, Vector2 _interval)
        {
            if (_m_itemContainerWnd != null)
            {
                _m_itemContainerWnd.showItemList(_itemList);
                _m_itemContainerWnd.showWnd();
            }

            ALUGUICommon.setLabelTxt(wnd.titleTxt, _titleStr);
            ALUGUICommon.setLabelTxt(wnd.contentTxt, _contentStr);

            //设置位置
            setPos(_targetTransRoot, _interval.x,_interval.y);
        }
    }
}