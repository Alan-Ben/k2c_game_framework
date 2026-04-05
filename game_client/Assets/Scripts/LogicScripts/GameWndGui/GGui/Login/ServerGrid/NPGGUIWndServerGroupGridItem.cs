using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 服务器组单个Item
    /// </summary>
    public class NPGGUIWndServerGroupGridItem : _ATALUGUIBasicGridItemWnd<NPGGUIMonoServerGroupGridItem>
    {
        private ServerGroupShowData _m_groupData;//服务器组信息
        private Action<NPGGUIWndServerGroupGridItem> _m_onSelected;//点击回调
        private GGUIWndCommonSetColorTab _m_clickToggle;//点击tab

        public NPGGUIWndServerGroupGridItem(NPGGUIMonoServerGroupGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        /// <summary>
        /// 服务器组信息
        /// </summary>
        public ServerGroupShowData groupData {get { return _m_groupData; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_clickToggle)
            {
                _m_clickToggle.discard();
                _m_clickToggle = null;
            }
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (null != wnd.clickBtn)
            {
                _m_clickToggle = new GGUIWndCommonSetColorTab(wnd.clickBtn);
                _m_clickToggle.clickDelegate  = _onClickTab;
                setSelected(false);
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_onSelected"></param>
        public void setInfo(ServerGroupShowData _data, Action<NPGGUIWndServerGroupGridItem> _onSelected)
        {
            _m_onSelected = _onSelected;
            _m_groupData = _data;
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_groupData)
                return;

            ALUGUICommon.setLabelTxt(wnd.textGroupName, TextTranslate.instance.getLanguage(_m_groupData.groupName));
        }
        
        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickTab(bool _obj)
        {
            if (null != _m_onSelected)
                _m_onSelected(this);
        }
        
        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (null != _m_clickToggle)
                _m_clickToggle.setSelected(_isSelected);
        }

    }
}
