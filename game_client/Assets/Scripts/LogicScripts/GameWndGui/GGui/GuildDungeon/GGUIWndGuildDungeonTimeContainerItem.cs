using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟PVE时间选择
    /// </summary>
    public class GGUIWndGuildDungeonTimeContainerItem : _ATNPGGUIWndSelectContainerItem<GGUIMonoGuildDungeonTimeContainerItem>
    {
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        private int _m_value; // 当前值

        public int value => _m_value;
        
        public GGUIWndGuildDungeonTimeContainerItem(GGUIMonoGuildDungeonTimeContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWndEx()
        {
        }
    
        protected override void _onResetEx()
        {
        }
    
        protected override void _onDiscardEx()
        {
        }
    
        protected override void _onWndInitDoneEx()
        {
            if(null == wnd)
                return;
        }

        public void setInfo(int _value)
        {
            _m_value = _value;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_value);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
