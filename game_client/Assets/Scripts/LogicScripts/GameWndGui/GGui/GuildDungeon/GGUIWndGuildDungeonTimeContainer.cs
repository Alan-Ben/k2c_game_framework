using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟PVE时间选择容器
    /// </summary>
    public class GGUIWndGuildDungeonTimeContainer : _ATNPGGUIWndSelectContainer<GGUIMonoGuildDungeonTimeContainerItem,GGUIMonoGuildDungeonTimeContainer,GGUIWndGuildDungeonTimeContainerItem>
    {
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonTimeContainer(GGUIMonoGuildDungeonTimeContainer _containerMono) : base(_containerMono)
        {
        }

        protected override void _onShowWndEx()
        {
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

        protected override GGUIWndGuildDungeonTimeContainerItem _createItemWnd(GGUIMonoGuildDungeonTimeContainerItem _itemMono)
        {
            GGUIWndGuildDungeonTimeContainerItem itemWnd = new GGUIWndGuildDungeonTimeContainerItem(_itemMono);
            return itemWnd;
        }

        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
