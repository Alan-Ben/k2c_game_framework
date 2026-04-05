using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跨服服务器列表的跟随窗口
    /// </summary>
    public class GGUIWndCommonItemToolTip_ServerList : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_ServerList>
    {
        //文本列表
        private GGUIWndCommonTextContainer _m_wTextContainer;

        public GGUIWndCommonItemToolTip_ServerList(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }
        
        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_wTextContainer?.hideWnd();
        }
        
        protected override void _onReset()
        {
            base._onReset();
            _m_wTextContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_wTextContainer?.discard();
            _m_wTextContainer = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if(wnd.monoTextContainer != null)
            {
                _m_wTextContainer = new GGUIWndCommonTextContainer(wnd.monoTextContainer);
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<string> _serverNameList, RectTransform _targetTransRoot, float _interval)
        {
            if(null == wnd)
                return;

            _m_wTextContainer?.showWnd();
            _m_wTextContainer?.setInfo(_serverNameList);

            //设置位置
            setPos(_targetTransRoot, _interval);
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_TOOL_TIP_SERVER_LIST);
        }
    }
}