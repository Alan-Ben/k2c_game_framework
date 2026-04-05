using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士挑战相关入口
    /// </summary>
    public class GGUIWndHeroBattleEntry : _ATALBasicUIWnd<GGUIMonoHeroBattleEntry>
    {
        private static GGUIWndHeroBattleEntry _g_instance;

        public static GGUIWndHeroBattleEntry instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndHeroBattleEntry();
                }

                return _g_instance;
            }
        }
        
        public GGUIWndHeroBattleEntry() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoHeroBattleEntry.assetPath; }
        protected override string _monoObjName { get => GGUIMonoHeroBattleEntry.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        

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
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

    }
}