using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用带自定义文本的跟随窗口
    /// </summary>
    public class NPGGUIWndCommonItemToolTip_IconDetail :_ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonToolTip_IconDetail>
    {
        private string _m_textContent;
        private NPGGuiWndTexture _m_wIconWnd;//图片

        public NPGGUIWndCommonItemToolTip_IconDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }
        
        protected override void _onReset()
        {
            base._onReset();
            
            if(null != _m_wIconWnd)
                _m_wIconWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if(null != _m_wIconWnd)
                _m_wIconWnd.discard();
            _m_wIconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd != null && null != wnd.icon) 
                _m_wIconWnd = new NPGGuiWndTexture(wnd.icon);
        }
        

        protected override float getSelfHeight()
        {
            if (null == wnd)
                return 0;
            
            return wnd.heightWithoutText + wnd.getTextHeight(_m_textContent);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _itemId, RectTransform _targetTransRoot, float _interval)
        {
            if(null == wnd)
                return;

            _m_textContent = GCommon.getItemDesc(ENPItemType.TOOL_TIP_ITEM, _itemId);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_textContent);
            
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.TOOL_TIP_ITEM, _itemId));

            if (null != _m_wIconWnd)
            {
                _m_wIconWnd.setTexture(GCommon.getItemTexIcon(ENPItemType.TOOL_TIP_ITEM, _itemId));
            }
            
            //设置位置
            setPos(_targetTransRoot, _interval);
        }
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_IconDetail));
        }
    }
}